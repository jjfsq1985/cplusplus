using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PcscLH;

namespace TestLhPcsc
{
    public partial class TestPcscForm : Form
    {
        PcscSmardCard SmartCard = new PcscSmardCard();
        public TestPcscForm()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cmbReaderName.Items.Count <= 0)
                return;
            int nIndex = cmbReaderName.SelectedIndex;
            string strReaderName = (string)cmbReaderName.Items[nIndex];
            byte[] CardAtr = null;
            SmartCard.LH_ConnectReader(strReaderName, out CardAtr);
            if (CardAtr != null)
                textAtr.Text = BitConverter.ToString(CardAtr).Replace("-", "");
            else
                textAtr.Text = "未读到卡片";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            SmartCard.LH_DisconnectReader();
        }

        private void btnEstablish_Click(object sender, EventArgs e)
        {
            List<string> Readers = new List<string>();
            SmartCard.LH_Open(ref Readers);
            cmbReaderName.Items.Clear();
            foreach (string strReaderName in Readers)
            {
                cmbReaderName.Items.Add(strReaderName);
            }
            if (Readers.Count > 0)
                cmbReaderName.SelectedIndex = 0;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            SmartCard.LH_Close();
        }
    }
}
