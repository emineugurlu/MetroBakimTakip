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
using System.IO;  // File.Copy kullanabilmek için

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

        // DataGridView'e tıklanınca veriyi TextBox'lara aktar
        private void dgvRecords_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Seçilen satırı al
                DataGridViewRow row = dgvRecords.Rows[e.RowIndex];

                // TextBox'lara verileri aktar
                txtStationName.Text = row.Cells["stationName"].Value.ToString();
                txtTitle.Text = row.Cells["title"].Value.ToString();
                txtDescription.Text = row.Cells["description"].Value.ToString();
                dtpDate.Value = Convert.ToDateTime(row.Cells["date"].Value.ToString());
                dtpTime.Value = Convert.ToDateTime(row.Cells["time"].Value.ToString());
            }
        }

        // Kayıt güncelleme
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Seçilen kaydın ID'sini al
            int id = Convert.ToInt32(dgvRecords.SelectedRows[0].Cells["Id"].Value);

            string stationName = txtStationName.Text;
            string title = txtTitle.Text;
            string description = txtDescription.Text;
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string time = dtpTime.Value.ToString("HH:mm:ss");

            string connectionString = "Data Source=metro.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Güncelleme sorgusu
                string sql = "UPDATE Faults SET StationName = @stationName, Title = @title, Description = @description, Date = @date, Time = @time WHERE Id = @id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@stationName", stationName);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@time", time);
                    command.Parameters.AddWithValue("@id", id); // Güncellenen ID'yi kullan

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            MessageBox.Show("Kayıt başarıyla güncellendi!");
            LoadRecords(); // Güncellenmiş verileri tabloya yansıtalım
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

        // Kayıt silme
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
                LoadRecords(); // Tabloyu güncelle
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir kayıt seçin.");
            }
        }

        // Yedekleme işlemi
        private void BackupDatabase()
        {
            try
            {
                string sourceFile = "metro.db"; // Veritabanı dosyasının yolu
                string backupFile = "metro_backup.db"; // Yedek dosyanın adı

                // Yedekleme işlemi
                File.Copy(sourceFile, backupFile, true);
                MessageBox.Show("Veritabanı yedeği başarıyla alındı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yedekleme işlemi başarısız oldu: " + ex.Message);
            }
        }

        // Yedekle butonuna tıklandığında


        private void yedekleme_button_Click(object sender, EventArgs e)
        {
            BackupDatabase();  // Yedekleme işlemini başlat
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
