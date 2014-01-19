<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="LoanManagement.Website.Services" %>

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
            height: 22px;
        }
        .auto-style3 {
            width: 34px;
        }
        .auto-style4 {
            width: 219px;
        }
        .auto-style5 {
            width: 34px;
            height: 22px;
        }
        .auto-style6 {
            width: 219px;
            height: 22px;
        }
        .auto-style7 {
            width: 353px;
        }
        .auto-style8 {
            height: 22px;
            width: 353px;
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
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <%if(Session["ID"]==null){ %>Welcome Guest!<asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">(login)</asp:LinkButton>
&nbsp;<%}else { %>Welcome <% string id = Session["NAME"].ToString(); Response.Write(id); %> 
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">(Logout)</asp:LinkButton>
            <%} %>
            </asp:PlaceHolder>
        </div>
    
    </div> <!-- templatemo_site_title_bar -->
    
    <div id="templatemo_banner_bar">
    	
        
        
      
    	
    </div> <!-- end of templatemo_banner_bar -->
    <div id="templatemo_menu">
    	<ul>
            <li><a href="Index.aspx" >Home</a></li>
            <li><a href="About.aspx">About/Contact</a></li>
            <li><a href="Services.aspx" class="current">Services</a></li>
            <li><a href="Downloads.aspx">Downloads</a></li>
            <li><a href="Application.aspx">Application</a></li>
            <li><a href="Calculator.aspx">Loan Calc.</a></li>
            <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                <%
                if(Session["ID"]==null) {%>
                    <li><a href="Login.aspx">Login/Register</a></li>
                        <% }else {%>
                    <li><a href="MyAccount_Main.aspx">My Account</a></li>
                        <% } %>
                </asp:PlaceHolder>
        </ul>
    </div> <!-- end of menu -->
    
    <div id="templatemo_content_wrapper">
    
    	<div id="templatemo_content">
        
            <div id="main_column">
            
                <div class="section_w590">
                    <h3>
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="Services Offered"></asp:Label>
                    </h3>
                    <table class="auto-style1">
                        <tr>
                            <td>
                                <asp:GridView ID="dg1" runat="server" AutoGenerateSelectButton="True" CellPadding="4" CellSpacing="3" Font-Size="Medium" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="895px">
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
                        </tr>
                        <tr>
                            <td class="auto-style2">
                                <table class="auto-style1">
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style4">
                                            <asp:Label ID="Label7" runat="server" Font-Size="Medium" Text="Total Deduction :"></asp:Label>
                                        </td>
                                        <td class="auto-style7">
                                            <asp:Label ID="lblDeduction" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style4">
                                            <asp:Label ID="Label8" runat="server" Font-Size="Medium" Text="Interest pero Mo. :"></asp:Label>
                                        </td>
                                        <td class="auto-style7">
                                            <asp:Label ID="lblInt" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style5"></td>
                                        <td class="auto-style6">
                                            <asp:Label ID="Label9" runat="server" Font-Size="Medium" Text="Loanable Amount :"></asp:Label>
                                        </td>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblAmt" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                        </td>
                                        <td class="auto-style2"></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style4">
                                            <asp:Label ID="Label10" runat="server" Font-Size="Medium" Text="Available Term :"></asp:Label>
                                        </td>
                                        <td class="auto-style7">
                                            <asp:Label ID="lblTerm" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">&nbsp;</td>
                                        <td class="auto-style4">
                                            <asp:Label ID="Label11" runat="server" Font-Size="Medium" Text="Description :"></asp:Label>
                                        </td>
                                        <td class="auto-style7">
                                            <asp:Label ID="lblDesc" runat="server" Font-Size="Medium" Text="-"></asp:Label>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    

                    
                </div>
                
                
                
                
            </div> 
            <asp:Panel ID="Panel1" runat="server">
            </asp:Panel>
            <!-- end of main column -->
             <!-- end of side column -->
            
            <div class="divider"> </div>
        </div> <!-- end of content -->
    
    	
    </div> <!-- end of templatmeo_content_wrapper -->
    
    <div id="templatemo_footer_bar_wrapper">
        <div id="templatemo_footer_bar">
                Copyright © 2013 | Guahan Financing Corporation | All Rights Reserved
		<br />
            <a href="Index.aspx">Home</a> | <a href="About.aspx">About/Contact</a> | <a href="Services.aspx">Services</a> | <a href="Downloads.aspx">Downloands</a> | <a href="Application.aspx">Application</a> | <a href="Calculator.aspx">Calculator</a> | <a href ="RegisterClient.aspx">Client Registration</a> | <a href="RegisterUser.aspx">Register User</a> | <a href="Login.aspx">Login</a> | <a href="MyAccount_Main.aspx">My Account</a> | <a href="MyAccount_Edit.aspx">Edit Account</a> <br /> <a href="MyAccount_Loans.aspx">My Loans</a> | <a href="MyAccount_Submit.aspx">Submit Requirements</a>
        </div>  <!-- end of footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>