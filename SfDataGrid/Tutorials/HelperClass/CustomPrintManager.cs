#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.SampleBrowser.UWP.SfDataGrid;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DataGrid
{
    class CustomPrintManager : GridPrintManager
    {
        private SfDataGrid dataGrid;

        public CustomPrintManager(SfDataGrid grid)
            : base(grid)
        {
            dataGrid = grid;
            dataGrid.PrintSettings.PrintPageHeaderHeight = 25;
            dataGrid.PrintSettings.PrintPageFooterHeight = 25;
            var res = new DataGrid.CustomPrinting();
            dataGrid.PrintSettings.PrintPageFooterTemplate = res.Resources["footerTemplate"] as DataTemplate;
            dataGrid.PrintSettings.PrintPageHeaderTemplate = res.Resources["headerTemplate"] as DataTemplate;
        }



        //customize the appearance of the cell 
        public override ContentControl GetPrintGridCell(object record, string mappingName)
        {
            var index = dataGrid.View.Records.IndexOfRecord(record);
            if (index % 2 == 0)
                return new PrintGridCell() { Background = new SolidColorBrush(Colors.Bisque) };
            return base.GetPrintGridCell(record, mappingName);
        }



    }
}
