using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Palatium.Clases
{
    public class ClaseCargarParametros
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        string sSql;
        string sRetorno;

        DataTable dtConsulta;
        bool bRespuesta = false;

        //CARGAR PARAMETROS EN EL SISTEMA
        public string cargarParametros()
        {
            try
            {
                sSql = "";
                sSql += "select iva, ice, servicio, descuento_empleados, leer_mesero, " + Environment.NewLine;
                sSql += "imprimir_orden, maneja_servicio, maneja_facturacion_electronica, " + Environment.NewLine;
                sSql += "etiqueta_mesa, tamano_letra_mesa, configuracion_decimales," + Environment.NewLine;
                sSql += "codigo_modificador, logo, maneja_nota_venta, seleccion_mesero," + Environment.NewLine;
                sSql += "vista_previa_impresion, opcion_login, habilitar_teclado_touch," + Environment.NewLine;
                sSql += "demo, descarga_receta, id_producto_modificador, id_producto_domicilio," + Environment.NewLine;
                sSql += "id_producto_item, animacion_mesas, rise, isnull(url_contabilidad, '') url_contabilidad," + Environment.NewLine;
                sSql += "precio_incluye_impuesto, descuenta_iva, maneja_nomina, maneja_almuerzos," + Environment.NewLine;
                sSql += "isnull(numero_personas_default, 0) numero_personas_default, ruta_reportes," + Environment.NewLine;
                sSql += "aplica_recargo_tarjetas, porcentaje_recargo_tarjetas, idtipocomprobante," + Environment.NewLine;
                sSql += "isnull(correo_electronico_default, '') correo_electronico_default" + Environment.NewLine;
                sSql += "from pos_parametro where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.iva = Convert.ToDouble(dtConsulta.Rows[0][0].ToString()) / 100;
                        Program.ice = Convert.ToDouble(dtConsulta.Rows[0][1].ToString()) / 100;
                        Program.servicio = Convert.ToDouble(dtConsulta.Rows[0][2].ToString()) / 100;
                        Program.descuento_empleados = Convert.ToDouble(dtConsulta.Rows[0][3].ToString()) / 100;
                        Program.iLeerMesero = Convert.ToInt32(dtConsulta.Rows[0][4].ToString());
                        Program.iImprimeOrden = Convert.ToInt32(dtConsulta.Rows[0][5].ToString());
                        Program.iManejaServicio = Convert.ToInt32(dtConsulta.Rows[0][6].ToString());
                        Program.iFacturacionElectronica = Convert.ToInt32(dtConsulta.Rows[0][7].ToString());
                        Program.iBanderaNumeroMesa = Convert.ToInt32(dtConsulta.Rows[0][8].ToString());
                        Program.iTamañoLetraMesa = float.Parse(dtConsulta.Rows[0][9].ToString());
                        Program.iHabilitarDecimal = Convert.ToInt32(dtConsulta.Rows[0][10].ToString());
                        Program.sCodigoModificador = dtConsulta.Rows[0][11].ToString();
                        Program.sLogo = dtConsulta.Rows[0][12].ToString();
                        Program.iManejaNotaVenta = Convert.ToInt32(dtConsulta.Rows[0][13].ToString());
                        Program.iSeleccionMesero = Convert.ToInt32(dtConsulta.Rows[0][14].ToString());
                        Program.iVistaPreviaImpresiones = Convert.ToInt32(dtConsulta.Rows[0][15].ToString());
                        Program.iUsuarioLogin = Convert.ToInt32(dtConsulta.Rows[0][16].ToString());
                        Program.iActivaTeclado = Convert.ToInt32(dtConsulta.Rows[0][17].ToString());
                        Program.iVersionDemo = Convert.ToInt32(dtConsulta.Rows[0][18].ToString());
                        Program.iUsarReceta = Convert.ToInt32(dtConsulta.Rows[0][19].ToString());
                        Program.iIdProductoModificador = Convert.ToInt32(dtConsulta.Rows[0][20].ToString());
                        Program.iIdProductoDomicilio = Convert.ToInt32(dtConsulta.Rows[0][21].ToString());
                        Program.iIdProductoNuevoItem = Convert.ToInt32(dtConsulta.Rows[0][22].ToString());
                        Program.iDisenioMesas = Convert.ToInt32(dtConsulta.Rows[0][23].ToString());
                        Program.iManejaRise = Convert.ToInt32(dtConsulta.Rows[0][24].ToString());
                        Program.sUrlContabilidad = dtConsulta.Rows[0][25].ToString();
                        Program.iCobrarConSinProductos = Convert.ToInt32(dtConsulta.Rows[0][26].ToString());
                        Program.iDescuentaIva = Convert.ToInt32(dtConsulta.Rows[0][27].ToString());
                        Program.iManejaNomina = Convert.ToInt32(dtConsulta.Rows[0][28].ToString());
                        Program.iManejaAlmuerzos = Convert.ToInt32(dtConsulta.Rows[0][29].ToString());
                        Program.iNumeroPersonasDefault = Convert.ToInt32(dtConsulta.Rows[0][30].ToString());
                        Program.sUrlReportes = dtConsulta.Rows[0][31].ToString();
                        Program.iAplicaRecargoTarjeta = Convert.ToInt32(dtConsulta.Rows[0][32].ToString());
                        Program.dbPorcentajeRecargoTarjeta = Convert.ToDecimal(dtConsulta.Rows[0][33].ToString()) / 100;
                        Program.iComprobanteNotaEntrega = Convert.ToInt32(dtConsulta.Rows[0]["idtipocomprobante"].ToString());
                        Program.sCorreoElectronicoDefault = dtConsulta.Rows[0]["correo_electronico_default"].ToString();

                        fechaSistema();

                        return "";
                    }

                    else
                    {                        
                        return "No se ha configurado los parámetros para el sistema.";
                    }
                }

                else
                {
                    return "Ocurrió un problema al realizar la consulta. Comuníquese con el administrador.";
                }
            }

            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string cargarParametrosPredeterminados()
        {
            try
            {
                //sSql = "select PL.id_localidad, PL.cg_ciudad, PL.id_pos_cajero, PL.id_pos_mesero, " +
                //       "M.descripcion, PL.cg_moneda, PL.id_pos_formato_precuenta, PL.id_pos_formato_factura, " +
                //       "PL.imprimir_cocina, PL.consumidor_final, PL.id_vendedor, PL.maneja_jornada " +
                //       "from pos_parametro_localidad PL, pos_mesero M where M.id_pos_mesero = PL.id_pos_mesero " +
                //       "and PL.estado = 'A' and M.estado = 'A'";

                sSql = "";
                sSql += "select PL.id_localidad, PL.cg_ciudad, PL.id_pos_cajero, PL.id_pos_mesero, M.descripcion," + Environment.NewLine;
                sSql += "PL.cg_moneda, PL.id_pos_formato_precuenta, PL.id_pos_formato_factura, PL.imprimir_cocina," + Environment.NewLine;
                sSql += "PL.consumidor_final, PL.id_vendedor, PL.maneja_jornada, C.descripcion, PL.imprimir_datos_factura, VC.valor_texto," + Environment.NewLine;
                sSql += "PL.id_producto_anula, PL.valor_precio_anula, PL.clave_acceso_admin," + Environment.NewLine;
                sSql += "isnull(PL.id_pos_impresora, 0), isnull(ejecutar_impresion, 0) ejecutar_impresion," + Environment.NewLine;
                sSql += "isnull(permitir_abrir_cajon, 0) permitir_abrir_cajon" + Environment.NewLine;
                sSql += "from pos_parametro_localidad PL, pos_mesero M, pos_cajero C, tp_vw_ciudad VC" + Environment.NewLine;
                sSql += "where M.id_pos_mesero = PL.id_pos_mesero" + Environment.NewLine;
                sSql += "and C.id_pos_cajero = PL.id_pos_cajero" + Environment.NewLine;
                sSql += "and VC.correlativo = PL.cg_ciudad" + Environment.NewLine;
                sSql += "and PL.estado = 'A'" + Environment.NewLine;
                sSql += "and M.estado = 'A'" + Environment.NewLine;
                sSql += "and C.estado = 'A'" + Environment.NewLine;
                sSql += "and PL.id_localidad = " + Program.iIdLocalidad;


                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.iCgLocalidad = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                        Program.iIdCajeroDefault = Convert.ToInt32(dtConsulta.Rows[0][2].ToString());
                        Program.iIdMesero = Convert.ToInt32(dtConsulta.Rows[0][3].ToString());
                        Program.nombreMesero = dtConsulta.Rows[0][4].ToString();

                        Program.iMoneda = Convert.ToInt32(dtConsulta.Rows[0][5].ToString());
                        Program.iFormatoPrecuenta = Convert.ToInt32(dtConsulta.Rows[0][6].ToString());
                        Program.iFormatoFactura = Convert.ToInt32(dtConsulta.Rows[0][7].ToString());
                        Program.iImprimirCocina = Convert.ToInt32(dtConsulta.Rows[0][8].ToString());
                        Program.iIdPersona = Convert.ToInt32(dtConsulta.Rows[0][9].ToString());
                        Program.iIdVendedor = Convert.ToInt32(dtConsulta.Rows[0][10].ToString());
                        Program.iManejaJornada = Convert.ToInt32(dtConsulta.Rows[0][11].ToString());
                        Program.sNombreCajeroDefault = dtConsulta.Rows[0][12].ToString();
                        Program.iImprimirDatosFactura = Convert.ToInt32(dtConsulta.Rows[0][13].ToString());
                        Program.sCiudadDefault = dtConsulta.Rows[0][14].ToString().ToUpper();
                        Program.iIdProductoAnular = Convert.ToInt32(dtConsulta.Rows[0][15].ToString());
                        Program.dValorProductoAnular = Convert.ToDouble(dtConsulta.Rows[0][16].ToString());
                        Program.dValorProductoAnular = Program.dValorProductoAnular + (Program.dValorProductoAnular * Program.iva);
                        Program.sPasswordAdmin = dtConsulta.Rows[0][17].ToString();
                        Program.iIdImpresoraReportes = Convert.ToInt32(dtConsulta.Rows[0][18].ToString());
                        Program.iEjecutarImpresion = Convert.ToInt32(dtConsulta.Rows[0][19].ToString());
                        Program.iPermitirAbrirCajon = Convert.ToInt32(dtConsulta.Rows[0][20].ToString());
                        return "";
                    }

                    else
                    {
                        return "No se ha configurado los parámetros predeterminados para el sistema.";
                    }
                }

                else
                {
                    return "Ocurrió un problema al realizar la consulta. Comuníquese con el administrador.";
                }
            }

            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string cargarFormatosImpresiones()
        {
            try
            {
                sSql = "select * from pos_formato_precuenta where estado = 'A'";
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.iFormatoPrecuenta =  Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                        sSql = "";

                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                        if (bRespuesta == true)
                        {
                            if (dtConsulta.Rows.Count > 0)
                            {
                                Program.iFormatoPrecuenta = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                                return "";
                            }
                            else
                            {
                                return "Ocurrió un problema al realizar la consulta. Comuníquese con el administrador.";
                            }
                        }

                        else
                        {
                            return "Ocurrió un problema al realizar la consulta. Comuníquese con el administrador.";
                        }

                    }

                    else
                    {
                        return "Ocurrió un problema al realizar la consulta. Comuníquese con el administrador.";
                    }
                }

                else
                {
                    return "Ocurrió un problema al realizar la consulta. Comuníquese con el administrador.";
                }
            }

            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string cargarDatosTerminal()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_terminal from pos_terminal" + Environment.NewLine;
                sSql += "where nombre_maquina = '" + Environment.MachineName.ToString() + "'" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.iIdTerminal = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                        return "";
                    }
                    else
                    {
                        return "Ocurrió un problema al obtener el ID del terminal. Comuníquese con el administrador.";
                    }
                }

                else
                {
                    return "Ocurrió un problema al obtener el ID del terminal. Comuníquese con el administrador.";
                }
            }

            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public void cargarDatosImpresion()
        {
            try
            {
                sSql = "";
                sSql += "select direccion, telefono1, telefono2, establecimiento," + Environment.NewLine;
                sSql += "punto_emision nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades" + Environment.NewLine;
                sSql += "where id_localidad = " + Program.iIdLocalidad;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.direccion = dtConsulta.Rows[0][0].ToString();
                        Program.telefono1 = dtConsulta.Rows[0][1].ToString();
                        Program.telefono2 = dtConsulta.Rows[0][2].ToString();
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se encontraron registros.";
                        ok.ShowInTaskbar = false;
                        ok.ShowDialog();
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

        public void cargarDatosEmpresa()
        {
            try
            {
                sSql = "";
                sSql += "select razonsocial, direccionmatriz" + Environment.NewLine;
                sSql += "from sis_empresa" + Environment.NewLine;
                sSql += "where idempresa = " + Program.iIdEmpresa;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.local = dtConsulta.Rows[0][0].ToString();
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se encontraron registros.";
                        ok.ShowDialog();
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
        
        //CARGAR FECHA DE LA BASE DE DATOS
        private void fechaSistema()
        {
            try
            {
                sSql = "select getdate()";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                
                if (bRespuesta == true)
                {
                    Program.sFechaSistema = Convert.ToDateTime(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }

            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        public void cargarParametrosFacturacionElectronica()
        {
            try
            {
                sSql = "";
                sSql += "select id_tipo_ambiente, id_tipo_emision," + Environment.NewLine;
                sSql += "id_tipo_certificado_digital, numeroruc" + Environment.NewLine;
                sSql += "from sis_empresa" + Environment.NewLine;
                sSql += "where idempresa = " + Program.iIdEmpresa;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.iTipoAmbiente = Convert.ToInt32(dtConsulta.Rows[0]["id_tipo_ambiente"].ToString());
                        Program.iTipoEmision = Convert.ToInt32(dtConsulta.Rows[0]["id_tipo_emision"].ToString());
                        Program.iTipoCertificado = Convert.ToInt32(dtConsulta.Rows[0]["id_tipo_certificado_digital"].ToString());
                        Program.sNumeroRucEmisor = dtConsulta.Rows[0]["numeroruc"].ToString().Trim();

                        if (Program.iTipoAmbiente == 0)
                        {
                            ok.LblMensaje.Text = "No se encuentra configurado el tipo de ambiente para facturación electrónica. Comuníquese con el administrador.";
                            ok.ShowDialog();
                            return;
                        }

                        else if (Program.iTipoEmision == 0)
                        {
                            ok.LblMensaje.Text = "No se encuentra configurado el tipo de emisión para facturación electrónica. Comuníquese con el administrador.";
                            ok.ShowDialog();
                            return;
                        }

                        else if (Program.iTipoCertificado == 0)
                        {
                            ok.LblMensaje.Text = "No se encuentra configurado el tipo de certificado digital para facturación electrónica. Comuníquese con el administrador.";
                            ok.ShowDialog();
                            return;
                        }
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se encuentra la configuración para la facturación electrónica.";
                        ok.ShowDialog();
                        return;
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCION:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "select certificado_ruta, certificado_palabra_clave, correo_smtp, correo_puerto," + Environment.NewLine;
                sSql += "correo_que_envia, correo_palabra_clave, correo_con_copia, correo_consumidor_final," + Environment.NewLine;
                sSql += "correo_ambiente_prueba, wsdl_pruebas, url_pruebas, wsdl_produccion, url_produccion," + Environment.NewLine;
                sSql += "maneja_SSL" + Environment.NewLine;
                sSql += "from cel_parametro" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.sWebServiceEnvioPruebas = dtConsulta.Rows[0]["wsdl_pruebas"].ToString();
                        Program.sWebServiceConsultaPruebas = dtConsulta.Rows[0]["url_pruebas"].ToString();
                        Program.sWebServiceEnvioProduccion = dtConsulta.Rows[0]["wsdl_produccion"].ToString();
                        Program.sWebServiceConsultaProduccion = dtConsulta.Rows[0]["url_produccion"].ToString();
                        Program.sRutaCertificado = dtConsulta.Rows[0]["certificado_ruta"].ToString();
                        Program.sClaveCertificado = dtConsulta.Rows[0]["certificado_palabra_clave"].ToString();
                        Program.sCorreoSmtp = dtConsulta.Rows[0]["correo_smtp"].ToString();
                        Program.sCorreoEmisor = dtConsulta.Rows[0]["correo_que_envia"].ToString();
                        Program.sClaveCorreoEmisor = dtConsulta.Rows[0]["correo_palabra_clave"].ToString();
                        Program.sCorreoCopia = dtConsulta.Rows[0]["correo_con_copia"].ToString();
                        Program.sCorreoConsumidorFinal = dtConsulta.Rows[0]["correo_consumidor_final"].ToString();
                        Program.sCorreoAmbientePruebas = dtConsulta.Rows[0]["correo_ambiente_prueba"].ToString();
                        Program.iCorreoPuerto = Convert.ToInt32(dtConsulta.Rows[0]["correo_puerto"].ToString());
                        Program.iManejaSSL = Convert.ToInt32(dtConsulta.Rows[0]["maneja_SSL"].ToString());
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se encuentra la configuración de parámetros para la facturación electrónica.";
                        ok.ShowDialog();
                        return;
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCION:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }
    }
}
