using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketManagementSystem
{
    public partial class UserMainForm : Form
    {
        public UserMainForm()
        {
            InitializeComponent();
        }

        private void btnDashbord_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = true;
            cashierPaymentForm1.Visible = false;
            addProductForm1.Visible = false;
            totalSalesForm1.Visible = false;

            DashbordForm adFrom = dashbordForm1 as DashbordForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = false;
            cashierPaymentForm1.Visible = true;
            addProductForm1.Visible = false;
            totalSalesForm1.Visible = false;

            DashbordForm adFrom = dashbordForm1 as DashbordForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = false;
            cashierPaymentForm1.Visible = false;
            addProductForm1.Visible = true;
            totalSalesForm1.Visible = false;

            DashbordForm adFrom = dashbordForm1 as DashbordForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void btnTotalSales_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = false;
            cashierPaymentForm1.Visible = false;
            addProductForm1.Visible = false;
            totalSalesForm1.Visible = true;

            TotalSalesForm adFrom = totalSalesForm1 as TotalSalesForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Are you sure you want to Logout?", "Infomation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                MainWindow f1 = new MainWindow();
                f1.Show();
                this.Hide();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
