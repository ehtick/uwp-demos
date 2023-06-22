#region Copyright Syncfusion Inc. 2001-2023.
// Copyright Syncfusion Inc. 2001-2023. All rights reserved.
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

namespace Notification.HubTile
{
    public class HubTileProperties
    {
        private static string header;

        public static  string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
            }
        }

        public static  string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        private static  string title;
    }
}
