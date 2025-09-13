namespace ASTI_Vision.DTO
{
    public class CameraConfigDTO
    {
        public string SerialNumber { get; set; }
        public string ModelName { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public double ExposureTime { get; set; }
        public string ExposureAuto { get; set; }
        public double Gain { get; set; }
        public string GainAuto { get; set; }
        public double Gamma { get; set; }
        public string PixelFormat { get; set; }
        public string WhiteBalanceAuto { get; set; }
        public bool AcqusitionFrameRateEnable { get; set; }
    }
}
