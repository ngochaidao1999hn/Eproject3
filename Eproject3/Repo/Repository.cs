using Eproject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eproject3.Repo
{
    public class Repository
    {
        public string HashPwd(string input)
        {
            System.Security.Cryptography.MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public bool check(string extension, string[] format)
        {
            foreach (string exten in format)
            {
                if (extension.Contains(exten))
                {
                    return true;
                }
            }
            return false;
        }
    }

}