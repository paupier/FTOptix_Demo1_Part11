#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.Alarm;
using FTOptix.Recipe;
using FTOptix.EventLogger;
using FTOptix.DataLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Report;
using FTOptix.RAEtherNetIP;
using FTOptix.Modbus;
using FTOptix.Retentivity;
using FTOptix.AuditSigning;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.ODBCStore;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
using FTOptix.MelsecFX3U;
#endregion

public class FileCopy : BaseNetLogic
{
    [ExportMethod]
    public void CopyFile()
    {
        string sourcePath = "Z:\\01_Projects\\FTOptix\\FTOptix_Demo\\ProjectFiles\\Source";
        string targetPath = "Z:\\01_Projects\\FTOptix\\FTOptix_Demo\\ProjectFiles\\Dest";
        string fileName = string.Empty;
        string destFile = string.Empty;

        // To copy all the files in one directory to another directory. 
        // Get the files in the source folder. (To recursively iterate through 
        // all subfolders under the current directory, see 
        // "How to: Iterate Through a Directory Tree.")
        // Note: Check for target path was performed previously 
        //       in this code example. 
        if (System.IO.Directory.Exists(sourcePath))
        {
            string[] files = System.IO.Directory.GetFiles(sourcePath);

            // Copy the files and overwrite destination files if they already exist. 
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                Log.Info("Filename = " + s);
                fileName = System.IO.Path.GetFileName(s);
                destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(s, destFile, true);
            }
        }
        else
        {
            Console.WriteLine("Source path does not exist!");
        }
    }
}
