using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Parametros
{
    public partial class frmParametrosLocalidad : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeNuevoOk ok = new VentanasMensajes.frmMensajeNuevoOk();
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();

        Clases.ClaseCargarParametros parametros = new Clases.ClaseCargarParametros();
        DataTable dtConsulta;
        string sSql;
        bool bActualizar;
        bool bRespuesta;

        int iIdParametroLocalidad = 0;
        int iImprimirCocina = 0;
        int iManejaJornada = 0;
        int iEjecutarImpresion = 0;
        int iAbrirCajon = 0;

        int iMesas = 0;
        int iLlevar = 0;
        int iDomicilio = 0;
        int iMenuExpress = 0;
        int iCanjes = 0;
        int iCortesia = 0;
        int iFuncionarios = 0;
        int iConsumoEmpleados = 0;
        int iImprimeDatosFactura = 0;        

        double dValor;

        public frmParametrosLocalidad()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //INSTRUCCION PARA OBTENER EL PRECIO DEL PRODUCTO
        private bool obtenerValorProducto()
        {
            try
            {
                sSql = "";
                sSql += "select valor from cv403_precios_productos" + Environment.NewLine;
                sSql += "where id_producto = " + dBAyudaProducto.iId + Environment.NewLine;
                sSql += "and estado = 'A'" + Environment.NewLine;
                sSql += "and id_lista_precio = 4";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dValor = Convert.ToDouble(dtConsulta.Rows[0][0].ToString());
                        return true;                        
                    }

                    else
                    {
                        goto reversa;
                    }
                }

                else
                {
                    goto reversa;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }

        reversa:
            {
                return false;
            }        
        }

        //CARGAR DATOS DE LA LOCALIDAD
        private void llenarComboPrecuenta()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                sSql = "";
                sSql += "select id_pos_formato_precuenta, descripcion" + Environment.NewLine;
                sSql += "from pos_formato_precuenta" + Environment.NewLine;
                sSql += "where estado = 'A'";

                cmbPrecuenta.llenar(dtConsulta, sSql);
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //CARGAR DATOS DE LA LOCALIDAD
        private void llenarComboFactura()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                sSql = "";
                sSql += "select id_pos_formato_factura, descripcion" + Environment.NewLine;
                sSql += "from pos_formato_factura" + Environment.NewLine;
                sSql += "where estado = 'A'";

                cmbFactura.llenar(dtConsulta, sSql);
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //CARGAR DATOS DE LA LOCALIDAD
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad,nombre_localidad from tp_vw_localidades";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                cmbLocalidad.llenar(dtConsulta, sSql);

                if (cmbLocalidad.Items.Count > 0)
                {
                    cmbLocalidad.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //CARGAR DATOS DE LA MONEDA
        private void llenarComboMoneda()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                sSql = "select * from tp_vw_moneda";

                cmbMoneda.llenar(dtConsulta, sSql);
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LIMPIAR TODO EL FORMULARIO
        private void limpiar()
        {
            cmbLocalidad.SelectedIndexChanged -= new EventHandler(cmbLocalidad_SelectedIndexChanged);
            llenarComboLocalidad();
            cmbLocalidad.SelectedIndexChanged += new EventHandler(cmbLocalidad_SelectedIndexChanged);
            llenarComboMoneda();
            llenarComboImpresoras();
            llenarComboFactura();
            llenarComboPrecuenta();
            dBAyudaCiudad.limpiar();
            dBAyudaCajero.limpiar();
            dBAyudaMesero.limpiar();
            dBAyudaConsumidorFinal.limpiar();
            dBAyudaVendedor.limpiar();
            dBAyudaProducto.limpiar();
        }

        //FUNCION PARA LIMPIAR SIN REINICIAR
        private void limpiarReinicio()
        {
            llenarComboMoneda();
            llenarComboImpresoras();
            llenarComboFactura();
            llenarComboPrecuenta();

            dBAyudaCiudad.limpiar();
            dBAyudaCajero.limpiar();
            dBAyudaMesero.limpiar();
            dBAyudaConsumidorFinal.limpiar();
            dBAyudaVendedor.limpiar();
            dBAyudaProducto.limpiar();

            chkCocina.Checked = false;
            chkJornada.Checked = false;
            chkImprimeDatosFactura.Checked = false;
            chkEjecutarImpresiones.Checked = false;
            chkAbrirCajon.Checked = false;
            chkMesas.Checked = false;
            chkLlevar.Checked = false;
            chkDomicilio.Checked = false;
            chkRepartidoresExternos.Checked = false;
            chkCanjes.Checked = false;
            chkCortesias.Checked = false;
            chkFuncionarios.Checked = false;
            chkConsumoEmpleados.Checked = false;
        }

        //CONSULTAR EL REGISTRO 
        private void consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_parametros_localidad" + Environment.NewLine;
                sSql += "where id_localidad = " + Convert.ToInt32(cmbLocalidad.SelectedValue);

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdParametroLocalidad = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                        dBAyudaCiudad.iId = Convert.ToInt32(dtConsulta.Rows[0][2].ToString());
                        dBAyudaCiudad.txtDatos.Text = dtConsulta.Rows[0][3].ToString().Trim().ToUpper();
                        dBAyudaCiudad.txtIdentificacion.Text = dtConsulta.Rows[0][4].ToString().Trim().ToUpper();                        
                        dBAyudaCiudad.sNombre = dtConsulta.Rows[0][3].ToString();
                        dBAyudaCiudad.sCodigo = dtConsulta.Rows[0][4].ToString();

                        dBAyudaCajero.iId = Convert.ToInt32(dtConsulta.Rows[0][5].ToString());
                        dBAyudaCajero.txtDatos.Text = dtConsulta.Rows[0][6].ToString().Trim().ToUpper();
                        dBAyudaCajero.txtIdentificacion.Text = dtConsulta.Rows[0][7].ToString().Trim().ToUpper();
                        dBAyudaCajero.sNombre = dtConsulta.Rows[0][6].ToString();
                        dBAyudaCajero.sCodigo = dtConsulta.Rows[0][7].ToString();

                        dBAyudaMesero.iId = Convert.ToInt32(dtConsulta.Rows[0][8].ToString());
                        dBAyudaMesero.txtDatos.Text = dtConsulta.Rows[0][9].ToString().Trim().ToUpper();
                        dBAyudaMesero.txtIdentificacion.Text = dtConsulta.Rows[0][10].ToString().Trim().ToUpper();
                        dBAyudaMesero.sNombre = dtConsulta.Rows[0][9].ToString();
                        dBAyudaMesero.sCodigo = dtConsulta.Rows[0][10].ToString();

                        dBAyudaConsumidorFinal.iId = Convert.ToInt32(dtConsulta.Rows[0][11].ToString());
                        dBAyudaConsumidorFinal.txtDatos.Text = dtConsulta.Rows[0][12].ToString().Trim().ToUpper();
                        dBAyudaConsumidorFinal.txtIdentificacion.Text = dtConsulta.Rows[0][13].ToString().Trim().ToUpper();
                        dBAyudaConsumidorFinal.sNombre = dtConsulta.Rows[0][12].ToString();
                        dBAyudaConsumidorFinal.sCodigo = dtConsulta.Rows[0][13].ToString();

                        dBAyudaVendedor.iId = Convert.ToInt32(dtConsulta.Rows[0][14].ToString());
                        dBAyudaVendedor.txtDatos.Text = dtConsulta.Rows[0][15].ToString().Trim().ToUpper();
                        dBAyudaVendedor.txtIdentificacion.Text = dtConsulta.Rows[0][16].ToString().Trim().ToUpper();
                        dBAyudaVendedor.sNombre = dtConsulta.Rows[0][15].ToString();
                        dBAyudaVendedor.sCodigo = dtConsulta.Rows[0][16].ToString();

                        cmbMoneda.SelectedValue = dtConsulta.Rows[0][17].ToString();
                        cmbPrecuenta.SelectedValue = dtConsulta.Rows[0][18].ToString();
                        cmbFactura.SelectedValue = dtConsulta.Rows[0][19].ToString();
                        cmbImpresoras.SelectedValue = dtConsulta.Rows[0][20].ToString();

                        //CHECKBOX IMPRESION COCINA
                        if (dtConsulta.Rows[0][21].ToString() == "0")
                        {
                            chkCocina.Checked = false;
                        }
                        else
                        {
                            chkCocina.Checked = true;
                        }

                        //CHECKBOX JORNADAS
                        if (dtConsulta.Rows[0][22].ToString() == "0")
                        {
                            chkJornada.Checked = false;
                        }
                        else
                        {
                            chkJornada.Checked = true;
                        }

                        //IMPRIMIR DATOS PARA LA FACTURA
                        if (dtConsulta.Rows[0][23].ToString() == "0")
                        {
                            chkImprimeDatosFactura.Checked = false;
                        }
                        else
                        {
                            chkImprimeDatosFactura.Checked = true;
                        }
                        
                        //MESAS
                        if (dtConsulta.Rows[0][24].ToString() == "0")
                        {
                            chkMesas.Checked = false;
                        }
                        else
                        {
                            chkMesas.Checked = true;
                        }

                        //LLEVAR
                        if (dtConsulta.Rows[0][25].ToString() == "0")
                        {
                            chkLlevar.Checked = false;
                        }
                        else
                        {
                            chkLlevar.Checked = true;
                        }

                        //DOMICILIOS
                        if (dtConsulta.Rows[0][26].ToString() == "0")
                        {
                            chkDomicilio.Checked = false;
                        }
                        else
                        {
                            chkDomicilio.Checked = true;
                        }

                        //MENU EXPRESS
                        if (dtConsulta.Rows[0][27].ToString() == "0")
                        {
                            chkRepartidoresExternos.Checked = false;
                        }
                        else
                        {
                            chkRepartidoresExternos.Checked = true;
                        }

                        //CORTESIAS
                        if (dtConsulta.Rows[0][28].ToString() == "0")
                        {
                            chkCortesias.Checked = false;
                        }
                        else
                        {
                            chkCortesias.Checked = true;
                        }

                        //CANJES
                        if (dtConsulta.Rows[0][29].ToString() == "0")
                        {
                            chkCanjes.Checked = false;
                        }
                        else
                        {
                            chkCanjes.Checked = true;
                        }

                        //VALE FUNCIONARIO
                        if (dtConsulta.Rows[0][30].ToString() == "0")
                        {
                            chkFuncionarios.Checked = false;
                        }
                        else
                        {
                            chkFuncionarios.Checked = true;
                        }

                        //CONSUMO EMPLEADOS
                        if (dtConsulta.Rows[0][31].ToString() == "0")
                        {
                            chkConsumoEmpleados.Checked = false;
                        }
                        else
                        {
                            chkConsumoEmpleados.Checked = true;
                        }

                        dBAyudaProducto.iId = Convert.ToInt32(dtConsulta.Rows[0][32].ToString());                        
                        dBAyudaProducto.sNombre = dtConsulta.Rows[0][33].ToString();
                        dBAyudaProducto.sCodigo = dtConsulta.Rows[0][34].ToString();
                        dBAyudaProducto.txtIdentificacion.Text = dtConsulta.Rows[0][33].ToString();
                        dBAyudaProducto.txtDatos.Text = dtConsulta.Rows[0][34].ToString();

                        //EJECUTAR IMPRESION
                        if (dtConsulta.Rows[0][35].ToString() == "0")
                        {
                            chkEjecutarImpresiones.Checked = false;
                        }
                        else
                        {
                            chkEjecutarImpresiones.Checked = true;
                        }

                        //PERMITIR ABRIR EL CAJÓN DE DINERO
                        if (dtConsulta.Rows[0][36].ToString() == "0")
                        {
                            chkAbrirCajon.Checked = false;
                        }
                        else
                        {
                            chkAbrirCajon.Checked = true;
                        }
                        
                        bActualizar = true;
                    }

                    else
                    {
                        bActualizar = false;
                        limpiarReinicio();
                    }
                }

                else
                {
                    catchMensaje.lblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //CONSULTAR EL PRODUCTO PREDETERMINADO
        private void consultarProducto(int iIdProducto)
        {
            try
            {
                sSql = "";
                sSql += "select P.codigo, NP.nombre" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_nombre_productos NP" + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and P.id_producto = " + iIdProducto;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dBAyudaProducto.sCodigo = dtConsulta.Rows[0][0].ToString();
                        dBAyudaProducto.txtIdentificacion.Text = dtConsulta.Rows[0][0].ToString();
                        dBAyudaProducto.txtDatos.Text = dtConsulta.Rows[0][1].ToString();
                    }
                }

                else
                {
                    catchMensaje.lblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }
        
        //FUNCION PARA INSERTAR EL REGISTRO
        private void insertarRegistro()
        {
            try
            {
                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.lblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    limpiar();
                    return;
                }

                //INSTRUCCION PARA INSERTAR
                sSql = "";
                sSql += "insert into pos_parametro_localidad (" + Environment.NewLine;
                sSql += "id_localidad, cg_ciudad, id_pos_cajero, id_pos_mesero," + Environment.NewLine;
                sSql += "cg_moneda, id_pos_formato_precuenta, id_pos_formato_factura," + Environment.NewLine;
                sSql += "imprimir_cocina, consumidor_final, id_vendedor, maneja_jornada," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso, " + Environment.NewLine;
                sSql += "maneja_mesas, maneja_llevar, maneja_domicilio, maneja_menu_express," + Environment.NewLine;
                sSql += "maneja_cortesia, maneja_canjes, maneja_vale_funcionario," + Environment.NewLine;
                sSql += "maneja_consumo_empleados, imprimir_datos_factura," + Environment.NewLine;
                sSql += "id_producto_anula, valor_precio_anula, id_pos_impresora, ejecutar_impresion," + Environment.NewLine;
                sSql += "permitir_abrir_cajon)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Convert.ToInt32(cmbLocalidad.SelectedValue) + ", " + dBAyudaCiudad.iId + "," + Environment.NewLine;
                sSql += dBAyudaCajero.iId + ", " + dBAyudaMesero.iId + ", " + Convert.ToInt32(cmbMoneda.SelectedValue) + "," + Environment.NewLine;
                sSql += Convert.ToInt32(cmbPrecuenta.SelectedValue) + ", " + Convert.ToInt32(cmbFactura.SelectedValue) + "," + Environment.NewLine;
                sSql += iImprimirCocina + ", " + dBAyudaConsumidorFinal.iId + ", " + dBAyudaVendedor.iId + ", " + iManejaJornada + "," + Environment.NewLine;
                sSql += "'A', GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', " + iMesas + ", " + iLlevar + "," + Environment.NewLine;
                sSql += iDomicilio + ", " + iMenuExpress + ", " + iCortesia + ", " + iCanjes + "," + Environment.NewLine;
                sSql += iFuncionarios + ", " + iConsumoEmpleados + ", " + iImprimeDatosFactura + "," + Environment.NewLine;
                sSql += dBAyudaProducto.iId + ", " + dValor + ", " + Convert.ToInt32(cmbImpresoras.SelectedValue) + "," + Environment.NewLine;
                sSql += iEjecutarImpresion + ", " + iAbrirCajon + ")";

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.lblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //SI SE EJECUTA TODO REALIZA EL COMMIT
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.lblMensaje.Text = "Registro ingresado éxitosamente. Los cambios se aplicarán al reiniciar el programa.";
                parametros.cargarParametrosPredeterminados();
                ok.ShowDialog();
                limpiar();
                consultarRegistro();
                return;
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return; }
        }

        //FUNCION PARA ACTUALIZAR EL REGISTRO
        private void actualizarRegistro()
        {
            try
            {                
                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.lblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    limpiar();
                    return;
                }

                //INSTRUCCION PARA ACTUALIZAR
                sSql = "";
                sSql += "update pos_parametro_localidad set" + Environment.NewLine;
                sSql += "id_localidad = " + Convert.ToInt32(cmbLocalidad.SelectedValue) + "," + Environment.NewLine;
                sSql += "cg_ciudad = " + dBAyudaCiudad.iId + "," + Environment.NewLine;
                sSql += "id_pos_cajero = " + dBAyudaCajero.iId + "," + Environment.NewLine;
                sSql += "id_pos_mesero = " + dBAyudaMesero.iId + "," + Environment.NewLine;
                sSql += "cg_moneda = " + Convert.ToInt32(cmbMoneda.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_pos_formato_precuenta = " + Convert.ToInt32(cmbPrecuenta.SelectedValue) + "," + Environment.NewLine;
                sSql += "id_pos_formato_factura = " + Convert.ToInt32(cmbFactura.SelectedValue) + "," + Environment.NewLine;
                sSql += "imprimir_cocina = " + iImprimirCocina + "," + Environment.NewLine;
                sSql += "consumidor_final = " + dBAyudaConsumidorFinal.iId + "," + Environment.NewLine;
                sSql += "id_vendedor = " + dBAyudaVendedor.iId + "," + Environment.NewLine;
                sSql += "maneja_jornada = " + iManejaJornada + "," + Environment.NewLine;
                sSql += "maneja_mesas = " + iMesas + "," + Environment.NewLine;
                sSql += "maneja_llevar = " + iLlevar + "," + Environment.NewLine;
                sSql += "maneja_domicilio = " + iDomicilio + "," + Environment.NewLine;
                sSql += "maneja_menu_express = " + iMenuExpress + "," + Environment.NewLine;
                sSql += "maneja_canjes = " + iCanjes + "," + Environment.NewLine;
                sSql += "maneja_cortesia = " + iCortesia + "," + Environment.NewLine;
                sSql += "maneja_vale_funcionario = " + iFuncionarios + "," + Environment.NewLine;
                sSql += "maneja_consumo_empleados = " + iConsumoEmpleados + "," + Environment.NewLine;
                sSql += "imprimir_datos_factura = " + iImprimeDatosFactura + "," + Environment.NewLine;
                sSql += "id_producto_anula = " + dBAyudaProducto.iId + "," + Environment.NewLine;
                sSql += "valor_precio_anula = " + dValor + "," + Environment.NewLine;
                sSql += "id_pos_impresora = " + Convert.ToInt32(cmbImpresoras.SelectedValue) + "," + Environment.NewLine;
                sSql += "ejecutar_impresion = " + iEjecutarImpresion + "," + Environment.NewLine;
                sSql += "permitir_abrir_cajon = " + iAbrirCajon + Environment.NewLine;
                sSql += "where id_pos_parametro_localidad = " + iIdParametroLocalidad + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.lblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //SI SE EJECUTA TODO REALIZA EL COMMIT
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.lblMensaje.Text = "Registro actualizado éxitosamente. Los cambios se aplicarán al reiniciar el programa.";
                parametros.cargarParametrosPredeterminados();
                ok.ShowDialog();
                limpiar();
                consultarRegistro();
                return;
            }

            catch (Exception)
            {
                ok.lblMensaje.Text = "Ocurrió un problema al actualizar el registro.";
                ok.ShowDialog();
                goto reversa;
            }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return; }
        }

        //LLENAR COMBO DE IMPRESORAS
        private void llenarComboImpresoras()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_impresora," + Environment.NewLine;
                sSql += "descripcion + ' - (' + path_url + ')' as impresora" + Environment.NewLine;
                sSql += "from pos_impresora" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                cmbImpresoras.llenar(dtConsulta, sSql);

                if (cmbImpresoras.Items.Count > 0)
                {
                    cmbImpresoras.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION  PARA CARGAR LOS CONTROLES DB AYUDA
        private void cargarDbAyuda()
        {
            try
            {
                //DBAYUDA CIUDADES
                sSql = "";
                sSql += "select * from tp_vw_ciudad";
                dBAyudaCiudad.Ver(sSql, "valor_texto", 0, 2, 1);

                //DBAYUDA CAJEROS
                sSql = "";
                sSql += "select id_pos_cajero, descripcion as 'DESCRIPCION', codigo as 'CODIGO'" + Environment.NewLine;
                sSql += "from pos_cajero" + Environment.NewLine;
                sSql += "where estado = 'A'";
                dBAyudaCajero.Ver(sSql, "descripcion", 0, 2, 1);

                //DBAYUDA MESEROS
                sSql = "";
                sSql += "select id_pos_mesero, descripcion as 'DESCRIPCION', codigo as 'CODIGO'" + Environment.NewLine;
                sSql += "from pos_mesero" + Environment.NewLine;
                sSql += "where estado = 'A'";
                dBAyudaMesero.Ver(sSql, "descripcion", 0, 2, 1);

                //DBAYUDA PERSONAS
                sSql = "";
                sSql += "select id_persona, apellidos + ' ' + isnull(nombres, '') nombre, identificacion" + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'";
                dBAyudaConsumidorFinal.Ver(sSql, "identificacion", 0, 2, 1);

                //DBAYUDA VENDEDORES
                sSql = "";
                sSql += "select id_vendedor, descripcion as 'DESCRIPCION', codigo as 'CODIGO'" + Environment.NewLine;
                sSql += "from cv403_vendedores" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                dBAyudaVendedor.Ver(sSql, "descripcion", 0, 2, 1);

                //DBAYUDA PRODUCTO ANULADO
                sSql = "";
                sSql += "select P.id_producto, P.codigo, NP.nombre, PP.valor" + Environment.NewLine;
                sSql += "from cv401_productos P, cv401_nombre_productos NP, cv403_precios_productos PP" + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and PP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "and P.nivel = 3" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = 4" + Environment.NewLine;
                sSql += "order by NP.nombre";
                dBAyudaProducto.Ver(sSql, "NP.nombre", 0, 1, 2);
            }

            catch(Exception ex)
            {
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void frmParametrosLocalidad_Load(object sender, EventArgs e)
        {
            cmbLocalidad.SelectedIndexChanged -= new EventHandler(cmbLocalidad_SelectedIndexChanged);
            llenarComboLocalidad();
            cmbLocalidad.SelectedIndexChanged += new EventHandler(cmbLocalidad_SelectedIndexChanged);
            cargarDbAyuda();
            llenarComboMoneda();
            llenarComboPrecuenta();
            llenarComboFactura();
            llenarComboImpresoras();
            consultarRegistro();
        }

        private void BtnLimpiarCiudad_Click(object sender, EventArgs e)
        {
            dBAyudaCiudad.limpiar();
        }

        private void BtnLimpiarCajero_Click(object sender, EventArgs e)
        {
            dBAyudaCajero.limpiar();
        }

        private void BtnLimpiarMesero_Click(object sender, EventArgs e)
        {
            dBAyudaMesero.limpiar();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (cmbLocalidad.SelectedValue.ToString() == "0")
            {
                ok.lblMensaje.Text = "Favor seleccione la localidad.";
                ok.ShowDialog();
            }            

            else if (dBAyudaCiudad.iId == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione la ciudad predeterminada para la localidad.";
                ok.ShowDialog();
                dBAyudaCiudad.Focus();
            }

            else if (dBAyudaCajero.iId == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione el cajero predeterminado para la localidad.";
                ok.ShowDialog();
                dBAyudaCajero.Focus();
            }

            else if (dBAyudaMesero.iId == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione el mesero predeterminado para la localidad.";
                ok.ShowDialog();
                dBAyudaMesero.Focus();
            }
            
            else if (dBAyudaConsumidorFinal.iId == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione el valor predeterminado para consumidor final de la localidad.";
                ok.ShowDialog();
                dBAyudaConsumidorFinal.Focus();
            }

            else if (dBAyudaVendedor.iId == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione el vendedor para la localidad.";
                ok.ShowDialog();
                dBAyudaVendedor.Focus();
            }

            else if (Convert.ToInt32(cmbMoneda.SelectedValue) == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione el tipo de moneda para la localidad.";
                ok.ShowDialog();
                cmbMoneda.Focus();
            }

            else if (Convert.ToInt32(cmbPrecuenta.SelectedValue) == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione el tipo de formato de precuenta para la localidad.";
                ok.ShowDialog();
                cmbPrecuenta.Focus();
            }

            else if (Convert.ToInt32(cmbFactura.SelectedValue) == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione el tipo de formato de factura para la localidad.";
                ok.ShowDialog();
                cmbFactura.Focus();
            }

            else if (dBAyudaProducto.iId == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione un producto para ingresar como anulación.";
                ok.ShowDialog();
            }

            else if (Convert.ToInt32(cmbImpresoras.SelectedValue) == 0)
            {
                ok.lblMensaje.Text = "Favor seleccione una impresora para la impresión de reportes.";
                ok.ShowDialog();
                cmbImpresoras.Focus();
            }

            else
            {
                if (chkCocina.Checked == true)
                {
                    iImprimirCocina = 1;
                }

                else
                {
                    iImprimirCocina = 0;
                }

                if (chkJornada.Checked == true)
                {
                    iManejaJornada = 1;
                }

                else
                {
                    iManejaJornada = 0;
                }

                //MESAS
                if (chkMesas.Checked == true)
                {
                    iMesas = 1;
                }

                else
                {
                    iMesas = 0;
                }

                //PARA LLEVAR
                if (chkLlevar.Checked == true)
                {
                    iLlevar = 1;
                }

                else
                {
                    iLlevar = 0;
                }

                //DOMICILIOS
                if (chkDomicilio.Checked == true)
                {
                    iDomicilio = 1;
                }

                else
                {
                    iDomicilio = 0;
                }

                //MENU EXPRESS
                if (chkRepartidoresExternos.Checked == true)
                {
                    iMenuExpress = 1;
                }

                else
                {
                    iMenuExpress = 0;
                }

                //CANJES
                if (chkCanjes.Checked == true)
                {
                    iCanjes = 1;
                }

                else
                {
                    iCanjes = 0;
                }

                //CORTESIAS
                if (chkCortesias.Checked == true)
                {
                    iCortesia = 1;
                }

                else
                {
                    iCortesia = 0;
                }

                //VALE FUNCIONARIOS
                if (chkFuncionarios.Checked == true)
                {
                    iFuncionarios = 1;
                }

                else
                {
                    iFuncionarios = 0;
                }

                //CONSUMO EMPLEADOS
                if (chkConsumoEmpleados.Checked == true)
                {
                    iConsumoEmpleados = 1;
                }

                else
                {
                    iConsumoEmpleados = 0;
                }

                //IMPRIMIR DATOS PARA LA FACTURA
                if (chkImprimeDatosFactura.Checked == true)
                {
                    iImprimeDatosFactura = 1;
                }

                else
                {
                    iImprimeDatosFactura = 0;
                }

                //EJECUTAR IMPRESIONES
                if (chkEjecutarImpresiones.Checked == true)
                {
                    iEjecutarImpresion = 1;
                }

                else
                {
                    iEjecutarImpresion = 0;
                }

                //PERMITIR ABRIR EL CAJON
                if (chkAbrirCajon.Checked == true)
                {
                    iAbrirCajon = 1;
                }

                else
                {
                    iAbrirCajon = 0;
                }

                //PROCESO PARA INSERTAR O ACTUALIZAR
                if (bActualizar == true)
                {
                    if (obtenerValorProducto() == true)
                    {
                        actualizarRegistro();
                    }

                    else
                    {
                        ok.lblMensaje.Text = "No se pudo actualizar el registro. Error al obtener el valor del producto.";
                        ok.ShowDialog();
                    }
                }

                else
                {
                    if (obtenerValorProducto() == true)
                    {
                        insertarRegistro();
                    }

                    else
                    {
                        ok.lblMensaje.Text = "No se pudo insertar el registro. Error al obtener el valor del producto.";
                        ok.ShowDialog();
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void cmbLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbLocalidad.SelectedValue) == 0)
            {
                ok.lblMensaje.Text = "El registro seleccionado no contiene información.";
                ok.ShowDialog();
                cmbLocalidad.Focus();
            }

            else
            {
                consultarRegistro();
            }
        }

        private void btnEliminarCF_Click(object sender, EventArgs e)
        {
            dBAyudaConsumidorFinal.limpiar();
        }

        private void btnLimpiarVendedor_Click(object sender, EventArgs e)
        {
            dBAyudaVendedor.limpiar();
        }
    }
}
