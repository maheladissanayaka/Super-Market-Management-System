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
    public partial class TotalCategoryForm : UserControl
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public TotalCategoryForm()
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

        public void loadDataGridView()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connect))
                {
                    conn.Open();
                    string sql = "SELECT * FROM [category]";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn);

                    DataSet ds = new DataSet();
                    dataAdapter.Fill(ds, "category");

                    gridCategoryData.DataSource = ds;
                    gridCategoryData.DataMember = "category";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Category data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool emptyfields()
        {
            if (txtCategoryID.Text == "" || txtCategoryName.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void clearForm()
        {
            txtCategoryID.Clear();
            txtCategoryName.Clear();
            txtSearch.Clear();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(connect);

            try
            {
                connection.Open();

                string query = "INSERT INTO category VALUES(@id,@name)";

                cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@id", txtCategoryID.Text.Trim());
                cmd.Parameters.AddWithValue("@name", txtCategoryName.Text.Trim());

                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("Category Data Save Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Error: " + ex, "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private int id = 0;
        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(connect);

            try
            {
                connection.Open();

                string query = "UPDATE category SET categoryID = @cId , categoryName = @name WHERE Id =@id";

                cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@cId", txtCategoryID.Text.Trim());
                cmd.Parameters.AddWithValue("@name", txtCategoryName.Text.Trim());
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("Category Data Updated Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Error: " + ex, "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(connect);

            try
            {
                connection.Open();

                string query = "DELETE FROM category WHERE Id =@id";

                cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("Category Data Deleted Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Error: " + ex, "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnClearCategory_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void gridCategoryData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = gridCategoryData.Rows[e.RowIndex];
            id = Convert.ToInt32(row.Cells[0].Value);
            txtCategoryID.Text = row.Cells[1].Value.ToString();
            txtCategoryName.Text = row.Cells[2].Value.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            connection = new SqlConnection(connect);
            try
            {
                connection.Open();
                string sql = "SELECT * FROM category WHERE categoryName LIKE '%" + txtSearch.Text + "%' ";

                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "category");
                gridCategoryData.DataSource = ds;
                gridCategoryData.DataMember = "category";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { connection.Close(); }
        }
    }
}
