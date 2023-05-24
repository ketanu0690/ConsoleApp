using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class UserSignup
    {
        private string connectionString;
        public UserSignup(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void CreateAccount()


        {
            Console.WriteLine("Create Account");
            Console.WriteLine("----------------");

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.Write("Gender: ");
            string gender = Console.ReadLine();

            Console.Write("Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Select Department:");
            Console.WriteLine("1. Mechanical");
            Console.WriteLine("2. IT");
            Console.WriteLine("3. Computer Science");
            Console.WriteLine("4. Civil");

            int departmentId = 0;
            bool isValidDepartment = false;

            while (!isValidDepartment)
            {
                Console.Write("Department ID: ");
                if (int.TryParse(Console.ReadLine(), out departmentId) && departmentId >= 1 && departmentId <= 4)
                {
                    isValidDepartment = true;
                }
                else
                {
                    Console.WriteLine("Invalid department ID. Please try again.");
                }
            }
            //here are using entity framework  to  enter student details 
            using (var context = new TestDatabaseEntities1())
              
            {
                var student = new Student
                {
                    Name = name,
                    Gender = gender,
                    email = email,
                    Password = password,
                    Age = age,
                    DepartmentID = departmentId
                };

                context.Students.Add(student);
                context.SaveChanges();
            }

            Console.WriteLine("Account created successfully!");
        }
    
 
    
    }
}
