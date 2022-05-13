#region Copyright Syncfusion Inc. 2001-2022.
// Copyright Syncfusion Inc. 2001-2022. All rights reserved.
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
    public sealed partial class Column3D : SampleLayout
    {
        public Column3D()
        {
            this.InitializeComponent();

            ColumnChart3D.Rotation = -39;
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                ColumnChart3D.Margin = new Thickness(10);
            }
            else
            {
                ColumnChart3D.Margin = new Thickness(70, 20, 75, 25);
            }
        }

        public override void Dispose()
        {
            if (this.grid.DataContext is CategoryDataViewModel)
                (this.grid.DataContext as CategoryDataViewModel).Dispose();

            if (this.ColumnChart3D != null)
            {
                foreach (var series in this.ColumnChart3D.Series)
                {
                    series.ClearValue(ChartSeriesBase.ItemsSourceProperty);
                }
                this.ColumnChart3D = null;
            }

            this.grid.Resources = null;

            base.Dispose();
        }
    }

    public class CategoryDataViewModel : IDisposable
    {
        public CategoryDataViewModel()
        {
            CategoricalDatas = new ObservableCollection<CategoryData>();
            CategoricalDatas.Add(new CategoryData(6, 7, 5, "2008"));
            CategoricalDatas.Add(new CategoryData(10, 13, 10, "2009"));
            CategoricalDatas.Add(new CategoryData(23, 15, 12, "2010"));
            CategoricalDatas.Add(new CategoryData(26, 21, 20, "2011"));
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                CategoricalDatas.Add(new CategoryData(30, 25, 23, "2012"));
        }

        public ObservableCollection<CategoryData> CategoricalDatas
        {
            get;
            set;
        }

        public void Dispose()
        {
            if (this.CategoricalDatas != null)
            {
                this.CategoricalDatas.Clear();
            }
        }
    }

    public class CategoryData : INotifyPropertyChanged
    {
        private double value;

        public CategoryData(double iron, double metal, double plastic, string year)
        {
            Plastic = plastic;
            Year = year;
            Metal = metal;
            Iron = iron;
        }

        public string Year
        {
            get;
            set;
        }

        public double Iron
        {
            get;
            set;
        }

        public double Plastic
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public double Metal
        {
            get;
            set;
        }

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
