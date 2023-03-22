using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Drawing.Text;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace SSE_R
{
        //
    public class BugReport
    {
        public static void CreateIssue()
        {
            Form bug = new bugReportForm();
            Application.Run(bug);
        }
    }

}
