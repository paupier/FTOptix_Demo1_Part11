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

public class CheckBatchNumber : BaseNetLogic
{

    /*
    private Button _confirmButton;
    private UANode msglabel;
    
    public override void Start()
    {
       _confirmButton = Owner.Get<Button>("Confirm");
       msglabel = Project.Current.Get<Label>("UI/Screens/08Reports/BatchNumberDialog/LabelMsg");
    }
    
    private void BatchNoVar_VariableChange(object sender, VariableChangeEventArgs e)
    {
        CheckEnteredBacthNumber();
    }
    */
    [ExportMethod]
    public void CheckEnteredBacthNumber(string CheckBatchName)
    {
        //Label MsgLabel = Project.Current.Get<Label>("UI/Screens/08Reports/BatchNumberDialog/LabelMsg");
        //Button ownerbtn = Owner.Get<Button>("Confirm");
        //DialogType BatchNumCheckResultDialog = Project.Current.Get<DialogType>("UI/Screens/08Reports/BatchNumberDialog");
        string EnterdBacthNum = CheckBatchName;
        if (string.IsNullOrEmpty(EnterdBacthNum))
        {
            ShowMessage("Please enter a valid batch number");
            //Button BatchMsg = Project.Current.Get<Button>("UI/Screens/08Reports/BatchReportGenerate/Button1");
            //BatchMsg.Text = "Please enter a valid batch number";
            //BatchMsg.Visible = false;
        }
        else
        {
            var myStore = Project.Current.Get<Store>("DataStores/EDB_BatchEvents");
            string SetSQLQuery = LogicObject.GetVariable("SQLQuery").Value;
            myStore.Query(SetSQLQuery, out string[] header, out object[,] resultSet);
            int lastval = resultSet.GetLength(0) - 1;
            if (resultSet.GetLength(0) > 0)
            {
                for (int i = 0; i < resultSet.GetLength(0); i++)
                {
                    string BatchNum = resultSet[i,0].ToString();
                    if (BatchNum.Equals(EnterdBacthNum, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMessage("Batch number already exist, please enter new one");
                        goto exitloop;
                    }
                    else if (i == lastval)
                    {
                        StartBatch();
                    }
                }
            }
            else
            {
                StartBatch();
            }

            exitloop:
            ;
        }
    }

    private void StartBatch()
    {
        Project.Current.GetVariable("Model/BatchReport/BatchEventMessage").Value = "Batch Started";
        Project.Current.GetVariable("Model/BatchReport/BatchStartTime").Value = DateTime.Now;
        Project.Current.GetVariable("Model/BatchReport/BatchStopTime").Value = DateTime.Now;
        Project.Current.GetVariable("Model/BatchReport/OperatorName").Value = Session.User.BrowseName;
        Project.Current.GetVariable("Model/BatchReport/BatchRunning").Value = true;
    }

    
    private void ShowMessage(string message)
	{
		//Label BatchMsg = Project.Current.Get<Label>("UI/Screens/08Reports/BatchReportGenerate/Backgroung/ReportControlBorder/Rectangle2/Label1");
        //BatchMsg.Text = message;
        //BatchMsg.Visible = true;
        Label BatchMsg = Project.Current.Get<Label>("UI/Screens/08Reports/BatchReportGenerate/Backgroung/ReportControlBorder/Rectangle2/MessageText");
        BatchMsg.Text = message;
        BatchMsg.Visible = true;
		if (delayedTask != null)
			delayedTask?.Dispose();
		
		delayedTask = new DelayedTask(DelayedAction, 3000, LogicObject);
		delayedTask.Start();
	}

	private void DelayedAction(DelayedTask task)
	{
		if (task.IsCancellationRequested)
			return;
        
        //Label BatchMsg = Project.Current.Get<Label>("UI/Screens/08Reports/BatchReportGenerate/Backgroung/ReportControlBorder/Rectangle2/Label1");
        //BatchMsg.Text = "";
        //BatchMsg.Visible = false;
        Label BatchMsg = Project.Current.Get<Label>("UI/Screens/08Reports/BatchReportGenerate/Backgroung/ReportControlBorder/Rectangle2/MessageText");
        BatchMsg.Text = "";
        BatchMsg.Visible = false;
		delayedTask?.Dispose();
	}

    private DelayedTask delayedTask;
    //private IUAVariable BatchNoVar;
    
}
