#region Copyright Syncfusion Inc. 2001-2023.
// Copyright Syncfusion Inc. 2001-2023. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Common;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Syncfusion.SampleBrowser.UWP.SfChart
{
    public sealed partial class TechnicalIndicator : SampleLayout
    {
        private StockViewModel dataViewModel;

        public ObservableCollection<ChartSeries> indicatorsCollection = new ObservableCollection<ChartSeries>();
        public TechnicalIndicator()
        {
            Resources.MergedDictionaries.Add(UWP.SfChart.Resources.ColorModelResource.Resource);
            this.InitializeComponent();
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                dataViewModel = new StockViewModel(0);
                this.DataContext = dataViewModel.Stocks;
                this.SfChart.Series[0].ItemsSource = dataViewModel.Stocks[0].Datas;
                string[] technicalIndicators = {"MACD","Average True Range", "Accumulation Distribution", "Bollinger Band",
                                                "Exponential Average", "Momentum", "RSI", "Simple Average", "Stochastic",
                                                "Triangular Average"};
                Indicators.ItemsSource = technicalIndicators;
                Indicators.SelectedItem = technicalIndicators[0];
            }
            else
            {
                dataViewModel = new StockViewModel(0);
                this.DataContext = dataViewModel.Stocks;
                this.SfChart.Series[0].ItemsSource = dataViewModel.Stocks[0].Datas;
                this.Chart2.Series[0].ItemsSource = dataViewModel.Stocks[0].Datas;
            }
        }
        private void OnIndicatorsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            indicatorsCollection.Clear();
            this.SfChart.TechnicalIndicators.Clear();

            if (comboBox != null)
            {
                var indicator = Addindicator(comboBox.SelectedItem.ToString(), 1);
                if (indicator != null)
                {
                    indicatorsCollection.Add(indicator);
                }
            }

            foreach (var item in indicatorsCollection)
            {
                ISupportAxes2D indicatorAxis = item as ISupportAxes2D;
                if (indicatorAxis != null)
                {
                    this.SfChart.TechnicalIndicators.Add(item);
                    NumericalAxis axis = new NumericalAxis();
                    axis.OpposedPosition = true;
                    axis.ShowGridLines = false;
                    axis.Visibility = Visibility.Collapsed;
                    indicatorAxis.YAxis = axis;
                }
            }
        }

        private FinancialTechnicalIndicator Addindicator(string value, int rowIndex)
        {
            FinancialTechnicalIndicator indicator;
            string a = "";
            object obj = a;
            string b = (obj as string);
            switch (value)
            {
                case "Accumulation Distribution":
                    StockViewModel model = new StockViewModel(1);

                    DataContext = model.Stocks;
                    indicator = new AccumulationDistributionIndicator() { SignalLineColor = new SolidColorBrush(Colors.Blue) };
                    SfChart.Annotations.Clear();
                    LineAnnotation annotationAD = new LineAnnotation { Text = "High in Prices", HorizontalTextAlignment = HorizontalAlignment.Center, VerticalTextAlignment = VerticalAlignment.Top, CoordinateUnit = CoordinateUnit.Axis, X1 = 22, Y1 = 30, X2 = 26, Y2 = 40 };
                    LineAnnotation annotation2AD = new LineAnnotation { Text = "Decline in Prices", CoordinateUnit = CoordinateUnit.Axis, X1 = 38, Y1 = 28, X2 = 46, Y2 = 15 };
                    LineAnnotation annotation3AD = new LineAnnotation { Text = "Price is Trending Down", CoordinateUnit = CoordinateUnit.Axis, X1 = 0.1, Y1 = 41.5, X2 = 14, Y2 = 13 };

                    SfChart.Annotations.Add(annotationAD);
                    SfChart.Annotations.Add(annotation2AD);
                    SfChart.Annotations.Add(annotation3AD);
                    break;

                case "Average True Range":
                    StockViewModel model9 = new StockViewModel(1);

                    DataContext = model9.Stocks;
                    SfChart.Annotations.Clear();
                    indicator = new AverageTrueRangeIndicator()
                    {
                        SignalLineColor = new SolidColorBrush(Colors.Blue),
                        Period = 2
                    };
                    TextAnnotation annotationAT = new TextAnnotation { Text = "Buy Signal", CoordinateUnit = CoordinateUnit.Axis, X1 = 6, Y1 = 37 };
                    TextAnnotation annotationAT1 = new TextAnnotation { Text = "Buy Signal", CoordinateUnit = CoordinateUnit.Axis, X1 = 8, Y1 = 18 };
                    TextAnnotation annotationAT2 = new TextAnnotation { Text = "Sell Signal", CoordinateUnit = CoordinateUnit.Axis, X1 = 20, Y1 = 24 };
                    TextAnnotation annotationAT3 = new TextAnnotation { Text = "Sell Signal", CoordinateUnit = CoordinateUnit.Axis, X1 = 27.3, Y1 = 41 };
                    TextAnnotation annotationAT4 = new TextAnnotation { Text = "Average True Range" + "\n" + "Trailing Stops", CoordinateUnit = CoordinateUnit.Axis, X1 = 35, Y1 = 22 };
                    SfChart.Annotations.Add(annotationAT);
                    SfChart.Annotations.Add(annotationAT1);
                    SfChart.Annotations.Add(annotationAT2);
                    SfChart.Annotations.Add(annotationAT3);
                    SfChart.Annotations.Add(annotationAT4);

                    break;

                case "Bollinger Band":
                    StockViewModel model1 = new StockViewModel(0);

                    DataContext = model1.Stocks;

                    indicator = new BollingerBandIndicator()
                    {
                        UpperLineColor = new SolidColorBrush(Colors.Green),
                        LowerLineColor = new SolidColorBrush(Colors.Red),
                        SignalLineColor = new SolidColorBrush(Colors.Transparent),
                        Period = 2
                    };

                    SfChart.Annotations.Clear();
                    TextAnnotation annotation = new TextAnnotation { Text = "Upper Bollinger Band", CoordinateUnit = CoordinateUnit.Axis, X1 = 6, Y1 = 650 };
                    TextAnnotation annotation1 = new TextAnnotation { Text = "Sell Signal", CoordinateUnit = CoordinateUnit.Axis, X1 = 33.7, Y1 = 660 };
                    TextAnnotation annotation2 = new TextAnnotation { Text = "Lower Bollinger Band", CoordinateUnit = CoordinateUnit.Axis, X1 = 40, Y1 = 390 };
                    TextAnnotation annotation3 = new TextAnnotation { Text = "Buy Signal", CoordinateUnit = CoordinateUnit.Axis, X1 = 7, Y1 = 230 };

                    SfChart.Annotations.Add(annotation);
                    SfChart.Annotations.Add(annotation1);
                    SfChart.Annotations.Add(annotation2);
                    SfChart.Annotations.Add(annotation3);
                    break;
                case "Exponential Average":
                    StockViewModel model8 = new StockViewModel(0);

                    DataContext = model8.Stocks;
                    indicator = new ExponentialAverageIndicator() { SignalLineColor = new SolidColorBrush(Colors.Blue) };
                    SfChart.Annotations.Clear();
                    TextAnnotation annotationE = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 0.3, Y1 = 700 };
                    TextAnnotation annotationE1 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 2, Y1 = 605 };
                    TextAnnotation annotationE2 = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 8, Y1 = 260 };
                    TextAnnotation annotationE3 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 20, Y1 = 580 };
                    TextAnnotation annotationE4 = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 26, Y1 = 450 };
                    TextAnnotation annotationE5 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 35, Y1 = 660 };
                    TextAnnotation annotationE6 = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 40, Y1 = 460 };
                    TextAnnotation annotationE7 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 54, Y1 = 760 };

                    SfChart.Annotations.Add(annotationE);
                    SfChart.Annotations.Add(annotationE1);
                    SfChart.Annotations.Add(annotationE2);
                    SfChart.Annotations.Add(annotationE3);
                    SfChart.Annotations.Add(annotationE4);
                    SfChart.Annotations.Add(annotationE5);
                    SfChart.Annotations.Add(annotationE6);
                    SfChart.Annotations.Add(annotationE7);
                    break;

                case "MACD":
                    StockViewModel model7 = new StockViewModel(2);

                    DataContext = model7.Stocks;
                    SfChart.Annotations.Clear();
                    indicator = new MACDTechnicalIndicator()
                    {
                        Period = 5,
                        LongPeriod = 8,
                        ShortPeriod = 3,

                        SignalLineColor = new SolidColorBrush(Colors.Blue),
                        ConvergenceLineColor = new SolidColorBrush(Colors.Green),
                        DivergenceLineColor = new SolidColorBrush(Colors.Red)
                    };
                    TextAnnotation annotationMA = new TextAnnotation { Text = "MACD" + "\n" + "Sell Signals", CoordinateUnit = CoordinateUnit.Axis, X1 = 17, Y1 = 700 };
                    TextAnnotation annotationMA1 = new TextAnnotation { Text = "MACD" + "\n" + "Buy Signals", CoordinateUnit = CoordinateUnit.Axis, X1 = 2, Y1 = 400 };
                    TextAnnotation annotationMA2 = new TextAnnotation { Text = "Buy", CoordinateUnit = CoordinateUnit.Axis, X1 = 10, Y1 = 250 };
                    TextAnnotation annotationMA3 = new TextAnnotation { Text = "Sell", CoordinateUnit = CoordinateUnit.Axis, X1 = 24, Y1 = 620 };
                    TextAnnotation annotationMA4 = new TextAnnotation { Text = "Buy", CoordinateUnit = CoordinateUnit.Axis, X1 = 30, Y1 = 450 };
                    TextAnnotation annotationMA5 = new TextAnnotation { Text = "Sell", CoordinateUnit = CoordinateUnit.Axis, X1 = 48, Y1 = 650 };

                    SfChart.Annotations.Add(annotationMA);
                    SfChart.Annotations.Add(annotationMA1);
                    SfChart.Annotations.Add(annotationMA2);
                    SfChart.Annotations.Add(annotationMA3);
                    SfChart.Annotations.Add(annotationMA4);
                    SfChart.Annotations.Add(annotationMA5);
                    break;
                case "Momentum":
                    SfChart.Annotations.Clear();
                    StockViewModel model6 = new StockViewModel(2);

                    DataContext = model6.Stocks;
                    indicator = new MomentumTechnicalIndicator()
                    {
                        MomentumLineColor = new SolidColorBrush(Colors.Blue),

                        Period = 4
                    };
                    LineAnnotation annotationM = new LineAnnotation { Text = "Downtrend", CoordinateUnit = CoordinateUnit.Axis, X1 = 2, Y1 = 463, X2 = 12, Y2 = 245 };
                    TextAnnotation annotationM1 = new TextAnnotation { Text = "Buy", CoordinateUnit = CoordinateUnit.Axis, X1 = 8, Y1 = 450 };
                    TextAnnotation annotationM2 = new TextAnnotation { Text = "Sell", CoordinateUnit = CoordinateUnit.Axis, X1 = 21, Y1 = 530 };
                    LineAnnotation annotationM3 = new LineAnnotation { Text = "Divergence of Price" + "\n" + "& Momentum", CoordinateUnit = CoordinateUnit.Axis, X1 = 36.5, Y1 = 468, X2 = 47.5, Y2 = 394 };

                    SfChart.Annotations.Add(annotationM);
                    SfChart.Annotations.Add(annotationM1);
                    SfChart.Annotations.Add(annotationM2);
                    SfChart.Annotations.Add(annotationM3);
                    break;
                case "RSI":
                    StockViewModel model5 = new StockViewModel(2);

                    DataContext = model5.Stocks;
                    SfChart.Annotations.Clear();
                    indicator = new RSITechnicalIndicator()
                    {
                        SignalLineColor = new SolidColorBrush(Colors.Blue),
                        LowerLineColor = new SolidColorBrush(Colors.Red),
                        Period = 5,
                        UpperLineColor = new SolidColorBrush(Colors.Green)
                    };
                    TextAnnotation annotationRS = new TextAnnotation { Text = "Oversold", CoordinateUnit = CoordinateUnit.Axis, X1 = 42, Y1 = 645 };
                    TextAnnotation annotationRS1 = new TextAnnotation { Text = "Overbought", CoordinateUnit = CoordinateUnit.Axis, X1 = 0.3, Y1 = 380 };
                    TextAnnotation annotationRS2 = new TextAnnotation { Text = "Seller Territory" + "\n" + "(over 500)", CoordinateUnit = CoordinateUnit.Axis, X1 = 10, Y1 = 600 };
                    TextAnnotation annotationRS3 = new TextAnnotation { Text = "Buyers Territory" + "\n" + "(below 500)", CoordinateUnit = CoordinateUnit.Axis, X1 = 50, Y1 = 450 };

                    SfChart.Annotations.Add(annotationRS);
                    SfChart.Annotations.Add(annotationRS1);
                    SfChart.Annotations.Add(annotationRS2);
                    SfChart.Annotations.Add(annotationRS3);
                    break;
                case "Simple Average":
                    StockViewModel model4 = new StockViewModel(0);

                    DataContext = model4.Stocks;
                    SfChart.Annotations.Clear();
                    indicator = new SimpleAverageIndicator() { SignalLineColor = new SolidColorBrush(Colors.Blue) };
                    TextAnnotation annotationSA = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 0, Y1 = 405 };
                    TextAnnotation annotationSA1 = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 8, Y1 = 250 };
                    TextAnnotation annotationSA2 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 21, Y1 = 620 };
                    TextAnnotation annotationSA3 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 31, Y1 = 630 };

                    TextAnnotation annotationSA6 = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 40, Y1 = 475 };
                    TextAnnotation annotationSA7 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 54, Y1 = 760 };

                    SfChart.Annotations.Add(annotationSA);
                    SfChart.Annotations.Add(annotationSA1);
                    SfChart.Annotations.Add(annotationSA2);
                    SfChart.Annotations.Add(annotationSA3);
                    SfChart.Annotations.Add(annotationSA6);
                    SfChart.Annotations.Add(annotationSA7);
                    break;
                case "Stochastic":
                    SfChart.Annotations.Clear();
                    StockViewModel model3 = new StockViewModel(0);

                    DataContext = model3.Stocks;
                    indicator = new StochasticTechnicalIndicator()
                    {
                        SignalLineColor = new SolidColorBrush(Colors.Red),
                        UpperLineColor = new SolidColorBrush(Colors.Green),
                        Period = 3
                    };
                    LineAnnotation annotationS = new LineAnnotation { Text = "Lower Low", CoordinateUnit = CoordinateUnit.Axis, X1 = 0.8, Y1 = 433, X2 = 9.4, Y2 = 243 };
                    TextAnnotation annotationS1 = new TextAnnotation { Text = "Low#1", CoordinateUnit = CoordinateUnit.Axis, X1 = -0.6, Y1 = 420 };
                    TextAnnotation annotationS2 = new TextAnnotation { Text = "Low#2", CoordinateUnit = CoordinateUnit.Axis, X1 = 8, Y1 = 243 };
                    LineAnnotation annotationS4 = new LineAnnotation { Text = "Higher High", CoordinateUnit = CoordinateUnit.Axis, X1 = 11.5, Y1 = 369, X2 = 22.5, Y2 = 632 };
                    TextAnnotation annotationS5 = new TextAnnotation { Text = "High#2", CoordinateUnit = CoordinateUnit.Axis, X1 = 21, Y1 = 650 };
                    TextAnnotation annotationS6 = new TextAnnotation { Text = "High#1", CoordinateUnit = CoordinateUnit.Axis, X1 = 10, Y1 = 370 };
                    LineAnnotation annotationS7 = new LineAnnotation { Text = "Higher Low", CoordinateUnit = CoordinateUnit.Axis, X1 = 38, Y1 = 637, X2 = 48, Y2 = 680 };
                    TextAnnotation annotationS8 = new TextAnnotation { Text = "Low#3", CoordinateUnit = CoordinateUnit.Axis, X1 = 37, Y1 = 670 };
                    TextAnnotation annotationS9 = new TextAnnotation { Text = "Low#4", CoordinateUnit = CoordinateUnit.Axis, X1 = 48, Y1 = 700 };

                    SfChart.Annotations.Add(annotationS);
                    SfChart.Annotations.Add(annotationS1);
                    SfChart.Annotations.Add(annotationS2);
                    SfChart.Annotations.Add(annotationS4);
                    SfChart.Annotations.Add(annotationS5);
                    SfChart.Annotations.Add(annotationS6);
                    SfChart.Annotations.Add(annotationS7);
                    SfChart.Annotations.Add(annotationS8);
                    SfChart.Annotations.Add(annotationS9);
                    break;
                case "Triangular Average":
                    StockViewModel model2 = new StockViewModel(0);

                    DataContext = model2.Stocks;
                    SfChart.Annotations.Clear();
                    indicator = new TriangularAverageIndicator() { SignalLineColor = new SolidColorBrush(Colors.Blue) };
                    TextAnnotation annotationT = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 2, Y1 = 400 };
                    TextAnnotation annotationT1 = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 8, Y1 = 250 };
                    TextAnnotation annotationT2 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 21, Y1 = 620 };
                    TextAnnotation annotationT3 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 31, Y1 = 630 };

                    TextAnnotation annotationT6 = new TextAnnotation { Text = "BUY", CoordinateUnit = CoordinateUnit.Axis, X1 = 40, Y1 = 475 };
                    TextAnnotation annotationT7 = new TextAnnotation { Text = "SELL", CoordinateUnit = CoordinateUnit.Axis, X1 = 54, Y1 = 760 };

                    SfChart.Annotations.Add(annotationT);
                    SfChart.Annotations.Add(annotationT1);
                    SfChart.Annotations.Add(annotationT2);
                    SfChart.Annotations.Add(annotationT3);
                    SfChart.Annotations.Add(annotationT6);
                    SfChart.Annotations.Add(annotationT7);
                    break;
                default:
                    return null;
            }
            var index = rowIndex == 0 ? 1 : 0;
            ChartSeries Series = this.SfChart.VisibleSeries[index] as ChartSeries;
            indicator.XBindingPath = "TimeStamp";
            indicator.High = "High";
            indicator.Low = "Low";
            indicator.Open = "Open";
            indicator.Close = "Last";
            indicator.Volume = "Volume";
            Binding binding = new Binding();
            binding.Path = new PropertyPath("ItemsSource");
            binding.Source = Series;
            binding.Mode = BindingMode.TwoWay;
            indicator.SetBinding(FinancialTechnicalIndicator.ItemsSourceProperty, binding);

            return indicator;
        }

        public override void Dispose()
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                if (this.DataContext is StockViewModel && (this.DataContext as StockViewModel).Stocks != null)
                {
                    (this.DataContext as StockViewModel).Stocks.Clear();
                }
                
            }

            if (SfChart != null)
            {
                foreach(var series in SfChart.Series)
                {
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                }
                foreach (var series in SfChart.TechnicalIndicators)
                {
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                }
                this.SfChart = null;
            }

            this.Grid.Resources = null;

            base.Dispose();
        }
    }

    public class StockViewModel : INotifyPropertyChanged
    {
        private List<Stock> stocks;

        private string[] stockNames = { "GOOG", "ADI", "MACD" };

        private int stockID = 0;

        public StockViewModel(int id)
        {
            stockID = id;
            LoadData();
        }

        void LoadData()
        {
            Stocks = GenerateStocks();
        }

        public List<Stock> Stocks
        {
            get
            {
                return stocks;
            }
            set
            {
                stocks = value;
                OnPropertyChanged("Stocks");
            }
        }

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        List<Stock> GenerateStocks()
        {
            List<Stock> stocks = new List<Stock>();
            Stock stock = new Stock();

            string path = "Syncfusion.SampleBrowser.UWP.SfChart.Chart.Tutorials.Data.GOOG.txt";
            stock.OrgDatas = GetDatas(path);
            stock.Datas = GetDatas(path);
            int count = stock.Datas.Count;
            stock.CurrentHigh = stock.Datas[count - 1].High;
            stock.CurrentLow = stock.Datas[count - 1].Low;
            stock.CurrentClose = stock.Datas[count - 1].Last;
            stock.PreviousClose = stock.Datas[count - 2].Last;
            stocks.Add(stock);
            return stocks;
        }

        public static List<StockData> GetDatas(string fileName)
        {
            char[] comma = new char[] { ',' };
            char[] slashN = new char[] { '\n' };
            List<StockData> list = new List<StockData>();
            Random r = new Random();
            //string s = File.ReadAllText(fileName);
            //string[] lines = s.Split(slashN);

            IList<string> lines = new List<string>();
            var assembly = typeof(StockViewModel).GetTypeInfo().Assembly;
            var fileStream = assembly.GetManifestResourceStream(fileName);
            StreamReader reader = new StreamReader(fileStream);
            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            bool firstLine = true;
            string[] values;
            int count = lines.Count() - 2;

            StockData priceInfo;
            int index = 0;
            foreach (string line in lines)
            {
                if (count != -1 && index >= count)
                    break;
                if (!firstLine)
                {
                    values = line.Split(comma);
                    if (values.GetLength(0) > 5)
                    {
                        priceInfo = new StockData()
                        {
                            TimeStamp = DateTime.Parse(values[0], CultureInfo.InvariantCulture),
                            Open = double.Parse(values[1]),
                            High = double.Parse(values[2]),
                            Low = double.Parse(values[3]),
                            Last = double.Parse(values[4]),
                            Volume = double.Parse(values[5])
                        };
                        list.Insert(index, priceInfo);
                        index++;
                    }
                }
                else
                {
                    firstLine = false;
                }
            }
            return list;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Stock : INotifyPropertyChanged
    {
        public Stock()
        {

        }

        public string StockName { get; set; }

        public List<StockData> OrgDatas
        {
            get;
            set;
        }

        List<StockData> datas;

        public List<StockData> Datas
        {
            get
            {
                return datas;
            }
            set
            {
                datas = value;
                OnPropertyChanged("Datas");
            }
        }


        private double currentLow;

        private double currentClose;

        private double currentHigh;

        public double CurrentLow
        {
            get
            {
                return currentLow;
            }
            set
            {
                currentLow = value;
                OnPropertyChanged("CurrentLow");
            }
        }

        public double CurrentHigh
        {
            get
            {
                return currentHigh;
            }
            set
            {
                currentHigh = value;
                OnPropertyChanged("CurrentHigh");
            }
        }

        public double CurrentClose
        {
            get
            {
                return currentClose;
            }
            set
            {
                currentClose = value;
                OnPropertyChanged("TodayClose");
            }
        }

        private double previousClose;

        public double PreviousClose
        {
            get
            {
                return previousClose;
            }
            set
            {
                previousClose = value;
                OnPropertyChanged("PreviousClose");
            }
        }

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StockData
    {
        public StockData()
        {

        }

        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public double Last { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
