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
    public partial class MyAccount_Submit_Requirements : System.Web.UI.Page
    {
        public int id;
        public string status;
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var ctx = new finalContext())
            {
                var set = ctx.OnlineSettings.Find(1);
                lblVisitor.Text = set.Visitor.ToString();
            }
            lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy | hh:mm tt");
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Session["Service"] = null;
            Session["UpdateChecker"] = null;
            Session["iService"] = null;
            if (Session["ID"] == null)
            {
                Response.Redirect("/Login.aspx");
            }

            int cID = Convert.ToInt32(Session["ID"]);
            try
            {
                id = Convert.ToInt32(Request.QueryString["LoanID"]);
                status = Request.QueryString["Status"];
                string path = "";
                if (status == "Online")
                    path = @"F:/Loan Files/Applications Online/Application " + id.ToString();
                else
                    path = @"F:/Loan Files/Loan " + id.ToString();

                if (status == "Online")
                {
                    using (var ctx = new finalContext())
                    {
                        var ctr = ctx.TemporaryLoanApplications.Where(x => x.TemporaryLoanApplicationID == id && x.ClientID == cID).Count();
                        if (ctr < 1)
                        {
                            Response.Redirect("MyAccount_Submit.aspx");
                        }
                    }
                }
                else
                {
                    using (var ctx = new finalContext())
                    {
                        var ctr = ctx.Loans.Where(x => x.LoanID == id && x.ClientID == cID).Count();
                        if (ctr < 1)
                        {
                            Response.Redirect("MyAccount_Submit.aspx");
                        }
                    }
                }

                if (Directory.Exists(path))
                {
                    List<String> lst = new List<String>();
                    int c = 0;
                    foreach (string s in Directory.GetFiles(path).Select(Path.GetFileName))
                    {
                        lst.Add(s);
                        c++;
                    }
                    if (lst.Count() > 0)
                    {
                        pnlHidden.Visible = true;
                        lblCheck.Visible = false;
                    }
                    else
                    {
                        pnlHidden.Visible = false;
                        lblCheck.Visible = true;
                    }
                    dg.DataSource = lst;
                    dg.DataBind();
                    foreach (GridViewRow row in dg.Rows)
                    {
                        LinkButton lb = (LinkButton)row.Cells[0].Controls[0];
                        lb.Text = "View";
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(path);
                    pnlHidden.Visible = false;
                    lblCheck.Visible = true;
                }

            }
            catch (Exception)
            {
                if (Session["ID"] == null)
                    Response.Redirect("/Login.aspx");
                else
                    Response.Redirect("MyAccount_Submit.aspx");
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("MMM dd yyyy, | hh:mm tt");
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

        protected void dg_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                string path = "";
                name = dg.SelectedRow.Cells[1].Text;
                if (status == "Online")
                    path = @"F:/Loan Files/Applications Online/Application " + id.ToString();
                else
                    path = @"F:/Loan Files/Loan " + id.ToString();

                path = path + "/" + name;

                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "text/plain";
                    Response.Flush();
                    Response.TransmitFile(file.FullName);
                    Response.End();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string path = "";
                if (status == "Online")
                    path = @"F:/Loan Files/Applications Online/Application " + id.ToString();
                else
                    path = @"F:/Loan Files/Loan " + id.ToString();

                if (FileUpload1.HasFile)
                {
                    //create the path to save the file to
                    string fileName = Path.Combine(path, FileUpload1.FileName);
                    //save the file to our local path
                    FileUpload1.SaveAs(fileName);
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    Span1.Text = "Please select a file to upload";
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}