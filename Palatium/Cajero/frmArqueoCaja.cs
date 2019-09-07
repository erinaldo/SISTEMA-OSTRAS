using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Cajero
{
    public partial class frmResumenCaja : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();
        Clases_Factura_Electronica.ClaseEnviarMail correo = new Clases_Factura_Electronica.ClaseEnviarMail();
        
        Clases.ClaseCierreCajero arqueo = new Clases.ClaseCierreCajero();
        Clases.ClaseArqueoCaja2 arqueo2 = new Clases.ClaseArqueoCaja2();
        Clases.ClaseReporteVendido reporte = new Clases.ClaseReporteVendido();
        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();
                
        bool bRespuesta = false;
        bool bRespuestaEnvioMail;
        
        DataTable dtConsulta;
        string sFecha;
        double dTotal;
        double dSumaTarjetas = 0, dSumaCheques = 0, dSumaEfectivo = 0, dSumaTransferencias = 0;

        string sSql;
        string sFechaCierre;
        string sHoraCierre;
        string sCorreoEmisor;
        string sCorreoCopia1;
        string sCorreoCopia2;
        string sPalabraClave;
        string sSmtp;
        string sPuerto;
        string sManejaSSL;
        string sNombreComercial;
        string sRazonSocial;
        string sMensajeEnviar;
        string sFacturaActual;
        string sAsuntoMail;
        string sMensajeRetorno;
        string sTextoDesglose;

        int iPuedeGuardar;
        int iOp;
        int iJornada;

        double dTotalPagadoCortesiaP;
        double dTotalProductosCortesiaP;

        public frmResumenCaja(int iPuedeGuardar)
        {
            this.iPuedeGuardar = iPuedeGuardar;
            InitializeComponent();            
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CONSULTAR LOS DATOS DEL CORREO DEL EMISOR
        private void consultarDatosMail()
        {
            try
            {
                sSql = "";
                sSql += "select correo_que_envia, correo_con_copia_1," + Environment.NewLine;
                sSql += "correo_con_copia_2, correo_palabra_clave," + Environment.NewLine;
                sSql += "correo_smtp, correo_puerto, maneja_SSL" + Environment.NewLine;
                sSql += "from pos_correo_emisor" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sCorreoEmisor = dtConsulta.Rows[0][0].ToString();
                        sCorreoCopia1 = dtConsulta.Rows[0][1].ToString();
                        sCorreoCopia2 = dtConsulta.Rows[0][2].ToString();
                        sPalabraClave = dtConsulta.Rows[0][3].ToString();
                        sSmtp = dtConsulta.Rows[0][4].ToString();
                        sPuerto = dtConsulta.Rows[0][5].ToString();
                        sManejaSSL = dtConsulta.Rows[0][6].ToString();

                        consultarNombreComercial();
                    }

                    else
                    {
                        iOp = 0;
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA EXTRAER EL NOMBRE COMERCIAL
        private void consultarNombreComercial()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(nombrecomercial, '') nombrecomercial, razonsocial" + Environment.NewLine;
                sSql += "from sis_empresa" + Environment.NewLine;
                sSql += "where idempresa = " + Program.iIdEmpresa;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    sNombreComercial = dtConsulta.Rows[0][0].ToString();
                    sRazonSocial = dtConsulta.Rows[0][1].ToString();
                    iOp = 1;
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA ENVIAR EL CORREO ELECTRONICO
        private bool enviarMail()
        {
            try
            {
                sAsuntoMail = "IMPORTANTE: INFORME DE CIERRE DE CAJERO DE LA FECHA " + sFecha;

                sMensajeRetorno = crearMensajeEnvio();

                bRespuestaEnvioMail = correo.enviarCorreo(sSmtp, Convert.ToInt32(sPuerto), sCorreoEmisor,
                                      sPalabraClave, sCorreoEmisor, sCorreoEmisor,
                                      sCorreoCopia1, sCorreoCopia2, sAsuntoMail,
                                      "", sMensajeRetorno, Convert.ToInt32(sManejaSSL));

                if (bRespuestaEnvioMail == true)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return false;
            }
        }


        //FUNCION PARA CREAR EL CUERPO DEL MENSAJE
        private string crearMensajeEnvio()
        {
            try
            {
                sMensajeEnviar = "";

                sMensajeEnviar = sMensajeEnviar + "Estimado(a) " + sRazonSocial + ":" + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "Se procede a informar el cierre de cajero realizado en  la fecha " + sFecha + "." + Environment.NewLine + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + arqueo.llenarCierreCajero(sFecha) + Environment.NewLine + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + reporte.llenarReporteVentas(sFecha) + Environment.NewLine + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + arqueo2.llenarInforme(sFecha) + Environment.NewLine + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "Enviado por: " + Program.sNombreUsuario.ToUpper() + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "Fecha y Hora: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                return sMensajeEnviar;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return "";
            }
        }

        //==================================================================================================================================================================================


        //FUNCION PARA CONSULTAR EL ESTADO DE LA CAJA A ABRIR
        private void consultarEstadoCaja()
        {
            try
            {
                if (iOp == 1)
                {
                    sSql = "";
                    sSql += "select estado_cierre_cajero" + Environment.NewLine;
                    sSql += "from pos_cierre_cajero" + Environment.NewLine;
                    sSql += "where fecha_cierre = '" + sFecha + "'" + Environment.NewLine;
                    sSql += "and estado = 'A'" + Environment.NewLine;
                    sSql += "and id_jornada = " + iJornada;

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            if (dtConsulta.Rows[0][0].ToString() == "Cerrada")
                            {
                                btnEnviarInforme.Visible = true;
                            }

                            else
                            {
                                btnEnviarInforme.Visible = false;
                            }
                        }

                        else
                        {
                            btnEnviarInforme.Visible = false;
                        }
                    }

                    else
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                        btnEnviarInforme.Visible = false;
                    }
                }
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        public void extraerOtrosValoresCortesia(string sCodigo)
        {
            try
            {
                sTextoDesglose = "";
                dTotalPagadoCortesiaP = 0;

                sSql = "";
                sSql += "select isnull(sum(DP.cantidad * (DP.precio_unitario + DP.valor_otro + DP.valor_iva - DP.valor_dscto)), 0) total" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_det_pedidos DP," + Environment.NewLine;
                sSql += "pos_origen_orden OO" + Environment.NewLine;
                sSql += "where OO.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and DP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and OO.codigo = '" + sCodigo + "'" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and DP.estado = 'A'" + Environment.NewLine;
                sSql += "and OO.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Pagada'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + iJornada;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dTotalPagadoCortesiaP = Convert.ToDouble(dtConsulta.Rows[0][0].ToString());
                    }

                    else
                    {
                        dTotalPagadoCortesiaP = 0;
                    }
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //Función para cargar el grid de cheques y tarjetas
        private void cargarGridChequesTarjetas()
        {
            try
            {
                //sFecha = DateTime.Now.ToString("yyyy/MM/dd");
                dSumaCheques = 0;
                dSumaEfectivo = 0;
                dSumaTarjetas = 0;

                dgvCheques.Rows.Clear();
                dgvCheques.Refresh();

                dgvTarjetas.Rows.Clear();
                dgvTarjetas.Refresh();

                sSql = "";
                sSql += "select NP.numero_pedido, FP.descripcion, FP.valor,FP.codigo " + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_numero_cab_pedido NP, pos_vw_pedido_forma_pago FP " + Environment.NewLine;
                sSql += "where CP.fecha_pedido ='" + sFecha + "'" + Environment.NewLine;
                //sSql += "and CP.id_pos_jornada = " + Program.iJORNADA + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + iJornada + Environment.NewLine;
                sSql += "and CP.id_pedido = NP.id_pedido" + Environment.NewLine;
                sSql += "and CP.id_pedido = FP.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Pagada'" + Environment.NewLine;
                sSql += "order by FP.descripcion";

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {                        
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            if (dtConsulta.Rows[i][3].ToString() == "CH")
                            {
                                dTotal = Convert.ToDouble(dtConsulta.Rows[i][2].ToString());
                                dSumaCheques = dSumaCheques + dTotal;
                                //dgvCheques.Rows.Add(false, dtConsulta.Rows[i][0].ToString(), dtConsulta.Rows[i][1].ToString(), iTotal.ToString("N2"));
                                dgvCheques.Rows.Add(false, dtConsulta.Rows[i][0].ToString() + " - CHEQUE", dTotal.ToString("N2"));
                            }

                            else if (dtConsulta.Rows[i][3].ToString() == "EF")
                            {
                                dTotal = Convert.ToDouble(dtConsulta.Rows[i][2].ToString());
                                dSumaEfectivo = dSumaEfectivo + dTotal;
                                //dgvCheques.Rows.Add(false, dtConsulta.Rows[i][0].ToString(), dtConsulta.Rows[i][1].ToString(), iTotal.ToString("N2"));
                                //dgvCheques.Rows.Add(false, dtConsulta.Rows[i][1].ToString(), dTotal.ToString("N2"));
                            }

                            else if ((dtConsulta.Rows[i][3].ToString() == "TC") || (dtConsulta.Rows[i][3].ToString() == "TD"))
                            {
                                dTotal = Convert.ToDouble(dtConsulta.Rows[i][2].ToString());
                                dSumaTarjetas = dSumaTarjetas + dTotal;
                                //dgvTarjetas.Rows.Add(false, dtConsulta.Rows[i][0].ToString(), dtConsulta.Rows[i][1].ToString(), iTotal.ToString("N2"));
                                dgvTarjetas.Rows.Add(false, dtConsulta.Rows[i][1].ToString(), dTotal.ToString("N2"));
                            }

                            else if (dtConsulta.Rows[i][3].ToString() == "TR")
                            {
                                dTotal = Convert.ToDouble(dtConsulta.Rows[i][2].ToString());
                                dSumaTransferencias = dSumaTransferencias + dTotal;
                            }
                        }

                        dgvCheques.ClearSelection();
                        dgvTarjetas.ClearSelection();

                        txtCobradoEfectivo.Text = dSumaEfectivo.ToString("N2");
                        txtCobradoTarjetas.Text = dSumaTarjetas.ToString("N2");
                        txtCobradoCheques.Text = dSumaCheques.ToString("N2");
                        //AQUI CAJA DE TEXTO DE TRANSFERENCIAS
                    }

                    else
                    {
                        dgvCheques.Rows.Clear();
                        dgvCheques.Refresh();

                        dgvTarjetas.Rows.Clear();
                        dgvTarjetas.Refresh();

                        txtCobradoEfectivo.Text = dSumaEfectivo.ToString("N2");
                        txtCobradoTarjetas.Text = dSumaTarjetas.ToString("N2");
                        txtCobradoCheques.Text = dSumaCheques.ToString("N2");
                    }
                               
                    txtTotalVendido.Text = (dSumaCheques + dSumaTarjetas + dSumaEfectivo).ToString("N2");
                }

            }
            catch (Exception)
            {
                ok.LblMensaje.Text = "Error al cargar las tajertas y cheques.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }
        }

        //FUNCION PARA MOSTRAR LAS ENTRADAS Y SALIDAS 
        private void sumaEntradasSalidas(int op)
        {
            try
            {
                //sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select isnull(sum(valor),0) suma from pos_movimiento_caja" + Environment.NewLine;
                sSql += "where tipo_movimiento = " + op + Environment.NewLine;
                sSql += "and fecha = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_documento_pago is null" + Environment.NewLine;
                sSql += "and id_pos_jornada = " + iJornada + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dTotal = Convert.ToDouble(dtConsulta.Rows[0][0].ToString());

                        if (op == 1)
                        {
                            txtEntradasManuales.Text = dTotal.ToString("N2");
                        }

                        else
                        {
                            txtSalidasManuales.Text = dTotal.ToString("N2");
                        }
                    }

                    else
                    {
                        if (op == 1)
                        {
                            txtEntradasManuales.Text = "0.00";
                        }

                        else
                        {
                            txtSalidasManuales.Text = "0.00";
                        }
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA OBTENER EL VALOR TOTAL DE CORTESIAS
        private void sumarProductosCortesia()
        {
            try
            {
                sSql = "";
                sSql += "select NP.nombre, PC.motivo_cortesia," + Environment.NewLine;
                sSql += "ltrim(str(DP.precio_unitario, 10, 2)) precio_unitario," + Environment.NewLine;
                sSql += "DP.cantidad, O.descripcion" + Environment.NewLine;
                sSql += "from cv403_det_pedidos DP INNER JOIN" + Environment.NewLine;
                sSql += "cv403_cab_pedidos CP ON DP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and DP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "pos_origen_orden O ON CP.id_pos_origen_orden = O.id_pos_origen_orden INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON DP.id_producto = NP.id_producto and NP.estado = 'A' INNER JOIN " + Environment.NewLine;
                sSql += "pos_cortesia PC ON (DP.id_det_pedido = PC.id_det_pedido and PC.estado='A')" + Environment.NewLine;
                sSql += "where CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Pagada'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + iJornada; 

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    double suma = 0;
                    if (dtConsulta.Rows.Count != 0)
                    {
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            //suma = suma + Convert.ToDouble(dtConsulta.Rows[i][2].ToString()) * (1 + Program.iva + Program.recargo);
                            suma = suma + Convert.ToDouble(dtConsulta.Rows[i][2].ToString());
                        }

                        dTotalProductosCortesiaP = suma;
                    }

                    else
                    {
                        dTotalProductosCortesiaP = 0;
                    }


                }
                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //Función para calcular el total de personas que ocupan las mesas
        private void calcularTotalPersonas()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(sum(CP.numero_personas),0) numero " + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_numero_cab_pedido NP, pos_origen_orden ORI " + Environment.NewLine;
                sSql += "where CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + iJornada + Environment.NewLine;
                sSql += "and CP.id_pedido = NP.id_pedido " + Environment.NewLine;
                sSql += "and ORI.id_pos_origen_orden = CP.id_pos_origen_orden " + Environment.NewLine;
                sSql += "and ORI.id_pos_origen_orden = 1" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and ORI.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Pagada'";

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtTotalPersonas.Text = dtConsulta.Rows[0][0].ToString();
                    }
                    else
                    {
                        txtTotalPersonas.Text = "0";
                    }
                }
                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowInTaskbar = false;
                    catchMensaje.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //Función para calcular el número de órdenes (Mesa, Llevar, Domicilio, Consumo, etc)
        private int calcularNumeroOrdenes(int iIdPosOrigenOrden)
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_pos_origen_orden = " + iIdPosOrigenOrden + Environment.NewLine;
                sSql += "and id_pos_jornada = " + iJornada + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and estado_orden in ('Pagada', 'Cerrada')";

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return 0;
            }
        }


        //Función para calcular el total de descuentos
        private void calcularDescuentos()
        {
            try
            {
                //sSql = "select isnull(sum(DP.cantidad  * DP.valor_dscto),0) valor_descuento from cv403_cab_pedidos CP, cv403_det_pedidos DP " +
                //                "where CP.id_pedido = DP.id_pedido " +
                //                "and CP.fecha_pedido ='" + sFecha + "' " +
                //                "and CP.id_pos_jornada = " + Program.iJORNADA +
                //                " and DP.estado='A' " +
                //                "and CP.estado='A'";

                sSql = "";
                sSql += "select ltrim(str(isnull(DP.cantidad,0), 10, 2)) cantidad, ltrim(str(isnull(DP.valor_dscto,0), 10, 2)) valor_dscto" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_det_pedidos DP" + Environment.NewLine;
                sSql += "where CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                sSql += "and CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + iJornada + Environment.NewLine;
                sSql += "and DP.estado='A'" + Environment.NewLine;
                sSql += "and CP.estado='A'" + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Pagada'" + Environment.NewLine;
                sSql += "and DP.valor_dscto <> 0";


                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dTotal = 0;

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            dTotal = dTotal + (Convert.ToDouble(dtConsulta.Rows[i][0].ToString()) * Convert.ToDouble(dtConsulta.Rows[i][1].ToString()));
                        }

                        txtTotalDescuentos.Text = dTotal.ToString("N2");
                    }
                    else
                    {
                        txtTotalDescuentos.Text = "0.00";
                    }
                }
                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //Función para calcular el total de orden (Mesa, Llevar, Domicilio, Consumo, etc)
        private double calcularTotalOrigenOrden(int iIdPosOrigenOrden)
        {
            try
            {
                sSql = "";
                sSql += "select ORI.descripcion,sum(FP.valor) valor" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_numero_cab_pedido NP," + Environment.NewLine;
                sSql += "pos_origen_orden ORI, pos_vw_pedido_forma_pago FP" + Environment.NewLine;
                sSql += "where CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + iJornada + Environment.NewLine;
                sSql += "and CP.id_pedido = NP.id_pedido" + Environment.NewLine;
                sSql += "and CP.id_pedido = FP.id_pedido" + Environment.NewLine;
                sSql += "and ORI.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and ORI.id_pos_origen_orden = " + iIdPosOrigenOrden + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and ORI.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado_orden in ('Pagada', 'Cerrada')" + Environment.NewLine;
                sSql += "group by ORI.descripcion";

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return Convert.ToDouble(dtConsulta.Rows[0][1].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return 0;
            }
        }

        //FUNCION PARA EXTRAER EL IVA COBRADO
        private void extraerIva()
        {
            try
            {
                sSql = "";
                sSql += "select ltrim(str(isnull(ltrim(str(sum(DP.cantidad * DP.valor_iva), 10, 2)), 0), 10, 2)) suma" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                sSql += "cv403_det_pedidos DP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and DP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "pos_origen_orden O ON O.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where O.genera_factura = 1" + Environment.NewLine;
                sSql += "and CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Pagada'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtImpuestoIVA.Text = dtConsulta.Rows[0][0].ToString();
                    }

                    else
                    {
                        catchMensaje.LblMensaje.Text = "No se pudo extraer el total del IVA cobrado." + Environment.NewLine + "Comuníquese con el administrador del sistema.";
                        catchMensaje.ShowDialog();
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA EXTRAER LAA CUENTAS POR COBRAR
        private void cargarCuentasPorCobrar()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(ltrim(str(sum(DP.cantidad * (DP.precio_unitario + DP.valor_iva - DP.valor_otro - DP.valor_dscto)), 10, 2)), 0.00) valor" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                sSql += "cv403_det_pedidos DP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and DP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "cv403_dctos_por_cobrar XC ON CP.id_pedido = XC.id_pedido" + Environment.NewLine;
                sSql += "and XC.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "tp_personas TP ON TP.id_persona = CP.id_persona" + Environment.NewLine;
                sSql += "and TP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "pos_origen_orden O ON O.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "where XC.cg_estado_dcto = 7460" + Environment.NewLine;
                sSql += "and O.cuenta_por_cobrar = 1" + Environment.NewLine;
                sSql += "and CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Cerrada'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + iJornada + Environment.NewLine;
                sSql += "and CP.id_localidad = " + Program.iIdLocalidad;
                
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                
                if (bRespuesta == true)
                {
                    txtCuentasPorCobrar.Text = dtConsulta.Rows[0][0].ToString();
                }
                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CARGAR LOS PARAMETROS
        private void cargarValores()
        {       
            dSumaTarjetas = 0;
            dSumaCheques = 0; 
            dSumaEfectivo = 0;
            dSumaTransferencias = 0;
            cargarGridChequesTarjetas();
            sumaEntradasSalidas(1);
            sumaEntradasSalidas(0);
            sumarProductosCortesia();
            extraerOtrosValoresCortesia("04");
            calcularTotalPersonas();
            calcularDescuentos();
            txtParaMesa.Text = calcularNumeroOrdenes(1).ToString();
            txtParaLlevar.Text = calcularNumeroOrdenes(2).ToString();
            txtParaDomicilio.Text = calcularNumeroOrdenes(3).ToString();
            txtTotalParaMesa.Text = calcularTotalOrigenOrden(1).ToString("N2");
            txtTotalParaLlevar.Text = calcularTotalOrigenOrden(2).ToString("N2");
            txtTotalParaDomicilio.Text = calcularTotalOrigenOrden(3).ToString("N2");
            txtTotalCortesias.Text = dTotalPagadoCortesiaP.ToString("N2");

            extraerIva();
            txtTotalCaja.Text = (Convert.ToDouble(txtTotalVendido.Text) + Convert.ToDouble(txtEntradasManuales.Text) - Convert.ToDouble(txtSalidasManuales.Text)).ToString("N2");
            txtCuentasPorCobrar.Text = "-" + txtTotalCaja.Text;
            txtTotalEfectivo.Text = (Convert.ToDouble(txtCobradoEfectivo.Text) - Convert.ToDouble(txtSalidasManuales.Text) + Convert.ToDouble(txtEntradasManuales.Text)).ToString("N2");
        }

        #endregion

        private void frmArqueoCaja_Load(object sender, EventArgs e)
        {
            iJornada = Program.iJORNADA;
            Program.iJornadaRecuperada = Program.iJORNADA;
            sFecha = DateTime.Now.ToString("yyyy/MM/dd");
            lblFechaCaja.Text = sFecha;
            consultarEstadoCaja();
            cargarValores();
            consultarDatosMail();

            if (Program.iVerCaja == 0)
            {
                BtnGuardar.Visible = false;                
            }

            if (Program.iUsarReceta == 1)
            {
                btnListarMateriaPrima.Visible = true;
            }

            else
            {
                btnListarMateriaPrima.Visible = false;
            }
        }

        private void btnSalidas_Click(object sender, EventArgs e)
        {
            //Oficina.frmMovimientosCaja movimiento = new Oficina.frmMovimientosCaja(2);
            //movimiento.ShowInTaskbar = false;
            //movimiento.ShowDialog();
            //sumaEntradasSalidas(0);

            Oficina.frmEntradasSalidasManuales movimiento = new Oficina.frmEntradasSalidasManuales(0, sFecha);
            movimiento.ShowDialog();
        }

        private void btnEntradas_Click(object sender, EventArgs e)
        {
            //Oficina.frmMovimientosCaja movimiento = new Oficina.frmMovimientosCaja(1);
            //movimiento.ShowInTaskbar = false;
            //movimiento.ShowDialog();
            //sumaEntradasSalidas(1);
            Oficina.frmEntradasSalidasManuales movimiento = new Oficina.frmEntradasSalidasManuales(1, sFecha);
            movimiento.ShowDialog();
        }

        private void btnReporteVendido_Click(object sender, EventArgs e)
        {
            ReportesTextBox.frmReporteVendido vendido = new ReportesTextBox.frmReporteVendido(sFecha, 1);
            vendido.ShowDialog();

            //Pedidos.frmVerCierreCajero arqueo = new Pedidos.frmVerCierreCajero(0, sFecha);
            //arqueo.ShowInTaskbar = false;
            //arqueo.ShowDialog();
        }

        private void TimerHora_Tick(object sender, EventArgs e)
        {
            LblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void BtnAbrirCaja_Click(object sender, EventArgs e)
        {
            if (Program.iManejaJornada == 1)
            {
                Program.iMostrarJornada = 1;
            }

            Menú.frmCodigoAdministrador codigo = new Menú.frmCodigoAdministrador();
            codigo.ShowDialog();

            if (codigo.DialogResult == DialogResult.OK)
            {
                codigo.Close();

                Pedidos.frmCalendario calendario = new Pedidos.frmCalendario(DateTime.Now.ToString("dd/MM/yyyy"));
                calendario.ShowDialog();

                if (calendario.DialogResult == DialogResult.OK)
                {
                    sFecha = Convert.ToDateTime(calendario.txtFecha.Text).ToString("yyyy/MM/dd");
                    lblFechaCaja.Text = sFecha;
                    iJornada = Program.iJornadaRecuperada;
                    cargarValores();
                    consultarEstadoCaja();
                }
            }            
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (iPuedeGuardar == 1)
                {
                    VentanasMensajes.frmMensajeAceptar mensaje = new VentanasMensajes.frmMensajeAceptar();
                    mensaje.LblMensaje.Text = "¿Está seguro que desea realizar el cierre de caja?\nUna vez guardado no podrá realizar modificaciones.";
                    mensaje.ShowDialog();

                    if (mensaje.DialogResult == DialogResult.OK)
                    {
                        //ACTUALIZAR EL REGISTRO PARA CERRAR EL CAJERO
                        //INICIAMOS UNA NUEVA TRANSACCION
                        //=======================================================================================================
                        //=======================================================================================================
                        if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                        {
                            ok.LblMensaje.Text = "Error al abrir transacción.";
                            ok.ShowDialog();
                            goto fin;
                        }
                        //=======================================================================================================

                        else
                        {
                            sFechaCierre = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            //sFechaCorta = DateTime.Now.ToString("yyyy/MM/ss HH:mm:ss");
                            sHoraCierre = sFechaCierre.Substring(11, 8);

                            sSql = "";
                            sSql += "update pos_cierre_cajero set" + Environment.NewLine;
                            sSql += "fecha_cierre = '" + sFechaCierre.Substring(0, 10) + "'," + Environment.NewLine;
                            sSql += "hora_cierre = '" + sHoraCierre + "'," + Environment.NewLine;
                            sSql += "estado_cierre_cajero = 'Cerrada' " + Environment.NewLine;
                            sSql += "where id_pos_cierre_cajero = " + Program.iIdPosCierreCajero;

                            if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                            {
                                catchMensaje.LblMensaje.Text = sSql;
                                catchMensaje.ShowInTaskbar = false;
                                catchMensaje.ShowDialog();
                                goto reversa;
                            }

                            conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                            ReportesTextBox.frmVerResumenCaja resumen = new ReportesTextBox.frmVerResumenCaja(1, sFecha);
                            resumen.ShowDialog();

                            ReportesTextBox.frmReporteVendido vendido = new ReportesTextBox.frmReporteVendido(sFecha, 1);
                            vendido.ShowDialog();

                            if (iOp == 1)
                            {
                                SiNo.LblMensaje.Text = "El cierre de caja se ha registrado con éxito.\n¿Desea enviar el informe al administrador?.";
                                SiNo.ShowDialog();

                                if (SiNo.DialogResult == DialogResult.OK)
                                {
                                    if (enviarMail() == true)
                                    {
                                        ok.LblMensaje.Text = "El informe de ventas se ha enviado con éxito.";
                                    }

                                    else
                                    {
                                        ok.LblMensaje.Text = "No se pudo enviar el informe al administrador.";

                                    }
                                }

                                this.DialogResult = DialogResult.OK;
                            }

                            else
                            {
                                ok.LblMensaje.Text = "El cierre de caja se ha registrado con éxito.";
                                ok.ShowDialog();

                                if (ok.DialogResult == DialogResult.OK)
                                {
                                    this.DialogResult = DialogResult.OK;
                                }
                            }

                            this.Close();
                            goto fin;
                        }
                    }
                }

                else
                {
                    ok.LblMensaje.Text = "No puede cerrar la caja, ya que aún existen órdenes abiertas. Favor cobrar las cuentas abiertas.";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                }
            }
            catch (Exception)
            {
                goto reversa;
            }

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
            }

        //=======================================================================================================
        fin:
            { }
        }

        private void btnVentasMesero_Click(object sender, EventArgs e)
        {
            Oficina.frmVentasPorMesero mesero = new Oficina.frmVentasPorMesero(sFecha);
            mesero.ShowDialog();
        }

        private void btnReimpresionTickets_Click(object sender, EventArgs e)
        {
            ReportesTextBox.frmVerResumenCaja vendido = new ReportesTextBox.frmVerResumenCaja(1, sFecha);
            vendido.ShowDialog();
        }

        private void BtnVistaArqueo_Click(object sender, EventArgs e)
        {
            Pedidos.frmVerCierreCajero arqueo = new Pedidos.frmVerCierreCajero(1, sFecha);
            arqueo.ShowDialog();
        }

        private void frmResumenCaja_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            
            if (Program.iPermitirAbrirCajon == 1)
            {
                if (e.KeyCode == Keys.F7)
                {
                    if (Program.iPuedeCobrar == 1)
                    {
                        abrir.consultarImpresoraAbrirCajon();
                    }
                }
            }
        }

        private void btnContarDinero_Click(object sender, EventArgs e)
        {
            Cajero.frmContarDinero contar = new frmContarDinero();
            //Cajero.frmRecuperarDinero contar = new frmRecuperarDinero();
            contar.ShowDialog();
        }

        private void btnEnviarInforme_Click(object sender, EventArgs e)
        {
            SiNo.LblMensaje.Text = "¿Desea reenviar el informe de cajero de la fecha " + sFecha + " ?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                if (enviarMail() == true)
                {
                    ok.LblMensaje.Text = "El informe de ventas se ha enviado con éxito.";
                }

                else
                {
                    ok.LblMensaje.Text = "No se pudo enviar el informe al administrador.";

                }

                this.Cursor = Cursors.Default;
                ok.ShowDialog();
            }
        }

        private void btnListarVentas_Click(object sender, EventArgs e)
        {
            ReportesTextBox.frmReporteVentasLista vendido = new ReportesTextBox.frmReporteVentasLista(sFecha, 1);
            vendido.ShowDialog();
        }

        private void btnListarMateriaPrima_Click(object sender, EventArgs e)
        {
            ReportesTextBox.frmVerReporteMateriaPrima vendido = new ReportesTextBox.frmVerReporteMateriaPrima(sFecha, 1);
            vendido.ShowDialog();
        }

        private void btnDetallarVentas_Click(object sender, EventArgs e)
        {
            Cajero.frmDetalleVentas detalle = new frmDetalleVentas(iJornada, sFecha);
            detalle.ShowDialog();
        }
    }
}
