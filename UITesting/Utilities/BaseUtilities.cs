using System;
using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using OpenQA.Selenium;

namespace UITesting
{
    public class BaseUtilities
    {
        private static bool loggerConfigured = false; // Flag to check if logger is already configured

        public static void DeleteOlderReportFolders(string baseReportFolder)
        {
            var directories = new DirectoryInfo(baseReportFolder).GetDirectories().OrderByDescending(d => d.CreationTime).ToList();
            for (int i = 2; i < directories.Count; i++)
            {
                directories[i].Delete(true); // Deletes the directory and its contents
            }
        }

        public static void SetupLogger(string path, bool writeToFile)
        {
            if (loggerConfigured)
            {
                // Logger is already configured, no need to set up again
                return;
            }

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            if (writeToFile)
            {
                RollingFileAppender roller = new RollingFileAppender();
                roller.AppendToFile = true;
                roller.File = $"{path}/log-file.txt";
                roller.Layout = patternLayout;
                roller.MaxSizeRollBackups = 5;
                roller.MaximumFileSize = "10MB";
                roller.RollingStyle = RollingFileAppender.RollingMode.Size;
                roller.StaticLogFileName = true;
                roller.ActivateOptions();
                hierarchy.Root.AddAppender(roller);
            }
            else
            {
                ConsoleAppender consoleAppender = new ConsoleAppender();
                consoleAppender.Layout = patternLayout;
                consoleAppender.ActivateOptions();
                hierarchy.Root.AddAppender(consoleAppender);
            }

            hierarchy.Root.Level = log4net.Core.Level.All;
            hierarchy.Configured = true;

            loggerConfigured = true; // Set the flag to indicate logger is configured
        }


    }
}
