<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="LoanManagement.Website.Calculator" %>

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
            height: 21px;
        }
        .auto-style45 {
            width: 172px;
            height: 21px;
        }
        .auto-style46 {
            width: 291px;
            height: 21px;
        }
        .auto-style47 {
            width: 206px;
            height: 21px;
        }
        .auto-style48 {
            height: 21px;
        }
        .auto-style49 {
            width: 41px;
            height: 25px;
        }
        .auto-style50 {
            width: 172px;
            height: 25px;
        }
        .auto-style51 {
            width: 291px;
            height: 25px;
        }
        .auto-style52 {
            width: 206px;
            height: 25px;
        }
        .auto-style53 {
            height: 25px;
        }
        .auto-style54 {
            width: 41px;
            height: 11px;
        }
        .auto-style55 {
            width: 172px;
            height: 11px;
        }
        .auto-style56 {
            width: 291px;
            height: 11px;
        }
        .auto-style57 {
            width: 206px;
            height: 11px;
        }
        .auto-style58 {
            height: 11px;
        }
        .auto-style59 {
            width: 41px;
            height: 6px;
        }
        .auto-style60 {
            width: 172px;
            height: 6px;
        }
        .auto-style61 {
            width: 291px;
            height: 6px;
        }
        .auto-style62 {
            width: 206px;
            height: 6px;
        }
        .auto-style63 {
            height: 6px;
        }
        .auto-style64 {
            width: 41px;
            height: 22px;
        }
        .auto-style65 {
            width: 172px;
            height: 22px;
        }
        .auto-style66 {
            width: 291px;
            height: 22px;
        }
        .auto-style67 {
            width: 206px;
            height: 22px;
        }
        .auto-style68 {
            height: 22px;
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
            <li><a href="Application.aspx" >Application</a></li>
            <li><a href="Calculator.aspx" class="current">Loan Calc.</a></li>
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
                        <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Loan Calculator"></asp:Label>
                            <br />
                            <br />
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
                                        <td class="auto-style49"></td>
                                        <td class="auto-style50">
                                            <asp:Label ID="Label4" runat="server" Font-Size="Medium" Text="Mode Of Payment :"></asp:Label>
                                        </td>
                                        <td class="auto-style51">
                                            <asp:DropDownList ID="cmbMode" runat="server" Font-Size="Medium" Height="33px" Width="289px">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblInt" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style52"></td>
                                        <td class="auto-style53"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style54"></td>
                                        <td class="auto-style55">
                                            <asp:Label ID="Label5" runat="server" Font-Size="Medium" Text="Desired Amount :"></asp:Label>
                                        </td>
                                        <td class="auto-style56">
                                            <asp:TextBox ID="txtAmt" runat="server" Width="281px"></asp:TextBox>
                                            <asp:Label ID="lblAmt" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style57"></td>
                                        <td class="auto-style58"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style59"></td>
                                        <td class="auto-style60">
                                            <asp:Label ID="Label6" runat="server" Font-Size="Medium" Text="Desired Term(months) :"></asp:Label>
                                        </td>
                                        <td class="auto-style61">
                                            <asp:TextBox ID="txtTerm" runat="server" Width="281px"></asp:TextBox>
                                            <asp:Label ID="lblTerm" runat="server"></asp:Label>
                                        </td>
                                        <td class="auto-style62"></td>
                                        <td class="auto-style63"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style33"></td>
                                        <td class="auto-style34"></td>
                                        <td class="auto-style35">
                                            <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" Text="Calculate" Width="83px" />
                                            <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset" Width="83px" />
                                        </td>
                                        <td class="auto-style36"></td>
                                        <td class="auto-style37"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style64"></td>
                                        <td class="auto-style65"></td>
                                        <td class="auto-style66">
                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Text="Enter Correct Captcha" Visible="False"></asp:Label>
                                        </td>
                                        <td class="auto-style67"></td>
                                        <td class="auto-style68"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style10">
                                            <asp:Label ID="Label7" runat="server" Font-Size="Medium" Text="Total Deduction :"></asp:Label>
                                        </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtDeduction" runat="server" Width="281px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="auto-style12">&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style10">
                                            <asp:Label ID="Label8" runat="server" Font-Size="Medium" Text="Net Proceed :"></asp:Label>
                                        </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtNet" runat="server" Width="281px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="auto-style12">&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style10">
                                            <asp:Label ID="Label9" runat="server" Font-Size="Medium" Text="Total Blaance :"></asp:Label>
                                        </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtBalance" runat="server" Width="281px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="auto-style12">&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style10">
                                            <asp:Label ID="Label10" runat="server" Font-Size="Medium" Text="Ammortization:"></asp:Label>
                                        </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtAmmortization" runat="server" Width="281px" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td class="auto-style12">&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            <br />
                
                
                            <br />
                        </td>
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