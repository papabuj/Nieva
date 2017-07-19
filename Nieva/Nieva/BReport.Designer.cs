namespace Nieva
{
    partial class BReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BReport));
            this.tlpBReport = new System.Windows.Forms.TableLayoutPanel();
            this.flpHBReport = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHBR = new System.Windows.Forms.Label();
            this.pnlBRButtons = new System.Windows.Forms.Panel();
            this.lblPBInformation = new System.Windows.Forms.Label();
            this.btnBRPClient = new System.Windows.Forms.Button();
            this.btnBRSummary = new System.Windows.Forms.Button();
            this.tlpBReport.SuspendLayout();
            this.flpHBReport.SuspendLayout();
            this.pnlBRButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpBReport
            // 
            this.tlpBReport.ColumnCount = 1;
            this.tlpBReport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBReport.Controls.Add(this.flpHBReport, 0, 0);
            this.tlpBReport.Controls.Add(this.pnlBRButtons, 0, 1);
            this.tlpBReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBReport.Location = new System.Drawing.Point(0, 0);
            this.tlpBReport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tlpBReport.Name = "tlpBReport";
            this.tlpBReport.RowCount = 2;
            this.tlpBReport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpBReport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBReport.Size = new System.Drawing.Size(364, 231);
            this.tlpBReport.TabIndex = 1;
            // 
            // flpHBReport
            // 
            this.flpHBReport.Controls.Add(this.lblHBR);
            this.flpHBReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHBReport.Location = new System.Drawing.Point(0, 0);
            this.flpHBReport.Margin = new System.Windows.Forms.Padding(0);
            this.flpHBReport.Name = "flpHBReport";
            this.flpHBReport.Size = new System.Drawing.Size(364, 45);
            this.flpHBReport.TabIndex = 7;
            // 
            // lblHBR
            // 
            this.lblHBR.AutoSize = true;
            this.lblHBR.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHBR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHBR.Location = new System.Drawing.Point(23, 12);
            this.lblHBR.Margin = new System.Windows.Forms.Padding(23, 12, 0, 0);
            this.lblHBR.Name = "lblHBR";
            this.lblHBR.Size = new System.Drawing.Size(89, 19);
            this.lblHBR.TabIndex = 4;
            this.lblHBR.Text = "Bills Report";
            // 
            // pnlBRButtons
            // 
            this.pnlBRButtons.Controls.Add(this.lblPBInformation);
            this.pnlBRButtons.Controls.Add(this.btnBRPClient);
            this.pnlBRButtons.Controls.Add(this.btnBRSummary);
            this.pnlBRButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBRButtons.Location = new System.Drawing.Point(0, 45);
            this.pnlBRButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBRButtons.Name = "pnlBRButtons";
            this.pnlBRButtons.Size = new System.Drawing.Size(364, 186);
            this.pnlBRButtons.TabIndex = 8;
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
            // btnBRPClient
            // 
            this.btnBRPClient.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBRPClient.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBRPClient.FlatAppearance.BorderSize = 0;
            this.btnBRPClient.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnBRPClient.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnBRPClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBRPClient.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBRPClient.ForeColor = System.Drawing.Color.White;
            this.btnBRPClient.Location = new System.Drawing.Point(52, 30);
            this.btnBRPClient.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnBRPClient.Name = "btnBRPClient";
            this.btnBRPClient.Size = new System.Drawing.Size(120, 35);
            this.btnBRPClient.TabIndex = 7;
            this.btnBRPClient.TabStop = false;
            this.btnBRPClient.Text = "P&er Client";
            this.btnBRPClient.UseVisualStyleBackColor = false;
            this.btnBRPClient.Click += new System.EventHandler(this.btnBRPClient_Click);
            // 
            // btnBRSummary
            // 
            this.btnBRSummary.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBRSummary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBRSummary.FlatAppearance.BorderSize = 0;
            this.btnBRSummary.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnBRSummary.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnBRSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBRSummary.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBRSummary.ForeColor = System.Drawing.Color.White;
            this.btnBRSummary.Location = new System.Drawing.Point(192, 30);
            this.btnBRSummary.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.btnBRSummary.Name = "btnBRSummary";
            this.btnBRSummary.Size = new System.Drawing.Size(120, 35);
            this.btnBRSummary.TabIndex = 8;
            this.btnBRSummary.TabStop = false;
            this.btnBRSummary.Text = "&Summary";
            this.btnBRSummary.UseVisualStyleBackColor = false;
            this.btnBRSummary.Click += new System.EventHandler(this.btnBRSummary_Click);
            // 
            // BReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(364, 231);
            this.Controls.Add(this.tlpBReport);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(380, 270);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 270);
            this.Name = "BReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bills Report";
            this.tlpBReport.ResumeLayout(false);
            this.flpHBReport.ResumeLayout(false);
            this.flpHBReport.PerformLayout();
            this.pnlBRButtons.ResumeLayout(false);
            this.pnlBRButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBReport;
        private System.Windows.Forms.FlowLayoutPanel flpHBReport;
        private System.Windows.Forms.Label lblHBR;
        private System.Windows.Forms.Panel pnlBRButtons;
        private System.Windows.Forms.Button btnBRPClient;
        private System.Windows.Forms.Button btnBRSummary;
        private System.Windows.Forms.Label lblPBInformation;
    }
}