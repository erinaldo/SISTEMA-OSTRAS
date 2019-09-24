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
    public partial class frmInicioComedores : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

        Clases.ClaseLimpiarArreglos limpiarArreglos = new Clases.ClaseLimpiarArreglos();

        string sSql;

        DataTable dtConsulta;

        bool bRespuesta;

        public frmInicioComedores()
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

        //FUNCION PARA RELLENAR EL ARREGLO DE MAXIMOS
        private void llenarArregloMaximo()
        {
            Program.iIDMESA = 0;

            Program.sDatosMaximo[0] = Program.sNombreUsuario;
            Program.sDatosMaximo[1] = Environment.MachineName.ToString();
            Program.sDatosMaximo[2] = "A";
        }

        #endregion

        private void btnClienteEmpresarial_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnClienteEmpresarial);
        }

        private void btnClienteEmpresarial_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnClienteEmpresarial);
        }

        private void btnVentaExpress_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnVentaExpress);
        }

        private void btnVentaExpress_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnVentaExpress);
        }

        private void btnTarjetaAlmuerzo_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnTarjetaAlmuerzo);
        }

        private void btnTarjetaAlmuerzo_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnTarjetaAlmuerzo);
        }

        private void btnCobroAlmuerzos_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCobroAlmuerzos);
        }

        private void btnCobroAlmuerzos_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCobroAlmuerzos);
        }

        private void btnDatosClientes_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnDatosClientes);
        }

        private void btnDatosClientes_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnDatosClientes);
        }

        private void btnRevisar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnRevisar);
        }

        private void btnRevisar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnRevisar);
        }

        private void btnCancelar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCancelar);
        }

        private void btnCancelar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCancelar);
        }

        private void btnMovimientoCaja_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnMovimientoCaja);
        }

        private void btnMovimientoCaja_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnMovimientoCaja);
        }

        private void btnSalidaCajero_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnSalidaCajero);
        }

        private void btnSalidaCajero_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnSalidaCajero);
        }

        private void btnClienteEmpresarial_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnClienteEmpresarial);
            Program.sIDPERSONA = (string)null;
            consultarDatos("12", "");

            Empresa.frmSeleccionEmpresaEmpleado seleccion = new Empresa.frmSeleccionEmpresaEmpleado(Program.iIdOrigenOrden);
            seleccion.ShowDialog();
        }

        private void btnVentaExpress_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnVentaExpress);

            consultarDatos("10", "");

            Comida_Rapida.frmComandaComidaRapida comanda = new Comida_Rapida.frmComandaComidaRapida(Program.iIdOrigenOrden, 0);
            comanda.ShowDialog();
        }

        private void btnTarjetaAlmuerzo_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnTarjetaAlmuerzo);

            consultarDatos("13", "");

            Comida_Rapida.frmComandaComidaRapida comanda = new Comida_Rapida.frmComandaComidaRapida(Program.iIdOrigenOrden, 1);
            comanda.ShowDialog();
        }

        private void btnCobroAlmuerzos_Click(object sender, EventArgs e)
        {
            ingresaBoton(btnCobroAlmuerzos);
            Pedidos.frmCobrarAlmuerzos almuerzo = new Pedidos.frmCobrarAlmuerzos();
            almuerzo.ShowDialog();
        }

        private void btnDatosClientes_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnDatosClientes);

            //Facturador.frmNuevoCliente personas = new Facturador.frmNuevoCliente("", false);
            Facturador.frmNuevoClienteRegistro personas = new Facturador.frmNuevoClienteRegistro(0);
            personas.ShowDialog();
        }

        private void btnRevisar_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnRevisar);

            Program.iModoDelivery = 0;
            Revisar.Revisar r = new Revisar.Revisar();
            r.ShowDialog();

            if (r.DialogResult == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnCancelar);

            if (Program.iPuedeCobrar == 1)
            {
                Program.iModoDelivery = 0;
                CancelarOrdenes c = new CancelarOrdenes();
                c.ShowDialog();
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }
        }

        private void btnMovimientoCaja_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();
            ingresaBoton(btnMovimientoCaja);

            if (Program.iPuedeCobrar == 1)
            {
                Oficina.frmMovimientosCaja movimiento = new Oficina.frmMovimientosCaja(0);
                movimiento.Owner = this;
                movimiento.ShowDialog();
            }

            else
            {
                ok.LblMensaje.Text = "No tiene permisos para ingresar en esta opción.";
                ok.ShowDialog();
            }
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

        private void frmInicioComedores_Load(object sender, EventArgs e)
        {
            if (Program.sLogo != "")
            {
                if (File.Exists(Program.sLogo))
                {
                    logo.Image = Image.FromFile(Program.sLogo);
                }
            }
        }
    }
}
