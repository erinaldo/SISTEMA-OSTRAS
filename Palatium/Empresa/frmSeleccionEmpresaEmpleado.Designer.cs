namespace Palatium.Empresa
{
    partial class frmSeleccionEmpresaEmpleado
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSiguienteEmpleado = new System.Windows.Forms.Button();
            this.btnAnteriorEmpleado = new System.Windows.Forms.Button();
            this.btnSiguienteEmpresa = new System.Windows.Forms.Button();
            this.btnAnteriorEmpresa = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblNombreEmpresa = new System.Windows.Forms.Label();
            this.btnAgregarEmpleado = new System.Windows.Forms.Button();
            this.btnAgregarEmpresa = new System.Windows.Forms.Button();
            this.pnlEmpleados = new System.Windows.Forms.Panel();
            this.pnlEmpresa = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(349, 47);
            this.panel2.TabIndex = 116;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Maiandra GD", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(93, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 39);
            this.label3.TabIndex = 22;
            this.label3.Text = "EMPRESAS";
            // 
            // btnSiguienteEmpleado
            // 
            this.btnSiguienteEmpleado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnSiguienteEmpleado.Image = global::Palatium.Properties.Resources.derecha;
            this.btnSiguienteEmpleado.Location = new System.Drawing.Point(748, 618);
            this.btnSiguienteEmpleado.Name = "btnSiguienteEmpleado";
            this.btnSiguienteEmpleado.Size = new System.Drawing.Size(82, 71);
            this.btnSiguienteEmpleado.TabIndex = 115;
            this.btnSiguienteEmpleado.UseVisualStyleBackColor = false;
            this.btnSiguienteEmpleado.Visible = false;
            this.btnSiguienteEmpleado.Click += new System.EventHandler(this.btnSiguienteEmpleado_Click);
            // 
            // btnAnteriorEmpleado
            // 
            this.btnAnteriorEmpleado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnAnteriorEmpleado.Enabled = false;
            this.btnAnteriorEmpleado.Image = global::Palatium.Properties.Resources.izquierda;
            this.btnAnteriorEmpleado.Location = new System.Drawing.Point(666, 618);
            this.btnAnteriorEmpleado.Name = "btnAnteriorEmpleado";
            this.btnAnteriorEmpleado.Size = new System.Drawing.Size(82, 71);
            this.btnAnteriorEmpleado.TabIndex = 114;
            this.btnAnteriorEmpleado.UseVisualStyleBackColor = false;
            this.btnAnteriorEmpleado.Visible = false;
            this.btnAnteriorEmpleado.Click += new System.EventHandler(this.btnAnteriorEmpleado_Click);
            // 
            // btnSiguienteEmpresa
            // 
            this.btnSiguienteEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnSiguienteEmpresa.Image = global::Palatium.Properties.Resources.derecha;
            this.btnSiguienteEmpresa.Location = new System.Drawing.Point(292, 618);
            this.btnSiguienteEmpresa.Name = "btnSiguienteEmpresa";
            this.btnSiguienteEmpresa.Size = new System.Drawing.Size(82, 71);
            this.btnSiguienteEmpresa.TabIndex = 113;
            this.btnSiguienteEmpresa.UseVisualStyleBackColor = false;
            this.btnSiguienteEmpresa.Click += new System.EventHandler(this.btnSiguienteEmpresa_Click);
            // 
            // btnAnteriorEmpresa
            // 
            this.btnAnteriorEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnAnteriorEmpresa.Enabled = false;
            this.btnAnteriorEmpresa.Image = global::Palatium.Properties.Resources.izquierda;
            this.btnAnteriorEmpresa.Location = new System.Drawing.Point(211, 618);
            this.btnAnteriorEmpresa.Name = "btnAnteriorEmpresa";
            this.btnAnteriorEmpresa.Size = new System.Drawing.Size(82, 71);
            this.btnAnteriorEmpresa.TabIndex = 112;
            this.btnAnteriorEmpresa.UseVisualStyleBackColor = false;
            this.btnAnteriorEmpresa.Click += new System.EventHandler(this.btnAnteriorEmpresa_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.lblNombreEmpresa);
            this.panel1.Location = new System.Drawing.Point(467, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(872, 47);
            this.panel1.TabIndex = 117;
            // 
            // lblNombreEmpresa
            // 
            this.lblNombreEmpresa.Font = new System.Drawing.Font("Maiandra GD", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreEmpresa.ForeColor = System.Drawing.Color.Lime;
            this.lblNombreEmpresa.Location = new System.Drawing.Point(3, 8);
            this.lblNombreEmpresa.Name = "lblNombreEmpresa";
            this.lblNombreEmpresa.Size = new System.Drawing.Size(866, 33);
            this.lblNombreEmpresa.TabIndex = 22;
            this.lblNombreEmpresa.Text = "PERSONAL INGRESADO";
            // 
            // btnAgregarEmpleado
            // 
            this.btnAgregarEmpleado.BackColor = System.Drawing.Color.Yellow;
            this.btnAgregarEmpleado.Font = new System.Drawing.Font("Maiandra GD", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarEmpleado.Image = global::Palatium.Properties.Resources.icono_boton_empleado;
            this.btnAgregarEmpleado.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregarEmpleado.Location = new System.Drawing.Point(467, 618);
            this.btnAgregarEmpleado.Name = "btnAgregarEmpleado";
            this.btnAgregarEmpleado.Size = new System.Drawing.Size(168, 71);
            this.btnAgregarEmpleado.TabIndex = 121;
            this.btnAgregarEmpleado.Text = "Agregar\r\nEmpleado";
            this.btnAgregarEmpleado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAgregarEmpleado.UseVisualStyleBackColor = false;
            this.btnAgregarEmpleado.Click += new System.EventHandler(this.btnAgregarEmpleado_Click);
            // 
            // btnAgregarEmpresa
            // 
            this.btnAgregarEmpresa.BackColor = System.Drawing.Color.Yellow;
            this.btnAgregarEmpresa.Font = new System.Drawing.Font("Maiandra GD", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarEmpresa.Image = global::Palatium.Properties.Resources.icono_boton_empresa;
            this.btnAgregarEmpresa.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregarEmpresa.Location = new System.Drawing.Point(12, 618);
            this.btnAgregarEmpresa.Name = "btnAgregarEmpresa";
            this.btnAgregarEmpresa.Size = new System.Drawing.Size(168, 71);
            this.btnAgregarEmpresa.TabIndex = 120;
            this.btnAgregarEmpresa.Text = "Agregar\r\nEmpresas";
            this.btnAgregarEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAgregarEmpresa.UseVisualStyleBackColor = false;
            this.btnAgregarEmpresa.Click += new System.EventHandler(this.btnAgregarEmpresa_Click);
            // 
            // pnlEmpleados
            // 
            this.pnlEmpleados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pnlEmpleados.Location = new System.Drawing.Point(467, 72);
            this.pnlEmpleados.Name = "pnlEmpleados";
            this.pnlEmpleados.Size = new System.Drawing.Size(883, 540);
            this.pnlEmpleados.TabIndex = 119;
            // 
            // pnlEmpresa
            // 
            this.pnlEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pnlEmpresa.Location = new System.Drawing.Point(12, 72);
            this.pnlEmpresa.Name = "pnlEmpresa";
            this.pnlEmpresa.Size = new System.Drawing.Size(362, 540);
            this.pnlEmpresa.TabIndex = 118;
            // 
            // frmSeleccionEmpresaEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(1362, 741);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnSiguienteEmpleado);
            this.Controls.Add(this.btnAnteriorEmpleado);
            this.Controls.Add(this.btnSiguienteEmpresa);
            this.Controls.Add(this.btnAnteriorEmpresa);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnAgregarEmpleado);
            this.Controls.Add(this.btnAgregarEmpresa);
            this.Controls.Add(this.pnlEmpleados);
            this.Controls.Add(this.pnlEmpresa);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSeleccionEmpresaEmpleado";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seleccione la Empresa y el empleado";
            this.Load += new System.EventHandler(this.frmSeleccionEmpresaEmpleado_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSeleccionEmpresaEmpleado_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSiguienteEmpleado;
        private System.Windows.Forms.Button btnAnteriorEmpleado;
        private System.Windows.Forms.Button btnSiguienteEmpresa;
        private System.Windows.Forms.Button btnAnteriorEmpresa;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNombreEmpresa;
        private System.Windows.Forms.Button btnAgregarEmpleado;
        private System.Windows.Forms.Button btnAgregarEmpresa;
        private System.Windows.Forms.Panel pnlEmpleados;
        private System.Windows.Forms.Panel pnlEmpresa;
    }
}