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

public class DelayedAction : BaseNetLogic
{
    [ExportMethod]
    public void UpdateBacthNum()
    {
        UpdateBNum();
    }

    private void UpdateBNum()
	{
		//Label BatchMsg = Project.Current.Get<Label>("UI/Screens/08Reports/BatchReportGenerate/Backgroung/ReportControlBorder/Rectangle2/Label1");
        //BatchMsg.Text = message;
        //BatchMsg.Visible = true;
        if (delayedTask != null)
			delayedTask?.Dispose();
		
		delayedTask = new DelayedTask(DelayedAction2, 5000, LogicObject);
		delayedTask.Start();
	}

	private void DelayedAction2(DelayedTask task)
	{
		if (task.IsCancellationRequested)
			return;
        
        string SelBatchNum = "";
        Project.Current.GetVariable("Model/BatchPara/SelectedBatchNO").Value = SelBatchNum;
        
		delayedTask?.Dispose();
	}

    private DelayedTask delayedTask;
}
