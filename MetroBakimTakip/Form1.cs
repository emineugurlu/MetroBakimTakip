using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetroBakimTakip
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Data Source=metro.db;Version=3;";
        // ML model yolu (Model sınıfları Models.cs içinde)
        private readonly string _modelPath = "faultModel.zip";

        public Form1()
        {
            InitializeComponent();

            // Form Load
            this.Load += Form1_Load;

            // Koddan bağlamak istediğin event’ler (optional, Designer’da da olabilir)
            textsearch.TextChanged += TxtSearch_TextChanged;
            btnExportExcel.Click += BtnExportExcel_Click;
            btnPredict.Click += BtnPredict_Click;
            btnTrain.Click += BtnTrain_Click;    // eğer train butonu varsa
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        /// <summary>
        /// DataGridView’i doldurur, RiskScore hesaplar, sayaçı günceller
        /// </summary>
        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            var riskDict = new Dictionary<string, int>();

            // 1) DB’den çek
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (var ad = new SQLiteDataAdapter(
                    @"SELECT Id, StationName, Title, Description, Date, Time
                      FROM Faults", conn))
                {
                    ad.Fill(dt);
                }

                // 2) Son 7 gündeki arıza sayısını hesapla
                using (var cmd = new SQLiteCommand(
                    @"SELECT StationName, COUNT(*) AS C
                      FROM Faults
                      WHERE Date >= date('now','-7 days')
                      GROUP BY StationName", conn))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        riskDict[rdr.GetString(0)] = Convert.ToInt32(rdr["C"]);
                }
            }

            // 3) RiskScore sütunu ekle ve doldur
            if (!dt.Columns.Contains("RiskScore"))
                dt.Columns.Add("RiskScore", typeof(int));
            foreach (DataRow row in dt.Rows)
            {
                var st = row["StationName"].ToString();
                row["RiskScore"] = riskDict.ContainsKey(st) ? riskDict[st] : 0;
            }

            // 4) Arama filtresi
            if (!string.IsNullOrEmpty(textsearch.Text))
            {
                var dv = dt.DefaultView;
                dv.RowFilter = $"StationName LIKE '%{textsearch.Text.Replace("'", "''")}%'";
                dgvRecords.DataSource = dv.ToTable();
                lblTotalRecords.Text = $"Toplam Kayıt: {dv.Count}";
            }
            else
            {
                dgvRecords.DataSource = dt;
                lblTotalRecords.Text = $"Toplam Kayıt: {dt.Rows.Count}";
            }
        }

        // Anlık arama
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        // Kaydet
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(
                    @"INSERT INTO Faults
                      (StationName, Title, Description, Date, Time)
                      VALUES (@st,@ti,@de,@da,@ti2)", conn))
                {
                    cmd.Parameters.AddWithValue("@st", txtStationName.Text);
                    cmd.Parameters.AddWithValue("@ti", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@de", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@da", dtpDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ti2", dtpTime.Value.ToString("HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt eklendi.");
            LoadRecords();
        }

        // Güncelle
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
                using (var cmd = new SQLiteCommand(
                    @"UPDATE Faults SET
                        StationName = @st,
                        Title       = @ti,
                        Description = @de,
                        Date        = @da,
                        Time        = @ti2
                      WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@st", txtStationName.Text);
                    cmd.Parameters.AddWithValue("@ti", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@de", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@da", dtpDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ti2", dtpTime.Value.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt güncellendi.");
            LoadRecords();
        }

        // Sil
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
                using (var cmd = new SQLiteCommand("DELETE FROM Faults WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt silindi.");
            LoadRecords();
        }

        // Filtrele
        private void btnFilter_Click(object sender, EventArgs e)
        {
            string s = dtpStart.Value.ToString("yyyy-MM-dd");
            string e2 = dtpEnd.Value.ToString("yyyy-MM-dd");
            var dt = new DataTable();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var ad = new SQLiteDataAdapter(
                    @"SELECT Id, StationName, Title, Description, Date, Time
                      FROM Faults
                      WHERE Date BETWEEN @s AND @e", conn);
                ad.SelectCommand.Parameters.AddWithValue("@s", s);
                ad.SelectCommand.Parameters.AddWithValue("@e", e2);
                ad.Fill(dt);
            }
            dgvRecords.DataSource = dt;
            lblTotalRecords.Text = $"Toplam Kayıt: {dt.Rows.Count}";
        }

        // Yedekle
        private void yedekleme_button_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy("metro.db", "metro_backup.db", true);
                MessageBox.Show("Yedek alındı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yedekleme hatası: {ex.Message}");
            }
        }

        // Grid satırına tıklandığında formu doldur
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

        // PDF dışa aktar
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            var s = dtpStart.Value;
            var e2 = dtpEnd.Value;
            ExportToPDF(s, e2);
        }

        // CSV dışa aktar
        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (!(dgvRecords.DataSource is DataTable dt) || dt.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak kayıt yok.");
                return;
            }
            using (var sfd = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "faults.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    // başlık
                    sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));
                    // satırlar
                    foreach (var row in dt.Rows.Cast<DataRow>())
                    {
                        sw.WriteLine(string.Join(",", row.ItemArray.Select(f =>
                        {
                            var t = f.ToString();
                            return (t.Contains(",") || t.Contains("\""))
                                ? $"\"{t.Replace("\"", "\"\"")}\""
                                : t;
                        })));
                    }
                }
                MessageBox.Show("CSV oluşturuldu.");
            }
        }

        // ML.NET eğitim (eğer train butonunuz varsa)
        private void BtnTrain_Click(object sender, EventArgs e)
        {
            var ml = new MLContext(seed: 0);
            var data = ml.Data.LoadFromTextFile<FaultData>("faults_train.csv", hasHeader: true, separatorChar: ',');

            var pipeline = ml.Transforms.Concatenate("Features",
                                nameof(FaultData.FaultCountLast7Days),
                                nameof(FaultData.DayOfWeek),
                                nameof(FaultData.HourOfDay))
                           .Append(ml.BinaryClassification.Trainers.SdcaLogisticRegression());

            var model = pipeline.Fit(data);
            ml.Model.Save(model, data.Schema, _modelPath);
            MessageBox.Show("Model eğitildi ve kaydedildi.");
        }

        // Tahmin
        private void BtnPredict_Click(object sender, EventArgs e)
        {
            // seçim veya manuel değer
            var sample = new FaultData
            {
                FaultCountLast7Days = /* buraya 7-gün sayısını hesaplayın */
                DayOfWeek = (int)dtpDate.Value.DayOfWeek + 1,
                HourOfDay = dtpTime.Value.Hour
            };

            var ml = new MLContext();
            ITransformer model;
            DataViewSchema schema;
            using (var fs = new FileStream(_modelPath, FileMode.Open, FileAccess.Read))
                model = ml.Model.Load(fs, out schema);

            var engine = ml.Model.CreatePredictionEngine<FaultData, FaultPrediction>(model);
            var pred = engine.Predict(sample);

            MessageBox.Show(pred.PredictedFailure
                ? $"Arıza bekleniyor (%{pred.Probability:P1})"
                : $"Arıza beklenmiyor (%{pred.Probability:P1})");
        }

        // PDF helper
        private void ExportToPDF(DateTime start, DateTime end)
        {
            using (var sfd = new SaveFileDialog { Filter = "PDF|*.pdf", FileName = "faults.pdf" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var writer = new PdfWriter(sfd.FileName))
                using (var pdf = new PdfDocument(writer))
                {
                    var doc = new Document(pdf);
                    doc.Add(new Paragraph("Metro Bakım Takip\n\n")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    var table = new Table(5);
                    table.AddHeaderCell("İstasyon");
                    table.AddHeaderCell("Başlık");
                    table.AddHeaderCell("Açıklama");
                    table.AddHeaderCell("Tarih");
                    table.AddHeaderCell("Saat");

                    using (var conn = new SQLiteConnection(ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new SQLiteCommand(
                            @"SELECT StationName,Title,Description,Date,Time
                              FROM Faults
                              WHERE Date BETWEEN @s AND @e", conn))
                        {
                            cmd.Parameters.AddWithValue("@s", start.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@e", end.ToString("yyyy-MM-dd"));
                            using (var rdr = cmd.ExecuteReader())
                                while (rdr.Read())
                                {
                                    table.AddCell(rdr.GetString(0));
                                    table.AddCell(rdr.GetString(1));
                                    table.AddCell(rdr.GetString(2));
                                    table.AddCell(rdr.GetString(3));
                                    table.AddCell(rdr.GetString(4));
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
