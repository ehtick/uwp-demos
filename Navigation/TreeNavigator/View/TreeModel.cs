#region Copyright Syncfusion Inc. 2001-2022.
// Copyright Syncfusion Inc. 2001-2022. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.TreeNavigator
{
    public class TreeModel : INotifyPropertyChanged
    {
        public TreeModel()
        {
            Models = new ObservableCollection<TreeModel>();
            Models.CollectionChanged += Models_CollectionChanged;
        }

        void Models_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Models.Count > 0)
                Count = "( "+ Models.Count.ToString()+ " )";
        }
        private string header;

        public string Header
        {
            get { return header; }
            set { header = value; 
                OnPropertyChanged("Header");
            }
        }

        private ObservableCollection<TreeModel> models;

        public ObservableCollection<TreeModel> Models
        {
            get { return models; }
            set { models = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _count;
        public string Count
        {
            get { return _count; }
            set { _count = value;
                OnPropertyChanged("Count");
            }
        }

        private string icon;

        public string Icon
        {
            get { return icon; }
            set {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(name));
        }
    }
}
