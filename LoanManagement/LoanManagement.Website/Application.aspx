<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Application.aspx.cs" Inherits="LoanManagement.Website.Application" %>

<%@ Register assembly="MSCaptcha" namespace="MSCaptcha" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .auto-style1 {
            width: 95%;
            height: 14px;
        }
        .auto-style2 {
            height: 81px;
        }
        .auto-style3 {
            width: 41px;
        }
        .auto-style10 {
            width: 172px;
        }
        .auto-style12 {
            width: 206px;
        }
        .auto-style14 {
            width: 291px;
        }
        .auto-style27 {
            height: 123px;
        }
        .auto-style33 {
            width: 41px;
            height: 30px;
        }
        .auto-style34 {
            width: 172px;
            height: 30px;
        }
        .auto-style35 {
            width: 291px;
            height: 30px;
        }
        .auto-style36 {
            width: 206px;
            height: 30px;
        }
        .auto-style37 {
            height: 30px;
        }
        .auto-style44 {
            width: 41px;
            height: 19px;
        }
        .auto-style45 {
            width: 172px;
            height: 19px;
        }
        .auto-style46 {
            width: 291px;
            height: 19px;
        }
        .auto-style47 {
            width: 206px;
            height: 19px;
        }
        .auto-style48 {
            height: 19px;
        }
        .auto-style49 {
            width: 41px;
            height: 18px;
        }
        .auto-style50 {
            width: 172px;
            height: 18px;
        }
        .auto-style51 {
            width: 291px;
            height: 18px;
        }
        .auto-style52 {
            width: 206px;
            height: 18px;
        }
        .auto-style53 {
            height: 18px;
        }
        .auto-style54 {
            width: 41px;
            height: 72px;
        }
        .auto-style55 {
            width: 172px;
            height: 72px;
        }
        .auto-style56 {
            width: 291px;
            height: 72px;
        }
        .auto-style57 {
            width: 206px;
            height: 72px;
        }
        .auto-style58 {
            height: 72px;
        }
        .auto-style59 {
            height: 16px;
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
            <li><a href="Application.aspx" class="current">Application</a></li>
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
                        <td class="auto-style2">
                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Online Loan Application"></asp:Label>
                            <br />
                            <br />
                            <asp:LinkButton ID="lnkBtn" runat="server" PostBackUrl="~/Login.aspx" Visible="False">Login/Register first here</asp:LinkButton>
                            <br />
                            <asp:Label ID="lblCheck" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style27">
                            <asp:Panel ID="pnlForm" runat="server" Height="135px">
                                <table class="auto-style1">
                                    <tr>
                                        <td class="auto-style44"></td>
                                        <td class="auto-style45">
                                            <asp:Label ID="Label2" runat="server" Font-Size="Medium" Text="Type Of Loan:"></asp:Label>
                                        </td>
                                        <td class="auto-style46">
                                            <asp:DropDownList ID="cmbTOL" runat="server" Font-Size="Medium" Height="33px" Width="289px" AutoPostBack="True" OnSelectedIndexChanged="cmbTOL_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblDed" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style47"></td>
                                        <td class="auto-style48"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3"></td>
                                        <td class="auto-style10">
                                            <asp:Label ID="Label4" runat="server" Font-Size="Medium" Text="Mode Of Payment :"></asp:Label>
                                        </td>
                                        <td class="auto-style14">
                                            <asp:DropDownList ID="cmbMode" runat="server" Font-Size="Medium" Height="33px" Width="289px">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblInt" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style12"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3"></td>
                                        <td class="auto-style10">
                                            <asp:Label ID="Label5" runat="server" Font-Size="Medium" Text="Desired Amount :"></asp:Label>
                                        </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtAmt" runat="server" Width="281px"></asp:TextBox>
                                            <asp:Label ID="lblAmt" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style12">
                                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtAmt" ErrorMessage="Enter double values only" Font-Size="Medium" ForeColor="Red" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                            <br />
                                            <br />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style49"></td>
                                        <td class="auto-style50">
                                            <asp:Label ID="Label6" runat="server" Font-Size="Medium" Text="Desired Term(months) :"></asp:Label>
                                        </td>
                                        <td class="auto-style51">
                                            <asp:TextBox ID="txtTerm" runat="server" Width="281px"></asp:TextBox>
                                            <asp:Label ID="lblTerm" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style52">
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTerm" ErrorMessage="Enter whole numbers only" Font-Size="Medium" ForeColor="Red" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                            <br />
                                            <br />
                                        </td>
                                        <td class="auto-style53"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style54"></td>
                                        <td class="auto-style55">
                                            <asp:Label ID="Label7" runat="server" Font-Size="Medium" Text="Captcha:"></asp:Label>
                                        </td>
                                        <td class="auto-style56">
                                            <table class="auto-style1">
                                                <tr>
                                                    <td>
                                                        <cc1:CaptchaControl ID="CaptchaControl1" runat="server" Width="180px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="auto-style59">
                                                        <asp:TextBox ID="txtCaptcha" runat="server" Width="272px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="auto-style57">
                                            <asp:Label ID="lblCaptcha" runat="server" Font-Bold="True" ForeColor="Red" Text="Enter Correct Captcha" Visible="False"></asp:Label>
                                        </td>
                                        <td class="auto-style58"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style33"></td>
                                        <td class="auto-style34"></td>
                                        <td class="auto-style35">
                                            <asp:Button ID="btnApply" runat="server" OnClick="btnApply_Click" Text="Apply" Width="83px" />
                                            <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Reset" Width="83px" />
                                        </td>
                                        <td class="auto-style36"></td>
                                        <td class="auto-style37"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3"></td>
                                        <td class="auto-style10"></td>
                                        <td class="auto-style14">
                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Text="Enter Correct Captcha" Visible="False"></asp:Label>
                                        </td>
                                        <td class="auto-style12"></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                
                <div class="cleaner"></div>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                
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
		
        </div>  <!-- end of footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>