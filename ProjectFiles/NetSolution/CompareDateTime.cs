#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.UI;
using FTOptix.NativeUI;
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

public class CompareDateTime : BaseNetLogic
{
    [ExportMethod]
    public void CompareTime(NodeId labelNodeId, DateTime dt1, DateTime dt2)
    {
        var LabelMsg = InformationModel.Get<Label>(labelNodeId);
        if (dt1 < dt2)
        {
            LabelMsg.Text = "DateTime1 < DateTime2";
        }
        else if (dt1 == dt2)
        {
            LabelMsg.Text = "DateTime1 = DateTime2";
        }
        else if (dt1 > dt2)
        {
            LabelMsg.Text = "DateTime1 > DateTime2";
        }
    }
}
