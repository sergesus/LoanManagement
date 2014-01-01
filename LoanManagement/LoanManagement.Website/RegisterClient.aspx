<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterClient.aspx.cs" Inherits="LoanManagement.Website.RegisterClient" %>

<%@ Register assembly="MSCaptcha" namespace="MSCaptcha" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("[id$=txtBirthday]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
            });
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 180px;
        }
        .auto-style3 {
            width: 180px;
            height: 22px;
        }
        .auto-style4 {
            height: 22px;
        }
        .auto-style5 {
            width: 295px;
        }
        .auto-style6 {
            height: 22px;
            width: 295px;
        }
        .auto-style10 {
            width: 180px;
            height: 2px;
        }
        .auto-style11 {
            width: 295px;
            height: 2px;
        }
        .auto-style12 {
            height: 2px;
        }
        .auto-style13 {
            width: 30px;
        }
        .auto-style14 {
            width: 30px;
            height: 22px;
        }
        .auto-style16 {
            width: 30px;
            height: 2px;
        }
        .auto-style17 {
            width: 30px;
            height: 86px;
        }
        .auto-style18 {
            width: 180px;
            height: 86px;
        }
        .auto-style19 {
            width: 295px;
            height: 86px;
        }
        .auto-style20 {
            height: 86px;
        }
        .auto-style21 {
            width: 30px;
            height: 38px;
        }
        .auto-style22 {
            width: 180px;
            height: 38px;
        }
        .auto-style23 {
            width: 295px;
            height: 38px;
        }
        .auto-style25 {
            height: 38px;
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
                    <li><a href="MyAccount_Main.aspx">My Account</a></li>
                        <% } %>
        </ul>
    </div> <!-- end of menu -->
    
    <div id="templatemo_content_wrapper">
    
    	<div id="templatemo_content">
        
            <div id="main_column">
            
                
                
                
                <table class="auto-style1">
                    <tr>
                        <td class="auto-style21"></td>
                        <td class="auto-style22">
                        <asp:Label ID="Label19" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Client Registration"></asp:Label>
                        </td>
                        <td class="auto-style23"></td>
                        <td class="auto-style25"></td>
                        <td class="auto-style25"></td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style5">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label2" runat="server" Font-Size="Medium" Text="Last Name:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtLastName" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style14">&nbsp;</td>
                        <td class="auto-style3">
                            <asp:Label ID="Label3" runat="server" Font-Size="Medium" Text="First Name:"></asp:Label>
                        </td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtFirstName" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td class="auto-style4"></td>
                        <td class="auto-style4"></td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label4" runat="server" Font-Size="Medium" Text="Middle Name:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtMiddleName" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label5" runat="server" Font-Size="Medium" Text="Suffix:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtSuffix" runat="server" Width="45px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label6" runat="server" Font-Size="Medium" Text="Birthday:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtBirthday" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13"></td>
                        <td class="auto-style2">
                            <asp:Label ID="Label7" runat="server" Font-Size="Medium" Text="Gender:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:DropDownList ID="cmbGender" runat="server" Font-Size="Medium" Height="25px" Width="158px">
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="auto-style16"></td>
                        <td class="auto-style10">
                            <asp:Label ID="Label8" runat="server" Font-Size="Medium" Text="Status:"></asp:Label>
                        </td>
                        <td class="auto-style11">
                            <asp:DropDownList ID="cmbStatus" runat="server" Font-Size="Medium" Height="25px" Width="158px">
                                <asp:ListItem>Single</asp:ListItem>
                                <asp:ListItem>Married</asp:ListItem>
                                <asp:ListItem>Widowed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="auto-style12"></td>
                        <td class="auto-style12"></td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label9" runat="server" Font-Size="Medium" Text="SSS:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtSSS" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label10" runat="server" Font-Size="Medium" Text="TIN:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtTIN" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label11" runat="server" Font-Size="Medium" Text="Email:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtEmail" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label15" runat="server" Font-Size="Medium" Text="Contact:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtContact" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label16" runat="server" Font-Size="Medium" Text="Username:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtUsername" runat="server" Width="320px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label17" runat="server" Font-Size="Medium" Text="Password:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtPassword" runat="server" Width="320px" TextMode="Password"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">
                            <asp:Label ID="Label18" runat="server" Font-Size="Medium" Text="Confirm Password:"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <asp:TextBox ID="txtConfirm" runat="server" Width="320px" TextMode="Password"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style17"></td>
                        <td class="auto-style18">
                            <asp:Label ID="Label14" runat="server" Font-Size="Medium" Text="Captcha:"></asp:Label>
                        </td>
                        <td class="auto-style19">
                            <table class="auto-style1">
                                <tr>
                                    <td>
                                        <cc1:CaptchaControl ID="CaptchaControl1" runat="server" Width="180px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCaptcha" runat="server" Width="314px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="auto-style20">
                            <asp:Label ID="lblCaptcha" runat="server" Font-Bold="True" ForeColor="Red" Text="Enter Correct Captcha" Visible="False"></asp:Label>
                        </td>
                        <td class="auto-style20"></td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style5">
                            <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click1" />
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style13">&nbsp;</td>
                        <td class="auto-style2">&nbsp;</td>
                        <td class="auto-style5">
                            <asp:Label ID="lblExists" runat="server" Font-Bold="True" ForeColor="Red" Text="Client already exists." Visible="False"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            
                
                
                
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
