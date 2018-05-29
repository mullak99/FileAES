﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileAES
{
    static class Program
    {

        public static string fileName = "";
        public static string tempPathInstance = "";
        public static bool doEncryptFile = false;
        public static bool doEncryptFolder = false;
        public static bool doDecrypt = false;
        static bool skipUpdate = false;
        static bool fullInstall = false;
        static bool cleanUpdates = false;
        static string branch = "";
        static string launchTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

        [STAThread]
        static void Main(string[] args)
        {
            tempPathInstance = Path.Combine(Path.GetTempPath(), "FileAES");
            List<string> arguments = new List<string>();
            arguments.AddRange(args);
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg"))) arguments.AddRange(readLaunchParams());
            if (File.Exists(@"Config\launchParams.cfg")) arguments.AddRange(readLaunchParams(true));

            string[] param = arguments.ToArray();

            for (int i = 0; i < param.Length; i++)
            {
                param[i].ToLower();
                if (File.Exists(param[i]) && Core.isValidFiletype(Path.GetExtension(param[i])) && !doEncryptFile && !doEncryptFolder)
                {
                    doDecrypt = true;
                    fileName = param[i];
                }
                else if (Directory.Exists(param[i]) && !doDecrypt && !doEncryptFile)
                {
                    doEncryptFolder = true;
                    fileName = param[i];
                }
                else if (File.Exists(param[i]) && !doDecrypt && !doEncryptFolder)
                {
                    doEncryptFile = true;
                    fileName = param[i];
                }
                if (param[i].Equals("-fullinstall") || param[i].Equals("--fullinstall") || param[i].Equals("-f") || param[i].Equals("--f")) fullInstall = true;
                if (param[i] == "--dev") branch = "dev";
                else if (param[i] == "--stable") branch = "stable";
                if (param[i] == "--skipupdate" || param[i] == "-skipupdate") skipUpdate = true;

                if (param[i].Equals("-cleanupdates") || param[i].Equals("--cleanupdates") || param[i].Equals("-c") || param[i].Equals("--c"))
                    cleanUpdates = true;
                if (param[i].Equals("-update") || param[i].Equals("--update") || param[i].Equals("-u") || param[i].Equals("--u"))
                    FileAES_Update.selfUpdate(cleanUpdates);
                
            }
            if (String.IsNullOrEmpty(branch)) branch = "stable";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (doEncryptFile || doEncryptFolder) Application.Run(new FileAES_Encrypt(fileName));
            else if (doDecrypt) Application.Run(new FileAES_Decrypt(fileName));
            else Application.Run(new FileAES_Main());

        }

        public static string[] readLaunchParams(bool local = false)
        {
            string dir;
            if (local)
                dir = Path.Combine(Directory.GetCurrentDirectory(), @"config\launchParams.cfg");
            else
                dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg");

            return File.ReadAllLines(dir);
        }

        public static string getBranch()
        {
            return branch;
        }

        public static bool getCleanUpdates()
        {
            return cleanUpdates;
        }

        public static bool getFullInstall()
        {
            return fullInstall;
        }

        public static bool getSkipUpdate()
        {
            return skipUpdate;
        }
    }
}