namespace Server
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
            this.lvClients = new System.Windows.Forms.ListView();
            this.chClientID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsFunctions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.remoteDesktopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbConnectedClients = new System.Windows.Forms.GroupBox();
            this.chRemoteEndPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsFunctions.SuspendLayout();
            this.gbConnectedClients.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvClients
            // 
            this.lvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chClientID,
            this.chUser,
            this.chRemoteEndPoint});
            this.lvClients.ContextMenuStrip = this.cmsFunctions;
            this.lvClients.FullRowSelect = true;
            this.lvClients.GridLines = true;
            this.lvClients.Location = new System.Drawing.Point(6, 21);
            this.lvClients.MultiSelect = false;
            this.lvClients.Name = "lvClients";
            this.lvClients.Size = new System.Drawing.Size(343, 270);
            this.lvClients.TabIndex = 0;
            this.lvClients.UseCompatibleStateImageBehavior = false;
            this.lvClients.View = System.Windows.Forms.View.Details;
            // 
            // chClientID
            // 
            this.chClientID.Text = "Client ID";
            this.chClientID.Width = 93;
            // 
            // chUser
            // 
            this.chUser.Text = "User";
            this.chUser.Width = 101;
            // 
            // cmsFunctions
            // 
            this.cmsFunctions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteDesktopToolStripMenuItem});
            this.cmsFunctions.Name = "cmsFunctions";
            this.cmsFunctions.Size = new System.Drawing.Size(162, 26);
            // 
            // remoteDesktopToolStripMenuItem
            // 
            this.remoteDesktopToolStripMenuItem.Name = "remoteDesktopToolStripMenuItem";
            this.remoteDesktopToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.remoteDesktopToolStripMenuItem.Text = "Remote Desktop";
            this.remoteDesktopToolStripMenuItem.Click += new System.EventHandler(this.remoteDesktopToolStripMenuItem_Click);
            // 
            // gbConnectedClients
            // 
            this.gbConnectedClients.Controls.Add(this.lvClients);
            this.gbConnectedClients.Location = new System.Drawing.Point(12, 12);
            this.gbConnectedClients.Name = "gbConnectedClients";
            this.gbConnectedClients.Size = new System.Drawing.Size(355, 297);
            this.gbConnectedClients.TabIndex = 1;
            this.gbConnectedClients.TabStop = false;
            this.gbConnectedClients.Text = "Connected Clients:";
            // 
            // chRemoteEndPoint
            // 
            this.chRemoteEndPoint.Text = "Remote End Point";
            this.chRemoteEndPoint.Width = 145;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 321);
            this.Controls.Add(this.gbConnectedClients);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remote Desktop - Server";
            this.cmsFunctions.ResumeLayout(false);
            this.gbConnectedClients.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvClients;
        private System.Windows.Forms.ColumnHeader chClientID;
        private System.Windows.Forms.ColumnHeader chUser;
        private System.Windows.Forms.GroupBox gbConnectedClients;
        private System.Windows.Forms.ContextMenuStrip cmsFunctions;
        private System.Windows.Forms.ToolStripMenuItem remoteDesktopToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader chRemoteEndPoint;
    }
}

