using System;
using System.Collections.Generic;
using System.Text;

namespace PublishCardX
{
    public class PublicFunc
    {
        //获取当前BCD码格式的系统时间
        public static byte[] GetBCDTime()
        {
            string strTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            int nByteSize = strTime.Length / 2;
            byte[] byteBCD = new byte[nByteSize];
            for (int i = 0; i < nByteSize; i++)
            {
                byteBCD[i] = Convert.ToByte(strTime.Substring(i * 2, 2), 16);
            }
            return byteBCD;
        }

        public static bool ByteDataEquals(byte[] byteL, byte[] byteR)
        {
            if (byteL.Length != byteR.Length)
                return false;
            for (int i = 0; i < byteL.Length; i++)
            {
                if (byteL[i] != byteR[i])
                    return false;
            }
            return true;
        }

        public static byte[] StringToBCD(string strData)
        {
            if (string.IsNullOrEmpty(strData) || strData.Length % 2 != 0)
                return null;
            try
            {
                int nByteSize = strData.Length / 2;
                byte[] byteBCD = new byte[nByteSize];
                for (int i = 0; i < nByteSize; i++)
                {
                    byte bcdbyte = 0;
                    byte.TryParse(strData.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber, null, out bcdbyte);
                    byteBCD[i] = bcdbyte;
                }
                return byteBCD;
            }
            catch
            {
                return null;
            }
        }

    }
}
