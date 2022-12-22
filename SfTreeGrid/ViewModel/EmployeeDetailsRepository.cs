#region Copyright Syncfusion Inc. 2001 - 2022
// Copyright Syncfusion Inc. 2001 - 2022. All rights reserved.
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
    public class EmployeeDetailsRepository : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>

        #region Constructor

        public EmployeeDetailsRepository()
        {
            this.PopulateWithSampleData(500);
        }

        #endregion

        #region Public Properties

        static int baseCount = -1;

        private List<EmployeeInfo> _EmployeeDetails;

        /// <summary>
        /// Gets or sets the employee details.
        /// </summary>
        /// <value>The employee details.</value>
        public List<EmployeeInfo> EmployeeDetails
        {
            get { return _EmployeeDetails; }
            set { _EmployeeDetails = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the with sample data.
        /// </summary>
        /// <param name="count">The count.</param>
        public void PopulateWithSampleData(int count)
        {
            PopulateWithSampleData(count, true);
        }

        /// <summary>
        /// Populates the with flat data.
        /// </summary>
        /// <param name="count">The count.</param>
        public void PopulateWithFlatData(int count)
        {
            baseCount++;
            Random r = new Random(12313);
            EmployeeInfo emp;

            //just generate to random data in some manner - not really related to the TreeGrid itself...
            for (int i = 0; i < count; ++i)
            {
                emp = new EmployeeInfo();
                emp.ID = i;
                emp.FirstName = names1[r.Next(names1.GetLength(0))];
                emp.LastName = names2[r.Next(names2.GetLength(0))];
                emp.Salary = 30000d + r.Next(9) * 10000;
                emp.Title = title[r.Next(title.GetLength(0))];
                emp.City = city[r.Next(city.GetLength(0))];
                emp.Email = emp.LastName + "@hotmail.com";
                emp.Department = dept[r.Next(dept.GetLength(0))];
                this.EmployeeDetails.Add(emp);
            }
        }

        /// <summary>
        /// Populates with sample data.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="multipleRootNodes">if set to <c>true</c> [multiple root nodes].</param>
        public void PopulateWithSampleData(int count, bool multipleRootNodes)
        {
            this.EmployeeDetails = new List<EmployeeInfo>();
            Random r = new Random(12313);

            //just generate to random data in some manner - not really related to the TreeGrid itself...
            PopulateWithFlatData(count);

            //now need to set up the reportsto property...
            // first create a list of all the employees
            List<int> all = new List<int>();
            for (int i = 0; i < count; ++i)
            {
                all.Add(i);
            }

            //choose 2 big bosses - with half the employees reporting to each...

            //first big boss
            int randonLocation = r.Next(all.Count);
            int parentID = all[randonLocation];
            EmployeeInfo bigBoss = this.EmployeeDetails[parentID];
            bigBoss.FirstName = "Madison";
            bigBoss.Department = "General manager";
            bigBoss.Title = "Vice President";
            bigBoss.Salary = 200000d;
            bigBoss.ReportsTo = -1; //big boss reports to no one

            //remove boss for pool
            all.Remove(parentID);
            ObservableCollection<EmployeeInfo> employeesToProcess = new ObservableCollection<EmployeeInfo>();
            employeesToProcess = new ObservableCollection<EmployeeInfo>();
            employeesToProcess.Add(bigBoss);

            //now loop through and set direct reports to this parent
            int half = multipleRootNodes ? all.Count / 2 : 0;
            while (all.Count > half)
            {
                ObservableCollection<EmployeeInfo> nextSetToProcess = new ObservableCollection<EmployeeInfo>();
                nextSetToProcess = new ObservableCollection<EmployeeInfo>();

                foreach (EmployeeInfo e in employeesToProcess)
                {
                    int numberOfReports = r.Next(6) + 1;
                    while (numberOfReports > 0 && all.Count > half)
                    {
                        randonLocation = r.Next(all.Count);
                        int childID = all[randonLocation];
                        EmployeeInfo child = this.EmployeeDetails[childID];
                        nextSetToProcess.Add(child);
                        child.ReportsTo = e.ID;
                        numberOfReports--;
                        all.Remove(childID);
                    }
                    if (all.Count <= half)
                        break;
                }
                employeesToProcess = nextSetToProcess;
            }

            if (multipleRootNodes)
            {
                //second big boss
                randonLocation = r.Next(all.Count);
                parentID = all[randonLocation];
                bigBoss = this.EmployeeDetails[parentID];
                bigBoss.FirstName = "Hawkin";
                bigBoss.Department = "Business Manager";
                bigBoss.Title = "Sales Representative";
                bigBoss.Salary = 200000d;
                bigBoss.ReportsTo = -1; //big boss reports to no one

                //remove boss for pool
                all.Remove(parentID);
                employeesToProcess = new ObservableCollection<EmployeeInfo>();
                employeesToProcess.Add(bigBoss);

                int halfCount = all.Count / 2;
                //now loop through and set direct reports to this parent
                while (all.Count > halfCount)
                {
                    ObservableCollection<EmployeeInfo> nextSetToProcess = new ObservableCollection<EmployeeInfo>();
                    foreach (EmployeeInfo e in employeesToProcess)
                    {
                        int numberOfReports = r.Next(6) + 1;
                        while (numberOfReports > 0 && all.Count > halfCount)
                        {
                            randonLocation = r.Next(all.Count);
                            int childID = all[randonLocation];
                            EmployeeInfo child = this.EmployeeDetails[childID];
                            nextSetToProcess.Add(child);
                            child.ReportsTo = e.ID;
                            numberOfReports--;
                            all.Remove(childID);
                        }
                        if (all.Count == 0)
                            break;
                    }
                    employeesToProcess = nextSetToProcess;
                }


                //third big boss
                randonLocation = r.Next(all.Count);
                parentID = all[randonLocation];
                bigBoss = this.EmployeeDetails[parentID];
                bigBoss.FirstName = "Richard";
                bigBoss.Department = "Human resources";
                bigBoss.Title = "HR Coordinator";
                bigBoss.Salary = 200000d;
                bigBoss.ReportsTo = -1; //big boss reports to no one

                //remove boss for pool
                all.Remove(parentID);
                employeesToProcess = new ObservableCollection<EmployeeInfo>();
                employeesToProcess.Add(bigBoss);

                //now loop through and set direct reports to this parent
                while (all.Count > 0)
                {
                    ObservableCollection<EmployeeInfo> nextSetToProcess = new ObservableCollection<EmployeeInfo>();
                    foreach (EmployeeInfo e in employeesToProcess)
                    {
                        int numberOfReports = r.Next(6) + 1;
                        while (numberOfReports > 0 && all.Count > 0)
                        {
                            randonLocation = r.Next(all.Count);
                            int childID = all[randonLocation];
                            EmployeeInfo child = this.EmployeeDetails[childID];
                            nextSetToProcess.Add(child);
                            child.ReportsTo = e.ID;
                            numberOfReports--;
                            all.Remove(childID);
                        }
                        if (all.Count == 0)
                            break;
                    }
                    employeesToProcess = nextSetToProcess;
                }
            }
            this.EmployeeDetails.Sort();
        }

        /// <summary>
        /// Finds the ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public int FindID(int id)
        {
            int loc = -1;

            int top = 0;
            int bot = this.EmployeeDetails.Count - 1;

            int mid;
            do
            {
                mid = (top + bot) / 2;
                if (this.EmployeeDetails[mid].ReportsTo > id)
                {
                    bot = mid - 1;
                }
                else if (this.EmployeeDetails[mid].ReportsTo < id)
                {
                    top = mid + 1;
                }
            }
            while (bot > -1 && top < this.EmployeeDetails.Count && this.EmployeeDetails[mid].ReportsTo != id && bot >= top);
            if (this.EmployeeDetails[mid].ReportsTo == id)
            {
                while (mid > 0 && this.EmployeeDetails[mid - 1].ReportsTo == id)
                    mid--;
                loc = mid;
            }
            return loc;
        }

        /// <summary>
        /// Gets the reportees.
        /// </summary>
        /// <param name="bossID">The boss ID.</param>
        /// <returns></returns>
        public IEnumerable<EmployeeInfo> GetReportees(int bossID)
        {

            List<EmployeeInfo> list = new List<EmployeeInfo>();
            int loc = FindID(bossID);
            if (loc > -1)
            {
                while (loc < this.EmployeeDetails.Count && this.EmployeeDetails[loc].ReportsTo == bossID)
                    list.Add(this.EmployeeDetails[loc++]);
            }
            return list;
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
        }
        #endregion

        #region Array Collection

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

        private static string[] names1 = new string[]{
            "George","John","Thomas","James","Andrew","Martin","William","Zachary",
            "Millard","Franklin","Abraham","Ulysses","Rutherford","Chester","Grover","Benjamin",
            "Theodore","Woodrow","Warren","Calvin","Herbert","Franklin","Harry","Dwight","Lyndon",
            "Gerald","Jimmy","Ronald","George","Bill", "Barack", "Warner","Peter", "Larry"
        };

        private static string[] names2 = new string[]{
             "Washington","Adams","Jefferson","Madison","Monroe","Jackson","Van Buren","Harrison","Tyler",
             "Polk","Taylor","Fillmore","Pierce","Buchanan","Lincoln","Johnson","Grant","Hayes","Garfield",
             "Arthur","Cleveland","Harrison","McKinley","Roosevelt","Taft","Wilson","Harding","Coolidge",
             "Hoover","Truman","Eisenhower","Kennedy","Johnson","Nixon","Ford","Carter","Reagan","Bush",
             "Clinton","Bush","Obama","Smith","Jones","Stogner"
        };

        private static string[] dept = new string[]{
              "Accounts","Sales","BackOffice","HumanResource","Purchasing","Production"
        };

        private static string[] title = new string[]
        {
            "Engineering Manager","Production Control Manager","Design Engineer","Network Administrator",
            "Stocker","Shipping and Receiving Clerk","Production Technician","Master Scheduler",
            "Marketing Specialist","Recruiter","Maintenance Supervisor","Marketing Assistant","Tool Designer",
             "Senior Tool Designer","Quality Assurance Supervisor","Information Services Manager"
        };

        #endregion

    }
}

