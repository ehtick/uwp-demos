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

namespace Syncfusion.SampleBrowser.UWP.TreeMap
{
   public class SamplesConfiguration
    {
        public SamplesConfiguration()
        {
            CollectSampleView();
        }
        public void CollectSampleView()
        {
#if STORE_SAMPLE
            SampleHelper.SampleViews.Add(new SampleInfo() { SampleView = typeof(TreeMapWinRTSamples.FlatCollectionTreeMap).AssemblyQualifiedName, Product = "TreeMap", ProductIcons= "ms-appx:///Syncfusion.SampleBrowser.UWP.TreeMap/Assets/Icons/TreeMap.png", Header = "FlatCollectionTreeMap", Tag = Tags.None, Category = "Data Visualization", HasOptions = false });
            SampleHelper.SampleViews.Add(new SampleInfo() { SampleView = typeof(TreeMapWinRTSamples.HierarchicalCollectionTreeMap).AssemblyQualifiedName,  Product = "TreeMap",ProductIcons= "ms-appx:///Syncfusion.SampleBrowser.UWP.TreeMap/Assets/Icons/TreeMap.png", Header = "HierarchicalCollectionTreeMap", Tag = Tags.None, Category = "Data Visualization", HasOptions = false });
            
#else
            if (DeviceFamily.GetDeviceFamily() == Devices.Mobile)
            {
                SampleHelper.SampleViews.Add(new SampleInfo() { SampleView = typeof(TreeMapWinRTSamples.FlatCollectionTreeMap).AssemblyQualifiedName, Product = "TreeMap", ProductIcons = "ms-appx:///Syncfusion.SampleBrowser.UWP.TreeMap/Assets/Icons/TreeMap.png", Header = "FlatCollectionTreeMap", Tag = Tags.None, Category = Categories.DataVisualization, HasOptions = false });
            }

            else if (DeviceFamily.GetDeviceFamily() == Devices.Desktop)
            {
                SampleHelper.SampleViews.Add(new SampleInfo() { SampleView = typeof(TreeMapWinRTSamples.FlatCollectionTreeMap).AssemblyQualifiedName, Product = "TreeMap", ProductIcons = "ms-appx:///Syncfusion.SampleBrowser.UWP.TreeMap/Assets/Icons/TreeMap.png", Header = "FlatCollectionTreeMap", Tag = Tags.None, Category = Categories.DataVisualization, HasOptions = false });
                SampleHelper.SampleViews.Add(new SampleInfo() { SampleView = typeof(TreeMapWinRTSamples.HierarchicalCollectionTreeMap).AssemblyQualifiedName, Product = "TreeMap", ProductIcons = "ms-appx:///Syncfusion.SampleBrowser.UWP.TreeMap/Assets/Icons/TreeMap.png", Header = "HierarchicalCollectionTreeMap", Tag = Tags.None, Category = Categories.DataVisualization, HasOptions = false });
#if (!WINDOWS_STORE)
                SampleHelper.SampleViews.Add(new SampleInfo() { SampleView = typeof(TreeMapWinRTSamples.TreeMapCustomization).AssemblyQualifiedName, Product = "TreeMap", ProductIcons = "ms-appx:///Syncfusion.SampleBrowser.UWP.TreeMap/Assets/Icons/TreeMap.png", Header = "TreeMapCustomization", Tag = Tags.None, Category = Categories.DataVisualization, HasOptions = false });
                SampleHelper.SampleViews.Add(new SampleInfo() { SampleView = typeof(TreeMapWinRTSamples.TreeMapDrillDown).AssemblyQualifiedName, Product = "TreeMap", ProductIcons = "ms-appx:///Syncfusion.SampleBrowser.UWP.TreeMap/Assets/Icons/TreeMap.png", Header = "TreeMapDrillDown", Tag = Tags.None, Category = Categories.DataVisualization, HasOptions = false });
#endif
                }
#endif

            }
        }
}
