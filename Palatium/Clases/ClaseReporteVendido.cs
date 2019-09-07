using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Palatium.Clases
{
    class ClaseReporteVendido
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        DataTable dtConsulta;
        DataTable dtPagosDesglose;
        string sSql;
        bool bRespuesta = false;
        double dSuma;
        string sTexto;
        string sFecha;

        string sNombreProducto;
        double dCantidad;
        double dTotal;

        double dbTotalOrdenes;
        double dbTotalTarjetas;
        double dbTotalEfectivo;
        double dbTotalCheques;
        double dbAuxiliar;
        double dTotalDescuentos;

        //******************************************************************
        string sNombreCliente;
        string sNumeroPedido;
        string sFechaPedido;
        double dbTotalPedido;
        int iIdPedido;
        string sTipoOrden;
        double dbNumeroFactura;
        string sIvaCobrado;

        string sDescripcion;
        double dbTotalPago;
        double dbCambio;
        string sCodigo;
        string sTextoPagos;

        DataTable dtPagos;
        DataTable dtTipoProducto;

        string sFechaApertura;
        string sHoraApertura;
        string sFechaCierre;
        string sHoraCierre;

        //------------------------------------------------------------------
        string sNumeroOrden_P;
        string sNumeroFactura_P;
        string sNombreTarjeta_P;
        string sTextoDesglose;
        string sCodigoCobro;
        string sTextoDevuelta;
        int iTipoComprobante;
        int iIdOrden_P;
        int iBandera;
        double dTotalPagadoP;
        double dEfectivoCheque_P;
        double dTarjeta_P;
        double dbPorcentajeIva;
        double dbPorcentajeServicio;

        Decimal dbSumarPorTipoProducto;
        //------------------------------------------------------------------
        //FUNCION PARAR REORGANIZAR LOS ITEMS PRODUCTOS
        private int consultarTipoProductos()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_tipo_producto, descripcion" + Environment.NewLine;
                sSql += "from pos_tipo_producto" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtTipoProducto = new DataTable();
                dtTipoProducto.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtTipoProducto, sSql);

                if (bRespuesta == true)
                {
                    if (dtTipoProducto.Rows.Count > 0)
                    {
                        return 1;
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    return 0;
                }
            }

            catch(Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowDialog();
                return 0;
            }
        }

        //******************************************************************

        private string calcularDescuentos()
        {
            try
            {
                sSql = "";
                sSql += "select ltrim(str(isnull(DP.cantidad,0), 10, 2)) cantidad," + Environment.NewLine;
                sSql += "ltrim(str(isnull(DP.valor_dscto,0), 10, 2)) valor_dscto" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP, cv403_det_pedidos DP" + Environment.NewLine;
                sSql += "where CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                sSql += "and CP.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CP.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and DP.estado='A'" + Environment.NewLine;
                sSql += "and CP.estado='A'" + Environment.NewLine;
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
                            dTotalDescuentos = dTotalDescuentos + (Convert.ToDouble(dtConsulta.Rows[i][0].ToString()) * Convert.ToDouble(dtConsulta.Rows[i][1].ToString()));
                        }

                       return dTotalDescuentos.ToString("N2");
                    }
                    else
                    {
                        return "0.00";
                    }
                }
                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return "";
                }

            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return "";
            }
        }

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
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                return false;
            }
        }
        
        public string llenarReporteVentas(string sFecha_P)
        {
            try
            {
                this.sFecha = sFecha_P;

                if (consultarFechaHora() == false)
                {
                    sTexto = "";
                }

                else
                {
                    dSuma = 0;

                    sTexto = "";
                    sTexto = sTexto + "*".PadRight(40, '*') + Environment.NewLine;
                    sTexto = sTexto + Program.local.PadLeft(30, ' ') + Environment.NewLine;
                    sTexto = sTexto + "*".PadRight(40, '*') + Environment.NewLine;
                    sTexto = sTexto + "REPORTE DE LO VENDIDO".PadLeft(30, ' ') + Environment.NewLine;
                    sTexto = sTexto + "FECHA: " + sFecha + Environment.NewLine;
                    sTexto = sTexto + "DESDE: " + sHoraApertura + Environment.NewLine;
                    sTexto = sTexto + "HASTA: " + sHoraCierre + Environment.NewLine;
                    
                    if (consultarTipoProductos() == 1)
                    {
                        sTexto = sTexto + llenarPorDetalle();
                    }

                    else
                    {
                        sTexto = sTexto + "".PadRight(40, '=') + Environment.NewLine;
                        sTexto = sTexto + "".PadRight(5, ' ') + "DESCRIPCION".PadRight(20, ' ') + "CANT.".PadLeft(5, ' ') + "TOTAL".PadLeft(10, ' ') + Environment.NewLine;
                        sTexto = sTexto + "".PadRight(40, '=') + Environment.NewLine;

                        sSql = "";
                        sSql += "select NOM.nombre, sum(DET.cantidad) CANTIDAD," + Environment.NewLine;
                        sSql += "sum(DET.cantidad *(((DET.precio_unitario + valor_iva+ valor_otro)-valor_dscto))) TOTAL" + Environment.NewLine;
                        sSql += "from cv403_det_pedidos DET inner join" + Environment.NewLine;
                        sSql += "cv401_nombre_productos NOM on DET.id_producto = NOM.id_producto" + Environment.NewLine;
                        sSql += "and DET.estado = 'A'" + Environment.NewLine;
                        sSql += "and NOM.estado = 'A' inner join" + Environment.NewLine;
                        sSql += "cv403_cab_pedidos CAB on CAB.id_pedido = DET.id_pedido" + Environment.NewLine;
                        sSql += "and CAB.estado = 'A'" + Environment.NewLine;
                        sSql += "where CAB.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;                        
                        sSql += "and CAB.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                        sSql += "and CAB.estado_orden in ('Pagada', 'Cerrada')" + Environment.NewLine;
                        sSql += "and CAB.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                        sSql += "group by NOM.nombre" + Environment.NewLine;
                        sSql += "order by sum(DET.cantidad)";

                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                        if (bRespuesta == true)
                        {
                            if (dtConsulta.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                                {
                                    sNombreProducto = dtConsulta.Rows[i][0].ToString();
                                    dCantidad = Convert.ToDouble(dtConsulta.Rows[i][1].ToString());
                                    dTotal = Convert.ToDouble(dtConsulta.Rows[i][2].ToString());
                                    dSuma = dSuma + dTotal;

                                    if (sNombreProducto.Length > 25)
                                    {
                                        sNombreProducto = sNombreProducto.Substring(0, 25);
                                    }

                                    sTexto = sTexto + sNombreProducto.PadRight(25, ' ') + dCantidad.ToString("N0").PadLeft(5, ' ') + dTotal.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                                }

                                sTexto = sTexto + "*".PadRight(40, '*') + Environment.NewLine + Environment.NewLine;
                                sTexto = sTexto + "TOTALES".PadRight(30, ' ') + dSuma.ToString("N2").PadLeft(10, ' ') + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                                sTexto = sTexto + Environment.NewLine + Environment.NewLine + Environment.NewLine;

                                sTexto = sTexto + reporteCantidadPagos();
                                sTexto = sTexto + Environment.NewLine + Environment.NewLine + Environment.NewLine + ".";
                            }

                            else
                            {
                                sTexto = "";
                            }
                        }

                        else
                        {
                            sTexto = "";
                        }
                    }
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


        //FUNCION PARA CLASIFICAR LOS ITEMS
        private string llenarPorDetalle()
        {
            string sContinuar = "";

            for (int i = 0; i < dtTipoProducto.Rows.Count; i++)
            {
                sSql = "";
                sSql += "select NOM.nombre, sum(DET.cantidad) CANTIDAD," + Environment.NewLine;
                sSql += "sum(DET.cantidad *(((DET.precio_unitario + valor_iva+ valor_otro)-valor_dscto))) TOTAL" + Environment.NewLine;
                sSql += "from cv403_det_pedidos DET inner join" + Environment.NewLine;
                sSql += "cv401_nombre_productos NOM on DET.id_producto = NOM.id_producto" + Environment.NewLine;
                sSql += "and NOM.estado = 'A' inner join" + Environment.NewLine;
                sSql += "cv403_cab_pedidos CAB on CAB.id_pedido = DET.id_pedido" + Environment.NewLine;
                sSql += "and CAB.estado = 'A'" + Environment.NewLine;
                sSql += "and DET.estado = 'A' inner join" + Environment.NewLine;
                sSql += "cv401_productos PROD on NOM.id_producto = PROD.id_producto" + Environment.NewLine;
                sSql += "and PROD.estado = 'A'" + Environment.NewLine;
                sSql += "where CAB.fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and CAB.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and CAB.estado_orden in ('Pagada', 'Cerrada')" + Environment.NewLine;
                sSql += "and CAB.id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql += "and PROD.id_pos_tipo_producto = " + Convert.ToInt32(dtTipoProducto.Rows[i][0].ToString()) + Environment.NewLine;
                sSql += "group by NOM.nombre" + Environment.NewLine;
                sSql += "order by sum(DET.cantidad)";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dbSumarPorTipoProducto = 0;

                        sContinuar += "".PadRight(40, '=') + Environment.NewLine;
                        sContinuar += "TIPO DE PRODUCTO: " + dtTipoProducto.Rows[i][1].ToString().ToUpper().Trim() + Environment.NewLine;
                        sContinuar += "".PadRight(40, '=') + Environment.NewLine;
                        sTexto = sTexto + " ".PadRight(5, ' ') + "DESCRIPCION".PadRight(20, ' ') + "CANT.".PadLeft(5, ' ') + "TOTAL".PadLeft(10, ' ') + Environment.NewLine;
                        sTexto = sTexto + "*".PadRight(40, '=') + Environment.NewLine;

                        for (int j = 0; j < dtConsulta.Rows.Count; j++)
                        {
                            sNombreProducto = dtConsulta.Rows[j][0].ToString();
                            dCantidad = Convert.ToDouble(dtConsulta.Rows[j][1].ToString());
                            dTotal = Convert.ToDouble(dtConsulta.Rows[j][2].ToString());
                            dbSumarPorTipoProducto += Convert.ToDecimal(dtConsulta.Rows[j][2].ToString());
                            dSuma = dSuma + dTotal;

                            if (sNombreProducto.Length > 25)
                            {
                                sNombreProducto = sNombreProducto.Substring(0, 25);
                            }

                            sContinuar = sContinuar + sNombreProducto.PadRight(25, ' ') + dCantidad.ToString("N0").PadLeft(5, ' ') + dTotal.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                        }

                        sContinuar += "".PadLeft(40, '-') + Environment.NewLine + ("TOTAL " + dtTipoProducto.Rows[i][1].ToString().ToUpper().Trim()).PadRight(30, ' ') + dbSumarPorTipoProducto.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                    }
                }

                sContinuar += Environment.NewLine;
            }

            sContinuar = sContinuar + "*".PadRight(40, '*') + Environment.NewLine + Environment.NewLine;
            sContinuar = sContinuar + "TOTALES".PadRight(30, ' ') + dSuma.ToString("N2").PadLeft(10, ' ') + Environment.NewLine + Environment.NewLine;
            sContinuar = sContinuar + "NOTA: EN EL TOTAL INCLUYE LOS VALORES DE" + Environment.NewLine;
            sContinuar = sContinuar + "CUENTAS POR COBRAR Y CORTESÍAS" + Environment.NewLine;
            sContinuar = sContinuar + Environment.NewLine + Environment.NewLine;

            sContinuar = sContinuar + reporteCantidadPagos();
            sContinuar = sContinuar + Environment.NewLine + Environment.NewLine + Environment.NewLine + ".";

            return sContinuar;

        }

        private string reporteCantidadPagos()
        {
            try
            {
                dbTotalOrdenes = 0;
                dbTotalTarjetas = 0;
                dbTotalEfectivo = 0;
                dbTotalCheques = 0;

                sTextoPagos = "";
                //sTexto = sTexto + Environment.NewLine + Environment.NewLine;

                sSql = "";
                sSql += "select NCP.numero_pedido, NF.numero_factura, FP.id_pedido, NF.idtipocomprobante" + Environment.NewLine;
                sSql += "from cv403_facturas_pedidos FP, cv403_numeros_facturas NF," + Environment.NewLine;
                sSql += "cv403_numero_cab_pedido NCP, cv403_facturas F, cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where NF.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and FP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and NCP.id_pedido = FP.id_pedido" + Environment.NewLine;
                sSql += "and NCP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and NCP.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.estado = 'A'" + Environment.NewLine;
                sSql += "and NF.estado = 'A'" + Environment.NewLine;
                sSql += "and F.estado = 'A'" + Environment.NewLine;
                sSql += "and fecha_factura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "order by FP.id_pedido";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sTextoPagos += "DOCUMENTOS DE VENTA REALIZADOS".PadLeft(35, ' ') + Environment.NewLine;
                        sTextoPagos += "".PadRight(24, '=') + "".PadRight(16, '-') + Environment.NewLine;
                        sTextoPagos += " TICKET  DOCUM.   TOTAL   EF/CH. TARJETA" + Environment.NewLine;
                        sTextoPagos += "".PadRight(24, '=') + "".PadRight(16, '-') + Environment.NewLine;

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sNumeroOrden_P = dtConsulta.Rows[i][0].ToString();
                            sNumeroFactura_P = dtConsulta.Rows[i][1].ToString();;
                            iIdOrden_P = Convert.ToInt32(dtConsulta.Rows[i][2].ToString());
                            iTipoComprobante = Convert.ToInt32(dtConsulta.Rows[i][3].ToString());

                            if (iTipoComprobante == 1)
                            {
                                sNumeroOrden_P = sNumeroOrden_P + "F";
                            }

                            else
                            {
                                sNumeroOrden_P = sNumeroOrden_P + "N";
                            }

                            sTextoDevuelta = desglosePagos(iIdOrden_P);

                            if (sTextoDevuelta != "")
                            {
                                sTextoPagos += "T" + sNumeroOrden_P.PadLeft(8, ' ') + sNumeroFactura_P.PadLeft(7, ' ') + sTextoDevuelta;
                            }
                        }

                        sTextoPagos += Environment.NewLine;
                        sTextoPagos += "".PadLeft(40, '-') + Environment.NewLine;
                        sTextoPagos += "TOTAL VENDIDO  : " + dbTotalOrdenes.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                        dbAuxiliar = dbTotalOrdenes;
                        sTextoPagos += "TOTAL EN FACT. : " + dbTotalOrdenes.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                        //sTextoPagos += "IVA COBRADO    : " + (dbTotalOrdenes - (dbTotalOrdenes / (1 + (dbPorcentajeIva/100)))).ToString("N2").PadLeft(10, ' ') + Environment.NewLine + Environment.NewLine;
                        sIvaCobrado = "0.00";
                        extraerIva();
                        sTextoPagos += "IVA COBRADO    : " + sIvaCobrado.PadLeft(10, ' ') + Environment.NewLine + Environment.NewLine;

                        //MOSTRAR VALORES EN CORTESIAS, VALES FUNCIONARIOS
                        //CONTAR LAS CORTESIA

                        sTextoDevuelta = extraerOtrosValores("04");
                        
                        if (sTextoDevuelta != "")
                        {
                            sTextoPagos += "TOT. CORTESIAS : " + sTextoDevuelta.PadLeft(10, ' ') + Environment.NewLine;
                        }

                        //VALES FUNCIONARIOS
                        sTextoDevuelta = extraerOtrosValores("05");

                        if (sTextoDevuelta != "")
                        {
                            sTextoPagos += "T. FUNCIONARIOS: " + sTextoDevuelta.PadLeft(10, ' ') + Environment.NewLine;
                        }

                        sTextoPagos += "".PadLeft(40, '-') + Environment.NewLine;
                        sTextoDevuelta = "";
                        sTextoDevuelta = calcularDescuentos();
                        if (sTextoDevuelta != "")
                        {
                            sTextoPagos += "TOT. DESCUENTOS: " + sTextoDevuelta.PadLeft(10, ' ') + Environment.NewLine;
                        }


                        sTextoPagos += "".PadLeft(40, '-') + Environment.NewLine;
                        sTextoPagos += "TOTAL REPORTADO: " + dbTotalOrdenes.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                        sTextoPagos += "".PadLeft(40, '-') + Environment.NewLine;

                        //TOTALES DESGLOSADOS
                        if (dbTotalEfectivo != 0)
                        {
                            sTextoPagos += "TOTAL EFECTIVO : " + dbTotalEfectivo.ToString("N2").PadLeft(10, ' ') + Environment.NewLine; ;
                        }

                        if (dbTotalCheques != 0)
                        {
                            sTextoPagos += "TOTAL CHEQUES  : " + dbTotalCheques.ToString("N2").PadLeft(10, ' ') + Environment.NewLine; ;
                        }

                        if (dbTotalTarjetas != 0)
                        {
                            sTextoPagos += "TOTAL TARJETAS : " + dbTotalTarjetas.ToString("N2").PadLeft(10, ' ') + Environment.NewLine; ;
                        }

                        sTextoPagos += "".PadLeft(40, '-') + Environment.NewLine;
                        sTextoPagos += "TOTAL COBRADO  : " + dbAuxiliar.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;

                        sTextoPagos += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                        //sTextoPagos += "TOTAL DE PAGO DE ORDENES".PadRight(30, ' ') + ("$" + dbTotalOrdenes.ToString("N2")).PadLeft(10, ' ') + Environment.NewLine + Environment.NewLine;
                        //sTextoPagos += "TOTAL EN TARJETAS".PadRight(30, ' ') + ("$" + dbTotalTarjetas.ToString("N2")).PadLeft(10, ' ') + Environment.NewLine;
                        //sTextoPagos += "TOTAL EN EFECTIVO".PadRight(30, ' ') + ("$" + dbTotalEfectivo.ToString("N2")).PadLeft(10, ' ') + Environment.NewLine;

                        return sTextoPagos;
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
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                return "";
            }
        }

        public string desglosePagos(int iIdPedido)
        {
            try
            {
                sSql = "";
                //sSql += "select descripcion, valor, cambio, count(*) cuenta, codigo" + Environment.NewLine;
                sSql += "select descripcion, sum(valor) valor, cambio, count(*) cuenta, codigo" + Environment.NewLine;
                sSql += "from pos_vw_pedido_forma_pago" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "group by descripcion, valor, codigo, cambio" + Environment.NewLine;
                sSql += "having count(*) >= 1";

                dtPagosDesglose = new DataTable();
                dtPagosDesglose.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtPagosDesglose, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dTotalPagadoP = 0;
                        iBandera = 0;
                        sTextoDesglose = "";

                        

                        //CICLO PARA OBTENER EL TOTAL FACTURADO EN LA ORDEN

                        for (int i = 0; i < dtPagosDesglose.Rows.Count; i++)
                        {
                            dTotalPagadoP = dTotalPagadoP + Convert.ToDouble(dtPagosDesglose.Rows[i][1].ToString());
                        }

                        dbTotalOrdenes = dbTotalOrdenes + dTotalPagadoP;

                        //CICLO PARA DETALLAR LOS PAGOS REALIZADOS EN LA ORDEN

                        for (int i = 0; i < dtPagosDesglose.Rows.Count; i++)
                        {
                            if (iBandera == 0)
                            {                                
                                sCodigoCobro = dtPagosDesglose.Rows[i][4].ToString();

                                if ((sCodigoCobro == "EF") || (sCodigoCobro == "CH"))
                                {
                                    dEfectivoCheque_P = Convert.ToDouble(dtPagosDesglose.Rows[i][1].ToString());
                                    sTextoDesglose = sTextoDesglose + dTotalPagadoP.ToString("N2").PadLeft(8, ' ') + dEfectivoCheque_P.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;

                                    if (sCodigoCobro == "EF")
                                    {
                                        dbTotalEfectivo = dbTotalEfectivo + dEfectivoCheque_P;
                                    }

                                    else if (sCodigoCobro == "CH")
                                    {
                                        dbTotalCheques = dbTotalCheques + dEfectivoCheque_P;
                                    }
                                }

                                else
                                {
                                    sNombreTarjeta_P = dtPagosDesglose.Rows[i][0].ToString();
                                    dTarjeta_P = Convert.ToDouble(dtPagosDesglose.Rows[i][1].ToString());
                                    //sTextoDesglose = sTextoDesglose + dTotalPagadoP.ToString("N2").PadLeft(8, ' ') + dTarjeta_P.ToString("N2").PadLeft(16, ' ') + Environment.NewLine;

                                    if (sNombreTarjeta_P.Length > 11)
                                    {
                                        sNombreTarjeta_P = sNombreTarjeta_P.Substring(0, 11);
                                    }

                                    if (dtPagosDesglose.Rows.Count == 1)
                                    {
                                        //sTextoDesglose = sTextoDesglose + "".PadLeft(19, ' ') + sNombreTarjeta_P.PadRight(12, ' ') + ":" + dTarjeta_P.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;
                                        sTextoDesglose = sTextoDesglose + dTotalPagadoP.ToString("N2").PadLeft(8, ' ') + dTarjeta_P.ToString("N2").PadLeft(16, ' ') + Environment.NewLine;
                                        sTextoDesglose = sTextoDesglose + "".PadLeft(19, ' ') + sNombreTarjeta_P.PadRight(12, ' ') + ":" + dTarjeta_P.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;
                                    }
                                    else
                                    {
                                        sTextoDesglose = sTextoDesglose + dTotalPagadoP.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;
                                        sTextoDesglose = sTextoDesglose + "".PadLeft(19, ' ') + sNombreTarjeta_P.PadRight(12, ' ') + ":" + dTarjeta_P.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;
                                    }

                                    dbTotalTarjetas = dbTotalTarjetas + dTarjeta_P;
                                }

                                iBandera = 1;
                            }

                            else
                            {
                                sCodigoCobro = dtPagosDesglose.Rows[i][4].ToString();

                                if ((sCodigoCobro == "EF") || (sCodigoCobro == "CH"))
                                {
                                    dEfectivoCheque_P = Convert.ToDouble(dtPagosDesglose.Rows[i][1].ToString());
                                    sTextoDesglose = sTextoDesglose + dEfectivoCheque_P.ToString("N2").PadLeft(32, ' ') + Environment.NewLine;

                                    if (sCodigoCobro == "EF")
                                    {
                                        dbTotalEfectivo = dbTotalEfectivo + dEfectivoCheque_P;
                                    }

                                    else if (sCodigoCobro == "CH")
                                    {
                                        dbTotalCheques = dbTotalCheques + dEfectivoCheque_P;
                                    }
                                }

                                else
                                {
                                    dTarjeta_P = Convert.ToDouble(dtPagosDesglose.Rows[i][1].ToString());
                                    sNombreTarjeta_P = dtPagosDesglose.Rows[i][0].ToString();
                                    sTextoDesglose = sTextoDesglose + "".PadLeft(19, ' ') + sNombreTarjeta_P.PadRight(12, ' ') + ":" + dTarjeta_P.ToString("N2").PadLeft(8, ' ') + Environment.NewLine;

                                    dbTotalTarjetas = dbTotalTarjetas + dTarjeta_P;
                                }
                            }
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
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                return "";
            }
        }

        public string extraerOtrosValores(string sCodigo)
        {
            try
            {
                sTextoDesglose = "";
                dTotalPagadoP = 0;

                sSql = "";
                sSql += "select " + conexion.GFun_St_esnulo() + "(sum(DP.cantidad * (DP.precio_unitario + DP.valor_iva - DP.valor_dscto)), 0) total" + Environment.NewLine;
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
                        dbTotalOrdenes = dbTotalOrdenes + dTotalPagadoP;

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
                catchMensaje.ShowInTaskbar = false;
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
