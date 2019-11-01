using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sparta_Online_Shop
{
    public class Crypto
    {
        public static string Hash(string value)
        {
            if(value == null)
            {
                return "";
            }
            return Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }

        public static string URLSafeHash(string value)
        {
            string Hash = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value))
                );

            Hash = Hash.Replace('+', '-').Replace('/', '_');
            return Hash;
        }
    }
}