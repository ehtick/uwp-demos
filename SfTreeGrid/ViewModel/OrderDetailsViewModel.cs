#region Copyright Syncfusion® Inc. 2001-2025.
// Copyright Syncfusion® Inc. 2001-2025. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.SampleBrowser.UWP.SfTreeGrid
{
    public class OrderInfoRepository
    {
        private static Random random = new Random(123123);
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public OrderInfoRepository()
        {
            this.EmployeeDetails = this.CreateGenericPersonData(10, 8);
            CityCollection = new ObservableCollection<string>();
            foreach (var item in city)
            {
                CityCollection.Add(item);
            }
        }

        private ObservableCollection<OrderDetails> _EmployeeDetails;

        /// <summary>
        /// Gets or sets the person details.
        /// </summary>
        /// <value>The person details.</value>
        public ObservableCollection<OrderDetails> EmployeeDetails
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
        public OrderInfoRepository(int count, int maxGenerations)
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
        public ObservableCollection<OrderDetails> CreateChildList(int count, int maxGenerations, string lastName)
        {
            ObservableCollection<OrderDetails> _children = new ObservableCollection<OrderDetails>();
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
        public ObservableCollection<OrderDetails> CreateGenericPersonData(int count, int maxGenerations)
        {
            var personList = new ObservableCollection<OrderDetails>();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var lastname = names3[random.Next(names3.GetLength(0))];
                    personList.Add(new OrderDetails()
                    {
                        OrderDate = GenerateRandomDate().Date,
                        CustomerID=lastname,
                        OrderID=1000+i,
                        
                        CustomerCity= city[random.Next(city.GetLength(0))],
                        Discount = hike[random.Next(hike.GetLength(0))],
                        UnitPrice= 30000d + random.Next(9) * 1000,
                        CustomerArea= city[random.Next(city.GetLength(0))],
                        Children = this.CreateChildList(count, (int)Math.Max(0, maxGenerations - 1), lastname),
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
        private static  string[] names3 = new string[]{
            "ALFKI",
            "FRANS",
            "MEREP",
            "FOLKO",
            "SIMOB",
            "WARTH",
            "VAFFE",
            "FURIB",
            "SEVES",
            "LINOD",
            "RISCU",
            "PICCO",
            "BLONP",
            "WELLI",
            "FOLIG"
        };

        private static double[] hike = new double[]{
            6,5.5,10,10.2,11, 15, 6.8,14,7.7,9.5,8.2,16
        };
        #endregion
    }
}
