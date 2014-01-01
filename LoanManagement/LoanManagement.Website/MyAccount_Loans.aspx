<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyAccount_Loans.aspx.cs" Inherits="LoanManagement.Website.MyAccount_Loans" %>

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
            height: 30px;
            margin-bottom: 0px;
        }
        .auto-style9 {
            height: 223px;
        }
        .auto-style10 {
            height: 117px;
        }
        .auto-style11 {
            height: 189px;
        }
        .auto-style12 {
            height: 8px;
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
                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="X-Large" ForeColor="Lime" Text="My Loans"></asp:Label>
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
                                        <td>
                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="Large" ForeColor="White" Text="Current Online Application(Unverified)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style10">
                                            <table class="auto-style7">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCheck1" runat="server" Font-Size="Large" ForeColor="Red" Text="You currently don't have any online application" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="auto-style11">
                                                        <asp:Panel ID="pnlCurrent" runat="server" Height="156px" Visible="False">
                                                            <asp:GridView ID="dgCurrent" runat="server" Height="16px" Width="634px" CellPadding="4" CellSpacing="2" ForeColor="#333333" GridLines="None">
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
                                                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" Width="111px" />
                                                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel Application" Width="116px" />
                                                            <br />
                                                            
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style12">
                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="Large" ForeColor="White" Text="List Of Loans"></asp:Label>
                                            <br />
                                                        <asp:Label ID="lblCheck2" runat="server" Font-Size="Large" ForeColor="Red" Text="You currently don't have any online loans" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style9">
                                                        <asp:Panel ID="pnlLoans" runat="server" Height="156px" Visible="False">
                                                            <asp:GridView ID="dgLoans" runat="server" Height="16px" Width="634px" CellPadding="4" CellSpacing="2" ForeColor="#333333" GridLines="None" AutoGenerateSelectButton="True">
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
                                                            <asp:Button ID="btnView" runat="server" Text="View" Width="111px" OnClick="btnView_Click" />
                                                            <br />
                                                            
                                                        </asp:Panel>
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
		
         </div> <!-- end of footer -->
	</div> <!-- end of footer_wrapper -->
</div> <!-- end of container -->


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>
