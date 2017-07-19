namespace Nieva
{
    partial class SConn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SConn));
            this.flpSPlacing = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSConn = new System.Windows.Forms.Label();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.flpSConn = new System.Windows.Forms.FlowLayoutPanel();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSHeader = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.ptbLogo = new System.Windows.Forms.PictureBox();
            this.flpSPlacing.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.flpSConn.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // flpSPlacing
            // 
            this.flpSPlacing.BackColor = System.Drawing.Color.Gainsboro;
            this.flpSPlacing.Controls.Add(this.lblSConn);
            this.flpSPlacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpSPlacing.Location = new System.Drawing.Point(0, 80);
            this.flpSPlacing.Margin = new System.Windows.Forms.Padding(0);
            this.flpSPlacing.Name = "flpSPlacing";
            this.flpSPlacing.Size = new System.Drawing.Size(364, 45);
            this.flpSPlacing.TabIndex = 12;
            // 
            // lblSConn
            // 
            this.lblSConn.AutoSize = true;
            this.lblSConn.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSConn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSConn.Location = new System.Drawing.Point(20, 10);
            this.lblSConn.Margin = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.lblSConn.Name = "lblSConn";
            this.lblSConn.Size = new System.Drawing.Size(150, 19);
            this.lblSConn.TabIndex = 5;
            this.lblSConn.Text = "Server Connection";
            // 
            // flpButtons
            // 
            this.flpButtons.BackColor = System.Drawing.Color.Gainsboro;
            this.flpButtons.Controls.Add(this.btnOk);
            this.flpButtons.Controls.Add(this.btnCancel);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtons.Location = new System.Drawing.Point(0, 301);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(364, 70);
            this.flpButtons.TabIndex = 11;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.LimeGreen;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(48, 0);
            this.btnOk.Margin = new System.Windows.Forms.Padding(48, 0, 30, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 34);
            this.btnOk.TabIndex = 8;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LimeGreen;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(198, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 34);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // flpSConn
            // 
            this.flpSConn.BackColor = System.Drawing.Color.Gainsboro;
            this.flpSConn.Controls.Add(this.lblServer);
            this.flpSConn.Controls.Add(this.txtServer);
            this.flpSConn.Controls.Add(this.lblPort);
            this.flpSConn.Controls.Add(this.txtPort);
            this.flpSConn.Controls.Add(this.lblUsername);
            this.flpSConn.Controls.Add(this.txtUsername);
            this.flpSConn.Controls.Add(this.lblPassword);
            this.flpSConn.Controls.Add(this.txtPassword);
            this.flpSConn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSConn.Location = new System.Drawing.Point(0, 80);
            this.flpSConn.Margin = new System.Windows.Forms.Padding(0);
            this.flpSConn.Name = "flpSConn";
            this.flpSConn.Size = new System.Drawing.Size(364, 291);
            this.flpSConn.TabIndex = 10;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblServer.Location = new System.Drawing.Point(63, 66);
            this.lblServer.Margin = new System.Windows.Forms.Padding(63, 66, 0, 0);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(54, 17);
            this.lblServer.TabIndex = 59;
            this.lblServer.Text = "Server :";
            // 
            // txtServer
            // 
            this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServer.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServer.Location = new System.Drawing.Point(120, 63);
            this.txtServer.Margin = new System.Windows.Forms.Padding(3, 63, 10, 10);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(200, 23);
            this.txtServer.TabIndex = 0;
            this.txtServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPort.Location = new System.Drawing.Point(75, 102);
            this.lblPort.Margin = new System.Windows.Forms.Padding(75, 6, 0, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(42, 17);
            this.lblPort.TabIndex = 65;
            this.lblPort.Text = "Port :";
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(120, 99);
            this.txtPort.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(200, 23);
            this.txtPort.TabIndex = 1;
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblUsername.Location = new System.Drawing.Point(38, 138);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(38, 6, 0, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(79, 17);
            this.lblUsername.TabIndex = 56;
            this.lblUsername.Text = "Username :";
            // 
            // txtUsername
            // 
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(120, 135);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPassword.Location = new System.Drawing.Point(40, 174);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(40, 6, 0, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(77, 17);
            this.lblPassword.TabIndex = 57;
            this.lblPassword.Text = "Password :";
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(120, 171);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblSHeader);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Controls.Add(this.ptbLogo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(364, 80);
            this.pnlHeader.TabIndex = 9;
            // 
            // lblSHeader
            // 
            this.lblSHeader.AutoSize = true;
            this.lblSHeader.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSHeader.Location = new System.Drawing.Point(94, 48);
            this.lblSHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblSHeader.Name = "lblSHeader";
            this.lblSHeader.Size = new System.Drawing.Size(248, 17);
            this.lblSHeader.TabIndex = 2;
            this.lblSHeader.Text = "Accounting Office and Allied Services";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblHeader.Location = new System.Drawing.Point(90, 10);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(108, 38);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Nieva";
            // 
            // ptbLogo
            // 
            this.ptbLogo.Image = ((System.Drawing.Image)(resources.GetObject("ptbLogo.Image")));
            this.ptbLogo.Location = new System.Drawing.Point(20, 10);
            this.ptbLogo.Margin = new System.Windows.Forms.Padding(0);
            this.ptbLogo.Name = "ptbLogo";
            this.ptbLogo.Size = new System.Drawing.Size(60, 57);
            this.ptbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ptbLogo.TabIndex = 0;
            this.ptbLogo.TabStop = false;
            // 
            // SConn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(364, 371);
            this.Controls.Add(this.flpSPlacing);
            this.Controls.Add(this.flpButtons);
            this.Controls.Add(this.flpSConn);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(380, 410);
            this.MinimumSize = new System.Drawing.Size(380, 410);
            this.Name = "SConn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Connection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SConn_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SConn_KeyDown);
            this.flpSPlacing.ResumeLayout(false);
            this.flpSPlacing.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            this.flpSConn.ResumeLayout(false);
            this.flpSConn.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpSPlacing;
        private System.Windows.Forms.Label lblSConn;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.FlowLayoutPanel flpSConn;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblSHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox ptbLogo;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
    }
}