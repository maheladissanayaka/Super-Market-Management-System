using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketManagementSystem
{
    internal class cashierOrderData
    {
        SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\user\\OneDrive - University of Kelaniya\\Documents\\SMarketSystem.mdf\";Integrated Security=True;Connect Timeout=30");

        public int CID { get; set; }
        public string ProdID { get; set; }
        public string ProdName { get; set; }
        public string ProdCategory { get; set; }
        public string Price { get; set; }
        public int Qty { get; set; }
        public String cashier { get; set; }

        public List<cashierOrderData> orderListData()
        {
            List<cashierOrderData> listData = new List<cashierOrderData>();

            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();

                    int custID = 0;
                    string selectCustData = "SELECT MAX(customer_id) FROM orders";

                    using (SqlCommand getCustData = new SqlCommand(selectCustData, connection))
                    {
                        object result = getCustData.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            int temp = Convert.ToInt32(result);

                            if (temp == 0)
                            {
                                custID = 1;
                            }
                            else
                            {
                                custID = temp;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error ID");
                        }
                    }


                    /////////////////////////////
                    string selectOrders = "SELECT * FROM orders WHERE customer_id = @customerID";

                    using (SqlCommand cmd = new SqlCommand(selectOrders, connection))
                    {
                        cmd.Parameters.AddWithValue("@customerID", custID);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            cashierOrderData coData = new cashierOrderData();

                            coData.CID = (int)reader["customer_id"];
                            coData.ProdID = reader["prod_id"].ToString();
                            coData.ProdName = reader["prod_name"].ToString();
                            coData.ProdCategory = reader["prod_category"].ToString();
                            coData.Price = reader["prod_price"].ToString();
                            coData.Qty = (int)reader["qty"];
                            coData.cashier = reader["cashier"].ToString();

                            listData.Add(coData);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection Failed" + ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return listData;

        }
    }
}
