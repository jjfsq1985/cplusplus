using System;
using System.Collections.Generic;
using System.Text;

namespace ApduParam
{
    public class IccCardInfoParam
    {
        private string m_strIccCardID;
        public string PSAMCardID
        {
            get { return m_strIccCardID; }
            set { m_strIccCardID = value; }
        }

        private int m_nClientId;
        public int ClientID
        {
            get { return m_nClientId; }
            set { m_nClientId = value; }
        }

        private DateTime m_ValidAppForm;
        public DateTime ValidAppForm
        {
            get { return m_ValidAppForm; }
            set { m_ValidAppForm = value; }
        }

        private DateTime m_ValidAppTo;
        public DateTime ValidAppTo
        {
            get { return m_ValidAppTo; }
            set { m_ValidAppTo = value; }
        }

        private string m_strTermialID;
        public string TermialID
        {
            get { return m_strTermialID; }
            set { m_strTermialID = value; }
        }

        private string m_strCompanyIssue;
        public string CompanyIssue
        {
            get { return m_strCompanyIssue; }
            set { m_strCompanyIssue = value; }
        }

        private string m_strCompanyRecv;
        public string CompanyRecv
        {
            get { return m_strCompanyRecv; }
            set { m_strCompanyRecv = value; }
        }

        private string m_strRemark;
        public string Remark
        {
            get { return m_strRemark; }
            set { m_strRemark = value; }
        }

        public IccCardInfoParam()
        {
            m_nClientId = 1;
            m_strIccCardID = "2010010100000001";
            m_ValidAppForm = new DateTime(2010, 1, 1);
            m_ValidAppTo = new DateTime(2020, 12, 31);
            m_strTermialID = "201001010001";
            m_strCompanyIssue = "10FFFFFFFFFFFFFF";
            m_strCompanyRecv = "35FFFFFFFFFFFFFF";
            m_strRemark = "";
        }

        public byte[] GetBytePsamId()
        {
            int nLen = m_strIccCardID.Length;
            if (nLen != 16)
                return null;
            int nByteSize = nLen / 2;
            byte[] byteCardId = new byte[nByteSize];

            for (int i = 0; i < nByteSize; i++)
            {
                byteCardId[i] = Convert.ToByte(m_strIccCardID.Substring(i * 2, 2), 16);
            }
            return byteCardId;
        }

        public byte[] GetByteTermId()
        {
            int nLen = m_strTermialID.Length;
            if (nLen != 12)
                return null;
            int nByteSize = nLen / 2;
            byte[] byteId = new byte[nByteSize];

            for (int i = 0; i < nByteSize; i++)
            {
                byteId[i] = Convert.ToByte(m_strTermialID.Substring(i * 2, 2), 16);
            }
            return byteId;
        }

        public byte[] GetByteCompanyIssue()
        {
            int nLen = m_strCompanyIssue.Length;
            if (nLen != 16)
                return null;
            int nByteSize = nLen / 2;
            byte[] byteId = new byte[nByteSize];

            for (int i = 0; i < nByteSize; i++)
            {
                byteId[i] = Convert.ToByte(m_strCompanyIssue.Substring(i * 2, 2), 16);
            }
            return byteId;
        }

        public byte[] GetByteCompanyRecv()
        {
            int nLen = m_strCompanyRecv.Length;
            if (nLen != 16)
                return null;
            int nByteSize = nLen / 2;
            byte[] byteId = new byte[nByteSize];

            for (int i = 0; i < nByteSize; i++)
            {
                byteId[i] = Convert.ToByte(m_strCompanyRecv.Substring(i * 2, 2), 16);
            }
            return byteId;
        }
    }
}
