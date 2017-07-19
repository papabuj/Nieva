namespace Nieva
{
    partial class Transfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Transfer));
            this.tlpTransfer = new System.Windows.Forms.TableLayoutPanel();
            this.flpTButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTCancel = new System.Windows.Forms.Button();
            this.btnTOk = new System.Windows.Forms.Button();
            this.flpHTransfer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHTransfer = new System.Windows.Forms.Label();
            this.flpTransfer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTTo = new System.Windows.Forms.Label();
            this.cmbTTo = new System.Windows.Forms.ComboBox();
            this.lblTInformation = new System.Windows.Forms.Label();
            this.tlpTransfer.SuspendLayout();
            this.flpTButtons.SuspendLayout();
            this.flpHTransfer.SuspendLayout();
            this.flpTransfer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpTransfer
            // 
            this.tlpTransfer.ColumnCount = 1;
            this.tlpTransfer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTransfer.Controls.Add(this.flpTButtons, 0, 2);
            this.tlpTransfer.Controls.Add(this.flpHTransfer, 0, 0);
            this.tlpTransfer.Controls.Add(this.flpTransfer, 0, 1);
            this.tlpTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTransfer.Location = new System.Drawing.Point(0, 0);
            this.tlpTransfer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tlpTransfer.Name = "tlpTransfer";
            this.tlpTransfer.RowCount = 3;
            this.tlpTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpTransfer.Size = new System.Drawing.Size(364, 231);
            this.tlpTransfer.TabIndex = 1;
            // 
            // flpTButtons
            // 
            this.flpTButtons.Controls.Add(this.btnTCancel);
            this.flpTButtons.Controls.Add(this.btnTOk);
            this.flpTButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpTButtons.Location = new System.Drawing.Point(0, 161);
            this.flpTButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpTButtons.Name = "flpTButtons";
            this.flpTButtons.Size = new System.Drawing.Size(364, 70);
            this.flpTButtons.TabIndex = 8;
            // 
            // btnTCancel
            // 
            this.btnTCancel.BackColor = System.Drawing.Color.LimeGreen;
            this.btnTCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTCancel.FlatAppearance.BorderSize = 0;
            this.btnTCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnTCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnTCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTCancel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTCancel.ForeColor = System.Drawing.Color.White;
            this.btnTCancel.Location = new System.Drawing.Point(224, 0);
            this.btnTCancel.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnTCancel.Name = "btnTCancel";
            this.btnTCancel.Size = new System.Drawing.Size(120, 35);
            this.btnTCancel.TabIndex = 3;
            this.btnTCancel.TabStop = false;
            this.btnTCancel.Text = "&Cancel";
            this.btnTCancel.UseVisualStyleBackColor = false;
            this.btnTCancel.Click += new System.EventHandler(this.btnTCancel_Click);
            // 
            // btnTOk
            // 
            this.btnTOk.BackColor = System.Drawing.Color.LimeGreen;
            this.btnTOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTOk.FlatAppearance.BorderSize = 0;
            this.btnTOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnTOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnTOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTOk.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTOk.ForeColor = System.Drawing.Color.White;
            this.btnTOk.Location = new System.Drawing.Point(84, 0);
            this.btnTOk.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnTOk.Name = "btnTOk";
            this.btnTOk.Size = new System.Drawing.Size(120, 35);
            this.btnTOk.TabIndex = 4;
            this.btnTOk.TabStop = false;
            this.btnTOk.Text = "&Ok";
            this.btnTOk.UseVisualStyleBackColor = false;
            this.btnTOk.Click += new System.EventHandler(this.btnTOk_Click);
            // 
            // flpHTransfer
            // 
            this.flpHTransfer.Controls.Add(this.lblHTransfer);
            this.flpHTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHTransfer.Location = new System.Drawing.Point(0, 0);
            this.flpHTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.flpHTransfer.Name = "flpHTransfer";
            this.flpHTransfer.Size = new System.Drawing.Size(364, 45);
            this.flpHTransfer.TabIndex = 7;
            // 
            // lblHTransfer
            // 
            this.lblHTransfer.AutoSize = true;
            this.lblHTransfer.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHTransfer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHTransfer.Location = new System.Drawing.Point(20, 10);
            this.lblHTransfer.Margin = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.lblHTransfer.Name = "lblHTransfer";
            this.lblHTransfer.Size = new System.Drawing.Size(67, 19);
            this.lblHTransfer.TabIndex = 4;
            this.lblHTransfer.Text = "Transfer";
            // 
            // flpTransfer
            // 
            this.flpTransfer.Controls.Add(this.lblTTo);
            this.flpTransfer.Controls.Add(this.cmbTTo);
            this.flpTransfer.Controls.Add(this.lblTInformation);
            this.flpTransfer.Location = new System.Drawing.Point(0, 45);
            this.flpTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.flpTransfer.Name = "flpTransfer";
            this.flpTransfer.Size = new System.Drawing.Size(364, 116);
            this.flpTransfer.TabIndex = 9;
            // 
            // lblTTo
            // 
            this.lblTTo.AutoSize = true;
            this.lblTTo.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTTo.Location = new System.Drawing.Point(67, 7);
            this.lblTTo.Margin = new System.Windows.Forms.Padding(67, 7, 0, 0);
            this.lblTTo.Name = "lblTTo";
            this.lblTTo.Size = new System.Drawing.Size(30, 17);
            this.lblTTo.TabIndex = 75;
            this.lblTTo.Text = "To :";
            // 
            // cmbTTo
            // 
            this.cmbTTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTTo.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTTo.FormattingEnabled = true;
            this.cmbTTo.Location = new System.Drawing.Point(100, 3);
            this.cmbTTo.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.cmbTTo.Name = "cmbTTo";
            this.cmbTTo.Size = new System.Drawing.Size(200, 25);
            this.cmbTTo.TabIndex = 77;
            // 
            // lblTInformation
            // 
            this.lblTInformation.AutoSize = true;
            this.lblTInformation.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTInformation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTInformation.Location = new System.Drawing.Point(20, 53);
            this.lblTInformation.Margin = new System.Windows.Forms.Padding(20, 15, 20, 0);
            this.lblTInformation.Name = "lblTInformation";
            this.lblTInformation.Size = new System.Drawing.Size(314, 34);
            this.lblTInformation.TabIndex = 76;
            this.lblTInformation.Text = "      All clients displayed in the table will transfer to selected staff.";
            // 
            // Transfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(364, 231);
            this.Controls.Add(this.tlpTransfer);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Transfer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transfer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Transfer_KeyDown);
            this.tlpTransfer.ResumeLayout(false);
            this.flpTButtons.ResumeLayout(false);
            this.flpHTransfer.ResumeLayout(false);
            this.flpHTransfer.PerformLayout();
            this.flpTransfer.ResumeLayout(false);
            this.flpTransfer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpTransfer;
        private System.Windows.Forms.FlowLayoutPanel flpTButtons;
        private System.Windows.Forms.Button btnTCancel;
        private System.Windows.Forms.Button btnTOk;
        private System.Windows.Forms.FlowLayoutPanel flpHTransfer;
        private System.Windows.Forms.Label lblHTransfer;
        private System.Windows.Forms.FlowLayoutPanel flpTransfer;
        private System.Windows.Forms.Label lblTTo;
        private System.Windows.Forms.Label lblTInformation;
        private System.Windows.Forms.ComboBox cmbTTo;
    }
}