<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<%@ page import="smos.bean.*" %>
<%@ page import="smos.storage.ManagerUser"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib prefix="display" uri="http://displaytag.sf.net" %>
<%@ page import="smos.utility.Utility"%>

<%
	User loggedUser = (User) session.getAttribute("loggedUser");
	if ((loggedUser == null) || ((!ManagerUser.getInstance().isAdministrator(loggedUser)))) {
		response.sendRedirect("../index.htm");
		return;
	}
%>

<html>
<head>

	<title>School MOnitoring System</title>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" >
	<link rel="stylesheet" href="../../stylesheets/generic.css" type="text/css">
	<link rel="stylesheet" href="../../stylesheets/display.css" type="text/css">
</head>

<body>
		
	  <div id="container">
	  
	  	   <div id="header">
		   	    
			</div>
		   	      
			<div id="content_center" style="overflow-y:scroll;">
		    
            <div id="navigation">
           <a class="genericLink" href="<% if (ManagerUser.getInstance().isAdministrator(loggedUser)){%>../../homePage/homeAdmin.jsp<%}%>">Home</a> -> <font style="color:#0066FF; font-size:9pt">Gestione Utenti</font>
           </div>
            
            <div id="userspace">
				 User: <b><%=loggedUser.getFirstName() + " " + loggedUser.getLastName()%></b><br>
                 [<a style="font-size:11px" class="genericLink" href="../../logout">Esci</a>, <a style="font-size:11px" class="genericLink" href="../userManagement/alterPersonalDate.jsp">Modifica Password</a>]
			</div>
           
		  

		   <h1 align="left"> GESTIONE UTENTI </h1>

           <table id="transparent" align="center">
           <tr id="transparent">
           <td id="transparent">
		<% if (ManagerUser.getInstance().isAdministrator(loggedUser)) {%>
           <p class="button"><a class="menuLink" href="../userManagement/insertUser.jsp">Nuovo Utente</a></p>
		<%}%>
           </td>
           </tr>
           </table>
		   
		   <br>
		   <display:table name="sessionScope.userList" pagesize="15" sort="list" id="user" defaultsort="1" class="datatable" export="true">
				<display:setProperty name="basic.msg.empty_list">
					<caption>
						<p>Non ci sono utenti da visualizzare</p>
					</caption>
				</display:setProperty>
				
    			<display:setProperty name="export.pdf" value="true"/>				
				<display:column property="name" title="Nome" style="width:200px" sortable="true" headerClass="tdHeaderColumn"/>
				<display:column property="EMail" style="width:200px" title="e-mail" sortable="true" headerClass="tdHeaderColumn"/>	
				
				<display:column style="width:25px" title="Visualizza" sortable="false" headerClass="tdHeaderColumn"> 
				<%  if (((UserListItem)user).getId()!=0) { %>
					<a href="../../showUserDetails?userId=<%=((UserListItem)user).getId()%>"><img src="../../images/details.jpg" border="none"></a>
				<% } %>
				</display:column>
	       	</display:table>
				
		   </div>
		   
		   <div id="footer">
		   		<p><%= Utility.getTextFooter() %></p>
		   </div>
	  
	  </div>

</body>
</html>