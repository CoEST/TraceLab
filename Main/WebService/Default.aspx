<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Default</title>
    <style type="text/css">
        #form1 {
            height: 167px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <div>
        <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
    </div>
       <asp:TextBox ID="TextBox1" runat="server" Text="C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml" Width="663px">C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml</asp:TextBox>
        
    <div>
        
    
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Display Help" />

        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </div> 
    <div>
       

        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Open File" />

         <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    </div>

    <div>
       

        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Run Experiment" />

         <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
    </div>

    <div>
       

        <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Get Log" />

         <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
    </div>
        
    </form>
</body>
</html>
