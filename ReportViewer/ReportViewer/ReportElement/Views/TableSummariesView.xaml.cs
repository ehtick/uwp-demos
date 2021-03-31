#region Copyright Syncfusion Inc. 2001-2021.
// Copyright Syncfusion Inc. 2001-2021. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Common;
using Syncfusion.UI.Xaml.Reports;
using System;
using System.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Syncfusion.SampleBrowser.UWP.ReportViewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TableSummariesView : SampleLayout, IDisposable
    {
        ReportViewerSampleHelper SampleView
        {
            get;
            set;
        }

        public TableSummariesView()
        {
            this.InitializeComponent();
            SampleView = new TableSummaries(this.reportViewer);
            this.Loaded += ReportParametersDemo_Loaded;
        }

        async void ReportParametersDemo_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.DeviceFamily.GetDeviceFamily() != Common.Devices.Desktop)
            {
                grd_controlPanel.Margin = new Thickness(0, 0, 0, 20);
            }

            this.reportViewer.ReportLoaded += reportViewer_ReportLoaded;
            this.reportViewer.ViewButtonClick += reportViewer_ViewButtonClick;

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                if (SampleView != null)
                {
                    SampleView.LoadReport();
                }
            }));

        }

        void reportViewer_ViewButtonClick(object sender, CancelEventArgs args)
        {
            SampleView.UpdateDataSet();
        }

        void reportViewer_ReportLoaded(object sender, EventArgs e)
        {
            SampleView.SetParameter();
            SampleView.UpdateDataSet();
        }

        public override void Dispose()
        {
            SampleView = null;
            this.reportViewer.ReportLoaded -= reportViewer_ReportLoaded;
            this.reportViewer.ViewButtonClick -= reportViewer_ViewButtonClick;
            this.Loaded -= ReportParametersDemo_Loaded;

            if (this.reportViewer.DataSources != null)
            {
                foreach (var dataDataSource in this.reportViewer.DataSources)
                {
                    IList list = dataDataSource.Value as IList;

                    if (list != null)
                    {
                        list.Clear();
                    }
                }
                this.reportViewer.DataSources.Clear();
            }

            this.reportViewer.Reset();
            this.reportViewer.Dispose();
        }
    }
}
