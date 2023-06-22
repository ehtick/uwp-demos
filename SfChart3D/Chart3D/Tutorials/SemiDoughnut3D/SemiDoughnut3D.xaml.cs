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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Syncfusion.SampleBrowser.UWP.SfChart3D
{
    public sealed partial class SemiDoughnut3D : SampleLayout
    {
        public SemiDoughnut3D()
        {
            this.InitializeComponent();

            SemiDoughnutChart3D.Rotation = -11;
            dsChartAdornmentInfo3D.SegmentLabelContent = LabelContent.LabelContentPath;
            dsChartAdornmentInfo3D.VerticalAlignment = VerticalAlignment.Center;
            dsChartAdornmentInfo3D.HorizontalAlignment = HorizontalAlignment.Center;
            dsChartAdornmentInfo3D.AdornmentsPosition = AdornmentsPosition.Bottom;
            dsChartAdornmentInfo3D.SegmentLabelFormat = "##.#";
            dsChartAdornmentInfo3D.UseSeriesPalette = true;
            dsChartAdornmentInfo3D.ShowLabel = true;
            dsChartAdornmentInfo3D.ShowConnectorLine = true;
            dsChartAdornmentInfo3D.ConnectorHeight = 35;
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                SemiDoughnutChart3D.Margin = new Thickness(70, 20, 75, 25);
            }
        }

        public override void Dispose()
        {
            if (this.grid.DataContext is DataViewModel)
                (this.grid.DataContext as DataViewModel).Dispose();

            if (this.SemiDoughnutChart3D != null)
            {
                foreach (var series in this.SemiDoughnutChart3D.Series)
                {
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                }
                this.SemiDoughnutChart3D = null;
            }

            this.grid.Resources = null;

            base.Dispose();
        }
    }
}
