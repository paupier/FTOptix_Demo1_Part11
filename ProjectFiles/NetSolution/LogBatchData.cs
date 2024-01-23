#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Core;
using FTOptix.ODBCStore;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
using FTOptix.MelsecFX3U;
#endregion

public class LogBatchData : BaseNetLogic
{
    [ExportMethod]
    public void BatchDataLogging()
    {
        Store myStore = Project.Current.Get<Store>("DataStores/EDB_BatchEvents");
        Table myTable = myStore.Tables.Get<Table>("BatchData");
        
        string batchno = Project.Current.GetVariable("Model/BatchReport/BatchName").Value;
        DateTime batchstart = Project.Current.GetVariable("Model/BatchReport/BatchStartTime").Value;
        DateTime batchstop = Project.Current.GetVariable("Model/BatchReport/BatchStopTime").Value;
        string optname = Project.Current.GetVariable("Model/BatchReport/OperatorName").Value;
        int prodvial = Project.Current.GetVariable("Model/BatchReport/ProducedVials").Value;
        int goodvial = Project.Current.GetVariable("Model/BatchReport/GoodVials").Value;
        int badvial = Project.Current.GetVariable("Model/BatchReport/BedVials").Value;

        object[,] rawValues = new object [1,10]; // [Raw, Column]; Column = number columns in Table of Audit event logger  database 
        rawValues[0,0] = DateTime.Now;
        rawValues[0,1] = batchno;
        rawValues[0,2] = batchstart;
        rawValues[0,3] = batchstop;
        rawValues[0,4] = optname;
        rawValues[0,5] = prodvial;
        rawValues[0,6] = goodvial;
        rawValues[0,7] = badvial;
        rawValues[0,8] = "NO";
        rawValues[0,9] = "";
        string[] columns = new string[10] {"LocalTimeStamp", "BatchNumber", "BatchStartTime", "BatchStopTime", "OperatorName", "ProducedVials", "GoodVials", "BadVials", "Checked", "CheckedBy"};
        myTable.Insert(columns,rawValues);
    }
}
