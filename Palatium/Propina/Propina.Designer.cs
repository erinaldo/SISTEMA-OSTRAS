namespace Palatium
{
    partial class Propina
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
            this.txtValor = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnRetroceder = new DevComponents.DotNetBar.ButtonX();
            this.btnIngresar = new DevComponents.DotNetBar.ButtonX();
            this.btn0 = new DevComponents.DotNetBar.ButtonX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPunto = new DevComponents.DotNetBar.ButtonX();
            this.btnCancelar = new DevComponents.DotNetBar.ButtonX();
            this.btn3 = new DevComponents.DotNetBar.ButtonX();
            this.btn2 = new DevComponents.DotNetBar.ButtonX();
            this.btn1 = new DevComponents.DotNetBar.ButtonX();
            this.btn6 = new DevComponents.DotNetBar.ButtonX();
            this.btn5 = new DevComponents.DotNetBar.ButtonX();
            this.btn4 = new DevComponents.DotNetBar.ButtonX();
            this.btn9 = new DevComponents.DotNetBar.ButtonX();
            this.btn8 = new DevComponents.DotNetBar.ButtonX();
            this.btn7 = new DevComponents.DotNetBar.ButtonX();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtValor
            // 
            // 
            // 
            // 
            this.txtValor.Border.Class = "TextBoxBorder";
            this.txtValor.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Location = new System.Drawing.Point(6, 5);
            this.txtValor.MaxLength = 10;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(221, 38);
            this.txtValor.TabIndex = 28;
            this.txtValor.Text = "0";
            this.txtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            // 
            // btnRetroceder
            // 
            this.btnRetroceder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRetroceder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRetroceder.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btnRetroceder.Image = global::Palatium.Properties.Resources.borrar_caracteres;
            this.btnRetroceder.Location = new System.Drawing.Point(228, 3);
            this.btnRetroceder.Name = "btnRetroceder";
            this.btnRetroceder.Size = new System.Drawing.Size(73, 42);
            this.btnRetroceder.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnRetroceder.TabIndex = 25;
            this.btnRetroceder.Click += new System.EventHandler(this.btnRetroceder_Click);
            // 
            // btnIngresar
            // 
            this.btnIngresar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnIngresar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnIngresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.Image = global::Palatium.Properties.Resources.aceptar_digitos;
            this.btnIngresar.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnIngresar.Location = new System.Drawing.Point(228, 164);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(73, 117);
            this.btnIngresar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnIngresar.TabIndex = 27;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // btn0
            // 
            this.btn0.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn0.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn0.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn0.Location = new System.Drawing.Point(6, 223);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(147, 58);
            this.btn0.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn0.TabIndex = 26;
            this.btn0.Text = "0";
            this.btn0.Click += new System.EventHandler(this.btn0_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Teal;
            this.panel2.Controls.Add(this.btnPunto);
            this.panel2.Controls.Add(this.txtValor);
            this.panel2.Controls.Add(this.btnRetroceder);
            this.panel2.Controls.Add(this.btnIngresar);
            this.panel2.Controls.Add(this.btn0);
            this.panel2.Controls.Add(this.btnCancelar);
            this.panel2.Controls.Add(this.btn3);
            this.panel2.Controls.Add(this.btn2);
            this.panel2.Controls.Add(this.btn1);
            this.panel2.Controls.Add(this.btn6);
            this.panel2.Controls.Add(this.btn5);
            this.panel2.Controls.Add(this.btn4);
            this.panel2.Controls.Add(this.btn9);
            this.panel2.Controls.Add(this.btn8);
            this.panel2.Controls.Add(this.btn7);
            this.panel2.Location = new System.Drawing.Point(4, 6);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(309, 287);
            this.panel2.TabIndex = 10;
            // 
            // btnPunto
            // 
            this.btnPunto.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPunto.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPunto.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btnPunto.Location = new System.Drawing.Point(154, 223);
            this.btnPunto.Name = "btnPunto";
            this.btnPunto.Size = new System.Drawing.Size(73, 58);
            this.btnPunto.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPunto.TabIndex = 29;
            this.btnPunto.Text = ".";
            this.btnPunto.Click += new System.EventHandler(this.btnPunto_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancelar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = global::Palatium.Properties.Resources.cancelar_digitos;
            this.btnCancelar.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.btnCancelar.Location = new System.Drawing.Point(228, 46);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(73, 117);
            this.btnCancelar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancelar.TabIndex = 25;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btn3
            // 
            this.btn3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn3.Location = new System.Drawing.Point(154, 164);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(73, 58);
            this.btn3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn3.TabIndex = 24;
            this.btn3.Text = "3";
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn2
            // 
            this.btn2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn2.Location = new System.Drawing.Point(80, 164);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(73, 58);
            this.btn2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn2.TabIndex = 23;
            this.btn2.Text = "2";
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn1
            // 
            this.btn1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn1.Location = new System.Drawing.Point(6, 164);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(73, 58);
            this.btn1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn1.TabIndex = 22;
            this.btn1.Text = "1";
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn6
            // 
            this.btn6.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn6.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn6.Location = new System.Drawing.Point(154, 105);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(73, 58);
            this.btn6.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn6.TabIndex = 21;
            this.btn6.Text = "6";
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // btn5
            // 
            this.btn5.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn5.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn5.Location = new System.Drawing.Point(80, 105);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(73, 58);
            this.btn5.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn5.TabIndex = 20;
            this.btn5.Text = "5";
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // btn4
            // 
            this.btn4.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn4.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn4.Location = new System.Drawing.Point(6, 105);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(73, 58);
            this.btn4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn4.TabIndex = 19;
            this.btn4.Text = "4";
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn9
            // 
            this.btn9.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn9.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn9.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn9.Location = new System.Drawing.Point(154, 46);
            this.btn9.Name = "btn9";
            this.btn9.Size = new System.Drawing.Size(73, 58);
            this.btn9.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn9.TabIndex = 18;
            this.btn9.Text = "9";
            this.btn9.Click += new System.EventHandler(this.btn9_Click);
            // 
            // btn8
            // 
            this.btn8.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn8.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn8.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn8.Location = new System.Drawing.Point(80, 46);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(73, 58);
            this.btn8.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn8.TabIndex = 17;
            this.btn8.Text = "8";
            this.btn8.Click += new System.EventHandler(this.btn8_Click);
            // 
            // btn7
            // 
            this.btn7.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn7.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn7.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Bold);
            this.btn7.Location = new System.Drawing.Point(6, 46);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(73, 58);
            this.btn7.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn7.TabIndex = 16;
            this.btn7.Text = "7";
            this.btn7.Click += new System.EventHandler(this.btn7_Click);
            // 
            // Propina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(318, 298);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Propina";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Propina";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Propina_KeyDown);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtValor;
        private DevComponents.DotNetBar.ButtonX btnRetroceder;
        private DevComponents.DotNetBar.ButtonX btnIngresar;
        private DevComponents.DotNetBar.ButtonX btn0;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.ButtonX btnPunto;
        private DevComponents.DotNetBar.ButtonX btnCancelar;
        private DevComponents.DotNetBar.ButtonX btn3;
        private DevComponents.DotNetBar.ButtonX btn2;
        private DevComponents.DotNetBar.ButtonX btn1;
        private DevComponents.DotNetBar.ButtonX btn6;
        private DevComponents.DotNetBar.ButtonX btn5;
        private DevComponents.DotNetBar.ButtonX btn4;
        private DevComponents.DotNetBar.ButtonX btn9;
        private DevComponents.DotNetBar.ButtonX btn8;
        private DevComponents.DotNetBar.ButtonX btn7;
    }
}