using System;
public static class RegressionUtils
{
    public struct RegressionResult
    {
        public float Slope;
        public float Intercept;
        public float RValue;
        public float PredictedValue;
        
        public override string ToString()
        {
            return $"slope: {Slope}, intercept: {Intercept}, rValue: {RValue}, PredictedValue: {PredictedValue}";
        }
        
        public RegressionResult(float slope, float intercept, float rValue, float predictedValue)
        {
            Slope = slope;
            Intercept = intercept;
            RValue = rValue;
            PredictedValue = predictedValue;
        }
    }
    
    public static void CalculateLinearRegression(float[] y, out float slope, out float intercept, out float rValue)
    {
        int n = y.Length;

        // X değerlerini otomatik oluştur
        int[] x = CreateSequentialArray(n);

        float xSum = 0, ySum = 0, xySum = 0, xSquaredSum = 0, ySquaredSum = 0;

        for (int i = 0; i < n; i++)
        {
            xSum += x[i];
            ySum += y[i];
            xySum += x[i] * y[i];
            xSquaredSum += x[i] * x[i];
            ySquaredSum += y[i] * y[i];
        }

        // Eğim hesaplama
        slope = (n * xySum - xSum * ySum) / (n * xSquaredSum - xSum * xSum);
        // Y-Kesişimi hesaplama
        intercept = (ySum - slope * xSum) / n;

        // R değeri hesaplama
        rValue = (n * xySum - xSum * ySum) /
                 (float)Math.Sqrt((n * xSquaredSum - xSum * xSum) * (n * ySquaredSum - ySum * ySum));
    }
    
    // X değerlerini sıralı olarak oluşturur
    private static int[] CreateSequentialArray(int length)
    {
        int[] xValues = new int[length];
        for (int i = 0; i < length; i++)
        {
            xValues[i] = i + 1;
        }
        return xValues;
    }
    
    static float[] ConvertIntArrayToFloatArray(int[] intArray)
    {
        return Array.ConvertAll(intArray, item => (float)item);
    }

    // Verilen X değeri için tahmin yapar
    public static RegressionResult PredictValue(int index,float[] values)
    {
        float slope, intercept, rValue;
        CalculateLinearRegression(values, out slope, out intercept, out rValue);
        
        return new RegressionResult
        {
            Slope = slope,
            Intercept = intercept,
            RValue = rValue,
            PredictedValue = slope * index + intercept
        };
    }
    
    public static RegressionResult PredictValue(int index,int[] values)
    {
        return PredictValue(index,ConvertIntArrayToFloatArray(values));
    }
}