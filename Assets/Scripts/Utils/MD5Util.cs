using System.Linq;
using UnityEngine;

namespace Utils
{
    public class MD5Util
    {
        public static string Md5Sum(string strToEncrypt)
        {
            var ue = new System.Text.UTF8Encoding();
            var bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            var md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            var hashBytes = md5.ComputeHash(bytes);

            return hashBytes.Aggregate("", (current, t) => current + System.Convert.ToString(t, 16).PadLeft(2, '0'));
        }
    }
}