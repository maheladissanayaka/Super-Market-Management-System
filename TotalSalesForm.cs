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
    public partial class TotalSalesForm : UserControl
    {
        string connect = null;
        SqlConnection connection = null;
        SqlCommand cmd = null;
        public TotalSalesForm()
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
            connection = new SqlConnection(connect);
            try
            {
                connection.Open();
                string sql = "SELECT * FROM [sales]";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "sales");

                gridSalesData.DataSource = ds;
                gridSalesData.DataMember = "sales";
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
    }
}
