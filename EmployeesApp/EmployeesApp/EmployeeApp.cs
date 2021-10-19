using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EmployeesApp
{
    class EmployeeApp
    {
        private static List<Employee> Employees = new List<Employee>();

        public static bool isAdmin;

        private static bool initialStatus;

        public static void Run()
        {
            Console.Write("User (Admin/Standart): ");
            string standartOrAdmin = Console.ReadLine().ToLower();

            isAdmin = standartOrAdmin == "admin";

            initialStatus = isAdmin;

            if (standartOrAdmin == "admin" || standartOrAdmin == "standart")
                ParseEmployeeInfo();

            PrintEmployeeInfo(standartOrAdmin);

            ModifyEmployeesInfoAccordingly(standartOrAdmin);
        }

        private static void ModifyEmployeesInfoAccordingly(string standartOrAdmin)
        {
            string command = string.Empty;

            while (command != "stop")
            {
                string availableOptions = isAdmin ? "\"Sort\" or \"Add\"/\"Remove\" employees and \"Print\" employees" : "\"Sort\" and \"Print\" employees";
                Console.WriteLine($"Hello user you can: {availableOptions}, what do you want to do?"); Console.Write("Input: ");
                command = Console.ReadLine();

                switch (command.ToLower())
                {
                    case "sort":
                        SortEmployees(standartOrAdmin);
                        break;
                    case "add":
                        if (isAdmin)
                        {
                            AddEmployee();
                        }
                        else
                        {
                            PrintErrorMessage("Only admins can do that!");
                            ModifyEmployeesInfoAccordingly(standartOrAdmin);
                        }
                        break;
                    case "remove":
                        if (isAdmin)
                        {
                            RemoveEmployee();
                        }
                        else
                        {
                            PrintErrorMessage("Only admins can do that!");
                            ModifyEmployeesInfoAccordingly(standartOrAdmin);
                        }
                        break;
                    case "print":
                        PrintEmployeeInfo(standartOrAdmin);
                        break;
                    case "exit":
                        return;
                    default:
                        PrintErrorMessage($"Invalid input, your options are to {availableOptions}");
                        ModifyEmployeesInfoAccordingly(standartOrAdmin);
                        break;
                }
            }

        }

        private static void RemoveEmployee()
        {
            int employeeForRemovalID = GetEmployeeID();
            Employee employeeForRemoval = Employees.SingleOrDefault(e => e.ID == employeeForRemovalID);
            if (employeeForRemoval == null)
            {
                PrintErrorMessage("Employee not found!");
                RemoveEmployee();
            }
            else
            {
                Employees.Remove(employeeForRemoval);
                PrintSuccessMessage($"{employeeForRemoval.Name.Trim()} {employeeForRemoval.Surname.Trim()} {employeeForRemoval.LastName.Trim()} successfully removed!");
                WriteEmployeesToExternalFile();
            }
        }

        private static void WriteEmployeesToExternalFile()
        {
            FileStream fcreate = File.Open("Employees.txt", FileMode.Create);
            isAdmin = true;
            using (StreamWriter writter = new StreamWriter(fcreate))
            {
                foreach (Employee employee in Employees)
                {
                    writter.WriteLine(employee.Print());
                }
            }
            isAdmin = initialStatus;
        }

        private static int GetEmployeeID()
        {
            Console.Write("ID(5 digits): ");
            string employeeID = Console.ReadLine();
            int id;
            if (int.TryParse(employeeID, out id) && employeeID.Length == 5)
            {
                return id;
            }
            else
            {
                PrintErrorMessage("Invalid ID!");
                GetEmployeeID();
                return 0;
            }
        }

        private static void PrintSuccessMessage(string text)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        private static void SortEmployees(string standartOrAdmin)
        {
            Console.Write("Please type in the parameter you want to sort by: ");
            string prop = FixFormatingProp(Console.ReadLine());
            if (prop != "Invalid prop!")
            {
                Employees = Employees.OrderBy(e => e.GetType().GetProperty(prop).GetValue(e)).ToList(); PrintEmployeeInfo(standartOrAdmin);
                WriteEmployeesToExternalFile();
            }
            else
            {
                PrintErrorMessage($"{prop}");
                ModifyEmployeesInfoAccordingly(standartOrAdmin);
            }
        }

        private static void AddEmployee()
        {
            string[] employeeRequiredInfo = GetDesiredEmployeeInfo();

            Employees.Add(new Employee(employeeRequiredInfo[6], employeeRequiredInfo[7], employeeRequiredInfo[8], employeeRequiredInfo[9], int.Parse(employeeRequiredInfo[10]), employeeRequiredInfo[11]));
            FileStream fileToAppend = File.Open("Employees.txt", FileMode.Append);

            Employee employeeToWrite = Employees[Employees.Count - 1];

            using (StreamWriter writter = new StreamWriter(fileToAppend))
            {
                writter.WriteLine(employeeToWrite.Print());
            }

            PrintSuccessMessage($"{employeeToWrite.Name} {employeeToWrite.Surname} {employeeToWrite.LastName} successfully added!");
        }

        private static string[] GetDesiredEmployeeInfo()
        {
            string[] employeeRequiredInfo = new string[12] { "Name:", "Surname:", "Last Name:", "Adress:", "ID(5 dig its):", "Wage:", "", "", "", "", "", "" };
            for (int i = 0; i < employeeRequiredInfo.Length / 2; i++)
            {
                if (i == 4)
                {
                    employeeRequiredInfo[i + employeeRequiredInfo.Length / 2] = generateUniqueID().ToString(); 
                    PrintSuccessMessage($"Successfuly generated unique ID!");
                }
                else
                {
                    Console.Write($"{employeeRequiredInfo[i]} ");
                    string input = Console.ReadLine();
                    employeeRequiredInfo[i + employeeRequiredInfo.Length / 2] = input;
                }
            }
            return employeeRequiredInfo;
        }

        private static int generateUniqueID()
        {
            Random rnd = new Random();
            int id = rnd.Next(10000, 99999);
            if (Employees.SingleOrDefault(e => e.ID == id) == null)
            {
                return id;
            }
            else
            {
                generateUniqueID();
                return 0;
            }
        }

        private static string FixFormatingProp(string parameter)
        {
            switch (parameter.ToLower())
            {
                case "name": return "Name";
                case "surname": return "Surname";
                case "last name": return "LastName";
                case "adress": return "Adress";
                case "wage": return "Wage";
                case "id": return "ID";
                default: return "Invalid prop!";
            }
        }

        private static void PrintEmployeeInfo(string standartOrAdmin)
        {
            switch (standartOrAdmin)
            {
                case "admin":
                case "standart":
                    foreach (Employee employee in Employees)
                    {
                        Console.WriteLine(employee.Print());
                    }
                    break;
                default:
                    PrintErrorMessage($"{standartOrAdmin} doesn't exist, your options are: Admin/Standart"); Run();
                    break;
            }
        }

        private static void PrintErrorMessage(string text)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Restarting...");
            Console.WriteLine();
        }

        private static void ParseEmployeeInfo()
        {

            using (StreamReader reader = new StreamReader("Employees.txt"))
            {
                string line = reader.ReadLine();
                List<string> employeeInfo = new List<string>();
                int employeeCount = -1;
                while (line != null)
                {
                    if (line != "")
                    {
                        string[] rawInfo = line.Split(':');
                        string currentEmployeeInfo = rawInfo[1].Substring(1);

                        if (rawInfo[0] == "Wage")
                        {
                            employeeInfo.Add(currentEmployeeInfo);
                            if (Employees.Count > 0)
                            {
                                if (!Employees[employeeCount].isEqualTo(employeeInfo))
                                {
                                    Employees.Add(new Employee(employeeInfo[0], employeeInfo[1], employeeInfo[2], employeeInfo[3], int.Parse(employeeInfo[4]), employeeInfo[5]));
                                    employeeCount++;
                                }
                            }
                            else
                            {
                                Employees.Add(new Employee(employeeInfo[0], employeeInfo[1], employeeInfo[2], employeeInfo[3], int.Parse(employeeInfo[4]), employeeInfo[5]));
                                employeeCount++;
                            }
                            employeeInfo.Clear();
                            line = reader.ReadLine();
                            if (line != "" && line != null)
                            {
                                rawInfo = line.Split(':');
                                currentEmployeeInfo = rawInfo[1].Substring(1);
                            }
                        }

                        if (line != "")
                            employeeInfo.Add(currentEmployeeInfo);

                        line = reader.ReadLine();
                    }
                    else
                    {
                        line = reader.ReadLine();
                    }
                }
            }
        }
    }    
}
