#region Copyright Syncfusion Inc. 2001-2021.
// Copyright Syncfusion Inc. 2001-2021. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitConverter
{
    public class CultureModel
    {
        public CultureInfo US
        {
            get
            {
                return new CultureInfo("en-US");
            }
        }

        public CultureInfo India
        {
            get
            {
                return new CultureInfo("hi-IN");
            }
        }

        public CultureInfo French
        {
            get
            {
                return new CultureInfo("fr-FR");
            }
        }

        public CultureInfo German
        {
            get
            {
                return new CultureInfo("de-DE");
            }
        }

        public CultureInfo UK
        {
            get
            {
                return new CultureInfo("en-GB");
            }
        }

        public CultureInfo Chineese
        {
            get
            {
                return new CultureInfo("zh-CN");
            }
        }

        public CultureInfo Arabic
        {
            get
            {
                return new CultureInfo("ar-BH");
            }
        }

        public CultureInfo Zulu
        {
            get
            {
                return new CultureInfo("zu-ZA");
            }
        }

        public CultureInfo Japan
        {
            get
            {
                return new CultureInfo("ja-JP");
            }
        }

        public CultureInfo SouthAfrica
        {
            get
            {
                return new CultureInfo("af-ZA");
            }
        }
    }
}
