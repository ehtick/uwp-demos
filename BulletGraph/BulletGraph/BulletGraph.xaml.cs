#region Copyright Syncfusion Inc. 2001-2023.
// Copyright Syncfusion Inc. 2001-2023. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Common;
using Syncfusion.UI.Xaml.BulletGraph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace  BulletGraphUWP_Samples

{
    public sealed partial class BulletGraph : SampleLayout,IDisposable
    {
        public BulletGraph()
        {
            InitializeComponent();
            cmb_TickPosition.ItemsSource = Enum.GetValues(typeof(BulletGraphTicksPosition));
            cmb_LabelPosition.ItemsSource = Enum.GetValues(typeof(BulletGraphLabelsPosition));
            if (   !Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                cmb_CaptionPosition.ItemsSource = Enum.GetValues(typeof(BulletGraphCaptionPosition));
            }
            cmb_LabelPosition.SelectedIndex = cmb_TickPosition.SelectedIndex = 1;
            cmb_CaptionPosition.SelectedIndex = 0;

            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                this.SfBulletGraph1.QuantitativeScaleLength = 450;
                this.SfBulletGraph2.QuantitativeScaleLength = 450;
                this.SfBulletGraph3.QuantitativeScaleLength = 450;
            }
        }

        public override void Dispose()
        {
            this.SfBulletGraph1.Dispose();
            this.SfBulletGraph2.Dispose();
            this.SfBulletGraph3.Dispose();
            this.SfBulletGraph1 = null;
            this.SfBulletGraph2 = null;
            this.SfBulletGraph3 = null;
        }

 
    }
}
