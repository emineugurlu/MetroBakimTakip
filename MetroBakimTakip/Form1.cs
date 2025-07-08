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

        // Form yüklendiğinde verileri getir
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        // Kayıt ekleme
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

            LoadRecords(); // Yeni kayıt sonrası tabloyu güncelle
        }

        // Tüm kayıtları DataGridView'e yükle
        private void LoadRecords()
        {
            string connectionString = "Data Source=metro.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Faults";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvRecords.DataSource = dt;

                connection.Close();
            }
        }

        // Tarih aralığına göre filtreleme
        private void btnFilter_Click(object sender, EventArgs e)
        {
            string startDate = dtpStart.Value.ToString("yyyy-MM-dd");
            string endDate = dtpEnd.Value.ToString("yyyy-MM-dd");

            string connectionString = "Data Source=metro.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Faults WHERE Date BETWEEN @start AND @end";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@start", startDate);
                    command.Parameters.AddWithValue("@end", endDate);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvRecords.DataSource = dt;
                }

                connection.Close();
            }
        }

        // Olay işleyicileri (gerekliyse boş bırakabilirsin)
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void dgvRecords_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRecords.SelectedRows.Count > 0)
            {
                int selectedId = Convert.ToInt32(dgvRecords.SelectedRows[0].Cells["Id"].Value);

                string connectionString = "Data Source=metro.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Faults WHERE Id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedId);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }

                MessageBox.Show("Kayıt başarıyla silindi.");
                LoadRecords(); // tabloyu yenile
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir kayıt seçin.");
            }
        }
    }
}
