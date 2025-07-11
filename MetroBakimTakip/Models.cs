using Microsoft.ML.Data;

public class FaultData
{
    [LoadColumn(0)] public float FaultCountLast7Days;
    [LoadColumn(1)] public float DayOfWeek;
    [LoadColumn(2)] public float HourOfDay;
    [LoadColumn(3), ColumnName("Label")] public bool Label;
}

public class FaultPrediction
{
    [ColumnName("PredictedLabel")] public bool PredictedLabel;
    public float Probability;
    public float Score;
}
