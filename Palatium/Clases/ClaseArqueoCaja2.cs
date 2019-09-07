using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Palatium.Clases
{
    class ClaseArqueoCaja2
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
                
        DataTable dtConsulta;
        DataTable dtCuentas;

        bool bRespuesta;
        
        double dbTotalEntregado = 0;
        double dbTotalPendiente = 0;
        double dTotalPagadoP = 0;
        double dSumaCuentasCobrar;
        double dbPorcentajeIva;
        double dbPorcentajeServicio;

        string sSql;
        string sFecha;
        string sTexto;
        string sFechaApertura;
        string sHoraApertura;
        string sFechaCierre;
        string sHoraCierre;
        string sTextoDesglose = "";
        string sTextoDevuelta;
        string sIvaCobrado;

        //FUNCION PARA CONSULTAR FECHA Y HORA
        private bool consultarFechaHora()
        {
            try
            {
                sSql = "";
                sSql += "select fecha_apertura, hora_apertura, isnull(fecha_cierre, fecha_apertura) fecha_apertura," + Environment.NewLine;
                sSql += "isnull(hora_cierre, '') hora_cierre, porcentaje_iva, porcentaje_servicio" + Environment.NewLine;
                sSql += "from pos_cierre_cajero" + Environment.NewLine;
                sSql += "where fecha_apertura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sFechaApertura = Convert.ToDateTime(dtConsulta.Rows[0][0].ToString()).ToString("dd/MM/yyyy");
                        sHoraApertura = dtConsulta.Rows[0][1].ToString();
                        sFechaCierre = Convert.ToDateTime(dtConsulta.Rows[0][2].ToString()).ToString("dd/MM/yyyy");

                        if (dtConsulta.Rows[0][3].ToString() == "")
                        {
                            sHoraCierre = DateTime.Now.ToString("HH:mm:dd");
                        }

                        else
                        {
                            sHoraCierre = Convert.ToDateTime(dtConsulta.Rows[0][3].ToString()).ToString("HH:mm:ss");
                        }

                        dbPorcentajeIva = Convert.ToDouble(dtConsulta.Rows[0]["porcentaje_iva"]);
                        dbPorcentajeServicio = Convert.ToDouble(dtConsulta.Rows[0]["porcentaje_servicio"]);

                        return true;
                    }

                    else
                    {
                        return false;
                    }
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

        //FUNCION PARA EXTRAER EL RANGO DE COMANDAS HECHAS EN LA FECHA
        private void verRangoComandas()
        {
            try
            {
                sSql = "";
                sSql += "select numero_pedido from cv403_numero_cab_pedido" + Environment.NewLine;
                sSql += "where id_pedido = (select min(id_pedido)from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and estado = 'A')" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select numero_pedido from cv403_numero_cab_pedido" + Environment.NewLine;
                sSql += "where id_pedido = (select max(id_pedido)from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and estado = 'A')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 1)
                    {
                        sTexto = sTexto + "Tickets desde:  " + dtConsulta.Rows[0][0].ToString().PadLeft(8, ' ') + "  hasta: " + dtConsulta.Rows[0][0].ToString().PadLeft(7, ' ') + Environment.NewLine;
                    }

                    else if (dtConsulta.Rows.Count > 1)
                    {
                        sTexto = sTexto + "Tickets desde:  " + dtConsulta.Rows[0][0].ToString().PadLeft(8, ' ') + "  hasta: " + dtConsulta.Rows[1][0].ToString().PadLeft(7, ' ') + Environment.NewLine;
                    }
                }

                else
                {
                    sTexto = sTexto + "Tickets desde:  " + "".PadLeft(8, ' ') + " hasta: " + "".PadLeft(8, ' ') + Environment.NewLine;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA EXTRAER EL RANGO DE FACTURAS HECHAS EN LA FECHA
        private void verRangoFacturas()
        {
            try
            {
                sSql = "";
                sSql += "select numero_factura" + Environment.NewLine;
                sSql += "from cv403_numeros_facturas" + Environment.NewLine;
                sSql += "where id_factura =" + Environment.NewLine;
                sSql += "(select min(F.id_factura)" + Environment.NewLine;
                sSql += "from cv403_facturas F, cv403_facturas_pedidos FP," + Environment.NewLine;
                sSql += "cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and FP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and F.fecha_factura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and F.idtipocomprobante = 1" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and F.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A')" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select numero_factura" + Environment.NewLine;
                sSql += "from cv403_numeros_facturas" + Environment.NewLine;
                sSql += "where id_factura =" + Environment.NewLine;
                sSql += "(select max(F.id_factura)" + Environment.NewLine;
                sSql += "from cv403_facturas F, cv403_facturas_pedidos FP," + Environment.NewLine;
                sSql += "cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and FP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and F.fecha_factura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and F.idtipocomprobante = 1" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and F.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 1)
                    {                           
                        sTexto = sTexto + "Facturas desde: " + dtConsulta.Rows[0][0].ToString().PadLeft(8, ' ') + "  hasta: " + dtConsulta.Rows[0][0].ToString().PadLeft(7, ' ') + Environment.NewLine;
                    }

                    else if (dtConsulta.Rows.Count > 1)
                    {
                        sTexto = sTexto + "Facturas desde: " + dtConsulta.Rows[0][0].ToString().PadLeft(8, ' ') + "  hasta: " + dtConsulta.Rows[1][0].ToString().PadLeft(7, ' ') + Environment.NewLine;
                    }
                }

                else
                {
                    sTexto = sTexto + "Facturas desde:" + "".PadLeft(8, ' ') + "   hasta:" + "".PadLeft(8, ' ') + Environment.NewLine;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA EXTRAER EL RANGO DE NOTAS DE VENTA HECHAS EN LA FECHA
        private void verRangoNotasVenta()
        {
            try
            {
                sSql = "";
                sSql += "select numero_factura" + Environment.NewLine;
                sSql += "from cv403_numeros_facturas" + Environment.NewLine;
                sSql += "where id_factura =" + Environment.NewLine;
                sSql += "(select min(F.id_factura)" + Environment.NewLine;
                sSql += "from cv403_facturas F, cv403_facturas_pedidos FP," + Environment.NewLine;
                sSql += "cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and FP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and F.fecha_factura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and F.idtipocomprobante = 2" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and F.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A')" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select numero_factura" + Environment.NewLine;
                sSql += "from cv403_numeros_facturas" + Environment.NewLine;
                sSql += "where id_factura =" + Environment.NewLine;
                sSql += "(select max(F.id_factura)" + Environment.NewLine;
                sSql += "from cv403_facturas F, cv403_facturas_pedidos FP," + Environment.NewLine;
                sSql += "cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and FP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and F.fecha_factura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and F.idtipocomprobante = 2" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and F.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count != 0)
                    {
                        if (dtConsulta.Rows.Count == 1)
                        {
                            sTexto = sTexto + "N. Venta desde: " + dtConsulta.Rows[0][0].ToString().PadLeft(8, ' ') + "  hasta: " + dtConsulta.Rows[0][0].ToString().PadLeft(7, ' ') + Environment.NewLine;
                        }

                        else if (dtConsulta.Rows.Count > 1)
                        {
                            sTexto = sTexto + "N. Venta desde: " + dtConsulta.Rows[0][0].ToString().PadLeft(8, ' ') + "  hasta: " + dtConsulta.Rows[1][0].ToString().PadLeft(7, ' ') + Environment.NewLine;
                        }
                    }
                }

                else
                {
                    sTexto = sTexto + "N. Venta desde:" + "".PadLeft(8, ' ') + "   hasta:" + "".PadLeft(8, ' ') + Environment.NewLine;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //Función para llenar el informa de arqueo de caja
        public string llenarInforme(string sFecha)
        {
            try
            {
                this.sFecha = sFecha;

                if (consultarFechaHora() == false)
                {
                    sTexto = "";
                }

                else
                {
                    sTexto = "";
                    sTexto = sTexto + "ARQUEO DE CAJA".PadLeft(27, ' ') + Environment.NewLine;
                    sTexto = sTexto + "".PadRight(40, '=') + Environment.NewLine;
                    sTexto = sTexto + "Fecha:".PadRight(8, ' ') + sFechaApertura + " - " + sFechaCierre + Environment.NewLine;
                    sTexto = sTexto + "Desde las " + sHoraApertura + Environment.NewLine; //OBTENER DE POS_CIERRE_CAJERO
                    sTexto = sTexto + "Hasta las " + sHoraCierre + Environment.NewLine; //OBTENER DE POS_CIERRE_CAJERO
                    sTexto = sTexto + "Caja: <Todas>" + Environment.NewLine;
                    sTexto = sTexto + "Cajero: ".PadRight(8, ' ') + Program.sNombreCajero + Environment.NewLine + Environment.NewLine; //OBTENER DE POS_CIERRE_CAJERO

                    double dbTotalCobrado = (calcularTotalPago(1) + calcularTotalPago(2) + calcularTotalPagoTarjetas());
                    sTexto = sTexto + "Total Vendido.....:".PadRight(30, ' ') + dbTotalCobrado.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "Cobrado Efectivo..:".PadRight(30, ' ') + calcularTotalPago(1).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "Cobrado Tarjetas..:".PadRight(30, ' ') + calcularTotalPagoTarjetas().ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    llenarDesgloseTarjetas();
                    sTexto = sTexto + "Cobrado Cheques...:".PadRight(30, ' ') + calcularTotalPago(2).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "Total Cobrado.....:".PadRight(30, ' ') + (calcularTotalPago(1) + calcularTotalPago(2) + calcularTotalPagoTarjetas()).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    
                    //MOSTRAR VALORES EN CORTESIAS, VALES FUNCIONARIOS
                    //CONTAR LAS CORTESIA

                    sTextoDevuelta = extraerOtrosValores("04");

                    if (sTextoDevuelta != "")
                    {
                        sTexto = sTexto + "Ordenes Cortesias.:".PadRight(30, ' ') + sTextoDevuelta.PadLeft(10, ' ') + Environment.NewLine;
                    }

                    //VALES FUNCIONARIOS
                    sTextoDevuelta = extraerOtrosValores("05");

                    if (sTextoDevuelta != "")
                    {
                        sTexto = sTexto + "T. Funcionarios...:".PadRight(30, ' ') + sTextoDevuelta.PadLeft(10, ' ') + Environment.NewLine;
                    }

                    sTexto = sTexto + "Entradas Manuales.:".PadRight(30, ' ') + sumarEntradasManuales().ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "Salidas Manuales..:".PadRight(30, ' ') + sumarSalidasManuales().ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "".PadRight(25, ' ') + "-".PadRight(15, '-') + Environment.NewLine;
                    sTexto = sTexto + "Total Efectivo....:".PadRight(30, ' ') + ((calcularTotalPago(1) + sumarEntradasManuales()) - sumarSalidasManuales()).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "".PadRight(25, ' ') + "-".PadRight(15, '-') + Environment.NewLine;

                    sTexto = sTexto + "Saldo en Caja.....:".PadRight(30, ' ') + ((dbTotalCobrado + sumarEntradasManuales()) - sumarSalidasManuales()).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "".PadRight(25, ' ') + "-".PadRight(15, '-') + Environment.NewLine;
                    //sTexto = sTexto + "Cuentas por Cobrar:".PadRight(30, ' ') + sumarCuentasPorCobrar().ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    //sTexto = sTexto + "".PadRight(25, ' ') + "-".PadRight(15, '-') + Environment.NewLine;
                    sTexto = sTexto + "Total Entregado...:".PadRight(30, ' ') + dbTotalEntregado.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "".PadRight(25, ' ') + "-".PadRight(15, '-') + Environment.NewLine;
                    sTexto = sTexto + "Diferencia........:".PadRight(30, ' ') + (dbTotalEntregado - ((dbTotalCobrado + sumarEntradasManuales()) - sumarSalidasManuales())).ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    sTexto = sTexto + "".PadRight(25, ' ') + "-".PadRight(15, '-') + Environment.NewLine + Environment.NewLine;
                    sTexto = sTexto + "Total Pendiente...:".PadRight(30, ' ') + dbTotalPendiente.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;

                    #region Llenar Cortesías

                    sSql = "";
                    sSql += "select NP.nombre, PC.motivo_cortesia," + Environment.NewLine;
                    sSql += "ltrim(str(DP.precio_unitario, 10, 2)) precio_unitario," + Environment.NewLine;
                    sSql += "DP.cantidad, O.descripcion" + Environment.NewLine;
                    sSql += "from cv403_det_pedidos DP INNER JOIN" + Environment.NewLine;
                    sSql += "cv403_cab_pedidos CP ON DP.id_pedido = CP.id_pedido INNER JOIN" + Environment.NewLine;
                    sSql += "pos_origen_orden O ON CP.id_pos_origen_orden = O.id_pos_origen_orden INNER JOIN" + Environment.NewLine;
                    sSql += "cv401_nombre_productos NP ON DP.id_producto = NP.id_producto and NP.estado = 'A' INNER JOIN " + Environment.NewLine;
                    sSql += "pos_cortesia PC ON (DP.id_det_pedido = PC.id_det_pedido and PC.estado='A')" + Environment.NewLine;
                    sSql += "where CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                    sSql += "and DP.estado = 'A'" + Environment.NewLine;
                    sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada;
                    
                    dtConsulta = new DataTable();
                    dtConsulta.Clear();
                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                    double total = 0;
                    double dbTotal = 0;
                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtConsulta.Rows.Count; i++)
                            {
                                total = total + (Convert.ToDouble(dtConsulta.Rows[i][2].ToString()) * Convert.ToDouble(dtConsulta.Rows[i][3].ToString()));
                            }

                            sTexto = sTexto + "Total Items Cortesia: ".PadRight(30, ' ') + total.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;

                        }
                        else
                        {
                            sTexto = sTexto + "Total Cortesia: ".PadRight(30, ' ') + "0.00".PadLeft(10, ' ') + Environment.NewLine;
                        }
                    }

                    #endregion

                    sIvaCobrado = "0.00";
                    extraerIva();
                    //sTexto = sTexto + "I.V.A. Cobrado....:".PadRight(30, ' ') + (dbTotalCobrado - (dbTotalCobrado / (1 + (dbPorcentajeIva/100)))).ToString("N2").PadLeft(10, ' ') + Environment.NewLine + Environment.NewLine;
                    sTexto = sTexto + "I.V.A. Cobrado....:".PadRight(30, ' ') + sIvaCobrado.PadLeft(10, ' ') + Environment.NewLine + Environment.NewLine;
                    sTexto = sTexto + "Personas Atendidas:".PadRight(30, ' ') + calcularTotalPersonas(1).ToString().PadLeft(10, ' ') + Environment.NewLine;
                    verRangoComandas();
                    verRangoFacturas();
                    verRangoNotasVenta();
                    sTexto = sTexto + Environment.NewLine + Environment.NewLine + Environment.NewLine + ".";
                }

                return sTexto;

            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return "";
            }
        }


        //Función para calcular el total
        private double calcularTotalPago(int iFormaPago)
        {
            try
            {
                //sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select FP.descripcion,sum(FP.valor) valor" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_numero_cab_pedido NP, pos_vw_pedido_forma_pago FP" + Environment.NewLine;
                sSql += "where CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and CP.id_pedido = NP.id_pedido" + Environment.NewLine;
                sSql += "and CP.id_pedido = FP.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.id_pos_tipo_forma_cobro = " + iFormaPago + Environment.NewLine;
                sSql += "group by FP.descripcion";

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
                        return 0;
                }
                else
                    return 0;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Función para calcular el total de tarjetas
        private double calcularTotalPagoTarjetas()
        {
            try
            {
                double sumaTarjetas = 0;
                //sFecha = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select FP.descripcion,sum(FP.valor) valor" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_numero_cab_pedido NP, pos_vw_pedido_forma_pago FP" + Environment.NewLine;
                sSql += "where CP.fecha_pedido ='" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and CP.id_pedido = NP.id_pedido" + Environment.NewLine;
                sSql += "and CP.id_pedido = FP.id_pedido" + Environment.NewLine;
                sSql += "and FP.codigo in('TC', 'TD')" + Environment.NewLine;
                sSql += "group by FP.descripcion";

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sumaTarjetas += Convert.ToDouble(dtConsulta.Rows[i][1].ToString());
                        }
                        //MessageBox.Show(sumaTarjetas.ToString());
                        return sumaTarjetas;
                    }
                    else
                        return 0.00;
                }
                else
                    return 0.00;

            }
            catch (Exception)
            {
                ok.LblMensaje.Text = "Error al calcular el total de tarjetas";
                ok.ShowDialog();
                return 0.00;
            }
        }

        //Función para mostrar las tarjetas de crédito
        private void llenarDesgloseTarjetas()
        {
            try
            {
                sSql = "";
                sSql += "select FP.descripcion,sum(FP.valor) valor, count (CP.id_pedido) Numero" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_numero_cab_pedido NP,  pos_vw_pedido_forma_pago FP" + Environment.NewLine;
                sSql += "where CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pedido = NP.id_pedido" + Environment.NewLine;
                sSql += "and CP.id_pedido = FP.id_pedido and CP.id_pos_origen_orden in (1,2,3)" + Environment.NewLine;
                sSql += "and CP.estado_orden= 'Pagada'" + Environment.NewLine;
                sSql += "and FP.codigo in ('TC','TD')" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "group by FP.descripcion";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            string sNombreTarjeta = dtConsulta.Rows[i][0].ToString();

                            if (sNombreTarjeta.Length > 15)
                            {
                                sNombreTarjeta = sNombreTarjeta.Substring(0, 15);
                            }

                            double dbValorTarjeta = Convert.ToDouble(dtConsulta.Rows[i][1].ToString());
                            sTexto = sTexto + " ".PadRight(6, ' ') + sNombreTarjeta.PadRight(15, '.') + ":" + dbValorTarjeta.ToString("N2").PadLeft(15, ' ') + Environment.NewLine;
                        }
                    }
                    else
                    {
                        sTexto = sTexto + "No hay datos para ser mostrados" + Environment.NewLine;
                    }
                }
                else
                {
                    ok.LblMensaje.Text = "Error al mostrar las tarjetas de crédito";
                    ok.ShowDialog();
                }


            }
            catch (Exception)
            {
                ok.LblMensaje.Text = "Error al mostrar las tarjetas de crédito";
                ok.ShowDialog();
            }
        }

        //Función para calcular el total de personas que ocupan las mesas
        private double calcularTotalPersonas(int iIdPosOrigenOrden)
        {
            try
            {
                sSql = "";
                sSql += "select isnull(sum(CP.numero_personas),0) numero" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_numero_cab_pedido NP, pos_origen_orden ORI" + Environment.NewLine;
                sSql += "where CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and CP.id_pedido = NP.id_pedido" + Environment.NewLine;
                sSql += "and ORI.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and ORI.id_pos_origen_orden = " + iIdPosOrigenOrden + Environment.NewLine;
                sSql += "and CP.estado_orden = 'Pagada'";

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return Convert.ToDouble(dtConsulta.Rows[0][0].ToString());
                    }
                    else
                        return 0;
                }
                else
                    return 0;

            }
            catch (Exception)
            {
                ok.LblMensaje.Text = "Error al calcular el numero de personas";
                ok.ShowDialog();
                return 0;
            }
        }

        //FUNCION PARA SACAR EL TOTAL DE CUENTAS POR COBRAR
        private double sumarCuentasPorCobrar()
        {
            try
            {
                sSql = "";
                sSql += "select XC.id_pedido, CP.fecha_pedido" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar XC, cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where XC.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and XC.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and XC.cg_estado_dcto = 7460";

                dtCuentas = new DataTable();
                dtCuentas.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtCuentas, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return 0;
                }

                

                if (dtCuentas.Rows.Count > 0)
                {
                    dSumaCuentasCobrar = 0;

                    for (int i = 0; i < dtCuentas.Rows.Count; i++)
                    {
                        sSql = "";
                        sSql += "select ltrim(str(isnull(sum(cantidad * (precio_unitario + valor_iva + valor_otro - valor_dscto)), 0), 10, 2)) suma" + Environment.NewLine;
                        sSql += "from pos_vw_det_pedido " + Environment.NewLine;
                        sSql += "where id_pedido = " + Convert.ToInt32(dtCuentas.Rows[i]["id_pedido"]) + Environment.NewLine;

                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                        if (bRespuesta == true)
                        {
                            dSumaCuentasCobrar = dSumaCuentasCobrar + Convert.ToDouble(dtConsulta.Rows[0]["suma"]);
                        }

                        else
                        {
                            catchMensaje.LblMensaje.Text = sSql;
                            catchMensaje.ShowDialog();
                            dSumaCuentasCobrar = dSumaCuentasCobrar + 0;
                        }
                    }

                    return dSumaCuentasCobrar;
                }

                else
                {
                    return 0;
                }
            }

            catch(Exception)
            {
                ok.LblMensaje.Text = "Error al extraer los valores de las cuentas por cobrar.";
                ok.ShowDialog();
                return 0;
            }
        }

        //FUNCION PARA OBTENER EL VALOR DE LA SALIDAS MANUALES
        private double sumarSalidasManuales()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(sum(valor), 0) suma from pos_movimiento_caja  " + Environment.NewLine;
                sSql += "where estado = 'A' " + Environment.NewLine;
                sSql += "and tipo_movimiento = 0 " + Environment.NewLine;
                sSql += "and id_documento_pago is null" + Environment.NewLine;
                sSql += "and id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and fecha = '" + sFecha + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return Convert.ToDouble(dtConsulta.Rows[0][0].ToString());
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

        //FUNCION PARA OBTENER EL VALOR DE LA SALIDAS ENTRADAS
        private double sumarEntradasManuales()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(sum(valor), 0) suma from pos_movimiento_caja  " + Environment.NewLine;
                sSql += "where estado = 'A' " + Environment.NewLine;
                sSql += "and tipo_movimiento = 1 " + Environment.NewLine;
                sSql += "and id_documento_pago is null" + Environment.NewLine;
                sSql += "and id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and fecha = '" + sFecha + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return Convert.ToDouble(dtConsulta.Rows[0][0].ToString());
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


        public string extraerOtrosValores(string sCodigo)
        {
            try
            {
                sTextoDesglose = "";
                dTotalPagadoP = 0;

                sSql = "";
                sSql += "select isnull(sum(DP.cantidad * (DP.precio_unitario + DP.valor_iva - DP.valor_dscto)), 0) total" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_det_pedidos DP," + Environment.NewLine;
                sSql += "pos_origen_orden OO" + Environment.NewLine;
                sSql += "where OO.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and DP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and OO.codigo = '" + sCodigo + "'" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and DP.estado = 'A'" + Environment.NewLine;
                sSql += "and OO.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dTotalPagadoP = Convert.ToDouble(dtConsulta.Rows[0][0].ToString());

                        if (dTotalPagadoP != 0)
                        {
                            sTextoDesglose = dTotalPagadoP.ToString("N2");
                        }
                    }

                    else
                    {
                        return "";
                    }
                }

                else
                {
                    return "";
                }


                return sTextoDesglose;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return "";
            }
        }

        //FUNCION PARA EXTRAER EL IVA COBRADO
        private void extraerIva()
        {
            try
            {
                sSql = "";
                sSql += "select ltrim(str(sum(DP.cantidad * DP.valor_iva), 10, 2)) suma" + Environment.NewLine;
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
                        sIvaCobrado = dtConsulta.Rows[0][0].ToString();
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
    }
}
