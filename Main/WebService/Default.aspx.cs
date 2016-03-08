using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TraceLab.Core;
using TraceLabWeb;

public partial class _Default : System.Web.UI.Page
{
    TraceLabApplicationWebConsole app;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //WebConsoleAdapter.Run();
        string[] args = { };
        app = new TraceLabApplicationWebConsole();
        app.Run(args);
        app.RunThis();
        app.DisplayHelp();

        Label2.Text = app.GetLog();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        app.DisplayHelp();
        Label2.Text = app.GetLog();
    }

}