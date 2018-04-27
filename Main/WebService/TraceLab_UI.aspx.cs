using System;
using System.Collections.Generic;
using System.Data;
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
        try
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
            ComponentLabelText.DataSource = app.GetNodesForDropdown();
            ComponentLabelText.DataTextField = "Label";
            ComponentLabelText.DataValueField = "ID";
            ComponentLabelText.DataBind();
            Workspace.Text = app.GetWorkspace();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallFunction", "LoadExperiment(50);", true);
            buildGraph();
        }
        catch (Exception ex)
        {
            Workspace.Text = "Error";
        }
        //TODO reload the raphael paper when project is opened


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

    protected void Delete_Edge(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        app.Delete_Edge(DeleteNameText.Text, DeleteSourceText.Text, DeleteTargetText.Text);
    }

    protected void AddNode(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        int x = 0;
        int y = 0;
        Int32.TryParse(txt_nodeX.Text,out x);
        Int32.TryParse(txt_nodeY.Text, out y);
        app.AddNode(ComponentDropDown.SelectedItem.Value , x, y);
        //TODO add a node to raphael paper when node is added

    }

    protected void Delete_Node(object sender, EventArgs e)
    {
        var app = TraceLabApplicationWebConsole.Instance;
        app.Delete_Node(DeleteNodeText.Text);
        //TODO remove node from raphael paper when node is removed
    }


    protected void GetComponentInfo(object sender,EventArgs e)
    {
        ComponentConfig.Controls.Clear();//Remove any controls that already exist
        if (ComponentLabelText .Items.Count >0)
        {
            var app = TraceLabApplicationWebConsole.Instance;
            string comps="";//= app.GetComponentConfigInfo(ComponentLabelText.SelectedValue);
            DataTable dt = app.GetComponentConfigInfo (ComponentLabelText .SelectedValue );
       
            string[] compControls = comps.Split(';');

            foreach (DataRow drow in dt.Rows)
            {
                if (drow["Type"].Equals("TraceLabSDK.Types.TLArtifactsCollection") ||
                    drow["Type"].Equals("TraceLabSDK.Component.Config.FilePath") ||
                    drow["Type"].Equals("System.String") ||
                    drow["Type"].Equals("System.Int32"))
                {
                    ComponentConfig.Controls.Add(new LiteralControl("<br/>"));
                    Label newLabel = new Label();
                    newLabel.Text = drow["Section"].ToString()  + drow["Name"].ToString() ;
                    ComponentConfig.Controls.Add(newLabel);
                    TextBox newControl = new TextBox();
                    newControl.ID = drow["Section"].ToString () + "/" + drow["Name"].ToString ();
                    newControl.Text = drow["Value"].ToString ();
                    ComponentConfig.Controls.Add(newControl);

                }
                else if (drow["Type"].Equals("System.Boolean"))
                {
                    ComponentConfig.Controls.Add(new LiteralControl("<br/>"));
                    Label newLabel = new Label();
                    newLabel.Text = drow["Section"].ToString() + drow["Name"].ToString();
                    ComponentConfig.Controls.Add(newLabel);
                    CheckBox newControl = new CheckBox();
                    newControl.ID = drow["Section"].ToString() + "/" + drow["Name"].ToString();
                    if (drow["Value"].Equals(true))
                    {
                        newControl.Checked = true;
                    }
                    else
                    {
                        newControl.Checked = false;
                    }
                    ComponentConfig.Controls.Add(newControl);

                }
                else
                {

                    Label newControl = new Label();
                    newControl.ID = drow[2].ToString ();
                    newControl.Text = drow["Type"].ToString ();
                    ComponentConfig.Controls.Add(new LiteralControl("<br/>"));
                    ComponentConfig.Controls.Add(newControl);
                }
            }
            
        }

        //Add controls that that component would have

    }

    protected void UpdateComponentInfo(object sender,EventArgs e)
    {
       String updatedComponent = "";
        DataTable updatedDT = new DataTable();
        var app = TraceLabApplicationWebConsole.Instance;

        updatedDT.Columns.Add("Section");
        updatedDT.Columns.Add("Name");
        updatedDT.Columns.Add("Value");

        foreach (TextBox  tex in ComponentConfig.Controls.OfType<TextBox>())
        {
            updatedComponent += tex.ID + ":" + tex.Text+";";
            DataRow drow = updatedDT.NewRow();
            drow["Section"] = tex.ID.Split('/')[0];
            drow["Name"] = tex.ID.Split('/')[1];
            drow["Value"] = tex.Text;
        }
        foreach (CheckBox chk in ComponentConfig.Controls.OfType<CheckBox >())
        {
            updatedComponent += chk.ID + ":" + chk.Text + ";";
            DataRow drow = updatedDT.NewRow();
            drow["Section"] = chk.ID.Split('/')[0];
            drow["Name"] = chk.ID.Split('/')[1];
            drow["Value"] = chk.Checked ;

        }

        app.UpdateComponentConfigInfo(ComponentDropDown.SelectedValue, updatedDT);
        

        //run the update
    }

    protected void MoveNode(object sender, EventArgs e)
    {
        //TODO handle event when node is moved on raphael paper
        var app = TraceLabApplicationWebConsole.Instance;
        //app.MoveNode(nodeName,x,y);
    }

    protected void SetComponentConfig(object sender, EventArgs e)
    {

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

    public void buildGraph()
    {
        DataTable dt = new DataTable();
        var app = TraceLabApplicationWebConsole.Instance;

        dt = app.GetNodesForDropdown ();
        foreach(DataRow drow in dt.Rows)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "NodeFunction"+Guid .NewGuid (), "addNode("+drow["X"]+","+drow["Y"]+",150,40,'"+drow["Label"].ToString () +"','"+drow["ID"].ToString ()+"');", true);            
        }

        DataTable linkdt = new DataTable();

        linkdt = app.GetEdges ();
        foreach (DataRow drow in linkdt.Rows )
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "EdgeFunction" + Guid.NewGuid(), "addLink('" + drow["source"] + "','" + drow["target"]+ "');", true);
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), "resetHandlers" , "resetNodeHandlers();", true);
    }

}