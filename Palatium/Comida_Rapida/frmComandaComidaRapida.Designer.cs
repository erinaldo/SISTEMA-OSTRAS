namespace Palatium.Comida_Rapida
{
    partial class frmComandaComidaRapida
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
            this.pnlComanda = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlComanda
            // 
            this.pnlComanda.BackColor = System.Drawing.SystemColors.Window;
            this.pnlComanda.Location = new System.Drawing.Point(12, 18);
            this.pnlComanda.Name = "pnlComanda";
            this.pnlComanda.Size = new System.Drawing.Size(915, 555);
            this.pnlComanda.TabIndex = 0;
            // 
            // frmComandaComidaRapida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1362, 741);
            this.Controls.Add(this.pnlComanda);
            this.Name = "frmComandaComidaRapida";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Comanda";
            this.Load += new System.EventHandler(this.frmComandaComidaRapida_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlComanda;
    }
}