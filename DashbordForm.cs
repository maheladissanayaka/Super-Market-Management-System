using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketManagementSystem
{
    public partial class DashbordForm : UserControl
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public DashbordForm()
        {
            InitializeComponent();
            connect = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\user\\OneDrive - University of Kelaniya\\Documents\\SMarketSystem.mdf\";Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connect);
            connection.Open();
            displayTotalCashier();
            displayTotalCategory();
            displayTotalIncome();
            displayTodaysIncome();
            connection.Close();
        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }
            displayTotalCashier();
            displayTotalCategory();
            displayTotalIncome();
            displayTodaysIncome();
        }
        public void displayTotalCashier()
        {
            connection = new SqlConnection(connect);
            try
            {
                connection.Open();

                string selectData = "SELECT COUNT(id) FROM [user] WHERE role = @role AND status = @status";

                using (SqlCommand cmd = new SqlCommand(selectData, connection))
                {
                    cmd.Parameters.AddWithValue("@role", "Cashier");
                    cmd.Parameters.AddWithValue("@status", "Active");

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int count = Convert.ToInt32(reader[0]);
                        lblUser.Text = count.ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Connection: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

        }

        public void displayTotalCategory()
        {
            connection = new SqlConnection(connect);
            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT COUNT(Id) FROM category";

                    using (SqlCommand cmd = new SqlCommand(selectData, connection))
                    {

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            lblCategories.Text = count.ToString();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed Connection: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void displayTotalIncome()
        {
            connection = new SqlConnection(connect);
            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT SUM(total_price) FROM sales";

                    using (SqlCommand cmd = new SqlCommand(selectData, connection))
                    {

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            lblIncome.Text = "Rs " + count.ToString("0.00");
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed Connection: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void displayTodaysIncome()
        {
            connection = new SqlConnection(connect);
            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT SUM(total_price) FROM sales WHERE pay_date = @date";

                    using (SqlCommand cmd = new SqlCommand(selectData, connection))
                    {
                        DateTime today = DateTime.Today;
                        string getToday = today.ToString("yyyy-MM-dd");

                        cmd.Parameters.AddWithValue("@date", getToday);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            lblRevenue.Text = "Rs " + count.ToString("0.00");
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed Connection", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
