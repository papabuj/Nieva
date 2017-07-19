namespace Nieva
{
    partial class SScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SScreen));
            this.ptbNieva = new System.Windows.Forms.PictureBox();
            this.tmrSplash = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ptbNieva)).BeginInit();
            this.SuspendLayout();
            // 
            // ptbNieva
            // 
            this.ptbNieva.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ptbNieva.Image = ((System.Drawing.Image)(resources.GetObject("ptbNieva.Image")));
            this.ptbNieva.Location = new System.Drawing.Point(0, 0);
            this.ptbNieva.Margin = new System.Windows.Forms.Padding(0);
            this.ptbNieva.Name = "ptbNieva";
            this.ptbNieva.Size = new System.Drawing.Size(440, 250);
            this.ptbNieva.TabIndex = 0;
            this.ptbNieva.TabStop = false;
            // 
            // tmrSplash
            // 
            this.tmrSplash.Tick += new System.EventHandler(this.tmrSplash_Tick);
            // 
            // SScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(440, 250);
            this.ControlBox = false;
            this.Controls.Add(this.ptbNieva);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(440, 250);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 250);
            this.Name = "SScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SScreen";
            ((System.ComponentModel.ISupportInitialize)(this.ptbNieva)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ptbNieva;
        private System.Windows.Forms.Timer tmrSplash;
    }
}