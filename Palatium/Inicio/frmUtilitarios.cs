using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Inicio
{
    public partial class frmUtilitarios : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        Clases.ClaseLimpiarArreglos limpiarArreglos = new Clases.ClaseLimpiarArreglos();
        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();

        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

        string sSql;

        DataTable dtConsulta;

        bool bRespuesta;

        public frmUtilitarios()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

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

        //FUNCION PARA RELLENAR EL ARREGLO DE MAXIMOS
        private void llenarArregloMaximo()
        {
            Program.iIDMESA = 0;

            Program.sDatosMaximo[0] = Program.sNombreUsuario;
            Program.sDatosMaximo[1] = Environment.MachineName.ToString();
            Program.sDatosMaximo[2] = "A";
        }

        //CONSULTA ÀRA HABILITAR LAS OPCIONES
        private void consultarDatos(string sOpcion, string sAuxiliar)
        {
            try
            {
                Program.sIDPERSONA = null;
                Program.iIdPersonaFacturador = 0;
                Program.iIdentificacionFacturador = "";

                Program.iDomicilioEspeciales = 0;
                Program.iModoDelivery = 0;
                Program.iIDMESA = 0;

                Program.dbValorPorcentaje = 25;
                Program.dbDescuento = Program.dbValorPorcentaje / 100;

                limpiarArreglos.limpiarArregloComentarios();

                sSql = "";
                sSql += "select id_pos_origen_orden, descripcion, genera_factura," + Environment.NewLine;
                sSql += "id_persona, id_pos_modo_delivery, presenta_opcion_delivery," + Environment.NewLine;
                sSql += "codigo, maneja_servicio" + Environment.NewLine;
                sSql += "from pos_origen_orden where codigo = '" + sOpcion + "'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    Program.iIdOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    Program.sDescripcionOrigenOrden = dtConsulta.Rows[0][1].ToString();
                    Program.iGeneraFactura = Convert.ToInt32(dtConsulta.Rows[0][2].ToString());
                    Program.iManejaServicioOrden = Convert.ToInt32(dtConsulta.Rows[0][7].ToString());

                    if ((dtConsulta.Rows[0][3].ToString() == null) || (dtConsulta.Rows[0][3].ToString() == ""))
                    {
                        Program.iIdPersonaOrigenOrden = 0;
                    }

                    else
                    {
                        Program.iIdPersonaOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0][3].ToString());
                        Program.sIDPERSONA = dtConsulta.Rows[0][3].ToString();

                    }
                    Program.iIdPosModoDelivery = Convert.ToInt32(dtConsulta.Rows[0][4].ToString());
                    Program.iPresentaOpcionDelivery = Convert.ToInt32(dtConsulta.Rows[0][5].ToString());
                    Program.sCodigoAsignadoOrigenOrden = dtConsulta.Rows[0][6].ToString();

                    if (Program.iGeneraFactura == 0)
                    {
                        sSql = "";
                        sSql += "select id_pos_tipo_forma_cobro, descripcion" + Environment.NewLine;
                        sSql += "from pos_tipo_forma_cobro" + Environment.NewLine;
                        sSql += "where codigo = '" + sAuxiliar + "'";

                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                        if (bRespuesta == true)
                        {
                            if (dtConsulta.Rows.Count > 0)
                            {
                                Program.sIdGrid = dtConsulta.Rows[0][0].ToString();
                                Program.sFormaPagoGrid = dtConsulta.Rows[0][1].ToString();
                            }
                        }

                        else
                        {
                            ok.LblMensaje.Text = "Ocurrió un problema al realizar la consulta.";
                            ok.ShowDialog();
                        }
                    }

                }
                else
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al realizar la consulta.";
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void btnAbrirCajonDinero_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnAbrirCajonDinero);
        }

        private void btnAbrirCajonDinero_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnAbrirCajonDinero);
        }

        private void btnReimprimirFactura_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnReimprimirFactura);
        }

        private void btnReimprimirFactura_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnReimprimirFactura);
        }

        private void btnAnularFactura_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnAnularFactura);
        }

        private void btnAnularFactura_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnAnularFactura);
        }

        private void btnEditarFactura_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnEditarFactura);
        }

        private void btnEditarFactura_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnEditarFactura);
        }

        private void btnCambioCajero_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCambioCajero);
        }

        private void btnCambioCajero_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCambioCajero);
        }

        private void btnConsultarPrecios_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnConsultarPrecios);
        }

        private void btnConsultarPrecios_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnConsultarPrecios);
        }

        private void btnReabrirCaja_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnReabrirCaja);
        }

        private void btnReabrirCaja_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnReabrirCaja);
        }

        private void btnCambioOrigen_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCambioOrigen);
        }

        private void btnCambioOrigen_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCambioOrigen);
        }

        private void btnOficina_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnOficina);
        }

        private void btnOficina_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnOficina);
        }

        private void btnAbrirCajonDinero_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnAbrirCajonDinero);

            if (Program.iPuedeCobrar == 1)
            {
                Menú.frmCodigoOficina acceso = new Menú.frmCodigoOficina();
                acceso.ShowDialog();

                if (acceso.DialogResult == DialogResult.OK)
                {
                    abrir.consultarImpresoraAbrirCajon();
                }
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para utilizar en esta opción.";
                ok.ShowDialog();
            }
        }

        private void btnReimprimirFactura_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnReimprimirFactura);

            if (Program.iPuedeCobrar == 1)
            {
                Facturador.frmReimprimirFactura factura = new Facturador.frmReimprimirFactura();
                factura.ShowDialog();

                if (factura.DialogResult == DialogResult.OK)
                {
                    factura.Close();
                }
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }
        }

        private void btnAnularFactura_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnAnularFactura);

            if (Program.iPuedeCobrar == 1)
            {
                Facturador.frmBuscarTicket buscarOrden = new Facturador.frmBuscarTicket();
                buscarOrden.ShowDialog();

                if (buscarOrden.DialogResult == DialogResult.OK)
                {
                    buscarOrden.Close();
                }
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }
        }

        private void btnEditarFactura_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnEditarFactura);

            if (Program.iPuedeCobrar == 1)
            {
                Facturador.frmEditarDatosClienteFactura editar = new Facturador.frmEditarDatosClienteFactura();
                editar.ShowDialog();
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }
        }

        private void btnCambioCajero_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnCambioCajero);

            if (Program.iPuedeCobrar == 1)
            {
                Cajero.frmCambiarCajero cambiar = new Cajero.frmCambiarCajero();
                cambiar.ShowDialog();

                if (cambiar.DialogResult == DialogResult.OK)
                {
                    //etiqueta.crearEtiquetaUsuario();
                    frmVerMenu principal = (frmVerMenu)this.MdiParent;
                    principal.Text = Program.sEtiqueta;

                    cambiar.Close();
                }
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }  
        }

        private void btnConsultarPrecios_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnConsultarPrecios);

            Menú.frmConsultarPreciosProductos precios = new Menú.frmConsultarPreciosProductos();
            precios.ShowDialog();
        }

        private void btnReabrirCaja_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnReabrirCaja);

            if (Program.iPuedeCobrar == 1)
            {
                Menú.frmCodigoOficina oficina = new Menú.frmCodigoOficina();
                oficina.ShowDialog();

                if (oficina.DialogResult == DialogResult.OK)
                {
                    Pedidos.frmReabrirCaja caja = new Pedidos.frmReabrirCaja();
                    caja.ShowDialog();
                }
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }
        }

        private void btnCambioOrigen_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();

            if (Program.iPuedeCobrar == 1)
            {
                Pedidos.frmConvertirComanda convertir = new Pedidos.frmConvertirComanda();
                convertir.ShowDialog();
            }
            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }            
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

        private void frmUtilitarios_Load(object sender, EventArgs e)
        {
            if (Program.sLogo != "")
            {
                if (File.Exists(Program.sLogo))
                {
                    logo.Image = Image.FromFile(Program.sLogo);
                }
            }
        }

        private void btnSalidaCajero_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnSalidaCajero);
        }

        private void btnSalidaCajero_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnSalidaCajero);
        }

        private void btnSalidaCajero_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnSalidaCajero);

            if (Program.iPuedeCobrar == 1)
            {
                string sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where estado_orden in ('Abierta', 'Pre-Cuenta')" + Environment.NewLine;
                sSql += "and fecha_orden = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_pos_jornada = " + Program.iJORNADA;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al realizar la consulta.";
                    ok.ShowDialog();
                    return;
                }

                if (Convert.ToInt32(dtConsulta.Rows[0][0].ToString()) > 0)
                {
                    Cajero.frmResumenCaja caja = new Cajero.frmResumenCaja(0);
                    caja.ShowDialog();
                }

                else
                {
                    Cajero.frmResumenCaja caja = new Cajero.frmResumenCaja(1);
                    caja.ShowDialog();

                    if (caja.DialogResult == DialogResult.OK)
                    {
                        caja.Close();
                    }
                }
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }
        }
    }
}
