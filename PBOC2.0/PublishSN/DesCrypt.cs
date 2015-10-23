using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace PublishSN
{
    class DesCrypt
    {
        //加密方法： 8字节为1块。密钥左半部分对数据进行加密，然后用右半部分解密，再用左半部分加密得到结果
        public static byte[] TripleEncryptData(byte[] byteSrc, byte[] TripleKey)
        {
            if (byteSrc.Length % 8 != 0)
                return byteSrc;

            int nSrcLen = byteSrc.Length;

            byte[] KeyLeft = new byte[8];
            byte[] KeyRight = new byte[8];
            Buffer.BlockCopy(TripleKey, 0, KeyLeft, 0, 8);
            Buffer.BlockCopy(TripleKey, 8, KeyRight, 0, 8);

            byte[] DataReturn = new byte[nSrcLen];
            byte[] calcData = new byte[8];

            int nCalcOffset = 0;
            int nCalcCount = nSrcLen / 8;//必然能整除
            for (int nCalcIndex = 0; nCalcIndex < nCalcCount; nCalcIndex++)
            {
                Buffer.BlockCopy(byteSrc, nCalcOffset, calcData, 0, 8);
                EncryptData(calcData, KeyLeft);//用密钥的左半部来进行加密操作
                DecryptData(calcData, KeyRight);//右半部解密
                EncryptData(calcData, KeyLeft); //左半部加密
                Buffer.BlockCopy(calcData, 0, DataReturn, nCalcOffset, 8);
                nCalcOffset += 8;
            }

            return DataReturn;
        }


        //解密方法： 8字节为1块。密钥左半部分对数据进行解密，然后用右半部分加密，再用左半部分解密得到结果
        public static byte[] TripleDecryptData(byte[] byteSrc, byte[] TripleKey)
        {
            if (byteSrc.Length % 8 != 0)
                return byteSrc;

            int nSrcLen = byteSrc.Length;

            byte[] KeyLeft = new byte[8];
            byte[] KeyRight = new byte[8];
            Buffer.BlockCopy(TripleKey, 0, KeyLeft, 0, 8);
            Buffer.BlockCopy(TripleKey, 8, KeyRight, 0, 8);

            byte[] DataReturn = new byte[nSrcLen];
            byte[] calcData = new byte[8];

            int nCalcOffset = 0;
            int nCalcCount = nSrcLen / 8;//必然能整除
            for (int nCalcIndex = 0; nCalcIndex < nCalcCount; nCalcIndex++)
            {
                Buffer.BlockCopy(byteSrc, nCalcOffset, calcData, 0, 8);
                DecryptData(calcData, KeyLeft);//用密钥的左半部来进行解密操作
                EncryptData(calcData, KeyRight);//右半部加密
                DecryptData(calcData, KeyLeft);//再左半边解密
                Buffer.BlockCopy(calcData, 0, DataReturn, nCalcOffset, 8);
                nCalcOffset += 8;
            }

            return DataReturn;
        }

        //DES加密
        public static void EncryptData(byte[] byteData, byte[] Key)
        {
            if (byteData.Length != 8 || Key.Length != 8)
            {
                Trace.WriteLine("DES加密的数据长度或密钥长度不正确");
                return;
            }

            DESCryptoServiceProvider alg = new DESCryptoServiceProvider();

            alg.Padding = PaddingMode.Zeros; //全0填充，使输入输出长度一致
            alg.Key = Key;//密钥
            alg.IV = new byte[8];//初始化向量，全0

            ICryptoTransform ITrans = alg.CreateEncryptor(alg.Key, alg.IV);
            byte[] outData = ITrans.TransformFinalBlock(byteData, 0, byteData.Length);
            Buffer.BlockCopy(outData, 0, byteData, 0, 8);
            ITrans.Dispose();
            alg.Clear();
        }

        //DES解密
        public static void DecryptData(byte[] byteData, byte[] Key)
        {
            if (byteData.Length != 8 || Key.Length != 8)
            {
                Trace.WriteLine("DES解密的数据长度或密钥长度不正确");
                return;
            }

            DESCryptoServiceProvider alg = new DESCryptoServiceProvider();
            alg.Padding = PaddingMode.Zeros; //全0填充，使输入输出长度一致
            alg.Key = Key;//密钥
            alg.IV = new byte[8];//初始化向量，全0

            ICryptoTransform ITrans = alg.CreateDecryptor(alg.Key, alg.IV);
            byte[] outData = ITrans.TransformFinalBlock(byteData, 0, byteData.Length);
            Buffer.BlockCopy(outData, 0, byteData, 0, 8);
            ITrans.Dispose();
            alg.Clear();
        }
    }
}
