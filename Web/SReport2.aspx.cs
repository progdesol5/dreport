using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Transactions;
using Database;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Web
{
    public partial class SReport2 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CRMNewEntitiesNew"].ConnectionString);
        SqlCommand command2 = new SqlCommand();
        CallEntities DB = new CallEntities();
        int TID = 10;
        bool FromHangFire = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FromHangFire || Request.QueryString != null)
            {
                // DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
                //DateTime sdt = DateTime.Now.AddDays(-1);
                //DateTime Edt = DateTime.Now.AddDays(0);

                //string stdate = sdt.ToString("yyyy-dd-MM");
                //string etdate = Edt.ToString("yyyy-dd-MM");
                //string timede1 = " 12:00:00";
                //string timede2 = " 11:59:59";

                //string sdt1 = stdate + timede1;
                //string Edt1 = etdate + timede2;

                DateTime sdt = DateTime.Now.AddDays(-1);
                DateTime Edt = DateTime.Now.AddDays(-1);

                string stdate = sdt.ToString("yyyy-MM-dd");
                string etdate = Edt.ToString("yyyy-MM-dd");
                string timede1 = " 12:00:00 AM";
                string timede2 = " 11:59:59 PM";

                string sdt1 = stdate + timede1;
                string Edt1 = etdate + timede2;

                string SQOCommads = "select distinct NoticeEmail  from  ViewDeptWiseComplainDailyReport where TenentID = 10 and  UploadDate BETWEEN ' " + sdt1 + " ' AND ' " + Edt1 + "' ";
                secificEmailSend("idrissawerwala@gmail.com");
                /*SqlCommand CMD2 = new SqlCommand(SQOCommads, con);
                SqlDataAdapter ADB1 = new SqlDataAdapter(CMD2);
                DataSet ds1 = new DataSet();
                ADB1.Fill(ds1);
                DataTable dt1 = ds1.Tables[0];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string email = dt1.Rows[i].ItemArray[0].ToString();
                    secificEmailSend(email);
                }*/
            }
        }
        public void HangfireLoad()
        {
            //Request.QueryString["p"] = "s";
            FromHangFire = true;
            Page_Load(null, null);
        }
        public void secificEmailSend(string Email)
        {
            bool Flag = false;
            if (FromHangFire || (Request.QueryString != null ) )
            {
                //DateTime sdt = DateTime.Now.AddDays(-1);
                //DateTime Edt = DateTime.Now.AddDays(0);

                ////string stdate = sdt.ToString("yyyy-MM-dd");
                ////string etdate = Edt.ToString("yyyy-MM-dd");
                ////string timede1 = " 12:00:00";
                ////string timede2 = " 11:59:59";

                ////string sdt1 = stdate + timede1;
                ////string Edt1 = etdate + timede2;



                //string stdate = sdt.ToString("yyyy-MM-dd hh:mm:ss ttt");
                //string etdate = Edt.ToString("yyyy-MM-dd hh:mm:ss ttt");
                DateTime sdt = DateTime.Now.AddDays(-1);
                DateTime Edt = DateTime.Now.AddDays(-1);

                string stdate = sdt.ToString("yyyy-MM-dd");
                string etdate = Edt.ToString("yyyy-MM-dd");
                string timede1 = " 12:00:00 AM";
                string timede2 = " 11:59:59 PM";

                string sdt1 = stdate + timede1;
                string Edt1 = etdate + timede2;


                string SQOCommads = " select ComplaintNumber as 'ComplaintNumber' , DeptName as 'DeptName' , MasterCODE as 'MasterCODE' , TickComplainType as 'complain' , TickDepartmentID as 'Department' , " +
                        " Patient_Name as Name , MRN as 'MRN' , aspcomment as 'aspcomment' , MyStatus as MyStatus , Patient_Name as Patient_Name, investigation as investigation , MRN as MRN , Contact as Contact ,NoticeEmail as 'NoticeEmail' , TickCatID as 'Category' ,  " +
                        " TickSubCatID as 'SubCategory' , ActivityPerform as 'Remarks', ReportedBy as 'ReportedBy' , UploadDate as 'UploadDate' " +
                        " from ViewDeptWiseComplainDailyReport where TenentID = " + TID + " and  UploadDate BETWEEN ' " + sdt1 + " ' AND ' " + Edt1 + "' " +
                        " and NoticeEmail='" + Email + "'  order by TickDepartmentID , NoticeEmail";

                SqlCommand CMD2 = new SqlCommand(SQOCommads, con);
                SqlDataAdapter ADB1 = new SqlDataAdapter(CMD2);
                DataSet ds1 = new DataSet();
                ADB1.Fill(ds1);
                DataTable dt1 = ds1.Tables[0];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string email = dt1.Rows[i].ItemArray[0].ToString();
                    secificEmailSend(email);
                }

               
                int dID = 0;

                string Tablecontant = "<table border='1' class='table table-bordered table-hover' style='with:100%'><thead><tr> <th style='background-color:skyblue;font-size:large;font-weight:bold;'>Complaint No </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> Reference </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> Created By </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> Action Status </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> Patient  </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> MRN </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> Investigation Result </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> Attachment Link </th><th style='background-color:skyblue;font-size:large;font-weight:bold;'> Activity </th></tr></thead><tbody>";
                //string email = "";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string complainno = dt1.Rows[i]["ComplaintNumber"].ToString();
                    int Master = Convert.ToInt32(dt1.Rows[i]["MasterCODE"]);
                    int department = Convert.ToInt32(dt1.Rows[i]["Department"]);
                    DateTime dt = Convert.ToDateTime(dt1.Rows[i]["UploadDate"]);
                    int action = Convert.ToInt32(dt1.Rows[i]["aspcomment"]);
                    string dname = dt1.Rows[i]["DeptName"].ToString();
                    string Patient = dt1.Rows[i]["Patient_Name"].ToString();
                    string MRN = dt1.Rows[i]["MRN"].ToString();
                    string MobileNo = dt1.Rows[i]["Contact"].ToString();
                    string remark = dt1.Rows[i]["Remarks"].ToString();
                    //string MRN = dt1.Rows[i]["MRN"].ToString();
                    string MyStatus = dt1.Rows[i]["MyStatus"].ToString();
                    string dat = dt.ToString();
                    string reference = complainno + "-" + dat + "-" + remark;
                    string Patientn = "Patient: " + Patient + " Mob:" + MobileNo;
                    int ReportedBy = Convert.ToInt32(dt1.Rows[i]["ReportedBy"]);
                    string UName = DB.USER_MST.Single(p => p.TenentID == TID && p.USER_ID == ReportedBy).FIRST_NAME;
                    string iname = "";
                    if(dt1.Rows[i]["investigation"] != null)
                    {
                        int investigation = Convert.ToInt32((dt1.Rows[i]["investigation"] == System.DBNull.Value ? "0" : dt1.Rows[i]["investigation"]));
                        
                        if (investigation == 1)
                        {
                            iname = "Releveant";
                        }
                        else if (investigation == 2)
                        {
                            iname = "Irrelevant";
                        }
                        else
                        {
                            iname = "Releveant";
                        }
                    }
                    
                    int cno = Convert.ToInt32(complainno);
                    string attachmentdetail = "";
                    if (DB.tbl_DMSAttachmentMst.Where(p => p.TenentID == TID && p.AttachmentById == cno).Count() > 0)
                    {
                        attachmentdetail = DB.tbl_DMSAttachmentMst.Single(p => p.TenentID == TID && p.AttachmentById == cno).AttachmentPath;
                    }

                    int depta = Convert.ToInt32(DB.CRMMainActivities.Single(p => p.TenentID == 10 && p.MasterCODE == cno).TickDepartmentID);
                    string departmet = DB.DeptITSupers.Single(p => p.TenentID == 10 && p.DeptID == depta).DeptName;
                    List<Database.CRMActivity> HWList = DB.CRMActivities.Where(p => p.TenentID == 10 && p.MasterCODE == Master).OrderBy(p => p.MyLineNo).ToList();
                    if (HWList.Count() > 0)
                    {
                        foreach (Database.CRMActivity itemhw in HWList)
                        {
                            if (DB.CRMActivities.Where(p => p.TenentID == TID && p.MasterCODE == Master && p.ActivityID == itemhw.ActivityID && p.Active == "N").Count() > 0)
                            {
                                Database.CRMActivity hwobj = DB.CRMActivities.Single(p => p.TenentID == TID && p.MasterCODE == cno && p.ActivityID == itemhw.ActivityID && p.Active == "N");
                                string activity = hwobj.REMINDERNOTE;
                                string active = dat + " - " + activity;
                                string statu = dat + " - " + "Pending";
                                Tablecontant += "<tr><td><a href=viewticket.aspx?Mastercode" + Master + ">" + cno + "</a></td><td><a href=ViewTicket.aspx?Mastercode=" + Master + ">" + reference + "</a></td> <td><a href=ViewTicket.aspx?Mastercode=" + Master + ">" + UName + "</a> </td> <td><a href=ViewTicket.aspx?Mastercode=" + Master + ">" + MyStatus + " </a></td><td><a href=ViewTicket.aspx?Mastercode=" + Master + ">" + Patientn + " </a></td> <td><a href=ViewTicket.aspx?Mastercode=" + Master + ">" + MRN + "</a> </td><td><a href=ViewTicket.aspx?Mastercode=" + Master + ">" + iname + "</a> </td><td><a href='ViewTicket.aspx?Mastercode='" + Master + ">" + attachmentdetail + "</a> </td><td><a href='ViewTicket.aspx?Mastercode='" + Master + ">" + statu + "</a> </td> </tr>";
                            }
                            else if (DB.CRMActivities.Where(p => p.TenentID == TID && p.MasterCODE == Master && p.ActivityID == itemhw.ActivityID && p.Active == "Y").Count() > 0)
                            {
                                Database.CRMActivity hwobj = DB.CRMActivities.Single(p => p.TenentID == TID && p.MasterCODE == cno && p.ActivityID == itemhw.ActivityID && p.Active == "Y");
                                string activity = hwobj.REMINDERNOTE;
                                string active = dat + " - " + activity;
                                Tablecontant += "<tr><td><a href=viewticket.aspx?Mastercode" + Master + "> </a></td><td><a href=ViewTicket.aspx?Mastercode=" + Master + "></a></td> <td><a href=ViewTicket.aspx?Mastercode=" + Master + "></a>" + UName + " </td> <td><a href=ViewTicket.aspx?Mastercode=" + Master + "> </a></td><td><a href=ViewTicket.aspx?Mastercode=" + Master + "></a></td> <td><a href=ViewTicket.aspx?Mastercode=" + Master + "></a> </td><td><a href=ViewTicket.aspx?Mastercode=" + Master + ">" + iname + "</a> </td><td><a href='ViewTicket.aspx?Mastercode='" + Master + ">" + attachmentdetail + "</a> </td><td><a href='ViewTicket.aspx?Mastercode='" + Master + ">" + active + "</a> </td> </tr>";
                            }
                        }
                        ViewState["depart"] = department;
                    }
                }
                if (Flag == false)
                {
                    if(ViewState["depart"]!="" && ViewState["depart"]!= null)
                    {
                        Tablecontant += " </tbody> </table>";
                        string Ourcontant = " <span style = \"font-family:Arial;font-size:10pt\"> Subject :" + sdt + "<b></b>,<br /><br />This Summary Report for all Department " + sdt1 + "," + Edt1 + " ,  <br /><br />" + Tablecontant + "<br /><br /><br /><br /><br /></span>";
                        //string Tocontant = " <span style = \"font-family:Arial;font-size:10pt\">Dear Sir, <b></b>,<br /><br />DepartmentManager,<br />This email is reference to the Complain recorded on " + sdt + "," + Edt + ",  <br />" + Tablecontant + "<br /><br /><br /><br />Thanks<br /></span>";
                        string subject = ViewState["depart"].ToString();
                        //sendEmail(Tocontant, Email);
                        sendEmail(Ourcontant, Email);

                        // Response.Redirect("reachsilent.aspx");                    
                        return;
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                   
                    return;
                }
            }
        }
        public void sendEmail(string body, string email)
        {

            if (String.IsNullOrEmpty(email))
                return;
            string[] all = email.ToString().Split(',');
            string Sep1 = "";
            for (int a = 0; a <= all.Count() - 1; a++)
            {
                Sep1 = all[a];
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                string subject = ViewState["depart"].ToString();
                msg.Subject = "Summary Report for " + DateTime.Now;
                msg.From = new System.Net.Mail.MailAddress("complaints@royalehayat.com");//("supportteam@digital53.com ");
                msg.To.Add(new System.Net.Mail.MailAddress(Sep1));
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.Body = body;
                msg.IsBodyHtml = true;
                msg.Priority = System.Net.Mail.MailPriority.High;
                System.Net.Mail.SmtpClient smpt = new System.Net.Mail.SmtpClient();
                smpt.UseDefaultCredentials = false;
                smpt.Host = "smtp.siteprotect.com";//for google required smtp.gmail.com and must be check Google Account setting https://myaccount.google.com/lesssecureapps?pli=1 ON//"mail.digital53.com";
                smpt.Port = 587;
                smpt.EnableSsl = true;
                smpt.Credentials = new System.Net.NetworkCredential("complaints@royalehayat.com", "Royal123$");
                smpt.Send(msg);
            }
        }
    }
}
