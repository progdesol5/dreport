using RHReportScheduler.DB;
using RHReportScheduler.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHReportScheduler
{
  public static class DReport1
  {
    public static void ProcessReport()
    {
      string headerBackColor = ConfigurationManager.AppSettings["headerBackColor"];
      DateTime sdt = DateTime.Now.AddDays(-1);
      //DateTime sdt = DateTime.Now.AddDays(-28);//-5
      DateTime Edt = DateTime.Now.AddDays(-1);

      string stdate = sdt.ToString("yyyy-MM-dd");
      string etdate = Edt.ToString("yyyy-MM-dd");
      string timede1 = " 12:00:00 AM";
      string timede2 = " 11:59:59 PM";

      string sdt1 = stdate + timede1;
      string Edt1 = etdate + timede2;

      DataTable dt = DBHelper.GetDataTableFromDB("GetSReport1", sdt1, Edt1, "10", ConfigurationManager.AppSettings["ConnectionString"]);
      DataView viewDept = new DataView(dt);
      DataTable distinctDeptName = viewDept.ToTable(true, "DeptName");
      string TableContant = "";
      int countMail = 0;

      foreach (DataRow dRow in distinctDeptName.Rows)
      {
        TableContant = "";
        if (dRow["DeptName"].ToString() != "")
        {
          string departmentName = dRow["DeptName"].ToString();
          DataTable dtDepatrment = dt.Select("DeptName='" + departmentName + "'").CopyToDataTable();
          string subject = "Department " + departmentName + " Report for " + System.DateTime.Now;
          TableContant = GetHtmlTable(dtDepatrment);
          string Ourcontant = " <span style = \"font-family:Arial;font-size:10pt\"><br />Daily complaints registered report of <b><u>" + departmentName + "</u></b> for the period between <b>" + sdt1 + "</b> and <b>" + Edt1 + "</b>.  <br /><br />" + TableContant + "<br /><br /><span style='color:" + headerBackColor + "'><b>Thanks & Regards,<br/>Complaint Department</span></b><br/></span>";

          DataView view = new DataView(dtDepatrment);
          DataTable distinctValues = view.ToTable(true, "NoticeEmail");
          if (ConfigurationManager.AppSettings["IsTest"] == "true")
          {
              subject = subject + "-TestEmail";
            //EmailHelper.SendEmail(subject, Ourcontant, "johar@writeme.com,yehia.khafaja@royalehayat.com");//sonia25sep@gmail.com

            EmailHelper.SendEmail(subject, Ourcontant, "idrissawerwala@gmail.com,johar@writeme.com,yehia.khafaja@royalehayat.com,ginu.george@royalehayat.com");
            countMail++;
          }
          else
          {
              foreach (DataRow dRowE in distinctValues.Rows)
              {
                  if (dRowE["NoticeEmail"].ToString() != "")
                  {
                      EmailHelper.SendEmail(subject, Ourcontant, dRowE["NoticeEmail"].ToString());
                     // EmailHelper.SendEmail(subject, Ourcontant, "sonia25sep@gmail.com");
                  }
              }
            EmailHelper.SendEmail(subject, Ourcontant, "johar@writeme.com,yehia.khafaja@royalehayat.com,ginu.george@royalehayat.com");
            countMail++;
          }

        }

      }
      /*if (countMail > 0)
      { EmailHelper.SendEmail("RoyalHayat DReport Sent : " + countMail, "Successfully sent :" + countMail, "sayzaf@gmail.com,johar@writeme.com,yehia.khafaja@royalehayat.com"); }
      else { EmailHelper.SendEmail("RoyalHayat DReport Sent : " + countMail, "No Mail sent today :", "sayzaf@gmail.com,johar@writeme.com,yehia.khafaja@royalehayat.com"); }*/
    }
    private static string GetHtmlTable(DataTable dt)
    {
      string domainURL = ConfigurationManager.AppSettings["domainURL"];
      string headerBackColor = ConfigurationManager.AppSettings["headerBackColor"];
      string headerForeColor = ConfigurationManager.AppSettings["headerForeColor"];
      string alternateColor = ConfigurationManager.AppSettings["alternateColor"];
      string tableBorder = ConfigurationManager.AppSettings["tableBorder"];
      string downloadURL = ConfigurationManager.AppSettings["downloadURL"];

      string bodyText = string.Empty;
      StringBuilder html = new StringBuilder();
      try
      {
        //Table start.
        html.Append("<table style='border: " + tableBorder + "px solid black;border-collapse: collapse;' class='table table-bordered table-hover' style='with:100%'><thead>");
        //Building the Header row.
        html.Append("<tr>");
        html.Append("<th style='background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>No</th>");
        html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>Main Complaint Reference / Daily Activity</th>");
        html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>Created By</th>");
        html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>Action Status</th>");
        html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>Patient</th>");
        html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>MRN</th>");
        html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>Investigation Result</th>");
        html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>Attachment Link</th>");
        //html.Append("<th style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + headerBackColor + ";color:" + headerForeColor + ";font-size:14px;font-weight:bold;'>Activity</th>");
        html.Append("</tr></thead><tbody>");
        Boolean Row = false;
        string oldcomplainno = "";
        string olddname = "";
        foreach (DataRow dRow in dt.Rows)
        {
          string complainno = dRow["ComplaintNumber"].ToString();
          int Master = Convert.ToInt32(dRow["MasterCODE"]);
          int department = Convert.ToInt32(dRow["Department"]);
          DateTime dt1 = Convert.ToDateTime(dRow["UploadDate"]);
          DateTime dtMain = Convert.ToDateTime(dRow["MainUploadDate"]);
          int action = Convert.ToInt32(dRow["aspcomment"]);
          string dname = dRow["DeptName"].ToString();
          //if (olddname == dname)
          //    dname = "";
          //else
          //    olddname = dname = dRow["DeptName"].ToString();
          string Patient = dRow["Patient_Name"].ToString();
          string MRN = dRow["MRN"].ToString();
          string MobileNo = dRow["Contact"].ToString();
          string dat = dt1.ToString("HH:mm:ss");
          string datMain = dtMain.ToString("HH:mm:ss");
          string remark = "";
          if (oldcomplainno == complainno)
          {
            remark = dRow["Remarks"].ToString();
          }
          else
          {
            if (dRow["MainRemarks"].ToString() != dRow["Remarks"].ToString())
            {
              remark = datMain + "-" + dRow["MainRemarks"].ToString() + "<br/><br/>" + dat + "-" + dRow["Remarks"].ToString();
            }
            else
              remark = dat + "-" + dRow["Remarks"].ToString();
            oldcomplainno = complainno;
          }
          //string MRN = dt1.Rows[i]["MRN"].ToString();
          // string ActivityPerform = dt1.Rows[i]["ActivityPerform"].ToString();
          string MyStatus = dRow["MyStatus"].ToString();
          string statu = dat + " - " + "Pending";
          string reference = complainno + " " + "-" + dat + "-" + remark;
          string Patientn = "Patient: " + Patient + " Mob:" + MobileNo;
          int ReportedBy = Convert.ToInt32(dRow["ReportedBy"]);
          string UName = dRow["UserFirstName"].ToString();
          int investigation = Convert.ToInt32((dRow["investigation"] == System.DBNull.Value ? "0" : dRow["investigation"]));
          string iname = "";
          if (investigation == 1)
            iname = "Releveant";
          else if (investigation == 3)
            iname = "Irrelevant";
          else
            iname = "Irrelevant";

          string attachlink = dRow["attachmentdetail"] != DBNull.Value ? dRow["attachmentdetail"].ToString() : "";
          string attachmentdetail = "<a href='" + downloadURL + attachlink + "'>" + attachlink + "</a>";

          if (dRow["Active"].ToString() == "Y")
          {
            // DataTable Started
            if (Row)
            {
              html.Append("<tr style='border: " + tableBorder + "px solid black;border-collapse: collapse;background-color:" + alternateColor + ";'>");
              Row = false;
            }
            else
            {
              html.Append("<tr style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
              Row = true;
            }

            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(complainno);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append("<a href=" + domainURL + "ViewTicket.aspx?Mastercode=" + Master + ">" + reference + "</a>");
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(UName);
            html.Append("</td>");
            html.Append("<td style='border: '" + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(MyStatus);
            html.Append("</td>");
            html.Append("<td style=border: '" + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(Patientn);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(MRN);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(iname);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(attachmentdetail);
            html.Append("</td>");
            //html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            //html.Append(statu);
            //html.Append("</td>");
            html.Append("</tr>");
          }
          else
          {
            if (Row)
            {
              html.Append("<tr style=border: '" + tableBorder + "px solid black;border-collapse: collapse;background-color:" + alternateColor + ";'>");
              Row = false;
            }
            else
            {
              html.Append("<tr style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
              Row = true;
            }
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(complainno);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append("<a href=" + domainURL + "ViewTicket.aspx?Mastercode=" + Master + ">" + reference + "</a>");
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(UName);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(MyStatus);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(Patientn);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(MRN);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(iname);
            html.Append("</td>");
            html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            html.Append(attachmentdetail);
            html.Append("</td>");
            //html.Append("<td style='border: " + tableBorder + "px solid black;border-collapse: collapse;'>");
            //html.Append(statu);
            //html.Append("</td>");
            html.Append("</tr>");
          }
        }
        //Table end.
        html.Append("</tbody></table>");
        bodyText += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
        bodyText += "</HEAD><BODY>";
        bodyText += html.ToString();
        bodyText += "</BODY></HTML>";
        return bodyText;
      }
      catch (Exception ex)
      {
        return "";
      }
    }
  }
}
