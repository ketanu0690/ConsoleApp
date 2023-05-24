using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleApp3
{
    class Program
    {
        static string connectionString = "Data Source=DESKTOP-9PD5REF;Initial Catalog=TestDatabase;Integrated Security=True";

        // Define the constants for full-screen mode
        const int SW_MAXIMIZE = 3;
        const int SWP_SHOWWINDOW = 0x0040;

        // Import the necessary functions from the Windows API
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static void Main(string[] args)
        {
            // Maximize the window and show the cursor
            MaximizeWindow();
            Console.CursorVisible = true;

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            Console.WriteLine("Welcome to the Application!");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(new string('=', Console.WindowWidth - 1));
                Console.WriteLine("Please choose an option:");
                Console.WriteLine(new string('-', Console.WindowWidth - 1));
                Console.WriteLine("|" + CenterText("1. Create Account") + "|");
                Console.WriteLine("|" + CenterText("2. Login") + "|");
                Console.WriteLine("|" + CenterText("3. Exit") + "|");
                Console.WriteLine(new string('-', Console.WindowWidth - 1));
                Console.WriteLine();

                //Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enter your choice: ");
                //Console.ForegroundColor = ConsoleColor.DarkGreen;

                string input = Console.ReadLine();

                if (input == "1")
                {
                    UserSignup signup = new UserSignup(connectionString);
                    signup.CreateAccount();
                }
                else if (input == "2")
                {
                    UserLogin login = new UserLogin(connectionString);
                    bool isLoggedIn = login.LoginSystem();
                    if (isLoggedIn)
                    {
                        Console.Clear();
                        UserDashboard dashboard = new UserDashboard(connectionString);
                        AdminDashboard adminDashboard = new AdminDashboard(connectionString);
                        string email = login.GetEmail();
                        string password = login.GetPassword();

                        if (email == "admin" && password == "123")
                        {
                            
                            adminDashboard.ShowDashboard(email, password);
                            Console.Clear();
                        }
                        else
                        {
                            dashboard.ShowDashboard(email, password);
                            Console.Clear();
                        }
                    }

                    else
                    {
                        Console.WriteLine("Invalid username or password. Login failed.");
                        Console.Clear();
                        AnimateErrorMessage();
                    }
                }
                else if (input == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.Clear();
                    AnimateErrorMessage();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Thank you for using the Application!");
            Console.ReadLine();
        }

        static string CenterText(string text)
        {
            int width = Console.WindowWidth - 2;
            int padding = (width - text.Length) / 2;
            return text.PadLeft(padding + text.Length).PadRight(width);
        }

        static void AnimateErrorMessage()
        {
            // Store the current window position and size
            IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            RECT rect;
            GetWindowRect(hwnd, out rect);

            // Calculate the center position for the error message
            int centerX = (rect.Left + rect.Right) / 2;
            int centerY = (rect.Top + rect.Bottom) / 2;

            // Define the error message
            string errorMessage = "Invalid username or password. Login failed.";

            // Show the error message as a pop-up
            MessageBox(IntPtr.Zero, errorMessage, "Error", 0x00000030 | 0x00000001);
        }

        // Structures and functions for getting the window position and size
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // Import the MessageBox function from the Windows API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, int options);

        // Function to maximize the window and remove the title bar
        static void MaximizeWindow()
        {
            IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(hwnd, SW_MAXIMIZE);

            // Remove the title bar and set the window position to (0, 0)
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_SHOWWINDOW);
        }
    }
}
