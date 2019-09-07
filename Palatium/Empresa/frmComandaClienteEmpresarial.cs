using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Empresa
{
    public partial class frmComandaClienteEmpresarial : Form
    {
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();

         ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
         Button[,] boton = new Button[2, 4];
         Button[,] botonProductos = new Button[5, 5];
         
         
         string sSql;
         string sPagaIva_P;
         string sNombreProducto_P;
         string sNombreEmpresa;
         string sNombreEmpleado;
         string sFechaOrden;
         string sTabla;
         string sCampo;
         string sFechaConsulta;

         long iMaximo;

         bool bRespuesta;

         Button botonSeleccionadoCategoria;
         Button botonSeleccionadoProducto;

         DataTable dtConsulta;
         DataTable dtCategorias;
         DataTable dtProductos;

         int iPosXProductos;
         int iPosYProductos;
         int iCuentaAyudaProductos;
         int iCuentaCategorias;
         int iPosXCategorias;
         int iPosYCategorias;
         int iCuentaAyudaCategorias;
         int iCuentaProductos;
         int iIdPersona;
         int iIdPersonaEmpresa;
         int iIdOrigenOrden;
         int iIdPedido;
         int iCuentaDiaria;
         int iNumeroPedidoOrden;
         int iIdEventoCobro;
         int iIdCabDespachos;
         int iIdDespachoPedido;
         int iIdProducto_P;
         int iCgTipoDocumento = 2725;


         Decimal dPrecioUnitario_P;
         Decimal dCantidad_P;
         Decimal dIVA_P;
         Decimal dbIva_P;
         Decimal dbTotal_P;
         Decimal dTotalDebido;
         Decimal dbCantidadRecalcular;
         Decimal dbPrecioRecalcular;
         Decimal dbValorTotalRecalcular;

         public frmComandaClienteEmpresarial(int iIdPersona_P, string sNombreEmpresa_P, string sNombreEmpleado_P, int iIdPersonaEmpresa_P, int iIdOrigenOrden_P)
         {
            this.iIdPersona = iIdPersona_P;
            this.sNombreEmpresa = sNombreEmpresa_P;
            this.sNombreEmpleado = sNombreEmpleado_P;
            this.iIdPersonaEmpresa = iIdPersonaEmpresa_P;
            this.iIdOrigenOrden = iIdOrigenOrden_P;
            InitializeComponent();
         }

        #region FUNCIONES EL USUARIO

        //FUNCION PARA EXTRAER EL NUMERO DE CUENTA
         private void extraerNumeroCuenta()
         {
             try
             {
                 sFechaConsulta = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 sSql = "";
                 sSql += "select isnull(max(cuenta), 0) cuenta" + Environment.NewLine;
                 sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                 sSql += "where fecha_pedido = '" + sFechaConsulta + "'";

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
                         boton[i, j] = new Button();
                         boton[i, j].Cursor = Cursors.Hand;
                         boton[i, j].Click += new EventHandler(boton_clic_categorias);
                         boton[i, j].Size = new Size(130, 71);
                         boton[i, j].Location = new Point(iPosXCategorias, iPosYCategorias);
                         boton[i, j].BackColor = Color.Lime;
                         boton[i, j].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                         boton[i, j].Tag = dtCategorias.Rows[iCuentaCategorias]["id_producto"].ToString();
                         boton[i, j].Text = dtCategorias.Rows[iCuentaCategorias]["nombre"].ToString();
                         boton[i, j].AccessibleDescription = dtCategorias.Rows[iCuentaCategorias]["subcategoria"].ToString();
                         pnlCategorias.Controls.Add(boton[i, j]);

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
                 }
             }

             //dTotalDebido = num1 + num2 - num3 - num4 + (num1 - num3) * Convert.ToDecimal(Program.iva) + num7;
             dTotalDebido = dSubtotalConIva + dSubtotalCero;
             lblTotal.Text = "$ " + dTotalDebido.ToString("N2");
         }

        //FUNCION PARA INSERTAR LA COMANDA
         private void insertarComanda()
         {
             try
             {
                 Cursor = Cursors.WaitCursor;
                 extraerNumeroCuenta();

                 sFechaOrden = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                 {
                     ok.LblMensaje.Text = "Error al abrir transacción";
                     ok.ShowDialog();
                     return;
                 }

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
                 sSql += "'" + sFechaOrden + "', " + iIdPersonaEmpresa + ",8032," + Program.iMoneda + "," + Environment.NewLine;
                 sSql += (Program.iva * 100) + "," + Program.iIdVendedor + ",6967, 0, 7471," + Environment.NewLine;
                 sSql += "GETDATE(),'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "'," + iCuentaDiaria + ", 0, ";
                 sSql += Program.iIdCajeroDefault + "," + iIdOrigenOrden + ", 0," + Program.iJORNADA + "," + Environment.NewLine;
                 sSql += "'" + sFechaOrden + "', GETDATE(), null, 'Cerrada'," + Environment.NewLine;
                 sSql += "1, 1, 1, 0, 0, 'A', 1, null, null," + Environment.NewLine;
                 sSql += Program.iIdMesero + ", " + Program.iIdTerminal + ", " + (Program.servicio * 100) + ", 0)";

                 Program.iBanderaCliente = 0;

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 sSql = "";
                 sSql += "insert into cv403_cab_despachos (" + Environment.NewLine;
                 sSql += "idempresa, id_persona, cg_empresa, id_localidad, fecha_despacho," + Environment.NewLine;
                 sSql += "cg_motivo_despacho, id_destinatario, punto_partida, cg_ciudad_entrega," + Environment.NewLine;
                 sSql += "direccion_entrega, id_transportador, fecha_inicio_transporte," + Environment.NewLine;
                 sSql += "fecha_fin_transporte, cg_estado_despacho, punto_venta, fecha_ingreso," + Environment.NewLine;
                 sSql += "usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                 sSql += "values (" + Environment.NewLine;
                 sSql += Program.iIdEmpresa + ", " + iIdPersonaEmpresa + ", " + Program.iCgEmpresa + ", " + Program.iIdLocalidad + "," + Environment.NewLine;
                 sSql += "'" + sFechaOrden + "', " + Program.iCgMotivoDespacho + ", " + iIdPersonaEmpresa + "," + Environment.NewLine;
                 sSql += "'" + Program.sPuntoPartida + "', " + Program.iCgCiudadEntrega + ", '" + Program.sDireccionEntrega + "'," + Environment.NewLine;
                 sSql += "'" + Program.iIdPersona + "', '" + sFechaOrden + "', '" + sFechaOrden + "', " + Program.iCgEstadoDespacho + "," + Environment.NewLine;
                 sSql += "1, GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();
                 sTabla = "cv403_cab_pedidos";
                 sCampo = "Id_Pedido";
                 iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                 if (iMaximo == -1)
                 {
                     ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                     ok.ShowDialog();
                     goto reversa;
                 }

                 iIdPedido = Convert.ToInt32(iMaximo);
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
                     goto reversa;
                 }

                 iNumeroPedidoOrden = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                 sSql = "";
                 sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                 sSql += "numero_pedido = numero_pedido + 1" + Environment.NewLine;
                 sSql += "where id_localidad = " + Program.iIdLocalidad;

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 sSql = "";
                 sSql += "insert into cv403_numero_cab_pedido (" + Environment.NewLine;
                 sSql += "idtipocomprobante,id_pedido, numero_pedido," + Environment.NewLine;
                 sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                 sSql += "estado, numero_control_replica, numero_replica_trigger)" + Environment.NewLine;
                 sSql += "values (" + Environment.NewLine;
                 sSql += "1, " + iIdPedido + ", " + iNumeroPedidoOrden + ", GETDATE()," + Environment.NewLine;
                 sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 0, 0)";

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 sTabla = "cv403_cab_despachos";
                 sCampo = "id_despacho";
                 iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                 if (iMaximo == -1)
                 {
                     ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                     ok.ShowDialog();
                     goto reversa;
                 }

                 iIdCabDespachos = Convert.ToInt32(iMaximo);

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
                     goto reversa;
                 }

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 sTabla = "cv403_despachos_pedidos";
                 sCampo = "id_despacho_pedido";
                 iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                 if (iMaximo == -1)
                 {
                     ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                     ok.ShowDialog();
                     goto reversa;
                 }

                 iIdDespachoPedido = Convert.ToInt32(iMaximo);

                 sSql = "";
                 sSql += "insert into cv403_eventos_cobros (" + Environment.NewLine;
                 sSql += "idempresa, cg_empresa, id_persona, id_localidad, cg_evento_cobro," + Environment.NewLine;
                 sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado," + Environment.NewLine;
                 sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                 sSql += "values(" + Environment.NewLine;
                 sSql += Program.iIdEmpresa + ", " + Program.iCgEmpresa + ", " + iIdPersonaEmpresa + "," + Program.iIdLocalidad + "," + Environment.NewLine;
                 sSql += "7466, GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 sTabla = "cv403_eventos_cobros";
                 sCampo = "id_evento_cobro";
                 iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                 if (iMaximo == -1)
                 {
                     ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                     ok.ShowDialog();
                     goto reversa;
                 }

                 iIdEventoCobro = Convert.ToInt32(iMaximo);

                 sSql = "";
                 sSql += "insert into cv403_dctos_por_cobrar (" + Environment.NewLine;
                 sSql += "id_evento_cobro, id_pedido, cg_tipo_documento, fecha_vcto, cg_moneda," + Environment.NewLine;
                 sSql += "valor, cg_estado_dcto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                 sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                 sSql += "values (" + Environment.NewLine;
                 sSql += iIdEventoCobro + ", " + iIdPedido + ", " + iCgTipoDocumento + "," + Environment.NewLine;
                 sSql += "'" + sFechaOrden + "', " + Program.iMoneda + ", " + dTotalDebido + "," + Environment.NewLine;
                 sSql += "7460, 'A', GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                 sSql += "'" + Program.sDatosMaximo[1] + "', 1, 0)";

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

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
                         goto reversa;
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
                         goto reversa;
                     }
                 }

                 conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                 if (Program.iImprimirCocina == 1)
                 {
                     if (Program.iEjecutarImpresion == 1)
                     {
                         Pedidos.frmVerReporteCocinaTextBox cocina = new Pedidos.frmVerReporteCocinaTextBox(iIdPedido.ToString(), 1);
                         cocina.ShowDialog();
                     }

                     ReportesTextBox.frmVerPrecuentaEmpresaTextBox precuenta = new ReportesTextBox.frmVerPrecuentaEmpresaTextBox(iIdPedido.ToString(), 1);
                     precuenta.ShowDialog();
                 }

                 ok.LblMensaje.Text = "Guardado en la orden: " + iNumeroPedidoOrden.ToString() + ".";
                 ok.ShowDialog();
                 Cursor = Cursors.Default;
                 this.DialogResult = DialogResult.OK;
                 Close();
                 return;
             }

             catch (Exception ex)
             {
                 Cursor = Cursors.Default;
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
             }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }
         }

        #endregion

         private void frmComandaClienteEmpresarial_Load(object sender, EventArgs e)
         {
             lblEmpresa.Text = sNombreEmpresa;
             lblEmpleado.Text = sNombreEmpleado;
             cargarCategorias();
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
                 dbValorTotalRecalcular = dbCantidadRecalcular * dbPrecioRecalcular;
                 dgvPedido.Rows[iFila].Cells["valor"].Value = dbValorTotalRecalcular.ToString("N2");
                 calcularTotales();
                 sumar.Close();
                 dgvPedido.ClearSelection();
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

         private void btnSalir_Click(object sender, EventArgs e)
         {
             this.Close();
         }

         private void btnAceptar_Click(object sender, EventArgs e)
         {
             if (dgvPedido.Rows.Count == 0)
             {
                 ok.LblMensaje.Text = "No hay ítems para generar la comanda.";
                 ok.ShowDialog();
             }

             else
             {
                 insertarComanda();
             }
         }

         private void btnSiguiente_Click(object sender, EventArgs e)
         {
             btnAnterior.Enabled = true;
             crearBotonesCategorias();
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

         private void btnSiguienteProducto_Click(object sender, EventArgs e)
         {
             btnAnteriorProducto.Enabled = true;
             crearBotonesProductos();
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
    }
}
