﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TraceLabConsole;


public partial class TraceLab_UI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string[] args = { };
        var app = new TraceLabConsole.TraceLabApplicationConsole();
        app.Run(args);

        String Command = "open: " + "C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml";
        Adaptor.RunCommand("Open", Command);
        Adaptor.RunCommand("Run", "run");

        //var app = TraceLabApplicationWebConsole.Instance; 
        //Components.Text = app.GetComponents();
        //Workspace.Text = app.GetWorkspace();
    }

    protected void OpenButton_Click(object sender, EventArgs e)
    {
  
        /*
        string directory = OpenDirText.Text;
        var app = TraceLabApplicationWebConsole.Instance;
        app.OpenExperiment(directory);

        Console.Text += app.GetLog();
        Components.Text = app.GetComponents();
        Workspace.Text = app.GetWorkspace();
         */
    }

    protected void Log_Click(object sender, EventArgs e)
    {
        /*
        var app = TraceLabApplicationWebConsole.Instance;
        Console.Text = app.GetLog();
        Components.Text += app.GetComponents();
        Workspace.Text = app.GetWorkspace();
         * */
    }

    protected void Run_Click(object sender, EventArgs e)
    {
        
        /*
        var app = TraceLabApplicationWebConsole.Instance;
        app.RunExperiment();
        Console.Text = app.GetLog();
        Components.Text += app.GetComponents();
        Workspace.Text = app.GetWorkspace();
         */
    }
}