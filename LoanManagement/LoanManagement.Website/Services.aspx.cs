using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using LoanManagement.Domain;

namespace LoanManagement.Website
{
    public partial class Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["UpdateChecker"] = null;
                Session["Service"] = null;
                Session["iService"] = null;
                using (var ctx = new finalContext())
                {
                    var ser = from se in ctx.Services
                              where se.Active == true
                              select new { ServiceNumber = se.ServiceID, Name = se.Name, Department = se.Department, Type = se.Type };
                    dg1.DataSource = ser.ToList();
                    dg1.DataBind();
                }
            }
            catch (Exception)
            {
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int sID = Convert.ToInt32(dg1.SelectedRow.Cells[1].Text);
                using (var ctx = new finalContext())
                {
                    var ser = ctx.Services.Find(sID);
                    double ded = ser.AgentCommission;
                    var de = from d in ctx.Deductions
                             where d.ServiceID == sID
                             select d;
                    foreach (var itm in de)
                    {
                        ded = ded + itm.Percentage;
                    }
                    lblDeduction.Text = ded.ToString("N2") + "%";
                    lblAmt.Text = ser.MinValue.ToString("N2") + " to " + ser.MaxValue.ToString("N2");
                    lblDesc.Text = ser.Description;
                    lblInt.Text = ser.Interest.ToString() + "%";
                    lblTerm.Text = ser.MinTerm + " month(s) to " + ser.MaxTerm + " month(s)";
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Index.aspx");
            }
        }
    }
}