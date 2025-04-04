#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.SampleBrowser.UWP.SfTreeGrid
{
    public class EmployeeInfo : IComparable<EmployeeInfo>
    {
        int _id;
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        string _firstName;

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }
        string _lastName;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }
        string _department;

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        /// <value>The department.</value>
        public string Department
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
            }
        }

        private string email;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The Email Address.</value>
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(17, ErrorMessage = "Email ID is invalid")]
        [EmailAddress]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        string _city;

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        private string _title;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        double? _salary;
        /// <summary>
        /// Gets or sets the salary.
        /// </summary>
        /// <value>The salary.</value>
        public double? Salary
        {
            get
            {
                return _salary;
            }
            set
            {
                _salary = value;
            }
        }

        int _reportsTo;
        /// <summary>
        /// Gets or sets the reports to.
        /// </summary>
        /// <value>The reports to.</value>
        public int ReportsTo
        {
            get
            {
                return _reportsTo;
            }
            set
            {
                _reportsTo = value;
            }
        }


        private bool availability = true;

        [Display(AutoGenerateField = false)]
        public bool Availability
        {
            get
            {
                return availability;
            }
            set
            {
                availability = value;
            }
        }


        #region IComparable<Employee> Members

        public int CompareTo(EmployeeInfo other)
        {
            // return this.reportsTo - other.reportsTo;
            if (other == null)
                return -1;

            return this.ReportsTo.CompareTo(other.ReportsTo);
        }

        #endregion
    }
}

