using ASTI_Vision.DTO;
using Basler.Pylon;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace ASTI_Vision.Helpers
{
    public class Helper
    {
        public static string[] GetProgramList(string keyword = null)
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string programsDir = Path.Combine(exeDir, "Programs");

            if (!Directory.Exists(programsDir))
            {
                Directory.CreateDirectory(programsDir);
            }

            string[] folders = Directory.GetDirectories(programsDir);

            for (int i = 0; i < folders.Length; i++)
            {
                folders[i] = Path.GetFileName(folders[i]);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                folders = folders
                    .Where(f => f.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToArray();
            }

            return folders;
        }

        public static void OpenProgramsFolder()
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string programsDir = Path.Combine(exeDir, "Programs");

            if (!Directory.Exists(programsDir))
            {
                Directory.CreateDirectory(programsDir);
            }

            Process.Start("explorer.exe", programsDir);
        }

        public static string GetProgramValue(string folderName)
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string programsDir = Path.Combine(exeDir, "Programs");
            string selectedFolder = Path.Combine(programsDir, folderName);

            string jsonPath = Path.Combine(selectedFolder, "Config.json");
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine("Không tìm thấy Config.json trong folder đã chọn!");
                return null;
            }

            try
            {
                string json = File.ReadAllText(jsonPath);

                return json;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;

            }
        }

        public static string LightingError(string msg)
        {
            if (msg[2] == 'N')
            {
                string errorCode = msg.Substring(3, 2); // ký tự thứ 4-5 (index 3-4)

                string errorMessage;
                switch (errorCode)
                {
                    case "01":
                        errorMessage = "Command Error";
                        break;
                    case "02":
                        errorMessage = "Checksum Error";
                        break;
                    default:
                        errorMessage = $"Unknown Error Code: {errorCode}";
                        break;
                }

                return $"Device error returned: {errorMessage}";
            }
            return null;
        }

        public static string CalculateChecksum(string input)
        {
            int sum = 0;
            foreach (char c in input)
            {
                sum += (int)c;
            }

            int checksum = sum & 0xFF;

            return checksum.ToString("X2");
        }

        public static void ExportConfigToJson(Camera cam, string filePath)
        {
            try
            {
                var config = new CameraConfigDTO
                {
                    SerialNumber = cam.CameraInfo[CameraInfoKey.SerialNumber],
                    ModelName = cam.CameraInfo[CameraInfoKey.ModelName],
                    Width = cam.Parameters[PLCamera.Width].GetValue(),
                    Height = cam.Parameters[PLCamera.Height].GetValue(),
                    ExposureTime = cam.Parameters[PLCamera.ExposureTime].GetValue(),
                    Gain = cam.Parameters[PLCamera.Gain].GetValue(),
                    Gamma = cam.Parameters[PLCamera.Gamma].GetValue(),
                    PixelFormat = cam.Parameters[PLCamera.PixelFormat].GetValue(),
                    GainAuto = cam.Parameters[PLCamera.GainAuto].GetValue(),
                    ExposureAuto = cam.Parameters[PLCamera.ExposureAuto].GetValue(),
                    WhiteBalanceAuto = cam.Parameters[PLCamera.BalanceWhiteAuto].GetValue(),
                    AcqusitionFrameRateEnable = cam.Parameters[PLCamera.AcquisitionFrameRateEnable].GetValue(),
                };

                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi export JSON: " + ex.Message);
            }
        }
    }
}
