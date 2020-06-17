using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EasyAssetManagerCore.Models.CommonModel
{
    public class Logging
    {
        public static string LogPath(string logPath)
        {
            string subPath = "Log";
            bool exists = Directory.Exists(subPath);
            if (!exists)
                Directory.CreateDirectory(subPath);
            return null;
        }
        public static string GenerateDefaultLogFileName(string BaseFileName)
        {
            // return HttpContext.Current.Server.MapPath("~") + "\\log\\" + BaseFileName + "_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".log";
            return null;
        }

        public static void WriteToLog(string Ip, string User, string Location, string Message)
        {
            string logFile = "Log/EasyBankLog_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (StreamWriter s = File.AppendText(LogPath("") + logFile))
                {
                    s.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Ip + "|" + User + "|" + Location + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToErrLog(string Ip, string User, string Location, string Message)
        {
            string logFile = "Log/EasyBankErr_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (StreamWriter s = File.AppendText(LogPath("") + logFile))
                {
                    s.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Ip + "|" + User + "|" + Location + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToCbsLog(string Message)
        {
            string logFile = "Log/EasyBankCbs_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (StreamWriter s = File.AppendText(LogPath("") + logFile))
                {
                    s.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToCbsErrLog(string Message)
        {
            string logFile = "Log/EasyBankCbsErr_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (StreamWriter s = File.AppendText(LogPath("") + logFile))
                {
                    s.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToOtpLog(string Message)
        {
            string logFile = "Log/EasyBankOtp_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (StreamWriter s = File.AppendText(LogPath("") + logFile))
                {
                    s.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToOtpErrLog(string Message)
        {
            string logFile = "Log/EasyBankCbsOtp_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (StreamWriter s = File.AppendText(LogPath("") + logFile))
                {
                    s.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToSessionLog(string Ip, string User, string Location, string Message)
        {
            string logFile = "Log/EasyBankSessionLog_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (FileStream fs = File.Open(Path.Combine(LogPath(""), logFile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    AddText(fs, DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Ip + "|" + User + "|" + Location + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToSessionErr(string Ip, string User, string Location, string Message)
        {
            string logFile = "Log/EasyBankSessionErr_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (FileStream fs = File.Open(Path.Combine(LogPath(""), logFile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    AddText(fs, DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Ip + "|" + User + "|" + Location + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToAppLog(string Location, string Message)
        {
            string logFile = "Log/EasyBankApplog_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (FileStream fs = File.Open(Path.Combine(LogPath(""), logFile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    AddText(fs, DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Location + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void WriteToAppErr(string Location, string Message)
        {
            string logFile = "Log/EasyBankAppErr_" + DateTime.Now.ToString("ddMMyyyy") + ".log";

            try
            {
                using (FileStream fs = File.Open(Path.Combine(LogPath(""), logFile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    AddText(fs, DateTime.Now.ToString("dd/MM/yyyy hh:mm:sstt") + "|" + Location + "|" + Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);

            byte[] newLine = Encoding.Default.GetBytes(Environment.NewLine);
            fs.Write(newLine, 0, newLine.Length);
        }


        /// <summary>
        /// Writes a message to the application event log
        /// /// </summary>
        /// <param name="Source">Source is the source of the message ususally you will want this to be the application name</param>
        /// <param name="Message">message to be written</param>
        /// <param name="EntryType">the entry type to use to categorize the message like for exmaple error or information</param>

        //public static void WriteToEventLog(string Source, string Message, System.Diagnostics.EventLogEntryType EntryType)
        //{
        //    try
        //    {
        //        if (!EventLog.SourceExists(Source))
        //        {
        //            EventLog.CreateEventSource(Source, Application.ProductName);
        //        }
        //        EventLog.WriteEntry(Source, Message, EntryType);
        //    }

        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}
    }
}
