using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dailyWO19Nisan
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();

            UlkeLoad();

            cmbCountry.Items.Insert(0, "~Seçiniz~");
            cmbCountry.SelectedIndex = 0;
        }

        private void UlkeLoad()
        {
            // Bağlantıyı aç
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sorgu = "SELECT DISTINCT Country FROM Customers ORDER BY Country";

                using (SqlCommand command = new SqlCommand(sorgu, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbCountry.Items.Add(reader["Country"].ToString());
                        }
                    }
                }
            }
        }

        private void DataLoad()
        {
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sorgu = "SELECT CustomerID, CompanyName, ContactName, City, Country FROM Customers";

                if (chckbSec.Checked)
                {
                    sorgu += " ORDER BY Country, City";
                }
                else
                {
                    if (!string.IsNullOrEmpty(cmbCountry.Text))
                    {
                        sorgu += " WHERE Country=@Country ORDER BY City";
                    }
                }

                using (SqlCommand command = new SqlCommand(sorgu, connection))
                {
                    if (!string.IsNullOrEmpty(cmbCountry.Text))
                    {
                        command.Parameters.AddWithValue("@Country", cmbCountry.Text);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dgwListe.DataSource = table;
                    }
                }
            }
        }

        private void chckbSec_CheckedChanged(object sender, EventArgs e)
        {
            DataLoad();
            
        }

        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataLoad();
        }
    }
}
