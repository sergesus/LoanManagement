<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="LoanManagement.Website.About" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />

</head>
<body>
<form id="Form1" runat="server">
<div id="templatemo_container">
	<div id="templatemo_site_title_bar">
        <div id="site_title">
            <h1>
                <a href="#">GFC</a>
            </h1>
        </div>
        
        <div id="top_menu">
            <%if(Session["ID"]==null){ %>
                Welcome Guest!<asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">(login)</asp:LinkButton>
&nbsp;<%}else { %>Welcome <% string id = Session["NAME"].ToString(); Response.Write(id); %> 
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">(Logout)</asp:LinkButton>
            <%} %>
        </div>
    
    </div> <!-- templatemo_site_title_bar -->
    
    <div id="templatemo_banner_bar">
    	
        <h2>Guahan Finance Corporation</h2>
        
      
    	
    </div> <!-- end of templatemo_banner_bar -->
    <div id="templatemo_menu">
    	<ul>
            <li><a href="Index.aspx">Home</a></li>
            <li><a href="About.aspx" class="current">About Us</a></li>
            <li><a href="Services.aspx">Services</a></li>
            <li><a href="Downloads.aspx">Downloads</a></li>
            <li><a href="Application.aspx">Application</a></li>
            <li><a href="Calculator.aspx">Loan Calc.</a></li>
            <%
                if(Session["ID"]==null) {%>
                    <li><a href="Login.aspx">Login/Register</a></li>
            <% }else {%>
                    <li><a href="#">My Account</a></li>
            <% } %>
        </ul>
    </div> <!-- end of menu -->
    
    <div id="templatemo_content_wrapper">
    
    	<div id="templatemo_content">
        
            <div id="main_column">
            
                <div class="section_w590">
                    <h3>
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="About Us"></asp:Label>
                    </h3>
                    <p>
                        <asp:Label ID="Label3" runat="server" Text="A group of enterprising executives belonging to various industries saw the opportunity to venture in the booming consumer financing business. Combining their strengths, Robert B. Jordan handled credit; Ruben Y. Lugtu Jr. labored on the financials while the late Wilfredo T. Anastacio took care of the human resources and operations. With the good demand and sound credit, the company, which they formed, Asialink Finance Corporation was off to a good start. Established in June of 1997, Asialink Finance Corporation is now the leading and fastest growing finance company in the Philippines. " Font-Size="Large"></asp:Label>
                    </p>
                    
                     &nbsp;<h4><p>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Our Mission - Vision"></asp:Label>
                    </p>
                    <p>&nbsp;</p>
                    <p>
                        <asp:Label ID="Label2" runat="server" Text="Asialink Finance Corporation strives to deliver fast, convenient, efficient and accessible service to businesses and individuals in need of financial assistance.   It constantly pushes to attain a competitive edge in the financing industry through raising the standards of quality and integrity in its services, thus gaining a greater market share and showing attractive returns on the investment of its stakeholders.  It also prioritizes the welfare of its workforce through giving fair wages, benefits, and incentives, thus fostering a healthy and goal-oriented working environment.  It envisions itself to be the best in the Philippines and a significant player in the rest of Southeast Asia in the financing industry by the year 2015." Font-Size="Medium"></asp:Label>
                    </p>
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
		
        </div>  <!-- end of footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>
