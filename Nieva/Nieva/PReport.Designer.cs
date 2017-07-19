namespace Nieva
{
    partial class PReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PReport));
            this.tlpPReport = new System.Windows.Forms.TableLayoutPanel();
            this.flpHPReport = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHPR = new System.Windows.Forms.Label();
            this.pnlPRButtons = new System.Windows.Forms.Panel();
            this.lblPBInformation = new System.Windows.Forms.Label();
            this.btnPRPClient = new System.Windows.Forms.Button();
            this.btnPRSummary = new System.Windows.Forms.Button();
            this.tlpPReport.SuspendLayout();
            this.flpHPReport.SuspendLayout();
            this.pnlPRButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpPReport
            // 
            this.tlpPReport.ColumnCount = 1;
            this.tlpPReport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPReport.Controls.Add(this.flpHPReport, 0, 0);
            this.tlpPReport.Controls.Add(this.pnlPRButtons, 0, 1);
            this.tlpPReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPReport.Location = new System.Drawing.Point(0, 0);
            this.tlpPReport.Name = "tlpPReport";
            this.tlpPReport.RowCount = 2;
            this.tlpPReport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpPReport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPReport.Size = new System.Drawing.Size(364, 231);
            this.tlpPReport.TabIndex = 2;
            // 
            // flpHPReport
            // 
            this.flpHPReport.Controls.Add(this.lblHPR);
            this.flpHPReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHPReport.Location = new System.Drawing.Point(0, 0);
            this.flpHPReport.Margin = new System.Windows.Forms.Padding(0);
            this.flpHPReport.Name = "flpHPReport";
            this.flpHPReport.Size = new System.Drawing.Size(364, 45);
            this.flpHPReport.TabIndex = 7;
            // 
            // lblHPR
            // 
            this.lblHPR.AutoSize = true;
            this.lblHPR.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHPR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHPR.Location = new System.Drawing.Point(23, 13);
            this.lblHPR.Margin = new System.Windows.Forms.Padding(23, 13, 0, 0);
            this.lblHPR.Name = "lblHPR";
            this.lblHPR.Size = new System.Drawing.Size(137, 19);
            this.lblHPR.TabIndex = 4;
            this.lblHPR.Text = "Payments Report";
            // 
            // pnlPRButtons
            // 
            this.pnlPRButtons.Controls.Add(this.lblPBInformation);
            this.pnlPRButtons.Controls.Add(this.btnPRPClient);
            this.pnlPRButtons.Controls.Add(this.btnPRSummary);
            this.pnlPRButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPRButtons.Location = new System.Drawing.Point(0, 45);
            this.pnlPRButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPRButtons.Name = "pnlPRButtons";
            this.pnlPRButtons.Size = new System.Drawing.Size(364, 186);
            this.pnlPRButtons.TabIndex = 8;
            // 
            // lblPBInformation
            // 
            this.lblPBInformation.AutoSize = true;
            this.lblPBInformation.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPBInformation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPBInformation.Location = new System.Drawing.Point(29, 90);
            this.lblPBInformation.Margin = new System.Windows.Forms.Padding(20, 15, 20, 0);
            this.lblPBInformation.Name = "lblPBInformation";
            this.lblPBInformation.Size = new System.Drawing.Size(305, 51);
            this.lblPBInformation.TabIndex = 77;
            this.lblPBInformation.Text = "      Summary is based on selected frequency\r\noption, if none it displays all tim" +
    "e summary by \r\ndefault.";
            // 
            // btnPRPClient
            // 
            this.btnPRPClient.BackColor = System.Drawing.Color.LimeGreen;
            this.btnPRPClient.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPRPClient.FlatAppearance.BorderSize = 0;
            this.btnPRPClient.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnPRPClient.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnPRPClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPRPClient.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPRPClient.ForeColor = System.Drawing.Color.White;
            this.btnPRPClient.Location = new System.Drawing.Point(52, 30);
            this.btnPRPClient.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnPRPClient.Name = "btnPRPClient";
            this.btnPRPClient.Size = new System.Drawing.Size(120, 35);
            this.btnPRPClient.TabIndex = 7;
            this.btnPRPClient.TabStop = false;
            this.btnPRPClient.Text = "P&er Client";
            this.btnPRPClient.UseVisualStyleBackColor = false;
            this.btnPRPClient.Click += new System.EventHandler(this.btnPRPClient_Click);
            // 
            // btnPRSummary
            // 
            this.btnPRSummary.BackColor = System.Drawing.Color.LimeGreen;
            this.btnPRSummary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPRSummary.FlatAppearance.BorderSize = 0;
            this.btnPRSummary.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnPRSummary.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnPRSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPRSummary.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPRSummary.ForeColor = System.Drawing.Color.White;
            this.btnPRSummary.Location = new System.Drawing.Point(192, 30);
            this.btnPRSummary.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnPRSummary.Name = "btnPRSummary";
            this.btnPRSummary.Size = new System.Drawing.Size(120, 35);
            this.btnPRSummary.TabIndex = 8;
            this.btnPRSummary.TabStop = false;
            this.btnPRSummary.Text = "&Summary";
            this.btnPRSummary.UseVisualStyleBackColor = false;
            this.btnPRSummary.Click += new System.EventHandler(this.btnPRSummary_Click);
            // 
            // PReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(364, 231);
            this.Controls.Add(this.tlpPReport);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(380, 270);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 270);
            this.Name = "PReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payments Report";
            this.tlpPReport.ResumeLayout(false);
            this.flpHPReport.ResumeLayout(false);
            this.flpHPReport.PerformLayout();
            this.pnlPRButtons.ResumeLayout(false);
            this.pnlPRButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpPReport;
        private System.Windows.Forms.FlowLayoutPanel flpHPReport;
        private System.Windows.Forms.Label lblHPR;
        private System.Windows.Forms.Panel pnlPRButtons;
        private System.Windows.Forms.Label lblPBInformation;
        private System.Windows.Forms.Button btnPRPClient;
        private System.Windows.Forms.Button btnPRSummary;
    }
}