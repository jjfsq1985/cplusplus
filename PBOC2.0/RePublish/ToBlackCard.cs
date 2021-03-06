﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using SqlServerHelper;
using System.Data.SqlClient;
using System.Diagnostics;

namespace RePublish
{
    public partial class ToBlackCard : Form
    {
        public enum CardStateSetting
        {
            Unknown = -1,
            CardToNormal = 0, //卡片正常
            CardToLost = 1, //卡片挂失
            CardToRePublish = 2,  //补卡
            CardToClose = 3    //退卡
        }

        private RePublishController m_CardControl = null;

        private const Char Backspace = (Char)8;
        private const Char Key_X = (Char)88;
        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private CardStateSetting m_SettingState = CardStateSetting.Unknown;
        private string m_CardId;

        public string m_SettingName;
        public string m_SettingPersonalID;
        public string m_SettingTel;

        public ToBlackCard()
        {
            InitializeComponent();
        }

        public void SetFormParam(CardStateSetting eState, string strCardId, SqlConnectInfo dbinfo)
        {
            LabelCardType.Visible = false;
            cmbDevType.Visible = false;
            ContactCard.Visible = false;
            btnSetting.Text = "确定";

            m_SettingState = eState;
            m_CardId = strCardId;
            m_DBInfo = dbinfo;
            switch (eState)
            {
                case CardStateSetting.CardToNormal://解挂
                    LabelCardID.Text = "解挂卡号";
                    textCardID.Text = m_CardId;
                    LabelName.Text = "解挂人姓名";
                    LabelPersonalID.Text = "解挂人证件号";
                    LabelTel.Text = "解挂人联系电话";
                    break;
                case CardStateSetting.CardToLost: //挂失
                    LabelCardID.Text = "挂失卡号";
                    textCardID.Text = m_CardId;
                    LabelName.Text  = "挂失人姓名";
                    LabelPersonalID.Text = "挂失人证件号";
                    LabelTel.Text = "挂失人联系电话";
                    break;
                case CardStateSetting.CardToRePublish: //补卡
                    LabelCardID.Text = "失效卡号";
                    textCardID.Text = m_CardId;
                    LabelName.Text = "补卡人姓名";
                    LabelPersonalID.Text = "补卡人证件号";
                    LabelTel.Text = "补卡人联系电话";
                    LabelCardType.Visible = true;
                    cmbDevType.Visible = true;
                    ContactCard.Visible = true;
                    cmbDevType.SelectedIndex = 0;
                    btnSetting.Text = "补卡";
                    break;
                case CardStateSetting.CardToClose: //退卡
                    LabelCardID.Text = "退卡卡号";
                    textCardID.Text  = m_CardId;
                    LabelName.Text   = "退卡人姓名";
                    LabelPersonalID.Text = "退卡人证件号";
                    LabelTel.Text = "退卡人联系电话";
                    break;

            }
        }

        private void textPersonalID_KeyPress(object sender, KeyPressEventArgs e)
        {
            //身份证号只接受数字值和字母X
            if (textPersonalID.Text.Length < 17)
            {
                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                    e.Handled = true;
            }
            else
            {
                if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Key_X)
                    e.Handled = true;
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            m_SettingName = textName.Text;
            m_SettingPersonalID = textPersonalID.Text;
            m_SettingTel = textTel.Text;

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SettingDataBase(ObjSql,m_SettingState);

            ObjSql.CloseConnection();
            ObjSql = null;

            DialogResult = DialogResult.OK;
        }

        private void UpdateCardState(SqlHelper ObjSql, CardStateSetting eSettingState, bool bBlackCard)
        {
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, m_CardId);
            sqlparams[1] = ObjSql.MakeParam("CardState", SqlDbType.Int, 4, ParameterDirection.Input, (int)eSettingState);
            sqlparams[2] = ObjSql.MakeParam("BlackCard", SqlDbType.Bit, 1, ParameterDirection.Input, bBlackCard);
            ObjSql.ExecuteProc("PROC_UpdateCardState", sqlparams);
        }

