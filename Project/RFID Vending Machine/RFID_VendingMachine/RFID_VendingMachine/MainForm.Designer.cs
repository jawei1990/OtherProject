namespace RFID_VendingMachine
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPortRFID = new System.IO.Ports.SerialPort(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCola = new System.Windows.Forms.Button();
            this.btnPotato = new System.Windows.Forms.Button();
            this.btnBlackTea = new System.Windows.Forms.Button();
            this.lblCardInfo = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(362, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsSToolStripMenuItem,
            this.toolStripMenuItem1,
            this.quitToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // settingsSToolStripMenuItem
            // 
            this.settingsSToolStripMenuItem.Name = "settingsSToolStripMenuItem";
            this.settingsSToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.settingsSToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.settingsSToolStripMenuItem.Text = "&Settings";
            this.settingsSToolStripMenuItem.Click += new System.EventHandler(this.settingsSToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "cola.png");
            this.imageList1.Images.SetKeyName(1, "potato.png");
            this.imageList1.Images.SetKeyName(2, "blacktee.png");
            // 
            // btnCola
            // 
            this.btnCola.Enabled = false;
            this.btnCola.Image = ((System.Drawing.Image)(resources.GetObject("btnCola.Image")));
            this.btnCola.Location = new System.Drawing.Point(24, 83);
            this.btnCola.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnCola.Name = "btnCola";
            this.btnCola.Size = new System.Drawing.Size(134, 179);
            this.btnCola.TabIndex = 2;
            this.btnCola.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCola.UseVisualStyleBackColor = true;
            this.btnCola.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // btnPotato
            // 
            this.btnPotato.Enabled = false;
            this.btnPotato.Image = ((System.Drawing.Image)(resources.GetObject("btnPotato.Image")));
            this.btnPotato.Location = new System.Drawing.Point(201, 83);
            this.btnPotato.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnPotato.Name = "btnPotato";
            this.btnPotato.Size = new System.Drawing.Size(134, 179);
            this.btnPotato.TabIndex = 3;
            this.btnPotato.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPotato.UseVisualStyleBackColor = true;
            this.btnPotato.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // btnBlackTea
            // 
            this.btnBlackTea.Enabled = false;
            this.btnBlackTea.ImageIndex = 2;
            this.btnBlackTea.ImageList = this.imageList1;
            this.btnBlackTea.Location = new System.Drawing.Point(378, 83);
            this.btnBlackTea.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.btnBlackTea.Name = "btnBlackTea";
            this.btnBlackTea.Size = new System.Drawing.Size(134, 179);
            this.btnBlackTea.TabIndex = 4;
            this.btnBlackTea.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBlackTea.UseVisualStyleBackColor = true;
            this.btnBlackTea.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // lblCardInfo
            // 
            this.lblCardInfo.AutoSize = true;
            this.lblCardInfo.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCardInfo.Location = new System.Drawing.Point(13, 38);
            this.lblCardInfo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblCardInfo.Name = "lblCardInfo";
            this.lblCardInfo.Size = new System.Drawing.Size(100, 16);
            this.lblCardInfo.TabIndex = 5;
            this.lblCardInfo.Text = "請感應卡片...";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 283);
            this.Controls.Add(this.lblCardInfo);
            this.Controls.Add(this.btnBlackTea);
            this.Controls.Add(this.btnPotato);
            this.Controls.Add(this.btnCola);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsSToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.IO.Ports.SerialPort serialPortRFID;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnCola;
        private System.Windows.Forms.Button btnPotato;
        private System.Windows.Forms.Button btnBlackTea;
        private System.Windows.Forms.Label lblCardInfo;
        private System.IO.Ports.SerialPort serialPort1;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}