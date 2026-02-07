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
    public partial class AddUserForm : UserControl
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public AddUserForm()
        {
            InitializeComponent();
            connect = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\user\\OneDrive - University of Kelaniya\\Documents\\SMarketSystem.mdf\";Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connect);
            connection.Open();
            loadDataGridView();
            connection.Close();
        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }
            loadDataGridView();
        }
        public bool emptyfields()
        {
            if (txtUsername.Text == "" || txtPassword.Text == "" || txtPhone.Text == "" ||
                cmbRole.Text == "" || cmbStatus.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void loadDataGridView()
        {
            connection = new SqlConnection(connect);
            try
            {
                connection.Open();
                string sql = "SELECT * FROM [user]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "user");

                gridUserData.DataSource = ds;
                gridUserData.DataMember = "user";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { connection.Close(); }
        }

        public void clearForm()
        {
            txtUserID.Clear();
            txtPhone.Clear();
            cmbRole.Text = string.Empty;
            cmbStatus.Text = string.Empty;
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (emptyfields())
            {
                MessageBox.Show("All field are required to be filled", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection = new SqlConnection(connect);

                try
                {
                    connection.Open();

                    string query = "INSERT INTO [user] (username,password,role,status,phone,date) VALUES(@name,@pass,@role,@status,@phone,@date)";
                    DateTime today = DateTime.Today;

                    cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@name", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("pass", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@date", today);



                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Members Data Save Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadDataGridView();
                        clearForm();
                    }
                    else
                    {
                        MessageBox.Show("Try again!!!", "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (emptyfields())
            {
                MessageBox.Show("All field are required to be filled", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection = new SqlConnection(connect);

                DialogResult result = MessageBox.Show("Are you sure you want to Update Username: " + txtUsername.Text.Trim()
                    + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();

                        string query = "UPDATE [user] SET username = @name ,password = @pass, role = @role, status = @status,phone = @phone,date = @date WHERE Id =@id";
                        DateTime today = DateTime.Today;

                        cmd = new SqlCommand(query, connection);

                        cmd.Parameters.AddWithValue("@id", txtUserID.Text);
                        cmd.Parameters.AddWithValue("@name", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("pass", txtPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@date", today);

                        int results = cmd.ExecuteNonQuery();

                        if (results == 1)
                        {
                            MessageBox.Show("Members Data Updated Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadDataGridView();
                            clearForm();
                        }
                        else
                        {
                            MessageBox.Show("Try again!!!", "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:" + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (emptyfields())
            {
                MessageBox.Show("All field are required to be filled", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection = new SqlConnection(connect);

                DialogResult result = MessageBox.Show("Are you sure you want to Deleted Username: " + txtUsername.Text.Trim()
                    + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();

                        string query = "DELETE FROM [user] WHERE Id =@id";
                        DateTime today = DateTime.Today;

                        cmd = new SqlCommand(query, connection);

                        cmd.Parameters.AddWithValue("@id", txtUserID.Text);

                        int results = cmd.ExecuteNonQuery();

                        if (results == 1)
                        {
                            MessageBox.Show("Members Data Deleted Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadDataGridView();
                            clearForm();
                        }
                        else
                        {
                            MessageBox.Show("Try again!!!", "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:" + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void gridUserData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = gridUserData.Rows[e.RowIndex];
            txtUserID.Text = row.Cells[0].Value.ToString();
            txtUsername.Text = row.Cells[1].Value.ToString();
            txtPassword.Text = row.Cells[2].Value.ToString();
            cmbRole.Text = row.Cells[3].Value.ToString();
            cmbStatus.Text = row.Cells[4].Value.ToString();
            txtPhone.Text = row.Cells[5].Value.ToString();
        }
    }
}
