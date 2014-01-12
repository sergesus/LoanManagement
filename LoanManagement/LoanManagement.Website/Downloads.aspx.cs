using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;

namespace LoanManagement.Website
{
    public partial class Downloads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["Service"] = null;
                Session["iService"] = null;
                Session["UpdateChecker"] = null;
                string path = @"F:/Loan Files/Downloads/";
                List<String> lst = new List<String>();

                foreach (string s in Directory.GetFiles(path).Select(Path.GetFileName))
                    lst.Add(s);
                dg.DataSource = lst;
                dg.DataBind();
                foreach (GridViewRow row in dg.Rows)
                {
                    LinkButton lb = (LinkButton)row.Cells[0].Controls[0];
                    lb.Text = "Download";
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


        protected void dg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void dg_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = dg.SelectedRow.Cells[1].Text;
                string path = @"F:/Loan Files/Downloads/" + name;
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
    }
}