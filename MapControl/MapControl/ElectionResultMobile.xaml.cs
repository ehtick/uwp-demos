#region Copyright Syncfusion® Inc. 2001-2025.
// Copyright Syncfusion® Inc. 2001-2025. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.UI.Xaml.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MapControlUWP_Samples
{
    public sealed partial class ElectionResultMobile : UserControl
    {
        public ElectionResultMobile()
        {
            this.InitializeComponent();
            map.Loaded += Map_Loaded;
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            LoadShapeFile();
        }

        private void LoadShapeFile()
        {
            var assembly = typeof(ElectionResult).GetTypeInfo().Assembly;
            string resourcePath = "Syncfusion.SampleBrowser.UWP.Maps.Assets.ShapeFiles.usa_state.shp";
            string valuePath = "Syncfusion.SampleBrowser.UWP.Maps.Assets.ShapeFiles.usa_state.dbf";
            var fileStream = assembly.GetManifestResourceStream(resourcePath);
            var file = assembly.GetManifestResourceStream(valuePath);
            this.shapeLayer.LoadFromStream(fileStream, file);
        }

        public void Dispose()
        {
            (this.grid.DataContext as IDisposable).Dispose();
            this.grid.DataContext = null;
            if (this.shapeLayer.ItemsSource != null)
                this.shapeLayer.ItemsSource = null;
            map.Dispose();
            GC.Collect();
        }

       

        private void shapeLayer_ShapesSelected(object sender, SelectionEventArgs args)
        {
            if (Properties_Panel != null && Properties_Panel.Visibility == Visibility.Collapsed)
            {
                Properties_Panel.Visibility = Visibility.Visible;
            }
            if (shapeLayer != null && args.Items is ObservableCollection<MapShape>)
            {
                ObservableCollection<MapShape> mapShapes = (args.Items as ObservableCollection<MapShape>);
                if (mapShapes != null && mapShapes.Count > 0)
                {
                    var selectedShape = (mapShapes[0] as MapShape);
                    if (selectedShape != null && selectedShape.DataContext is ElectionData)
                    {
                        txt_State.Text = (selectedShape.DataContext as ElectionData).State;
                        txt_Winner.Text = (selectedShape.DataContext as ElectionData).Candidate;
                        txt_Electors.Text = (selectedShape.DataContext as ElectionData).Electors.ToString();
                    }
                }
            }
        }

        private void shapeLayer_ShapesUnSelected(object sender, SelectionEventArgs args)
        {
            if (Properties_Panel != null)
            {
                Properties_Panel.Visibility = Visibility.Collapsed;
            }
        }

    }
}
