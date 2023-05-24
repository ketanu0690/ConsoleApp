using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace ConsoleApp3
{
    class AdminDashboard
    {
        private string connectionString;

        public AdminDashboard(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void ShowDashboard(string email, string password)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Admin Dashboard!");
            Console.WriteLine("1. Get all male students");
            Console.WriteLine("2. Get all female students");
            Console.WriteLine("3. Exit");

            Console.WriteLine("\nEnter your choice (1-3):");
            string choice = Console.ReadLine();

            while (choice != "3")
            {
                switch (choice)
                {
                    case "1":
                        GetAllMaleStudents();
                        break;

                    case "2":
                        GetAllFemaleStudents();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                Console.WriteLine("\nEnter your choice (1-3):");
                choice = Console.ReadLine();
            }

            Console.WriteLine("Exiting Admin Dashboard...");
        }

        private void GetAllMaleStudents()
        {
            Console.WriteLine("Displaying all male students...");

   
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("spGetMaleStudentsOnly", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string studentName = reader["Name"].ToString(); // Replace "Name" with the actual column name for student name

                            // Display the student's name or perform any other desired operations
                            Console.WriteLine(studentName);
                        }
                    }
                }
            }
        }


        private void GetAllFemaleStudents()
        {
            // TODO: Implement the logic to retrieve and display all female students
            Console.WriteLine("Displaying all female students...");
        }
    }
}
