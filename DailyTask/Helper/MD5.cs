using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace DailyTask.Helper
{
    public class MD5Helper
    {
        public bool IsMD5Modified<T>(T obj, string originalMD5Info)
        {
            if (obj == null)
                return false;

            MemoryStream memoryStream = new MemoryStream();
            DataContractSerializer dcs = new DataContractSerializer(obj.GetType());
            dcs.WriteObject(memoryStream, obj);

            byte[] fromData = memoryStream.ToArray();
            MD5 md5Provider = new MD5CryptoServiceProvider();
            if (BitConverter.ToString(md5Provider.ComputeHash(fromData)) == originalMD5Info)
                return false;
            else
                return true;
        }
        public string GenerateMD5<T>(T obj)
        {
            if (obj == null)
                return null;

            MemoryStream memoryStream = new MemoryStream();
            DataContractSerializer dcs = new DataContractSerializer(obj.GetType());
            dcs.WriteObject(memoryStream, obj);

            byte[] fromData = memoryStream.ToArray();
            MD5 md5Provider = new MD5CryptoServiceProvider();

            return BitConverter.ToString(md5Provider.ComputeHash(fromData));
        }
    }
}
