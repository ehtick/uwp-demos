#region Copyright Syncfusion Inc. 2001-2023.
// Copyright Syncfusion Inc. 2001-2023. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.SampleBrowser.UWP.SfSparkline
{
    public class SamplesConfiguration
    {
        public SamplesConfiguration()
        {
            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(SparklineDemo).AssemblyQualifiedName,
                Category = Categories.DataVisualization,
                Product = "Sparkline",
                ProductIcons = "Icons/sparkline_chart.png",
                Header = "Getting Started",
                SampleCategory = "Sparkline",
                SearchKeys = new string[] { "sparkline", "spark", "line", "getting", "sample" },
                Tag = Tags.None,
                HasOptions = false
            });
        }
    }
}
