using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace HRRcp.App_Code
{
    public static class Pdf2Img
    {
        static volatile PdfPngR PdfToPngRunning;
        const int PdfToPngTimeOut = 240; // in Seconds
        const int PdfToPngUserTimeOut = 120000; // in Miliseconds

        /// <summary>
        /// </summary>
        /// <param name="filePath">Path to pdf file</param>
        /// <returns>Returns paths to png files</returns>
        public static string[] getPNGs(string filePath)
        {
            string DirectoryPath = Path.GetDirectoryName(filePath);
            string FileNameBase = Path.GetFileNameWithoutExtension(filePath);
            return Directory.GetFiles(DirectoryPath, string.Format("{0}_*.png", FileNameBase));
        }

        /// <summary>
        /// returns 0 when success or 1 on exception
        /// </summary>
        /// <param name="filePath">Path to pdf file</param>
        public static int DeletePNGImages(string filePath)
        {
            string[] files = getPNGs(filePath);
            foreach (var Item in files)
            {
                try
                {
                    File.Delete(Item);
                }
                catch (Exception ex)
                {
                    return 1;
                }
            }
            return 0;
        }

        private class PdfPngR
        {
            public string file { get; private set; }
            public DateTime time { get; private set; }
            public PdfPngR(string file)
            {
                this.file = file;
                this.time = DateTime.Now;
            }
        }

        /// <summary>
        /// Returns 0 when success or other value when error occured
        /// 1 - already running (less than timeout)
        /// 2 - application closed without errors but files not created
        /// 3 - timeout
        /// >256 (Error Code >> 8) App error code
        /// </summary>
        /// <param name="PdfToPngPath">Path to PdfToPng_v2.exe</param>
        /// <param name="filePath">Path to pdf file</param>
        /// <returns>Error code</returns>
        public static int AddPNGImages(string PdfToPngPath, string filePath)
        {
            string cPath = Path.GetDirectoryName(filePath) + Path.DirectorySeparatorChar + "Pdf2PngTemp.pdf";
            if (File.Exists(cPath))
            {
                Log.Error(Log.PDFTOPNG, filePath, "PDF->PNG Temp file already exist");
                File.Delete(cPath);
            }
            File.Copy(filePath, cPath);
            DeletePNGImages(filePath);
            if (PdfToPngRunning != null)
            {
                TimeSpan s = DateTime.Now - PdfToPngRunning.time;
                if (s.TotalSeconds > PdfToPngTimeOut)
                {
                    Log.Error(Log.PDFTOPNG, filePath, string.Format("PDF->PNG Running more than {0}sec", PdfToPngTimeOut));
                }
                else
                {
                    Tools.ShowMessage("Obecnie jest już wykonywana konwersja pliku " + PdfToPngRunning.file);
                    return 1;
                }
            }

            string filename = HttpContext.Current.Server.MapPath(PdfToPngPath);
            ProcessStartInfo sInfo = new ProcessStartInfo(filename, string.Format("\"{0}\"", cPath));
            sInfo.WorkingDirectory = Path.GetDirectoryName(filename);
            Process proc = Process.Start(sInfo);    //20160514 <<<<< dodać try catch !!! bo się może wywalic z bledami np niezgodna wersja .net, brak pliku exe
            PdfToPngRunning = new PdfPngR(filePath);

            if (proc.WaitForExit(PdfToPngUserTimeOut))
            {
                File.Delete(cPath);
                PdfToPngRunning = null;
                if (proc.ExitCode != 0)
                {
                    Log.Error(Log.PDFTOPNG, filePath, string.Format("PDF->PNG ExitCode {0}", proc.ExitCode));
                    return (proc.ExitCode << 8);
                }
                else
                {
                    int n = Pdf2Img.getPNGs(cPath).Count();
                    if (n == 0)
                    {
                        Log.Error(Log.PDFTOPNG, filePath, string.Format("PDF->PNG No files created"));
                        return 2;
                    }
                    else
                    {
                        Log.Info(Log.PDFTOPNG, filePath, string.Format("PDF->PNG Success with {0} files", n));
                        string baseFn = Path.GetDirectoryName(cPath) + 
                            Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(filePath) + "_";
                        foreach (var Item in getPNGs(cPath))
                        {
                            string num = Path.GetFileNameWithoutExtension(Item.Substring(Item.LastIndexOf('_')+1));
                            string fn = baseFn + num + ".png";
                            File.Move(Item, fn);
                        }
                    }
                }
            }
            else
            {
                //PdfToPngRunning = null;
                Log.Error(Log.PDFTOPNG, filePath, string.Format("PDF->PNG Timeout"));
                return 3;
            }
            return 0;
        }
    }
}
