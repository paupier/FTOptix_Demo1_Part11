#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
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

public class DisplayPDFDialog : BaseNetLogic
{
    public override void Start()
    {
        task = new DelayedTask(() =>
        {
            ShowPDFDiaglog();
        }, 5000, LogicObject);
    }

    public override void Stop()
    {
        task?.Dispose();
    }

    [ExportMethod]
    public void ShowDialog()
    {
        task.Start();
    }

    public void ShowPDFDiaglog()
    {
        var PDFdiag = (DialogType)Project.Current.Get<DialogType>("UI/Screens/02DataLogging/PDFViewer");
        var ownerButton4 = (Button)Owner;
        ownerButton4.OpenDialog(PDFdiag, NodeId.Empty);
    }

    private DelayedTask task;
}
