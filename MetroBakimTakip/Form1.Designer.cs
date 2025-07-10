namespace MetroBakimTakip
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblStationName = new System.Windows.Forms.Label();
            this.txtStationName = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblSection1 = new System.Windows.Forms.Label();
            this.lblSection2 = new System.Windows.Forms.Label();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.textsearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnPredict = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStationName
            // 
            this.lblStationName.AutoSize = true;
            this.lblStationName.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblStationName.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblStationName.Location = new System.Drawing.Point(12, 217);
            this.lblStationName.Name = "lblStationName";
            this.lblStationName.Size = new System.Drawing.Size(125, 19);
            this.lblStationName.TabIndex = 23;
            this.lblStationName.Text = "İstasyon Adı     :";
            // 
            // txtStationName
            // 
            this.txtStationName.Location = new System.Drawing.Point(154, 217);
            this.txtStationName.Name = "txtStationName";
            this.txtStationName.Size = new System.Drawing.Size(213, 22);
            this.txtStationName.TabIndex = 22;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(12, 271);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(119, 19);
            this.lblTitle.TabIndex = 21;
            this.lblTitle.Text = "Arıza Başlığı    :";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(154, 274);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(213, 22);
            this.txtTitle.TabIndex = 20;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblDescription.Location = new System.Drawing.Point(12, 323);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(118, 19);
            this.lblDescription.TabIndex = 19;
            this.lblDescription.Text = "Açıklama         :";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(154, 323);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(213, 22);
            this.txtDescription.TabIndex = 18;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(154, 369);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(213, 22);
            this.dtpDate.TabIndex = 17;
            // 
            // dtpTime
            // 
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTime.Location = new System.Drawing.Point(154, 417);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(213, 22);
            this.dtpTime.TabIndex = 16;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblDate.Location = new System.Drawing.Point(12, 372);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(99, 19);
            this.lblDate.TabIndex = 15;
            this.lblDate.Text = "Tarih            :";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTime.Location = new System.Drawing.Point(12, 417);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(98, 19);
            this.lblTime.TabIndex = 14;
            this.lblTime.Text = "Saat             :";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnSave.Location = new System.Drawing.Point(56, 475);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(134, 36);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvRecords
            // 
            this.dgvRecords.BackgroundColor = System.Drawing.Color.Silver;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Location = new System.Drawing.Point(684, 312);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.RowHeadersWidth = 51;
            this.dgvRecords.RowTemplate.Height = 24;
            this.dgvRecords.Size = new System.Drawing.Size(528, 260);
            this.dgvRecords.TabIndex = 12;
            this.dgvRecords.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecords_CellClick);
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(715, 214);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(224, 22);
            this.dtpStart.TabIndex = 11;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(715, 262);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(224, 22);
            this.dtpEnd.TabIndex = 10;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblStart.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblStart.Location = new System.Drawing.Point(544, 217);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(134, 19);
            this.lblStart.TabIndex = 9;
            this.lblStart.Text = "Başlangıç Tarihi :";
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblEnd.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblEnd.Location = new System.Drawing.Point(544, 262);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(114, 19);
            this.lblEnd.TabIndex = 8;
            this.lblEnd.Text = "Bitiş Tarihi      :";
            // 
            // btnFilter
            // 
            this.btnFilter.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnFilter.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnFilter.Location = new System.Drawing.Point(535, 417);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(123, 30);
            this.btnFilter.TabIndex = 7;
            this.btnFilter.Text = "Filtrele";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(535, 312);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 30);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Sil";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnBackup.ForeColor = System.Drawing.Color.Red;
            this.btnBackup.Location = new System.Drawing.Point(532, 365);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(126, 33);
            this.btnBackup.TabIndex = 5;
            this.btnBackup.Text = "Yedekle";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.yedekleme_button_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft YaHei", 16.2F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblHeader.Location = new System.Drawing.Point(508, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(397, 37);
            this.lblHeader.TabIndex = 4;
            this.lblHeader.Text = "Metro Bakım Takip Sistemi";
            // 
            // lblSection1
            // 
            this.lblSection1.AutoSize = true;
            this.lblSection1.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblSection1.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSection1.Location = new System.Drawing.Point(81, 124);
            this.lblSection1.Name = "lblSection1";
            this.lblSection1.Size = new System.Drawing.Size(195, 24);
            this.lblSection1.TabIndex = 3;
            this.lblSection1.Text = "Bakım Kayıt Yönetimi";
            // 
            // lblSection2
            // 
            this.lblSection2.AutoSize = true;
            this.lblSection2.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblSection2.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSection2.Location = new System.Drawing.Point(620, 124);
            this.lblSection2.Name = "lblSection2";
            this.lblSection2.Size = new System.Drawing.Size(198, 24);
            this.lblSection2.TabIndex = 2;
            this.lblSection2.Text = "Sistem Kayıt Yönetimi";
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnExportPDF.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnExportPDF.Location = new System.Drawing.Point(535, 465);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(119, 31);
            this.btnExportPDF.TabIndex = 1;
            this.btnExportPDF.Text = "PDF";
            this.btnExportPDF.UseVisualStyleBackColor = true;
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // lblTotalRecords
            // 
            this.lblTotalRecords.AutoSize = true;
            this.lblTotalRecords.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblTotalRecords.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTotalRecords.Location = new System.Drawing.Point(680, 600);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(144, 24);
            this.lblTotalRecords.TabIndex = 0;
            this.lblTotalRecords.Text = "Toplam Kayıt: 0";
            // 
            // textsearch
            // 
            this.textsearch.Location = new System.Drawing.Point(1111, 126);
            this.textsearch.Name = "textsearch";
            this.textsearch.Size = new System.Drawing.Size(213, 22);
            this.textsearch.TabIndex = 24;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSearch.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSearch.Location = new System.Drawing.Point(954, 124);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(140, 24);
            this.lblSearch.TabIndex = 25;
            this.lblSearch.Text = "Ara (İstasyon) :";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnExportExcel.Location = new System.Drawing.Point(535, 517);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(123, 30);
            this.btnExportExcel.TabIndex = 26;
            this.btnExportExcel.Text = "Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnPredict
            // 
            this.btnPredict.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnPredict.Location = new System.Drawing.Point(56, 538);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(134, 34);
            this.btnPredict.TabIndex = 27;
            this.btnPredict.Text = "Öngörü";
            this.btnPredict.UseVisualStyleBackColor = true;
            // 
            // btnTrain
            // 
            this.btnTrain.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTrain.Location = new System.Drawing.Point(56, 602);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(134, 31);
            this.btnTrain.TabIndex = 28;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(1338, 718);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.btnPredict);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.textsearch);
            this.Controls.Add(this.lblTotalRecords);
            this.Controls.Add(this.btnExportPDF);
            this.Controls.Add(this.lblSection2);
            this.Controls.Add(this.lblSection1);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.dgvRecords);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtStationName);
            this.Controls.Add(this.lblStationName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ForeColor = System.Drawing.Color.DarkBlue;
            this.Name = "Form1";
            this.Text = "Metro Bakım Takip";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStationName;
        private System.Windows.Forms.TextBox txtStationName;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSection1;
        private System.Windows.Forms.Label lblSection2;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.TextBox textsearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.Button btnTrain;
    }
}
