<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyAccount_Submit.aspx.cs" Inherits="LoanManagement.Website.MyAccount_Submit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .auto-style1 {
            width: 100%;
            height: 428px;
        }
        .auto-style2 {
            width: 29px;
        }
        .auto-style3 {
            width: 210px;
        }
        .auto-style4 {
            width: 29px;
            height: 24px;
        }
        .auto-style5 {
            width: 210px;
            height: 24px;
        }
        .auto-style6 {
            height: 24px;
        }
        .auto-style7 {
            width: 100%;
            height: 381px;
        }
        .auto-style8 {
            height: 6px;
        }
        .auto-style9 {
            height: 344px;
        }
        .auto-style10 {
            width: 100%;
            height: 353px;
        }
        .auto-style11 {
            height: 239px;
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
    	
        
        
      
    	
    </div> <!-- end of templatemo_banner_bar -->
    <div id="templatemo_menu">
    	<ul>
            <li><a href="Index.aspx">Home</a></li>
            <li><a href="About.aspx">About Us</a></li>
            <li><a href="Services.aspx">Services</a></li>
            <li><a href="Downloads.aspx">Downloads</a></li>
            <li><a href="Application.aspx">Application</a></li>
            <li><a href="Calculator.aspx">Loan Calc.</a></li>
                <%
                if(Session["ID"]==null) {%>
                    <li><a href="Login.aspx">Login/Register</a></li>
                        <% }else {%>
                    <li><a href="MyAccount_Main.aspx" class="current">My Account</a></li>
                        <% } %>
        </ul>
    </div> <!-- end of menu -->
    
    <div id="templatemo_content_wrapper">
    
    	<div id="templatemo_content">
        
            <div id="main_column">
            
                <div class="section_w590">
                    <h3>
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="My Page"></asp:Label>
                    </h3>
                    <table class="auto-style1">
                        <tr>
                            <td class="auto-style4"></td>
                            <td class="auto-style5">
                                <asp:Panel ID="Panel1" runat="server" Height="26px">
                                    <div id="menu">
    	                                <ul>
                                            <li><a href="MyAccount_Main.aspx">My Profile</a></li>
                                            <li><a href="MyAccount_Edit.aspx">Edit Account</a></li>
                                            <li><a href="MyAccount_Loans.aspx">View/Update Loans</a></li>
                                            <li><a href="MyAccount_Submit.aspx" class="current">Submit Requirements</a></li>
                                        </ul>
                                    </div> <!-- end of menu -->
                                </asp:Panel>
                            </td>
                            <td class="auto-style6">
                                <asp:Panel ID="Panel2" runat="server">
                                </asp:Panel>
                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Submit Requirements"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2">&nbsp;</td>
                            <td class="auto-style3">
                                &nbsp;</td>
                            <td>
                                <table class="auto-style7">
                                    <tr>
                                        <td class="auto-style8">
                                                        <asp:Label ID="lblCheck" runat="server" Font-Size="Large" ForeColor="Red" Text="You currently don't have any applied loans" Visible="False"></asp:Label>
                                                    </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style9">
                                            <table class="auto-style10">
                                                <tr>
                                                    <td class="auto-style11">
                                                        <asp:Panel ID="pnlHidden" runat="server" Height="235px" Visible="False">
                                                            <asp:GridView ID="dg" runat="server" AutoGenerateSelectButton="True" CellPadding="4" CellSpacing="2" ForeColor="#333333" GridLines="None" Width="634px">
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
                                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="111px" OnClick="btnSubmit_Click" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                
                
                
                
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
            <a href="Index.aspx">Home</a> | <a href="About.aspx">About</a> | <a href="Services.aspx">Services</a> | <a href="Downloads.aspx">Downloands</a> | <a href="Application.aspx">Application</a> | <a href="Calculator.aspx">Calculator</a> | <a href ="RegisterClient.aspx">Client Registration</a> | <a href="RegisterUser.aspx">Register User</a> | <a href="Login.aspx">Login</a> | <a href="MyAccount_Main.aspx">My Account</a> | <a href="MyAccount_Edit.aspx">Edit Account</a> | <a href="MyAccount_Loans.aspx">My Loans</a> | <a href="MyAccount_Submit.aspx">Submit Requirements</a>
        </div> <!-- end of footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>
