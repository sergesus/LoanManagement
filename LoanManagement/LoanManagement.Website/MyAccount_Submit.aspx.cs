using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data.Entity;
using LoanManagement.Domain;

namespace LoanManagement.Website
{
    public partial class MyAccount_Submit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session["Service"] = null;
                Session["UpdateChecker"] = null;
                Session["iService"] = null;
                if (Session["ID"] == null)
                {
                    Response.Redirect("/Login.aspx");
                }

                int cID = Convert.ToInt32(Session["ID"]);

                using (var ctx = new finalContext())
                {
                    var c1 = ctx.TemporaryLoanApplications.Where(x => x.ClientID == cID).Count();
                    if (c1 > 0)
                    {
                        var lns = from l in ctx.TemporaryLoanApplications
                                  where l.ClientID == cID
                                  select new { ID = l.TemporaryLoanApplicationID, TypeOfLoan = l.Service.Name, ServiceType = l.Service.Type, DesiredTerm = l.Term, AmountApplied = l.AmountApplied, ModeOfPayment = l.Mode };

                        dg.DataSource = lns.ToList();
                        dg.DataBind();
                        pnlHidden.Visible = !false;
                        lblCheck.Visible = !true;
                        return;
                    }
                    var c2 = ctx.Loans.Where(x => x.ClientID == cID && x.Status == "Applied").Count();
                    if (c2 > 0)
                    {
                        var lns = from l in ctx.Loans
                                  where l.ClientID == cID && l.Status == "Applied"
                                  select new { LoanID = l.LoanID, TypeOfLoan = l.Service.Name, Type = l.Service.Type, Term = l.Term, ModeOfPayment = l.Mode, Status = l.Status };

                        dg.DataSource = lns.ToList();
                        dg.DataBind();
                        pnlHidden.Visible = !false;
                        lblCheck.Visible = !true;
                        return;
                    }

                    if (c1 == 0 && c2 == 0)
                    {
                        pnlHidden.Visible = false;
                        lblCheck.Visible = true;
                    }


                }
            }
            catch (Exception)
            {
                if (Session["ID"] == null)
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int cID = Convert.ToInt32(Session["ID"]);

                using (var ctx = new finalContext())
                {
                    string lID = dg.SelectedRow.Cells[1].Text;
                    var c1 = ctx.TemporaryLoanApplications.Where(x => x.ClientID == cID).Count();
                    if (c1 > 0)
                    {
                        Response.Redirect("/MyAccount_Submit_Requirements.aspx?LoanID=" + lID.ToString() + "&Status=Online");
                    }
                    else
                    {
                        Response.Redirect("/MyAccount_Submit_Requirements.aspx?LoanID=" + lID.ToString() + "&Status=Applied");
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}