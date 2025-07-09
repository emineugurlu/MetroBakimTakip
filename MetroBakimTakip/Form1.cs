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
        // Veritabanı bağlantı dizesi
        private const string ConnectionString = "Data Source=metro.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            // Form yüklendiğinde kayıtları ve toplam sayıyı çek
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        // ---------------------------------------
        // Centralized LoadRecords(): 
        //  • DataGridView'e yükler 
        //  • lblTotalRecords'i günceller
        // ---------------------------------------
        private void LoadRecords()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                // 1) Tüm kayıtlari DataGridView'e çek
                string sql = "SELECT FaultID, StationName, Title, Description, Date, Time FROM Faults";
                var adapter = new SQLiteDataAdapter(sql, conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                dgvRecords.DataSource = dt;

                // 2) Toplam kayit sayisini al
                string countSql = "SELECT COUNT(*) FROM Faults";
                using (var cmd = new SQLiteCommand(countSql, conn))
                {
                    var total = Convert.ToInt32(cmd.ExecuteScalar());
                    lblTotalRecords.Text = $"Toplam Kayıt: {total}";
                }
            }
        }

        // -------------------------
        // CRUD & Filtreleme Olaylari
        // -------------------------

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string insert = @"
                    INSERT INTO Faults
                        (StationName, Title, Description, Date, Time)
                    VALUES
                        (@station, @title, @desc, @date, @time)";
                using (var cmd = new SQLiteCommand(insert, conn))
                {
                    cmd.Parameters.AddWithValue("@station", txtStationName.Text);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@desc", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@date", dtpDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@time", dtpTime.Value.ToString("HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt eklendi.");
            LoadRecords();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvRecords.SelectedRows.Count == 0)
            {
                MessageBox.Show("Güncellenecek kaydı seçin.");
                return;
            }

            int id = Convert.ToInt32(dgvRecords.SelectedRows[0].Cells["FaultID"].Value);
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string update = @"
                    UPDATE Faults SET
                        StationName = @station,
                        Title       = @title,
                        Description = @desc,
                        Date        = @date,
                        Time        = @time
                    WHERE FaultID = @id";
                using (var cmd = new SQLiteCommand(update, conn))
                {
                    cmd.Parameters.AddWithValue("@station", txtStationName.Text);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@desc", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@date", dtpDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@time", dtpTime.Value.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt güncellendi.");
            LoadRecords();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRecords.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silinecek kaydı seçin.");
                return;
            }

            int id = Convert.ToInt32(dgvRecords.SelectedRows[0].Cells["FaultID"].Value);
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string delete = "DELETE FROM Faults WHERE FaultID = @id";
                using (var cmd = new SQLiteCommand(delete, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt silindi.");
            LoadRecords();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var start = dtpStart.Value.Date.ToString("yyyy-MM-dd");
            var end = dtpEnd.Value.Date.ToString("yyyy-MM-dd");

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string filterSql = @"
                    SELECT FaultID, StationName, Title, Description, Date, Time
                    FROM Faults
                    WHERE Date BETWEEN @start AND @end";
                var adapter = new SQLiteDataAdapter(filterSql, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@start", start);
                adapter.SelectCommand.Parameters.AddWithValue("@end", end);

                var dt = new DataTable();
                adapter.Fill(dt);
                dgvRecords.DataSource = dt;

                // Filtre sonrası, DataTable.Rows.Count ile sayaci güncelle
                lblTotalRecords.Text = $"Toplam Kayıt: {dt.Rows.Count}";
            }
        }

        // -------------------------
        // DataGridView Satır Seçimi
        // -------------------------

        private void dgvRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvRecords.Rows[e.RowIndex];
            txtStationName.Text = row.Cells["StationName"].Value.ToString();
            txtTitle.Text = row.Cells["Title"].Value.ToString();
            txtDescription.Text = row.Cells["Description"].Value.ToString();
            dtpDate.Value = Convert.ToDateTime(row.Cells["Date"].Value);
            dtpTime.Value = Convert.ToDateTime(row.Cells["Time"].Value);
        }

        // -------------------------
        // Yedekleme (Backup) Butonu
        // -------------------------

        private void yedekleme_button_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy("metro.db", "metro_backup.db", true);
                MessageBox.Show("Yedek alındı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yedek başarısız: {ex.Message}");
            }
        }

        // -------------------------
        // PDF Dışa Aktarma
        // -------------------------

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            var start = dtpStart.Value.Date;
            var end = dtpEnd.Value.Date;
            ExportToPDF(start, end);
        }

        private void ExportToPDF(DateTime startDate, DateTime endDate)
        {
            using (var sfd = new SaveFileDialog() { Filter = "PDF Dosyası|*.pdf", FileName = "Ariza_Kayitlari.pdf" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var writer = new PdfWriter(sfd.FileName))
                using (var pdf = new PdfDocument(writer))
                {
                    var doc = new Document(pdf);
                    doc.Add(new Paragraph("Metro Bakım Takip Sistemi\n\n")
                                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    var table = new Table(5);
                    table.AddHeaderCell("İstasyon Adı");
                    table.AddHeaderCell("Arıza Başlığı");
                    table.AddHeaderCell("Açıklama");
                    table.AddHeaderCell("Tarih");
                    table.AddHeaderCell("Saat");

                    using (var conn = new SQLiteConnection(ConnectionString))
                    {
                        conn.Open();
                        string sql = @"
                            SELECT StationName, Title, Description, Date, Time
                            FROM Faults
                            WHERE Date BETWEEN @start AND @end";
                        using (var cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@start", startDate.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@end", endDate.ToString("yyyy-MM-dd"));
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    table.AddCell(reader.GetString(0));
                                    table.AddCell(reader.GetString(1));
                                    table.AddCell(reader.GetString(2));
                                    table.AddCell(reader.GetString(3));
                                    table.AddCell(reader.GetString(4));
                                }
                            }
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                }
                MessageBox.Show("PDF oluşturuldu.");
            }
        }
    }
}
