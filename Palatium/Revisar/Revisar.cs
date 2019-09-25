using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Palatium.Revisar
{
    public partial class Revisar : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeEspere espere = new VentanasMensajes.frmMensajeEspere();

        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();

        int iCoordenadaX;
        int iCoordenadaY;
        int iIdPedido;
        int iNumeroPedido;
        int iNumeroPersonas;
        int iNumeroCuentaDiaria;
        int iOp;
        int iOrdenesJornada;
        int iCuenta;
        
        string iIdPosMesa;
        string sNombreCajero;
        string sNombreMesero;
        string sTipoOrden;
        string sFechaIngresoOrden;
        string sEstadoOrden;
        string sNombreMesa;
        string sSql;
        string sFechaActual;
        string sFechaActual2;
        string sFechaOrdenComanda;
        string sFechaAperturaCajero_P;
        
        DataTable dtConsultaMesa;
        DataTable dtConsulta;

        Double DSumaDetalleOrden;

        double dbCantidad;
        double dbPrecioUnitario;
        double dbDescuento;
        double dbIva;
        double dbServicio;

        bool bRespuesta;

        //VARIABLES PARA RECUPERAR LA COMANDA
        double dbTotal;

        int iIdPosOrigenOrden_P;
        int iNumeroPersonas_P;
        int iIdMesa_P;
        int iIdPersona_P;
        int iIdCajero_P;
        int iIdMesero_P;
        int iIdJornada_P;

        string sOrigenOrden_P;
        string sNombreMesero_P;
        string sFechaOrden_P;

        delegate void mostrarBotonesDelegado();
        
        public Revisar()
        {
            //Encerar el número de cuentas
            Program.TotalCuentasCanceladas = 0;
            Program.iTotalCuentasDomicilio = 0;
            Program.iTotalCuentasLlevar = 0;
            Program.iTotalCuentasMesa = 0;
            iOp = 1;

            InitializeComponent();
            ScrollBar vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Scroll += (sender, e) => { pnlOrdenes.VerticalScroll.Value = vScrollBar1.Value; };

            pnlOrdenes.Controls.Add(vScrollBar1);

            //EXTRAMOS LOS REGISTROS DEL DIA CON LA JORNADA INGRESADA
            //sFechaActual = DateTime.Now.ToString("yyyy/MM/dd");
            sFechaActual = Program.sFechaSistema.ToString("yyyy/MM/dd");            
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBO DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    cmbLocalidades.DisplayMember = "nombre_localidad";
                    cmbLocalidades.ValueMember = "id_localidad";
                    cmbLocalidades.DataSource = dtConsulta;

                    cmbLocalidades.SelectedValue = Program.iIdLocalidad;
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }
        
        //FUNCION PARA RECUPERAR LOS DATOS DE LA COMANDA
        private void recuperarComanda(int iIdPedido_P)
        {
            try
            {
                dbTotal = 0;

                sSql = "";
                sSql += "select CP.id_pos_origen_orden, ORD.descripcion origen_orden," + Environment.NewLine;
                sSql += "isnull(CP.numero_personas, 0) numero_personas," + Environment.NewLine;
                sSql += "isnull(CP.id_pos_mesa, 0) id_pos_mesa, CP.id_persona, CP.id_pos_cajero," + Environment.NewLine;
                sSql += "CP.id_pos_mesero, isnull(MESERO.descripcion, 'NINGUNO') mesero," + Environment.NewLine;
                sSql += "isnull(MESA.descripcion, 'NINGUNA') descripcion_mesa," + Environment.NewLine;
                sSql += "isnull(CP.id_pos_modo_delivery, 0) id_pos_modo_delivery," + Environment.NewLine;
                sSql += "ORD.genera_factura, isnull(ORD.id_persona, 0) id_persona_origen_orden," + Environment.NewLine;
                sSql += "ORD.id_pos_modo_delivery id_pos_modo_delivery_orden," + Environment.NewLine;
                sSql += "ORD.presenta_opcion_delivery, ORD.codigo, CP.porcentaje_dscto" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                sSql += "pos_origen_orden ORD ON CP.id_pos_origen_orden = ORD.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and CP.estado in ('A', 'N')" + Environment.NewLine;
                sSql += "and ORD.estado = 'A' LEFT OUTER JOIN" + Environment.NewLine;
                sSql += "pos_mesa MESA ON CP.id_pos_mesa = MESA.id_pos_mesa" + Environment.NewLine;
                sSql += "and MESA.estado = 'A' LEFT OUTER JOIN" + Environment.NewLine;
                sSql += "pos_mesero MESERO ON CP.id_pos_mesero = MESERO.id_pos_mesero" + Environment.NewLine;
                sSql += "and MESERO.estado = 'A'" + Environment.NewLine;
                sSql += "where CP.id_pedido = " + iIdPedido_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdPosOrigenOrden_P = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                        sOrigenOrden_P = dtConsulta.Rows[0][1].ToString();
                        iNumeroPersonas_P = Convert.ToInt32(dtConsulta.Rows[0][2].ToString());
                        iIdMesa_P = Convert.ToInt32(dtConsulta.Rows[0][3].ToString());
                        iIdPersona_P = Convert.ToInt32(dtConsulta.Rows[0][4].ToString());
                        iIdCajero_P = Convert.ToInt32(dtConsulta.Rows[0][5].ToString());
                        iIdMesero_P = Convert.ToInt32(dtConsulta.Rows[0][6].ToString());
                        sNombreMesero_P = dtConsulta.Rows[0][7].ToString();

                        Program.sNombreMesa = dtConsulta.Rows[0][8].ToString();

                        if (dtConsulta.Rows[0][9].ToString() == "0")
                        {
                            Program.iDomicilioEspeciales = 0;
                        }

                        else
                        {
                            Program.iDomicilioEspeciales = 1;
                        }

                        Program.sDescripcionOrigenOrden = dtConsulta.Rows[0][2].ToString();
                        Program.iGeneraFactura = Convert.ToInt32(dtConsulta.Rows[0][10].ToString());

                        if (dtConsulta.Rows[0][11].ToString() == "0")
                        {
                            Program.iIdPersonaOrigenOrden = 0;
                        }

                        else
                        {
                            Program.iIdPersonaOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0][11].ToString());
                        }

                        Program.iIdPosModoDelivery = Convert.ToInt32(dtConsulta.Rows[0][12].ToString());
                        Program.iPresentaOpcionDelivery = Convert.ToInt32(dtConsulta.Rows[0][13].ToString());
                        Program.sCodigoAsignadoOrigenOrden = dtConsulta.Rows[0][14].ToString();
                        Program.dbValorPorcentaje = Convert.ToDouble(dtConsulta.Rows[0][15].ToString());

                        Orden o = new Orden(iIdPosOrigenOrden_P, sOrigenOrden_P, iNumeroPersonas, iIdMesa_P, iIdPedido_P, "OK", iIdPersona_P, iIdCajero_P, iIdMesero_P, sNombreMesero_P);
                        o.ShowDialog();
                        this.Close();
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se pudo cargar la información de la comanda. Favor comuníquese con el administrador.";
                        ok.ShowDialog();
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.Show();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA REABRIR LA COMANDA
        private void reabrirComanda(int iIdPedido_P)
        {
            try
            {
                sSql = "";
                sSql += "select fecha_orden, id_pos_jornada" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedido_P + Environment.NewLine;
                sSql += "and estado in ('A', 'N')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sFechaOrden_P = Convert.ToDateTime(dtConsulta.Rows[0][0].ToString()).ToString("yyyy/MM/dd");
                        iIdJornada_P = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se pudo cargar la información de la comanda. Favor comuníquese con el administrador.";
                        ok.ShowDialog();

                        return;
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }

                sFechaAperturaCajero_P = Convert.ToDateTime(Program.sFechaAperturaCajero).ToString("yyyy/MM/dd");

                if ((sFechaOrden_P == sFechaAperturaCajero_P) && (iIdJornada_P == Program.iJornadaCajero) && (Program.sEstadoCajero == "Abierta"))
                {
                    frmOpcionesReabrir r = new frmOpcionesReabrir(iIdPedido_P.ToString(), dbTotal);
                    AddOwnedForm(r);
                    r.ShowDialog();

                    if (r.DialogResult == DialogResult.OK)
                    {
                        if (Program.iBanderaReabrir == 1)
                        {
                            Program.iBanderaReabrir = 0;
                            this.Close();
                        }
                    }
                }

                else
                {
                    ok.LblMensaje.Text = "Ya se encuentra un cierre de caja registrado para esta orden.";
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            } 
        }

        #endregion

        #region FUNCIONES NECESARIAS DEL USUARIO

        //FUNCION ACTIVA TECLADO
        private void activaTeclado()
        {
            //this.TecladoVirtual.SetShowTouchKeyboard(this.txtBusqueda, DevComponents.DotNetBar.Keyboard.TouchKeyboardStyle.Floating);
        }

        //FUNCION PARA CONCATENAR
        private void concatenarValores(string sValor)
        {
            try
            {
                txtBusqueda.Text = txtBusqueda.Text + sValor;
                txtBusqueda.Focus();
                txtBusqueda.SelectionStart = txtBusqueda.Text.Trim().Length;
            }

            catch (Exception)
            {
                ok.LblMensaje.Text = "Ocurrió un problema al concatenar los valores.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }
        }


        //FUNCION PARA CONTAR CUANTAS ORDENES VA EN LA JORNADA EL CAJERO
        private void contarOrdenesCreadas()
        {
            try
            {
                //NECESITAMOS EL NUMERO DE CUENTAS REALIZADAS EN LA JORNADA YA SEA DIURNA O NOCTURA
                //Y QUE SEAN DE LA FECHA ACTUAL
                sFechaActual2 = DateTime.Now.ToString("yyyy/MM/dd");

                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where id_pos_jornada = " + Program.iJORNADA + Environment.NewLine;
                sSql += "and fecha_orden = '" + sFechaActual2 +"'" + Environment.NewLine;
                //sSql += "and id_localidad = " + Program.iIdLocalidad;
                sSql += "and id_localidad = " + cmbLocalidades.SelectedValue;

                dtConsulta = new DataTable();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iOrdenesJornada = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
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

        #endregion

        #region FUNCIONES PARA MOSTRAR LOS BOTONES
        private void mostrarBotones()
        {
            try
            {

                if (this.InvokeRequired)
                {
                    mostrarBotonesDelegado delegado = new mostrarBotonesDelegado(mostrarBotones);
                    this.Invoke(delegado);
                }

                else
                {
                    contarOrdenesCreadas();

                    int h = 1;
                    int controlBoton1 = 1;
                    iCuenta = 0;
                    //pnlOrdenes.Controls.Clear();
                    iCoordenadaX = 0;
                    iCoordenadaY = 0;

                    //INSTRUCCIONES SQL UTILIZADAS EN EL FORMULARIO
                    //EXTRAMOS LOS REGISTROS DEL DIA CON LA JORNADA INGRESADA

                    //EXTRAMOS LOS REGISTROS DEL DIA CON LA JORNADA INGRESADA

                    sSql = "";
                    sSql += "select CP.id_pedido, NP.numero_pedido, isnull(CP.id_pos_mesa,0) id_pos_mesa," + Environment.NewLine;
                    sSql += "isnull(M.descripcion,'NINGUNA') descripcion, C.descripcion, O.descripcion," + Environment.NewLine;
                    sSql += "CP.fecha_apertura_orden, CP.estado_orden, CP.numero_personas, CP.cuenta," + Environment.NewLine;
                    sSql += "MES.descripcion, CP.fecha_orden, CP.id_pos_jornada" + Environment.NewLine;
                    sSql += "from cv403_cab_pedidos as CP inner join" + Environment.NewLine;
                    sSql += "pos_origen_orden as O on O.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                    sSql += "and CP.estado in ('A', 'N')" + Environment.NewLine;
                    sSql += "and O.estado = 'A'inner join" + Environment.NewLine;
                    sSql += "cv403_numero_cab_pedido as NP on NP.id_pedido = CP.id_pedido" + Environment.NewLine;
                    sSql += "and NP.estado in ('A', 'N') left outer join" + Environment.NewLine;
                    sSql += "pos_mesa as M on M.id_pos_mesa = CP.id_pos_mesa" + Environment.NewLine;
                    sSql += "and M.estado = 'A' inner join" + Environment.NewLine;
                    sSql += "pos_cajero as C on C.id_pos_cajero = CP.id_pos_cajero" + Environment.NewLine;
                    sSql += "and C.estado = 'A' inner join" + Environment.NewLine;
                    sSql += "pos_mesero as MES on MES.id_pos_mesero = CP.id_pos_mesero" + Environment.NewLine;
                    sSql += "and MES.estado = 'A'" + Environment.NewLine;
                    sSql += "where CP.fecha_orden = '" + sFechaActual + "'" + Environment.NewLine;
                    //sSql += "and CP.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                    sSql += "and CP.id_localidad = " + cmbLocalidades.SelectedValue + Environment.NewLine;

                    if (iOp == 2)
                    {
                        sSql += "and CP.estado_orden in ('Abierta', 'Pre-Cuenta')" + Environment.NewLine;
                    }

                    else if (iOp == 3)
                    {
                        sSql += "and NP.numero_pedido = " + Convert.ToInt32(txtBusqueda.Text.Trim()) + Environment.NewLine;
                    }

                    else if (iOp == 4)
                    {
                        sSql += "and O.id_pos_origen_orden = 3" + Environment.NewLine;
                    }

                    else if (iOp == 5)
                    {
                        sSql += "and O.id_pos_origen_orden = 1" + Environment.NewLine;
                    }

                    else if (iOp == 6)
                    {
                        sSql += "and O.id_pos_origen_orden = 2" + Environment.NewLine;
                    }

                    else if (iOp == 7)
                    {
                        sSql += "and CP.cuenta = " + Convert.ToInt32(txtBusqueda.Text.Trim()) + Environment.NewLine;
                    }

                    else if (iOp == 8)
                    {
                        sSql += "and CP.estado ='N'" + Environment.NewLine;
                    }

                    sSql += "order by CP.id_pedido desc";


                    dtConsulta = new DataTable();
                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            iCuenta = dtConsulta.Rows.Count;

                            Button[] boton = new Button[iCuenta];
                            Button[] boton1 = new Button[iCuenta];
                            Button[] botonEditar = new Button[iCuenta];

                            //btnTotalOrdenes.Text = "Total de Órdenes \n" + iCuenta;

                            for (int i = 0; i < iCuenta; i++)
                            {
                                //DATOS CARGADOS EN EL DATATABLE RECUPERADO DE LA BASE DE DATOS
                                //USAREMOS VARIABLES PARA QUE EVITAR COLOCAR EL NOMBRE DE TODA LA INSTRUCCIÒN DEL DATATABLE
                                //==========================================================================================================

                                //POSICIÒN 0 = orden.id_pedido
                                iIdPedido = Convert.ToInt32(dtConsulta.Rows[i][0].ToString());
                                //POSICION 1 = numero_pedido                        
                                iNumeroPedido = Convert.ToInt32(dtConsulta.Rows[i][1].ToString());
                                //POSICION 2 = id_pos_mesa
                                iIdPosMesa = dtConsulta.Rows[i][2].ToString();
                                //POSICION 3 = mesa.descripcion
                                sNombreMesa = dtConsulta.Rows[i][3].ToString();
                                //POSICION 4 = nombre del cajero
                                sNombreCajero = dtConsulta.Rows[i][4].ToString();
                                //POSICION 5 = tipo de orden
                                sTipoOrden = dtConsulta.Rows[i][5].ToString();
                                //POSICION 6 = fecha de apertura de la orden 
                                sFechaIngresoOrden = dtConsulta.Rows[i][6].ToString();
                                //POSICION 7 = orden.estado_orden
                                sEstadoOrden = dtConsulta.Rows[i][7].ToString();
                                //POSICION 8 = mesa.descripcion
                                iNumeroPersonas = Convert.ToInt32(dtConsulta.Rows[i][8].ToString());
                                //POSICION 9 = cuenta diarias
                                iNumeroCuentaDiaria = Convert.ToInt32(dtConsulta.Rows[i][9].ToString());
                                //POSICION 10 = nombre del mesero
                                sNombreMesero = dtConsulta.Rows[i][10].ToString();
                                //POSICION 11 = fecha_orden comanda
                                sFechaOrdenComanda = dtConsulta.Rows[i][11].ToString();

                                //==========================================================================================================

                                boton[i] = new Button();
                                boton[i].Name = iIdPedido.ToString();
                                boton[i].Click += boton_clic;
                                boton[i].Size = new Size(700, 100);
                                boton[i].Location = new Point(iCoordenadaX, iCoordenadaY);
                                boton[i].Text = "" + h;
                                boton[i].Font = new Font("Maiandra GD", 11);
                                boton[i].BackColor = Color.FromArgb(255, 224, 192);
                                boton[i].Image = Palatium.Properties.Resources.comanda_revisar;
                                boton[i].ImageAlign = ContentAlignment.MiddleLeft;

                                iCoordenadaX += 701;

                                botonEditar[i] = new Button();
                                botonEditar[i].Name = iIdPedido.ToString();
                                botonEditar[i].Click += botonEditar_Click;
                                botonEditar[i].Image = Palatium.Properties.Resources.editar_comanda_revisar;
                                botonEditar[i].ImageAlign = ContentAlignment.TopCenter;
                                botonEditar[i].Size = new Size(95, 100);
                                botonEditar[i].Location = new Point(iCoordenadaX, iCoordenadaY);
                                botonEditar[i].Font = new Font("Maiandra GD", 11);
                                botonEditar[i].TextAlign = ContentAlignment.BottomCenter;
                                botonEditar[i].BackColor = Color.FromArgb(192, 192, 255);
                                botonEditar[i].AccessibleName = sEstadoOrden.ToUpper();
                                botonEditar[i].AccessibleDescription = sFechaOrdenComanda.ToUpper();

                                if ((sEstadoOrden.ToUpper()) == "PAGADA" || (sEstadoOrden.ToUpper() == "CERRADA"))
                                {
                                    botonEditar[i].Text = "Reabrir" + Environment.NewLine + "Comanda";
                                    botonEditar[i].Tag = 1;
                                }

                                if (sEstadoOrden.ToUpper() == "CANCELADA")
                                {
                                    botonEditar[i].Text = "Reabrir" + Environment.NewLine + "Comanda";
                                    botonEditar[i].Tag = 3;
                                }

                                else if ((sEstadoOrden.ToUpper() == "ABIERTA") || (sEstadoOrden.ToUpper() == "PRE-CUENTA"))
                                {
                                    botonEditar[i].Text = "Editar" + Environment.NewLine + "Comanda";
                                    botonEditar[i].Tag = 2;
                                }

                                iCoordenadaX += 95;

                                boton1[i] = new Button();
                                boton1[i].Name = iIdPedido.ToString();
                                boton1[i].Click += botonImprimir_clic;
                                boton1[i].Image = Palatium.Properties.Resources.impresora_icono;
                                boton1[i].ImageAlign = ContentAlignment.TopCenter;
                                boton1[i].Size = new Size(95, 100);
                                boton1[i].Location = new Point(iCoordenadaX, iCoordenadaY);
                                boton1[i].Font = new Font("Maiandra GD", 11);
                                boton1[i].Text = "Imprimir" + Environment.NewLine + "Precuenta";
                                boton1[i].TextAlign = ContentAlignment.BottomCenter;
                                boton1[i].BackColor = Color.FromArgb(192, 255, 255);
                                controlBoton1++;

                                iCoordenadaX = 0;
                                iCoordenadaY += 102;

                                //Función para verificar el número de mesas
                                //contarNumeroCuentas(i);

                                for (int j = 1; j <= iCuenta; j++)
                                {

                                    if (boton[i].Text == j.ToString())
                                    {
                                        //domicilio /llevar
                                        string t_st_linea1 = "";
                                        string t_st_linea2 = "";
                                        string t_st_linea3 = "";
                                        string t_st_linea4 = "";
                                        string t_st_linea5 = "";
                                        string t_st_linea6 = "";
                                        string t_st_linea7 = "";
                                        string t_st_linea8 = "";
                                        string t_st_linea9 = "";

                                        int iBandera = 0;

                                        if (dtConsulta.Rows[0][5].ToString() == "Pre-Cuenta")
                                        {
                                            //Program.Orden[j][8] = "Abierta";
                                            iBandera = 1;
                                        }

                                        //INSTRUCCIONES PARA SUMAR EL VALOR DEL DETALLE DE LA ORDEN
                                        //UTILIZAREMOS EL DATATABLE DE  LA INSTRUCCION DE LA MESA
                                        dtConsultaMesa = new DataTable();
                                        dtConsultaMesa.Clear();

                                        sSql = "";
                                        sSql += "select DP.cantidad, DP.precio_unitario, DP.valor_dscto, DP.valor_iva, DP.valor_otro" + Environment.NewLine;
                                        sSql += "from cv403_det_pedidos as DP, cv403_cab_pedidos as CP" + Environment.NewLine;
                                        sSql += "where CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                                        sSql += "and CP.id_pedido = " + iIdPedido + Environment.NewLine;
                                        sSql += "and CP.estado in ('A', 'N')" + Environment.NewLine;
                                        sSql += "and DP.estado in ('A', 'N')";

                                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsultaMesa, sSql);

                                        if (bRespuesta == true)
                                        {
                                            DSumaDetalleOrden = 0;

                                            for (int k = 0; k < dtConsultaMesa.Rows.Count; k++)
                                            {
                                                dbCantidad = Convert.ToDouble(dtConsultaMesa.Rows[k][0].ToString());
                                                dbPrecioUnitario = Convert.ToDouble(dtConsultaMesa.Rows[k][1].ToString());
                                                dbDescuento = Convert.ToDouble(dtConsultaMesa.Rows[k][2].ToString());
                                                dbIva = Convert.ToDouble(dtConsultaMesa.Rows[k][3].ToString());
                                                dbServicio = Convert.ToDouble(dtConsultaMesa.Rows[k][4].ToString());

                                                dbPrecioUnitario = dbCantidad * dbPrecioUnitario;
                                                dbDescuento = dbCantidad * dbDescuento;
                                                dbIva = dbCantidad * dbIva;

                                                if (Program.iManejaServicio == 1)
                                                {
                                                    if (dbCantidad >= 1)
                                                    {
                                                        dbServicio = dbCantidad * dbServicio;
                                                    }
                                                }

                                                DSumaDetalleOrden = DSumaDetalleOrden + dbPrecioUnitario + dbIva + dbServicio - dbDescuento;
                                            }
                                        }

                                        //Mesa
                                        t_st_linea1 = "Número de Cuenta: " + iNumeroCuentaDiaria.ToString() + "   Número de Orden: " + (iNumeroPedido).ToString() + "  Mesero: " + sNombreMesero + "      Total: " + "$ " + DSumaDetalleOrden.ToString("N2");
                                        t_st_linea2 = "Fecha y Hora de la Orden: " + sFechaIngresoOrden;
                                        t_st_linea3 = sTipoOrden + " # : " + sNombreMesa + " -  Nº de Personas: " + iNumeroPersonas.ToString() + "\n Orden " + sEstadoOrden;

                                        //A domicilio
                                        t_st_linea4 = "Número de Cuenta: " + iNumeroCuentaDiaria.ToString() + "   Número de Orden: " + (iNumeroPedido).ToString() + "  Mesero: " + sNombreMesero + "      Total: " + "$ " + DSumaDetalleOrden.ToString("N2");
                                        t_st_linea5 = "Fecha y Hora de la Orden: " + sFechaIngresoOrden;
                                        t_st_linea6 = sTipoOrden + " - QUITO - ECUADOR" + "\n Orden " + sEstadoOrden;

                                        //Para llevar

                                        t_st_linea7 = "Número de Cuenta: " + iNumeroCuentaDiaria.ToString() + "   Número de Orden: " + (iNumeroPedido).ToString() + "  Mesero: " + sNombreMesero + "      Total: " + "$ " + DSumaDetalleOrden.ToString("N2");
                                        t_st_linea8 = "Fecha y Hora de la Orden: " + sFechaIngresoOrden;
                                        t_st_linea9 = sTipoOrden + "\n Orden " + sEstadoOrden;

                                        if (sTipoOrden == "MESAS")
                                        {
                                            boton[i].Text = t_st_linea1 + "\n" + t_st_linea2 + "\n" + t_st_linea3;
                                        }

                                        else if (sTipoOrden == "DOMICILIOS")
                                        {
                                            boton[i].Text = t_st_linea4 + "\n" + t_st_linea5 + "\n" + t_st_linea6;
                                        }

                                        else if (sTipoOrden == "PARA LLEVAR")
                                        {
                                            boton[i].Text = t_st_linea7 + "\n" + t_st_linea8 + "\n" + t_st_linea9;
                                        }

                                        else
                                        {
                                            boton[i].Text = t_st_linea7 + "\n" + t_st_linea8 + "\n" + t_st_linea9;
                                        }
                                    }
                                }

                                pnlOrdenes.Controls.Add(boton1[i]);
                                pnlOrdenes.Controls.Add(botonEditar[i]);
                                pnlOrdenes.Controls.Add(boton[i]);
                                h++;
                            }
                        }

                        else
                        {
                            //SE PUEDE OMITIR RELLENAR ESTA LINEA YA QUE NO DEVUELVE INFORMACION
                            //btnTotalOrdenes.Text = "Total de Órdenes \n0";
                        }
                    }

                    else
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                    }
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        private void botonImprimir_clic(object sender, EventArgs e)
        {
            //PARA ABRIR EL FORMULARIO ORIGINAL
            Button botonsel = sender as Button;

            sSql = "";
            sSql += "select estado_orden" + Environment.NewLine;
            sSql += "from cv403_cab_pedidos" + Environment.NewLine;
            sSql += "where id_pedido = " + Convert.ToInt32(botonsel.Name);

            dtConsulta = new DataTable();
            dtConsulta.Clear();

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    Pedidos.frmVerPrecuentaTextBox precuenta = new Pedidos.frmVerPrecuentaTextBox(botonsel.Name, 1, dtConsulta.Rows[0][0].ToString());
                    precuenta.ShowDialog();    
                }
            }

            else
            {
                ok.LblMensaje.Text = "Ocurrió un problema al imprimir la precuenta.";
                ok.ShowDialog();
            }
        }

        private void botonEditar_Click(object sender, EventArgs e)
        {
            //PARA ABRIR EL FORMULARIO ORIGINAL
            Button btnEditarComanda = sender as Button;
            
            if (Convert.ToInt32(btnEditarComanda.Tag) == 1)
            {
                if (Program.iPuedeCobrar == 1)
                {
                    reabrirComanda(Convert.ToInt32(btnEditarComanda.Name));
                }

                else
                {
                    ok.LblMensaje.Text = "Su usuario no le permite reabrir la cuenta.";
                    ok.ShowDialog();
                }
            }

            else if (Convert.ToInt32(btnEditarComanda.Tag) == 3)
            {
                ok.LblMensaje.Text = "No se puede reabrir una comanda que ha sido cancelada o eliminada.";
                ok.ShowDialog();
            }

            else if (Convert.ToInt32(btnEditarComanda.Tag) == 2)
            {
                recuperarComanda(Convert.ToInt32(btnEditarComanda.Name));
            }
        }
        
        private void boton_clic(object sender, EventArgs e)
        {
            //PARA ABRIR EL FORMULARIO ORIGINAL
            Button botonsel = sender as Button;
            //Prueba p = new Prueba(botonsel.Name);
            Pedidos.frmVerReporteRevisar p = new Pedidos.frmVerReporteRevisar(botonsel.Name);
            p.ShowDialog();

            if (p.DialogResult == DialogResult.OK)
            {
                p.Close();
                this.Close();
            }
        }

        #endregion

        private void Revisar_Load(object sender, EventArgs e)
        {
            //Clases.ClaseRedimension redimension = new Clases.ClaseRedimension();
            //redimension.ResizeForm(this, Program.iLargoPantalla, Program.iAnchoPantalla);

            cmbLocalidades.SelectedIndexChanged -= new EventHandler(cmbLocalidades_SelectedIndexChanged);
            llenarComboLocalidades();
            cmbLocalidades.SelectedIndexChanged += new EventHandler(cmbLocalidades_SelectedIndexChanged);

            Program.dbDescuento = 0;
            Program.dbValorPorcentaje = 0;
            btnOrdenes.Text = "Órdenes Abiertas";
            sFechaActual = DateTime.Now.ToString("yyyy/MM/dd");
            //btnFecha.Text = DateTime.Now.ToString("yyyy/MM/dd");
            btnFecha.Text = sFechaActual.Substring(8, 2) + "/" + sFechaActual.Substring(5, 2) + "/" + sFechaActual.Substring(0, 4);
            
            if (Program.iActivaTeclado == 1)
            {
                activaTeclado();
            }

            else
            {
                this.ActiveControl = txtBusqueda;
            }

            pnlOrdenes.Controls.Clear();

            //using (VentanasMensajes.frmMensajeEspere espere = new VentanasMensajes.frmMensajeEspere())
            //{
            //    espere.AccionEjecutar = mostrarBotones;

            //    if (espere.ShowDialog() != DialogResult.OK)
            //    {
            //        MessageBox.Show("ERROR");
            //    }
            //}

            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();           

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void Revisar_KeyDown(object sender, KeyEventArgs e)
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

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            btnOrdenes.Text = "Todas las Órdenes";

            if (txtBusqueda.Text == "")
            {
                ok.LblMensaje.Text = "Favor ingrese datos para la búsqueda.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }
            else
            {
                sFechaActual = DateTime.Now.ToString("yyyy/MM/dd");

                if (btnBusquedaOrdenCuenta.Text == "Por número de orden")
                {
                    iOp = 3;
                }

                else
                {
                    iOp = 7;
                }

                pnlOrdenes.Controls.Clear();
                //mostrarBotones();

                espere.AccionEjecutar = mostrarBotones;
                espere.ShowDialog();

                btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();

                txtBusqueda.Text = "";
            }
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            DateTime fecha = Convert.ToDateTime(btnFecha.Text);
            fecha = fecha.AddDays(1);
            sFechaActual = fecha.ToString("yyyy/MM/dd");
            //btnFecha.Text = fecha.ToString("yyyy/MM/dd");
            btnFecha.Text = sFechaActual.Substring(8, 2) + "/" + sFechaActual.Substring(5, 2) + "/" + sFechaActual.Substring(0, 4);
            pnlOrdenes.Controls.Clear();
            iOp = 1;
            
            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void btnBajar_Click(object sender, EventArgs e)
        {
            DateTime fecha = Convert.ToDateTime(btnFecha.Text);
            fecha = fecha.AddDays(-1);
            sFechaActual = fecha.ToString("yyyy/MM/dd");
            //btnFecha.Text = fecha.ToString("yyyy/MM/dd");
            btnFecha.Text = sFechaActual.Substring(8, 2) + "/" + sFechaActual.Substring(5, 2) + "/" + sFechaActual.Substring(0, 4);
            pnlOrdenes.Controls.Clear();
            iOp = 1;
            
            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            //if (espere.ShowDialog() != DialogResult.OK)
            //    {
            //        MessageBox.Show("ERROR");
            //    }

            

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void btnOrdenes_Click(object sender, EventArgs e)
        {
            sFechaActual = DateTime.Now.ToString("yyyy/MM/dd");
            //btnFecha.Text = sFechaActual.Substring(8, 2) + "/" + sFechaActual.Substring(5, 2) + "/" + sFechaActual.Substring(0, 4);
            btnFecha.Text = Convert.ToDateTime(sFechaActual).ToString("dd/MM/yyyy");

            if (btnOrdenes.Text == "Todas las Órdenes")
            {
                iOp = 1;
                pnlOrdenes.Controls.Clear();

                espere.AccionEjecutar = mostrarBotones;
                espere.ShowDialog();

                //using (VentanasMensajes.frmMensajeEspere espere = new VentanasMensajes.frmMensajeEspere())
                //{
                //    espere.AccionEjecutar = mostrarBotones;

                //    if (espere.ShowDialog() != DialogResult.OK)
                //    {
                //        MessageBox.Show("ERROR");
                //    }
                //}

                btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();

                btnOrdenes.Text = "Órdenes Abiertas";

            }
            else
            {
                pnlOrdenes.Controls.Clear();
                iOp = 2;

                espere.AccionEjecutar = mostrarBotones;
                espere.ShowDialog();

                //using (VentanasMensajes.frmMensajeEspere espere = new VentanasMensajes.frmMensajeEspere())
                //{
                //    espere.AccionEjecutar = mostrarBotones;

                //    if (espere.ShowDialog() != DialogResult.OK)
                //    {
                //        MessageBox.Show("ERROR");
                //    }
                //}

                btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();

                btnOrdenes.Text = "Todas las Órdenes";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDomicilio_Click_1(object sender, EventArgs e)
        {
            pnlOrdenes.Controls.Clear();
            iOp = 4;

            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void btnMesa_Click_1(object sender, EventArgs e)
        {
            pnlOrdenes.Controls.Clear();
            iOp = 5;

            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void btnLlevar_Click_1(object sender, EventArgs e)
        {
            pnlOrdenes.Controls.Clear();
            iOp = 6;

            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void btnCanceladas_Click(object sender, EventArgs e)
        {
            pnlOrdenes.Controls.Clear();
            iOp = 8;

            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void btnRetroceder_Click(object sender, EventArgs e)
        {
            string str;
            int loc;

            if (txtBusqueda.Text.Length > 0)
            {

                str = txtBusqueda.Text.Substring(txtBusqueda.Text.Length - 1);
                loc = txtBusqueda.Text.Length;
                txtBusqueda.Text = txtBusqueda.Text.Remove(loc - 1, 1);
            }

            txtBusqueda.Focus();
            txtBusqueda.SelectionStart = txtBusqueda.Text.Trim().Length;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            concatenarValores(btn1.Text);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            concatenarValores(btn2.Text);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            concatenarValores(btn3.Text);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            concatenarValores(btn4.Text);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            concatenarValores(btn5.Text);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            concatenarValores(btn6.Text);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            concatenarValores(btn7.Text);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            concatenarValores(btn8.Text);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            concatenarValores(btn9.Text);
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            concatenarValores(btn0.Text);
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBusqueda_Click(sender, e);
            }
        }

        private void btnFecha_Click(object sender, EventArgs e)
        {
            Pedidos.frmCalendario calendario = new Pedidos.frmCalendario(btnFecha.Text);
            calendario.ShowDialog();

            if (calendario.DialogResult == DialogResult.OK)
            {
                btnFecha.Text = calendario.txtFecha.Text;
                sFechaActual = btnFecha.Text.Substring(6, 4) + "/" + btnFecha.Text.Substring(3, 2) + "/" + btnFecha.Text.Substring(0, 2);
                pnlOrdenes.Controls.Clear();
                iOp = 1;

                espere.AccionEjecutar = mostrarBotones;
                espere.ShowDialog();                

                btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();

                //using (VentanasMensajes.frmMensajeEspere espere = new VentanasMensajes.frmMensajeEspere())
                //{
                //    espere.AccionEjecutar = mostrarBotones;

                //    if (espere.ShowDialog() != DialogResult.OK)
                //    {
                //        MessageBox.Show("ERROR");
                //    }
                //}

                //btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
                //pnlOrdenes = pnlCrear;
            }
        }

        private void btnBusquedaOrdenCuenta_Click(object sender, EventArgs e)
        {
            if (btnBusquedaOrdenCuenta.Text == "Por número de orden")
            {
                btnBusquedaOrdenCuenta.Text = "Por número de cuenta";
            }

            else
            {
                btnBusquedaOrdenCuenta.Text = "Por número de orden";
            }
        }

        private void btnTotalOrdenes_Click(object sender, EventArgs e)
        {
            iOp = 1;
            pnlOrdenes.Controls.Clear();

            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }

        private void btnMisOrdenes_Click(object sender, EventArgs e)
        {
            Pedidos.frmCobrarAlmuerzos almuerzo = new Pedidos.frmCobrarAlmuerzos();
            almuerzo.ShowDialog();
        }

        private void cmbLocalidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlOrdenes.Controls.Clear();

            espere.AccionEjecutar = mostrarBotones;
            espere.ShowDialog();

            btnTotalOrdenes.Text = "Total de Órdenes" + Environment.NewLine + iCuenta.ToString();
        }
    }
}