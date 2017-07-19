namespace Nieva
{
    partial class VDate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDate));
            this.tlpVDate = new System.Windows.Forms.TableLayoutPanel();
            this.flpVButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnVCancel = new System.Windows.Forms.Button();
            this.btnVOk = new System.Windows.Forms.Button();
            this.flpHVDate = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHVDate = new System.Windows.Forms.Label();
            this.flpVDate = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFDate = new System.Windows.Forms.Label();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblTDate = new System.Windows.Forms.Label();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.tlpVDate.SuspendLayout();
            this.flpVButtons.SuspendLayout();
            this.flpHVDate.SuspendLayout();
            this.flpVDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpVDate
            // 
            this.tlpVDate.ColumnCount = 1;
            this.tlpVDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVDate.Controls.Add(this.flpVButtons, 0, 2);
            this.tlpVDate.Controls.Add(this.flpHVDate, 0, 0);
            this.tlpVDate.Controls.Add(this.flpVDate, 0, 1);
            this.tlpVDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpVDate.Location = new System.Drawing.Point(0, 0);
            this.tlpVDate.Name = "tlpVDate";
            this.tlpVDate.RowCount = 3;
            this.tlpVDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpVDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpVDate.Size = new System.Drawing.Size(364, 231);
            this.tlpVDate.TabIndex = 1;
            // 
            // flpVButtons
            // 
            this.flpVButtons.Controls.Add(this.btnVCancel);
            this.flpVButtons.Controls.Add(this.btnVOk);
            this.flpVButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpVButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpVButtons.Location = new System.Drawing.Point(0, 161);
            this.flpVButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpVButtons.Name = "flpVButtons";
            this.flpVButtons.Size = new System.Drawing.Size(364, 70);
            this.flpVButtons.TabIndex = 8;
            // 
            // btnVCancel
            // 
            this.btnVCancel.BackColor = System.Drawing.Color.LimeGreen;
            this.btnVCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVCancel.FlatAppearance.BorderSize = 0;
            this.btnVCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnVCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnVCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVCancel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVCancel.ForeColor = System.Drawing.Color.White;
            this.btnVCancel.Location = new System.Drawing.Point(224, 0);
            this.btnVCancel.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnVCancel.Name = "btnVCancel";
            this.btnVCancel.Size = new System.Drawing.Size(120, 35);
            this.btnVCancel.TabIndex = 3;
            this.btnVCancel.TabStop = false;
            this.btnVCancel.Text = "&Cancel";
            this.btnVCancel.UseVisualStyleBackColor = false;
            this.btnVCancel.Click += new System.EventHandler(this.btnVCancel_Click);
            // 
            // btnVOk
            // 
            this.btnVOk.BackColor = System.Drawing.Color.LimeGreen;
            this.btnVOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVOk.FlatAppearance.BorderSize = 0;
            this.btnVOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnVOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnVOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVOk.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVOk.ForeColor = System.Drawing.Color.White;
            this.btnVOk.Location = new System.Drawing.Point(84, 0);
            this.btnVOk.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnVOk.Name = "btnVOk";
            this.btnVOk.Size = new System.Drawing.Size(120, 35);
            this.btnVOk.TabIndex = 4;
            this.btnVOk.TabStop = false;
            this.btnVOk.Text = "&Ok";
            this.btnVOk.UseVisualStyleBackColor = false;
            this.btnVOk.Click += new System.EventHandler(this.btnVOk_Click);
            // 
            // flpHVDate
            // 
            this.flpHVDate.Controls.Add(this.lblHVDate);
            this.flpHVDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHVDate.Location = new System.Drawing.Point(0, 0);
            this.flpHVDate.Margin = new System.Windows.Forms.Padding(0);
            this.flpHVDate.Name = "flpHVDate";
            this.flpHVDate.Size = new System.Drawing.Size(364, 45);
            this.flpHVDate.TabIndex = 7;
            // 
            // lblHVDate
            // 
            this.lblHVDate.AutoSize = true;
            this.lblHVDate.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHVDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHVDate.Location = new System.Drawing.Point(20, 10);
            this.lblHVDate.Margin = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.lblHVDate.Name = "lblHVDate";
            this.lblHVDate.Size = new System.Drawing.Size(107, 19);
            this.lblHVDate.TabIndex = 4;
            this.lblHVDate.Text = "Validity Date";
            // 
            // flpVDate
            // 
            this.flpVDate.Controls.Add(this.lblFDate);
            this.flpVDate.Controls.Add(this.dtpFDate);
            this.flpVDate.Controls.Add(this.lblTDate);
            this.flpVDate.Controls.Add(this.dtpTDate);
            this.flpVDate.Location = new System.Drawing.Point(0, 45);
            this.flpVDate.Margin = new System.Windows.Forms.Padding(0);
            this.flpVDate.Name = "flpVDate";
            this.flpVDate.Size = new System.Drawing.Size(364, 116);
            this.flpVDate.TabIndex = 9;
            // 
            // lblFDate
            // 
            this.lblFDate.AutoSize = true;
            this.lblFDate.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblFDate.Location = new System.Drawing.Point(56, 6);
            this.lblFDate.Margin = new System.Windows.Forms.Padding(56, 6, 0, 0);
            this.lblFDate.Name = "lblFDate";
            this.lblFDate.Size = new System.Drawing.Size(48, 17);
            this.lblFDate.TabIndex = 75;
            this.lblFDate.Text = "From :";
            // 
            // dtpFDate
            // 
            this.dtpFDate.CustomFormat = " ";
            this.dtpFDate.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFDate.Location = new System.Drawing.Point(107, 3);
            this.dtpFDate.Margin = new System.Windows.Forms.Padding(3, 3, 10, 20);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(200, 23);
            this.dtpFDate.TabIndex = 74;
            this.dtpFDate.ValueChanged += new System.EventHandler(this.dtpFDate_ValueChanged);
            // 
            // lblTDate
            // 
            this.lblTDate.AutoSize = true;
            this.lblTDate.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTDate.Location = new System.Drawing.Point(74, 52);
            this.lblTDate.Margin = new System.Windows.Forms.Padding(74, 6, 0, 0);
            this.lblTDate.Name = "lblTDate";
            this.lblTDate.Size = new System.Drawing.Size(30, 17);
            this.lblTDate.TabIndex = 77;
            this.lblTDate.Text = "To :";
            // 
            // dtpTDate
            // 
            this.dtpTDate.CustomFormat = " ";
            this.dtpTDate.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTDate.Location = new System.Drawing.Point(107, 49);
            this.dtpTDate.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(200, 23);
            this.dtpTDate.TabIndex = 76;
            this.dtpTDate.ValueChanged += new System.EventHandler(this.dtpTDate_ValueChanged);
            // 
            // VDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(364, 231);
            this.Controls.Add(this.tlpVDate);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VDate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validity Date";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VDate_KeyDown);
            this.tlpVDate.ResumeLayout(false);
            this.flpVButtons.ResumeLayout(false);
            this.flpHVDate.ResumeLayout(false);
            this.flpHVDate.PerformLayout();
            this.flpVDate.ResumeLayout(false);
            this.flpVDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpVDate;
        private System.Windows.Forms.FlowLayoutPanel flpVButtons;
        private System.Windows.Forms.Button btnVCancel;
        private System.Windows.Forms.Button btnVOk;
        private System.Windows.Forms.FlowLayoutPanel flpHVDate;
        private System.Windows.Forms.Label lblHVDate;
        private System.Windows.Forms.FlowLayoutPanel flpVDate;
        private System.Windows.Forms.Label lblFDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label lblTDate;
        private System.Windows.Forms.DateTimePicker dtpTDate;
    }
}