using System;

namespace EmployeesApp
{
    class Program
    {
        public static void Main()
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.White;

            EmployeeApp.Run();
        }
    }
}
