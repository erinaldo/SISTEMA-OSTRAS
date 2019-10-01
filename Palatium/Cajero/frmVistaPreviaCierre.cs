using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Cajero
{
    public partial class frmVistaPreviaCierre : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        Clases.ClaseCrearImpresion imprimir = new Clases.ClaseCrearImpresion();
        Clases.ClaseReportes reporte = new Clases.ClaseReportes();
        Clases.ClaseReportesAdicionales reportes_2 = new Clases.ClaseReportesAdicionales();

        string sSql;
        string sCodigo;

        DataTable dtImprimir;
        DataTable dtConsulta;

        bool bRespuesta;

        string sFecha;
        string sRetorno;

        int iIdLocalidad;
        int iIdJornada;
        int iCerrar;
        int iCortarPapel;
        int iAbrirCajon;
        int iCantidadImpresiones;
        int iIdPosCierreCajero;

        //VARIABLES DE CONFIGURACION DE LA IMPRESORA
        string sNombreImpresora;
        string sPuertoImpresora;
        string sIpImpresora;
        string sDescripcionImpresora;

        Decimal dbAhorroEmergencia;
        Decimal dbCajaInicial;
        Decimal dbCajaFinal;

        public frmVistaPreviaCierre(string sFecha_P, int iIdLocalidad_P, int iIdJornada_P)
        {
            this.sFecha = sFecha_P;
            this.iIdLocalidad = iIdLocalidad_P;
            this.iIdJornada = iIdJornada_P;

            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //EXTRAER LOS DATOS LAS IMPRESORAS
        private void consultarImpresoraTipoOrden()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select path_url, numero_impresion, puerto_impresora," + Environment.NewLine;
                sSql = sSql + "ip_impresora, descripcion, cortar_papel, abrir_cajon" + Environment.NewLine;
                sSql = sSql + "from pos_impresora" + Environment.NewLine;
                sSql = sSql + "where id_pos_impresora = " + Program.iIdImpresoraReportes;

                dtImprimir = new DataTable();
                dtImprimir.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtImprimir, sSql);

                if (bRespuesta == true)
                {
                    if (dtImprimir.Rows.Count > 0)
                    {
                        sNombreImpresora = dtImprimir.Rows[0][0].ToString();
                        iCantidadImpresiones = Convert.ToInt16(dtImprimir.Rows[0][1].ToString());
                        sPuertoImpresora = dtImprimir.Rows[0][2].ToString();
                        sIpImpresora = dtImprimir.Rows[0][3].ToString();
                        sDescripcionImpresora = dtImprimir.Rows[0][4].ToString();
                        iCortarPapel = Convert.ToInt32(dtImprimir.Rows[0][5].ToString());
                        iAbrirCajon = Convert.ToInt32(dtImprimir.Rows[0][6].ToString());

                        //ENVIAR A IMPRIMIR
                        imprimir.iniciarImpresion();
                        imprimir.escritoEspaciadoCorto(txtReporte.Text);

                        if (iCortarPapel == 1)
                        {
                            imprimir.cortarPapel();
                        }

                        imprimir.imprimirReporte(sNombreImpresora);
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No existe el registro de configuración de impresora. Comuníquese con el administrador.";
                        ok.ShowDialog();
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

        //FUNCION PARA CREAR EL REPORTE
        private void mostrarReporte()
        {
            try
            {
                sRetorno = "";

                sSql = "";
                sSql += "select * from pos_vw_reportes_por_localidad" + Environment.NewLine;
                sSql += "where id_localidad = 1" + Environment.NewLine;
                sSql += "order by orden";

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
                    ok.LblMensaje.Text = "No se encuentra configurado el reporte de cierre.";
                    ok.ShowDialog();
                    return;
                }

                sRetorno += reporte.encabezadoReporte(sFecha, iIdLocalidad, iIdJornada);
                dbAhorroEmergencia = reporte.dbAhorroEmergencia;
                dbCajaInicial = reporte.dbCajaInicial;
                dbCajaFinal = reporte.dbCajaFinal;
                iIdPosCierreCajero = reporte.iIdPosCierreCajero;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    sCodigo = dtConsulta.Rows[i]["codigo"].ToString();

                    sRetorno += devolverReporte(sCodigo);
                }

                if (sRetorno == "")
                {
                    ok.LblMensaje.Text = "";
                }

                else
                {
                    txtReporte.Text = sRetorno + Environment.NewLine + Environment.NewLine + ".";

                    //if (Program.iVistaPreviaImpresiones == 1)
                    //{
                    //    consultarImpresoraTipoOrden();
                    //    this.Close();
                    //}

                    sRetorno = "";
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CARGAR LOS REPORTES
        private string devolverReporte(string sCodigo_P)
        {
            try
            {
                string sTexto = "";

                if (sCodigo_P == "01")
                {
                    sTexto = reporte.resumenSistema(sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "02")
                {
                    sTexto = reporte.pagosPrioritarios(sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "03")
                {
                    sTexto = reporte.productosDespachados(sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "04")
                {
                    sTexto = reporte.detalleVentasOrigen(sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "05")
                {
                    sTexto = reporte.reporteCantidadPagos(sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "06")
                {
                    sTexto = reporte.arqueoCaja(sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "07")
                {
                    sTexto = reporte.ventasMesero(sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "08")
                {
                    sTexto = reporte.llenarMovimientosAgrupados(1, sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "09")
                {
                    sTexto = reporte.llenarMovimientosAgrupados(0, sFecha, iIdLocalidad, iIdJornada);
                }

                else if (sCodigo_P == "10")
                {
                    sTexto = reporte.ahorroEmergencia(sFecha, iIdLocalidad, iIdJornada, dbAhorroEmergencia);
                }

                else if (sCodigo_P == "11")
                {
                    sTexto = reporte.contarMonedas(iIdPosCierreCajero, iIdLocalidad, iIdJornada, dbCajaInicial, dbCajaFinal);
                }

                else if (sCodigo_P == "12")
                {
                    sTexto = reportes_2.crearReporte(sFecha, iIdLocalidad, iIdJornada, iIdPosCierreCajero);
                }


                return sTexto;
            }

            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        #endregion

        private void frmVistaPreviaCierre_Load(object sender, EventArgs e)
        {
            mostrarReporte();
            this.ActiveControl = lblRecibir;
        }

        private void frmVistaPreviaCierre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void menuImprimir_Click(object sender, EventArgs e)
        {
            consultarImpresoraTipoOrden();
        }
    }
}
