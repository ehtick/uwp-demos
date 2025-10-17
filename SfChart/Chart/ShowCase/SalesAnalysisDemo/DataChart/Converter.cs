#region Copyright Syncfusion® Inc. 2001-2025.
// Copyright Syncfusion® Inc. 2001-2025. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Syncfusion.SampleBrowser.UWP.SfChart
{
    class SalesToPriceConverter:IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            double sales=(double)value;
            if (value != null)
            {
                return " "+String.Format("{0:C}", System.Convert.ToInt32(value.ToString()));
            }
            return 0;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}