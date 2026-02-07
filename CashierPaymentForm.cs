using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketManagementSystem
{
    public partial class CashierPaymentForm : UserControl
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public static int getCustID;
        public CashierPaymentForm()
        {
            InitializeComponent();
            connect = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\user\\OneDrive - University of Kelaniya\\Documents\\SMarketSystem.mdf\";Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connect);
            connection.Open();
            ProductsDataGridView();
            OrdersDataGridView();
            displayTotalPrice();
            //getcategory();
            getChasier();
            connection.Close();
        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }
            ProductsDataGridView();
            OrdersDataGridView();
            displayTotalPrice();
            getChasier();
        }

        public bool emptyfields()
        {
            if (cmbCategory.Text == "" || cmbProdID.Text == "" || cmbquantity.Text == "" ||
                cmbchashiername.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }

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
        private void getChasier()
        {
            try
            {
                string selectData = "SELECT Id, username FROM [user] WHERE role = @role AND status = @status";
                SqlCommand cmd = new SqlCommand(selectData, connection);
                cmd.Parameters.AddWithValue("@role", "Cashier");
                cmd.Parameters.AddWithValue("@status", "Active");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbchashiername.DisplayMember = "username";
                cmbchashiername.ValueMember = "Id";
                cmbchashiername.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Cashier data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void ProductsDataGridView()
        {
            connection = new SqlConnection(connect);
            try
            {
                connection.Open();
                string sql = "SELECT * FROM products";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "products");

                gridMenuData.DataSource = ds;
                gridMenuData.DataMember = "products";
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

        public void OrdersDataGridView()
        {

            cashierOrderData allOrders = new cashierOrderData();

            List<cashierOrderData> listData = allOrders.orderListData();

            gridOrderData.DataSource = listData;

        }

        private float totalPrice = 0;
        private int idGen = 0;
        public void IDGenerator()
        {
            connection = new SqlConnection(connect);

            {
                try
                {
                    connection.Open();
                    string selectID = "SELECT MAX(SId) FROM sales";

                    using (SqlCommand cmd = new SqlCommand(selectID, connection))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            int temp = Convert.ToInt32(result);

                            if (temp == 0)
                            {
                                idGen = 1;
                            }
                            else
                            {
                                idGen = temp + 1;
                            }
                        }
                        else
                        {
                            idGen = 1;
                        }
                        getCustID = idGen;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to ID data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }

            }
        }


        public void displayTotalPrice()
        {
            IDGenerator();
            connection = new SqlConnection(connect);

            try
            {
                connection.Open();
                string selectData = "SELECT SUM(prod_price) FROM orders WHERE customer_id = @custID";

                using (SqlCommand cmd = new SqlCommand(selectData, connection))
                {
                    cmd.Parameters.AddWithValue("@custID", idGen);

                    object result = cmd.ExecuteScalar();

                    totalPrice = Convert.ToSingle(result);

                    lblOrderPrice.Text = totalPrice.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            IDGenerator();

            if (emptyfields())
            {
                MessageBox.Show("Please select the product first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection = new SqlConnection(connect);

                try
                {
                    connection.Open();

                    float getPrice = 0;
                    string selectOrder = "SELECT * FROM products WHERE prod_id = @prodID";

                    using (SqlCommand getOrder = new SqlCommand(selectOrder, connection))
                    {
                        getOrder.Parameters.AddWithValue("@prodID", cmbProdID.Text.Trim());

                        using (SqlDataReader reader = getOrder.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                object rawValue = reader["prod_price"];
                                if (rawValue != DBNull.Value)
                                {
                                    getPrice = Convert.ToSingle(rawValue);
                                }
                            }
                        }
                    }

                    string insertOrder = "INSERT INTO orders (customer_id, prod_id, prod_name, prod_category, prod_price, qty, cashier, order_date)" +
                        "VALUES(@customerID, @prodID, @prodName, @prodCategory, @prodPrice,@qty, @cashier, @orderDate)";

                    DateTime today = DateTime.Today;

                    using (SqlCommand cmd = new SqlCommand(insertOrder, connection))
                    {
                        cmd.Parameters.AddWithValue("@customerID", idGen);
                        cmd.Parameters.AddWithValue("@prodID", cmbProdID.Text.Trim());
                        cmd.Parameters.AddWithValue("@prodName", lblProdName.Text.Trim());
                        cmd.Parameters.AddWithValue("@prodCategory", cmbCategory.Text.Trim());

                        float totalPrice = (getPrice * (int)cmbquantity.Value);

                        cmd.Parameters.AddWithValue("@prodPrice", totalPrice);
                        cmd.Parameters.AddWithValue("@orderDate", today);
                        cmd.Parameters.AddWithValue("@qty", cmbquantity.Value);
                        cmd.Parameters.AddWithValue("@cashier", cmbchashiername.Text.Trim());

                        int result = cmd.ExecuteNonQuery();

                        if (result == 1)
                        {
                            MessageBox.Show("ordered Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            displayTotalPrice();
                            OrdersDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Try again!!!", "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed:" + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        private void cmbProdID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = cmbProdID.SelectedItem as string;
            connection = new SqlConnection(connect);

            try
            {
                connection.Open();

                if (selectedValue != null)
                {
                    try
                    {
                        string selectData = $"SELECT * FROM products WHERE prod_id = '{selectedValue}' AND prod_status = @status";

                        using (SqlCommand cmd = new SqlCommand(selectData, connection))
                        {
                            cmd.Parameters.AddWithValue("@status", "Available");
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string prodName = reader["prod_name"].ToString();
                                    string prodPrice = reader["prod_price"].ToString();

                                    lblProdName.Text = prodName;
                                    lblMenuPrice.Text = prodPrice;


                                }
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("Prod ID Error: " + exx, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            cmbProdID.SelectedIndex = -1;
            cmbProdID.Items.Clear();
            lblProdName.Text = "---";
            lblMenuPrice.Text = "0";
            string selectedValue = cmbCategory.SelectedItem as string;

            connection = new SqlConnection(connect);

            try
            {
                connection.Open();

                if (selectedValue != null)
                {
                    try
                    {
                        string selectData = $"SELECT * FROM products WHERE prod_category = '{selectedValue}' AND prod_status = @status";

                        using (SqlCommand cmd = new SqlCommand(selectData, connection))
                        {
                            cmd.Parameters.AddWithValue("@status", "Available");
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string value = reader["prod_id"].ToString();

                                    cmbProdID.Items.Add(value);
                                }
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("Error: " + exx, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        private void txtOrderAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    float getAmount = Convert.ToSingle(txtOrderAmount.Text);


                    float getChange = (getAmount - totalPrice);


                    if (getChange <= 0)
                    {
                        txtOrderAmount.Text = "";
                        lblOrderChange.Text = "0";
                    }
                    else
                    {
                        lblOrderChange.Text = getChange.ToString();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Invalid input. Please enter a valid number." + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtOrderAmount.Text = "";
                    lblOrderChange.Text = "0";
                }
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (txtOrderAmount.Text == "" || gridOrderData.Rows.Count < 0)
            {
                MessageBox.Show("Something went wrong", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure for paying?", "Confirmation Message"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {


                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connect))
                        {
                            connection.Open();

                            IDGenerator();


                            string insertData = "INSERT INTO sales (customer_id, total_price,amount, change, pay_date) " +
                                    "VALUES(@custID,@totalPrice, @amount, @change, @date)";

                            DateTime today = DateTime.Today;

                            using (SqlCommand cmd = new SqlCommand(insertData, connection))
                            {
                                cmd.Parameters.AddWithValue("@custID", idGen);
                                cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
                                cmd.Parameters.AddWithValue("@date", today);
                                cmd.Parameters.AddWithValue("@amount", float.Parse(txtOrderAmount.Text.Trim()));
                                cmd.Parameters.AddWithValue("@change", float.Parse(lblOrderChange.Text.Trim()));


                                int result = cmd.ExecuteNonQuery();

                                if (result == 1)
                                {
                                    MessageBox.Show("paid Successfuly!!! ", "Information Massage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Try again!!!", "Error Massage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }


                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Connection failed" + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
            displayTotalPrice();
        }

        private int rowIndex = 0;
        private void btnReceipt_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.BeginPrint += new PrintEventHandler(printDocument1_BeginPrint);

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            rowIndex = 0;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            displayTotalPrice();

            float y = 0;
            int count = 0;
            int colWidth = 115;
            int headerMargin = 10;
            int tableMargin = 20;

            Font font = new Font("Arial", 12);
            Font bold = new Font("Arial", 14, FontStyle.Bold);
            Font headerFont = new Font("Arial", 18, FontStyle.Bold);
            Font labelFont = new Font("Arial", 16, FontStyle.Bold);

            float margin = e.MarginBounds.Top;

            StringFormat alignCenter = new StringFormat();
            alignCenter.Alignment = StringAlignment.Center;
            alignCenter.LineAlignment = StringAlignment.Center;

            string headerText = "\nHANNA SUPER MARKET\n\n\n";
            y = (margin + count * headerFont.GetHeight(e.Graphics) + headerMargin);
            e.Graphics.DrawString(headerText, headerFont, Brushes.Black, e.MarginBounds.Left
                + (gridOrderData.Columns.Count / 2) * colWidth, y, alignCenter);

            count++;
            y += tableMargin;

            string[] header = { "CID", "ProdID", "ProdName", "ProdType", "Price", "Qty", "Cashier" };

            for (int i = 0; i < header.Length; i++)
            {
                y = margin + count * bold.GetHeight(e.Graphics) + tableMargin;
                e.Graphics.DrawString(header[i], bold, Brushes.Black, e.MarginBounds.Left + i * colWidth, y, alignCenter);
            }
            count++;

            float rSpace = e.MarginBounds.Bottom - y;

            while (rowIndex < gridOrderData.Rows.Count)
            {
                DataGridViewRow row = gridOrderData.Rows[rowIndex];

                for (int i = 0; i < gridOrderData.Columns.Count; i++)
                {
                    object cellValue = row.Cells[i].Value;
                    string cell = (cellValue != null) ? cellValue.ToString() : string.Empty;

                    y = margin + count * font.GetHeight(e.Graphics) + tableMargin;
                    e.Graphics.DrawString(cell, font, Brushes.Black, e.MarginBounds.Left + i * colWidth, y, alignCenter);

                }
                count++;
                rowIndex++;

                if (y + font.GetHeight(e.Graphics) > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            int labelMargin = (int)Math.Min(rSpace, 90);

            DateTime today = DateTime.Now;

            float labelX = e.MarginBounds.Right - e.Graphics.MeasureString("--------------------------", labelFont).Width;

            y = e.MarginBounds.Bottom - labelMargin - labelFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Total Price: \tRs" + totalPrice + "\nAmount: \tRs"
                + txtOrderAmount.Text + "\n\t\t------------\nChange: \tRs" + lblOrderChange.Text, labelFont, Brushes.Black, labelX, y);

            labelMargin = (int)Math.Min(rSpace, -40);

            string labelText = today.ToString();
            y = e.MarginBounds.Bottom - labelMargin - labelFont.GetHeight(e.Graphics);
            e.Graphics.DrawString(labelText, labelFont, Brushes.Black
                , e.MarginBounds.Right - e.Graphics.MeasureString("--------------------------", labelFont).Width, y);
        }

        public void clearFields()
        {
            cmbCategory.SelectedIndex = -1;
            lblProdName.Text = "";
            lblMenuPrice.Text = "0";
            cmbProdID.SelectedIndex = -1;
            lblOrderPrice.Text = "0";
            txtOrderAmount.Text = "";
            lblOrderChange.Text = "0";
            cmbquantity.Value = 0;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearFields();
        }
    }
}
