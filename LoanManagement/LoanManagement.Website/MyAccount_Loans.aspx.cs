using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LoanManagement.Domain;
using System.Data.Entity;
using System.IO;

namespace LoanManagement.Website
{
    public partial class MyAccount_Loans : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session["Service"] = null;
                Session["iService"] = null;
                Session["UpdateChecker"] = null;
                if (Session["ID"] == null)
                {
                    Response.Redirect("/Login.aspx");
                }
                using (var ctx = new newerContext())
                {
                    int cID = Convert.ToInt32(Session["ID"]);
                    var clt = ctx.Clients.Find(cID);
                    var c = ctx.TemporaryLoanApplications.Where(x => x.ClientID == cID).Count();
                    if (c > 0)
                    {
                        pnlCurrent.Visible = true;
                        var lon = from l in ctx.TemporaryLoanApplications
                                  where l.ClientID == cID
                                  select new { TypeOfLoan = l.Service.Name, ServiceType = l.Service.Type, DesiredTerm = l.Term, AmountApplied = l.AmountApplied, ModeOfPayment = l.Mode, DateApplied = l.DateApplied, ExpirationDate = l.ExpirationDate };
                        dgCurrent.DataSource = lon.ToList();
                        dgCurrent.DataBind();
                    }
                    else
                    {
                        lblCheck1.Visible = true;
                    }

                    c = ctx.Loans.Where(x => x.ClientID == cID).Count();
                    if (c > 0)
                    {
                        pnlLoans.Visible = true;
                        var lon = from l in ctx.Loans
                                  where l.ClientID == cID
                                  select new { LoanID = l.LoanID, TypeOfLoan = l.Service.Name, Type = l.Service.Type, Term = l.Term, ModeOfPayment = l.Mode, Status = l.Status };
                        dgLoans.DataSource = lon.ToList();
                        dgLoans.DataBind();
                    }
                    else
                    {
                        lblCheck2.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
                if(Session["ID"] == null)
                    Response.Redirect("/Login.aspx");
                else
                    Response.Redirect("/Index.aspx");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("/Index.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Login.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Application.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "showAl", "ShowConfirmation();", true); 
                using (var ctx = new newerContext())
                {
                    int n = Convert.ToInt32(Session["ID"]);
                    var lon = ctx.TemporaryLoanApplications.Where(x => x.ClientID == n).First();
                    string folderName = @"F:\Loan Files\Applications Online";
                    string pathString = System.IO.Path.Combine(folderName, "Application " + lon.TemporaryLoanApplicationID.ToString());
                    if (Directory.Exists(pathString))
                    {
                        Directory.Delete(pathString, true);
                    }
                    ctx.TemporaryLoanApplications.Remove(lon);
                    ctx.SaveChanges();

                    string myStringVariable = "Loan application has been successfuly cancelled";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                    Response.Redirect("/MyAccount_Loans.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        protected void btnAlert_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new newerContext())
                {
                    int n = Convert.ToInt32(Session["ID"]);
                    var lon = ctx.TemporaryLoanApplications.Where(x => x.ClientID == n).First();
                    ctx.TemporaryLoanApplications.Remove(lon);
                    ctx.SaveChanges();
                    string myStringVariable = "Loan application has been successfuly cancelled";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string s = dgLoans.SelectedRow.Cells[1].Text;
                Response.Redirect("/MyAccount_Loans_LoanInfo.aspx?LoanID=" +s );
            }
            catch (Exception) { return; }
        } 

    }
}