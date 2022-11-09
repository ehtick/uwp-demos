#region Copyright Syncfusion Inc. 2001-2022.
// Copyright Syncfusion Inc. 2001-2022. All rights reserved.
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
using Windows.UI.Xaml;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Animation;
using Common;

namespace Navigation.TreeNavigator
{
    public class TreeViewModel : NotificationObject
    {
        private List<TreeModel> models;

        public List<TreeModel> Models
        {
            get { return models; }
            set { models = value; }
        }

        public TreeViewModel()
        {
            Models = new List<TreeModel>();
        }
    }
}
