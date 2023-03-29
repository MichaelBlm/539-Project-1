using System;
using System.IO;
using System.Security.Cryptography;
namespace Part2{
  public class Cryptanalysis{
    static void Main(string[] args)
    {
      
      string plainText = args[0];
      string cipherText = args[1];

      Console.WriteLine(plainText);
      Console.WriteLine(cipherText);
     // Based on scenario time frame is
     // 7/3/2020 11:00 and 7/4/2020 11:00
      var startDate = new DateTime(2020,7,3,11,0,0); 
      var endDate = new DateTime(2020,7,4,11,0,0); 

      // Test each datetime obj
      for(var dt = startDate; dt.Date <= endDate; dt = dt.AddMinutes(1))
      {
        TimeSpan ts = dt.Subtract(new DateTime(1970, 1, 1));
        int seed = (int)ts.TotalMinutes;
        Random rng = new Random(seed);
        byte[] key = BitConverter.GetBytes(rng.NextDouble());
        string result = Encrypt(key, plainText);
        if(result == cipherText)
        {
          Console.WriteLine($"seed:{seed}");
          break;
        }      
      }
    }
    #pragma warning disable SYSLIB0021
    private static string Encrypt(byte[] key, string secretString)
{
      DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
      MemoryStream ms = new MemoryStream();
      CryptoStream cs = new CryptoStream(ms,
      csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
      StreamWriter sw = new StreamWriter(cs);
      sw.Write(secretString);
      sw.Flush();
      cs.FlushFinalBlock();
      sw.Flush();
      return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
}

    
  }
}
