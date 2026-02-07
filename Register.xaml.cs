using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperMarketManagementSystem
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public Register()
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
        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private bool isPasswordVisible = false;

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            Console.WriteLine("Togglng reveal");
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

        private bool isComPasswordVisible = false;
        private void comPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isPasswordVisible)
                comTextBox.Text = comPasswordBox.Password;
        }

        private void comTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPasswordVisible)
                comPasswordBox.Password = comTextBox.Text;
        }

        private void comRevealBtn_Click(object sender, RoutedEventArgs e)
        {
            isComPasswordVisible = !isComPasswordVisible;
            Console.WriteLine("Togglng reveal");
            if (isComPasswordVisible)
            {
                comTextBox.Text = comPasswordBox.Password;
                comPasswordBox.Visibility = Visibility.Collapsed;
                comTextBox.Visibility = Visibility.Visible;
                comRevealBtn.Content = "👁";
            }
            else
            {
                comPasswordBox.Password = comTextBox.Text;
                comPasswordBox.Visibility = Visibility.Visible;
                comTextBox.Visibility = Visibility.Collapsed;
                revealBtn.Content = "👁";

            }
        }
        public bool emptyFields()
        {
            if (txtUsername.Text == "" || passwordBox.ToString() == "" ||
                comPasswordBox.ToString() == "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (emptyFields())
            {
                System.Windows.MessageBox.Show("All fields are requird to be filled.", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                connection = new SqlConnection(connect);

                try
                {
                    connection.Open();

                    string query = "SELECT * FROM [user] WHERE username = @usern";

                    using (SqlCommand checkUsername = new SqlCommand(query, connection))
                    {
                        checkUsername.Parameters.AddWithValue("@usern", txtUsername.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(checkUsername);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count >= 1)
                        {
                            string usern = txtUsername.Text.Substring(0, 1).ToUpper() + txtUsername.Text.Substring(1);
                            System.Windows.MessageBox.Show(usern + " is already taken", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (passwordBox.Password != comPasswordBox.Password)
                        {
                            System.Windows.MessageBox.Show("Password dose not match.", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (passwordBox.Password.Length < 8)
                        {
                            System.Windows.MessageBox.Show("Invalid Password, at least 8 characters are needed.", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            string insertData = "INSERT INTO [user] (username, password, role, status, phone, date) " +
                                "values(@usern, @pass, @role, @status,@phone,@date)";
                            DateTime today = DateTime.Today;

                            using (SqlCommand cmd = new SqlCommand(insertData, connection))
                            {
                                cmd.Parameters.AddWithValue("@usern", txtUsername.Text.Trim());
                                cmd.Parameters.AddWithValue("@pass", passwordBox.Password.Trim());
                                cmd.Parameters.AddWithValue("@role", "Cashier");
                                cmd.Parameters.AddWithValue("@status", "Approval");
                                cmd.Parameters.AddWithValue("@phone", "");
                                cmd.Parameters.AddWithValue("@date", today);


                                cmd.ExecuteNonQuery();

                                System.Windows.MessageBox.Show("Registered successfully.", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);

                                //switch form into login form
                                MainWindow form1 = new MainWindow();
                                form1.Show();
                                this.Hide();
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Connection failed: " + ex, "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connection.Close();
                }

            }
        }
    }
}
