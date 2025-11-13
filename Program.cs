using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHReportScheduler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (ConfigurationManager.AppSettings["ReportName"] == "SReport1")
                SReport1.ProcessReport();
            if (ConfigurationManager.AppSettings["ReportName"] == "DReport1")
                DReport1.ProcessReport();
        }
    }
}
