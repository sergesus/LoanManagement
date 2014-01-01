﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterUser.aspx.cs" Inherits="LoanManagement.Website.RegisterUser" %>

<%@ Register assembly="MSCaptcha" namespace="MSCaptcha" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .auto-style1 {
            width: 844px;
        }
        .auto-style2 {
            width: 217px;
        }
        .auto-style3 {
            width: 139px;
        }
        .auto-style5 {
            width: 217px;
            height: 15px;
        }
        .auto-style7 {
            width: 139px;
            height: 15px;
        }
        .auto-style8 {
            height: 15px;
        }
        .auto-style9 {
            width: 217px;
            height: 30px;
        }
        .auto-style11 {
            width: 139px;
            height: 30px;
        }
        .auto-style12 {
            height: 30px;
        }
        .auto-style13 {
            width: 199%;
        }
        .auto-style14 {
            width: 228px;
        }
        .auto-style15 {
            width: 228px;
            height: 15px;
        }
        .auto-style16 {
            width: 228px;
            height: 30px;
        }
        .auto-style17 {
            width: 217px;
            height: 20px;
        }
        .auto-style18 {
            width: 228px;
            height: 20px;
        }
        .auto-style19 {
            width: 139px;
            height: 20px;
        }
        .auto-style20 {
            height: 20px;
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
            <%if(Session["ID"]==null){ %>Welcome Guest!<asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton2_Click">(login)</asp:LinkButton>
&nbsp;<%}else { %>Welcome <% string id = Session["NAME"].ToString(); Response.Write(id); %> 
            <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton1_Click">(Logout)</asp:LinkButton>
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
                    <li><a href="Login.aspx" class="current">Login/Register</a></li>
                        <% }else {%>
                    <li><a href="MyAccount_Main.aspx">My Account</a></li>
                        <% } %>
        </ul>
    </div> <!-- end of menu -->
    
    <div id="templatemo_content_wrapper">
    
    	<div id="templatemo_content">
        
            <div id="main_column">
                <div class="cleaner_h30">
                    </div>
            </div> <!-- end of main column -->

            <div id="login">
                 <table class="auto-style1">
                        <tr>
                            <td class="auto-style17"></td>
                            <td class="auto-style18">
                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="User Registration"></asp:Label>
                            </td>
                            <td class="auto-style19"></td>
                            <td class="auto-style20"></td>
                        </tr>
                        <tr>
                            <td class="auto-style2">&nbsp;</td>
                            <td class="auto-style14">
                                <asp:Label ID="Label3" runat="server" Font-Names="Segoe UI" Font-Size="Large" Text="Tracking Number:"></asp:Label>
                            </td>
                            <td class="auto-style3">
                                <asp:TextBox ID="txtTN" runat="server" Width="266px" ValidationGroup="0"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnCheck" runat="server" Text="Use" Width="79px" OnClick="btnCheck_Click" ValidationGroup="0" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style5"></td>
                            <td class="auto-style15">
                                <asp:Label ID="Label1" runat="server" Font-Names="Segoe UI" Font-Size="Large" Text="Username:"></asp:Label>
                            </td>
                            <td class="auto-style7">
                                <asp:TextBox ID="txtUsername" runat="server" Width="266px" ValidationGroup="1" Enabled="False"></asp:TextBox>
                            </td>
                            <td class="auto-style8">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsername" ErrorMessage="Please input your username" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2">&nbsp;</td>
                            <td class="auto-style14">
                                <asp:Label ID="Label2" runat="server" Font-Names="Segoe UI" Font-Size="Large" Text="Password:"></asp:Label>
                            </td>
                            <td class="auto-style3">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="266px" ValidationGroup="1" Enabled="False"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please input your password" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2">&nbsp;</td>
                            <td class="auto-style14">
                                <asp:Label ID="Label4" runat="server" Font-Names="Segoe UI" Font-Size="Large" Text="Confirm Password:"></asp:Label>
                            </td>
                            <td class="auto-style3">
                                <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" Width="266px" ValidationGroup="1" Enabled="False"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Confirm your password:" ControlToValidate="txtConfirm" ValidationGroup="1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9">&nbsp;</td>
                            <td class="auto-style16">
                                <asp:Label ID="Label5" runat="server" Font-Names="Segoe UI" Font-Size="Large" Text="Captcha:"></asp:Label>
                            </td>
                            <td class="auto-style11">
                                <table class="auto-style13">
                                    <tr>
                                        <td>
                                        <cc1:CaptchaControl ID="CaptchaControl1" runat="server" Width="180px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:TextBox ID="txtCaptcha" runat="server" Width="261px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="auto-style12">
                            <asp:Label ID="lblCaptcha" runat="server" Font-Bold="True" ForeColor="Red" Text="Enter Correct Captcha" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9"></td>
                            <td class="auto-style16"></td>
                            <td class="auto-style11">
                                <asp:Button ID="btnRegister" runat="server" Text="Register" Width="79px" OnClick="btnRegister_Click" ValidationGroup="1" Enabled="False" />
                            </td>
                            <td class="auto-style12"></td>
                        </tr>
                        <tr>
                            <td class="auto-style2">&nbsp;</td>
                            <td class="auto-style14">&nbsp;</td>
                            <td class="auto-style3">
                                <asp:Label ID="lblCheck" runat="server" Font-Bold="True" ForeColor="Red" Text="Invalid username or password" Visible="False"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        </table>
            </div>
                       
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