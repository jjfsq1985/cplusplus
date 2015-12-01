using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Xml;

namespace CustomMessageBox
{
    public partial class MyMessageBox : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool MessageBeep(uint type);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public extern static int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, int nIcons);

        static private IntPtr[] largeIcon;
        static private IntPtr[] smallIcon;

        static private MyMessageBox MyMsgBox;
        static private Label frmTitle;
        static private Label frmMessage;
        static private Label frmFontStr;
        static private Label frmNextMessage;
        static private PictureBox pIcon;
        static private FlowLayoutPanel flpButtons;
        static private Icon frmIcon;

        static private Button btnOK;
        static private Button btnAbort;
        static private Button btnRetry;
        static private Button btnIgnore;
        static private Button btnCancel;
        static private Button btnYes;
        static private Button btnNo;

        static private DialogResult MsgReturn;    

        public enum MyMsgIcon
        {
            Error,
            Explorer,
            Find,
            Information,
            Mail,
            Media,
            Print,
            Question,
            RecycleBinEmpty,
            RecycleBinFull,
            Stop,
            User,
            Warning
        }

        public enum MyMsgButtons
        {
            AbortRetryIgnore,
            OK,
            OKCancel,
            RetryCancel,
            YesNo,
            YesNoCancel
        }

        public MyMessageBox()
        {            
        }

        static private void BuildMessageBox(string title)
        {
            MyMsgBox = new MyMessageBox();
            MyMsgBox.Text = title;
            MyMsgBox.Size = new System.Drawing.Size(400, 200);
            MyMsgBox.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            MyMsgBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MyMsgBox.Paint += new PaintEventHandler(MyMsgBox_Paint);
            MyMsgBox.BackColor = System.Drawing.Color.White;

            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.RowCount = 3;
            tlp.ColumnCount = 0;
            tlp.Dock = System.Windows.Forms.DockStyle.Fill;
            tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22));
            tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));
            tlp.BackColor = System.Drawing.Color.Transparent;
            tlp.Padding = new Padding(2, 5, 2, 2);

            frmTitle = new Label();
            frmTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            frmTitle.BackColor = System.Drawing.Color.Transparent;
            frmTitle.ForeColor = System.Drawing.Color.White;
            frmTitle.Font = new Font("宋体", 10, FontStyle.Bold);

            frmMessage = new Label();
            frmMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            frmMessage.BackColor = System.Drawing.Color.White;
            frmMessage.Font = new Font("宋体", 10, FontStyle.Regular);
            frmMessage.Text = "前消息";

            frmFontStr = new Label();
            frmFontStr.Dock = System.Windows.Forms.DockStyle.Fill;
            frmFontStr.BackColor = System.Drawing.Color.White;
            frmFontStr.Font = new Font("宋体", 10, FontStyle.Regular);
            frmFontStr.Text = "指定消息";

            frmNextMessage = new Label();
            frmNextMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            frmNextMessage.BackColor = System.Drawing.Color.White;
            frmNextMessage.Font = new Font("宋体", 10, FontStyle.Regular);
            frmNextMessage.Text = "后消息";

            largeIcon = new IntPtr[250];
            smallIcon = new IntPtr[250];
            pIcon = new PictureBox();
            ExtractIconEx("shell32.dll", 0, largeIcon, smallIcon, 250);

            flpButtons = new FlowLayoutPanel();
            flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flpButtons.Padding = new Padding(0, 5, 5, 0);
            flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flpButtons.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            TableLayoutPanel tlpMessagePanel = new TableLayoutPanel();
            tlpMessagePanel.BackColor = System.Drawing.Color.White;
            tlpMessagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMessagePanel.ColumnCount = 2;
            tlpMessagePanel.RowCount = 0;
            tlpMessagePanel.Padding = new Padding(4, 5, 4, 4);
            tlpMessagePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50));
            tlpMessagePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            TableLayoutPanel tlpMyPanel = new TableLayoutPanel();
            tlpMyPanel.BackColor = System.Drawing.Color.White;
            tlpMyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMyPanel.ColumnCount = 0;
            tlpMyPanel.RowCount = 3;
            tlpMyPanel.Padding = new Padding(4, 5, 4, 4);
            tlpMyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMyPanel.Controls.Add(frmMessage);
            tlpMyPanel.Controls.Add(frmFontStr);
            tlpMyPanel.Controls.Add(frmNextMessage);
            tlpMessagePanel.Controls.Add(pIcon);
            tlpMessagePanel.Controls.Add(tlpMyPanel);

            tlp.Controls.Add(frmTitle);
            tlp.Controls.Add(tlpMessagePanel);
            tlp.Controls.Add(flpButtons);
            MyMsgBox.Controls.Add(tlp);
        }

        static public Color GetColor(string strColor)
        {
            if (strColor == "red")
            {
                return System.Drawing.Color.Red;
            }
            else if(strColor == "green")
            {
                return System.Drawing.Color.Green;
            }
            else if(strColor == "blue")
            {
                return System.Drawing.Color.Blue;
            }
            else if(strColor == "gray")
            {
                return System.Drawing.Color.Gray;
            }
            else if (strColor == "black")
            {
                return System.Drawing.Color.Black;
            }
            else
            {
                return System.Drawing.Color.FromArgb(Convert.ToInt32(strColor));
            }

        }

        /// <summary>
        /// Message: Text to display in the message box.
        /// </summary>
        static public DialogResult Show(string Message)
        {
            BuildMessageBox("");

            int nBeginXml = Message.IndexOf("<");
            int nEndXml = Message.LastIndexOf(">");
            if (nBeginXml != -1 && nEndXml != -1)
            {
                string strXml = Message.Substring(nBeginXml, nEndXml - nBeginXml + 1);
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(strXml);
                    XmlNode node = xml.SelectSingleNode("font");
                    frmMessage.Text = Message.Substring(0, nBeginXml);
                    frmFontStr.Text = node.InnerText;
                    frmFontStr.Font = new Font("宋体", Convert.ToSingle(node.Attributes["size"].Value), FontStyle.Regular);
                    frmFontStr.ForeColor = GetColor(node.Attributes["color"].Value);                    
                    frmNextMessage.Text = Message.Substring(nEndXml + 1, Message.Length - nEndXml - 1);
                }
                catch
                {
                    frmMessage.Text = Message;
                    frmFontStr.Text = "";
                    frmNextMessage.Text = "";
                }
            }
            else
            {
                frmMessage.Text = Message;
                frmFontStr.Text = "";
                frmNextMessage.Text = "";
            }

            ShowOKButton();
            MyMsgBox.ShowDialog();
            return MsgReturn;
        }

        /// <summary>
        /// Title: Text to display in the title bar of the messagebox.
        /// </summary>
        static public DialogResult Show(string Message, string Title)
        {
            BuildMessageBox(Title);
            frmTitle.Text = Title;
            frmMessage.Text = Message;
            ShowOKButton();
            MyMsgBox.ShowDialog();
            return MsgReturn;
        }

        /// <summary>
        /// MButtons: Display CYButtons on the message box.
        /// </summary>
        static public DialogResult Show(string Message, string Title, MyMsgButtons MButtons)
        {
            BuildMessageBox(Title); // BuildMessageBox method, responsible for creating the MessageBox
            frmTitle.Text = Title; // Set the title of the MessageBox

            int nBeginXml = Message.IndexOf("<");
            int nEndXml = Message.LastIndexOf(">");
            if (nBeginXml != -1 && nEndXml != -1)
            {
                string strXml = Message.Substring(nBeginXml, nEndXml - nBeginXml + 1);
                try
                {
                    XmlAttribute CustomAttrib = null;
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(strXml);
                    XmlNode node = xml.SelectSingleNode("font");
                    frmMessage.Text = Message.Substring(0, nBeginXml);
                    frmFontStr.Text = node.InnerText;
                    CustomAttrib = node.Attributes["size"];
                    if (CustomAttrib != null)
                        frmFontStr.Font = new Font("宋体", Convert.ToSingle(CustomAttrib.Value), FontStyle.Regular);
                    CustomAttrib = node.Attributes["color"];
                    if(CustomAttrib != null)
                        frmFontStr.ForeColor = GetColor(CustomAttrib.Value);
                    frmNextMessage.Text = Message.Substring(nEndXml + 1, Message.Length - nEndXml - 1);
                }
                catch
                {
                    frmMessage.Text = Message;
                    frmFontStr.Text = "";
                    frmNextMessage.Text = "";
                }
            }
            else
            {
                frmMessage.Text = Message;
                frmFontStr.Text = "";
                frmNextMessage.Text = "";
            } 
            
            ButtonStatements(MButtons); // ButtonStatements method is responsible for showing the appropreiate buttons
            MyMsgBox.ShowDialog(); // Show the MessageBox as a Dialog.
            return MsgReturn; // Return the button click as an Enumerator
        }

        /// <summary>
        /// MIcon: Display MyMsgIcon on the message box.
        /// </summary>
        static public DialogResult Show(string Message, string Title, MyMsgButtons MButtons, MyMsgIcon MIcon)
        {
            BuildMessageBox(Title);
            frmTitle.Text = Title;

            int nBeginXml = Message.IndexOf("<");
            int nEndXml = Message.LastIndexOf(">");
            if (nBeginXml != -1 && nEndXml != -1)
            {
                string strXml = Message.Substring(nBeginXml, nEndXml - nBeginXml + 1);
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(strXml);
                    XmlNode node = xml.SelectSingleNode("font");
                    frmMessage.Text = Message.Substring(0, nBeginXml);
                    frmFontStr.Text = node.InnerText;
                    frmFontStr.Font = new Font("宋体", Convert.ToSingle(node.Attributes["size"].Value), FontStyle.Regular);
                    frmFontStr.ForeColor = GetColor(node.Attributes["color"].Value);
                    frmNextMessage.Text = Message.Substring(nEndXml + 1, Message.Length - nEndXml - 1);
                }
                catch
                {
                    frmMessage.Text = Message;
                    frmFontStr.Text = "";
                    frmNextMessage.Text = "";
                }
            }
            else
            {
                frmMessage.Text = Message;
                frmFontStr.Text = "";
                frmNextMessage.Text = "";
            }

            ButtonStatements(MButtons);
            IconStatements(MIcon);
            Image imageIcon = new Bitmap(frmIcon.ToBitmap(), 38, 38);
            pIcon.Image = imageIcon;
            MyMsgBox.ShowDialog();
            return MsgReturn;
        }

        static void btnOK_Click(object sender, EventArgs e)
        {
            MsgReturn = DialogResult.OK;
            MyMsgBox.Dispose();
        }

        static void btnAbort_Click(object sender, EventArgs e)
        {
            MsgReturn = DialogResult.Abort;
            MyMsgBox.Dispose();
        }

        static void btnRetry_Click(object sender, EventArgs e)
        {
            MsgReturn = DialogResult.Retry;
            MyMsgBox.Dispose();
        }

        static void btnIgnore_Click(object sender, EventArgs e)
        {
            MsgReturn = DialogResult.Ignore;
            MyMsgBox.Dispose();
        }

        static void btnCancel_Click(object sender, EventArgs e)
        {
            MsgReturn = DialogResult.Cancel;
            MyMsgBox.Dispose();
        }

        static void btnYes_Click(object sender, EventArgs e)
        {
            MsgReturn = DialogResult.Yes;
            MyMsgBox.Dispose();
        }

        static void btnNo_Click(object sender, EventArgs e)
        {
            MsgReturn = DialogResult.No;
            MyMsgBox.Dispose();
        }

        static private void ShowOKButton()
        {
            btnOK = new Button();
            btnOK.Text = "OK";
            btnOK.Size = new System.Drawing.Size(80, 25);
            btnOK.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnOK.Font = new Font("宋体", 9, FontStyle.Regular);
            btnOK.Click += new EventHandler(btnOK_Click);
            flpButtons.Controls.Add(btnOK);
        }

        static private void ShowAbortButton()
        {
            btnAbort = new Button();
            btnAbort.Text = "Abort";
            btnAbort.Size = new System.Drawing.Size(80, 25);
            btnAbort.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnAbort.Font = new Font("宋体", 9, FontStyle.Regular);
            btnAbort.Click += new EventHandler(btnAbort_Click);
            flpButtons.Controls.Add(btnAbort);
        }

        static private void ShowRetryButton()
        {
            btnRetry = new Button();
            btnRetry.Text = "Retry";
            btnRetry.Size = new System.Drawing.Size(80, 25);
            btnRetry.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnRetry.Font = new Font("宋体", 9, FontStyle.Regular);
            btnRetry.Click += new EventHandler(btnRetry_Click);
            flpButtons.Controls.Add(btnRetry);
        }

        static private void ShowIgnoreButton()
        {
            btnIgnore = new Button();
            btnIgnore.Text = "Ignore";
            btnIgnore.Size = new System.Drawing.Size(80, 25);
            btnIgnore.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnIgnore.Font = new Font("宋体", 9, FontStyle.Regular);
            btnIgnore.Click += new EventHandler(btnIgnore_Click);
            flpButtons.Controls.Add(btnIgnore);
        }

        static private void ShowCancelButton()
        {
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new System.Drawing.Size(80, 25);
            btnCancel.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnCancel.Font = new Font("宋体", 9, FontStyle.Regular);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            flpButtons.Controls.Add(btnCancel);
        }

        static private void ShowYesButton()
        {
            btnYes = new Button();
            btnYes.Text = "Yes";
            btnYes.Size = new System.Drawing.Size(80, 25);
            btnYes.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnYes.Font = new Font("宋体", 9, FontStyle.Regular);
            btnYes.Click += new EventHandler(btnYes_Click);
            flpButtons.Controls.Add(btnYes);
        }

        static private void ShowNoButton()
        {
            btnNo = new Button();
            btnNo.Text = "No";
            btnNo.Size = new System.Drawing.Size(80, 25);
            btnNo.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            btnNo.Font = new Font("宋体", 9, FontStyle.Regular);
            btnNo.Click += new EventHandler(btnNo_Click);
            flpButtons.Controls.Add(btnNo);
        }

        static private void ButtonStatements(MyMsgButtons MButtons)
        {
            if (MButtons == MyMsgButtons.AbortRetryIgnore)
            {
                ShowIgnoreButton();
                ShowRetryButton();
                ShowAbortButton();
            }

            if (MButtons == MyMsgButtons.OK)
            {
                ShowOKButton();
            }

            if (MButtons == MyMsgButtons.OKCancel)
            {
                ShowCancelButton();
                ShowOKButton();
            }

            if (MButtons == MyMsgButtons.RetryCancel)
            {
                ShowCancelButton();
                ShowRetryButton();
            }

            if (MButtons == MyMsgButtons.YesNo)
            {
                ShowNoButton();
                ShowYesButton();
            }

            if (MButtons == MyMsgButtons.YesNoCancel)
            {
                ShowCancelButton();
                ShowNoButton();
                ShowYesButton();
            }
        }

        static private void IconStatements(MyMsgIcon MIcon)
        {
            if (MIcon == MyMsgIcon.Error)
            {
                MessageBeep(30);
                frmIcon = Icon.FromHandle(largeIcon[109]);
            }

            if (MIcon == MyMsgIcon.Explorer)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[220]);
            }

            if (MIcon == MyMsgIcon.Find)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[22]);
            }

            if (MIcon == MyMsgIcon.Information)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[221]);
            }

            if (MIcon == MyMsgIcon.Mail)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[156]);
            }

            if (MIcon == MyMsgIcon.Media)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[116]);
            }

            if (MIcon == MyMsgIcon.Print)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[136]);
            }

            if (MIcon == MyMsgIcon.Question)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[23]);
            }

            if (MIcon == MyMsgIcon.RecycleBinEmpty)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[31]);
            }

            if (MIcon == MyMsgIcon.RecycleBinFull)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[32]);
            }

            if (MIcon == MyMsgIcon.Stop)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[27]);
            }

            if (MIcon == MyMsgIcon.User)
            {
                MessageBeep(0);
                frmIcon = Icon.FromHandle(largeIcon[170]);
            }

            if (MIcon == MyMsgIcon.Warning)
            {
                MessageBeep(30);
                frmIcon = Icon.FromHandle(largeIcon[217]);
            }
        }

        static private void MyMsgBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle frmTitleL = new Rectangle(0, 0, (MyMsgBox.Width / 2), 22);
            Rectangle frmTitleR = new Rectangle((MyMsgBox.Width / 2), 0, (MyMsgBox.Width / 2), 22);
            Rectangle frmMessageBox = new Rectangle(0, 0, (MyMsgBox.Width - 1), (MyMsgBox.Height - 1));
            LinearGradientBrush frmLGBL = new LinearGradientBrush(frmTitleL, Color.FromArgb(87, 148, 160), Color.FromArgb(209, 230, 243), LinearGradientMode.Horizontal);
            LinearGradientBrush frmLGBR = new LinearGradientBrush(frmTitleR, Color.FromArgb(209, 230, 243), Color.FromArgb(87, 148, 160), LinearGradientMode.Horizontal);
            Pen frmPen = new Pen(Color.FromArgb(63, 119, 143), 1);
            g.FillRectangle(frmLGBL, frmTitleL);
            g.FillRectangle(frmLGBR, frmTitleR);
            g.DrawRectangle(frmPen, frmMessageBox);

        }
    }
}
