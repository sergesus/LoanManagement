<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyAccount_Edit.aspx.cs" Inherits="LoanManagement.Website.MyAccount_Edit" %>

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
        }
        .auto-style10 {
            width: 2px;
        }
        .auto-style11 {
            width: 154px;
        }
        .auto-style12 {
            width: 377px;
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
                                            <li><a href="MyAccount_Edit.aspx" class="current">Edit Account</a></li>
                                            <li><a href="MyAccount_Loans.aspx">View/Update Loans</a></li>
                                            <li><a href="MyAccount_Submit.aspx">Submit Requirements</a></li>
                                        </ul>
                                    </div> <!-- end of menu -->
                                </asp:Panel>
                            </td>
                            <td class="auto-style6">
                                <asp:Panel ID="Panel2" runat="server">
                                </asp:Panel>
                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="My Profile"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2">&nbsp;</td>
                            <td class="auto-style3">
                                &nbsp;</td>
                            <td>
                                <asp:Panel ID="Panel3" runat="server" Height="411px">
                                    <br />
                                    <table class="auto-style7">
                                        <tr>
                                            <td class="auto-style10">&nbsp;</td>
                                            <td class="auto-style11">
                                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Medium" Text="Username:"></asp:Label>
                                            </td>
                                            <td class="auto-style12">
                                                <asp:Label ID="lblUsername" runat="server" Font-Bold="True" Font-Size="Medium" Text="-"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10">&nbsp;</td>
                                            <td class="auto-style11">
                                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Medium" Text="Current Password:"></asp:Label>
                                            </td>
                                            <td class="auto-style12">
                                                <asp:TextBox ID="txtCurrent" runat="server" Font-Size="Medium" TextMode="Password" Width="268px"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10">&nbsp;</td>
                                            <td class="auto-style11">
                                                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="Medium" Text="New Password:"></asp:Label>
                                            </td>
                                            <td class="auto-style12">
                                                <asp:TextBox ID="txtNew" runat="server" Font-Size="Medium" TextMode="Password" Width="268px"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10">&nbsp;</td>
                                            <td class="auto-style11">
                                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="Medium" Text="Confirm New Pass:"></asp:Label>
                                            </td>
                                            <td class="auto-style12">
                                                <asp:TextBox ID="txtConfirm" runat="server" Font-Size="Medium" TextMode="Password" Width="268px"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10">&nbsp;</td>
                                            <td class="auto-style11">
                                                &nbsp;</td>
                                            <td class="auto-style12">
                                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="84px" />
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10">&nbsp;</td>
                                            <td class="auto-style11">&nbsp;</td>
                                            <td class="auto-style12">
                                                <asp:Label ID="lblCheck" runat="server" Font-Bold="True" ForeColor="Red" Text="-" Visible="False"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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
		
         </div> <!-- end of footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>