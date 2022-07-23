﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Power_Control_Panel.PowerControlPanel.Classes
{
    public class StreamWriterLog
    {
        //public static string BaseDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        public static Object objLock = new Object();
        public static void startStreamWriter(string newLog)
        {
            try
            {
                if (!File.Exists(BaseDir + "\\PowerControlPanel\\Logs\\application_log.txt")) { createLogFile(); }
                using (StreamWriter w = File.AppendText("PowerControlPanel/Logs/application_log.txt"))
                {
                    lock(objLock)
                    {
                        Log(newLog, w);
                    }
                    
                }




            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

         
        }
        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} {logMessage}");
            w.Flush();
        }
        public static void createLogFile()
        {
            if (!Directory.Exists(BaseDir + "\\PowerControlPanel\\Logs")) { System.IO.Directory.CreateDirectory(BaseDir + "\\PowerControlPanel\\Logs"); }
            if (!File.Exists(BaseDir + "\\PowerControlPanel\\Logs\\application_log.txt")) { File.CreateText(BaseDir + "\\PowerControlPanel\\Logs\\application_log.txt"); }
        }

    }
}
