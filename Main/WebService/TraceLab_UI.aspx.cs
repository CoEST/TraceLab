using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TraceLabWeb;

public partial class TraceLab_UI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        Components.Text = app.GetComponents();
        if (!IsPostBack)
        {
            ComponentDropDown.DataSource = app.GetComponentListForDropDown();
            ComponentDropDown.DataTextField = "Label";
            ComponentDropDown.DataValueField = "ID";
            ComponentDropDown.DataBind();
        }
        Workspace.Text = app.GetWorkspace();
    }

    protected void OpenButton_Click(object sender, EventArgs e)
    {
        string directory = OpenDirText.Text;
        var app = TraceLabApplicationWebConsole.Instance;
        app.OpenExperiment(directory);

        Console.Text += app.GetLog();

        ProjectNameText.Text = "";
        AuthorsText.Text = "";
        ContributorsText.Text = "";
        DescriptionText.Text = "";

        Components.Text = app.GetComponents();
        ComponentDropDown.DataSource = app.GetComponentListForDropDown();
        ComponentDropDown.DataTextField = "Label";
        ComponentDropDown.DataValueField = "ID";
        ComponentDropDown.DataBind();
        Workspace.Text = app.GetWorkspace();
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        app.SaveExperiment (SaveText.Text);
        Console.Text = app.GetLog();
        Components.Text = app.GetComponents();
        ComponentDropDown.DataSource = app.GetComponentListForDropDown();
        ComponentDropDown.DataTextField = "Label";
        ComponentDropDown.DataValueField = "ID";
        ComponentDropDown.DataBind();
        Workspace.Text = app.GetWorkspace();
    }

    protected void Log_Click(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        Console.Text = app.GetLog();
        Components.Text = app.GetComponents();
        Workspace.Text = app.GetWorkspace();
        ComponentList.Text = app.GetNodes();
        ComponentDropDown.DataSource = app.GetComponentListForDropDown();
        ComponentDropDown.DataTextField = "Label";
        ComponentDropDown.DataValueField = "ID";
        ComponentDropDown.DataBind();
    }

    protected void Run_Click(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        app.RunExperiment();
        app.ClearLog();
     
        Components.Text = app.GetComponents();
        Workspace.Text = app.GetWorkspace();
        //   running_refresh();
        runningTimer.Enabled = true;
        ComponentDropDown.DataSource = app.GetComponentListForDropDown();
        ComponentDropDown.DataTextField = "Label";
        ComponentDropDown.DataValueField = "ID";
        ComponentDropDown.DataBind();
        
    }

    protected void EdgeCommand(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        app.AddEdge(EdgeNameText .Text,EdgeSourceText.Text,EdgeTargetText.Text  );
    }

    protected void AddNode(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        int x = 0;
        int y = 0;
        Int32.TryParse(txt_nodeX.Text,out x);
        Int32.TryParse(txt_nodeY.Text, out y);
        app.AddNode(ComponentDropDown.SelectedItem.Value , x, y);
        
    }

    public void running_refresh(object sender, EventArgs e)
    {
        try
        { 
            Console.Text = TraceLabApplicationWebConsole.Instance.GetLogUntouched();
            if (TraceLabApplicationWebConsole.Instance.GetLogUntouched().ToUpper().Contains ("DONE"))
            {
                Console.Text = TraceLabApplicationWebConsole.Instance.GetLogUntouched();
                runningTimer.Enabled = false;
            }
        }
        catch(Exception ex)
        {
            Console.Text = ex.Message;
            runningTimer.Enabled = false;
        }
    }
}