namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    partial class FormClient
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.grpxServer = new System.Windows.Forms.GroupBox();
            this.btnApplySetting = new System.Windows.Forms.Button();
            this.btnTestServer = new System.Windows.Forms.Button();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnCheckDiff = new System.Windows.Forms.Button();
            this.txtServerPassword = new System.Windows.Forms.TextBox();
            this.lblServerPassword = new System.Windows.Forms.Label();
            this.txtServerUsername = new System.Windows.Forms.TextBox();
            this.lblServerUsername = new System.Windows.Forms.Label();
            this.txtServerURL = new System.Windows.Forms.TextBox();
            this.lblServerURL = new System.Windows.Forms.Label();
            this.grpxLocal = new System.Windows.Forms.GroupBox();
            this.txtLocalFolder = new System.Windows.Forms.TextBox();
            this.lblLocalFolder = new System.Windows.Forms.Label();
            this.grpxResult = new System.Windows.Forms.GroupBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.grpxProxy = new System.Windows.Forms.GroupBox();
            this.chkProxyUseIt = new System.Windows.Forms.CheckBox();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.lblProxyPassword = new System.Windows.Forms.Label();
            this.txtProxyUsername = new System.Windows.Forms.TextBox();
            this.lblProxyUsername = new System.Windows.Forms.Label();
            this.txtProxyURL = new System.Windows.Forms.TextBox();
            this.lblProxyURL = new System.Windows.Forms.Label();
            this.grpxServer.SuspendLayout();
            this.grpxLocal.SuspendLayout();
            this.grpxResult.SuspendLayout();
            this.grpxProxy.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpxServer
            // 
            this.grpxServer.Controls.Add(this.btnApplySetting);
            this.grpxServer.Controls.Add(this.btnTestServer);
            this.grpxServer.Controls.Add(this.btnSync);
            this.grpxServer.Controls.Add(this.btnCheckDiff);
            this.grpxServer.Controls.Add(this.txtServerPassword);
            this.grpxServer.Controls.Add(this.lblServerPassword);
            this.grpxServer.Controls.Add(this.txtServerUsername);
            this.grpxServer.Controls.Add(this.lblServerUsername);
            this.grpxServer.Controls.Add(this.txtServerURL);
            this.grpxServer.Controls.Add(this.lblServerURL);
            this.grpxServer.Location = new System.Drawing.Point(3, 3);
            this.grpxServer.Name = "grpxServer";
            this.grpxServer.Size = new System.Drawing.Size(487, 100);
            this.grpxServer.TabIndex = 2;
            this.grpxServer.TabStop = false;
            this.grpxServer.Text = "Cloud Server";
            // 
            // btnApplySetting
            // 
            this.btnApplySetting.Location = new System.Drawing.Point(326, 71);
            this.btnApplySetting.Name = "btnApplySetting";
            this.btnApplySetting.Size = new System.Drawing.Size(75, 23);
            this.btnApplySetting.TabIndex = 11;
            this.btnApplySetting.Text = "Apply";
            this.btnApplySetting.UseVisualStyleBackColor = true;
            this.btnApplySetting.Click += new System.EventHandler(this.btnApplySetting_Click);
            // 
            // btnTestServer
            // 
            this.btnTestServer.Location = new System.Drawing.Point(326, 44);
            this.btnTestServer.Name = "btnTestServer";
            this.btnTestServer.Size = new System.Drawing.Size(75, 23);
            this.btnTestServer.TabIndex = 10;
            this.btnTestServer.Text = "TestServer";
            this.btnTestServer.UseVisualStyleBackColor = true;
            this.btnTestServer.Click += new System.EventHandler(this.btnTestServer_Click);
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(407, 71);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(75, 23);
            this.btnSync.TabIndex = 9;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnCheckDiff
            // 
            this.btnCheckDiff.Location = new System.Drawing.Point(407, 44);
            this.btnCheckDiff.Name = "btnCheckDiff";
            this.btnCheckDiff.Size = new System.Drawing.Size(75, 23);
            this.btnCheckDiff.TabIndex = 8;
            this.btnCheckDiff.Text = "CheckDiff";
            this.btnCheckDiff.UseVisualStyleBackColor = true;
            this.btnCheckDiff.Click += new System.EventHandler(this.btnCheckDiff_Click);
            // 
            // txtServerPassword
            // 
            this.txtServerPassword.Location = new System.Drawing.Point(91, 72);
            this.txtServerPassword.Name = "txtServerPassword";
            this.txtServerPassword.Size = new System.Drawing.Size(229, 21);
            this.txtServerPassword.TabIndex = 7;
            this.txtServerPassword.Text = "mao778853";
            // 
            // lblServerPassword
            // 
            this.lblServerPassword.AutoSize = true;
            this.lblServerPassword.Location = new System.Drawing.Point(35, 76);
            this.lblServerPassword.Name = "lblServerPassword";
            this.lblServerPassword.Size = new System.Drawing.Size(53, 12);
            this.lblServerPassword.TabIndex = 6;
            this.lblServerPassword.Text = "Password";
            // 
            // txtServerUsername
            // 
            this.txtServerUsername.Location = new System.Drawing.Point(91, 45);
            this.txtServerUsername.Name = "txtServerUsername";
            this.txtServerUsername.Size = new System.Drawing.Size(229, 21);
            this.txtServerUsername.TabIndex = 5;
            this.txtServerUsername.Text = "zhenhua.mao";
            // 
            // lblServerUsername
            // 
            this.lblServerUsername.AutoSize = true;
            this.lblServerUsername.Location = new System.Drawing.Point(35, 49);
            this.lblServerUsername.Name = "lblServerUsername";
            this.lblServerUsername.Size = new System.Drawing.Size(53, 12);
            this.lblServerUsername.TabIndex = 4;
            this.lblServerUsername.Text = "Username";
            // 
            // txtServerURL
            // 
            this.txtServerURL.Location = new System.Drawing.Point(91, 18);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(391, 21);
            this.txtServerURL.TabIndex = 3;
            this.txtServerURL.Text = "http://192.168.1.115:49152";
            // 
            // lblServerURL
            // 
            this.lblServerURL.AutoSize = true;
            this.lblServerURL.Location = new System.Drawing.Point(23, 22);
            this.lblServerURL.Name = "lblServerURL";
            this.lblServerURL.Size = new System.Drawing.Size(65, 12);
            this.lblServerURL.TabIndex = 2;
            this.lblServerURL.Text = "Server URL";
            // 
            // grpxLocal
            // 
            this.grpxLocal.Controls.Add(this.txtLocalFolder);
            this.grpxLocal.Controls.Add(this.lblLocalFolder);
            this.grpxLocal.Location = new System.Drawing.Point(2, 234);
            this.grpxLocal.Name = "grpxLocal";
            this.grpxLocal.Size = new System.Drawing.Size(487, 74);
            this.grpxLocal.TabIndex = 3;
            this.grpxLocal.TabStop = false;
            this.grpxLocal.Text = "Local";
            // 
            // txtLocalFolder
            // 
            this.txtLocalFolder.Location = new System.Drawing.Point(91, 18);
            this.txtLocalFolder.Name = "txtLocalFolder";
            this.txtLocalFolder.Size = new System.Drawing.Size(393, 21);
            this.txtLocalFolder.TabIndex = 3;
            this.txtLocalFolder.Text = "E:\\workGit\\SyncFolder\\项目170";
            // 
            // lblLocalFolder
            // 
            this.lblLocalFolder.AutoSize = true;
            this.lblLocalFolder.Location = new System.Drawing.Point(11, 22);
            this.lblLocalFolder.Name = "lblLocalFolder";
            this.lblLocalFolder.Size = new System.Drawing.Size(77, 12);
            this.lblLocalFolder.TabIndex = 2;
            this.lblLocalFolder.Text = "Local Folder";
            // 
            // grpxResult
            // 
            this.grpxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpxResult.Controls.Add(this.txtResult);
            this.grpxResult.Location = new System.Drawing.Point(497, 3);
            this.grpxResult.Name = "grpxResult";
            this.grpxResult.Size = new System.Drawing.Size(326, 305);
            this.grpxResult.TabIndex = 4;
            this.grpxResult.TabStop = false;
            this.grpxResult.Text = "Result";
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(3, 17);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(320, 285);
            this.txtResult.TabIndex = 4;
            // 
            // grpxProxy
            // 
            this.grpxProxy.Controls.Add(this.chkProxyUseIt);
            this.grpxProxy.Controls.Add(this.txtProxyPassword);
            this.grpxProxy.Controls.Add(this.lblProxyPassword);
            this.grpxProxy.Controls.Add(this.txtProxyUsername);
            this.grpxProxy.Controls.Add(this.lblProxyUsername);
            this.grpxProxy.Controls.Add(this.txtProxyURL);
            this.grpxProxy.Controls.Add(this.lblProxyURL);
            this.grpxProxy.Location = new System.Drawing.Point(2, 109);
            this.grpxProxy.Name = "grpxProxy";
            this.grpxProxy.Size = new System.Drawing.Size(487, 119);
            this.grpxProxy.TabIndex = 5;
            this.grpxProxy.TabStop = false;
            this.grpxProxy.Text = "Proxy";
            // 
            // chkProxyUseIt
            // 
            this.chkProxyUseIt.AutoSize = true;
            this.chkProxyUseIt.Location = new System.Drawing.Point(91, 13);
            this.chkProxyUseIt.Name = "chkProxyUseIt";
            this.chkProxyUseIt.Size = new System.Drawing.Size(78, 16);
            this.chkProxyUseIt.TabIndex = 8;
            this.chkProxyUseIt.Text = "Use Proxy";
            this.chkProxyUseIt.UseVisualStyleBackColor = true;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Location = new System.Drawing.Point(91, 89);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(229, 21);
            this.txtProxyPassword.TabIndex = 7;
            this.txtProxyPassword.Text = "zhenhua5555";
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.Location = new System.Drawing.Point(35, 93);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(53, 12);
            this.lblProxyPassword.TabIndex = 6;
            this.lblProxyPassword.Text = "Password";
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.Location = new System.Drawing.Point(91, 62);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(229, 21);
            this.txtProxyUsername.TabIndex = 5;
            this.txtProxyUsername.Text = "zhenhua.mao@ranplan.net";
            // 
            // lblProxyUsername
            // 
            this.lblProxyUsername.AutoSize = true;
            this.lblProxyUsername.Location = new System.Drawing.Point(35, 66);
            this.lblProxyUsername.Name = "lblProxyUsername";
            this.lblProxyUsername.Size = new System.Drawing.Size(53, 12);
            this.lblProxyUsername.TabIndex = 4;
            this.lblProxyUsername.Text = "Username";
            // 
            // txtProxyURL
            // 
            this.txtProxyURL.Location = new System.Drawing.Point(91, 35);
            this.txtProxyURL.Name = "txtProxyURL";
            this.txtProxyURL.Size = new System.Drawing.Size(391, 21);
            this.txtProxyURL.TabIndex = 3;
            this.txtProxyURL.Text = "http://localhost:49152/";
            // 
            // lblProxyURL
            // 
            this.lblProxyURL.AutoSize = true;
            this.lblProxyURL.Location = new System.Drawing.Point(23, 39);
            this.lblProxyURL.Name = "lblProxyURL";
            this.lblProxyURL.Size = new System.Drawing.Size(59, 12);
            this.lblProxyURL.TabIndex = 2;
            this.lblProxyURL.Text = "Proxy URL";
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 316);
            this.Controls.Add(this.grpxProxy);
            this.Controls.Add(this.grpxResult);
            this.Controls.Add(this.grpxLocal);
            this.Controls.Add(this.grpxServer);
            this.Name = "FormClient";
            this.Text = "Folder Sync";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.grpxServer.ResumeLayout(false);
            this.grpxServer.PerformLayout();
            this.grpxLocal.ResumeLayout(false);
            this.grpxLocal.PerformLayout();
            this.grpxResult.ResumeLayout(false);
            this.grpxResult.PerformLayout();
            this.grpxProxy.ResumeLayout(false);
            this.grpxProxy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpxServer;
        private System.Windows.Forms.TextBox txtServerPassword;
        private System.Windows.Forms.Label lblServerPassword;
        private System.Windows.Forms.TextBox txtServerUsername;
        private System.Windows.Forms.Label lblServerUsername;
        private System.Windows.Forms.TextBox txtServerURL;
        private System.Windows.Forms.Label lblServerURL;
        private System.Windows.Forms.GroupBox grpxLocal;
        private System.Windows.Forms.TextBox txtLocalFolder;
        private System.Windows.Forms.Label lblLocalFolder;
        private System.Windows.Forms.GroupBox grpxResult;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnCheckDiff;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnTestServer;
        private System.Windows.Forms.GroupBox grpxProxy;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.Label lblProxyPassword;
        private System.Windows.Forms.TextBox txtProxyUsername;
        private System.Windows.Forms.Label lblProxyUsername;
        private System.Windows.Forms.TextBox txtProxyURL;
        private System.Windows.Forms.Label lblProxyURL;
        private System.Windows.Forms.CheckBox chkProxyUseIt;
        private System.Windows.Forms.Button btnApplySetting;
    }
}

