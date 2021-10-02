using System;
using System.Collections.Generic;

namespace EmployeesApp
{
    class Employee
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string LastName { get; private set; }
        public string Adress { get; private set; }
        public int ID { get; private set; }

        private string wager;

        public string Wager
        {
            get
            {
                if (!EmployeeApp.isAdmin)
                {
                    return "Wager unavailable for display!";
                }
                else
                {
                    return this.wager;
                }
            }
            private set { wager = value; }
        }


        public Employee(string name, string surname, string lastName, string adress, int id, string wager)
        {
            this.Name = name;
            this.Surname = surname;
            this.LastName = lastName;
            this.Adress = adress;
            this.ID = id;
            this.Wager = wager;
        }

        public string Print()
        {
            return $"{Environment.NewLine}Name: {this.Name} {Environment.NewLine}Surname: {this.Surname} {Environment.NewLine}Last Name: {this.LastName} {Environment.NewLine}Adress: {this.Adress} {Environment.NewLine}ID: {this.ID} {Environment.NewLine}Wager: {this.Wager}{Environment.NewLine}";
        }

        public bool isEqualTo(List<string> employeeInfo)
        {
            return employeeInfo[0] == this.Name && employeeInfo[1] == this.Surname && employeeInfo[2] == this.LastName && employeeInfo[3] == this.Adress && int.Parse(employeeInfo[4]) == this.ID && employeeInfo[5] == this.wager;
        }

    }
}
