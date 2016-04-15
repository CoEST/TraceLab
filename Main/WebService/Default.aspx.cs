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
    protected void Page_Load(object sender, EventArgs e)
    {
       // var app = TraceLabApplicationWebConsole.Instance;
     //   Label5.Text = app.GetLog();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //WebConsoleAdapter.Run();
        
      //  var app = TraceLabApplicationWebConsole.Instance;        
       // app.DisplayHelp();

       // Label1.Text = app.GetLog();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
    //    var app = TraceLabApplicationWebConsole.Instance;
    //    app.OpenExperiment(TextBox1.Text);
        //app.DisplayHelp();
      //  Label2.Text = app.GetLog();
    }


    protected void Button3_Click(object sender, EventArgs e)
    {
    //    var app = TraceLabApplicationWebConsole.Instance;
      //  app.RunExperiment();
        //app.DisplayHelp();
       // Label2.Text = app.GetLog();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
    //    var app = TraceLabApplicationWebConsole.Instance;
        
       // Label2.Text = app.GetLog();
    }
}