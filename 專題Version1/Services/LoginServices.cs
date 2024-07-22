using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace 專題Version1.Services
{
    public class LoginServices
    {

        public static string EncryptPassword(string password)
        {
            // 使用 SHA256 演算法
            using (SHA256 sha256 = SHA256.Create())
            {
                // 將輸入的密碼轉換為字節陣列，並計算其哈希值
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // 建立一個 StringBuilder 來儲存哈希值的 16 進制表示
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // 將每個位元組轉換為 16 進制字串並附加到 StringBuilder 中
                    builder.Append(bytes[i].ToString("x2"));
                }

                // 返回最終的 16 進制哈希值字串
                return builder.ToString();
            }
        }
    }
}