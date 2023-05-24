using System;
using System.Data.SqlClient;

namespace ConsoleApp3
{
    public class UserDashboard
    {

        private string connectionString;

        public UserDashboard(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void ShowDashboard(string email, string password)
        {
            bool isLoggedIn = true;
            UserLogin login = new UserLogin(connectionString); // Instantiate UserLogin class

            while (isLoggedIn)
            {
                Console.WriteLine("User Dashboard");
                Console.WriteLine("1. View Profile");
                Console.WriteLine("2. View Blogs");
                Console.WriteLine("3. Delete Blogs");
                Console.WriteLine("4. Edit Blogs");
                Console.WriteLine("5. Create Blog");
                Console.WriteLine("6. Logout");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewProfile(email, password);
                        break;
                    case "2":
                        ViewBlogs(email, password);
                        break;
                    case "3":
                        DeleteBlog(email, password);
                        break;
                    case "4":
                        EditBlogs(email, password);
                        break;
                    case "5":
                        CreateBlog(email, password);
                        break;
                    case "6":
                        Console.WriteLine("Logging out and exiting User Dashboard...");
                        isLoggedIn = false; // Set isLoggedIn to false to show the login screen again
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine();
            }

            if (!isLoggedIn)
            {
                Console.WriteLine("You are now logged out.");
                Console.WriteLine("Please log in to continue.");
                login.LoginSystem(); // Call the LoginSystem method to show the login screen again
            }
        }
        public void ViewProfile(string email, string password)
        {
            Console.WriteLine("Viewing Profile...");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Student WHERE Email = @Email AND Password = @Password";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string userGender = reader.GetString(2);
                            int userAge = reader.GetInt32(3);
                            string userPassword = reader.GetString(4);
                            string userEmail = reader.GetString(5);

                            // Print the values
                            Console.WriteLine("ID: {0}", id);
                            Console.WriteLine("Name: {0}", name);
                            Console.WriteLine("Gender: {0}", userGender);
                            Console.WriteLine("Age: {0}", userAge);
                            Console.WriteLine("Password: {0}", userPassword);
                            Console.WriteLine("Email: {0}", userEmail);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No profile found with the provided email and password.");
                    }
                }
            }
        }
        public void ViewBlogs(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve student ID based on email and password
                int studentId;
                string studentQuery = "SELECT Id FROM Student WHERE Email = @Email AND Password = @Password";
                
                
                using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                {
                    studentCommand.Parameters.AddWithValue("@Email", email);
                    studentCommand.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader studentReader = studentCommand.ExecuteReader())
                    {
                        if (studentReader.Read())
                        {
                            studentId = studentReader.GetInt32(0);
                        }
                        else
                        {
                            Console.WriteLine("Invalid email or password. Access denied.");
                            return;
                        }
                    }
                }

                string query = "SELECT * FROM Blog WHERE StudentId = @StudentId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Viewing Blogs:\n");

                        while (reader.Read())
                        {
                            int blogId = reader.GetInt32(0);
                            string blogDescription = reader.GetString(1);

                            Console.WriteLine($"Blog ID: {blogId}");
                            Console.WriteLine($"Blog Description: {blogDescription}");
                            Console.WriteLine($"Student ID: {studentId}");
                            Console.WriteLine("--------------------------");
                        }
                    }
                }
            }
        }
        public void DeleteBlog(string email, string password)
        {
            Console.WriteLine("Deleting Blog...");
            Console.WriteLine("Enter the Blog ID you want to delete: ");
            int blogId;
            if (int.TryParse(Console.ReadLine(), out blogId))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve student ID based on email and password
                    int studentId;
                    string studentQuery = "SELECT Id FROM Student WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                    {
                        studentCommand.Parameters.AddWithValue("@Email", email);
                        studentCommand.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader studentReader = studentCommand.ExecuteReader())
                        {
                            if (studentReader.Read())
                            {
                                studentId = studentReader.GetInt32(0);
                            }
                            else
                            {
                                Console.WriteLine("Invalid email or password. Access denied.");
                                return;
                            }
                        } // Close the SqlDataReader explicitly
                    }

                    // Update the Blog column in the Student table
                    string updateStudentQuery = "UPDATE Student SET Blog = NULL WHERE Id = @StudentId";
                    using (SqlCommand updateStudentCommand = new SqlCommand(updateStudentQuery, connection))
                    {
                        updateStudentCommand.Parameters.AddWithValue("@StudentId", studentId);
                        updateStudentCommand.ExecuteNonQuery();
                    }

                    // Delete from the Blog table
                    string deleteQuery = "DELETE FROM Blog WHERE BlogId = @BlogId AND StudentId = @StudentId";
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BlogId", blogId);
                        command.Parameters.AddWithValue("@StudentId", studentId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Blog deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Blog not found or you don't have permission to delete it.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Blog ID. Please enter a valid integer.");
            }
        }
        public void EditBlogs(string email, string password)
        {
            Console.WriteLine("Editing Blogs...");
            Console.WriteLine("Enter the Blog ID you want to edit: ");
            int blogId;
            if (int.TryParse(Console.ReadLine(), out blogId))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve student ID based on email and password
                    int studentId;
                    string studentQuery = "SELECT Id FROM Student WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                    {
                        studentCommand.Parameters.AddWithValue("@Email", email);
                        studentCommand.Parameters.AddWithValue("@Password", password);
                        using (SqlDataReader studentReader = studentCommand.ExecuteReader())
                        {
                            if (studentReader.Read())
                            {
                                studentId = studentReader.GetInt32(0);
                            }
                            else
                            {
                                Console.WriteLine("Invalid email or password. Access denied.");
                                return;
                            }
                        }
                    }

                    string selectQuery = "SELECT * FROM Blog WHERE blogId = @BlogId AND StudentId = @StudentId";

                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@BlogId", blogId);
                        selectCommand.Parameters.AddWithValue("@StudentId", studentId);

                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Existing blog details
                                int existingBlogId = reader.GetInt32(0);
                                string existingBlogDescription = reader.GetString(1);

                                Console.WriteLine($"Current Blog ID: {existingBlogId}");
                                Console.WriteLine($"Current Blog Description: {existingBlogDescription}");

                                reader.Close(); // Close the reader before executing the next command

                                // Prompt for new blog description
                                Console.WriteLine("Enter the new Blog Description: ");
                                string newBlogDescription = Console.ReadLine();

                                // Update the blog in the database
                                string updateQuery = "UPDATE Blog SET blogDescription = @NewBlogDescription WHERE blogId = @BlogId";

                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@NewBlogDescription", newBlogDescription);
                                    updateCommand.Parameters.AddWithValue("@BlogId", blogId);

                                    int rowsAffected = updateCommand.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        Console.WriteLine("Blog updated successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to update the blog. Please try again.");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Blog not found or you don't have permission to edit it.");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Blog ID. Please enter a valid integer.");
            }
        }
        public void CreateBlog(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the user exists in the Student table
                string checkUserQuery = "SELECT Id FROM Student WHERE Email = @Email AND Password = @Password";

                using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                {
                    checkUserCommand.Parameters.AddWithValue("@Email", email);
                    checkUserCommand.Parameters.AddWithValue("@Password", password);

                    int userId = (int)checkUserCommand.ExecuteScalar();

                    if (userId > 0)
                    {
                        // User exists, proceed with blog creation
                        Console.WriteLine("Enter Your Blog Description");
                        string blogDescription = Console.ReadLine();

                        //Random random = new Random();
                        //int blogId = random.Next(1, 100); // Generates a random blog ID between 1 and 100

                        string createBlogQuery = "INSERT INTO Blog ( blogDescription, StudentId) VALUES ( @BlogDescription, @StudentId)";

                        using (SqlCommand createBlogCommand = new SqlCommand(createBlogQuery, connection))
                        {
                            //createBlogCommand.Parameters.AddWithValue("@BlogId", blogId);
                            createBlogCommand.Parameters.AddWithValue("@BlogDescription", blogDescription);
                            createBlogCommand.Parameters.AddWithValue("@StudentId", userId);

                            createBlogCommand.ExecuteNonQuery();
                        }

                        Console.WriteLine("Blog created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("User not found. Cannot create blog.");
                    }
                }
            }
        }


    }
}


