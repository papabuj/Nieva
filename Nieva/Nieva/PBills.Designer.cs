namespace Nieva
{
    partial class PBills
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PBills));
            this.tlpPBills = new System.Windows.Forms.TableLayoutPanel();
            this.flpPBButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPBCancel = new System.Windows.Forms.Button();
            this.btnPBOk = new System.Windows.Forms.Button();
            this.flpHPBills = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHPB = new System.Windows.Forms.Label();
            this.flpBDate = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBDate = new System.Windows.Forms.Label();
            this.dtpBDate = new System.Windows.Forms.DateTimePicker();
            this.lblPBInformation = new System.Windows.Forms.Label();
            this.tlpPBills.SuspendLayout();
            this.flpPBButtons.SuspendLayout();
            this.flpHPBills.SuspendLayout();
            this.flpBDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpPBills
            // 
            this.tlpPBills.ColumnCount = 1;
            this.tlpPBills.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPBills.Controls.Add(this.flpPBButtons, 0, 2);
            this.tlpPBills.Controls.Add(this.flpHPBills, 0, 0);
            this.tlpPBills.Controls.Add(this.flpBDate, 0, 1);
            this.tlpPBills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPBills.Location = new System.Drawing.Point(0, 0);
            this.tlpPBills.Name = "tlpPBills";
            this.tlpPBills.RowCount = 3;
            this.tlpPBills.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpPBills.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPBills.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpPBills.Size = new System.Drawing.Size(364, 231);
            this.tlpPBills.TabIndex = 0;
            // 
            // flpPBButtons
            // 
            this.flpPBButtons.Controls.Add(this.btnPBCancel);
            this.flpPBButtons.Controls.Add(this.btnPBOk);
            this.flpPBButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPBButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpPBButtons.Location = new System.Drawing.Point(0, 161);
            this.flpPBButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpPBButtons.Name = "flpPBButtons";
            this.flpPBButtons.Size = new System.Drawing.Size(364, 70);
            this.flpPBButtons.TabIndex = 8;
            // 
            // btnPBCancel
            // 
            this.btnPBCancel.BackColor = System.Drawing.Color.LimeGreen;
            this.btnPBCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPBCancel.FlatAppearance.BorderSize = 0;
            this.btnPBCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnPBCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnPBCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPBCancel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPBCancel.ForeColor = System.Drawing.Color.White;
            this.btnPBCancel.Location = new System.Drawing.Point(224, 0);
            this.btnPBCancel.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnPBCancel.Name = "btnPBCancel";
            this.btnPBCancel.Size = new System.Drawing.Size(120, 35);
            this.btnPBCancel.TabIndex = 3;
            this.btnPBCancel.TabStop = false;
            this.btnPBCancel.Text = "&Cancel";
            this.btnPBCancel.UseVisualStyleBackColor = false;
            this.btnPBCancel.Click += new System.EventHandler(this.btnPBCancel_Click);
            // 
            // btnPBOk
            // 
            this.btnPBOk.BackColor = System.Drawing.Color.LimeGreen;
            this.btnPBOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPBOk.FlatAppearance.BorderSize = 0;
            this.btnPBOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnPBOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnPBOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPBOk.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPBOk.ForeColor = System.Drawing.Color.White;
            this.btnPBOk.Location = new System.Drawing.Point(84, 0);
            this.btnPBOk.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnPBOk.Name = "btnPBOk";
            this.btnPBOk.Size = new System.Drawing.Size(120, 35);
            this.btnPBOk.TabIndex = 4;
            this.btnPBOk.TabStop = false;
            this.btnPBOk.Text = "&Ok";
            this.btnPBOk.UseVisualStyleBackColor = false;
            this.btnPBOk.Click += new System.EventHandler(this.btnPBOk_Click);
            // 
            // flpHPBills
            // 
            this.flpHPBills.Controls.Add(this.lblHPB);
            this.flpHPBills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHPBills.Location = new System.Drawing.Point(0, 0);
            this.flpHPBills.Margin = new System.Windows.Forms.Padding(0);
            this.flpHPBills.Name = "flpHPBills";
            this.flpHPBills.Size = new System.Drawing.Size(364, 45);
            this.flpHPBills.TabIndex = 7;
            // 
            // lblHPB
            // 
            this.lblHPB.AutoSize = true;
            this.lblHPB.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHPB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHPB.Location = new System.Drawing.Point(20, 10);
            this.lblHPB.Margin = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.lblHPB.Name = "lblHPB";
            this.lblHPB.Size = new System.Drawing.Size(105, 19);
            this.lblHPB.TabIndex = 4;
            this.lblHPB.Text = "Produce Bills";
            // 
            // flpBDate
            // 
            this.flpBDate.Controls.Add(this.lblBDate);
            this.flpBDate.Controls.Add(this.dtpBDate);
            this.flpBDate.Controls.Add(this.lblPBInformation);
            this.flpBDate.Location = new System.Drawing.Point(0, 45);
            this.flpBDate.Margin = new System.Windows.Forms.Padding(0);
            this.flpBDate.Name = "flpBDate";
            this.flpBDate.Size = new System.Drawing.Size(364, 116);
            this.flpBDate.TabIndex = 9;
            // 
            // lblBDate
            // 
            this.lblBDate.AutoSize = true;
            this.lblBDate.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblBDate.Location = new System.Drawing.Point(46, 6);
            this.lblBDate.Margin = new System.Windows.Forms.Padding(46, 6, 0, 0);
            this.lblBDate.Name = "lblBDate";
            this.lblBDate.Size = new System.Drawing.Size(68, 17);
            this.lblBDate.TabIndex = 75;
            this.lblBDate.Text = "Bill Date :";
            // 
            // dtpBDate
            // 
            this.dtpBDate.CustomFormat = " ";
            this.dtpBDate.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBDate.Location = new System.Drawing.Point(117, 3);
            this.dtpBDate.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.dtpBDate.Name = "dtpBDate";
            this.dtpBDate.Size = new System.Drawing.Size(200, 23);
            this.dtpBDate.TabIndex = 74;
            this.dtpBDate.ValueChanged += new System.EventHandler(this.dtpBDate_ValueChanged);
            // 
            // lblPBInformation
            // 
            this.lblPBInformation.AutoSize = true;
            this.lblPBInformation.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPBInformation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPBInformation.Location = new System.Drawing.Point(20, 51);
            this.lblPBInformation.Margin = new System.Windows.Forms.Padding(20, 15, 20, 0);
            this.lblPBInformation.Name = "lblPBInformation";
            this.lblPBInformation.Size = new System.Drawing.Size(282, 34);
            this.lblPBInformation.TabIndex = 76;
            this.lblPBInformation.Text = "      All availed services of all active clients displayed in the table will prod" +
    "uce to bills.";
            // 
            // PBills
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(364, 231);
            this.Controls.Add(this.tlpPBills);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(380, 270);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 270);
            this.Name = "PBills";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Produce Bills";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PBills_KeyDown);
            this.tlpPBills.ResumeLayout(false);
            this.flpPBButtons.ResumeLayout(false);
            this.flpHPBills.ResumeLayout(false);
            this.flpHPBills.PerformLayout();
            this.flpBDate.ResumeLayout(false);
            this.flpBDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpPBills;
        private System.Windows.Forms.FlowLayoutPanel flpHPBills;
        private System.Windows.Forms.Label lblHPB;
        private System.Windows.Forms.FlowLayoutPanel flpPBButtons;
        private System.Windows.Forms.Button btnPBCancel;
        private System.Windows.Forms.Button btnPBOk;
        private System.Windows.Forms.FlowLayoutPanel flpBDate;
        private System.Windows.Forms.Label lblBDate;
        private System.Windows.Forms.DateTimePicker dtpBDate;
        private System.Windows.Forms.Label lblPBInformation;
    }
}