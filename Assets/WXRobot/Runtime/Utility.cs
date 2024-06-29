using System;
using System.Security.Cryptography;
using System.Text;

namespace WXRobot.Runtime
{
    public static class Utility
    {
        public static string ToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        
        public static string Md5(byte[] bytes)
        {
            var md5 = MD5.Create();
            var computeHash = md5.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var b in computeHash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}