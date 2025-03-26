#region Copyright Syncfusion Inc. 2001 - 2018
// Copyright Syncfusion Inc. 2001 - 2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

namespace BI.PivotGrid
{
    using Common;   
    using Windows.UI.Xaml;
    using ViewModel = Tutorials.PivotGridSamples.ViewModel;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Serialization : SampleLayout
    {
        #region Private Fields

        /// <summary>
        /// Initialize the view model class for further usage.
        /// </summary>
        ViewModel.ViewModel vm = new ViewModel.ViewModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the new instance of <see cref="Serialization">Serialization.</see>/>
        /// </summary>
        public Serialization()
        {
            this.InitializeComponent();
            this.pivotGrid1.ItemSource = vm.ProductSalesData;
            if (DeviceFamily.GetDeviceFamily() == Devices.Mobile)
                this.pivotGrid1.ShowGroupingBar = false;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Method is used to freeing the memory with clearing instances, events and objects.
        /// </summary>
        public sealed override void Dispose()
        {
            base.Dispose();
            if (pivotGrid1 != null)
                pivotGrid1.Dispose();
            pivotGrid1 = null;
            if (vm != null)
            {
                vm.ProductSalesData = null;
                vm.BusinessSalesData = null;
                vm.OlapDataManager = null;
                vm.Dispose();
            }
            vm = null;
            DataContext = null;
        }

        #endregion

        private void btnSerialize_Click(object sender, RoutedEventArgs e)
        {           
            this.pivotGrid1.Serialize();
        }

        private void btnDeSerialize_Click(object sender, RoutedEventArgs e)
        {           
            this.pivotGrid1.Deserialize();
        }
    }
}