        private void SettingDataBase(SqlHelper ObjSql, CardStateSetting eSettingState)
        {
            bool bBlackCard = true; //挂失、补卡、退卡都为true
            if (eSettingState == CardStateSetting.CardToNormal)//解挂为false
                bBlackCard = false;

            if (eSettingState == CardStateSetting.CardToRePublish)
            {
                //补卡,返回新卡的卡号                
                m_CardControl = new RePublishController(m_CardId, ContactCard.Checked, cmbDevType.SelectedIndex, m_DBInfo);
                string strRePublishId = m_CardControl.RePublishCard(m_CardId);
                m_CardControl.ReleaseController();
                if (!string.IsNullOrEmpty(strRePublishId))
                {
                    RePublishCardRecord(ObjSql, strRePublishId);
                    UpdateCardState(ObjSql,eSettingState, bBlackCard);
                 }
            }
            else
            {
                //写挂失等的记录表
                InvalidCardRecord(ObjSql);
                UpdateCardState(ObjSql, eSettingState, bBlackCard);
            }
        }

        private void ToBlackCard_Load(object sender, EventArgs e)
        {
            if (m_SettingState == CardStateSetting.CardToRePublish)
            {
                ContactCard.Checked = false;
                ContactCard.Enabled = false;                
            }
        }

        private void cmbDevType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSel = cmbDevType.SelectedIndex;            
            if(nSel == 0)
            {
                ContactCard.Checked = false;
                ContactCard.Enabled = false;
            }
            else
            {
                ContactCard.Enabled = true;
            }

        }

        private void RePublishCardRecord(SqlHelper ObjSql,string strNewCardId)
        {
            string strOpName = GetOperatorName(m_SettingState);
            //写卡记录表OperateCard_Record
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, m_CardId);
            sqlparams[1] = ObjSql.MakeParam("OperateName", SqlDbType.NVarChar, 16, ParameterDirection.Input, strOpName);
            sqlparams[2] = ObjSql.MakeParam("RelatedName", SqlDbType.NVarChar, 50, ParameterDirection.Input, m_SettingName);
            sqlparams[3] = ObjSql.MakeParam("RelatedPersonalId", SqlDbType.VarChar, 32, ParameterDirection.Input, m_SettingPersonalID);
            sqlparams[4] = ObjSql.MakeParam("RelatedTel", SqlDbType.VarChar, 32, ParameterDirection.Input, m_SettingTel);
            sqlparams[5] = ObjSql.MakeParam("RePublishCardId", SqlDbType.Char, 16, ParameterDirection.Input, strNewCardId);
            ObjSql.ExecuteProc("PROC_OperateCard", sqlparams);            
        }

        private string GetOperatorName(CardStateSetting eState)
        {
            string strName = "";
            switch (eState)
            {
                case CardStateSetting.CardToNormal:
                    strName = "解挂";
                    break;
                case CardStateSetting.CardToLost:
                    strName = "挂失";
                    break;
                case CardStateSetting.CardToRePublish:
                    strName = "补卡";
                    break;
                case CardStateSetting.CardToClose:
                    strName = "退卡";
                    break;
            }
            return strName;
        }

        private void InvalidCardRecord(SqlHelper ObjSql)
        {
            //写卡记录表
            string strOpName = GetOperatorName(m_SettingState);
            //写卡记录表OperateCard_Record
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, m_CardId);
            sqlparams[1] = ObjSql.MakeParam("OperateName", SqlDbType.NVarChar, 16, ParameterDirection.Input, strOpName);
            sqlparams[2] = ObjSql.MakeParam("RelatedName", SqlDbType.NVarChar, 50, ParameterDirection.Input, m_SettingName);
            sqlparams[3] = ObjSql.MakeParam("RelatedPersonalId", SqlDbType.VarChar, 32, ParameterDirection.Input, m_SettingPersonalID);
            sqlparams[4] = ObjSql.MakeParam("RelatedTel", SqlDbType.VarChar, 32, ParameterDirection.Input, m_SettingTel);
            sqlparams[5] = ObjSql.MakeParam("RePublishCardId", SqlDbType.Char, 16, ParameterDirection.Input, "");
            ObjSql.ExecuteProc("PROC_OperateCard", sqlparams);        
        }

        private void textTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Backspace)
                e.Handled = true;//不接受非数字值
        }
    }
}
