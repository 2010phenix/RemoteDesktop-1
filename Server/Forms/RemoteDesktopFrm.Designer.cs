namespace Server
{
    partial class RemoteDesktopFrm
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
            this.pbScreen = new System.Windows.Forms.PictureBox();
            this.msCommands = new System.Windows.Forms.MenuStrip();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxMonitors = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripTextBoxFPS = new System.Windows.Forms.ToolStripTextBox();
            this.tmrFPS = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbScreen)).BeginInit();
            this.msCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbScreen
            // 
            this.pbScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScreen.Location = new System.Drawing.Point(0, 27);
            this.pbScreen.Name = "pbScreen";
            this.pbScreen.Size = new System.Drawing.Size(683, 381);
            this.pbScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbScreen.TabIndex = 0;
            this.pbScreen.TabStop = false;
            this.pbScreen.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbScreen_MouseClick);
            // 
            // msCommands
            // 
            this.msCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripComboBoxMonitors,
            this.toolStripTextBoxFPS});
            this.msCommands.Location = new System.Drawing.Point(0, 0);
            this.msCommands.Name = "msCommands";
            this.msCommands.Size = new System.Drawing.Size(683, 27);
            this.msCommands.TabIndex = 3;
            this.msCommands.Text = "menuStrip1";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(43, 23);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(43, 23);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripComboBoxMonitors
            // 
            this.toolStripComboBoxMonitors.Name = "toolStripComboBoxMonitors";
            this.toolStripComboBoxMonitors.Size = new System.Drawing.Size(121, 23);
            this.toolStripComboBoxMonitors.Text = "Select Monitor";
            // 
            // toolStripTextBoxFPS
            // 
            this.toolStripTextBoxFPS.Name = "toolStripTextBoxFPS";
            this.toolStripTextBoxFPS.ReadOnly = true;
            this.toolStripTextBoxFPS.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBoxFPS.Text = "FPS: ";
            // 
            // tmrFPS
            // 
            this.tmrFPS.Interval = 1000;
            this.tmrFPS.Tick += new System.EventHandler(this.tmrFPS_Tick);
            // 
            // RemoteDesktopFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 408);
            this.Controls.Add(this.pbScreen);
            this.Controls.Add(this.msCommands);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.msCommands;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RemoteDesktopFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Desktop - ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteDesktopFrm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbScreen)).EndInit();
            this.msCommands.ResumeLayout(false);
            this.msCommands.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbScreen;
        private System.Windows.Forms.MenuStrip msCommands;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxMonitors;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxFPS;
        private System.Windows.Forms.Timer tmrFPS;
    }
}