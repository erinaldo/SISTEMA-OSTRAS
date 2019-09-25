using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Comida_Rapida
{
    public partial class frmComandaComidaRapida : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();

        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();

        string sSql;
        string sPagaIva_P;
        string sNombreProducto_P;
        string sFecha;
        string sTabla;
        string sCampo;
        string sSecuencial;
        string sMovimiento;
        string sDescripcionFormaPago;
        string sEstablecimiento;
        string sPuntoEmision;

        long iMaximo;

        DataTable dtConsulta;
        DataTable dtCategorias;
        DataTable dtProductos;
        DataTable dtRecargos;

        bool bRespuesta;

        Button[,] botonFamilias = new Button[2, 4];
        Button[,] botonProductos = new Button[5, 5];

        Button botonSeleccionadoCategoria;
        Button botonSeleccionadoProducto;

        int iCuentaCategorias;
        int iPosXCategorias;
        int iPosYCategorias;
        int iCuentaAyudaCategorias;
        int iCuentaProductos;
        int iPosXProductos;
        int iPosYProductos;
        int iCuentaAyudaProductos;

        int iCuentaDiaria;
        int iIdPersona;
        int iIdOrigenOrden;
        int iIdPedido;
        int iNumeroPedidoOrden;
        int iIdCabDespachos;
        int iIdDespachoPedido;
        int iIdEventoCobro;
        int iCgTipoDocumento = 2725;
        int iIdProducto_P;
        int iIdDocumentoCobrar;
        int iIdPago;
        int iNumeroPago;
        int iIdTipoFormaCobro;
        int iCgTipoDocumentoCobro;
        int iIdTipoComprobante;
        int iIdFormaPago_1;
        int iIdFactura;
        int iCgEstadoDctoPorCobrar = 7461;
        int iIdCaja;
        int iIdDocumentoPago;
        int iIdPosMovimientoCaja;
        int iNumeroMovimientoCaja;
        int iBanderaEfectivoTarjeta;
        int iBanderaAplicaRecargo;
        int iBanderaExpressTarjeta;

        Decimal dIVA_P;
        Decimal dPrecioUnitario_P;
        Decimal dCantidad_P;
        Decimal dTotalDebido;
        Decimal dbCantidadRecalcular;
        Decimal dbPrecioRecalcular;
        Decimal dbValorTotalRecalcular;
        Decimal dbSubtotalRecalcular;
        Decimal dbValorIVA;
        Decimal dbValorGrid;
        Decimal dbValorRecuperado;
        Decimal dbCambio;

        public frmComandaComidaRapida(int iIdPosOrigenOrden_P, int iBanderaExpressTarjeta_P)
        {
            this.iIdOrigenOrden = iIdPosOrigenOrden_P;
            this.iBanderaExpressTarjeta = iBanderaExpressTarjeta_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CARGAR LAS CATEGORIAS
        private void cargarCategorias()
        {
            try
            {
                sSql = "";
                sSql += "select P.id_Producto, NP.nombre as Nombre, P.paga_iva," + Environment.NewLine;
                sSql += "P.subcategoria" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_Producto = NP.id_Producto" + Environment.NewLine;
                sSql += "and P.estado ='A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "where P.nivel = 2" + Environment.NewLine;
                sSql += "and P.maneja_almuerzos = 1" + Environment.NewLine;
                sSql += "order by P.secuencia";

                dtCategorias = new DataTable();
                dtCategorias.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtCategorias, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN SQL:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }

                iCuentaCategorias = 0;

                if (dtCategorias.Rows.Count > 0)
                {
                    if (dtCategorias.Rows.Count > 8)
                    {
                        btnSiguiente.Enabled = true;
                        btnAnterior.Visible = true;
                        btnSiguiente.Visible = true;
                    }

                    else
                    {
                        btnSiguiente.Enabled = false;
                        btnAnterior.Visible = false;
                        btnSiguiente.Visible = false;
                    }

                    if (crearBotonesCategorias() == false)
                    { }

                }

                else
                {
                    ok.LblMensaje.Text = "No se encuentras ítems de categorías en el sistema.";
                    ok.ShowDialog();
                }

            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CREAR LOS BOTONES DE CATEGORIAS
        private bool crearBotonesCategorias()
        {
            try
            {
                pnlCategorias.Controls.Clear();

                iPosXCategorias = 0;
                iPosYCategorias = 0;
                iCuentaAyudaCategorias = 0;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        botonFamilias[i, j] = new Button();
                        botonFamilias[i, j].Cursor = Cursors.Hand;
                        botonFamilias[i, j].Click += new EventHandler(boton_clic_categorias);
                        botonFamilias[i, j].Size = new Size(130, 71);
                        botonFamilias[i, j].Location = new Point(iPosXCategorias, iPosYCategorias);
                        botonFamilias[i, j].BackColor = Color.Lime;
                        botonFamilias[i, j].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                        botonFamilias[i, j].Tag = dtCategorias.Rows[iCuentaCategorias]["id_producto"].ToString();
                        botonFamilias[i, j].Text = dtCategorias.Rows[iCuentaCategorias]["nombre"].ToString();
                        botonFamilias[i, j].AccessibleDescription = dtCategorias.Rows[iCuentaCategorias]["subcategoria"].ToString();
                        pnlCategorias.Controls.Add(botonFamilias[i, j]);

                        iCuentaCategorias++;
                        iCuentaAyudaCategorias++;

                        if (j + 1 == 4)
                        {
                            iPosXCategorias = 0;
                            iPosYCategorias += 71;
                        }

                        else
                        {
                            iPosXCategorias += 130;
                        }

                        if (dtCategorias.Rows.Count == iCuentaCategorias)
                        {
                            btnSiguiente.Enabled = false;
                            break;
                        }
                    }

                    if (dtCategorias.Rows.Count == iCuentaCategorias)
                    {
                        btnSiguiente.Enabled = false;
                        break;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //EVENTO CLIC DEL BOTON CATEGORIAS
        private void boton_clic_categorias(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                botonSeleccionadoCategoria = sender as Button;

                lblProductos.Text = botonSeleccionadoCategoria.Text.Trim().ToUpper();

                if (Convert.ToInt32(botonSeleccionadoCategoria.AccessibleDescription) == 0)
                {
                    cargarProductos(Convert.ToInt32(botonSeleccionadoCategoria.Tag), 3);
                }

                else
                {
                    cargarProductos(Convert.ToInt32(botonSeleccionadoCategoria.Tag), 4);
                }

                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CARGAR LOS PRODUCTOS
        private void cargarProductos(int iIdProducto_P, int iNivel_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_Producto, NP.nombre as Nombre, P.paga_iva, PP.valor" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_Producto = NP.id_Producto" + Environment.NewLine;
                sSql += "and P.estado ='A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "cv403_precios_productos PP ON P.id_producto = PP.id_producto" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "where P.nivel = " + iNivel_P + Environment.NewLine;
                sSql += "and PP.id_lista_precio = 4" + Environment.NewLine;
                sSql += "and P.id_producto_padre = " + iIdProducto_P + Environment.NewLine;
                sSql += "order by P.secuencia";

                dtProductos = new DataTable();
                dtProductos.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtProductos, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN SQL:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }

                iCuentaProductos = 0;

                if (dtProductos.Rows.Count > 0)
                {
                    if (dtProductos.Rows.Count > 25)
                    {
                        btnSiguienteProducto.Enabled = true;
                        btnSiguienteProducto.Visible = true;
                        btnAnteriorProducto.Visible = true;
                    }
                    else
                    {
                        btnSiguienteProducto.Enabled = false;
                        btnSiguienteProducto.Visible = false;
                        btnAnteriorProducto.Visible = false;
                    }
                    if (crearBotonesProductos() == false)
                    { }
                }

                else
                {
                    ok.LblMensaje.Text = "No se encuentras ítems de categorías en el sistema.";
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CREAR LOS BOTONES DE PRODUCTOS
        private bool crearBotonesProductos()
        {
            try
            {
                pnlProductos.Controls.Clear();

                iPosXProductos = 0;
                iPosYProductos = 0;
                iCuentaAyudaProductos = 0;

                for (int i = 0; i < 5; ++i)
                {
                    for (int j = 0; j < 5; ++j)
                    {
                        botonProductos[i, j] = new Button();
                        botonProductos[i, j].Cursor = Cursors.Hand;
                        botonProductos[i, j].Click += new EventHandler(boton_clic_productos);
                        botonProductos[i, j].Size = new Size(130, 71);
                        botonProductos[i, j].Location = new Point(iPosXProductos, iPosYProductos);
                        botonProductos[i, j].BackColor = Color.FromArgb(255, 255, 128);
                        botonProductos[i, j].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                        botonProductos[i, j].Tag = dtProductos.Rows[iCuentaProductos]["id_producto"].ToString();
                        botonProductos[i, j].Text = dtProductos.Rows[iCuentaProductos]["nombre"].ToString();
                        botonProductos[i, j].AccessibleDescription = dtProductos.Rows[iCuentaProductos]["paga_iva"].ToString();
                        botonProductos[i, j].AccessibleName = dtProductos.Rows[iCuentaProductos]["valor"].ToString();
                        pnlProductos.Controls.Add(botonProductos[i, j]);

                        iCuentaProductos++;
                        iCuentaAyudaProductos++;

                        if (j + 1 == 5)
                        {
                            iPosXProductos = 0;
                            iPosYProductos += 71;
                        }

                        else
                        {
                            iPosXProductos += 130;
                        }

                        if (dtProductos.Rows.Count == iCuentaProductos)
                        {
                            btnSiguienteProducto.Enabled = false;
                            break;
                        }
                    }

                    if (dtProductos.Rows.Count == iCuentaProductos)
                    {
                        btnSiguienteProducto.Enabled = false;
                        break;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //EVENTO CLIC DE LOS BOTONES DE PRODUCTOS
        private void boton_clic_productos(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                botonSeleccionadoProducto = sender as Button;

                int iExiste_R = 0;

                Decimal num2 = 0;
                Decimal num3;

                Decimal dbCantidad_R;
                Decimal dbValorUnitario_R;
                Decimal dbSubtotal_R;
                Decimal dbValorIVA_R;
                Decimal dbTotal_R;


                for (int i = 0; i < dgvPedido.Rows.Count; ++i)
                {
                    if (dgvPedido.Rows[i].Cells["idProducto"].Value.ToString() == botonSeleccionadoProducto.Tag.ToString())
                    {
                        dbCantidad_R = Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value);
                        dbCantidad_R += 1;
                        dgvPedido.Rows[i].Cells["cantidad"].Value = dbCantidad_R;
                        dbValorUnitario_R = Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value);
                        dbSubtotal_R = dbCantidad_R * dbValorUnitario_R;
                        dgvPedido.Rows[i].Cells["subtotal"].Value = dbSubtotal_R.ToString("N2");
                        dbValorIVA_R = dbSubtotal_R * Convert.ToDecimal(Program.iva);
                        dbTotal_R = dbSubtotal_R + dbValorIVA_R;
                        dgvPedido.Rows[i].Cells["valor"].Value = dbTotal_R.ToString("N2");
                        iExiste_R = 1;
                    }
                }

                if (iExiste_R == 0)
                {
                    int i = dgvPedido.Rows.Add();

                    dgvPedido.Rows[i].Cells["cantidad"].Value = "1";
                    dgvPedido.Rows[i].Cells["producto"].Value = botonSeleccionadoProducto.Text.ToString().Trim();
                    sNombreProducto_P = botonSeleccionadoProducto.Text.ToString().Trim();
                    dgvPedido.Rows[i].Cells["idProducto"].Value = botonSeleccionadoProducto.Tag;
                    sPagaIva_P = botonSeleccionadoProducto.AccessibleDescription.ToString().Trim();
                    dgvPedido.Rows[i].Cells["pagaIva"].Value = sPagaIva_P;

                    if (sPagaIva_P == "1")
                    {
                        dgvPedido.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                        dgvPedido.Rows[i].Cells["cantidad"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " PAGA IVA";
                        dgvPedido.Rows[i].Cells["producto"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " PAGA IVA";
                        dgvPedido.Rows[i].Cells["valor"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " PAGA IVA";
                    }
                    else
                    {
                        dgvPedido.Rows[i].DefaultCellStyle.ForeColor = Color.Purple;
                        dgvPedido.Rows[i].Cells["cantidad"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " NO PAGA IVA";
                        dgvPedido.Rows[i].Cells["producto"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " NO PAGA IVA";
                        dgvPedido.Rows[i].Cells["valor"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " NO PAGA IVA";
                    }

                    dbCantidad_R = 1;
                    dgvPedido.Rows[i].Cells["valuni"].Value = botonSeleccionadoProducto.AccessibleName;
                    dbValorUnitario_R = Convert.ToDecimal(botonSeleccionadoProducto.AccessibleName);
                    dbSubtotal_R = dbCantidad_R * dbValorUnitario_R;
                    dgvPedido.Rows[i].Cells["subtotal"].Value = dbSubtotal_R.ToString("N2");
                    dbValorIVA_R = dbSubtotal_R * Convert.ToDecimal(Program.iva);
                    dbTotal_R = dbSubtotal_R + dbValorIVA_R;
                    dgvPedido.Rows[i].Cells["valor"].Value = dbTotal_R.ToString("N2");
                }

                calcularTotales();
                dgvPedido.ClearSelection();

                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CALCULAR TOTALES
        public void calcularTotales()
        {
            Decimal dSubtotalConIva = 0;
            Decimal dSubtotalCero = 0;
            Decimal dbValorIva;
            Decimal dbSumaIva = 0;
            dTotalDebido = 0;

            for (int i = 0; i < dgvPedido.Rows.Count; ++i)
            {
                if (dgvPedido.Rows[i].Cells["pagaIva"].Value.ToString() == "0")
                {
                    dSubtotalCero += Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value.ToString()) * Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value.ToString());
                }

                else
                {
                    dSubtotalConIva += Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value.ToString()) * Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value.ToString());
                    dbValorIva = Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value.ToString()) * Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value.ToString()) * Convert.ToDecimal(Program.iva);
                    dbSumaIva += dbValorIva;
                }
            }

            //dTotalDebido = num1 + num2 - num3 - num4 + (num1 - num3) * Convert.ToDecimal(Program.iva) + num7;
            dTotalDebido = dSubtotalConIva + dSubtotalCero + dbSumaIva;
            lblTotal.Text = "$ " + dTotalDebido.ToString("N2");
        }

        #endregion

        #region FUNCIONES PARA GUARDAR LA COMANDA

        //FUNCION PARA OBTENER LA ID LOCALIDAD
        private void datosFactura()
        {
            try
            {
                sSql = "";
                sSql += "select L.id_localidad, L.establecimiento, L.punto_emision, P.numeronotaentrega" + Environment.NewLine;
                sSql += "from tp_localidades L, tp_localidades_impresoras P " + Environment.NewLine;
                sSql += "where L.id_localidad = P.id_localidad" + Environment.NewLine;
                sSql += "and L.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and L.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count == 0)
                    {
                        ok.LblMensaje.Text = "No se encuentran registros en la consulta.";
                        ok.ShowDialog();
                    }

                    else
                    {
                        txtfacturacion.Text = dtConsulta.Rows[0]["establecimiento"].ToString() + "-" + dtConsulta.Rows[0]["punto_emision"].ToString();

                        sEstablecimiento = dtConsulta.Rows[0]["establecimiento"].ToString();
                        sPuntoEmision = dtConsulta.Rows[0]["punto_emision"].ToString();

                        TxtNumeroFactura.Text = dtConsulta.Rows[0]["numeronotaentrega"].ToString();
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

        //FUNCION PARA EXTRAER EL NUMERO DE CUENTA
        private void extraerNumeroCuenta()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(max(cuenta), 0) cuenta" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    iCuentaDiaria = Convert.ToInt32(dtConsulta.Rows[0][0].ToString()) + 1;
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

        //SECUENCIA DE INSERCION DE DATOS Y CERRAR 
        private void crearComanda()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                sFecha = Program.sFechaSistema.ToString("yyyy/MM/dd");
                iIdPersona = Program.iIdPersona;
                iIdTipoComprobante = Program.iComprobanteNotaEntrega;

                extraerNumeroCuenta();

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    return;
                }

                if (insertarPedido() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return;
                }

                if (insertarPagos() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return;
                }

                if (insertarFactura() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                crearReporte();

            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //INSERTAR LA PRIMERA FASE - PEDIDO
        private bool insertarPedido()
        {
            try
            {
                string sEstadoOrden;

                if (iBanderaExpressTarjeta == 1)
                {
                    sEstadoOrden = "Cerrada";
                }

                else
                {
                    sEstadoOrden = "Pagada";
                }

                //INSERTAR EN LA TABLA CV403_CAB_PEDIDOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_cab_pedidos(" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_localidad, fecha_pedido, id_persona, " + Environment.NewLine;
                sSql += "cg_tipo_cliente, cg_moneda, porcentaje_iva, id_vendedor, cg_estado_pedido, porcentaje_dscto, " + Environment.NewLine;
                sSql += "cg_facturado, fecha_ingreso, usuario_ingreso, terminal_ingreso, cuenta, id_pos_mesa, id_pos_cajero, " + Environment.NewLine;
                sSql += "id_pos_origen_orden, id_pos_orden_dividida, id_pos_jornada, fecha_orden, fecha_apertura_orden, " + Environment.NewLine;
                sSql += "fecha_cierre_orden, estado_orden, numero_personas, origen_dato, numero_replica_trigger, " + Environment.NewLine;
                sSql += "estado_replica, numero_control_replica, estado, idtipoestablecimiento, comentarios, id_pos_modo_delivery," + Environment.NewLine;
                sSql += "id_pos_mesero, id_pos_terminal, porcentaje_servicio, consumo_alimentos) " + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Program.iIdEmpresa + "," + Program.iCgEmpresa + "," + Program.iIdLocalidad + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + iIdPersona + ",8032," + Program.iMoneda + "," + Environment.NewLine;
                sSql += (Program.iva * 100) + "," + Program.iIdVendedor + ",6967, 0, 7471," + Environment.NewLine;
                sSql += "GETDATE(),'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "'," + iCuentaDiaria + ", 0, ";
                sSql += Program.iIdCajeroDefault + "," + iIdOrigenOrden + ", 0," + Program.iJORNADA + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', GETDATE(), GETDATE(), '" + sEstadoOrden + "'," + Environment.NewLine;
                sSql += "1, 1, 1, 0, 0, 'A', 1, null, null," + Environment.NewLine;
                sSql += Program.iIdMesero + ", " + Program.iIdTerminal + ", " + (Program.servicio * 100) + ", 0)";

                Program.iBanderaCliente = 0;

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //INSERTAR EN LA TABLA CV403_CAB_DESPACHOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_cab_despachos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, cg_empresa, id_localidad, fecha_despacho," + Environment.NewLine;
                sSql += "cg_motivo_despacho, id_destinatario, punto_partida, cg_ciudad_entrega," + Environment.NewLine;
                sSql += "direccion_entrega, id_transportador, fecha_inicio_transporte," + Environment.NewLine;
                sSql += "fecha_fin_transporte, cg_estado_despacho, punto_venta, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Program.iIdEmpresa + ", " + iIdPersona + ", " + Program.iCgEmpresa + ", " + Program.iIdLocalidad + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Program.iCgMotivoDespacho + ", " + iIdPersona + "," + Environment.NewLine;
                sSql += "'" + Program.sPuntoPartida + "', " + Program.iCgCiudadEntrega + ", '" + Program.sDireccionEntrega + "'," + Environment.NewLine;
                sSql += "'" + Program.iIdPersona + "', '" + sFecha + "', '" + sFecha + "', " + Program.iCgEstadoDespacho + "," + Environment.NewLine;
                sSql += "1, GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //OBTENER EL ID DE LA TABLA CV403_CAB_PEDIDOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sTabla = "cv403_cab_pedidos";
                sCampo = "Id_Pedido";
                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    return false;
                }

                iIdPedido = Convert.ToInt32(iMaximo);

                //CONSULTAR EL NUMERO DE PEDIDO PARA INSERTAR EN LA TABLA CV403_NUMERO_CAB_PEDIDO
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "select numero_pedido" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Program.iIdLocalidad;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iNumeroPedidoOrden = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                //ACTUALIZAR EL NUMERO DE PEDIDO EN LA TABLA TP_LOCALIDADES_IMPRESORAS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pedido = numero_pedido + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //INSERTAR EN LA TABLA CV403_NUMERO_CAB_PEDIDO
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_numero_cab_pedido (" + Environment.NewLine;
                sSql += "idtipocomprobante,id_pedido, numero_pedido," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "estado, numero_control_replica, numero_replica_trigger)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdTipoComprobante + ", " + iIdPedido + ", " + iNumeroPedidoOrden + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 0, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //OBTENER EL ID DE LA TABLA CV403_CAB_DESPACHOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sTabla = "cv403_cab_despachos";
                sCampo = "id_despacho";
                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    return false;
                }

                iIdCabDespachos = Convert.ToInt32(iMaximo);

                //INSERTAR EN LA TABLA CV403_DESPACHOS_PEDIDOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_despachos_pedidos (" + Environment.NewLine;
                sSql += "id_despacho, id_pedido, estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdCabDespachos + "," + iIdPedido + ", 'A', GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //OBTENER EL ID DE LA TABLA CV403_DESPACHOS_PEDIDOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sTabla = "cv403_despachos_pedidos";
                sCampo = "id_despacho_pedido";
                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    return false;
                }

                iIdDespachoPedido = Convert.ToInt32(iMaximo);

                //INSERTAR EN LA TABLA CV403_EVENTOS_COBROS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_eventos_cobros (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_persona, id_localidad, cg_evento_cobro," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Program.iIdEmpresa + ", " + Program.iCgEmpresa + ", " + iIdPersona + "," + Program.iIdLocalidad + "," + Environment.NewLine;
                sSql += "7466, GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //EXTRAER EL ID DE LA TABLA CV403_EVENTOS_COBROS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sTabla = "cv403_eventos_cobros";
                sCampo = "id_evento_cobro";
                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    return false;
                }

                iIdEventoCobro = Convert.ToInt32(iMaximo);

                //INSERTAR EN LA TABLA CV403_DCTOS_POR_COBRAR
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_dctos_por_cobrar (" + Environment.NewLine;
                sSql += "id_evento_cobro, id_pedido, cg_tipo_documento, fecha_vcto, cg_moneda," + Environment.NewLine;
                sSql += "valor, cg_estado_dcto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdEventoCobro + ", " + iIdPedido + ", " + iCgTipoDocumento + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Program.iMoneda + ", " + dTotalDebido + "," + Environment.NewLine;
                sSql += "7460, 'A', GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (iBanderaAplicaRecargo == 0)
                {
                    //CICLO PARA INSERTAR EN LA TABLA CV403_DET_PEDIDOS
                    for (int i = 0; i < dgvPedido.Rows.Count; i++)
                    {
                        iIdProducto_P = Convert.ToInt32(dgvPedido.Rows[i].Cells["idProducto"].Value);
                        dPrecioUnitario_P = Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value);
                        dCantidad_P = Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value);
                        sPagaIva_P = dgvPedido.Rows[i].Cells["pagaIva"].Value.ToString();

                        if (sPagaIva_P == "1")
                        {
                            dIVA_P = dPrecioUnitario_P * Convert.ToDecimal(Program.iva);
                        }

                        else
                        {
                            dIVA_P = 0;
                        }

                        sSql = "";
                        sSql += "Insert Into cv403_det_pedidos(" + Environment.NewLine;
                        sSql += "Id_Pedido, id_producto, Cg_Unidad_Medida, precio_unitario," + Environment.NewLine;
                        sSql += "Cantidad, Valor_Dscto, Valor_Ice, Valor_Iva ,Valor_otro," + Environment.NewLine;
                        sSql += "comentario, Id_Definicion_Combo, fecha_ingreso," + Environment.NewLine;
                        sSql += "Usuario_Ingreso, Terminal_ingreso, id_pos_mascara_item, secuencia," + Environment.NewLine;
                        sSql += "id_pos_secuencia_entrega, Estado, numero_replica_trigger," + Environment.NewLine;
                        sSql += "numero_control_replica, id_empleado_cliente_empresarial)" + Environment.NewLine;
                        sSql += "values(" + Environment.NewLine;
                        sSql += iIdPedido + ", " + iIdProducto_P + ", 546, " + dPrecioUnitario_P + ", " + Environment.NewLine;
                        sSql += dCantidad_P + ", 0, 0, " + dIVA_P + ", 0, " + Environment.NewLine;
                        sSql += "null, null, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                        sSql += "'" + Program.sDatosMaximo[1] + "', 0, 1, null, 'A', 0, 0, " + iIdPersona + ")";

                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                            catchMensaje.ShowDialog();
                            return false;
                        }

                        sSql = "";
                        sSql += "insert into cv403_cantidades_despachadas(" + Environment.NewLine;
                        sSql += "id_despacho_pedido, id_producto, cantidad, estado," + Environment.NewLine;
                        sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                        sSql += "values (" + Environment.NewLine;
                        sSql += iIdDespachoPedido + ", " + dgvPedido.Rows[i].Cells["idProducto"].Value + "," + Environment.NewLine;
                        sSql += dgvPedido.Rows[i].Cells["cantidad"].Value + ", 'A', 1, 0)";

                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                            catchMensaje.ShowDialog();
                            return false;
                        }
                    }
                }

                else
                {
                    //CICLO PARA INSERTAR EN LA TABLA CV403_DET_PEDIDOS
                    for (int i = 0; i < dtRecargos.Rows.Count; i++)
                    {
                        iIdProducto_P = Convert.ToInt32(dtRecargos.Rows[i]["id_producto"].ToString());
                        dPrecioUnitario_P = Convert.ToDecimal(dtRecargos.Rows[i]["valor_recargo"].ToString());
                        dCantidad_P = Convert.ToDecimal(dtRecargos.Rows[i]["cantidad"].ToString());
                        sPagaIva_P = dtRecargos.Rows[i]["paga_iva"].ToString();

                        if (sPagaIva_P == "1")
                        {
                            dIVA_P = Convert.ToDecimal(dtRecargos.Rows[i]["valor_iva"].ToString());
                        }

                        else
                        {
                            dIVA_P = 0;
                        }

                        sSql = "";
                        sSql += "Insert Into cv403_det_pedidos(" + Environment.NewLine;
                        sSql += "Id_Pedido, id_producto, Cg_Unidad_Medida, precio_unitario," + Environment.NewLine;
                        sSql += "Cantidad, Valor_Dscto, Valor_Ice, Valor_Iva ,Valor_otro," + Environment.NewLine;
                        sSql += "comentario, Id_Definicion_Combo, fecha_ingreso," + Environment.NewLine;
                        sSql += "Usuario_Ingreso, Terminal_ingreso, id_pos_mascara_item, secuencia," + Environment.NewLine;
                        sSql += "id_pos_secuencia_entrega, Estado, numero_replica_trigger," + Environment.NewLine;
                        sSql += "numero_control_replica, id_empleado_cliente_empresarial)" + Environment.NewLine;
                        sSql += "values(" + Environment.NewLine;
                        sSql += iIdPedido + ", " + iIdProducto_P + ", 546, " + dPrecioUnitario_P + ", " + Environment.NewLine;
                        sSql += dCantidad_P + ", 0, 0, " + dIVA_P + ", 0, " + Environment.NewLine;
                        sSql += "null, null, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                        sSql += "'" + Program.sDatosMaximo[1] + "', 0, 1, null, 'A', 0, 0, " + iIdPersona + ")";

                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                            catchMensaje.ShowDialog();
                            return false;
                        }

                        sSql = "";
                        sSql += "insert into cv403_cantidades_despachadas(" + Environment.NewLine;
                        sSql += "id_despacho_pedido, id_producto, cantidad, estado," + Environment.NewLine;
                        sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                        sSql += "values (" + Environment.NewLine;
                        sSql += iIdDespachoPedido + ", " + dgvPedido.Rows[i].Cells["idProducto"].Value + "," + Environment.NewLine;
                        sSql += dgvPedido.Rows[i].Cells["cantidad"].Value + ", 'A', 1, 0)";

                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                            catchMensaje.ShowDialog();
                            return false;
                        }
                    }
                }


                return true;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //INSERTAR LA SEGUNDA FASE - PAGOS
        private bool insertarPagos()
        {
            try
            {
                //SELECCIONAR EL ID_DOCUMENTO_COBRAR
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "select id_documento_cobrar" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());

                //INSERTAR EN LA TABLA CV403_PAGOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_pagos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, " + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica, cambio) " + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Program.iIdEmpresa + ", " + Program.iIdPersona + ", '" + sFecha + "', " + Program.iMoneda + "," + Environment.NewLine;
                sSql += dTotalDebido + ", 0, " + Program.iCgEmpresa + ", " + Program.iIdLocalidad + ", 7799, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', 'A' , 0, 0, " + dbCambio + ")";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //OBTENER EL ID DE LA TABLA CV403_PAGOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sTabla = "cv403_pagos";
                sCampo = "id_pago";

                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    return false;
                }

                iIdPago = Convert.ToInt32(iMaximo);

                //SELECCIONAR EL NUMERO PAGO
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "select numero_pago" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iNumeroPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                //INSERTAR EN LA TABLA CV403_NUMEROS_PAGOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //ACTUALIZAR EL SECUENCIA NUMERO PAGO EN TP_LOCALIDADES_IMPRESORAS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //OBTENER EL ID DEL EFECTIVO Y CG_TIPO_DOCUMENTO
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "select FC.id_pos_tipo_forma_cobro, FC.cg_tipo_documento, FPA.id_forma_pago," + Environment.NewLine;
                sSql += "MP.descripcion" + Environment.NewLine;
                sSql += "from pos_tipo_forma_cobro FC INNER JOIN" + Environment.NewLine;
                sSql += "pos_metodo_pago MP ON MP.id_pos_metodo_pago = FC.id_pos_metodo_pago" + Environment.NewLine;
                sSql += "and MP.estado = 'A'" + Environment.NewLine;
                sSql += "and FC.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "sri_forma_pago FP ON FP.id_sri_forma_pago = MP.id_sri_forma_pago" + Environment.NewLine;
                sSql += "and FP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "cv403_formas_pagos FPA ON FP.id_sri_forma_pago = FPA.id_sri_forma_pago" + Environment.NewLine;
                sSql += "and FPA.estado = 'A'" + Environment.NewLine;
                sSql += "where FPA.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;

                if (iBanderaExpressTarjeta == 0)
                {
                    if (iBanderaEfectivoTarjeta == 0)
                    {
                        sSql += "and MP.codigo = 'EF'";
                    }

                    else
                    {
                        sSql += "and FC.id_pos_tipo_forma_cobro = " + iIdTipoFormaCobro;
                    }
                }

                else
                {
                    sSql += "and MP.codigo = 'TA'";
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iIdTipoFormaCobro = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_tipo_forma_cobro"].ToString());
                iCgTipoDocumentoCobro = Convert.ToInt32(dtConsulta.Rows[0]["cg_tipo_documento"].ToString());
                iIdFormaPago_1 = Convert.ToInt32(dtConsulta.Rows[0]["id_forma_pago"].ToString());
                sDescripcionFormaPago = dtConsulta.Rows[0]["descripcion"].ToString();

                //INSERTAR EN LA TABLA CV403_DOCUMENTOS_PAGOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                sSql += "cg_moneda, cotizacion, valor, id_pos_tipo_forma_cobro," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica, valor_recibido) " + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += iIdPago + ", " + iCgTipoDocumentoCobro + ", 9999, '" + sFecha + "', " + Environment.NewLine;
                sSql += Program.iMoneda + ", 1, " + dTotalDebido + ", " + iIdTipoFormaCobro + ", 'A', GETDATE()," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 0, 0, " + dbValorRecuperado + ")";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //OBTENER EL ID DEL DOCUMENTO PAGO
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sTabla = "cv403_documentos_pagos";
                sCampo = "id_documento_pago";

                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    return false;
                }

                else
                {
                    iIdDocumentoPago = Convert.ToInt32(iMaximo);
                }

                //INSERTAR EN LA TABLA CV403_DOCUMENTOS_PAGADOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                sSql += "id_documento_cobrar, id_pago, valor, " + Environment.NewLine;
                sSql += "estado, numero_replica_trigger,numero_control_replica," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dTotalDebido + ", 'A', 1, 0," + Environment.NewLine;
                sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }
                
                return true;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //INSERTAR LA TERCERA FASE - FACTURA
        private bool insertarFactura()
        {
            try
            {
                //INSERTAR EN LA TABLA CV403_FACTURAS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_facturas (idempresa, id_persona, cg_empresa, idtipocomprobante," + Environment.NewLine;
                sSql += "id_localidad, idformulariossri, id_vendedor, id_forma_pago, id_forma_pago2, id_forma_pago3," + Environment.NewLine;
                sSql += "fecha_factura, fecha_vcto, cg_moneda, valor, cg_estado_factura, editable, fecha_ingreso, " + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger, numero_control_replica, " + Environment.NewLine;
                sSql += "Direccion_Factura,Telefono_Factura,Ciudad_Factura, correo_electronico, servicio," + Environment.NewLine;
                sSql += "facturaelectronica, id_tipo_emision, id_tipo_ambiente)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Program.iIdEmpresa + ", " + iIdPersona + ", " + Program.iCgEmpresa + "," + Environment.NewLine;
                sSql += iIdTipoComprobante + "," + Program.iIdLocalidad + ", " + Program.iIdFormularioSri + ", " + Program.iIdVendedor + ", " + iIdFormaPago_1 + ", " + Environment.NewLine;
                sSql += "null, null, '" + sFecha + "', '" + sFecha + "', " + Program.iMoneda + ", " + dTotalDebido + ", 0, 0, GETDATE()," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0," + Environment.NewLine;
                sSql += "'QUITO', '2999999', 'QUITO', '" + Program.sCorreoElectronicoDefault + "', 0, 0, 0, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //OBTENER EL ID DE LA TABLA FACTURAS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sTabla = "cv403_facturas";
                sCampo = "id_factura";

                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    return false;
                }

                iIdFactura = Convert.ToInt32(iMaximo);

                //INSERTAR EN LA TABLA CV403_NUMEROS_FACTURAS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_numeros_facturas (id_factura, idtipocomprobante, numero_factura," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger," + Environment.NewLine;
                sSql += "numero_control_replica) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", " + iIdTipoComprobante + ", " + Convert.ToInt32(TxtNumeroFactura.Text.Trim()) + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 0, 0 )";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //ACTUALIZAR EL NUMERO DE NOTA DE ENTREGA EN TP_LOCALIDADES
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numeronotaentrega = numeronotaentrega + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and estado = 'A'";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //INSERTAR EN LA TABLA CV403_FACTURAS_PEDIDOS
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "insert into cv403_facturas_pedidos (" + Environment.NewLine;
                sSql += "id_factura, id_pedido, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "estado, numero_replica_trigger, numero_control_replica) " + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", " + iIdPedido + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 0, 0 )";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                //ACTUALIZAR LA TABLA CV403_DCTOS_POR_COBRAR
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "id_factura = " + iIdFactura + "," + Environment.NewLine;
                sSql += "cg_estado_dcto = " + iCgEstadoDctoPorCobrar + "," + Environment.NewLine;
                sSql += "numero_documento = " + Convert.ToInt32(TxtNumeroFactura.Text.Trim()) + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido;

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (iBanderaExpressTarjeta == 0)
                {
                    //SELECCIONAR EL ID DE CAJA
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    sSql = "";
                    sSql += "select id_caja" + Environment.NewLine;
                    sSql += "from cv405_cajas" + Environment.NewLine;
                    sSql += "where estado = 'A'" + Environment.NewLine;
                    sSql += "and id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                    sSql += "and cg_tipo_caja = 8906";

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == false)
                    {
                        catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                        catchMensaje.ShowDialog();
                        return false;
                    }

                    iIdCaja = Convert.ToInt32(dtConsulta.Rows[0]["id_caja"].ToString());

                    //VARIABLES PARA INSERTAR LOS MOVIMIENTOS DE CAJA
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    sSecuencial = TxtNumeroFactura.Text.Trim().PadLeft(9, '0');
                    sMovimiento = ("N. ENTREGA. No. " + txtfacturacion.Text.Trim() + "-" + sSecuencial).Trim();

                    //INSERTAR EN LA TABLA POS_MOVIMIENTO_CAJA
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    sSql = "";
                    sSql += "insert into pos_movimiento_caja (" + Environment.NewLine;
                    sSql += "tipo_movimiento, idempresa, id_localidad, id_persona, id_cliente," + Environment.NewLine;
                    sSql += "id_caja, id_pos_cargo, fecha, hora, cg_moneda, valor, concepto," + Environment.NewLine;
                    sSql += "documento_venta, id_documento_pago, id_pos_jornada, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "1, " + Program.iIdEmpresa + ", " + Program.iIdLocalidad + "," + Program.iIdPersonaMovimiento + ", ";
                    sSql += +iIdPersona + ", " + iIdCaja + ", 1," + Environment.NewLine;
                    sSql += "'" + sFecha + "', GETDATE(), " + Program.iMoneda + ", " + dTotalDebido + "," + Environment.NewLine;
                    sSql += "'COBRO No. CUENTA " + iNumeroPedidoOrden.ToString() + " (" + sDescripcionFormaPago + ")'," + Environment.NewLine;
                    sSql += "'" + sMovimiento.Trim() + "', " + iIdDocumentoPago + ", " + Program.iJORNADA + "," + Environment.NewLine;
                    sSql += "'A', GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                        catchMensaje.ShowDialog();
                        return false;
                    }

                    //EXTRAER EL ID DEL MOVIMIENTO DE CAJA
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    sTabla = "pos_movimiento_caja";
                    sCampo = "id_pos_movimiento_caja";

                    iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                    if (iMaximo == -1)
                    {
                        ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                        ok.ShowDialog();
                        return false;
                    }

                    iIdPosMovimientoCaja = Convert.ToInt32(iMaximo);

                    //CONSULTAR EL NUMERO DE MOVIMIENTO DE CAJA EN TP_LOCALIDADES_IMPRESORAS
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    sSql = "";
                    sSql += "select numeromovimientocaja" + Environment.NewLine;
                    sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                    sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                    sSql += "and estado = 'A'";

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();
                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == false)
                    {
                        catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                        catchMensaje.ShowDialog();
                        return false;
                    }

                    iNumeroMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                    //INSERTAR EN LA TABLA POS_NUMERO_MOVIMIENTO_CAJA
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    sSql = "";
                    sSql += "insert into pos_numero_movimiento_caja (" + Environment.NewLine;
                    sSql += "id_pos_movimiento_caja, numero_movimiento_caja, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += iIdPosMovimientoCaja + ", " + iNumeroMovimientoCaja + ", 'A', GETDATE()," + Environment.NewLine;
                    sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                        catchMensaje.ShowDialog();
                        return false;
                    }

                    //ACTUALIZAR EL NUMERO DE MOVIMIENTO DE CAJA
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    sSql = "";
                    sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                    sSql += "numeromovimientocaja = numeromovimientocaja + 1" + Environment.NewLine;
                    sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                    sSql += "and estado = 'A'";

                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                        catchMensaje.ShowDialog();
                        return false;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return false;
            }
        }
        
        //FUNCION PARA CREAR EL REPORTE
        private void crearReporte()
        {
            try
            {
                if (Program.iEjecutarImpresion == 1)
                {
                    ReportesTextBox.frmVerNotaVentaFactura notaVenta = new ReportesTextBox.frmVerNotaVentaFactura(iIdPedido.ToString(), 1);
                    notaVenta.ShowDialog();

                    if (notaVenta.DialogResult == DialogResult.OK)
                    {
                        Cambiocs ok = new Cambiocs("$ " + dbCambio.ToString("N2"));
                        ok.lblVerMensaje.Text = "NOTA DE ENTREGA GENERADA";
                        ok.ShowDialog();
                        notaVenta.Close();
                        this.Close();
                    }
                }

                else
                {
                    Cambiocs ok = new Cambiocs("$ " + dbCambio.ToString("N2"));
                    ok.lblVerMensaje.Text = "NOTA DE ENTREGA GENERADA";
                    ok.ShowDialog();
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                this.Close();
            }
        }

        #endregion

        private void frmComandaComidaRapida_Load(object sender, EventArgs e)
        {
            datosFactura();
            cargarCategorias();

            if (iBanderaExpressTarjeta == 1)
            {
                btnAceptar.Visible = false;
                btnTarjetas.Visible = false;
                btnCobroTarjetaAlmuerzo.Visible = true;
                this.Text = "COMANDA PARA TARJETA DE ALMUERZOS";
            }

            else
            {
                btnAceptar.Visible = true;
                btnTarjetas.Visible = true;
                btnCobroTarjetaAlmuerzo.Visible = false;
                this.Text = "COMANDA PARA VENTA EXPRESS";
            }

        }

        private void btnRemoverItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPedido.Rows.Count == 0)
                {
                    ok.LblMensaje.Text = "No hay ítems en la comanda.";
                    ok.ShowDialog();
                }

                else if (dgvPedido.SelectedRows.Count > 0)
                {
                    NuevoSiNo.lblMensaje.Text = "¿Desea eliminar la línea seleccionada?";
                    NuevoSiNo.ShowDialog();

                    if (NuevoSiNo.DialogResult == DialogResult.OK)
                    {
                        dgvPedido.Rows.Remove(dgvPedido.CurrentRow);
                        calcularTotales();
                        NuevoSiNo.Close();
                        dgvPedido.ClearSelection();
                    }
                }

                else
                {
                    ok.LblMensaje.Text = "No se ha seleccionado una línea para remover.";
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        private void dgvPedido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iFila = dgvPedido.CurrentCell.RowIndex;

            Pedidos.frmAumentaRemueveItems sumar = new Pedidos.frmAumentaRemueveItems(iFila);
            sumar.lblItem.Text = dgvPedido.CurrentRow.Cells["producto"].Value.ToString();
            sumar.lblCantidad.Text = dgvPedido.CurrentRow.Cells["cantidad"].Value.ToString();
            sumar.txtCantidad.Text = dgvPedido.CurrentRow.Cells["cantidad"].Value.ToString();
            sumar.ShowDialog();

            if (sumar.DialogResult == DialogResult.OK)
            {
                dgvPedido.Rows[iFila].Cells["cantidad"].Value = sumar.sValorRetorno;
                dbCantidadRecalcular = Convert.ToDecimal(dgvPedido.Rows[iFila].Cells["cantidad"].Value.ToString());
                dbPrecioRecalcular = Convert.ToDecimal(dgvPedido.Rows[iFila].Cells["valuni"].Value.ToString());
                dbSubtotalRecalcular = dbCantidadRecalcular * dbPrecioRecalcular;
                dgvPedido.Rows[iFila].Cells["subtotal"].Value = dbSubtotalRecalcular.ToString("N2");
                dbValorIVA = dbSubtotalRecalcular * Convert.ToDecimal(Program.iva);
                dbValorTotalRecalcular = dbSubtotalRecalcular + dbValorIVA;
                dgvPedido.Rows[iFila].Cells["valor"].Value = dbValorTotalRecalcular.ToString("N2");
                calcularTotales();
                sumar.Close();
                dgvPedido.ClearSelection();
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            iCuentaCategorias -= iCuentaAyudaCategorias;

            if (iCuentaCategorias <= 8)
            {
                btnAnterior.Enabled = false;
            }

            btnSiguiente.Enabled = true;
            iCuentaCategorias -= 8;
            crearBotonesCategorias();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            btnAnterior.Enabled = true;
            crearBotonesCategorias();
        }

        private void btnAnteriorProducto_Click(object sender, EventArgs e)
        {
            iCuentaProductos -= iCuentaAyudaProductos;

            if (iCuentaProductos <= 25)
            {
                btnAnteriorProducto.Enabled = false;
            }

            btnSiguienteProducto.Enabled = true;
            iCuentaProductos -= 8;
            crearBotonesProductos();
        }

        private void btnSiguienteProducto_Click(object sender, EventArgs e)
        {
            btnAnteriorProducto.Enabled = true;
            crearBotonesProductos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvPedido.Rows.Count == 0)
            {
                ok.LblMensaje.Text = "No hay ítems ingresados para crear la comanda";
                ok.ShowDialog();
            }

            else
            {
                Efectivo efectivo = new Efectivo("0", dTotalDebido.ToString("N2"), "", "EFECTIVO");
                efectivo.ShowDialog();

                if (efectivo.DialogResult == DialogResult.OK)
                {
                    dbValorGrid = efectivo.dbValorGrid;
                    dbValorRecuperado = efectivo.dbValorIngresado;
                    dbCambio = dbValorRecuperado - dbValorGrid;
                    efectivo.Close();

                    iBanderaEfectivoTarjeta = 0;
                    crearComanda();
                }
            }
        }

        private void frmComandaComidaRapida_KeyDown(object sender, KeyEventArgs e)
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

        private void btnTarjetas_Click(object sender, EventArgs e)
        {
            if (dgvPedido.Rows.Count == 0)
            {
                ok.LblMensaje.Text = "No hay ítems ingresados para crear la comanda";
                ok.ShowDialog();
            }

            else
            {
                DataTable dtItems = new DataTable();
                dtItems.Columns.Add("cantidad");
                dtItems.Columns.Add("valor_item");
                dtItems.Columns.Add("valor_recargo");
                dtItems.Columns.Add("valor_iva");
                dtItems.Columns.Add("total");
                dtItems.Columns.Add("id_producto");
                dtItems.Columns.Add("paga_iva");

                for (int i = 0; i < dgvPedido.Rows.Count; i++)
                {
                    DataRow row = dtItems.NewRow();
                    row["cantidad"] = dgvPedido.Rows[i].Cells[0].Value.ToString();
                    row["valor_item"] = dgvPedido.Rows[i].Cells[2].Value.ToString();
                    row["valor_recargo"] = "0";
                    row["valor_iva"] = "0";
                    row["total"] = "0";
                    row["id_producto"] = dgvPedido.Rows[i].Cells[6].Value.ToString();
                    row["paga_iva"] = dgvPedido.Rows[i].Cells[5].Value.ToString();
                    dtItems.Rows.Add(row);
                }


                Comida_Rapida.frmCobroRapidoTarjetas cobro = new frmCobroRapidoTarjetas(dTotalDebido, dtItems);
                cobro.ShowDialog();

                if (cobro.DialogResult == DialogResult.OK)
                {
                    iBanderaEfectivoTarjeta = 1;
                    iBanderaAplicaRecargo = cobro.iBanderaRecargo;
                    dtRecargos = new DataTable();
                    dtRecargos = cobro.dtValores;
                    iIdTipoFormaCobro = cobro.iIdFormaPago;
                    
                    if (iBanderaAplicaRecargo == 1)
                    {
                        dTotalDebido = cobro.dbPagar;
                        lblTotal.Text = "$ " + dTotalDebido.ToString("N2");
                    }

                    dbValorRecuperado = dTotalDebido;
                    dbCambio = 0;
                    cobro.Close();
                    crearComanda();
                }
            }
        }

        private void btnCobroTarjetaAlmuerzo_Click(object sender, EventArgs e)
        {
            if (dgvPedido.Rows.Count == 0)
            {
                ok.LblMensaje.Text = "No hay ítems ingresados para crear la comanda";
                ok.ShowDialog();
            }

            else
            {
                crearComanda();
            }
        }
    }
}
