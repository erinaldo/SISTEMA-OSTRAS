using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Inicio
{
    public partial class frmInicioPrograma : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        string sSql;

        DataTable dtConsulta;

        bool bRespuesta;

        public frmInicioPrograma()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA RELLENAR EL ARREGLO DE MAXIMOS
        private void llenarArregloMaximo()
        {
            Program.iIDMESA = 0;

            Program.sDatosMaximo[0] = Program.sNombreUsuario;
            Program.sDatosMaximo[1] = Environment.MachineName.ToString();
            Program.sDatosMaximo[2] = "A";
        }

        //INGRESAR EL CURSOR AL BOTON
        private void ingresaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.Black;
            btnProceso.BackColor = Color.LawnGreen;
        }

        //SALIR EL CURSOR DEL BOTON
        private void salidaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.White;
            btnProceso.BackColor = Color.Navy;
        }

        //FUNCION PARA EXTRAER LA PÁGINA WEB DEL FABRICANTE
        private void extraerContactos()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(contacto_fabricante, '0995610690') contacto_fabricante," + Environment.NewLine;
                sSql += "isnull(sitio_web_fabricante, 'www.aplicsis.nets') sitio_web_fabricante" + Environment.NewLine;
                sSql += "from pos_parametro" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        lblContacto.Text = "CONTACTO: " + dtConsulta.Rows[0]["contacto_fabricante"].ToString();
                        lblSitioWeb.Text = dtConsulta.Rows[0]["sitio_web_fabricante"].ToString();
                    }

                    else
                    {
                        lblContacto.Text = "CONTACTO: 0995610690";
                        lblSitioWeb.Text = "www.aplicsis.net";
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCION:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void btnEntradaCajero_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnEntradaCajero);
        }

        private void btnEntradaCajero_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnEntradaCajero);
        }

        private void btnOficina_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnOficina);
        }

        private void btnOficina_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnOficina);
        }

        private void btnAperturarCaja_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnAperturarCaja);
        }

        private void btnAperturarCaja_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnAperturarCaja);
        }

        private void btnEntradaCajero_Click(object sender, EventArgs e)
        {
            Inicio.frmIniciarSesion sesion = new frmIniciarSesion();
            Inicio.frmMenuTab tab = new Inicio.frmMenuTab();
            sesion.ShowDialog(tab);

            if (sesion.DialogResult == DialogResult.OK)
            {
                sesion.Close();
                btnEntradaCajero.Visible = false;
                btnAperturarCaja.Visible = true;

                tab.cambioEstado(true);

                //IForm MiInterface = this.MdiParent.MdiChildren.First(f => f.GetType() == typeof(frmMenuTab)) as IForm;

                ////Inicio.frmMenuTab tab = new frmMenuTab();

                //IForm formInterface = this.Owner as IForm;

                //if (formInterface != null)
                //{
                //    formInterface.cambioEstado(true);
                //}
            }
        }

        private void frmInicioPrograma_Load(object sender, EventArgs e)
        {

        }

        private void lblSitioWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lblSitioWeb.Text.Trim());
        }

        private void btnAcerca_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Oficina.frmSoporteTecnico soporte = new Oficina.frmSoporteTecnico();
            soporte.ShowDialog();
        }

        private void btnOficina_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnOficina);

            Menú.frmCodigoOficina acceso = new Menú.frmCodigoOficina();
            acceso.ShowDialog();

            if (acceso.DialogResult == DialogResult.OK)
            {
                Oficina.frmNuevoMenuConfiguracion menuOficina = new Oficina.frmNuevoMenuConfiguracion();
                menuOficina.ShowDialog();
            }
        }
    }
}
