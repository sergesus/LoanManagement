<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyAccount_Loans_LoanInfo.aspx.cs" Inherits="LoanManagement.Website.MyAccount_Loans_LoanInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Guahan Finance Corporation</title>
    <link href="templatemo_style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function ShowConfirmation() {
            if (confirm("Are you sure you want to delete?") == true) {
                //Calling the server side code after confirmation from the user
                document.getElementById("btnAlert").click();
            }
        }
    </script>

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
            height: 494px;
        }
        .auto-style9 {
            height: 95px;
        }
        .auto-style17 {
            width: 100%;
        }
        .auto-style8 {
            width: 99%;
            height: 252px;
        }
        .auto-style10 {
            width: 121px;
        }
        .auto-style18 {
            width: 316px;
        }
        .auto-style19 {
            width: 121px;
            height: 20px;
        }
        .auto-style20 {
            height: 20px;
        }
        .auto-style28 {
            width: 138px;
            height: 20px;
        }
        .auto-style30 {
            width: 99%;
            height: 178px;
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
                                            <li><a href="MyAccount_Edit.aspx">Edit Account</a></li>
                                            <li><a href="MyAccount_Loans.aspx" class="current">View/Update Loans</a></li>
                                            <li><a href="MyAccount_Submit.aspx">Submit Requirements</a></li>
                                        </ul>
                                    </div> <!-- end of menu -->
                                </asp:Panel>
                            </td>
                            <td class="auto-style6">
                                <asp:Panel ID="Panel2" runat="server">
                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Loan Information"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2">&nbsp;</td>
                            <td class="auto-style3">
                                &nbsp;</td>
                            <td>
                                <table class="auto-style7">
                                    <tr>
                                        <td class="auto-style9">
                                            <table class="auto-style17">
                                                <tr>
                                                    <td class="auto-style18">
                                                        <table class="auto-style30">
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Loan ID:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblID" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Type Of Loan:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblTOL" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Service Type:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblType" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label10" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Term:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblTerm" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label11" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Amt Applied:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblAmtApplied" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label12" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Status:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblStatus" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label19" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Approved Amt:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblAmtApproved" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">
                                                                    <asp:Label ID="Label21" runat="server" Font-Bold="False" Font-Italic="True" Font-Size="Medium" Text="Date of Releasing:"></asp:Label>
                                                                </td>
                                                                <td class="auto-style20">
                                                                    <asp:Label ID="lblDtReleasing" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style28">&nbsp;</td>
                                                                <td class="auto-style20">
                                                                    <br />
                                                                    <br />
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="pnlHidden" runat="server" Visible="False">
                                                            <table class="auto-style8">
                                                                <tr>
                                                                    <td class="auto-style19">
                                                                        <asp:Label ID="Label23" runat="server" Font-Bold="False" Font-Size="Medium" Text="Released Amt:" Font-Italic="True"></asp:Label>
                                                                    </td>
                                                                    <td class="auto-style20">
                                                                        <asp:Label ID="lblAmtReleased" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="auto-style19">
                                                                        <asp:Label ID="Label25" runat="server" Font-Bold="False" Font-Size="Medium" Text="Remaining:" Font-Italic="True"></asp:Label>
                                                                    </td>
                                                                    <td class="auto-style20">
                                                                        <asp:Label ID="lblRemaining" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="auto-style19">
                                                                        <asp:Label ID="Label27" runat="server" Font-Bold="False" Font-Size="Medium" Text="Next Payment:" Font-Italic="True"></asp:Label>
                                                                    </td>
                                                                    <td class="auto-style20">
                                                                        <asp:Label ID="lblNextPaymentDt" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="auto-style19">
                                                                        <asp:Label ID="Label29" runat="server" Font-Bold="False" Font-Size="Medium" Text="Payment Amt.:" Font-Italic="True"></asp:Label>
                                                                    </td>
                                                                    <td class="auto-style20">
                                                                        <asp:Label ID="lblPaymentAmt" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="auto-style10">&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="auto-style10">&nbsp;</td>
                                                                    <td>
                                                                        <br />
                                                                        <br />
                                                                        <br />
                                                                        <br />
                                                                        <br />
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
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
		
         f footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>
