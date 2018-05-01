<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TraceLab_UI.aspx.cs" Inherits="TraceLab_UI" %>
     
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TraceLab</title>
   
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous"/>

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css" integrity="sha384-fLW2N01lMqjakBkx3l/M9EahuwpSfeNvV63J5ezn3uZzapT0u7EYsXMjQV+0En5r" crossorigin="anonymous"/>
    <link rel="stylesheet" href="style.css" />
    <script type="text/javascript" src="raphael.js" charset="utf-8"></script>
    <script type="text/javascript" src="GraphBuilder.js" charset ="utf-8" ></script>
    <script type ="text/javascript" >
       
        var el;
        var rpaper;
        //window.onload = function () {
        //    var dragger = function () {
        //        this.ox = this.type == "rect" ? this.attr("x") : this.attr("cx");
        //        this.oy = this.type == "rect" ? this.attr("y") : this.attr("cy");
        //        this.animate({ "fill-opacity": .2 }, 500);
        //    },
        //        move = function (dx, dy) {
        //            var att = this.type == "rect" ? { x: this.ox + dx, y: this.oy + dy } : { cx: this.ox + dx, cy: this.oy + dy };
        //            this.attr(att);
        //            for (var i = connections.length; i--;) {
        //                paper.connection(connections[i]);
        //            }
        //            paper.safari();
        //        },
        //        up = function () {
        //            this.animate({ "fill-opacity": 0 }, 500);
        //        },
        //        paper = Raphael("holder", 640, 480),
        //        connections = [],
        //        shapes = [];
        //    paper.rect(0, 0, 640, 480, 10).attr({ fill: "#ddd", stroke: "none" });
        
            
        //    shapes.push(paper.rect(290, 80, 60, 40, 2));
        //    shapes.push(paper.rect(290, 180, 60, 40, 2));
        //    for (var i = 0, ii = shapes.length; i < ii; i++) {
        //        var color = Raphael.getColor();
        //        shapes[i].attr({ fill: color, stroke: color, "fill-opacity": 0, "stroke-width": 2, cursor: "move" });
        //        shapes[i].drag(move, dragger, up);
        //    }
        //   // paper.text(290+30, 80+20, "Trace");
        //    connections.push(paper.connection(shapes[0], shapes[1], "#000"));
            
        //////};
        ////var shapes = [];
        ////var connections = [];
        ////var text = [];
        ////var ID = [];

        ////        var dragger = function () {
        ////        this.ox = this.type == "rect" ? this.attr("x") : this.attr("cx");
        ////        this.oy = this.type == "rect" ? this.attr("y") : this.attr("cy");

        ////        }
        ////        move = function (dx, dy) {
        ////            var att = this.type == "rect" ? { x: this.ox + dx, y: this.oy + dy } : { cx: this.ox + dx, cy: this.oy + dy };
        ////            this.attr(att);                    
        ////            for (var i = connections.length; i--;) {
        ////                rpaper.connection(connections[i]);
        ////            }
        ////        }
        ////        up = function () {
                    
        ////            }
        ////function LoadExperiment(x) {
        ////    rpaper = Raphael("holder", 1000, 800),
        ////        shapes=[],
        ////        connections = [],
        ////        text = [],
        ////        ID = [];
        ////    rpaper.clear();
        ////    rpaper.rect(0, 0, 1000, 800, 10).attr({ fill: "#eee", stroke: "none" });
            
        ////};
        ////function addNode(x, y, w, l,Text,Identifier) {
        ////    shapes.push(rpaper.rect(x, y, w, l));
        ////    ID.push(Identifier);
        ////    text.push(rpaper.text(x + w / 2, y + l / 2, Text));

        ////                for (var i = 0, ii = shapes.length; i < ii; i++) {
        ////        shapes[i].attr({"stroke-width": 2, cursor: "move" });
        ////        shapes[i].drag(move, dragger, up);
        ////                    text[i].attr({cursor: "move"})
        ////        text[i].drag(move, dragger, up);
        ////    }
        ////}
        ////function addLink(targetID, sourceID)
        ////{
        ////    var i = 0;
        ////    var j = 0;
        ////    while (i < ID.length) {
        ////        if (ID[i] == targetID) {
        ////            break;
        ////        }
        ////        else {
        ////            i++;
        ////        }
        ////    }
        ////    while (j < ID.length) {
        ////        if (ID[j] == sourceID) {
        ////            break;
        ////        }
        ////        else {
        ////            j++;
        ////        }
        ////    }

        ////   connections.push(rpaper.connection(shapes[i], shapes[j], "#000"));

        ////}

        ////function resetNodeHandlers()
        ////{
            
        ////    for (var i = 0, ii = shapes.length; i < ii; i++) {
        ////        shapes[i].attr({ "stroke-width": 2, cursor: "move" });
        ////        shapes[i].drag(move, dragger, up);
        ////        text[i].attr({ cursor: "move" })
        ////        text[i].drag(move, dragger, up);
        ////    }
        ////}

    </script>
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
                    Save File Location:<asp:TextBox ID="SaveText" runat="server" Width="599px" Text ="C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"></asp:TextBox>
 
                    <br />
                    <asp:LinkButton ID="EdgeCommandButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="EdgeCommand"><span class="glyphicon glyphicon-ok""></span></asp:LinkButton>
                    <asp:TextBox ID="EdgeNameText" runat="server" Width="225px" Text =""></asp:TextBox>
                    <asp:TextBox ID="EdgeTargetText" runat="server" Width="225px" Text =""></asp:TextBox>
                    <asp:TextBox ID="EdgeSourceText" runat="server" Width="225px" Text =""></asp:TextBox>

                    <br />

                    <asp:LinkButton ID="DeleteEdgeButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="Delete_Edge"><span class="glyphicon glyphicon-remove""></span></asp:LinkButton>
                    <asp:TextBox ID="DeleteNameText" runat="server" Width="225px" Text =""></asp:TextBox>
                    <asp:TextBox ID="DeleteTargetText" runat="server" Width="225px" Text =""></asp:TextBox>
                    <asp:TextBox ID="DeleteSourceText" runat="server" Width="225px" Text =""></asp:TextBox>
                    <br />


                    <br />
                   <asp:LinkButton ID="NodeButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="AddNode"><span class="glyphicon glyphicon-ok""></span></asp:LinkButton>
                    X: <asp:TextBox ID="txt_nodeX" runat="server" Width="50px" Text =""></asp:TextBox>
                     Y:<asp:TextBox ID="txt_nodeY" runat="server" Width="50px" Text =""></asp:TextBox>
                    
                    ComponentType:<asp:DropDownList ID="ComponentDropDown" runat="server" Width ="400px">

                    </asp:DropDownList>
          <br />


                    <asp:LinkButton ID="DeleteNodeButton" CssClass="btn btn-primary btn-lg" runat="server" OnClick="Delete_Node"><span class="glyphicon glyphicon-remove""></span></asp:LinkButton>
                    <asp:TextBox ID="DeleteNodeText" runat="server" Width="225px" Text =""></asp:TextBox>
                    <br />

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
                            <asp:DropDownList ID="ComponentLabelText" runat ="server" OnSelectedIndexChanged ="GetComponentInfo"></asp:DropDownList>
                            <asp:Button ID="loadSelectedComponent" runat ="server"  Text ="Load Component" OnClick ="GetComponentInfo" />
                            <asp:Button ID="updateSelectedComponent" runat ="server"  Text ="Update Component" OnClick ="GetComponentInfo" />
                            <asp:placeholder ID="ComponentConfig" runat ="server"></asp:placeholder>
                            <asp:Label ID="Compcon" Font-Size="small" runat="server" >

                            </asp:Label>
                            <br />
                            <asp:Button ID="SetSelectedComponent" runat="server" Text ="Set Component" OnClick="SetComponentConfig"  Visible="false"/>
                            <br />
                            
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
                            <div id="holder"></div>
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
