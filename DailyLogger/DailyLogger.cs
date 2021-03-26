using System;
using System.Collections.Generic;
using System.IO;

namespace Logger
{
    /// <summary>
    /// Класс логгера, реализующий интерфейс ILog
    /// </summary>
    class DailyLogger : ILog
    {
        private const string _infoFile = "info.log";
        private const string _errorFile = "error.log";
        private readonly string _logsPath;
        private List<string> _uniqueWarnings = new List<string>();
        private List<string> _uniqueErrors = new List<string>();
        private string _todayDirectoryPath;
        private enum MessageCategory
        {
            Debug,
            Error,
            Fatal,
            Info,
            SystemInfo,
            Warning
        }

        public DailyLogger(string logsPath)
        {
            _logsPath = logsPath;
        }

        public void Debug(string message)
        {
            Write(_infoFile, message, MessageCategory.Debug);
        }

        public void Debug(string message, Exception e)
        {
            Write(_infoFile, message, MessageCategory.Debug, ex: e);
        }

        public void DebugFormat(string message, params object[] args)
        {
            Write(_infoFile, message, MessageCategory.Debug, args: args);
        }

        public void Error(string message)
        {
            Write(_errorFile, message, MessageCategory.Error);
        }

        public void Error(string message, Exception e)
        {
            Write(_errorFile, message, MessageCategory.Error, ex: e);
        }

        public void Error(Exception ex)
        {
            Write(_errorFile, "", MessageCategory.Error, ex: ex);
        }

        public void ErrorUnique(string message, Exception e)
        {
            if(!_uniqueErrors.Contains(message))
            {
                Write(_errorFile, message, MessageCategory.Error, true, e);
                _uniqueErrors.Add(message);
            }
        }

        public void Fatal(string message)
        {
            Write(_errorFile, message, MessageCategory.Fatal);
        }

        public void Fatal(string message, Exception e)
        {
            Write(_errorFile, message, MessageCategory.Fatal, ex: e);
        }

        public void Info(string message)
        {
            Write(_infoFile, message, MessageCategory.Info);
        }

        public void Info(string message, Exception e)
        {
            Write(_infoFile, message, MessageCategory.Info, ex: e);
        }

        public void Info(string message, params object[] args)
        {
            Write(_infoFile, message, MessageCategory.Info, args: args);
        }

        public void SystemInfo(string message, Dictionary<object, object> properties = null)
        {
            Write(_infoFile, message, MessageCategory.SystemInfo, properties: properties);
        }

        public void Warning(string message)
        {
            Write(_infoFile, message, MessageCategory.Warning);
        }

        public void Warning(string message, Exception e)
        {
            Write(_infoFile, message, MessageCategory.Warning, ex: e);
        }

        public void WarningUnique(string message)
        {
            if (!_uniqueWarnings.Contains(message))
            {
                Write(_infoFile, message, MessageCategory.Warning, true);
                _uniqueWarnings.Add(message);
            }
        }

        /// <summary>
        /// Метод записи сообщения лога в соответствующий файл
        /// </summary>
        private void Write(string fileName, string message, MessageCategory category, bool isUnique = false,
                           Exception ex = null, Dictionary<object, object> properties = null, params object[] args)
        {
            _todayDirectoryPath = $"{_logsPath}\\{DateTime.Today:yyyy-MM-dd}";
            if (Directory.Exists(_todayDirectoryPath))
            {
                Directory.CreateDirectory(_todayDirectoryPath);
                _uniqueErrors.Clear();
                _uniqueWarnings.Clear();
            }

            var filePath = $"{_todayDirectoryPath}\\{fileName}";

            using (var logFile = File.AppendText(filePath))
            {
                string categoryToWrite = isUnique ? "UNIQUE" + category.ToString() : category.ToString();
                logFile.WriteLine($"{DateTime.Now} ({categoryToWrite.ToUpper()}): {message}");
                if (ex != null)
                {
                    logFile.WriteLine($"Exception: {ex.Message}");
                    logFile.WriteLine($"Stack Trace: {ex.StackTrace}");
                }

                if (args.Length != 0)
                {
                    string arguments = "Arguments:";
                    foreach(var arg in args)
                    {
                        arguments += " " + arg;
                    }
                    logFile.WriteLine(arguments);
                }
                
                if (properties != null)
                {
                    foreach (var property in properties)
                    {
                        logFile.WriteLine($"\t{property.Key}: {property.Value}");
                    }
                }
            }
        }
    }
}
