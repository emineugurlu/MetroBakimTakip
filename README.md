🚇 MetroBakımTakip

One-click manage fault records, view maintenance history, and gain predictive insights for the future!

✨ Key Features

Manage Records 📝

Quickly add, delete, and filter fault entries

Real-time table search and date range filtering

Risk Score Calculation ⚠️

Automatically calculates the number of faults in the last 7 days

Instantly displays a RiskScore per station

Data Export 📤

Generate professional PDF reports using iText

Effortlessly export all data to CSV

Backup 💾

Create a metro_backup.db backup with a single click

Smart Prediction 🤖

One-click generation of faults_train.csv for training data

Train a FastTree model with ML.NET, saving faultModel.zip

Includes One-Hot Encoding and ultra-fast training (milliseconds!)

Predict fault probability (%) for any station/date/time

🚀 Getting Started

Clone the repository:

git clone [https://github.com/<username>/MetroBakimTakip.git](https://github.com/emineugurlu/MetroBakimTakip)


cd MetroBakimTakip

Open the solution:

Launch MetroBakimTakip.sln in Visual Studio

Install NuGet packages:

Microsoft.ML, Microsoft.ML.FastTree

System.Data.SQLite or Microsoft.Data.SQLite

iText7 (iText.Kernel, iText.Layout)

Prepare the database:

Copy metro.db to the project root or bin\Debug folder

Build and run:

Press F5 in Visual Studio

🎯 Usage Workflow

Fault Records: Fill out the form to add or delete records.

View Risk Scores: See each station’s 7-day fault score in the table.

Generate Training Data: Click "Training Data" to export faults_train.csv.

Train Model: Click "Train" to train the FastTree model; saves faultModel.zip.

Predict: Select station, date, and time; view fault probability (%) in seconds.

Export PDF & CSV: Share or archive reports as needed.

Backup: Create a backup for easy restoration.

📷 Screenshots

<img width="1670" height="926" alt="image" src="https://github.com/user-attachments/assets/a5d190d8-2e72-4f64-8db2-f94cd77dc6eb" />
<img width="1668" height="924" alt="image" src="https://github.com/user-attachments/assets/f4cb748e-74fd-4cd7-9799-4b3e674863ea" />
<img width="1731" height="872" alt="Ekran görüntüsü 2025-07-11 141216" src="https://github.com/user-attachments/assets/d36865bf-49b5-4100-b397-fd1f80024211" />
<img width="1645" height="933" alt="image" src="https://github.com/user-attachments/assets/9e2645ae-faa4-4c34-9771-3fb3cdc9600e" />

💡 Why MetroBakımTakip?

Fast & Intuitive: Instant desktop access with Windows Forms

Data-Driven Decisions: Make planning easier with ML-based forecasts

Flexible & Extensible: Integrate with your own data sources seamlessly

🤝 Contributing

Pull requests and issues are always welcome!

📜 License

This project is licensed under the MIT License. See LICENSE for details.

