#region Copyright Syncfusion® Inc. 2001-2025.
// Copyright Syncfusion® Inc. 2001-2025. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.UI.Xaml.Diagram.Stencil;
using Windows.UI.Xaml;

namespace DiagramBuilder.ViewModel
{
    public class SymbolVM : ISymbol
    {
            static Random r = new Random();

        public SymbolVM()
        {
        }

        public object Symbol
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public DataTemplate SymbolTemplate
        {
            get;
            set;
        }

        public string GroupName { get; set; }


        public ISymbol Clone()
        {
            return new SymbolVM()
            {
                Symbol = this.Symbol,
                SymbolTemplate = this.SymbolTemplate
            };
        }


        public object Key
        {
            get;
            set;
        }
    }


    public class SymbolCollection : ObservableCollection<object>
    {

    }
}
