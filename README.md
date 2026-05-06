# 🚇 MetroBakımTakip: Predictive Infrastructure Management

> **"An intelligent desktop ecosystem for metro maintenance, integrating ML.NET for predictive fault forecasting, SQLite for resilient data management, and automated risk scoring."**

![Repo Size](https://img.shields.io/github/repo-size/emineugurlu/MetroBakimTakip?color=0078d4&style=flat-square)
![Framework](https://img.shields.io/badge/Framework-.NET%20WinForms-512bd4?style=flat-square&logo=dotnet)
![Machine Learning](https://img.shields.io/badge/AI-ML.NET%20FastTree-512bd4?style=flat-square)

Infrastructure reliability is mission-critical. This project is a **Predictive Maintenance System** designed to transform reactive fault logs into proactive operational insights. By implementing an **ML.NET FastTree** regression/binary classification model, the system analyzes historical patterns to forecast future fault probabilities. This bridge between traditional desktop CRUD operations and Machine Learning allows for data-driven scheduling in high-stakes metro environments.

---

## 🚀 Engineering Mindset

This application demonstrates proficiency in **Industrial Software Architecture**:

*   **Predictive Modeling (ML.NET):** Implementing **FastTree** algorithms with One-Hot Encoding to process categorical station data and temporal features, enabling millisecond-speed fault probability forecasting.
*   **Resilient Data Layer:** Utilizing **SQLite** for lightweight yet robust local persistence, featuring an automated one-click backup mechanism (`metro_backup.db`) to ensure zero data loss.
*   **Algorithmic Risk Scoring:** Developing a dynamic calculation engine that monitors 7-day fault rolling windows to assign real-time **RiskScores** to individual stations.
*   **Professional Reporting Pipeline:** Integrating **iText7** for programmatic PDF generation and custom CSV exporters to facilitate formal maintenance documentation and inter-departmental data sharing.
*   **Binary Model Management:** Designing a workflow for on-the-fly model training (`faultModel.zip`) and immediate deployment within the application lifecycle.

## 🌟 Key Features

*   🤖 **AI-Powered Forecasting:** Predict fault probability (%) for specific stations based on date/time matrices.
*   ⚠️ **Dynamic Risk Mapping:** Instant visual feedback on station health using weighted fault frequency analysis.
*   📤 **Enterprise Export:** Generate high-fidelity PDF reports for maintenance audits and CSVs for external analysis.
*   💾 **Integrity Management:** Single-click database mirroring and maintenance history tracking.

## 🔧 Technical Stack

*   **Core:** .NET Framework / WinForms.
*   **Intelligence:** ML.NET (FastTree), One-Hot Encoding.
*   **Database:** System.Data.SQLite.
*   **Reporting:** iText7 (PDF), CSV Serializers.

## 📸 Visual Showcase

![Prediction Interface](https://github.com/user-attachments/assets/a5d190d8-2e72-4f64-8db2-f94cd77dc6eb)
![Training Logic](https://github.com/user-attachments/assets/f4cb748e-74fd-4cd7-9799-4b3e674863ea)
![Management Table](https://github.com/user-attachments/assets/d36865bf-49b5-4100-b397-fd1f80024211)
![Reporting View](https://github.com/user-attachments/assets/9e2645ae-faa4-4c34-9771-3fb3cdc9600e)

---

## 🛠️ Getting Started

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/emineugurlu/MetroBakimTakip.git](https://github.com/emineugurlu/MetroBakimTakip.git)
   ````
2.**Setup:**
  Open .sln in Visual Studio and restore NuGet packages (ML.NET, iText7, SQLite).

3.**Database:**
  Ensure metro.db is in the project root.

4. **Run:**
   Press F5 to launch the Maintenance Dashboard.

Developed by Emine Uğurlu - Computer Engineer. Inspired by industrial field operations.
