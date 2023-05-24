using System;

namespace ConsoleApp3
{
    class Program
    {
        static string connectionString = "Data Source=DESKTOP-9PD5REF;Initial Catalog=TestDatabase;Integrated Security=True";

        static void Main(string[] args)
        {
            UserLogin login = new UserLogin(connectionString);

            bool isLoggedIn = login.LoginSystem();

            if (isLoggedIn)
            {
                UserDashboard dashboard = new UserDashboard(connectionString);
                dashboard.ShowDashboard(login.GetEmail(), login.GetPassword());
            }
            else
            {
                Console.WriteLine("Invalid username or password. Login failed.");
            }

            Console.ReadLine();
        }
    }
}
