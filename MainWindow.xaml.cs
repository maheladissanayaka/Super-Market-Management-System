
using System.Windows;
using System.Windows.Input;
using System.Text;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Forms;
using System;
using static System.Net.Mime.MediaTypeNames;


namespace SuperMarketManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public MainWindow()
        {
            InitializeComponent();
            connect = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\user\\OneDrive - University of Kelaniya\\Documents\\SMarketSystem.mdf\";Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connect);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public bool emptyFields()
        {
            if (txtUsername.Text == "" || passwordBox.Password == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            if (emptyFields())
            {
                System.Windows.MessageBox.Show("All fields are required to be filled", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                connection = new SqlConnection(connect);

                try
                {
                    connection.Open();

                    string selectAccount = "SELECT COUNT(*) FROM [user] WHERE username = @usern AND password = @pass AND status = @status";

                    using (SqlCommand cmd = new SqlCommand(selectAccount, connection))
                    {
                        cmd.Parameters.AddWithValue("@usern", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@pass", passwordBox.Password.Trim());
                        cmd.Parameters.AddWithValue("@status", "Active");

                        int rowCount = (int)cmd.ExecuteScalar();

                        if (rowCount > 0)
                        {
                            string selectRole = "SELECT role FROM [user] WHERE username = @usern AND password = @pass";

                            using (SqlCommand getRole = new SqlCommand(selectRole, connection))
                            {
                                getRole.Parameters.AddWithValue("@usern", txtUsername.Text.Trim());
                                getRole.Parameters.AddWithValue("@pass", passwordBox.Password.Trim());

                                string userRole = getRole.ExecuteScalar() as string;

                                System.Windows.MessageBox.Show("Login successfully!", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);

                                if (userRole == "Admin")
                                {
                                    AdminMainForm adminForm1 = new AdminMainForm();
                                    adminForm1.Show();
                                    this.Hide();
                                }
                                else if (userRole == "Cashier")
                                {
                                    UserMainForm cachierMainForm1 = new UserMainForm();
                                    cachierMainForm1.Show();
                                    this.Hide();
                                }
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Incorrect Username/Password or there's no Admin's Approval.", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Connection Failed" + ex, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }

        private bool isPasswordVisible = false;
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                textBox.Text = passwordBox.Password;
                passwordBox.Visibility = Visibility.Collapsed;
                textBox.Visibility = Visibility.Visible;
                revealBtn.Content = "👁";
            }
            else
            {
                passwordBox.Password = textBox.Text;
                passwordBox.Visibility = Visibility.Visible;
                textBox.Visibility = Visibility.Collapsed;
                revealBtn.Content = "👁";

            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isPasswordVisible)
                textBox.Text = passwordBox.Password;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPasswordVisible)
                passwordBox.Password = textBox.Text;
        }
        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.Close();
            }
        }

        private void chkShow_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
