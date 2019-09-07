using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace Palatium.Clases
{
    class libroCaja
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        string sSql;
        bool bRespuesta;
        DataTable dtConsulta;
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        StreamWriter sw;
        string sFecha = Program.sFechaSistema.ToString("yyyy-MM-dd");
        double dbTotalEntradas = 0;
        double dbTotalSalidas = 0;

        string sFechaConsulta;

        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        string sTexto;

        //Función para llenar el informa de arqueo de caja
        public void llenarInforme()
        {
            try
            {
                string sPath = @"c:\reportes\LibroDiario.txt";
                string sFechaCierre = Program.sFechaSistema.ToString("dd/MM/yyyy");

                if (File.Exists(sPath))
                {
                    sw = new StreamWriter(sPath);
                    sw.WriteLine();
                    sw.WriteLine(Program.local.PadLeft(30, ' '));
                    sw.WriteLine("=".PadRight(40, '='));
                    sw.WriteLine("LIBRO CAJA".PadLeft(25, ' '));
                    sw.WriteLine("=".PadRight(40, '='));
                    sw.WriteLine("FECHA:".PadRight(8, ' ') + sFechaCierre);
                    sw.WriteLine("DESDE");
                    sw.WriteLine();
                    sw.WriteLine("-".PadRight(40, '-'));
                    sw.WriteLine("  " +"HORA".PadRight(7, ' ') + "CONCEPTO".PadRight(25, ' ') + "VALOR");
                    sw.WriteLine("-".PadRight(40, '-'));
                    sw.WriteLine();
                    llenarDetalle();
                    sw.Close();

                }
                else
                {
                    ok.LblMensaje.Text = "No existe el archivo en la ruta: " + sPath + " .\nPor favor, comuníquese con el administrador";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                }


            }
            catch (Exception ex)
            {
                ok.LblMensaje.Text = ex.ToString();
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }
        }

        //Función para llenar el detalle del reporte
        private void llenarDetalle()
        {
            sSql = "";
            sSql += "select POS.fecha_ingreso, 'Cobro Ticket Nº ' + convert(varchar(10), POS.numero_pedido)," + Environment.NewLine;
            sSql += "sum(DET.cantidad * (DET.Precio_Unitario + DET.Valor_iva + DET.valor_otro - DET.Valor_Dscto)) Total," + Environment.NewLine;
            sSql += "Pos.id_pedido, '1' 'Bandera Pedido', '+' Simbolo" + Environment.NewLine;
            sSql += "from pos_vw_factura POS inner join" + Environment.NewLine;
            sSql += "cv403_det_pedidos DET on POS.id_det_pedido = DET.id_det_pedido inner join" + Environment.NewLine;
            sSql += "cv403_cab_pedidos CAB on Pos.id_pedido = CAb.id_pedido" + Environment.NewLine;
            sSql += "where POS.fecha_factura = '" + sFecha + "'  and Cab.estado = 'A'" + Environment.NewLine;
            sSql += "group by POS.fecha_ingreso, POS.numero_pedido, Pos.id_pedido" + Environment.NewLine;
            sSql += "union " + Environment.NewLine;
            sSql += "select hora, concepto, valor, id_pos_movimiento_caja, '0' 'Bandera Pedido', '-' Simbolo " + Environment.NewLine;
            sSql += "from pos_movimiento_caja where estado = 'A'" + Environment.NewLine;
            sSql += "and fecha = '" + sFecha + "'" + Environment.NewLine;
            sSql += "and tipo_movimiento = 0" + Environment.NewLine;
            sSql += "and id_documento_pago is null " + Environment.NewLine;
            sSql += "union select hora, concepto, valor, id_pos_movimiento_caja, '2' 'Bandera Pedido', '+' Simbolo " + Environment.NewLine;
            sSql += "from pos_movimiento_caja where estado = 'A'" + Environment.NewLine;
            sSql += "and fecha = '" + sFecha + "'" + Environment.NewLine;
            sSql += "and tipo_movimiento = 1" + Environment.NewLine;
            sSql += "and id_documento_pago is null " + Environment.NewLine;
            sSql += "order by POS.fecha_ingreso";
                     

            dtConsulta = new DataTable();
            dtConsulta.Clear();
            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        string sHora = Convert.ToDateTime(dtConsulta.Rows[i].ItemArray[0]).ToString("yyyy-MM-dd HH:mm:ss");
                        sHora = sHora.Substring(10, 6);
                        string sConcepto = dtConsulta.Rows[i].ItemArray[1].ToString();
                        double dbTotal = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[2].ToString());
                        int iIdPedido = Convert.ToInt32(dtConsulta.Rows[i].ItemArray[3].ToString());
                        int iBanderaPedido = Convert.ToInt32(dtConsulta.Rows[i].ItemArray[4].ToString());
                        string sSimbolo = dtConsulta.Rows[i].ItemArray[5].ToString();

                        if (sConcepto.Length < 25)
                            sw.WriteLine(sSimbolo + sHora.PadLeft(6, ' ') + ' ' + sConcepto.PadRight(26, ' ') +  dbTotal.ToString("N2").PadLeft(6, ' '));
                        else
                        {
                            sw.WriteLine(sSimbolo + sHora.PadLeft(6, ' ') + ' ' + sConcepto.Substring(0,24).PadRight(26, ' ') +  dbTotal.ToString("N2").PadLeft(6, ' '));
                            sw.WriteLine(" ".PadRight(8, ' ')  + sConcepto.Substring(24).PadRight(30, ' ') );
                        }


                        if (iBanderaPedido == 1)
                            llenarFormasPago(iIdPedido);
                        else if (iBanderaPedido == 0)
                            dbTotalSalidas += dbTotal;

                        if ((iBanderaPedido == 1) || (iBanderaPedido == 2))
                            dbTotalEntradas += dbTotal;
                    }

                    sw.WriteLine("=".PadRight(40, '='));
                    sw.WriteLine("TOTAL ENTRADAS: ".PadRight(17, ' ') + dbTotalEntradas.ToString("N2").PadLeft(6, ' '));
                    sw.WriteLine("TOTAL SALIDAS: ".PadRight(17, ' ') + dbTotalSalidas.ToString("N2").PadLeft(6, ' '));

                }
                else
                    sw.WriteLine("No hay datos para ser reportados\n\n\n");
            }
            else
            {
                ok.LblMensaje.Text = "Ocurrió un problema al cargar el detalle del reporte";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }

        }

        //Función para llenar las formas de pago
        private void llenarFormasPago(int iIdPedido)
        {
            string sQuery;
            sQuery = "";
            sQuery += "select descripcion, sum(valor) valor, cambio,  count(*) cuenta, codigo " + Environment.NewLine;
            sQuery += "from pos_vw_pedido_forma_pago" + Environment.NewLine;
            sQuery += "where id_pedido = '" + iIdPedido + "'" + Environment.NewLine;
            sQuery += "group by descripcion, valor, codigo, cambio" + Environment.NewLine;
            sQuery += "having count(*) >= 1";

            DataTable dtPagos = new DataTable();
            dtPagos.Clear();
            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtPagos, sQuery);
            if (bRespuesta == true)
            {
                if (dtPagos.Rows.Count > 0)
                {
                    for (int i = 0; i < dtPagos.Rows.Count; i++)
                    {
                        string sDescripcion = dtPagos.Rows[i].ItemArray[0].ToString();

                        sw.WriteLine(" ".PadRight(8,' ') + "("+ sDescripcion +")" );
                    }

                }
            }
        }

        //FUNCIONES PARA LLENAR EL TEXTO EN UN TEXTBOX
        public string llenarEncabezadoLibroDiario(string sFecha)
        {
            try
            {
                this.sFechaConsulta = sFecha;

                sTexto = "";
                sTexto = sTexto + Program.local.PadLeft(30, ' ') + Environment.NewLine;
                sTexto = sTexto + "=".PadRight(40, '=') + Environment.NewLine;
                sTexto = sTexto + "LIBRO CAJA".PadLeft(25, ' ') + Environment.NewLine;
                sTexto = sTexto + "=".PadRight(40, '=') + Environment.NewLine;
                sTexto = sTexto + "FECHA:".PadRight(8, ' ') + DateTime.Now.ToString("dd/MM/yyyy") + Environment.NewLine + Environment.NewLine;
                //sTexto = sTexto + "DESDE" + Environment.NewLine + Environment.NewLine;
                sTexto = sTexto + "-".PadRight(40, '-') + Environment.NewLine;
                sTexto = sTexto + "  " + "HORA".PadRight(7, ' ') + "CONCEPTO".PadRight(25, ' ') + "VALOR" + Environment.NewLine;
                sTexto = sTexto + "-".PadRight(40, '-') + Environment.NewLine + Environment.NewLine;
                
                llenarDetalleTexto();
                sTexto = sTexto + Environment.NewLine + Environment.NewLine + Environment.NewLine + ".";

                return sTexto;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                return "";
            }
        }

        //FUNCIONES PARA LLENAR EL TEXTO EN UN TEXTBOX
        private void llenarDetalleTexto()
        {
            try
            {
                sSql = "";
                sSql += "select POS.fecha_ingreso, 'Cobro Ticket Nº ' + convert(varchar(10), POS.numero_pedido)," + Environment.NewLine;
                sSql += "sum(DET.cantidad * (DET.Precio_Unitario + DET.Valor_iva + DET.valor_otro - DET.Valor_Dscto)) Total," + Environment.NewLine;
                sSql += "Pos.id_pedido, '1' 'Bandera Pedido', '+' Simbolo " + Environment.NewLine;
                sSql += "from pos_vw_factura POS inner join" + Environment.NewLine;
                sSql += "cv403_det_pedidos DET on POS.id_det_pedido = DET.id_det_pedido" + Environment.NewLine;
                sSql += "inner join cv403_cab_pedidos CAB on Pos.id_pedido = CAb.id_pedido" + Environment.NewLine;
                sSql += "where POS.fecha_factura = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and Cab.estado = 'A'" + Environment.NewLine;
                sSql += "group by POS.fecha_ingreso, POS.numero_pedido, Pos.id_pedido" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select hora, concepto, valor, id_pos_movimiento_caja, '0' 'Bandera Pedido', '-' Simbolo " + Environment.NewLine;
                sSql += "from pos_movimiento_caja where estado = 'A'" + Environment.NewLine;
                sSql += "and fecha = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and tipo_movimiento = 0" +Environment.NewLine;
                sSql += "and id_documento_pago is null " + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select hora, concepto, valor, id_pos_movimiento_caja, '2' 'Bandera Pedido', '+' Simbolo" + Environment.NewLine;
                sSql += "from pos_movimiento_caja" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and fecha = '" + sFecha + "'" + Environment.NewLine;
                sSql += "and tipo_movimiento = 1" + Environment.NewLine;
                sSql += "and id_documento_pago is null " + Environment.NewLine;
                sSql += "order by POS.fecha_ingreso";


                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            string sHora = Convert.ToDateTime(dtConsulta.Rows[i].ItemArray[0]).ToString("yyyy-MM-dd HH:mm:ss");
                            sHora = sHora.Substring(10, 6);
                            string sConcepto = dtConsulta.Rows[i].ItemArray[1].ToString();
                            double dbTotal = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[2].ToString());
                            int iIdPedido = Convert.ToInt32(dtConsulta.Rows[i].ItemArray[3].ToString());
                            int iBanderaPedido = Convert.ToInt32(dtConsulta.Rows[i].ItemArray[4].ToString());
                            string sSimbolo = dtConsulta.Rows[i].ItemArray[5].ToString();

                            if (sConcepto.Length < 25)
                                sTexto = sTexto + sSimbolo + sHora.PadLeft(6, ' ') + ' ' + sConcepto.PadRight(26, ' ') + dbTotal.ToString("N2").PadLeft(6, ' ') + Environment.NewLine;
                            //sw.WriteLine(sSimbolo + sHora.PadLeft(6, ' ') + ' ' + sConcepto.PadRight(26, ' ') + dbTotal.ToString("N2").PadLeft(6, ' '));
                            else
                            {
                                sTexto = sTexto + sSimbolo + sHora.PadLeft(6, ' ') + ' ' + sConcepto.Substring(0, 24).PadRight(26, ' ') + dbTotal.ToString("N2").PadLeft(6, ' ') + Environment.NewLine;
                                sTexto = sTexto + " ".PadRight(8, ' ') + sConcepto.Substring(24).PadRight(30, ' ') + Environment.NewLine;
                                //sw.WriteLine(sSimbolo + sHora.PadLeft(6, ' ') + ' ' + sConcepto.Substring(0, 24).PadRight(26, ' ') + dbTotal.ToString("N2").PadLeft(6, ' '));
                                //sw.WriteLine(" ".PadRight(8, ' ') + sConcepto.Substring(24).PadRight(30, ' '));
                            }


                            if (iBanderaPedido == 1)
                                llenarFormasPagoTexto(iIdPedido);
                            else if (iBanderaPedido == 0)
                                dbTotalSalidas += dbTotal;

                            if ((iBanderaPedido == 1) || (iBanderaPedido == 2))
                                dbTotalEntradas += dbTotal;
                        }

                        sTexto = sTexto + "=".PadRight(40, '=') + Environment.NewLine;
                        sTexto = sTexto + "TOTAL ENTRADAS: ".PadRight(17, ' ') + dbTotalEntradas.ToString("N2").PadLeft(6, ' ') + Environment.NewLine;
                        sTexto = sTexto + "TOTAL SALIDAS: ".PadRight(17, ' ') + dbTotalSalidas.ToString("N2").PadLeft(6, ' ');

                    }
                    else
                    {
                        sTexto = sTexto + "No hay datos para ser reportados" + Environment.NewLine + Environment.NewLine;
                    }
                }
                else
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al cargar el detalle del reporte";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //Función para llenar las formas de pago
        private void llenarFormasPagoTexto(int iIdPedido)
        {
            try
            {
                string sQuery;
                sQuery = "";
                sQuery += "select descripcion, sum(valor) valor, cambio,  count(*) cuenta, codigo " + Environment.NewLine;
                sQuery += "from pos_vw_pedido_forma_pago" + Environment.NewLine;
                sQuery += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sQuery += "group by descripcion, valor, codigo, cambio" + Environment.NewLine;
                sQuery += "having count(*) >= 1";

                DataTable dtPagos = new DataTable();
                dtPagos.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtPagos, sQuery);
                if (bRespuesta == true)
                {
                    if (dtPagos.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtPagos.Rows.Count; i++)
                        {
                            string sDescripcion = dtPagos.Rows[i].ItemArray[0].ToString();
                            sTexto = sTexto + " ".PadRight(8, ' ') + "(" + sDescripcion + ")" + Environment.NewLine;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }
    }
}
