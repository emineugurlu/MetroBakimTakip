using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
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
            this.Load += Form1_Load;

            // Yeni eklenecek kontrollerin olaylarını bağla
            this.textsearch.TextChanged += TxtSearch_TextChanged;
            this.btnExportExcel.Click += BtnExportExcel_Click;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        // ------------------------------------------------
        // Kayıtları VeriGridView'e yükler ve sayacı günceller
        // ------------------------------------------------
        private void LoadRecords()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                // 1) Tüm kayıtlar
                string sql = @"
                    SELECT 
                        Id, StationName, Title, Description, Date, Time 
                    FROM Faults";
                var adapter = new SQLiteDataAdapter(sql, conn);
                var dt = new DataTable();
                adapter.Fill(dt);

                // Eğer arama kutusunda metin varsa, hemen filtre uygula
                if (!string.IsNullOrEmpty(textsearch.Text))
                {
                    var dv = dt.DefaultView;
                    dv.RowFilter = $"StationName LIKE '%{textsearch.Text.Replace("'", "''")}%'";
                    dgvRecords.DataSource = dv.ToTable();
                }
                else
                {
                    dgvRecords.DataSource = dt;
                }

                // 2) Toplam kayıt sayısı (DataGridView’de görünen satır sayısı)
                int visibleCount = (dgvRecords.DataSource as DataTable)?.Rows.Count ?? 0;
                lblTotalRecords.Text = $"Toplam Kayıt: {visibleCount}";
            }
        }

        // -------------------------
        // Anlık Arama (TextChanged)
        // -------------------------
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Tekrar LoadRecords çağırıp hem filtre hem sayaçı güncelliyoruz
            LoadRecords();
        }

        // --------------- CRUD ----------------

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

            int id = Convert.ToInt32(dgvRecords.SelectedRows[0].Cells["Id"].Value);
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
                    WHERE Id = @id";
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

            int id = Convert.ToInt32(dgvRecords.SelectedRows[0].Cells["Id"].Value);
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string delete = "DELETE FROM Faults WHERE Id = @id";
                using (var cmd = new SQLiteCommand(delete, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt silindi.");
            LoadRecords();
        }

        // -------------- Filtreleme --------------

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Tarih filtresi LoadRecords üzerinden değil, kendimiz uygulayıp DataGridView'e basıyoruz
            string start = dtpStart.Value.ToString("yyyy-MM-dd");
            string end = dtpEnd.Value.ToString("yyyy-MM-dd");

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string filterSql = @"
                    SELECT 
                        Id, StationName, Title, Description, Date, Time
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

        // ----------- Satır seçimi -----------

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

        // ---------- Yedekleme ------------

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

        // ---------- PDF Dışa Aktar ----------

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            var start = dtpStart.Value;
            var end = dtpEnd.Value;
            ExportToPDF(start, end);
        }

        private void ExportToPDF(DateTime startDate, DateTime endDate)
        {
            using (var sfd = new SaveFileDialog { Filter = "PDF Dosyası|*.pdf", FileName = "Ariza_Kayitlari.pdf" })
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

        // -------------------------
        // CSV (Excel) Dışa Aktar
        // -------------------------
        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            // DataGridView altında bir CSV çıktısı almak için
            if (!(dgvRecords.DataSource is DataTable dt) || dt.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak kayıt bulunamadı.");
                return;
            }

            using (var sfd = new SaveFileDialog { Filter = "CSV Dosyası|*.csv", FileName = "Ariza_Kayitlari.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                try
                {
                    using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        // Başlıklar
                        sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

                        // Satırlar
                        foreach (DataRow row in dt.Rows)
                        {
                            sw.WriteLine(string.Join(",", row.ItemArray.Select(field =>
                                field.ToString().Contains(",")
                                    ? $"\"{field.ToString().Replace("\"", "\"\"")}\""
                                    : field.ToString()
                            )));
                        }
                    }
                    MessageBox.Show("CSV başarıyla oluşturuldu!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"CSV aktarımı başarısız: {ex.Message}");
                }
            }
        }

        private void btnExportExcel_Click_1(object sender, EventArgs e)
        {
            // DataGridView'in DataSource'u DataTable mı?
            if (!(dgvRecords.DataSource is DataTable dt) || dt.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak kayıt bulunamadı.");
                return;
            }

            using (var sfd = new SaveFileDialog { Filter = "CSV Dosyası|*.csv", FileName = "Ariza_Kayitlari.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        // 1) Başlık satırı
                        sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

                        // 2) Her bir satırı döndür
                        foreach (DataRow row in dt.Rows)
                        {
                            sw.WriteLine(string.Join(",", row.ItemArray.Select(field =>
                            {
                                var text = field.ToString();
                                // Virgül içeriyorsa tırnak içine al
                                if (text.Contains(",") || text.Contains("\""))
                                    text = $"\"{text.Replace("\"", "\"\"")}\"";
                                return text;
                            }
                            )));
                        }
                    }
                    MessageBox.Show("CSV başarıyla oluşturuldu!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"CSV aktarımı başarısız: {ex.Message}");
                }
            }
        }
    }
    
}
