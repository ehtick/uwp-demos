#region Copyright Syncfusion® Inc. 2001-2025.
// Copyright Syncfusion® Inc. 2001-2025. All rights reserved.
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
using System.IO;
using System.Linq;
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
    public sealed partial class StackedCharts : SampleLayout
    {
        public StackingColumnChartViewModel ColumnChartViewModel
        {
            get;
            set;
        }

        public StackingBarChartViewModel BarChartViewModel
        {
            get;
            set;
        }

        public StackingAreaChartViewModel AreaChartViewModel
        {
            get;
            set;
        }

        public StackingLineChartViewModel LineChartViewModel
        {
            get;
            set;
        }

        public StackedCharts()
        {
            Resources.MergedDictionaries.Add(SfChart.Resources.ColorModelResource.Resource);
            this.InitializeComponent();
            ColumnChartViewModel = new StackingColumnChartViewModel();
            BarChartViewModel = new StackingBarChartViewModel();
            AreaChartViewModel = new StackingAreaChartViewModel();
            LineChartViewModel = new StackingLineChartViewModel();
            this.DataContext = this;

            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                sbsChartAdornmentInfo1.ShowLabel = true;
                sbsChartAdornmentInfo1.HorizontalAlignment = HorizontalAlignment.Left;

                sbsChartAdornmentInfo2.ShowLabel = true;
                sbsChartAdornmentInfo2.HorizontalAlignment = HorizontalAlignment.Left;

                sbsChartAdornmentInfo3.ShowLabel = true;
                sbsChartAdornmentInfo3.HorizontalAlignment = HorizontalAlignment.Left;

                sb100ChartAdornmentInfo1.ShowLabel = true;
                sb100ChartAdornmentInfo1.HorizontalAlignment = HorizontalAlignment.Left;

                sb100ChartAdornmentInfo2.ShowLabel = true;
                sb100ChartAdornmentInfo2.HorizontalAlignment = HorizontalAlignment.Left;

                sb100ChartAdornmentInfo3.ShowLabel = true;
                sb100ChartAdornmentInfo3.HorizontalAlignment = HorizontalAlignment.Left;

                saChartAdornmentInfo1.ShowLabel = true;
                saChartAdornmentInfo1.SegmentLabelContent = LabelContent.LabelContentPath;
                saChartAdornmentInfo1.LabelTemplate = MainGrid.Resources["stackingAreaSeriesLT1"] as DataTemplate;

                saChartAdornmentInfo2.ShowLabel = true;
                saChartAdornmentInfo2.SegmentLabelContent = LabelContent.LabelContentPath;
                saChartAdornmentInfo2.LabelTemplate = MainGrid.Resources["stackingAreaSeriesLT2"] as DataTemplate;

                saChartAdornmentInfo3.ShowLabel = true;
                saChartAdornmentInfo3.SegmentLabelContent = LabelContent.LabelContentPath;
                saChartAdornmentInfo3.LabelTemplate = MainGrid.Resources["stackingAreaSeriesLT3"] as DataTemplate;

                sa100ChartAdornmentInfo1.ShowLabel = true;
                sa100ChartAdornmentInfo1.SegmentLabelContent = LabelContent.LabelContentPath;
                sa100ChartAdornmentInfo1.LabelTemplate = MainGrid.Resources["stackingArea100SeriesLT1"] as DataTemplate;

                sa100ChartAdornmentInfo2.ShowLabel = true;
                sa100ChartAdornmentInfo2.SegmentLabelContent = LabelContent.LabelContentPath;
                sa100ChartAdornmentInfo2.LabelTemplate = MainGrid.Resources["stackingArea100SeriesLT2"] as DataTemplate;

                sa100ChartAdornmentInfo3.ShowLabel = true;
                sa100ChartAdornmentInfo3.SegmentLabelContent = LabelContent.LabelContentPath;
                sa100ChartAdornmentInfo3.LabelTemplate = MainGrid.Resources["stackingArea100SeriesLT3"] as DataTemplate;

                scChartAdornmentInfo1.ShowLabel = true;
                scChartAdornmentInfo1.LabelPosition = AdornmentsLabelPosition.Inner;

                scChartAdornmentInfo2.ShowLabel = true;
                scChartAdornmentInfo2.LabelPosition = AdornmentsLabelPosition.Inner;

                scChartAdornmentInfo3.ShowLabel = true;
                scChartAdornmentInfo3.LabelPosition = AdornmentsLabelPosition.Inner;

                sc100ChartAdornmentInfo1.ShowLabel = true;
                sc100ChartAdornmentInfo1.LabelPosition = AdornmentsLabelPosition.Inner;

                sc100ChartAdornmentInfo2.ShowLabel = true;
                sc100ChartAdornmentInfo2.LabelPosition = AdornmentsLabelPosition.Inner;

                sc100ChartAdornmentInfo3.ShowLabel = true;
                sc100ChartAdornmentInfo3.LabelPosition = AdornmentsLabelPosition.Inner;

                slChartAdornmentInfo1.ShowMarker = true;
                slChartAdornmentInfo1.Symbol = ChartSymbol.Ellipse; 
                slChartAdornmentInfo1.HorizontalAlignment = HorizontalAlignment.Center;
                slChartAdornmentInfo1.VerticalAlignment = VerticalAlignment.Center;

                slChartAdornmentInfo2.ShowMarker = true;
                slChartAdornmentInfo2.Symbol = ChartSymbol.Ellipse;
               
                slChartAdornmentInfo3.ShowMarker = true;
                slChartAdornmentInfo3.Symbol = ChartSymbol.Ellipse;

                slChartAdornmentInfo4.ShowMarker = true;
                slChartAdornmentInfo4.Symbol = ChartSymbol.Ellipse;

                sl100ChartAdornmentInfo1.ShowMarker = true;
                sl100ChartAdornmentInfo1.Symbol = ChartSymbol.Ellipse;
                 
                sl100ChartAdornmentInfo2.ShowMarker = true;
                sl100ChartAdornmentInfo2.Symbol = ChartSymbol.Ellipse;

                sl100ChartAdornmentInfo3.ShowMarker = true;
                sl100ChartAdornmentInfo3.Symbol = ChartSymbol.Ellipse;

                sl100ChartAdornmentInfo4.ShowMarker = true;
                sl100ChartAdornmentInfo4.Symbol = ChartSymbol.Ellipse;
            }
        }

        private void OnSelectStackSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stLineChart == null) return;
            if (selectStack != null)
            {
                if (selectStack.SelectedIndex == 0)
                {
                    stLineChart.PrimaryAxis.ZoomFactor = 1;
                    stLineChart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Collapsed;
                    stLineChart.Visibility = Visibility.Visible;
                    stColumnChart.Visibility = Visibility.Collapsed;
                    stAreaChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Collapsed;
                    stBar100Chart.Visibility = Visibility.Collapsed;
                    stColumn100Chart.Visibility = Visibility.Collapsed;
                    stArea100Chart.Visibility = Visibility.Collapsed;
                }
                else if (selectStack.SelectedIndex == 1)
                {
                    stBarChart.PrimaryAxis.ZoomFactor = 1;
                    stBarChart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Visible;
                    stColumnChart.Visibility = Visibility.Collapsed;
                    stAreaChart.Visibility = Visibility.Collapsed;
                    stLineChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Collapsed;
                    stBar100Chart.Visibility = Visibility.Collapsed;
                    stColumn100Chart.Visibility = Visibility.Collapsed;
                    stArea100Chart.Visibility = Visibility.Collapsed;
                }
                else if (selectStack.SelectedIndex == 2)
                {
                    stColumnChart.PrimaryAxis.ZoomFactor = 1;
                    stColumnChart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Collapsed;
                    stColumnChart.Visibility = Visibility.Visible;
                    stAreaChart.Visibility = Visibility.Collapsed;
                    stLineChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Collapsed;
                    stBar100Chart.Visibility = Visibility.Collapsed;
                    stColumn100Chart.Visibility = Visibility.Collapsed;
                    stArea100Chart.Visibility = Visibility.Collapsed;
                }
                else if (selectStack.SelectedIndex == 3)
                {
                    stAreaChart.PrimaryAxis.ZoomFactor = 1;
                    stAreaChart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Collapsed;
                    stColumnChart.Visibility = Visibility.Collapsed;
                    stAreaChart.Visibility = Visibility.Visible;
                    stLineChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Collapsed;
                    stBar100Chart.Visibility = Visibility.Collapsed;
                    stColumn100Chart.Visibility = Visibility.Collapsed;
                    stArea100Chart.Visibility = Visibility.Collapsed;
                }
                else if (selectStack.SelectedIndex == 4)
                {
                    stLine100Chart.PrimaryAxis.ZoomFactor = 1;
                    stLine100Chart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Collapsed;
                    stColumnChart.Visibility = Visibility.Collapsed;
                    stAreaChart.Visibility = Visibility.Collapsed;
                    stLineChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Visible;
                    stBar100Chart.Visibility = Visibility.Collapsed;
                    stColumn100Chart.Visibility = Visibility.Collapsed;
                    stArea100Chart.Visibility = Visibility.Collapsed;
                }
                else if (selectStack.SelectedIndex == 5)
                {
                    stBar100Chart.PrimaryAxis.ZoomFactor = 1;
                    stBar100Chart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Collapsed;
                    stColumnChart.Visibility = Visibility.Collapsed;
                    stAreaChart.Visibility = Visibility.Collapsed;
                    stLineChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Collapsed;
                    stBar100Chart.Visibility = Visibility.Visible;
                    stColumn100Chart.Visibility = Visibility.Collapsed;
                    stArea100Chart.Visibility = Visibility.Collapsed;
                }
                else if (selectStack.SelectedIndex == 6)
                {
                    stColumn100Chart.PrimaryAxis.ZoomFactor = 1;
                    stColumn100Chart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Collapsed;
                    stColumnChart.Visibility = Visibility.Collapsed;
                    stAreaChart.Visibility = Visibility.Collapsed;
                    stLineChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Collapsed;
                    stBar100Chart.Visibility = Visibility.Collapsed;
                    stColumn100Chart.Visibility = Visibility.Visible;
                    stArea100Chart.Visibility = Visibility.Collapsed;
                }
                else if (selectStack.SelectedIndex == 7)
                {
                    stArea100Chart.PrimaryAxis.ZoomFactor = 1;
                    stArea100Chart.SecondaryAxis.ZoomFactor = 1;
                    stBarChart.Visibility = Visibility.Collapsed;
                    stColumnChart.Visibility = Visibility.Collapsed;
                    stAreaChart.Visibility = Visibility.Collapsed;
                    stLineChart.Visibility = Visibility.Collapsed;
                    stLine100Chart.Visibility = Visibility.Collapsed;
                    stBar100Chart.Visibility = Visibility.Collapsed;
                    stColumn100Chart.Visibility = Visibility.Collapsed;
                    stArea100Chart.Visibility = Visibility.Visible;
                }
            }
        }

        public override void Dispose()
        {
            if (this.DataContext is StackingColumnChartViewModel)
                (this.DataContext as StackingColumnChartViewModel).Dispose();

            if (this.DataContext is StackingBarChartViewModel)
                (this.DataContext as StackingBarChartViewModel).Dispose();

            if (this.DataContext is StackingAreaChartViewModel)
                (this.DataContext as StackingAreaChartViewModel).Dispose();

            if (this.DataContext is StackingLineChartViewModel)
                (this.DataContext as StackingLineChartViewModel).Dispose();

            if (this.stBarChart != null)
            {
                foreach (var series in this.stBarChart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stBarChart = null;
            }

            if (this.stBar100Chart != null)
            {
                foreach (var series in this.stBar100Chart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stBar100Chart = null;
            }

            if (this.stAreaChart != null)
            {
                foreach (var series in this.stAreaChart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stAreaChart = null;
            }

            if (this.stArea100Chart != null)
            {
                foreach (var series in this.stArea100Chart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stArea100Chart = null;
            }

            if (this.stColumnChart != null)
            {
                foreach (var series in this.stColumnChart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stColumnChart = null;
            }

            if (this.stColumn100Chart != null)
            {
                foreach (var series in this.stColumn100Chart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stColumn100Chart = null;
            }

            if (this.stLineChart != null)
            {
                foreach (var series in this.stLineChart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stLineChart = null;
            }

            if (this.stLine100Chart != null)
            {
                foreach (var series in this.stLine100Chart.Series)
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                this.stLine100Chart = null;
            }

            this.Resources = null;

            base.Dispose();
        }
    }

    public class StackingColumnChartViewModel : IDisposable
    {
        public StackingColumnChartViewModel()
        {
            this.MedalDetails = new ObservableCollection<Medal>();

            MedalDetails.Add(new Medal() { CountryName = "USA", GoldMedals = 395, SilverMedals = 319, BronzeMedals = 296 });
            MedalDetails.Add(new Medal() { CountryName = "Germany", GoldMedals = 247, SilverMedals = 284, BronzeMedals = 320 });
            MedalDetails.Add(new Medal() { CountryName = "UK", GoldMedals = 207, SilverMedals = 255, BronzeMedals = 253 });
            MedalDetails.Add(new Medal() { CountryName = "France", GoldMedals = 191, SilverMedals = 212, BronzeMedals = 233 });
            MedalDetails.Add(new Medal() { CountryName = "Italy", GoldMedals = 190, SilverMedals = 157, BronzeMedals = 174 });
            
            ResourceFac = new ResourceFactory();
            AdornmentInfo5 = new ChartAdornmentInfo();
            AdornmentInfo51 = new ChartAdornmentInfo();
            AdornmentInfo52 = new ChartAdornmentInfo();
            AdornmentInfo50 = new ChartAdornmentInfo();
            AdornmentInfo510 = new ChartAdornmentInfo();
            AdornmentInfo520 = new ChartAdornmentInfo();

            AdornmentInfo5.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo5.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo5.ShowLabel = true;
            AdornmentInfo5.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo51.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo51.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo51.ShowLabel = true;
            AdornmentInfo51.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo52.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo52.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo52.ShowLabel = true;
            AdornmentInfo52.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo50.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo50.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo50.ShowLabel = true;
            AdornmentInfo50.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo510.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo510.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo510.ShowLabel = true;
            AdornmentInfo510.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo520.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo520.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo520.ShowLabel = true;
            AdornmentInfo520.LabelTemplate = ResourceFac.labelTemplate10;
        }
        public ResourceFactory ResourceFac { get; set; }
        public ChartAdornmentInfo AdornmentInfo5 { get; set; }
        public ChartAdornmentInfo AdornmentInfo51 { get; set; }
        public ChartAdornmentInfo AdornmentInfo52 { get; set; }
        public ChartAdornmentInfo AdornmentInfo50 { get; set; }
        public ChartAdornmentInfo AdornmentInfo510 { get; set; }
        public ChartAdornmentInfo AdornmentInfo520 { get; set; }

        public ObservableCollection<Medal> MedalDetails
        {
            get;
            set;
        }

        public void Dispose()
        {
            if (this.MedalDetails != null)
                this.MedalDetails.Clear();
        }
    }

    public class StackingBarChartViewModel : IDisposable
    {

        public StackingBarChartViewModel()
        {
            this.GoldInventoryDetails = new ObservableCollection<GoldInventory>();

            GoldInventoryDetails.Add(new GoldInventory() { Year = 2005, Inferred = 1100, Measured = 750, Reserves = 900 });
            GoldInventoryDetails.Add(new GoldInventory() { Year = 2006, Inferred = 1200, Measured = 500, Reserves = 1000 });
            GoldInventoryDetails.Add(new GoldInventory() { Year = 2007, Inferred = 900, Measured = 700, Reserves = 1200 });
            GoldInventoryDetails.Add(new GoldInventory() { Year = 2008, Inferred = 950, Measured = 1000, Reserves = 900 });
            GoldInventoryDetails.Add(new GoldInventory() { Year = 2009, Inferred = 900, Measured = 1100, Reserves = 1000 });

            ResourceFac = new ResourceFactory();

            AdornmentInfo1 = new ChartAdornmentInfo();
            AdornmentInfo11 = new ChartAdornmentInfo();
            AdornmentInfo12 = new ChartAdornmentInfo();
            AdornmentInfo10 = new ChartAdornmentInfo();
            AdornmentInfo110 = new ChartAdornmentInfo();
            AdornmentInfo120 = new ChartAdornmentInfo();
            AdornmentInfo2 = new ChartAdornmentInfo();
            AdornmentInfo5 = new ChartAdornmentInfo();
            AdornmentInfo51 = new ChartAdornmentInfo();
            AdornmentInfo52 = new ChartAdornmentInfo();

            AdornmentInfo1.ShowLabel = true;
            AdornmentInfo1.FontSize = 10;
            AdornmentInfo1.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            AdornmentInfo1.Foreground = new SolidColorBrush(Colors.White);

            AdornmentInfo11.ShowLabel = true;
            AdornmentInfo11.FontSize = 10;
            AdornmentInfo11.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            AdornmentInfo11.Foreground = new SolidColorBrush(Colors.White);

            AdornmentInfo12.ShowLabel = true;
            AdornmentInfo12.FontSize = 10;
            AdornmentInfo12.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            AdornmentInfo12.Foreground = new SolidColorBrush(Colors.White);

            AdornmentInfo10.ShowLabel = true;
            AdornmentInfo10.FontSize = 10;
            AdornmentInfo10.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            AdornmentInfo10.Foreground = new SolidColorBrush(Colors.White);

            AdornmentInfo110.ShowLabel = true;
            AdornmentInfo110.FontSize = 10;
            AdornmentInfo110.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            AdornmentInfo110.Foreground = new SolidColorBrush(Colors.White);

            AdornmentInfo120.ShowLabel = true;
            AdornmentInfo120.FontSize = 10;
            AdornmentInfo120.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            AdornmentInfo120.Foreground = new SolidColorBrush(Colors.White);

            AdornmentInfo2.SegmentLabelContent = LabelContent.LabelContentPath;
            AdornmentInfo2.ShowLabel = true;
            AdornmentInfo2.LabelTemplate = ResourceFac.labelTemplate12;

            AdornmentInfo5.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo5.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo5.ShowLabel = true;
            AdornmentInfo5.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo51.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo51.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo51.ShowLabel = true;
            AdornmentInfo51.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo52.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo52.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo52.ShowLabel = true;
            AdornmentInfo52.LabelTemplate = ResourceFac.labelTemplate10;
        }
        public ResourceFactory ResourceFac { get; set; }
        public ChartAdornmentInfo AdornmentInfo1 { get; set; }
        public ChartAdornmentInfo AdornmentInfo11 { get; set; }
        public ChartAdornmentInfo AdornmentInfo12 { get; set; }
        public ChartAdornmentInfo AdornmentInfo10 { get; set; }
        public ChartAdornmentInfo AdornmentInfo110 { get; set; }
        public ChartAdornmentInfo AdornmentInfo120 { get; set; }
        public ChartAdornmentInfo AdornmentInfo2 { get; set; }
        public ChartAdornmentInfo AdornmentInfo5 { get; set; }
        public ChartAdornmentInfo AdornmentInfo51 { get; set; }
        public ChartAdornmentInfo AdornmentInfo52 { get; set; }

        public ObservableCollection<GoldInventory> GoldInventoryDetails
        {
            get;
            set;
        }

        public void Dispose()
        {
            if (this.GoldInventoryDetails != null)
                this.GoldInventoryDetails.Clear();
        }
    }

    public class StackingAreaChartViewModel : IDisposable
    {
        public ResourceFactory ResourceFac { get; set; }
        public ChartAdornmentInfo AdornmentInfo1 { get; set; }
        public ChartAdornmentInfo AdornmentInfo2 { get; set; }
        public ChartAdornmentInfo AdornmentInfo3 { get; set; }
        public ChartAdornmentInfo AdornmentInfo11 { get; set; }
        public ChartAdornmentInfo AdornmentInfo12 { get; set; }
        public ChartAdornmentInfo AdornmentInfo13 { get; set; }
        public ChartAdornmentInfo AdornmentInfo4 { get; set; }

        public StackingAreaChartViewModel()
        {
            AdornmentInfo1 = new ChartAdornmentInfo();
            AdornmentInfo2 = new ChartAdornmentInfo();
            AdornmentInfo3 = new ChartAdornmentInfo();
            AdornmentInfo11 = new ChartAdornmentInfo();
            AdornmentInfo12 = new ChartAdornmentInfo();
            AdornmentInfo13 = new ChartAdornmentInfo();
            AdornmentInfo4 = new ChartAdornmentInfo();
            ResourceFac = new ResourceFactory();

            AdornmentInfo1.ShowLabel = true;
            AdornmentInfo1.FontSize = 10;
            AdornmentInfo1.AdornmentsPosition = AdornmentsPosition.TopAndBottom;
            AdornmentInfo1.Foreground = new SolidColorBrush(Colors.White);

            AdornmentInfo2.SegmentLabelContent = LabelContent.LabelContentPath;
            AdornmentInfo2.ShowLabel = true;
            AdornmentInfo2.LabelTemplate = ResourceFac.labelTemplate12;

            AdornmentInfo3.SegmentLabelContent = LabelContent.LabelContentPath;
            AdornmentInfo3.ShowLabel = true;
            AdornmentInfo3.LabelTemplate = ResourceFac.labelTemplate13;

            AdornmentInfo11.SegmentLabelContent = LabelContent.LabelContentPath;
            AdornmentInfo11.ShowLabel = true;
            AdornmentInfo11.LabelTemplate = ResourceFac.labelTemplate12;

            AdornmentInfo12.SegmentLabelContent = LabelContent.LabelContentPath;
            AdornmentInfo12.ShowLabel = true;
            AdornmentInfo12.LabelTemplate = ResourceFac.labelTemplate13;

            AdornmentInfo13.SegmentLabelContent = LabelContent.LabelContentPath;
            AdornmentInfo13.ShowLabel = true;
            AdornmentInfo13.VerticalAlignment = VerticalAlignment.Bottom;
            AdornmentInfo13.LabelTemplate = ResourceFac.labelTemplate14;

            AdornmentInfo4.SegmentLabelContent = LabelContent.LabelContentPath;
            AdornmentInfo4.ShowLabel = true;
            AdornmentInfo4.LabelTemplate = ResourceFac.labelTemplate14;

            this.TemperatureData = new ObservableCollection<Temperatue>();

            TemperatureData.Add(new Temperatue()
            {
                Margin = new Thickness(25, 0, 0, 0),
                Year = "2008",
                Autumn = 30,
                Spring = 35,
                Summer = 43,
                Winter = 29
            });
            TemperatureData.Add(new Temperatue()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Year = "2009",
                Autumn = 30,
                Spring = 35,
                Summer = 43,
                Winter = 29
            });
            TemperatureData.Add(new Temperatue()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Year = "2010",
                Autumn = 32,
                Spring = 37,
                Summer = 47,
                Winter = 27
            });
            TemperatureData.Add(new Temperatue()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Year = "2011",
                Autumn = 34,
                Spring = 35,
                Summer = 43,
                Winter = 25
            });
            TemperatureData.Add(new Temperatue()
            {
                Margin = AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile" ?
                                                        new Thickness(0, 0, 0, 0) : new Thickness(0, 0, 25, 0),
                Year = "2012",
                Autumn = 28,
                Spring = 36,
                Summer = 49,
                Winter = 27
            });
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                TemperatureData.Add(new Temperatue()
                {
                    Margin = new Thickness(0, 0, 25, 0),
                    Year = "2013",
                    Autumn = 31,
                    Spring = 30,
                    Summer = 52,
                    Winter = 30
                });
            }
        }
        public ObservableCollection<Temperatue> TemperatureData
        {
            get;
            set;
        }

        public void Dispose()
        {
            if (this.TemperatureData != null)
                this.TemperatureData.Clear();
        }
    }

    public class StackingLineChartViewModel : IDisposable
    {
        public StackingLineChartViewModel()
        {
            this.MonthlyExpense = new ObservableCollection<Expense>();

            MonthlyExpense.Add(new Expense() { Name = "Food", Father = 55, Mother = 40, Son = 45, Daughter = 48 });
            MonthlyExpense.Add(new Expense() { Name = "Transport", Father = 33, Mother = 45, Son = 54, Daughter = 28 });
            MonthlyExpense.Add(new Expense() { Name = "Medical", Father = 43, Mother = 23, Son = 20, Daughter = 34 });
            MonthlyExpense.Add(new Expense() { Name = "Clothes", Father = 32, Mother = 54, Son = 23, Daughter = 84 });
            MonthlyExpense.Add(new Expense() { Name = "Books", Father = 56, Mother = 18, Son = 43, Daughter = 55 });
            MonthlyExpense.Add(new Expense() { Name = "Others", Father = 23, Mother = 54, Son = 33, Daughter = 56 });
            
            ResourceFac = new ResourceFactory();
            AdornmentInfo5 = new ChartAdornmentInfo();
            AdornmentInfo51 = new ChartAdornmentInfo();
            AdornmentInfo52 = new ChartAdornmentInfo();
            AdornmentInfo53 = new ChartAdornmentInfo();
            AdornmentInfo50 = new ChartAdornmentInfo();
            AdornmentInfo510 = new ChartAdornmentInfo();
            AdornmentInfo520 = new ChartAdornmentInfo();
            AdornmentInfo530 = new ChartAdornmentInfo();

            AdornmentInfo5.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo5.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo5.ShowLabel = true;
            AdornmentInfo5.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo51.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo51.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo51.ShowLabel = true;
            AdornmentInfo51.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo52.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo52.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo52.ShowLabel = true;
            AdornmentInfo52.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo53.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo53.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo53.ShowLabel = true;
            AdornmentInfo53.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo50.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo50.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo50.ShowLabel = true;
            AdornmentInfo50.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo510.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo510.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo510.ShowLabel = true;
            AdornmentInfo510.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo520.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo520.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo520.ShowLabel = true;
            AdornmentInfo520.LabelTemplate = ResourceFac.labelTemplate10;

            AdornmentInfo530.HorizontalAlignment = HorizontalAlignment.Center;
            AdornmentInfo530.VerticalAlignment = VerticalAlignment.Center;
            AdornmentInfo530.ShowLabel = true;
            AdornmentInfo530.LabelTemplate = ResourceFac.labelTemplate10;
        }
        public ResourceFactory ResourceFac { get; set; }
        public ChartAdornmentInfo AdornmentInfo5 { get; set; }
        public ChartAdornmentInfo AdornmentInfo51 { get; set; }
        public ChartAdornmentInfo AdornmentInfo52 { get; set; }
        public ChartAdornmentInfo AdornmentInfo53 { get; set; }
        public ChartAdornmentInfo AdornmentInfo50 { get; set; }
        public ChartAdornmentInfo AdornmentInfo510 { get; set; }
        public ChartAdornmentInfo AdornmentInfo520 { get; set; }
        public ChartAdornmentInfo AdornmentInfo530 { get; set; }

        public ObservableCollection<Expense> MonthlyExpense
        {
            get;
            set;
        }

        public void Dispose()
        {
            if (this.MonthlyExpense != null)
                this.MonthlyExpense.Clear();
        }
    }

    public class Temperatue
    {
        public Thickness Margin
        {
            get;
            set;
        }

        public string Year
        {
            get;
            set;
        }

        public double Spring
        {
            get;
            set;
        }

        public double Summer
        {
            get;
            set;
        }

        public double Autumn
        {
            get;
            set;
        }

        public double Winter
        {
            get;
            set;
        }

    }

    public class Medal
    {
        public string CountryName
        {
            get;
            set;
        }

        public double GoldMedals
        {
            get;
            set;
        }

        public double SilverMedals
        {
            get;
            set;
        }

        public double BronzeMedals
        {
            get;
            set;
        }

    }

    public class GoldInventory
    {
        public double Year
        {
            get;
            set;
        }

        public double Inferred
        {
            get;
            set;
        }

        public double Measured
        {
            get;
            set;
        }

        public double Reserves
        {
            get;
            set;
        }
    }

    public class Expense
    {
        public string Name
        {
            get;
            set;
        }

        public double Father
        {
            get;
            set;
        }

        public double Mother
        {
            get;
            set;
        }

        public double Son
        {
            get;
            set;
        }

        public double Daughter
        {
            get;
            set;
        }

    }
}
