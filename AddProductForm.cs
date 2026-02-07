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
    public partial class AddProductForm : UserControl
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public AddProductForm()
        {
            InitializeComponent();
            connect = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\user\\OneDrive - University of Kelaniya\\Documents\\SMarketSystem.mdf\";Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connect);
            connection.Open();
            loadDataGridView();
            getcategory();
            connection.Close();
        }
        //ReFresh Code
        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }
            loadDataGridView();
            getcategory();
        }


        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, connection);
            sqlDataAdapter.Fill(dt);
            return dt;
        }

        private void getcategory()
        {
            string queary = "SELECT * FROM [category]";
            cmbCategory.DisplayMember = GetData(queary).Columns["categoryName"].ToString();
            cmbCategory.ValueMember = GetData(queary).Columns["categoryName"].ToString();
            cmbCategory.DataSource = GetData(queary);
        }


        /// Empty
        public bool emptyfields()
        {
            if (txtProductID.Text == "" || txtProductName.Text == "" || cmbCategory.Text == "" ||
                txtPrice.Text == "" || cmbStatus.Text == "" || txtStock.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Grid View data fill
        public void loadDataGridView()
        {
            connection = new SqlConnection(connect);
            try
            {
                connection.Open();
                string sql = "SELECT * FROM products";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "products");

                gridProductData.DataSource = ds;
                gridProductData.DataMember = "products";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Products data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        public void clearForm()
        {
            txtProductID.Clear();
            txtProductName.Clear();
            cmbCategory.Text = string.Empty;
            cmbCategory.SelectedIndex = 0;
            txtPrice.Clear();
            txtStock.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
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

                    string query = "INSERT INTO products VALUES(@id,@name,@category,@stock,@price,@status,@date)";
                    DateTime today = DateTime.Today;

                    cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@id", txtProductID.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@category", cmbCategory.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@stock", txtStock.Text.Trim());
                    cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtPrice.Text.Trim()));
                    cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@date", today);

                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Products Data Save Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private int id = 0;
        private void gridProductData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = gridProductData.Rows[e.RowIndex];
            id = Convert.ToInt32(row.Cells[0].Value);
            txtProductID.Text = row.Cells[1].Value.ToString();
            txtProductName.Text = row.Cells[2].Value.ToString();
            cmbCategory.Text = row.Cells[3].Value.ToString();
            txtStock.Text = row.Cells[4].Value.ToString();
            txtPrice.Text = row.Cells[5].Value.ToString();
            cmbStatus.Text = row.Cells[6].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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

                    string query = "UPDATE [products] SET prod_id = @pId, prod_name = @name, prod_category = @category, prod_stock=@stock, prod_price = @price,prod_status=@status,date_insert=@date WHERE Id = @id";
                    DateTime today = DateTime.Today;

                    cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@pId", txtProductID.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@category", cmbCategory.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@stock", txtStock.Text.Trim());
                    cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtPrice.Text.Trim()));
                    cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@date", today);
                    cmd.Parameters.AddWithValue("@id", id);

                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Products Data Updated Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
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

                    string query = "DELETE FROM [products] WHERE Id = @id";

                    cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@id", id);

                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Products Data Deleted Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }
    }
}
