<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TraceLab_UI.aspx.cs" Inherits="TraceLab_UI" %>
     
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TraceLab</title>
   
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css" integrity="sha384-fLW2N01lMqjakBkx3l/M9EahuwpSfeNvV63J5ezn3uZzapT0u7EYsXMjQV+0En5r" crossorigin="anonymous">
    <link rel="stylesheet" href="style.css" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
              <!-- Static navbar -->
          <nav class="navbar navbar-default">
            <div class="container-fluid">
              <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                  <span class="sr-only">Toggle navigation</span>
                  <span class="icon-bar"></span>
                  <span class="icon-bar"></span>
                  <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#">Trace Lab</a>
              </div>
              <div id="navbar" class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                  <li class="active"><a href="#">Home</a></li>
                  <li><a href="#">Projects</a></li>
                  <li><a href="#">Help</a></li>
                </ul>
              </div><!--/.nav-collapse -->
            </div><!--/.container-fluid -->
          </nav>

         <div class="row">
                <div class="col-md-8" >   
                    <div class="progress hidden">
                      <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: 45%">
                        <span class="sr-only">45% Complete</span>
                      </div>
                    </div>  

                     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      
                    <asp:Timer ID="runningTimer" runat="server" Interval="5" OnTick="running_refresh" Enabled ="False" />
                    <asp:LinkButton ID="Run" CssClass="btn btn-primary btn-lg" runat="server" OnClick="Run_Click"><span class="glyphicon glyphicon-play"></span></asp:LinkButton>
                    <asp:LinkButton ID="Stop" CssClass="btn btn-primary btn-lg" runat="server" ><span class="glyphicon glyphicon-stop"></span></asp:LinkButton>
                    <asp:LinkButton ID="ReloadLog" CssClass="btn btn-primary btn-lg" runat="server" OnClick="Log_Click"><span class="glyphicon glyphicon-refresh"></span></asp:LinkButton>
                    <asp:LinkButton ID="OpenButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="OpenButton_Click"><span class="glyphicon glyphicon-folder-open""></span></asp:LinkButton>
                    <asp:TextBox ID="OpenDirText" runat="server" Width="799px" Text ="C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"></asp:TextBox>

                    <br />

                    <asp:LinkButton ID="SaveButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="Save_Click"><span class="glyphicon glyphicon-saved""></span></asp:LinkButton>
                    Save File Location:<asp:TextBox ID="SaveText" runat="server" Width="799px" Text ="C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"></asp:TextBox>
 
                    <br />
                    <asp:LinkButton ID="EdgeCommandButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="EdgeCommand"><span class="glyphicon glyphicon-ok""></span></asp:LinkButton>
                    <asp:TextBox ID="EdgeNameText" runat="server" Width="299px" Text =""></asp:TextBox>
                    <asp:TextBox ID="EdgeTargetText" runat="server" Width="299px" Text =""></asp:TextBox>
                    <asp:TextBox ID="EdgeSourceText" runat="server" Width="299px" Text =""></asp:TextBox>

                    <br />
                   <asp:LinkButton ID="NodeButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="AddNode"><span class="glyphicon glyphicon-ok""></span></asp:LinkButton>
                    X: <asp:TextBox ID="txt_nodeX" runat="server" Width="50px" Text =""></asp:TextBox>
                     Y:<asp:TextBox ID="txt_nodeY" runat="server" Width="50px" Text =""></asp:TextBox>
                    
                    ComponentType:<asp:DropDownList ID="ComponentDropDown" runat="server" Width ="400px">

                    </asp:DropDownList>
          <br />
                    <asp:LinkButton ID="RemoveNodeButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="AddNode"><span class="glyphicon glyphicon-minus""></span></asp:LinkButton>
                   Label of node to remove:<asp:TextBox ID="RemoveNodeText" runat="server" Width="799px" Text =""></asp:TextBox>
  <br />
         
                    <asp:LinkButton ID="RemoveEdgeButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="AddNode"><span class="glyphicon glyphicon-minus""></span></asp:LinkButton>
                   Label of edge to remove:<asp:TextBox ID="RemoveEdgeText" runat="server" Width="799px" Text =""></asp:TextBox>

                    </div>

             <div class="col-md-4" style="height:200px">              
                    <div class="panel panel-primary fill-body-double" style="min-height :275px">
                        <div class="panel-heading">Experiment Info</div>
                        <div class="panel-body" style="min-height :90px">
                            <asp:Label ID="ExperimentInfo" runat="server" Text="ExperimentInfo: <br/>"></asp:Label>
                            <asp:Label ID="Info" runat="server" Text=""></asp:Label>
                              Project Name:<asp:TextBox ID="ProjectNameText" runat="server" Width="200px" Text =""></asp:TextBox>
                            <br />
                              Author(s)   :<asp:TextBox ID="AuthorsText" runat="server" Width="200px" Text =""></asp:TextBox>
                            <br />
                              Contributors:<asp:TextBox ID="ContributorsText" runat="server" Width="200px" Text =""></asp:TextBox>
                            <br />
                              Description:<asp:TextBox ID="DescriptionText" runat="server" Width="200px" Height ="50px" Text ="" TextMode="MultiLine"></asp:TextBox>
                            
                        </div>
                    </div>
                </div>
                

            </div><!-- Buttons for Start and Stop-->

            <div class="row top-buffer"> 
                <div class="col-md-4" >
                    <div class="panel panel-primary fill-body">
                        <div class="panel-heading">Components</div>
                        
                        <div class="panel-body">
                            <asp:PlaceHolder ID="ComponentConfig" runat ="server"></asp:PlaceHolder>
                            <asp:Label ID="Components" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="panel panel-primary fill-body">
                        <div class="panel-heading">Workspace</div>
                        <div class="panel-body">
                            <asp:Label ID="Workspace" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-md-8 " >              
                    <div class="panel panel-primary fill-body-double">
                        <div class="panel-heading">Graph</div>
                        <div class="panel-body">
                            <asp:Label ID="Labelconsole" runat="server" Text="Console: <br/>"></asp:Label>
                            <asp:Label ID="Console" runat="server" Text=""></asp:Label>

                        </div>
                    </div>
                </div>
                
                <div class="col-md-8 " >              
                    <div class="panel panel-primary fill-body-double">
                        <div class="panel-heading">Component List</div>
                        <div class="panel-body">
                            <asp:Label ID="LabelList" runat="server" Text="Components: <br/>"></asp:Label>
                            <asp:Label ID="ComponentList" runat="server" Text=""></asp:Label>

                        </div>
                    </div>
                </div>
            </div><!-- Main workspace-->
           
        <div class ="row">

        </div>
            
            <div class="row"> 
                <div class="col-md-12 well" >
                    <div class="alert alert-success " role="alert"><span class="glyphicon glyphicon-remove" onclick="hide(this)" aria-hidden="true">  Success!! </div>
                    <div class="alert alert-info " role="alert"><span class="glyphicon glyphicon-remove" onclick="hide(this)" aria-hidden="true"> Info!!       </div>
                    <div class="alert alert-warning " role="alert"><span class="glyphicon glyphicon-remove" onclick="hide(this)" aria-hidden="true"> Warning!! </div>
                    <div class="alert alert-danger " role="alert"><span class="glyphicon glyphicon-remove" onclick="hide(this)" aria-hidden="true"> Error!!    </div>
                </div>
            </div> <!-- Console -->
           
        </div>
    </form>

    <script type="text/javascript" src="tracelab_UI.js"></script>
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS" crossorigin="anonymous"></script>
</body>
</html>
