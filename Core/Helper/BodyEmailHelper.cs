using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Core.Helper
{
    public class BodyEmailHelper
    {
        public string Register(string email, string pass, string fullname)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\Register.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            string body = MailText.Replace("[FullName]", fullname).Replace("[Email]", email).Replace("[Password]", pass);
            return body;
        }

        public string ResetPass( string pass, string fullname)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\ResetPass.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            string body = MailText.Replace("[FullName]", fullname).Replace("[Password]", pass);

            return body;
        }
    }
}
