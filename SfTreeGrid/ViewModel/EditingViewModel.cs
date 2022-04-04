#region Copyright Syncfusion Inc. 2001-2022
// Copyright Syncfusion Inc. 2001-2022. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Syncfusion.SampleBrowser.UWP.SfTreeGrid
{
    public class EmployeeRepository : NotificationObject, IDisposable
    {
        private static Random random = new Random(123123);
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        public EmployeeRepository()
        {
            this.EmployeeDetails = this.CreateGenericPersonData(10, 8);
            CityCollection = new ObservableCollection<string>();
            foreach (var item in city)
            {
                CityCollection.Add(item);
            }
        }

        private ObservableCollection<Employee> _EmployeeDetails;

        /// <summary>
        /// Gets or sets the person details.
        /// </summary>
        /// <value>The person details.</value>
        public ObservableCollection<Employee> EmployeeDetails
        {
            get { return _EmployeeDetails; }
            set { _EmployeeDetails = value; }
        }

        private ObservableCollection<string> cityCollection;

        public ObservableCollection<string> CityCollection
        {
            get { return cityCollection; }
            set { cityCollection = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="maxGenerations">The max generations.</param>
        public EmployeeRepository(int count, int maxGenerations)
        {
            CreateGenericPersonData(count, maxGenerations);
        }

        /// <summary>
        /// Creates the child list.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="maxGenerations">The max generations.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns></returns>
        public ObservableCollection<Employee> CreateChildList(int count, int maxGenerations, string lastName)
        {
            ObservableCollection<Employee> _children = new ObservableCollection<Employee>();
            if (count > 0 && maxGenerations > 0)
            {
                _children = CreateGenericPersonData(count, maxGenerations - 1);
                //foreach (Employee p in _children)
                //    p.LastName = lastName;
            }
            return _children;
        }

        /// <summary>
        /// Creates the generic person data.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="maxGenerations">The max generations.</param>
        /// <returns></returns>
        public ObservableCollection<Employee> CreateGenericPersonData(int count, int maxGenerations)
        {
            var personList = new ObservableCollection<Employee>();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var lastname = names2[random.Next(names2.GetLength(0))];
                    personList.Add(new Employee()
                    {
                        FirstName = names1[random.Next(names1.GetLength(0))],
                        LastName = lastname,
                        Children = this.CreateChildList(count, (int)Math.Max(0, maxGenerations - 1), lastname),
                        Salary = 30000d + random.Next(9) * 10000,
                        CityDescription = city[random.Next(city.GetLength(0))],
                        City = city[random.Next(city.GetLength(0))],
                        DOB = GenerateRandomDate().Date,
                        IsAvailable = random.Next(100) % 3 == 0 ? true : false,
                        ContactNumber = random.Next(999111, 999119).ToString()

                    });
                }
            }
            return personList;
        }

        /// <summary>
        /// Generates the random date.
        /// </summary>
        /// <returns></returns>
        private DateTime GenerateRandomDate()
        {
            int randInt = random.Next(4000);
            return DateTime.Now.AddDays(-8000 + randInt);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isdisposable)
        {
            if (this.EmployeeDetails != null)
            {
                this.EmployeeDetails.Clear();
            }
            filterChanged = null;
        }

        string[] city = new string[]
        {
            "NewYork",
            "San Francisco",
            "Delhi",
            "Brisbane",
            "Tokyo",
            "Rome",
            "Durban",
            "Canberra",
            "Sydney",
            "London",
            "Manchester",
            "Birmingham",
            "Liverpool",
            "Cardiff",
            "Adelaide",
            "Perth",
            "Zurich",
            "Madrid",
            "Barcelona"
        };
        #region Array Collection

        private static string[] names1 = new string[]{
            "George","John","Thomas","James","Andrew","Martin","William","Zachary",
            "Millard","Franklin","Abraham","Ulysses","Rutherford","Chester","Grover","Benjamin",
            "Theodore","Woodrow","Warren","Calvin","Herbert","Franklin","Harry","Dwight","Lyndon",
            "Richard","Gerald","Jimmy","Ronald","George","Bill", "Barack", "Warner","Peter", "Larry"
        };
        private static string[] names2 = new string[]{
             "Washington","Adams","Jefferson","Madison","Monroe","Jackson","Van Buren","Harrison","Tyler",
             "Polk","Taylor","Fillmore","Pierce","Buchanan","Lincoln","Johnson","Grant","Hayes","Garfield",
             "Arthur","Cleveland","Harrison","McKinley","Roosevelt","Taft","Wilson","Harding","Coolidge",
             "Hoover","Truman","Eisenhower","Kennedy","Johnson","Nixon","Ford","Carter","Reagan","Bush",
             "Clinton","Bush","Obama","Smith","Jones","Stogner"
        };
        private static string[] color = new string[]{
            "Red", "Blue", "Brown", "Unknown"
        };
        #endregion

        #region Filtering
        internal FilterChanged filterChanged;
        public ICommand ComboChanged
        {
            get { return new DelegateCommand<object>(ComboBoxChangedEvent, args => { return true; }); }
        }

        public ICommand FilterComboChanged
        {
            get { return new DelegateCommand<object>(FilterComboBoxChangedEvent, args => { return true; }); }
        }

        /// <summary>
        /// Occurs when the ComboBox item is changed.
        /// </summary>
        /// <param name="param"></param>
        private void FilterComboBoxChangedEvent(object param)
        {
            if (param != null)
            {
                this.FilterOption = (param as ComboBoxItem).Content.ToString().Replace(" ", "");
                filterChanged();
            }
        }

        /// <summary>
        /// Occurs when comboBox item is changed
        /// </summary>
        /// <param name="param"></param>
        private void ComboBoxChangedEvent(object param)
        {
            if (param != null)
            {
                this.FilterCondition = (param as ComboBoxItem).Content.ToString().Replace(" ", "");
                filterChanged();
            }
        }

        private bool MakeStringFilter(Employee o, string option, string condition)
        {
            var value = (o.GetType().GetTypeInfo()).GetDeclaredProperty(option);
            var exactValue = value.GetValue(o);
            exactValue = exactValue.ToString().ToLower();
            string text = FilterText.ToLower();
            var methods = (typeof(string).GetTypeInfo()).GetDeclaredMethods(condition);
            if (methods.Count() != 0)
            {
                var methodInfo = methods.FirstOrDefault();
                bool result1 = (bool)methodInfo.Invoke(exactValue, new object[] { text });
                return result1;
            }
            else
                return false;
        }

        private bool MakeNumericFilter(Employee o, string option, string condition)
        {
            var value = (o.GetType().GetTypeInfo()).GetDeclaredProperty(option);
            var exactValue = value.GetValue(o);
            double res;
            bool checkNumeric = double.TryParse(exactValue.ToString(), out res);
            if (checkNumeric)
            {
                switch (condition)
                {
                    case "Equals":
                        if (Convert.ToDouble(exactValue) == (Convert.ToDouble(FilterText)))
                            return true;
                        break;
                    case "GreaterThan":
                        if (Convert.ToDouble(exactValue) > Convert.ToDouble(FilterText))
                            return true;
                        break;
                    case "LessThan":
                        if (Convert.ToDouble(exactValue) < Convert.ToDouble(FilterText))
                            return true;
                        break;
                    case "NotEquals":
                        if (Convert.ToDouble(FilterText) != Convert.ToDouble(exactValue))
                            return true;
                        break;
                }
            }
            return false;
        }

        public bool FilerRecords(object o)
        {
            double res;
            bool checkNumeric = double.TryParse(FilterText, out res);
            var item = o as Employee;
            if (item != null && FilterText.Equals(""))
            {
                return true;
            }
            else
            {
                if (item != null)
                {
                    if (checkNumeric && FilterOption.Equals("ContactNumber"))
                    {
                        if (FilterCondition == null || FilterCondition.Equals("Equals") || FilterCondition.Equals("LessThan") || FilterCondition.Equals("GreaterThan") || FilterCondition.Equals("NotEquals"))
                            FilterCondition = "Contains";
                        bool result = MakeStringFilter(item, FilterOption, FilterCondition);
                        return result;
                    }
                    else if (checkNumeric && !FilterOption.Equals("AllColumns"))
                    {
                        if (FilterCondition == null || FilterCondition.Equals("Contains") || FilterCondition.Equals("StartsWith") || FilterCondition.Equals("EndsWith"))
                            FilterCondition = "Equals";
                        bool result = MakeNumericFilter(item, FilterOption, FilterCondition);
                        return result;
                    }
                    else if (FilterOption.Equals("AllColumns"))
                    {
                        if (item.FirstName.ToLower().Contains(FilterText.ToLower()) ||
                            item.LastName.ToLower().Contains(FilterText.ToLower()) || item.City.ToLower().Contains(FilterText.ToLower()) ||
                            item.Salary.ToString().ToLower().Contains(FilterText.ToLower()) || item.EmployeeID.ToString().ToLower().Contains(FilterText.ToLower()) ||
                            item.ContactNumber.ToLower().Contains(FilterText.ToLower()))
                            return true;
                        return false;
                    }
                    else
                    {
                        if (FilterCondition == null || FilterCondition.Equals("Equals") || FilterCondition.Equals("LessThan") || FilterCondition.Equals("GreaterThan") || FilterCondition.Equals("NotEquals"))
                            FilterCondition = "Contains";
                        bool result = MakeStringFilter(item, FilterOption, FilterCondition);
                        return result;
                    }
                }
            }
            return false;
        }

        private string filterOption = "AllColumns";

        /// <summary>
        /// Get or set the FilterOption
        /// </summary>
        public string FilterOption
        {
            get { return filterOption; }
            set
            {
                filterOption = value;
            }
        }

        private string filterText = string.Empty;
        /// <summary>
        /// Get or set the FilterText
        /// </summary>
        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                if (filterChanged != null)
                    filterChanged();
                RaisePropertyChanged("FilterText");
            }
        }

        private string filterCondition;
        /// <summary>
        /// Get or set the FilterCondition
        /// </summary>
        public string FilterCondition
        {
            get { return filterCondition; }
            set
            {
                filterCondition = value;
            }
        }
        #endregion
    }
    internal delegate void FilterChanged();
}

