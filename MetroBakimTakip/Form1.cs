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
        }

    }
}
