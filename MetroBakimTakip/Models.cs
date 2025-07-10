using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ML.Data;

namespace MetroBakimTakip
{
    // — Eğitim için girdi verisi modeli —
    public class FaultData
    {
        [LoadColumn(0)]
        public float FaultCountLast7Days;   // Son 7 gündeki arıza sayısı

        [LoadColumn(1)]
        public float DayOfWeek;             // Pazartesi=1 … Pazar=7

        [LoadColumn(2)]
        public float HourOfDay;             // 0–23

        [LoadColumn(3), ColumnName("Label")]
        public bool WillFailNextDay;        // Ertesi gün arıza olacak mı?
    }

    // — Tahmin sonucu (prediction) modeli —
    public class FaultPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedFailure;       // Tahmin edilen etiket

        [ColumnName("Probability")]
        public float Probability;           // Olasılık

        [ColumnName("Score")]
        public float Score;                 // Score değeri (trainer’a bağlı)
    }
}
