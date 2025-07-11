using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.FastTree;
namespace MetroBakimTakip
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Data Source=metro.db;Version=3;";
        private const string ModelFileName = "faultModel.zip";

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            this.textsearch.TextChanged += TxtSearch_TextChanged;
            this.btnSave.Click += btnSave_Click;
            this.btnDelete.Click += btnDelete_Click;
            this.btnFilter.Click += btnFilter_Click;
            this.btnBackup.Click += btnBackup_Click;
            this.btnExportPDF.Click += btnExportPDF_Click;
            this.btnExportExcel.Click += btnExportExcel_Click;
            this.btnExportTrainData.Click += btnExportTrainData_Click;
            this.btnTrain.Click += btnTrain_Click;
            this.btnPredict.Click += btnPredict_Click;
            this.dgvRecords.CellClick += dgvRecords_CellClick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            var riskDict = new Dictionary<string, int>();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                // 1) Tablodaki tüm kayıtları getir
                using (var da = new SQLiteDataAdapter(
                    "SELECT Id,StationName,Title,Description,Date,Time FROM Faults", conn))
                {
                    da.Fill(dt);
                }

                // 2) Son 7 gündeki arıza sayısını hesapla
                using (var cmd = new SQLiteCommand(
                    "SELECT StationName, COUNT(*) AS C FROM Faults " +
                    "WHERE Date>=date('now','-7 days') GROUP BY StationName", conn))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        string station = rdr.GetString(0);
                        int count = Convert.ToInt32(rdr["C"]);
                        riskDict[station] = count;
                    }
                }
            }

            // 3) DataTable'a RiskScore sütunu ekle
            if (!dt.Columns.Contains("RiskScore"))
                dt.Columns.Add("RiskScore", typeof(int));

            foreach (DataRow row in dt.Rows)
            {
                string station = row["StationName"].ToString();
                row["RiskScore"] = riskDict.ContainsKey(station)
                    ? riskDict[station]
                    : 0;
            }

            // 4) Arama filtresi uygula
            if (!string.IsNullOrWhiteSpace(this.textsearch.Text))
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = "StationName LIKE '%" +
                    this.textsearch.Text.Replace("'", "''") + "%'";
                this.dgvRecords.DataSource = dv.ToTable();
                this.lblTotalRecords.Text = "Toplam Kayıt: " + dv.Count;
            }
            else
            {
                this.dgvRecords.DataSource = dt;
                this.lblTotalRecords.Text = "Toplam Kayıt: " + dt.Rows.Count;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        // — CRUD —

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO Faults(StationName,Title,Description,Date,Time) " +
                        "VALUES(@st,@ti,@de,@da,@tm)";
                    cmd.Parameters.AddWithValue("@st", txtStationName.Text);
                    cmd.Parameters.AddWithValue("@ti", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@de", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@da", dtpDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@tm", dtpTime.Value.ToString("HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt eklendi.");
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
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Faults WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt silindi.");
            LoadRecords();
        }

        // — Tarih filtreleme —

        private void btnFilter_Click(object sender, EventArgs e)
        {
            string s = dtpStart.Value.ToString("yyyy-MM-dd");
            string e2 = dtpEnd.Value.ToString("yyyy-MM-dd");
            DataTable dt2 = new DataTable();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var da = new SQLiteDataAdapter(
                    "SELECT Id,StationName,Title,Description,Date,Time FROM Faults WHERE Date BETWEEN @s AND @e",
                    conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@s", s);
                    da.SelectCommand.Parameters.AddWithValue("@e", e2);
                    da.Fill(dt2);
                }
            }
            dgvRecords.DataSource = dt2;
            lblTotalRecords.Text = "Toplam Kayıt: " + dt2.Rows.Count;
        }

        // — Yedekleme —

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy("metro.db", "metro_backup.db", true);
                MessageBox.Show("Yedek alındı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yedekleme hatası: " + ex.Message);
            }
        }

        // — Grid Satır Tıklama —

        private void dgvRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow r = dgvRecords.Rows[e.RowIndex];
            txtStationName.Text = r.Cells["StationName"].Value.ToString();
            txtTitle.Text = r.Cells["Title"].Value.ToString();
            txtDescription.Text = r.Cells["Description"].Value.ToString();
            dtpDate.Value = DateTime.Parse(r.Cells["Date"].Value.ToString());
            dtpTime.Value = DateTime.Parse(r.Cells["Time"].Value.ToString());
        }

        // — PDF Export —

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            ExportToPDF(dtpStart.Value, dtpEnd.Value);
        }
        private void ExportToPDF(DateTime s, DateTime e)
        {
            using (var sfd = new SaveFileDialog { Filter = "PDF Dosyası|*.pdf", FileName = "faults.pdf" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var writer = new PdfWriter(sfd.FileName))
                using (var pdf = new PdfDocument(writer))
                {
                    var doc = new Document(pdf);
                    doc.Add(new Paragraph("Metro Bakım Takip\n\n")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    var tbl = new Table(5);
                    tbl.AddHeaderCell("İstasyon");
                    tbl.AddHeaderCell("Başlık");
                    tbl.AddHeaderCell("Açıklama");
                    tbl.AddHeaderCell("Tarih");
                    tbl.AddHeaderCell("Saat");

                    using (var conn = new SQLiteConnection(ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText =
                                "SELECT StationName,Title,Description,Date,Time FROM Faults WHERE Date BETWEEN @s AND @e";
                            cmd.Parameters.AddWithValue("@s", s.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@e", e.ToString("yyyy-MM-dd"));
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    tbl.AddCell(rdr.GetString(0));
                                    tbl.AddCell(rdr.GetString(1));
                                    tbl.AddCell(rdr.GetString(2));
                                    tbl.AddCell(rdr.GetString(3));
                                    tbl.AddCell(rdr.GetString(4));
                                }
                            }
                        }
                    }

                    doc.Add(tbl);
                    doc.Close();
                }
                MessageBox.Show("PDF oluşturuldu.");
            }
        }

        // — CSV Export —

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dtX = dgvRecords.DataSource as DataTable;
            if (dtX == null || dtX.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak kayıt yok.");
                return;
            }
            using (var sfd = new SaveFileDialog { Filter = "CSV Dosyası|*.csv", FileName = "faults.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    sw.WriteLine(string.Join(",", dtX.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));
                    foreach (DataRow row in dtX.Rows)
                    {
                        string line = string.Join(",", row.ItemArray.Select(f =>
                        {
                            string t = f.ToString();
                            return (t.Contains(",") || t.Contains("\""))
                                ? ("\"" + t.Replace("\"", "\"\"") + "\"")
                                : t;
                        }));
                        sw.WriteLine(line);
                    }
                }
            }
            MessageBox.Show("CSV oluşturuldu.");
        }

        // — Eğitim Verisi Üret —

        private void btnExportTrainData_Click(object sender, EventArgs e)
        {
            string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "faults_train.csv");
            ExportTrainingData(csvPath);
        }
        private void ExportTrainingData(string path)
        {
            var lines = new List<string> { "FaultCountLast7Days,DayOfWeek,HourOfDay,Label" };
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var dtT = new DataTable();
                using (var da = new SQLiteDataAdapter(
                    "SELECT StationName,Date,Time FROM Faults ORDER BY StationName,Date,Time", conn))
                {
                    da.Fill(dtT);
                }
                foreach (DataRow r in dtT.Rows)
                {
                    string st = r["StationName"].ToString();
                    DateTime d = DateTime.Parse(r["Date"].ToString());
                    int hr = TimeSpan.Parse(r["Time"].ToString()).Hours;
                    string since = d.AddDays(-7).ToString("yyyy-MM-dd");
                    using (var cmdCnt = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Faults WHERE StationName=@st AND Date BETWEEN @s AND @e", conn))
                    {
                        cmdCnt.Parameters.AddWithValue("@st", st);
                        cmdCnt.Parameters.AddWithValue("@s", since);
                        cmdCnt.Parameters.AddWithValue("@e", d.ToString("yyyy-MM-dd"));
                        int cnt7 = Convert.ToInt32(cmdCnt.ExecuteScalar());
                        string tmr = d.AddDays(1).ToString("yyyy-MM-dd");
                        using (var cmdTm = new SQLiteCommand(
                            "SELECT COUNT(*) FROM Faults WHERE StationName=@st AND Date=@dt", conn))
                        {
                            cmdTm.Parameters.AddWithValue("@st", st);
                            cmdTm.Parameters.AddWithValue("@dt", tmr);
                            bool will = Convert.ToInt32(cmdTm.ExecuteScalar()) > 0;
                            int dow = (int)d.DayOfWeek + 1;
                            lines.Add($"{cnt7},{dow},{hr},{(will ? 1 : 0)}");
                        }
                    }
                }
            }
            File.WriteAllLines(path, lines, Encoding.UTF8);
            MessageBox.Show("Eğitim verisi oluşturuldu:\n" + path);
        }

        // — Model Eğitimi —

        private async void btnTrain_Click(object sender, EventArgs e)
            {
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var log = Path.Combine(exeDir, "train.log");
            File.WriteAllText(log, "--- Eğitim başladı ---\n");

            btnTrain.Text = "Eğitiliyor...";
            btnTrain.Enabled = false;
            try
            {
                await Task.Run(() =>
                {
                    File.AppendAllText(log, "1) MLContext oluşturuldu\n");

                    // CSV yolu aynı mı kontrol
                    string csvPath = Path.Combine(exeDir, "faults_train.csv");
                    File.AppendAllText(log, $"2) CSV yüklenecek: {csvPath}\n");

                    var mlContext = new MLContext(seed: 0);
                    var data = mlContext.Data.LoadFromTextFile<FaultData>(
                        path: csvPath, hasHeader: true, separatorChar: ',');

                    File.AppendAllText(log, "3) Pipeline oluşturuluyor\n");
                    var pipeline = mlContext.Transforms
                        .Concatenate("Features",
                                     nameof(FaultData.FaultCountLast7Days),
                                     nameof(FaultData.DayOfWeek),
                                     nameof(FaultData.HourOfDay))
                        .Append(mlContext.BinaryClassification.Trainers
                            .SdcaLogisticRegression(labelColumnName: "Label"));

                    File.AppendAllText(log, "4) Fit çağrısı\n");
                    ITransformer model = pipeline.Fit(data);
                    File.AppendAllText(log, "5) Fit tamamlandı\n");

                    string modelPath = Path.Combine(exeDir, ModelFileName);
                    mlContext.Model.Save(model, data.Schema, modelPath);
                    File.AppendAllText(log, $"6) Model kaydedildi: {modelPath}\n");
                });

                MessageBox.Show("✅ Eğitim tamamlandı");
            }
            catch (Exception ex)
            {
                File.AppendAllText(log, "‼️ Hata: " + ex + "\n");
                MessageBox.Show("Eğitim hatası: " + ex.Message);
            }
            finally
            {
                btnTrain.Text = "Train";
                btnTrain.Enabled = true;
                File.AppendAllText(log, "--- Bitti ---\n");
            }
        }

        // — Tahmin —

        private void btnPredict_Click(object sender, EventArgs e)
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string modelPath = Path.Combine(exeDir, ModelFileName);

            if (!File.Exists(modelPath))
            {
                MessageBox.Show("Model bulunamadı! Önce Train butonuna basın.");
                return;
            }

            // 7-günlük arıza sayısını al
            int cnt7;
            string station = txtStationName.Text;
            string today = dtpDate.Value.ToString("yyyy-MM-dd");
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(
                    "SELECT COUNT(*) FROM Faults WHERE StationName=@st AND Date BETWEEN date(@d,'-7 days') AND @d", conn))
                {
                    cmd.Parameters.AddWithValue("@st", station);
                    cmd.Parameters.AddWithValue("@d", today);
                    cnt7 = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            // Örnek veri
            var sample = new FaultData
            {
                FaultCountLast7Days = cnt7,
                DayOfWeek = (float)((int)dtpDate.Value.DayOfWeek + 1),
                HourOfDay = dtpTime.Value.Hour
            };

            // Modeli yükle ve tahmin et
            MLContext mlc = new MLContext();
            ITransformer model;
            DataViewSchema schema;
            using (var fs = new FileStream(modelPath, FileMode.Open, FileAccess.Read))
                model = mlc.Model.Load(fs, out schema);

            var engine = mlc.Model.CreatePredictionEngine<FaultData, FaultPrediction>(model);
            var pred = engine.Predict(sample);

            string msg = pred.PredictedLabel
                ? $"⚠️ Arıza bekleniyor (%{pred.Probability:P1})"
                : $"✅ Arıza beklenmiyor (%{pred.Probability:P1})";

            MessageBox.Show(msg, "Öngörü Sonucu");
        }
    }
}
