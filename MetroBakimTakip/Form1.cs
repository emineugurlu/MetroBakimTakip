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
            Dictionary<string, int> riskDict = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                // Tüm kayıtları çek
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(
                    "SELECT Id,StationName,Title,Description,Date,Time FROM Faults", conn))
                {
                    da.Fill(dt);
                }
                // 7 günlük arıza sayısını hesapla
                using (SQLiteCommand cmd = new SQLiteCommand(
                    "SELECT StationName, COUNT(*) AS C FROM Faults " +
                    "WHERE Date >= date('now','-7 days') GROUP BY StationName", conn))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string st = rdr.GetString(0);
                            int c = Convert.ToInt32(rdr["C"]);
                            riskDict[st] = c;
                        }
                    }
                }
            }

            // RiskScore sütunu
            if (!dt.Columns.Contains("RiskScore"))
            {
                dt.Columns.Add("RiskScore", typeof(int));
            }
            foreach (DataRow row in dt.Rows)
            {
                string st = row["StationName"].ToString();
                row["RiskScore"] = riskDict.ContainsKey(st) ? riskDict[st] : 0;
            }

            // Arama filtresi
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

        // --- CRUD ---
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
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
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Faults WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Kayıt silindi.");
            LoadRecords();
        }

        // --- Tarih filtresi ---
        private void btnFilter_Click(object sender, EventArgs e)
        {
            string s = dtpStart.Value.ToString("yyyy-MM-dd");
            string e2 = dtpEnd.Value.ToString("yyyy-MM-dd");
            DataTable dt2 = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(
                    "SELECT Id,StationName,Title,Description,Date,Time FROM Faults " +
                    "WHERE Date BETWEEN @s AND @e", conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@s", s);
                    da.SelectCommand.Parameters.AddWithValue("@e", e2);
                    da.Fill(dt2);
                }
            }
            dgvRecords.DataSource = dt2;
            lblTotalRecords.Text = "Toplam Kayıt: " + dt2.Rows.Count;
        }

        // --- Yedekleme ---
        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy("metro.db", "metro_backup.db", true);
                MessageBox.Show("Yedek alındı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        // --- Grid satır tıkla ---
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

        // --- PDF Export ---
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            ExportToPDF(dtpStart.Value, dtpEnd.Value);
        }
        private void ExportToPDF(DateTime s, DateTime e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF Dosyası|*.pdf";
            sfd.FileName = "faults.pdf";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            PdfWriter writer = new PdfWriter(sfd.FileName);
            PdfDocument pdf = new PdfDocument(writer);
            Document doc = new Document(pdf);
            doc.Add(new Paragraph("Metro Bakım Takip\n\n")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            Table tbl = new Table(5);
            tbl.AddHeaderCell("İstasyon");
            tbl.AddHeaderCell("Başlık");
            tbl.AddHeaderCell("Açıklama");
            tbl.AddHeaderCell("Tarih");
            tbl.AddHeaderCell("Saat");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT StationName,Title,Description,Date,Time FROM Faults " +
                        "WHERE Date BETWEEN @s AND @e";
                    cmd.Parameters.AddWithValue("@s", s.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@e", e.ToString("yyyy-MM-dd"));

                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
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
            MessageBox.Show("PDF oluşturuldu.");
        }

        // --- CSV Export ---
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dtX = dgvRecords.DataSource as DataTable;
            if (dtX == null || dtX.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak kayıt yok.");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV Dosyası|*.csv";
            sfd.FileName = "faults.csv";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                // header
                sw.WriteLine(string.Join(",", dtX.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));
                foreach (DataRow row in dtX.Rows)
                {
                    string line = string.Join(",",
                        row.ItemArray.Select(f =>
                        {
                            string t = f.ToString();
                            if (t.Contains(",") || t.Contains("\""))
                                t = "\"" + t.Replace("\"", "\"\"") + "\"";
                            return t;
                        }));
                    sw.WriteLine(line);
                }
            }
            MessageBox.Show("CSV oluşturuldu.");
        }

        // --- Eğitim Verisi Üret ---
        private void btnExportTrainData_Click(object sender, EventArgs e)
        {
            string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "faults_train.csv");
            ExportTrainingData(csvPath);
        }
        private void ExportTrainingData(string path)
        {
            List<string> lines = new List<string>();
            lines.Add("FaultCountLast7Days,DayOfWeek,HourOfDay,Label");

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                DataTable dtT = new DataTable();
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(
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
                    SQLiteCommand cmdCnt = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Faults WHERE StationName=@st AND Date BETWEEN @s AND @e", conn);
                    cmdCnt.Parameters.AddWithValue("@st", st);
                    cmdCnt.Parameters.AddWithValue("@s", since);
                    cmdCnt.Parameters.AddWithValue("@e", d.ToString("yyyy-MM-dd"));
                    int cnt7 = Convert.ToInt32(cmdCnt.ExecuteScalar());

                    string tmr = d.AddDays(1).ToString("yyyy-MM-dd");
                    SQLiteCommand cmdTm = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Faults WHERE StationName=@st AND Date=@dt", conn);
                    cmdTm.Parameters.AddWithValue("@st", st);
                    cmdTm.Parameters.AddWithValue("@dt", tmr);
                    bool will = Convert.ToInt32(cmdTm.ExecuteScalar()) > 0;

                    int dow = (int)d.DayOfWeek + 1;
                    lines.Add(string.Format("{0},{1},{2},{3}", cnt7, dow, hr, will ? "1" : "0"));
                }
            }

            File.WriteAllLines(path, lines, Encoding.UTF8);
            MessageBox.Show("Eğitim verisi oluşturuldu:\n" + path);
        }

        // --- Model Eğitimi ---
        private async void btnTrain_Click(object sender, EventArgs e)
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string csvPath = Path.Combine(exeDir, "faults_train.csv");
            if (!File.Exists(csvPath))
                ExportTrainingData(csvPath);

            // UI’ı bloke etmemek için arka planda çalıştır
            this.btnTrain.Text = "Eğitiliyor...";
            this.btnTrain.Enabled = false;

            await Task.Run(() =>
            {
                var mlContext = new MLContext(seed: 0);
                IDataView data = mlContext.Data.LoadFromTextFile<FaultData>(
                    path: csvPath, hasHeader: true, separatorChar: ',');

                var pipeline = mlContext.Transforms
                    .Concatenate("Features",
                                 nameof(FaultData.FaultCountLast7Days),
                                 nameof(FaultData.DayOfWeek),
                                 nameof(FaultData.HourOfDay))
                    .Append(mlContext.BinaryClassification.Trainers
                        .SdcaLogisticRegression(labelColumnName: "Label"));

                ITransformer model = pipeline.Fit(data);
                using (FileStream fs = new FileStream(ModelFileName, FileMode.Create, FileAccess.Write))
                    mlContext.Model.Save(model, data.Schema, fs);
            });

            MessageBox.Show("Model eğitildi ve kaydedildi:\n" + ModelFileName);
            this.btnTrain.Text = "Train";
            this.btnTrain.Enabled = true;
        }

        // --- Tahmin ---
        private void btnPredict_Click(object sender, EventArgs e)
        {
            // 7-günlük arıza sayısı
            int cnt7;
            string station = txtStationName.Text;
            string today = dtpDate.Value.ToString("yyyy-MM-dd");
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(
                    "SELECT COUNT(*) FROM Faults WHERE StationName=@st AND Date BETWEEN date(@d,'-7 days') AND @d", conn))
                {
                    cmd.Parameters.AddWithValue("@st", station);
                    cmd.Parameters.AddWithValue("@d", today);
                    cnt7 = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            FaultData sample = new FaultData
            {
                FaultCountLast7Days = cnt7,
                DayOfWeek = (float)((int)dtpDate.Value.DayOfWeek + 1),
                HourOfDay = (float)dtpTime.Value.Hour
            };

            var mlContext = new MLContext();
            ITransformer model;
            DataViewSchema schema;
            using (FileStream fs = new FileStream(ModelFileName, FileMode.Open, FileAccess.Read))
                model = mlContext.Model.Load(fs, out schema);

            var engine = mlContext.Model.CreatePredictionEngine<FaultData, FaultPrediction>(model);
            FaultPrediction pred = engine.Predict(sample);

            string msg = pred.PredictedFailure
    ? string.Format("⚠️ Arıza bekleniyor (%{0:P1})", pred.Probability)
    : string.Format("✅ Arıza beklenmiyor (%{0:P1})", pred.Probability);

            MessageBox.Show(msg, "Öngörü Sonucu");
        }
    }
}
