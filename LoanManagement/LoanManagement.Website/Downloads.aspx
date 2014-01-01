<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Downloads.aspx.cs" Inherits="LoanManagement.Website.Downloads" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 47px;
        }
        .auto-style3 {
            width: 181px;
        }
        .auto-style4 {
            width: 206px;
        }
    </style>

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
            <%if(Session["ID"]==null){ %>Welcome Guest!<asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">(login)</asp:LinkButton>
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
            <li><a href="About.aspx">About Us</a></li>
            <li><a href="Services.aspx">Services</a></li>
            <li><a href="Downloads.aspx" class="current">Downloads</a></li>
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
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Downloads"></asp:Label>
                    </h3>
                </div>
                
                <table class="auto-style1">
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td class="auto-style4">
                            <asp:GridView ID="dg" runat="server" AutoGenerateSelectButton="True" Width="364px" CellPadding="4" CellSpacing="2" Font-Size="Medium" ForeColor="#333333" GridLines="None" OnRowCommand="dg_RowCommand" OnSelectedIndexChanged="dg_SelectedIndexChanged">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                            
                            </asp:GridView>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">
                            &nbsp;</td>
                        <td class="auto-style4">
                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td class="auto-style4">
                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style3">&nbsp;</td>
                        <td class="auto-style4">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                
                
                
                
            </div> <!-- end of main column -->
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