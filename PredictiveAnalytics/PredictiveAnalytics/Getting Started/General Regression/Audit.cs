#region Copyright Syncfusion® Inc. 2001-2025.
// Copyright Syncfusion® Inc. 2001-2025. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.PMML;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SampleBrowser.UWP.PredictiveAnalytics
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GeneralRegression : Page, IDisposable
    {
        #region Private properties

        private ViewModel viewModel = null;
        private Table inputDataTable = null;
        private Table outputTable = null;
        private string pmmlPath = string.Empty;
        private Dictionary<string, object> predictedAdjustedCollection;
        private bool ageInvalid = false;
        private bool itemInvalid = false;

        #endregion Private properties

        #region Constructor

        public GeneralRegression()
        {
            this.InitializeComponent();
            viewModel = this.DataContext as ViewModel;

            // PMML path
            pmmlPath = "ms-appx:///PredictiveAnalytics/Data/Audit.pmml";

            //Initialize the predicted breast cancer status collection
            predictedAdjustedCollection = new Dictionary<string, object>();

            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                //Applies style to predicted column
                viewModel.PredictedColumnStyle = this.Resources["predictedColumnColor"] as Style;

                if (inputDataTable == null)
                {                    
                    //resource path
                    string resourcePath = "ms-appx:///PredictiveAnalytics/Getting Started/General Regression";
                    // input data
                    inputDataTable = new Table(resourcePath + "/Audit.csv", true, ',');

                    // Adds content for R, C# and PMML in the resulting tab control
                    viewModel.RCode = new SyntaxHighlighterR().FormatCode(Windows.Storage.PathIO.
                     ReadTextAsync(resourcePath + "/Audit.R").GetAwaiter().GetResult());
                    viewModel.CSharpCode = new SyntaxRichTextBoxCS().FormatCode(Windows.Storage.PathIO.
                     ReadTextAsync(resourcePath + "/Audit.cs").GetAwaiter().GetResult());
                    viewModel.PMML = SyntaxHighlighterXAML.FormatCode(Windows.Storage.PathIO.ReadTextAsync(pmmlPath).GetAwaiter().GetResult());

                    // Resizes grid column based on its value length
                    this.SfDataGrid.GridColumnSizer = new GridColumnSizerExt(this.SfDataGrid);
                }

                //InputRecord (combobox) loaded event trigger
                InputRecords.Loaded += InputRecords_Loaded;

                //Input record (combo box) selection changed event trigger
                InputRecords.SelectionChanged += InputRecords_SelectionChanged;
            }
        }

        #endregion Constructor

        #region InputRecords Loaded Event

        private void InputRecords_Loaded(object sender, RoutedEventArgs e)
        {
            if (predictedAdjustedCollection.Count == 0)
            {
                //Gets the input values based on page size
                Table currentPageTable = viewModel.Take(inputDataTable, 0, sfDataPager.PageSize);
                PredictResult(currentPageTable, sfDataPager.PageSize);
                //Loads items for combo box
                LoadComboBoxItems();
                viewModel.IsBusy = false;
            }
        }

        #endregion InputRecords Loaded Event        

        #region InputRecords_SelectionChanged Event

        private void InputRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
            {
                //Gets the start index of page selected
                int startIndex = sfDataPager.PageIndex * sfDataPager.PageSize;
                //Binds ouput result for visualization
                BindOutputResults((Dictionary<string, object>)predictedAdjustedCollection[(startIndex +
                    (sender as ComboBox).SelectedIndex + 1).ToString()]);
            }
        }

        #endregion InputRecords_SelectionChanged Event

        #region Methods

        /// <summary>
        /// Gets predicted result of input values as observable collection
        /// </summary>
        /// <param name="inputTable">current page input values</param>
        /// <param name="pageSize">page size</param>
        /// <returns>observable collection of predicted results</returns>
        private ObservableCollection<BusinessObject> PredictResult(Table inputTable, int pageSize)
        {
            //Get PMML Evaluator instance
            PMMLEvaluator evaluator = new PMMLEvaluatorFactory().
              GetPMMLEvaluatorInstance(viewModel.GetPMMLPath(pmmlPath));

            //Initialize the output table
            InitializeTable(inputTable.RowCount);

            //Gets the start index of page selected
            int startIndex = sfDataPager.PageIndex * sfDataPager.PageSize;

            //Predict the value for each record using the PMML Evaluator instance
            for (int i = 0; i < inputTable.RowCount; i++)
            {
                //Get input values as dictionary object
                Dictionary<string, object> audit = inputTable.ColumnNames.ToDictionary(column => column, column
                    => inputTable[i, column]);

                //Get result
                PredictedResult predictedResult = evaluator.GetResult(audit, null);

                //Add predicted value
                outputTable[i, 0] = predictedResult.PredictedValue;

                for (int j = 1; j <= predictedResult.GetPredictedCategories().Length; j++)
                    outputTable[i, j] = predictedResult.GetPredictedProbability(predictedResult.GetPredictedCategories()[j - 1]);

                //Adds predicted audit adjusted values to the collection for visualization
                if (!predictedAdjustedCollection.ContainsKey((startIndex + i + 1).ToString()))
                {
                    audit.Add("predictedAdjusted", predictedResult.PredictedValue);
                    audit.Add("lowRiskProbability", predictedResult.GetPredictedProbability(predictedResult.GetPredictedCategories()[0]));
                    audit.Add("highRiskProbability", predictedResult.GetPredictedProbability(predictedResult.GetPredictedCategories()[1]));
                    predictedAdjustedCollection.Add((startIndex + i + 1).ToString(), audit);
                }
            }
            // Merges the selected page inputs and their output values
            var result = viewModel.MergeTable(inputTable, outputTable, inputDataTable);

            return result;
        }

        /// <summary>
        /// Initialize the outputTable
        /// </summary>
        /// <param name="rowCount">rowCount of output table</param>
        private void InitializeTable(int rowCount)
        {
            //Create instance to hold evaluated results
            outputTable = new Table(rowCount, 3);
            //Add predicted column names
            outputTable.ColumnNames[0] = "Predicted_Adjusted";
            outputTable.ColumnNames[1] = "AuditLowriskProbability";
            outputTable.ColumnNames[2] = "AuditHighriskProbability";
        }

        /// <summary>
        /// Loads the items for Combo box (InputRecords)
        /// </summary>
        private void LoadComboBoxItems()
        {
            int count = 0;
            //Gets the start index of page selected
            int startIndex = sfDataPager.PageIndex * sfDataPager.PageSize;
            //Initializes the items list for transaction combo box
            string[] transactionItems = new string[sfDataPager.PageSize];
            for (int index = startIndex; index < (startIndex + sfDataPager.PageSize); index++)
            {
                if (predictedAdjustedCollection.ContainsKey((index + 1).ToString()))
                {
                    //Adds list of transaction ID's to the combo box
                    transactionItems[count] = (index + 1).ToString();
                    count++;
                }
            }
            InputRecords.ItemsSource = transactionItems;
            InputRecords.SelectedIndex = 0;
        }

        /// <summary>
        /// Binds the output results for visualization
        /// </summary>
        /// <param name="inputRecord">input record</param>
        private void BindOutputResults(Dictionary<string, object> inputRecord)
        {
            this.Id.Text = inputRecord["ID"].ToString();
            this.Age.Text = inputRecord["Age"].ToString();
            this.Employment.Text = inputRecord["Employment"].ToString();
            this.Education.Text = inputRecord["Education"].ToString();
            this.Marital.Text = inputRecord["Marital"].ToString();
            this.Occupation.Text = inputRecord["Occupation"].ToString();
            this.Income.Text = inputRecord["Income"].ToString();
            this.Sex.Text = inputRecord["Sex"].ToString();
            this.Deduction.Text = inputRecord["Deductions"].ToString();
            this.Hours.Text = inputRecord["Hours"].ToString();
            this.Accounts.Text = inputRecord["Accounts"].ToString();
            this.Adjustment.Text = inputRecord["Adjustment"].ToString();

            string auditRisk = inputRecord["Adjusted"].ToString() == "0" ? "Low" : "High";
            this.ActualAdjusted.Text = "\"" + auditRisk + "\"";

            viewModel.AuditCollection = new ObservableCollection<Audit>();
            viewModel.AuditCollection.Add(new Audit()
            {
                Status = string.Empty,
                Adjustable_probability = Convert.ToDouble(inputRecord["lowRiskProbability"]),
                NonAdjustable_probability = Convert.ToDouble(inputRecord["highRiskProbability"])
            });
        }

        #endregion Methods

        #region MobileButton events

        public void Predicted_Button(object sender, RoutedEventArgs e)
        {
            //Get PMML Evaluator instance
            PMMLEvaluator evaluator = new PMMLEvaluatorFactory().
                 GetPMMLEvaluatorInstance(viewModel.GetPMMLPath(pmmlPath));                 

            // Input object as dictionary
            var audit = new
            {
                Age = age.Text,
                Employment = ((ComboBoxItem)EmploymentCombo.SelectedItem).Content,
                Education = ((ComboBoxItem)EducationCombo.SelectedItem).Content,
                Marital = ((ComboBoxItem)MaritalCombo.SelectedItem).Content,
                Occupation = ((ComboBoxItem)OccupationCombo.SelectedItem).Content,
                Income = income.Text,
                Sex = ((ComboBoxItem)SexCombo.SelectedItem).Content,
                Deductions = deductions.Text,
                Hours = hours.Text,
                Accounts = ((ComboBoxItem)AccountsCombo.SelectedItem).Content,
                Adjustment = adjustment.Text,
            };

            //Get predicted result
            PredictedResult predictedResult = evaluator.GetResult(audit, null);
            viewModel.AuditCollection = new ObservableCollection<Audit>();
            viewModel.AuditCollection.Add(new Audit()
            {
                Status = string.Empty,
                Adjustable_probability = Convert.ToDouble(predictedResult.GetPredictedProbability("0")),
                NonAdjustable_probability = Convert.ToDouble(predictedResult.GetPredictedProbability("1")),
            });

            //Change of visibility over input and resultant grid
            this.InputGrid.Visibility = Visibility.Collapsed;
            this.ResultGrid.Visibility = Visibility.Visible;
        }

        private void Previous_Button(object sender, RoutedEventArgs e)
        {
            //Change of visibility over input and resultant grid
            this.ResultGrid.Visibility = Visibility.Collapsed;
            this.InputGrid.Visibility = Visibility.Visible;
        }

        #endregion MobileButton events

        #region TabControl events

        // "IsBusy" represents the loading indicator status
        private void analyticsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.IsBusy = true;
        }

        private void predictedResultTab_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = true;
        }

        private void predictedResultTab_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = false;
        }

        private void cSharpTab_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = true;
        }

        private void cSharpTab_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = false;
        }

        private void rTab_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = true;
        }

        private void rTab_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = false;
        }

        private void pmmlTab_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = false;
        }

        private void pmmlTab_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = true;
        }

        private void statisticsTab_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = true;
        }

        private void statisticsTab_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = false;
        }

        private void visualizeTab_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = true;
        }

        private void visualizeTab_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            viewModel.IsBusy = false;
        }

        private void statisticsTab_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor =
                new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void statisticsTab_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor =
                new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void visualizeTab_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor =
                new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void visualizeTab_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor =
                new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        #endregion TabControl events

        #region DataPager events

        private void sfDataPager_Loaded(object sender, RoutedEventArgs e)
        {
            int value = sfDataPager.PageSize;
            //Gets the input values based on page size
            Table currentPageTable = viewModel.Take(inputDataTable, 0, sfDataPager.PageSize);
            ObservableCollection<BusinessObject> result = PredictResult(currentPageTable, value);

            //computes total page count
            int pageCount = inputDataTable.RowCount / value;
            if (inputDataTable.RowCount % value > 0)
            {
                pageCount += 1;
            }

            sfDataPager.PageCount = pageCount;
            //Adds column values to the resulting grid
            this.SfDataGrid = viewModel.AddColumnToGrid(result, this.SfDataGrid, currentPageTable);
            sfDataPager.LoadDynamicItems(0, result);

            // Refreshes grid items
            if (SfDataGrid.View != null)
            {
                SfDataGrid.View.Refresh();

                SfDataGrid.GridColumnSizer.ResetAutoCalculationforAllColumns();

                SfDataGrid.GridColumnSizer.Refresh();
            }
            viewModel.IsBusy = false;
        }

        private void sfDataPager_PageIndexChanged(object sender, Syncfusion.UI.Xaml.Controls.DataPager.PageIndexChangedEventArgs args)
        {
            //Gets the start index of page selected
            int startIndex = args.NewPageIndex * sfDataPager.PageSize;
            // Takes input values based on page index and gets its predicted result
            int index = (startIndex + sfDataPager.PageSize) >= inputDataTable.RowCount ? inputDataTable.RowCount :
                startIndex + sfDataPager.PageSize;
            // Gets selected page input values
            Table currentPageTable = viewModel.Take(inputDataTable, startIndex, index);
            // Gets result for selected page input values
            var result = PredictResult(currentPageTable, sfDataPager.PageSize);
            sfDataPager.LoadDynamicItems(startIndex, result);
            // Refreshes grid items
            if (SfDataGrid.View != null)
            {
                SfDataGrid.View.Refresh();
                SfDataGrid.GridColumnSizer.ResetAutoCalculationforAllColumns();
                SfDataGrid.GridColumnSizer.Refresh();
            }
        }

        #endregion DataPager events

        #region Validation event
        private void InputValidation(object sender, RoutedEventArgs e)
        {
            //Selected item
            TextBox selectedItem = e.OriginalSource as TextBox;
            this.PredictButton.IsEnabled = false;

            //Validates selected item
            if (selectedItem != null)
            {
                switch (selectedItem.Name)
                {
                    case "age":
                        {
                            ageInvalid = (string.IsNullOrEmpty(selectedItem.Text) || (Convert.ToDouble(selectedItem.Text) < 1) ||
                           (Convert.ToDouble(selectedItem.Text) > 99)) ? true : false;
                            selectedItem.BorderBrush = ageInvalid ? new SolidColorBrush(Windows.UI.Colors.Red) :
                                new SolidColorBrush(Windows.UI.Colors.LightGray);
                            ageInvalidText.Visibility = ageInvalid ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                    case "adjustment":
                    case "deductions":
                    case "income":
                    case "hours":
                        {
                            itemInvalid = ((string.IsNullOrEmpty(selectedItem.Text) || Convert.ToDouble(selectedItem.Text) < 0)) ? true : false;
                            selectedItem.BorderBrush = itemInvalid ? new SolidColorBrush(Windows.UI.Colors.Red) :
                                new SolidColorBrush(Windows.UI.Colors.LightGray);
                            incomeInvalidText.Visibility = selectedItem.Name == "income" && itemInvalid ? Visibility.Visible : Visibility.Collapsed;
                            deductionInvalidText.Visibility = selectedItem.Name == "deductions" && itemInvalid ? Visibility.Visible : Visibility.Collapsed;                            
                            adjustmentInvalidText.Visibility = selectedItem.Name == "adjustment" && itemInvalid ? Visibility.Visible : Visibility.Collapsed;
                            hoursInvalidText.Visibility = selectedItem.Name == "hours" && itemInvalid ? Visibility.Visible : Visibility.Collapsed;
                        }
                        break;
                }
            }
            if (!itemInvalid && !ageInvalid)
            {
                age.BorderBrush = adjustment.BorderBrush = deductions.BorderBrush = income.BorderBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);
                ageInvalidText.Visibility = deductionInvalidText.Visibility = incomeInvalidText.Visibility = adjustmentInvalidText.Visibility = Visibility.Collapsed;
                this.PredictButton.IsEnabled = true;
            }

        }
        #endregion

        #region Chart loaded event
        private async void StackingColumnSeries_Loaded(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, LoadComboBoxItems);
        }
        #endregion

        #region Dispose method
        public void Dispose()
        {
            //Disposes objects
            if (predictedAdjustedCollection != null)
                predictedAdjustedCollection.Clear();
            if (InputRecords != null)
                InputRecords.SelectedItem = -1;
            if (SfDataGrid != null)
            {
                SfDataGrid.ItemsSource = null;
                SfDataGrid.Dispose();
            }
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                this.InputRecords.Loaded -= InputRecords_Loaded;
                this.InputRecords.SelectionChanged -= InputRecords_SelectionChanged;
                this.sfDataPager.Loaded -= sfDataPager_Loaded;
                this.sfDataPager.PageIndexChanged -= sfDataPager_PageIndexChanged;
                this.StackingColumnSeries2.Loaded -= StackingColumnSeries_Loaded;
                this.analyticsTabControl.SelectionChanged -= analyticsTabControl_SelectionChanged;
                visualizeTab.Dispose();
                predictedResultTab.Dispose();
                rTab.Dispose();
                cSharpTab.Dispose();
                pmmlTab.Dispose();
                analyticsTabControl.Items.Clear();
                analyticsTabControl = null;
                this.sfDataPager = null;
                statisticsTab.Dispose();
                this.tabControl.Dispose();
                this.BusyIndicator.Dispose();
                inputDataTable.Dispose();
                outputTable.Dispose();                
            }
            viewModel.Dispose();
            viewModel = null;
            this.SfChart = null;
            this.StackingColumnSeries1 = null;
            this.StackingColumnSeries2 = null;
            this.ChartLegend1 = null;
            this.primaryAxis = null;
            this.SecondaryAxis = null;
        }
        #endregion
        #region Chart event 
        private void SfChart_Loaded(object sender, RoutedEventArgs e)
        {
            //Chart adornments
            DataTemplate DataLabelTemplate = MainGrid.Resources["DataLabelTemplate"] as DataTemplate;
            ChartAdornmentInfo chartAdornmentInfo = new ChartAdornmentInfo();
            ChartAdornmentInfo chartAdornmentInfo2 = new ChartAdornmentInfo();
            chartAdornmentInfo.ShowMarker = false;
            chartAdornmentInfo.ShowLabel = true;
            chartAdornmentInfo.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            chartAdornmentInfo.HorizontalAlignment = HorizontalAlignment.Center;
            chartAdornmentInfo.VerticalAlignment = VerticalAlignment.Center;
            chartAdornmentInfo.LabelTemplate = DataLabelTemplate;
            chartAdornmentInfo2.ShowMarker = false;
            chartAdornmentInfo2.ShowLabel = true;
            chartAdornmentInfo2.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            chartAdornmentInfo2.HorizontalAlignment = HorizontalAlignment.Center;
            chartAdornmentInfo2.VerticalAlignment = VerticalAlignment.Center;
            chartAdornmentInfo2.LabelTemplate = DataLabelTemplate;
            StackingColumnSeries1.AdornmentsInfo = chartAdornmentInfo;
            StackingColumnSeries2.AdornmentsInfo = chartAdornmentInfo2;
        }
        #endregion
    }
}