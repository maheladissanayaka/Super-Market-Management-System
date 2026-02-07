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
    public partial class AdminMainForm : Form
    {
        public AdminMainForm()
        {
            InitializeComponent();
        }

        private void btnDashbord_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = true;
            addUserForm1.Visible = false;
            addProductForm1.Visible = false;
            totalCategoryForm1.Visible = false;

            DashbordForm adFrom = dashbordForm1 as DashbordForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void btnAddusers_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = false;
            addUserForm1.Visible = true;
            addProductForm1.Visible = false;
            totalCategoryForm1.Visible = false;

            DashbordForm adFrom = dashbordForm1 as DashbordForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = false;
            addUserForm1.Visible = false;
            addProductForm1.Visible = true;
            totalCategoryForm1.Visible = false;

            DashbordForm adFrom = dashbordForm1 as DashbordForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            dashbordForm1.Visible = false;
            addUserForm1.Visible = false;
            addProductForm1.Visible = false;
            totalCategoryForm1.Visible = true;

            DashbordForm adFrom = dashbordForm1 as DashbordForm;

            if (adFrom != null)
            {
                adFrom.refreshData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
