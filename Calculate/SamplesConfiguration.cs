#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
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

namespace Syncfusion.SampleBrowser.UWP.Calculate
{
    /// <summary>
    /// Represents to added samples for Calculate.
    /// </summary>
    public class SamplesConfiguration
    {
        /// <summary>
        /// Indicates the instance for the calculate samples.
        /// </summary>
        public SamplesConfiguration()
        {

            // Product Showcase
            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(CalculateSamples.ArrayIcalcDataDemo).AssemblyQualifiedName,
                Header = "ArrayICalc",
                ProductIcons="Icons/Calculate.png",
                SearchKeys = new string[] {"Array","Formula","Calculate"},
                Product = "Calculate",
                Category = Categories.Miscellaneous,
            });
            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(CalculateSamples.ComputeFormulaDemo).AssemblyQualifiedName,
                Header = "Compute Formula",
                SearchKeys = new string[] { "Compute", "Formula", "Calculate" },
                Product = "Calculate",
                Category = Categories.Miscellaneous,
            });
        }
    }
}
