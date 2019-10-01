﻿using System;
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

        Clases.ClaseValidarCaracteres caracter = new Clases.ClaseValidarCaracteres();

        string sSql;        
        string saldo, total;
        string sCodigo;
        string sFecha;
        string sLoteRecuperado;
        string sNumeroLoteRecibir;
        public string sNombrePago;
        public string sIdPago;
        public string sNumeroLote;

        float suma = 0.00f;

        int id_pago;
        int iBanderaInsertarLote;
        public int iConciliacion;

        DataTable dtConsulta;   

        bool bRespuesta;
        
        public Decimal dbValorGrid;
        public Decimal dbValorIngresado;
        public Decimal dbValorPropina;

        public Efectivo(string sIdPago_P, string saldo, string total, string nombre_pago, string sCodigo_P)
        {
            this.sIdPago = sIdPago_P;
            this.saldo = string.Format("{0:0.00}", saldo);
            this.total = total;
            this.sNombrePago = nombre_pago;
            this.sCodigo = sCodigo_P;
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

        //FUNCION PARA EXTRAER EL NUMERO DE LOTE
        private void numeroLote(string sCodigo_P)
        {
            try
            {
                sFecha = Program.sFechaSistema.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select NL.lote" + Environment.NewLine;
                sSql += "from pos_numero_lote NL INNER JOIN" + Environment.NewLine;
                sSql += "pos_operador_tarjeta OP ON OP.id_pos_operador_tarjeta = NL.id_pos_operador_tarjeta" + Environment.NewLine;
                sSql += "and NL.estado = 'A'" + Environment.NewLine;
                sSql += "and OP.estado = 'A'" + Environment.NewLine;
                sSql += "where NL.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and NL.estado_lote = 'Abierta'" + Environment.NewLine;
                sSql += "and NL.fecha_apertura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and OP.codigo = '" + sCodigo + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    sLoteRecuperado = "";
                    txtNumeroLote.Text = sLoteRecuperado;
                    iBanderaInsertarLote = 1;
                }

                else
                {
                    sLoteRecuperado = dtConsulta.Rows[0]["lote"].ToString().Trim();
                    txtNumeroLote.Text = sLoteRecuperado;
                    iBanderaInsertarLote = 0;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        #endregion


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

            if ((sCodigo == "TC") || (sCodigo == "TD"))
            {
                iConciliacion = 1;
                this.Size = new Size(535, 490);
                numeroLote("01");
            }

            else
            {
                iConciliacion = 0;
                this.Size = new Size(535, 383);
            }
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
                dbValorPropina = Convert.ToDecimal(txtPropina.Text.Trim());
                sNumeroLote = txtNumeroLote.Text.Trim();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void rdbDatafast_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDatafast.Checked == true)
            {
                rdbMedianet.CheckedChanged -= new EventHandler(rdbMedianet_CheckedChanged);
                rdbCredito.CheckedChanged -= new EventHandler(rdbCredito_CheckedChanged);
                rdbDebito.CheckedChanged -= new EventHandler(rdbDebito_CheckedChanged);
                rdbMedianet.Checked = false;
                rdbMedianet.CheckedChanged += new EventHandler(rdbMedianet_CheckedChanged);
                rdbCredito.CheckedChanged += new EventHandler(rdbCredito_CheckedChanged);
                rdbDebito.CheckedChanged += new EventHandler(rdbDebito_CheckedChanged);

                numeroLote("01");
            }
        }

        private void rdbMedianet_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMedianet.Checked == true)
            {
                rdbDatafast.CheckedChanged -= new EventHandler(rdbDatafast_CheckedChanged);
                rdbCredito.CheckedChanged -= new EventHandler(rdbCredito_CheckedChanged);
                rdbDebito.CheckedChanged -= new EventHandler(rdbDebito_CheckedChanged);
                rdbDatafast.Checked = false;
                rdbDatafast.CheckedChanged += new EventHandler(rdbDatafast_CheckedChanged);
                rdbCredito.CheckedChanged += new EventHandler(rdbCredito_CheckedChanged);
                rdbDebito.CheckedChanged += new EventHandler(rdbDebito_CheckedChanged);

                numeroLote("02");
            }
        }

        private void rdbCredito_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCredito.Checked == true)
            {
                rdbDebito.CheckedChanged -= new EventHandler(rdbDebito_CheckedChanged);
                rdbDatafast.CheckedChanged -= new EventHandler(rdbDatafast_CheckedChanged);
                rdbMedianet.CheckedChanged -= new EventHandler(rdbMedianet_CheckedChanged);
                rdbDebito.Checked = false;
                rdbDebito.CheckedChanged += new EventHandler(rdbDebito_CheckedChanged);
                rdbDatafast.CheckedChanged += new EventHandler(rdbDatafast_CheckedChanged);
                rdbMedianet.CheckedChanged += new EventHandler(rdbMedianet_CheckedChanged);
            }
        }

        private void rdbDebito_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDebito.Checked == true)
            {
                rdbCredito.CheckedChanged -= new EventHandler(rdbCredito_CheckedChanged);
                rdbDatafast.CheckedChanged -= new EventHandler(rdbDatafast_CheckedChanged);
                rdbMedianet.CheckedChanged -= new EventHandler(rdbMedianet_CheckedChanged);
                rdbCredito.Checked = false;
                rdbCredito.CheckedChanged += new EventHandler(rdbCredito_CheckedChanged);
                rdbDatafast.CheckedChanged += new EventHandler(rdbDatafast_CheckedChanged);
                rdbMedianet.CheckedChanged += new EventHandler(rdbMedianet_CheckedChanged);
            }
        }

        private void txtPropina_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter.soloDecimales(sender, e, 2);
        }

        private void btnValorSugerido_Click(object sender, EventArgs e)
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
            dbValorPropina = Convert.ToDecimal(txtPropina.Text.Trim());
            sNumeroLote = txtNumeroLote.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}
