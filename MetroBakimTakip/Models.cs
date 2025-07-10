using Microsoft.ML.Data;

namespace MetroBakimTakip
{
    public class FaultData
    {
        [LoadColumn(0)] public float FaultCountLast7Days { get; set; }
        [LoadColumn(1)] public float DayOfWeek { get; set; }
        [LoadColumn(2)] public float HourOfDay { get; set; }
        [LoadColumn(3), ColumnName("Label")]
        public bool Label { get; set; }
    }

    public class FaultPrediction
    {
        [ColumnName("PredictedLabel")] public bool PredictedFailure { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
