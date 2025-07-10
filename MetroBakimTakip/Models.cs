using Microsoft.ML.Data;

namespace MetroBakimTakip
{
    // Eğitim verisi satırları:
    public class FaultData
    {
        [LoadColumn(0)] public float FaultCountLast7Days { get; set; }
        [LoadColumn(1)] public float DayOfWeek { get; set; }
        [LoadColumn(2)] public float HourOfDay { get; set; }

        // CSV başlığında 4. sütun bu olduğu için:
        [LoadColumn(3), ColumnName("Label")]
        public bool Label { get; set; }
    }

    // Modelden dönen sonuç:
    public class FaultPrediction
    {
        // ML.NET’in ürettiği PredictedLabel sütununu burada yakalıyoruz:
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        // Olasılık ve score da gerekirse kullanılabilir:
        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
