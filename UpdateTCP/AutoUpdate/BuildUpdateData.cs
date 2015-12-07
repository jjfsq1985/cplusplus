using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoUpdate
{
    class BuildUpdateData
    {
        private int m_nFrameId = 0;

        public byte[] CreateHeader(UpdateFileParam FilePar, int nSegmentSize,int nUpdateDevCnt)
        {
            byte[] header = new byte[15];//无CRC16和转义
            header[0] = 0xFA;            
            header[1] = 0x00;
            header[2] = 0x00;
            header[3] = 0; //帧号
            byte[] validLen = UInt16ToBcd((ushort)9);//命令字2D有效数据长度9
            header[4] = validLen[0];
            header[5] = validLen[1];
            //有效数据
            header[6] = 0x2D;
            header[7] = (byte)FilePar.eType;
            header[8] = nUpdateDevCnt > 0 ? (byte)2 : (byte)0;
            byte[] FileSize = BitConverter.GetBytes(FilePar.FileLen);
            header[9] = FileSize[0];
            header[10] = FileSize[1];
            header[11] = FileSize[2];
            header[12] = FileSize[3];
            byte[] segmentSize = BitConverter.GetBytes((UInt16)nSegmentSize);
            header[13] = segmentSize[0];
            header[14] = segmentSize[1];
            return header;
        }

        private byte[] UInt16ToBcd(UInt16 nVal)
        {
            try
            {
                string strData = nVal.ToString("D4");
                byte[] byteBCD = new byte[2];
                for (int i = 0; i < 2; i++)
                {
                    byte bcdbyte = 0;
                    byte.TryParse(strData.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber, null, out bcdbyte);
                    byteBCD[i] = bcdbyte;
                }
                return byteBCD;
            }
            catch
            {
                return new byte[2];
            }
        }

        public byte[] CreateBody(UpdateFileParam FilePar, int nOffset,int nBodyLen)
        {
            m_nFrameId++;
            if (m_nFrameId >= 0x7F)
                m_nFrameId = 0;
            int nPacketLen = nBodyLen + 12;//无CRC16和转义
            byte[] bodyData = new byte[nPacketLen];
            bodyData[0] = 0xFA;
            bodyData[1] = 0x00;
            bodyData[2] = 0x00;
            bodyData[3] = (byte)m_nFrameId; //帧号
            //命令字2E有效数据长度
            bodyData[4] = 0;
            bodyData[5] = 0;
            //有效数据
            bodyData[6] = 0x2E;
            bodyData[7] = (byte)FilePar.eType;
            byte[] byteOffset = BitConverter.GetBytes(nOffset);
            bodyData[8] = byteOffset[0];
            bodyData[9] = byteOffset[1];
            bodyData[10] = byteOffset[2];
            bodyData[11] = byteOffset[3];
            byte[] fileData = ReadBinFile(FilePar.strFileFullPath,nOffset,nBodyLen);                
            Buffer.BlockCopy(fileData, 0, bodyData, 12, fileData.Length);
            //最后一包使用实际长度
            if(fileData.Length == nBodyLen)
            {
                byte[] validLen = UInt16ToBcd((ushort)(nBodyLen + 6));//命令字2E有效数据长度
                bodyData[4] = validLen[0];
                bodyData[5] = validLen[1];
                return bodyData;
            }
            else
            {
                byte[] validLen = UInt16ToBcd((ushort)(fileData.Length + 6));//命令字2E有效数据长度
                bodyData[4] = validLen[0];
                bodyData[5] = validLen[1];
                int nRealLen = 12 + fileData.Length;
                byte[] retBuffer = new byte[nRealLen];
                Buffer.BlockCopy(bodyData, 0, retBuffer, 0, nRealLen);
                return retBuffer;
            }
        }

        private byte[] ReadBinFile(string strFile, int nOffset, int nLen)
        {
            if (!File.Exists(strFile))
                return null;
            FileStream fs = File.Open(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(nOffset, SeekOrigin.Begin);
            byte[] data = new byte[nLen];
            int nReadLen = fs.Read(data, 0, nLen);
            fs.Close();
            if (nReadLen == nLen)
            {
                return data;
            }
            else
            {
                byte[] retBuffer = new byte[nReadLen];
                Buffer.BlockCopy(data, 0, retBuffer, 0, nReadLen);
                return retBuffer;
            }
        }
    }
}
