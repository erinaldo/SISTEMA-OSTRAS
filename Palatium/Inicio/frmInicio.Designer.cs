namespace Palatium.Inicio
{
    partial class frmInicio
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
            this.botonCircular1 = new Palatium.ControlesNuevos.BotonCircular();
            this.botonCircular2 = new Palatium.ControlesNuevos.BotonCircular();
            this.botonCircular3 = new Palatium.ControlesNuevos.BotonCircular();
            this.SuspendLayout();
            // 
            // botonCircular1
            // 
            this.botonCircular1.BackColor = System.Drawing.Color.Lime;
            this.botonCircular1.FlatAppearance.BorderSize = 0;
            this.botonCircular1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botonCircular1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonCircular1.Location = new System.Drawing.Point(103, 23);
            this.botonCircular1.Name = "botonCircular1";
            this.botonCircular1.Size = new System.Drawing.Size(100, 100);
            this.botonCircular1.TabIndex = 1;
            this.botonCircular1.Text = "1";
            this.botonCircular1.UseVisualStyleBackColor = false;
            // 
            // botonCircular2
            // 
            this.botonCircular2.BackColor = System.Drawing.Color.Lime;
            this.botonCircular2.FlatAppearance.BorderSize = 0;
            this.botonCircular2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botonCircular2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonCircular2.Location = new System.Drawing.Point(283, 23);
            this.botonCircular2.Name = "botonCircular2";
            this.botonCircular2.Size = new System.Drawing.Size(100, 100);
            this.botonCircular2.TabIndex = 2;
            this.botonCircular2.Text = "1";
            this.botonCircular2.UseVisualStyleBackColor = false;
            // 
            // botonCircular3
            // 
            this.botonCircular3.Location = new System.Drawing.Point(218, 129);
            this.botonCircular3.Name = "botonCircular3";
            this.botonCircular3.Size = new System.Drawing.Size(93, 84);
            this.botonCircular3.TabIndex = 3;
            this.botonCircular3.Text = "botonCircular3";
            this.botonCircular3.UseVisualStyleBackColor = true;
            // 
            // frmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(565, 273);
            this.Controls.Add(this.botonCircular3);
            this.Controls.Add(this.botonCircular2);
            this.Controls.Add(this.botonCircular1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PALATIUM REST - INICIO";
            this.ResumeLayout(false);

        }

        #endregion

        private ControlesNuevos.BotonCircular botonCircular1;
        private ControlesNuevos.BotonCircular botonCircular2;
        private ControlesNuevos.BotonCircular botonCircular3;

    }
}