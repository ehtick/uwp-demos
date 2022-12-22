#region Copyright Syncfusion Inc. 2001 - 2022
// Copyright Syncfusion Inc. 2001 - 2022. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace Syncfusion.SampleBrowser.UWP.SfTreeGrid
{
    public class Employee
    {
        #region Private Fields

        private static int _globalId = 0;
        private int _id;
        private string _firstName;
        private string _lastName;
        private DateTime? _dob;
        private double? _salary;
        private string city;
        private string _cityDescription;
        private ObservableCollection<Employee> _children;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public ObservableCollection<Employee> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int EmployeeID
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


        /// <summary>
        /// Gets or sets the DOB.
        /// </summary>
        /// <value>The DOB.</value>
        public DateTime? DOB
        {
            get
            {
                return _dob;
            }
            set
            {
                _dob = value;
            }
        }

        /// <summary>
        /// Gets or sets the Salary.
        /// </summary>
        /// <value>Salary</value>
        public double? Salary
        {
            get { return _salary; }
            set
            {
                _salary = value;
            }
        }
        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        /// <value>City</value>      

        public string City
        {
            get { return city; }
            set
            {
                city = value;
            }
        }

        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        /// <value>Country</value>
        public string CityDescription
        {
            get
            {
                return _cityDescription;
            }
            set
            {
                _cityDescription = value;
            }
        }

        private string contactNumber;

        public string ContactNumber
        {
            get { return contactNumber; }
            set { contactNumber = value; }
        }


        private bool isAvailable;

        public bool IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; }
        }
        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeInfo"/> class.
        /// </summary>
        public Employee()
            : this("Enter FirstName", "Enter LastName", false, new DateTime(2008, 10, 26), 78998, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeInfo"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="eyecolor">The eyecolor.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="maxGenerations">The max generations.</param>
        public Employee(string firstName, string lastName, bool availablility, DateTime dob, double? sal, ObservableCollection<Employee> child)
        {
            FirstName = firstName;
            LastName = lastName;
            Salary = sal;
            IsAvailable = availablility;
            DOB = dob;
            EmployeeID = _globalId++;
            Children = child;
        }

        #endregion Constructors
    }
}
