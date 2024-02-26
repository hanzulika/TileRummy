using System;

namespace Proiectul_Remi_Ioan_Hanzu
{
    partial class StartScreen
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtbEnterName = new System.Windows.Forms.TextBox();
            this.btnConnectServer = new System.Windows.Forms.Button();
            this.txtbAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(140, 140);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(140, 189);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtbEnterName
            // 
            this.txtbEnterName.Cursor = System.Windows.Forms.Cursors.No;
            this.txtbEnterName.Location = new System.Drawing.Point(117, 90);
            this.txtbEnterName.Name = "txtbEnterName";
            this.txtbEnterName.Size = new System.Drawing.Size(124, 22);
            this.txtbEnterName.TabIndex = 2;
            this.txtbEnterName.Text = "Name";
            this.txtbEnterName.Click += new System.EventHandler(this.txtbEnterName_Click);
            this.txtbEnterName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbEnterName_KeyPress);
            // 
            // btnConnectServer
            // 
            this.btnConnectServer.Location = new System.Drawing.Point(12, 140);
            this.btnConnectServer.Name = "btnConnectServer";
            this.btnConnectServer.Size = new System.Drawing.Size(75, 48);
            this.btnConnectServer.TabIndex = 3;
            this.btnConnectServer.Text = "Connect";
            this.btnConnectServer.UseVisualStyleBackColor = true;
            this.btnConnectServer.Click += new System.EventHandler(this.btnConnectServer_Click);
            // 
            // txtbAddress
            // 
            this.txtbAddress.Location = new System.Drawing.Point(12, 209);
            this.txtbAddress.Name = "txtbAddress";
            this.txtbAddress.Size = new System.Drawing.Size(100, 22);
            this.txtbAddress.TabIndex = 4;
            this.txtbAddress.Text = "127.0.0.1";
            // 
            // StartScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Proiectul_Remi_Ioan_Hanzu.Properties.Resources.background_remi;
            this.ClientSize = new System.Drawing.Size(356, 392);
            this.Controls.Add(this.txtbAddress);
            this.Controls.Add(this.btnConnectServer);
            this.Controls.Add(this.txtbEnterName);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Name = "StartScreen";
            this.Text = "Rummy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtbEnterName;
        private System.Windows.Forms.Button btnConnectServer;
        private System.Windows.Forms.TextBox txtbAddress;
    }
}

