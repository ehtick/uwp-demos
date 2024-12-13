#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Common;
using Syncfusion.SampleBrowser.UWP.ImageEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.SampleBrowser.UWP.ImageEditor
{
    public class SamplesConfiguration
    {
        public SamplesConfiguration()
        {
            CollectSampleView();
        }
        public void CollectSampleView()
        {

            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(FirstPage1).AssemblyQualifiedName,
                Product = "ImageEditor",
                ProductIcons = "ms-appx:/Assets/imageedit.png",
                DesktopImage = "ms-appx:/Assets/imageedit.png",
                MobileImage = "ms-appx:/Assets/imageedit.png",
                Header = "Image Editor",
                Tag = Tags.None,
                Category = Categories.Editors,
                Description = "The image editor control lets users annotate images with freehand drawing, text and shapes. It is also possible to perform simple image manipulation operations like cropping,flipping and rotation.",
                HasOptions = false
            });

            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(Serialization).AssemblyQualifiedName,
                Product = "ImageEditor",
                ProductIcons = "ms-appx:/Assets/imageedit.png",
                DesktopImage = "ms-appx:/Assets/imageedit.png",
                MobileImage = "ms-appx:/Assets/imageedit.png",
                Header = "Serialization",
                Tag = Tags.None,
                Category = Categories.Editors,
                Description = "The image editor control lets users annotate images with freehand drawing, text and shapes. It is also possible to perform simple image manipulation operations like cropping,flipping and rotation.",
                HasOptions = false
            });

            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(BannerPage).AssemblyQualifiedName,
                Product = "ImageEditor",
                ProductIcons = "ms-appx:/Assets/imageedit.png",
                DesktopImage = "ms-appx:/Assets/imageedit.png",
                MobileImage = "ms-appx:/Assets/imageedit.png",
                Header = "Banner Creator",
                Tag = Tags.None,
                Category = Categories.Editors,
                Description = "The image editor control lets users annotate images with freehand drawing, text and shapes. It is also possible to perform simple image manipulation operations like cropping,flipping and rotation.",
                HasOptions = false
            });

            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(Customization).AssemblyQualifiedName,
                Product = "ImageEditor",
                ProductIcons = "ms-appx:/Assets/imageedit.png",
                DesktopImage = "ms-appx:/Assets/imageedit.png",
                MobileImage = "ms-appx:/Assets/imageedit.png",
                Header = "Customization",
                Tag = Tags.None,
                Category = Categories.Editors,
                Description = "The image editor control lets users annotate images with freehand drawing, text and shapes. It is also possible to perform simple image manipulation operations like cropping,flipping and rotation.",
                HasOptions = false
            });


            SampleHelper.SampleViews.Add(new SampleInfo()
            {
                SampleView = typeof(CustomView).AssemblyQualifiedName,
                Product = "ImageEditor",
                ProductIcons = "ms-appx:/Assets/imageedit.png",
                DesktopImage = "ms-appx:/Assets/imageedit.png",
                MobileImage = "ms-appx:/Assets/imageedit.png",
                Header = "CustomView",
                Tag = Tags.None,
                Category = Categories.Editors,
                Description = "The image editor control lets users annotate images with freehand drawing, text and shapes. It is also possible to perform simple image manipulation operations like cropping,flipping and rotation.",
                HasOptions = false
            });
            SampleHelper.SetTagsForProduct("ImageEditor", Tags.None);


        }
    }
}

