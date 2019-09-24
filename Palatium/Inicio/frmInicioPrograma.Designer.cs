namespace Palatium.Inicio
{
    partial class frmInicioPrograma
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnAcerca = new System.Windows.Forms.LinkLabel();
            this.lblContacto = new System.Windows.Forms.Label();
            this.lblSitioWeb = new System.Windows.Forms.LinkLabel();
            this.btnOficina = new System.Windows.Forms.Button();
            this.btnEntradaCajero = new System.Windows.Forms.Button();
            this.btnAperturarCaja = new System.Windows.Forms.Button();
            this.btnSalidaCajero = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Palatium.Properties.Resources.logo_palatium_rest_2;
            this.pictureBox1.Location = new System.Drawing.Point(392, 142);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(611, 219);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnAcerca
            // 
            this.btnAcerca.AutoSize = true;
            this.btnAcerca.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcerca.Location = new System.Drawing.Point(551, 336);
            this.btnAcerca.Name = "btnAcerca";
            this.btnAcerca.Size = new System.Drawing.Size(81, 20);
            this.btnAcerca.TabIndex = 54;
            this.btnAcerca.TabStop = true;
            this.btnAcerca.Text = "Acerca de";
            this.btnAcerca.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnAcerca_LinkClicked);
            // 
            // lblContacto
            // 
            this.lblContacto.AutoSize = true;
            this.lblContacto.BackColor = System.Drawing.Color.White;
            this.lblContacto.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContacto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblContacto.Location = new System.Drawing.Point(418, 300);
            this.lblContacto.Name = "lblContacto";
            this.lblContacto.Size = new System.Drawing.Size(214, 24);
            this.lblContacto.TabIndex = 53;
            this.lblContacto.Text = "Contacto: 0995610690";
            // 
            // lblSitioWeb
            // 
            this.lblSitioWeb.AutoSize = true;
            this.lblSitioWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSitioWeb.Location = new System.Drawing.Point(418, 336);
            this.lblSitioWeb.Name = "lblSitioWeb";
            this.lblSitioWeb.Size = new System.Drawing.Size(124, 20);
            this.lblSitioWeb.TabIndex = 52;
            this.lblSitioWeb.TabStop = true;
            this.lblSitioWeb.Text = "www.aplicsis.net";
            this.lblSitioWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblSitioWeb_LinkClicked);
            // 
            // btnOficina
            // 
            this.btnOficina.BackColor = System.Drawing.Color.Navy;
            this.btnOficina.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOficina.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOficina.FlatAppearance.BorderSize = 2;
            this.btnOficina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOficina.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOficina.ForeColor = System.Drawing.Color.White;
            this.btnOficina.Image = global::Palatium.Properties.Resources.icono_oficina;
            this.btnOficina.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOficina.Location = new System.Drawing.Point(613, 419);
            this.btnOficina.Name = "btnOficina";
            this.btnOficina.Size = new System.Drawing.Size(192, 138);
            this.btnOficina.TabIndex = 72;
            this.btnOficina.Text = "Oficina\r\nAdministración";
            this.btnOficina.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOficina.UseVisualStyleBackColor = false;
            this.btnOficina.Click += new System.EventHandler(this.btnOficina_Click);
            this.btnOficina.MouseEnter += new System.EventHandler(this.btnOficina_MouseEnter);
            this.btnOficina.MouseLeave += new System.EventHandler(this.btnOficina_MouseLeave);
            // 
            // btnEntradaCajero
            // 
            this.btnEntradaCajero.BackColor = System.Drawing.Color.Navy;
            this.btnEntradaCajero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEntradaCajero.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEntradaCajero.FlatAppearance.BorderSize = 2;
            this.btnEntradaCajero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEntradaCajero.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEntradaCajero.ForeColor = System.Drawing.Color.White;
            this.btnEntradaCajero.Image = global::Palatium.Properties.Resources.icono_login;
            this.btnEntradaCajero.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEntradaCajero.Location = new System.Drawing.Point(392, 419);
            this.btnEntradaCajero.Name = "btnEntradaCajero";
            this.btnEntradaCajero.Size = new System.Drawing.Size(192, 138);
            this.btnEntradaCajero.TabIndex = 73;
            this.btnEntradaCajero.Text = "Ingreso Usuario\r\n ";
            this.btnEntradaCajero.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEntradaCajero.UseVisualStyleBackColor = false;
            this.btnEntradaCajero.Visible = false;
            this.btnEntradaCajero.Click += new System.EventHandler(this.btnEntradaCajero_Click);
            this.btnEntradaCajero.MouseEnter += new System.EventHandler(this.btnEntradaCajero_MouseEnter);
            this.btnEntradaCajero.MouseLeave += new System.EventHandler(this.btnEntradaCajero_MouseLeave);
            // 
            // btnAperturarCaja
            // 
            this.btnAperturarCaja.BackColor = System.Drawing.Color.Navy;
            this.btnAperturarCaja.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAperturarCaja.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAperturarCaja.FlatAppearance.BorderSize = 2;
            this.btnAperturarCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAperturarCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAperturarCaja.ForeColor = System.Drawing.Color.White;
            this.btnAperturarCaja.Image = global::Palatium.Properties.Resources.abrir_caja_inicial_menu;
            this.btnAperturarCaja.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAperturarCaja.Location = new System.Drawing.Point(996, 419);
            this.btnAperturarCaja.Name = "btnAperturarCaja";
            this.btnAperturarCaja.Size = new System.Drawing.Size(192, 138);
            this.btnAperturarCaja.TabIndex = 74;
            this.btnAperturarCaja.Text = "Aperturar Caja\r\n ";
            this.btnAperturarCaja.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAperturarCaja.UseVisualStyleBackColor = false;
            this.btnAperturarCaja.Visible = false;
            this.btnAperturarCaja.MouseEnter += new System.EventHandler(this.btnAperturarCaja_MouseEnter);
            this.btnAperturarCaja.MouseLeave += new System.EventHandler(this.btnAperturarCaja_MouseLeave);
            // 
            // btnSalidaCajero
            // 
            this.btnSalidaCajero.BackColor = System.Drawing.Color.Navy;
            this.btnSalidaCajero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSalidaCajero.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalidaCajero.FlatAppearance.BorderSize = 2;
            this.btnSalidaCajero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalidaCajero.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalidaCajero.ForeColor = System.Drawing.Color.White;
            this.btnSalidaCajero.Image = global::Palatium.Properties.Resources.icono_abrir_caja;
            this.btnSalidaCajero.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSalidaCajero.Location = new System.Drawing.Point(119, 232);
            this.btnSalidaCajero.Name = "btnSalidaCajero";
            this.btnSalidaCajero.Size = new System.Drawing.Size(192, 138);
            this.btnSalidaCajero.TabIndex = 75;
            this.btnSalidaCajero.Text = "Arqueo de Caja\r\n  ";
            this.btnSalidaCajero.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSalidaCajero.UseVisualStyleBackColor = false;
            this.btnSalidaCajero.Visible = false;
            // 
            // frmInicioPrograma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(1360, 694);
            this.Controls.Add(this.btnSalidaCajero);
            this.Controls.Add(this.btnAperturarCaja);
            this.Controls.Add(this.btnEntradaCajero);
            this.Controls.Add(this.btnOficina);
            this.Controls.Add(this.btnAcerca);
            this.Controls.Add(this.lblContacto);
            this.Controls.Add(this.lblSitioWeb);
            this.Controls.Add(this.pictureBox1);
            this.Name = "frmInicioPrograma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "INICIO";
            this.Load += new System.EventHandler(this.frmInicioPrograma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel btnAcerca;
        private System.Windows.Forms.Label lblContacto;
        private System.Windows.Forms.LinkLabel lblSitioWeb;
        private System.Windows.Forms.Button btnOficina;
        private System.Windows.Forms.Button btnEntradaCajero;
        private System.Windows.Forms.Button btnAperturarCaja;
        private System.Windows.Forms.Button btnSalidaCajero;
    }
}