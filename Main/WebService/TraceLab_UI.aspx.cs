using System;
using System.Collections.Generic;
using System.Linq;
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
        Workspace.Text = app.GetWorkspace();
    }

    protected void OpenButton_Click(object sender, EventArgs e)
    {
        string directory = OpenDirText.Text;
        var app = TraceLabApplicationWebConsole.Instance;
        app.OpenExperiment(directory);

        Console.Text += app.GetLog();
        Components.Text = app.GetComponents();
        Workspace.Text = app.GetWorkspace();
    }

    protected void Log_Click(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        Console.Text = app.GetLog();
        Components.Text = app.GetComponents();
        Workspace.Text = app.GetWorkspace();
        ComponentList.Text = app.GetNodes();
    }

    protected void Run_Click(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        app.RunExperiment();
        Console.Text = app.GetLog();
        Components.Text = app.GetComponents();
        Workspace.Text = app.GetWorkspace();
    }

    protected void EdgeCommand(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        app.AddEdge(EdgeCommandText.Text );
    }
}