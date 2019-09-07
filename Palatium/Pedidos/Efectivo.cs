using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium
{
    
    public partial class Efectivo : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        string sSql;        
        string saldo, total;
        public string sNombrePago;
        public string sIdPago;

        float suma = 0.00f;

        int id_pago;        

        DataTable dtConsulta;   

        bool bRespuesta;
        
        public Decimal dbValorGrid;
        public Decimal dbValorIngresado;

        public Efectivo(string sIdPago_P, string saldo, string total, string nombre_pago)
        {
            this.sIdPago = sIdPago_P;
            this.saldo = string.Format("{0:0.00}", saldo);
            this.total = total;
            this.sNombrePago = nombre_pago;
            InitializeComponent();
            btnValorSugerido.Text = string.Format("{0:0.00}", this.saldo);
        }

        #region FUNCIONES DEL USUARIO

        public void cargarPrecios()
        {
            btnOp1.Text = cambio(btnValorSugerido.Text);
            btnOp2.Text = cambio(btnOp1.Text);
            btnOp3.Text = cambio(btnOp2.Text);
            btnOp4.Text = cambio(btnOp3.Text);
        }

        static string cambio(string a)
        {

            string resultadox;
            double x = Convert.ToDouble(a);

            int totalEntero = (int)x;

            if (x - totalEntero != 0)
            {
                resultadox = string.Format("{0:0.00}", +Math.Ceiling(x)).ToString();
            }

            else if (x % 10 == 0)
            {

                if (x >= 1 && x < 5)
                    resultadox = string.Format("{0:0.00}", (5)).ToString();
                else if (x >= 5 && x < 10)
                    resultadox = string.Format("{0:0.00}", (10)).ToString();
                else if (x >= 10 && x < 20)
                    resultadox = string.Format("{0:0.00}", (20)).ToString();
                else if (x >= 20 && x < 40)
                    resultadox = string.Format("{0:0.00}", (40)).ToString();
                else if (x >= 40 && x < 50)
                    resultadox = string.Format("{0:0.00}", (50)).ToString();
                else if (x >= 50 && x < 100)
                    resultadox = string.Format("{0:0.00}", (100)).ToString();
                else if (x >= 100 && x < 200)
                    resultadox = string.Format("{0:0.00}", (200)).ToString();
                else if (x >= 200 && x < 300)
                    resultadox = string.Format("{0:0.00}", (300)).ToString();
                else if (x >= 300 && x < 500)
                    resultadox = string.Format("{0:0.00}", (500)).ToString();
                else if (x >= 500 && x < 1000)
                    resultadox = string.Format("{0:0.00}", (1000)).ToString();
                else if (x >= 1000 && x < 5000)
                    resultadox = string.Format("{0:0.00}", (5000)).ToString();

                else
                {
                    resultadox = "0";

                }

            }
            else if (x % 10 != 0)
            {
                if (x % 5 == 0)
                {
                    resultadox = string.Format("{0:0.00}", +Math.Ceiling(x + 5)).ToString();
                }
                else
                {
                    double valor = x / 5;
                    int r = ((int)valor) + 1;
                    int multiploCinco = r * 5;
                    resultadox = string.Format("{0:0.00}", +multiploCinco).ToString();
                }

            }
            else
            {
                resultadox = "0";
            }
            
            return resultadox;
        }

        private void abrirPropina()
        {
            //try
            //{
            //    PagoTarjetas efe = Owner as PagoTarjetas;
            //    sSql = "";
            //    sSql += "select lee_propina" + Environment.NewLine;
            //    sSql += "from pos_tipo_forma_cobro" + Environment.NewLine;
            //    sSql += "where id_pos_tipo_forma_cobro = " + Convert.ToInt32(origen) + Environment.NewLine;
            //    sSql += "and estado = 'A'";

            //    dtConsulta = new DataTable();
            //    dtConsulta.Clear();

            //    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

            //    if (bRespuesta == true)
            //    {
            //        if (dtConsulta.Rows[0].ItemArray[0].ToString() == "1")
            //        {
            //            //ABRIR EL FORMULARIO DE LA PROPINA
            //            Propina p = new Propina();
            //            p.ShowDialog();

            //            if (p.DialogResult == DialogResult.OK)
            //            {
            //                efe.lblPropina.Text = Program.dPropinas.ToString("N2");
            //                this.Close();
            //            }
            //        }

            //        else
            //        {
            //            //CERRAR EL FORMULARIO
            //            this.DialogResult = DialogResult.OK;
            //            this.Close();
            //        }
            //    }

            //    else
            //    {
            //        ok.LblMensaje.Text = "Ocurrió un problema al realizar la transacción.";
            //        ok.ShowDialog();
            //    }
            //}

            //catch (Exception ex)
            //{
            //    catchMensaje.LblMensaje.Text = ex.ToString();
            //    catchMensaje.ShowDialog();
            //}
        }

        //FUNCION PARA CONCATENAR
        private void concatenarValores(string sValor)
        {
            try
            {
                if ((txt_valor.Text == "0") && (sValor == "0"))
                {
                    goto fin;
                }

                else if ((txt_valor.Text == "0") && (sValor != "0"))
                {
                    txt_valor.Clear();
                }

                if (txt_valor.Text.Trim().Contains('.') == true)
                {
                    int longi = txt_valor.Text.Trim().Length;
                    int band = 0, cont = 0;

                    for (int i = 0; i < longi; i++)
                    {
                        if (band == 1)
                            cont++;

                        if (txt_valor.Text.Substring(i, 1) == ".")
                            band = 1;
                    }

                    if (cont < 2)
                    {
                        txt_valor.Text = txt_valor.Text + sValor;
                    }
                }

                else
                {
                    txt_valor.Text = txt_valor.Text + sValor;
                }    
                    
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }

        fin: { }
        }

        #endregion

        private void button13_Click(object sender, EventArgs e)
        {
            txt_valor.Text = string.Format("{0:0.00}", btnValorSugerido.Text);

            if (Convert.ToDouble(txt_valor.Text) <= Convert.ToDouble(btnValorSugerido.Text))
            {
                dbValorGrid = Convert.ToDecimal(txt_valor.Text);
            }

            else
            {
                dbValorGrid = Convert.ToDecimal(btnValorSugerido.Text);
            }

            dbValorIngresado = Convert.ToDecimal(txt_valor.Text);
            abrirPropina();
            this.DialogResult = DialogResult.OK;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            txt_valor.Text = btnOp1.Text;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            txt_valor.Text = btnOp2.Text; cargarPrecios();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            txt_valor.Text = btnOp3.Text;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            txt_valor.Text = btnOp4.Text;

        }

        private void Efectivo_Load(object sender, EventArgs e)
        {
            cargarPrecios();
        }

        private void btn1dolar_Click(object sender, EventArgs e)
        {
            suma += 1.00f;
            txt_valor.Text = string.Format("{0:0.00}", +suma).ToString();
        }

        private void btn2dolar_Click(object sender, EventArgs e)
        {
            suma += 2.00f;
            txt_valor.Text = string.Format("{0:0.00}", +suma).ToString();
        }

        private void btn5dolar_Click(object sender, EventArgs e)
        {
            suma += 5.00f;
            txt_valor.Text = string.Format("{0:0.00}", +suma).ToString();
        }

        private void btn10dolar_Click(object sender, EventArgs e)
        {
            suma += 10.00f;
            txt_valor.Text = string.Format("{0:0.00}", +suma).ToString();
        }

        private void btn20dolar_Click(object sender, EventArgs e)
        {
            suma += 20.00f;
            txt_valor.Text = string.Format("{0:0.00}", +suma).ToString();
        }

        private void btn50dolar_Click(object sender, EventArgs e)
        {
            suma += 50.00f;
            txt_valor.Text = string.Format("{0:0.00}", +suma).ToString();
        }

        private void txt_valor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }
            
            bool IsDec = false;
            int nroDec = 0;

            for (int i = 0; i < txt_valor.Text.Length; i++)
            {
                if (txt_valor.Text[i] == '.')
                    IsDec = true;

                if (IsDec && nroDec++ >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;
        }

        private void btnPunto_Click(object sender, EventArgs e)
        {
            if (txt_valor.Text.Trim().Contains('.') == true)
            {
                
            }

            else if (txt_valor.Text == "")
            {
                txt_valor.Text = txt_valor.Text + "0" + btnPunto.Text;
            }

            else
            {
                txt_valor.Text = txt_valor.Text + btnPunto.Text;
            }
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string str;
            int loc;

            if (txt_valor.Text.Length > 0)
            {
                str = txt_valor.Text.Substring(txt_valor.Text.Length - 1);
                loc = txt_valor.Text.Length;
                txt_valor.Text = txt_valor.Text.Remove(loc - 1, 1);
                if (txt_valor.Text == "")
                {
                    suma = 0;
                }

                else
                {
                    suma = (float)Convert.ToDouble(txt_valor.Text);
                }
            }

            else
            {
                suma = 0;
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            concatenarValores(btn1.Text);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            concatenarValores(btn2.Text);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            concatenarValores(btn3.Text);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            concatenarValores(btn4.Text);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            concatenarValores(btn5.Text);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            concatenarValores(btn6.Text);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            concatenarValores(btn7.Text);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            concatenarValores(btn8.Text);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            concatenarValores(btn9.Text);
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            concatenarValores(btn0.Text);
        }

        private void Efectivo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (txt_valor.Text == "")
            {
                ok.LblMensaje.Text = "Ingrese valor.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }

            else
            {
                if (Convert.ToDouble(txt_valor.Text) <= Convert.ToDouble(btnValorSugerido.Text))
                {
                    dbValorGrid = Convert.ToDecimal(txt_valor.Text);
                }

                else
                {
                    dbValorGrid =  Convert.ToDecimal(btnValorSugerido.Text);
                }

                dbValorIngresado = Convert.ToDecimal(txt_valor.Text);
                abrirPropina();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
