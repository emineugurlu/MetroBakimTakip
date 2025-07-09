using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
namespace MetroBakimTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Form yüklendiğinde verileri getir
      

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

        // PDF dışa aktarma işlemi
        // PDF dışa aktarma işlemi
        private void ExportToPDF(DateTime startDate, DateTime endDate)
        {
            string connectionString = "Data Source=metro.db;Version=3;";

            // SaveFileDialog ile dosya kaydetme yolu belirleme
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Dosyası|*.pdf";
            saveFileDialog.Title = "PDF Kaydet";
            saveFileDialog.FileName = "Ariza_Kayitlari.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = saveFileDialog.FileName;

                // PDF belgesini oluştur
                using (MemoryStream ms = new MemoryStream())
                {
                    // PdfWriter nesnesini oluştur
                    PdfWriter writer = new PdfWriter(savePath); // Dosya kaydetme yolunu belirle
                    PdfDocument pdf = new PdfDocument(writer); // PDF dosyasını oluştur
                    Document document = new Document(pdf); // Belgeyi oluştur

                    // Başlık
                    document.Add(new Paragraph("Metro Bakım Takip Sistemi\n\n")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    // Tablo başlıkları
                    Table table = new Table(5);
                    table.AddCell("İstasyon Adı");
                    table.AddCell("Arıza Başlığı");
                    table.AddCell("Açıklama");
                    table.AddCell("Tarih");
                    table.AddCell("Saat");

                    // Veritabanından verileri al
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT * FROM Faults WHERE Date BETWEEN @start AND @end";
                        SQLiteCommand command = new SQLiteCommand(query, connection);
                        command.Parameters.AddWithValue("@start", startDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@end", endDate.ToString("yyyy-MM-dd"));

                        SQLiteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            // Tabloya verileri ekle
                            table.AddCell(reader["StationName"].ToString());
                            table.AddCell(reader["Title"].ToString());
                            table.AddCell(reader["Description"].ToString());
                            table.AddCell(reader["Date"].ToString());
                            table.AddCell(reader["Time"].ToString());
                        }

                        connection.Close();
                    }

                    // Tabloyu PDF'ye ekle
                    document.Add(table);

                    // PDF'i tamamla ve kaydet
                    document.Close(); // **PDF'in doğru şekilde kaydedilmesi için Close() fonksiyonu gereklidir**
                }

                MessageBox.Show("PDF başarıyla oluşturuldu!");
            }
            else
            {
                MessageBox.Show("Dosya kaydetme işlemi iptal edildi.");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            DateTime startDate = dtpStart.Value;
            DateTime endDate = dtpEnd.Value;

            ExportToPDF(startDate, endDate); // PDF dışa aktarma işlemi
        }

        private void label7_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=metro.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // 1) Kayıtları al ve DataGridView'e yükle
                string query = "SELECT * FROM Faults";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvRecords.DataSource = dt;

                // 2) Toplam kayıt sayısını al
                string countQuery = "SELECT COUNT(*) FROM Faults";
                using (SQLiteCommand countCmd = new SQLiteCommand(countQuery, connection))
                {
                    int total = Convert.ToInt32(countCmd.ExecuteScalar());
                    lblTotalRecords.Text = $"Toplam Kayıt: {total}";
                }

                connection.Close();
            }
        }
    }
}
