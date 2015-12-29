namespace Client
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
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.gbConnectionInfo = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gbConnectionInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(89, 21);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(292, 22);
            this.txtIPAddress.TabIndex = 0;
            this.txtIPAddress.Text = "127.0.0.1";
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.AutoSize = true;
            this.lblIPAddress.Location = new System.Drawing.Point(6, 24);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(77, 16);
            this.lblIPAddress.TabIndex = 1;
            this.lblIPAddress.Text = "IP Address:";
            // 
            // gbConnectionInfo
            // 
            this.gbConnectionInfo.Controls.Add(this.lblIPAddress);
            this.gbConnectionInfo.Controls.Add(this.txtIPAddress);
            this.gbConnectionInfo.Location = new System.Drawing.Point(12, 12);
            this.gbConnectionInfo.Name = "gbConnectionInfo";
            this.gbConnectionInfo.Size = new System.Drawing.Size(387, 52);
            this.gbConnectionInfo.TabIndex = 2;
            this.gbConnectionInfo.TabStop = false;
            this.gbConnectionInfo.Text = "Connection Info";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 70);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(387, 34);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 116);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.gbConnectionInfo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Desktop - Client";
            this.gbConnectionInfo.ResumeLayout(false);
            this.gbConnectionInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.GroupBox gbConnectionInfo;
        private System.Windows.Forms.Button btnConnect;
    }
}

