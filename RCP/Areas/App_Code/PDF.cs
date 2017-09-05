using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

using System.Web.UI;
using System.Diagnostics;
using System.Text.RegularExpressions;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.App_Code
{
    public class PDF
    {
        private const String ConverterDir = "~/HTMLTOPDF/";
        private const String ConverterPath = "~/HTMLTOPDF/wkhtmltopdf.exe";
        private const String TempFile = "temp.html";

        public class ReplaceObject
        {
            public String Input, Replacement;
            public ReplaceObject(String Input, String Replacement)
            {
                this.Input = Input;
                this.Replacement = Replacement;
            }
        }

        public List<ReplaceObject> ReplaceObjects = null;
        public String[] CssClasses = {
                                //"../styles/default.css",
                                "../styles/Controls.css"
#if RCP
                                ,"../styles/master3.css"
                                ,"../styles/User/user.css"     
                                ,"../styles/print.css"
#endif
                            };
        public String[] SkipCssClasses = { "report_page" };
        public String[] SkipTags = { "select", "canvas" };
        public String Header = @"<!DOCTYPE html>

<html>
    <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
        <style>
            .pdffooter {{  font-family: Verdana; font-size: 9px; margin-top: 8px; }}
            .printon {{ display: block !important; }}
            .break {{ page-break-before: always; }}
            * {{ text-align: left; }}
        </style>
        {0}
    </head>";

        public String Footer = @"
        <div class=""pdffooter"">
            <span class=""left"">Wydrukowano z systemu {2} v.</span>
            <span class=""left"">{0}</span>
            <br />
            <span class=""left"">{1}</span>
        </div>";

        public String GetCssClassess()
        {
            String Output = String.Empty;
            foreach (String CssClass in CssClasses)
                Output += String.Format(@"<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", CssClass);
            return Output;
        }

        public String DrawHeader()
        {
            return String.Format(Header, GetCssClassess());
        }

        public String DrawFooter()
        {
            AppUser user = App.User;
            String Version = Tools.GetAppVersion();
            String PrintTime = Base.DateTimeToStr(DateTime.Now) + " " + user.ImieNazwisko;
            return String.Format(Footer, Version, PrintTime, Tools.GetAppName());
        }

        public String Prepare(String S, HttpRequest Request)
        {
            String Output = S;

            if (ReplaceObjects != null)
            {
                foreach (ReplaceObject RO in ReplaceObjects)
                {
                    Output = Output.Replace(RO.Input, RO.Replacement);
                }
            }


            Regex regex = new Regex(@"src=""\s*(.+?)\s*""");
            var results = regex.Matches(S);

            foreach (Match Match in results)
            {
                if (!Match.Value.Contains("base64"))
                {
                    Regex regex2 = new Regex(@"""\s*.+\s*""");
                    String result = regex2.Match(Match.Value).Value;
                    result = result.Substring(1, result.Length - 2);
                    int n = result.Split(new string[] { "../" }, StringSplitOptions.None).Length - 1;
                    string toReplace = string.Empty;
                    for (int i = 0; i < n; i++) toReplace += "../";
                    if (!String.IsNullOrEmpty(toReplace))
                    {
                        string newResult = result.Replace(toReplace, "../");
                        Output = Output.Replace(result, newResult);
                    }
                    else
                    {
                        string newResult = "../" + result;
                        Output = Output.Replace(result, newResult);
                    }
                }
            }

            foreach (String SkipCssClass in SkipCssClasses)
            {
                Output = Output.Replace(SkipCssClass, String.Empty);
            }

            foreach (String Tag in SkipTags)
            {
                String Expr = String.Format("<{0}(.|\n)*?</{0}>", Tag);
                Regex rgx = new Regex(Expr);
                Output = rgx.Replace(Output, String.Empty);
            }


            Output = Output.Replace("<a", "<span").Replace("</a", "</span");
            Output = Output.Replace("&lt;", "<").Replace("&gt;", ">");

            return Output;
        }

        public int Download(String Str, HttpServerUtility Server, HttpResponse Response, HttpRequest Request, String Filename)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            StreamWriter sw = new StreamWriter(Server.MapPath(String.Format("{0}{1}", ConverterDir, TempFile)));
            sw.Write(DrawHeader());
            sw.Write("<body>");
            if (Str.Contains("%%FOOTER%%"))
            {
                Str = Str.Replace("%%FOOTER%%", DrawFooter());
                sw.Write(Str);
            }
            else
            {
                sw.Write(Str);
                sw.Write(DrawFooter());
            }
            sw.Write("</body>");
            sw.Write("</html>");
            sw.Close();
            return ConvertAndDownload(Server, Response, Request, Filename);
        }

        public int Download(List<Control> Controls, HttpServerUtility Server, HttpResponse Response, HttpRequest Request, String Filename)
        {
            String Output = String.Empty;
            foreach (Control Cnt in Controls)
                Output += Prepare(HRRcp.Controls.EliteReports.DynamicControl.RenderControl(Cnt), Request);
            return Download(Output, Server, Response, Request, Filename);
        }

        public int Download(Control Control, HttpServerUtility Server, HttpResponse Response, HttpRequest Request)
        {
            return Download(Prepare(HRRcp.Controls.EliteReports.DynamicControl.RenderControl(Control), Request), Server, Response, Request, String.Empty);
        }

        public int Download(Control Control, HttpServerUtility Server, HttpResponse Response, HttpRequest Request, String Filename)
        {
            return Download(Prepare(HRRcp.Controls.EliteReports.DynamicControl.RenderControl(Control), Request), Server, Response, Request, Filename);
        }

        public int ConvertAndDownload(HttpServerUtility Server, HttpResponse Response, HttpRequest Request, String Filename)
        {
            Process ConverterProcess = new Process();
            ConverterProcess.StartInfo.FileName = Server.MapPath(ConverterPath);
            ConverterProcess.StartInfo.WorkingDirectory = Server.MapPath(ConverterDir);
            String FileToSave = String.IsNullOrEmpty(Filename) ? Guid.NewGuid().ToString("N") : Filename;
            ConverterProcess.StartInfo.Arguments = String.Format(@"{2} ""{0}"" ""{1}.pdf""", TempFile, FileToSave, Options);
            ConverterProcess.StartInfo.RedirectStandardInput = false;
            ConverterProcess.StartInfo.RedirectStandardOutput = true;
            ConverterProcess.StartInfo.CreateNoWindow = true;
            ConverterProcess.StartInfo.UseShellExecute = false;
            ConverterProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ConverterProcess.EnableRaisingEvents = true;
            ConverterProcess.Start();
            ConverterProcess.WaitForExit();

            String FilePath = Server.MapPath(String.Format("{0}{1}.pdf", ConverterDir, FileToSave));
            FileInfo file = new FileInfo(FilePath);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", file.Name));
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.Flush();
                File.Delete(FilePath);
                Response.End();
            }
            else
                return 1;
            return 0;
        }

        public String Title { get; set; }
        public String Options { get; set; }

    }
}
