#region Using directives
using System;
using FTOptix.HMIProject;
using UAManagedCore;
using FTOptix.NetLogic;
using FTOptix.UI;
using System.Linq;
using FTOptix.WebUI;
using FTOptix.Recipe;
using FTOptix.ODBCStore;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
using FTOptix.MelsecFX3U;
#endregion

public class DeleteUserButtonLogic : BaseNetLogic
{
    [ExportMethod]
    public void DeleteUser(NodeId userToDelete)
    {
        var userObjectToRemove = InformationModel.Get(userToDelete);
        if (userObjectToRemove == null)
        {
            Log.Error("UserEditor", "Cannot obtain the selected user.");
            return;
        }
        
        var userVariable = Owner.Owner.Owner.Owner.GetVariable("Users");
        if (userVariable == null)
        {
            Log.Error("UserEditor", "Missing user variable in UserEditor Panel.");
            return;
        }

        if (userVariable.Value == null || (NodeId)userVariable.Value == NodeId.Empty)
        {
            Log.Error("UserEditor", "Fill User variable in UserEditor.");
            return;
        }
        var usersFolder = InformationModel.Get(userVariable.Value);
        if (usersFolder == null)
        {
            Log.Error("UserEditor", "Cannot obtain Users folder.");
            return;
        }
        Log.Info("UserEditor","User to be deleted: " + userObjectToRemove.BrowseName + "Curr User: " + Session.User.BrowseName);
        
        if (userObjectToRemove.BrowseName != Session.User.BrowseName)
        {
            usersFolder.Remove(userObjectToRemove);
            //-----------Customized Logic Start-----------------
            // Delete User Activity Logging into Audit Database
            AuditTrailLogging DeleteUser = new AuditTrailLogging();
            DeleteUser.LogIntoAudit("User deleted", userObjectToRemove.BrowseName, Session.User.BrowseName, "UserDeleteEvent");
            //-----------Customized Logic End-------------------
            ShowMessage("User deleted successfully");
        }
        else
        {
            ShowMessage("LoggedIn user can not be deleted");
            Log.Error("DeleteUserButtonLogic", "LoggedIn user can not be deleted");
        }
        
        if (usersFolder.Children.Count > 0)
        {
            var usersList = (ListBox)Owner.Owner.Owner.Get<ListBox>("HorizontalLayout1/UsersList");
            usersList.SelectedItem = usersFolder.Children.First().NodeId;
        }
    }


    private void ShowMessage(string message)
	{
		Label DeleteResult = Project.Current.Get<Label>("UI/NewUserEditor/EditUserDetailPanel/HorizontalLayout1/Details/DeleteUserMessage");
        DeleteResult.Text = message;
        DeleteResult.Visible = true;
		if (delayedTask != null)
			delayedTask?.Dispose();
		
		delayedTask = new DelayedTask(DelayedAction, 5000, LogicObject);
		delayedTask.Start();
	}

	private void DelayedAction(DelayedTask task)
	{
		if (task.IsCancellationRequested)
			return;
        
        Label DeleteResult = Project.Current.Get<Label>("UI/NewUserEditor/EditUserDetailPanel/HorizontalLayout1/Details/DeleteUserMessage");
        DeleteResult.Text = "";
        DeleteResult.Visible = false;
		delayedTask?.Dispose();
	}

    private DelayedTask delayedTask;

}
