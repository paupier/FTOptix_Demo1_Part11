#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.Store;
using FTOptix.Core;
using FTOptix.ODBCStore;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
using FTOptix.MelsecFX3U;
#endregion

public class GetBatchInfo : BaseNetLogic
{
    public override void Start()
    {
        SelBatchNoVar = Project.Current.GetVariable("Model/BatchPara/SelectedBatchNO");
        SelBatchNoVar.VariableChange += SelBatchNoVar_VariableChange;

        LoadBatchInfo();
    }

    private void SelBatchNoVar_VariableChange(object sender, VariableChangeEventArgs e)
    {
        LoadBatchInfo();
    }

    private void LoadBatchInfo()
    {
        var myStore = Project.Current.Get<Store>("DataStores/EDB_BatchEvents");
        string batchsquery = Project.Current.GetVariable("Model/BatchPara/Batchsqlquery").Value;
        //Log.Warning(batchsquery);
        myStore.Query(batchsquery, out string[] header, out object[,] resultSet);
        //int LastVal = resultSet.GetLength(0) - 1;
        //Log.Warning("Number Results = " + resultSet.GetLength(0));

        string batchno = "";
        DateTime batchstart = default;
        DateTime batchstop = default;
        string optname = "";
        int prodvial = 0;
        int goodvial = 0;
        int badvial = 0;
        string reviewed = "";
        string reviewby = "";
        if (resultSet.GetLength(0) > 0)
        {
            batchno = resultSet[0,0].ToString();
            batchstart = DateTime.Parse(resultSet[0,1].ToString());
            batchstop = DateTime.Parse(resultSet[0,2].ToString());
            optname = resultSet[0,3].ToString();
            prodvial = int.Parse(resultSet[0,4].ToString());
            goodvial = int.Parse(resultSet[0,5].ToString());
            badvial = int.Parse(resultSet[0,6].ToString());
            //Log.Warning("Bool Value = " + resultSet[0,6].ToString());
            reviewed = resultSet[0,7].ToString();
            reviewby = resultSet[0,8].ToString();
        }
        
        Project.Current.GetVariable("Model/BatchPara/BatchNumber").Value = batchno;
        Project.Current.GetVariable("Model/BatchPara/BatchStartTime").Value = batchstart;
        Project.Current.GetVariable("Model/BatchPara/BatchStopTime").Value = batchstop;
        Project.Current.GetVariable("Model/BatchPara/OperatorName").Value = optname;
        Project.Current.GetVariable("Model/BatchPara/ProducedVials").Value = prodvial;
        Project.Current.GetVariable("Model/BatchPara/GoodVials").Value = goodvial;
        Project.Current.GetVariable("Model/BatchPara/BadVials").Value = badvial;
        Project.Current.GetVariable("Model/BatchPara/Checked").Value = reviewed;
        Project.Current.GetVariable("Model/BatchPara/CheckedBy").Value = reviewby;
        
    }

    private IUAVariable SelBatchNoVar;
}
