<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LoanManagement.Website.wfIndex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />

</head>
<body>
<form runat="server">
<div id="templatemo_container">
	<div id="templatemo_site_title_bar">
        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick">
        </asp:Timer>
        <div id="site_title">
            <h1>
                <a href="#">GFC</a>
            </h1>
        </div>
        
        <div id="top_menu">
            <%if(Session["ID"]==null){ %>Welcome Guest!<asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">(login)</asp:LinkButton>
&nbsp;<%}else { %>Welcome <% string id = Session["NAME"].ToString(); Response.Write(id); %> 
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">(Logout)</asp:LinkButton>
            <%} %>
            <br />
            <br />
        </div>
    
    </div> <!-- templatemo_site_title_bar -->
    
    <div id="templatemo_banner_bar">
    	
        
        
      
    	
    </div> <!-- end of templatemo_banner_bar -->
    <div id="templatemo_menu">
    	<ul>
            <li><a href="Index.aspx" class="current">Home</a></li>
            <li><a href="About.aspx">About/Contact </a></li>
            <li><a href="Services.aspx">Services</a></li>
            <li><a href="Downloads.aspx">Downloads</a></li>
            <li><a href="Application.aspx">Application</a></li>
            <li><a href="Calculator.aspx">Loan Calc.</a></li>
                <%
                if(Session["ID"]==null) {%>
                    <li><a href="Login.aspx">Login/Register</a></li>
                        <% }else {%>
                    <li><a href="MyAccount_Main.aspx">My Account</a></li>
                        <% } %>
        </ul>
    </div> <!-- end of menu -->
    
    <div id="templatemo_content_wrapper">
    
    	<div id="templatemo_content">
        
            <div id="main_column">
            
                <div class="section_w590">
                    <h3>
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Welcome to our website!"></asp:Label>
                    </h3>
                    
                     &nbsp;<h4>
                        <asp:Label ID="lblDesc" runat="server" Text="-"></asp:Label>
                    </h4>
                    
                <div class="cleaner"></div>
                </div>
                
                <div class="cleaner_h30"></div>
                
                
                
                
            </div> <!-- end of main column -->
            <div class="cleaner"></div>
             <!-- end of side column -->
            
            <div class="divider"> </div>
        </div> <!-- end of content -->
    
    	
    </div> <!-- end of templatmeo_content_wrapper -->
    
    <div id="templatemo_footer_bar_wrapper">
        <div id="templatemo_footer_bar">
                Copyright © 2013 | Guahan Financing Corporation | All Rights Reserved
		<br />
            <a href="Index.aspx">Home</a> | <a href="About.aspx">About/Contact</a> | <a href="Services.aspx">Services</a> | <a href="Downloads.aspx">Downloands</a> | <a href="Application.aspx">Application</a> | <a href="Calculator.aspx">Calculator</a> | <a href ="RegisterClient.aspx">Client Registration</a> | <a href="RegisterUser.aspx">Register User</a> | <a href="Login.aspx">Login</a> | <a href="MyAccount_Main.aspx">My Account</a> | <a href="MyAccount_Edit.aspx">Edit Account</a> <br /> <a href="MyAccount_Loans.aspx">My Loans</a> | <a href="MyAccount_Submit.aspx">Submit Requirements</a><br /><asp:Label ID="lblTime" runat="server" Text="-"></asp:Label>&nbsp;| Visitor No. : <asp:Label ID="lblVisitor" runat="server" Text="-"></asp:Label>
        </div>  <!-- end of footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>
