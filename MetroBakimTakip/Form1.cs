using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace MetroBakimTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string stationName = txtStationName.Text;
            string title = txtTitle.Text;
            string description = txtDescription.Text;
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string time = dtpTime.Value.ToString("HH:mm:ss");

            string connectionString = "Data Source=metro.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Faults (StationName, Title, Description, Date, Time) " +
                             "VALUES (@stationName, @title, @description, @date, @time)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@stationName", stationName);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@time", time);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            MessageBox.Show("Kayıt başarıyla eklendi!");

            LoadRecords(); // Yeni eklenen veriyi tabloya yansıt
        }

        private void dgvRecords_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecords(); // Form açıldığında verileri yükle
        }

        private void LoadRecords()
        {
            string connectionString = "Data Source=metro.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Faults"; // Faults tablosu

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvRecords.DataSource = dt;

                connection.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=metro.db;Version=3;";
            string startDate = dtpStart.Value.ToString("yyyy-MM-dd");
            string endDate = dtpEnd.Value.ToString("yyyy-MM-dd");

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Faults WHERE Date BETWEEN @start AND @end";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@start", startDate);
                    command.Parameters.AddWithValue("@end", endDate);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvRecords.DataSource = table;
                }

                connection.Close();
            }
        }
    }
}
