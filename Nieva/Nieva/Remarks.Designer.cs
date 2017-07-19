namespace Nieva
{
    partial class Remarks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Remarks));
            this.tlpRemarks = new System.Windows.Forms.TableLayoutPanel();
            this.flpRBButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRCancel = new System.Windows.Forms.Button();
            this.btnROk = new System.Windows.Forms.Button();
            this.flpHRemarks = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHRemarks = new System.Windows.Forms.Label();
            this.flpRemarks = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.tlpRemarks.SuspendLayout();
            this.flpRBButtons.SuspendLayout();
            this.flpHRemarks.SuspendLayout();
            this.flpRemarks.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRemarks
            // 
            this.tlpRemarks.ColumnCount = 1;
            this.tlpRemarks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemarks.Controls.Add(this.flpRBButtons, 0, 2);
            this.tlpRemarks.Controls.Add(this.flpHRemarks, 0, 0);
            this.tlpRemarks.Controls.Add(this.flpRemarks, 0, 1);
            this.tlpRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRemarks.Location = new System.Drawing.Point(0, 0);
            this.tlpRemarks.Name = "tlpRemarks";
            this.tlpRemarks.RowCount = 3;
            this.tlpRemarks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpRemarks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRemarks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpRemarks.Size = new System.Drawing.Size(364, 231);
            this.tlpRemarks.TabIndex = 1;
            // 
            // flpRBButtons
            // 
            this.flpRBButtons.Controls.Add(this.btnRCancel);
            this.flpRBButtons.Controls.Add(this.btnROk);
            this.flpRBButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRBButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpRBButtons.Location = new System.Drawing.Point(0, 161);
            this.flpRBButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpRBButtons.Name = "flpRBButtons";
            this.flpRBButtons.Size = new System.Drawing.Size(364, 70);
            this.flpRBButtons.TabIndex = 1;
            // 
            // btnRCancel
            // 
            this.btnRCancel.BackColor = System.Drawing.Color.LimeGreen;
            this.btnRCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRCancel.FlatAppearance.BorderSize = 0;
            this.btnRCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnRCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnRCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRCancel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRCancel.ForeColor = System.Drawing.Color.White;
            this.btnRCancel.Location = new System.Drawing.Point(224, 0);
            this.btnRCancel.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnRCancel.Name = "btnRCancel";
            this.btnRCancel.Size = new System.Drawing.Size(120, 35);
            this.btnRCancel.TabIndex = 1;
            this.btnRCancel.TabStop = false;
            this.btnRCancel.Text = "&Cancel";
            this.btnRCancel.UseVisualStyleBackColor = false;
            this.btnRCancel.Click += new System.EventHandler(this.btnRCancel_Click);
            // 
            // btnROk
            // 
            this.btnROk.BackColor = System.Drawing.Color.LimeGreen;
            this.btnROk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnROk.FlatAppearance.BorderSize = 0;
            this.btnROk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btnROk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnROk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnROk.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnROk.ForeColor = System.Drawing.Color.White;
            this.btnROk.Location = new System.Drawing.Point(84, 0);
            this.btnROk.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnROk.Name = "btnROk";
            this.btnROk.Size = new System.Drawing.Size(120, 35);
            this.btnROk.TabIndex = 0;
            this.btnROk.TabStop = false;
            this.btnROk.Text = "&Ok";
            this.btnROk.UseVisualStyleBackColor = false;
            this.btnROk.Click += new System.EventHandler(this.btnROk_Click);
            // 
            // flpHRemarks
            // 
            this.flpHRemarks.Controls.Add(this.lblHRemarks);
            this.flpHRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHRemarks.Location = new System.Drawing.Point(0, 0);
            this.flpHRemarks.Margin = new System.Windows.Forms.Padding(0);
            this.flpHRemarks.Name = "flpHRemarks";
            this.flpHRemarks.Size = new System.Drawing.Size(364, 45);
            this.flpHRemarks.TabIndex = 2;
            // 
            // lblHRemarks
            // 
            this.lblHRemarks.AutoSize = true;
            this.lblHRemarks.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHRemarks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHRemarks.Location = new System.Drawing.Point(20, 10);
            this.lblHRemarks.Margin = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.lblHRemarks.Name = "lblHRemarks";
            this.lblHRemarks.Size = new System.Drawing.Size(75, 19);
            this.lblHRemarks.TabIndex = 0;
            this.lblHRemarks.Text = "Remarks";
            // 
            // flpRemarks
            // 
            this.flpRemarks.Controls.Add(this.lblRemarks);
            this.flpRemarks.Controls.Add(this.txtRemarks);
            this.flpRemarks.Location = new System.Drawing.Point(0, 45);
            this.flpRemarks.Margin = new System.Windows.Forms.Padding(0);
            this.flpRemarks.Name = "flpRemarks";
            this.flpRemarks.Size = new System.Drawing.Size(364, 116);
            this.flpRemarks.TabIndex = 0;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemarks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblRemarks.Location = new System.Drawing.Point(44, 6);
            this.lblRemarks.Margin = new System.Windows.Forms.Padding(44, 6, 0, 0);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(70, 17);
            this.lblRemarks.TabIndex = 1;
            this.lblRemarks.Text = "Remarks :";
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(114, 3);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(0, 3, 10, 10);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(200, 90);
            this.txtRemarks.TabIndex = 0;
            this.txtRemarks.TabStop = false;
            // 
            // Remarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(364, 231);
            this.Controls.Add(this.tlpRemarks);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(380, 270);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 270);
            this.Name = "Remarks";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remarks";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Remarks_KeyDown);
            this.tlpRemarks.ResumeLayout(false);
            this.flpRBButtons.ResumeLayout(false);
            this.flpHRemarks.ResumeLayout(false);
            this.flpHRemarks.PerformLayout();
            this.flpRemarks.ResumeLayout(false);
            this.flpRemarks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRemarks;
        private System.Windows.Forms.FlowLayoutPanel flpRBButtons;
        private System.Windows.Forms.Button btnRCancel;
        private System.Windows.Forms.Button btnROk;
        private System.Windows.Forms.FlowLayoutPanel flpHRemarks;
        private System.Windows.Forms.Label lblHRemarks;
        private System.Windows.Forms.FlowLayoutPanel flpRemarks;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
    }
}