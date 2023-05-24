using System;
using System.Data.SqlClient;

namespace ConsoleApp3
{
    public class UserLogin
    {
        private readonly string connectionString;
        private string email;
        private string password;

        public UserLogin(string connectionString)
        {
            this.connectionString = connectionString;
        }
        
        public bool LoginSystem()
        {
            Console.WriteLine("Login System");

            Console.Write("Enter your email: ");
            email = Console.ReadLine();

            Console.Write("Enter your password: ");

            password = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Student WHERE Email = @Email AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public string GetEmail()
        {
            return email;
        }

        public string GetPassword()
        {
            return password;
        }
    }
}
