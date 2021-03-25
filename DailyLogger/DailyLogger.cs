using System;
using System.Collections.Generic;
using System.IO;

namespace Logger
{
    class DailyLogger : ILog
    {
        private string _logsPath;
        private string _infoFile = "info.log";
        private string _errorFile = "error.log";
        private List<string> _uniqueWarnings = new List<string>();
        private List<string> _uniqueErrors = new List<string>();
        private string _todayDirectoryPath;

        private bool TodayDirectoryExists()
        {
            return Directory.Exists(_todayDirectoryPath);
        }

        private void CreateTodayDirectory()
        {
            _todayDirectoryPath = $"{_logsPath}\\{DateTime.Today:yyyy-MM-dd}";
            Directory.CreateDirectory(_todayDirectoryPath);
            _uniqueErrors.Clear();
            _uniqueWarnings.Clear();
        }

        private void Write(string fileName, string message, string category)
        {
            var filePath = $"{_todayDirectoryPath}\\{fileName}";

            using (var logFile = File.AppendText(filePath))
            {
                logFile.WriteLine($"{DateTime.Now} ({category.ToUpper()}): {message}");
            } 
        }

        private void Write(string fileName, string message, string category, Exception ex)
        {
            var filePath = $"{_todayDirectoryPath}\\{fileName}";

            using (var logFile = File.AppendText(filePath))
            {
                logFile.WriteLine($"{DateTime.Now} ({category.ToUpper()}): {message}, Exception: {ex.Message}");
            }
        }

        private void Write(string fileName, string message, string category, params object[] args)
        {
            var filePath = $"{_todayDirectoryPath}\\{fileName}";

            using (var logFile = File.AppendText(filePath))
            {
                var formatted = String.Format(message, args);
                logFile.WriteLine($"{DateTime.Now} ({category.ToUpper()}): {formatted}");
            }
        }

        private void Write(string fileName, string message, string category, Dictionary<object, object> properties)
        {
            var filePath = $"{_todayDirectoryPath}\\{fileName}";

            using (var logFile = File.AppendText(filePath))
            {
                logFile.WriteLine($"{DateTime.Now} ({category.ToUpper()}): {message}");

                if (properties != null)
                {
                    foreach (var property in properties)
                    {
                        logFile.WriteLine($"\t{property.Key}: {property.Value}");
                    }
                }
            }
        }

        public DailyLogger(string logsPath)
        {
            _logsPath = logsPath;
        }

        public void Debug(string message)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "debug");
        }

        public void Debug(string message, Exception e)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "debug", e);
        }

        public void DebugFormat(string message, params object[] args)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "debug", args);
        }

        public void Error(string message)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_errorFile, message, "error");
        }

        public void Error(string message, Exception e)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_errorFile, message, "error", e);
        }

        public void Error(Exception ex)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_errorFile, "", "error", ex);
        }

        public void ErrorUnique(string message, Exception e)
        {
            if(!_uniqueErrors.Contains(message))
            {
                Write(_errorFile, message, "unique error", e);
                _uniqueErrors.Add(message);
            }
        }

        public void Fatal(string message)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_errorFile, message, "fatal");
        }

        public void Fatal(string message, Exception e)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_errorFile, message, "fatal", e);
        }

        public void Info(string message)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "info");
        }

        public void Info(string message, Exception e)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "info", e);
        }

        public void Info(string message, params object[] args)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "info", args);
        }

        public void SystemInfo(string message, Dictionary<object, object> properties = null)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "system info", properties);
        }

        public void Warning(string message)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "warning");
        }

        public void Warning(string message, Exception e)
        {
            if (!TodayDirectoryExists())
            {
                CreateTodayDirectory();
            }

            Write(_infoFile, message, "warning", e);
        }

        public void WarningUnique(string message)
        {
            if (!_uniqueWarnings.Contains(message))
            {
                Write(_infoFile, message, "warning unique");
                _uniqueWarnings.Add(message);
            }
        }
    }
}
