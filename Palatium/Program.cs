using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ConexionBD;
using Palatium;
using System.IO;
using System.Data;

namespace Palatium
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Program.asignar();

            //if (ayudaOrden == 0)
            //    EstadodeOrden = "Abierta";
            //else
            //    EstadodeOrden = "Pagada";

            
            //LLAMO A LA CLASE ENVIANDO LOS PARAMETROS DE CONEXION
            ConexionBD.ConexionBD conectar = new ConexionBD.ConexionBD();
            
            Clases.ClaseCargarParametros parametros = new Clases.ClaseCargarParametros();
            Clases.ClaseRedimension redimension = new Clases.ClaseRedimension();
            Clases.ClaseLlenarMonedas monedas = new Clases.ClaseLlenarMonedas();

            VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
            VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
            string sMensaje;
            
            string path = "C:\\palatium\\config.ini";

            if (File.Exists(path))
            {
                if (conectar.lecturaConfiguracion(path) == true)
                {
                    iIdEmpresa = Convert.ToInt32(conectar.id_Empresa);
                    iCgEmpresa = Convert.ToInt32(conectar.Cg_Empresa);
                    iIdLocalidad = Convert.ToInt32(conectar.id_Localidad);
                    iCgMotivoDespacho = Convert.ToInt32(conectar.Motivo_Despacho);

                    SQLBDATOS = conectar.SQLBDATOS;
                    SQLCONEXION = conectar.SQLCONEXION;
                    SQLSERVIDOR = conectar.SQLSERVIDOR;
                    SQLDNS = conectar.SQLDSN_ODBC;

                    sMensaje = parametros.cargarParametros();

                    if (sMensaje != "")
                    {
                        catchMensaje.LblMensaje.Text = sMensaje;
                        catchMensaje.ShowInTaskbar = false;
                        catchMensaje.ShowDialog();
                    }

                    sMensaje = parametros.cargarParametrosPredeterminados();
                    
                    if (sMensaje != "")
                    {
                        catchMensaje.LblMensaje.Text = sMensaje;
                        catchMensaje.ShowInTaskbar = false;
                        catchMensaje.ShowDialog();
                    }

                    //AQUI PARA LLENAR LA CONFIGURACION DE FACTURACION ELECTRONICA
                    if (Program.iFacturacionElectronica == 1)
                    {
                        parametros.cargarParametrosFacturacionElectronica();
                    }

                    sMensaje = parametros.cargarDatosTerminal();
                    if (sMensaje != "")
                    {
                        catchMensaje.LblMensaje.Text = sMensaje;
                        catchMensaje.ShowInTaskbar = false;
                        catchMensaje.ShowDialog();
                    }

                    parametros.cargarDatosImpresion();
                    parametros.cargarDatosEmpresa();
                    monedas.llenarMonedas();

                    redimension.extraerPixelado();

                    if (Program.iHabilitarDecimal == 1)
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CO");
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";

                        Program.iArregloMenuFila = 12;
                        Program.iArregloMenuColumna = 4;
                    }

                    else
                    {
                        Program.iArregloMenuFila = 10;
                        Program.iArregloMenuColumna = 5;
                    }

                    //Application.Run(new Cajero.frmRecuperarDinero());
                    Application.Run(new frmVerMenu());
                    //Application.Run(new Comida_Rapida.frmComandaComidaRapida());
                }

                else
                {
                    VentanasMensajes.frmVerDatosConfig config = new VentanasMensajes.frmVerDatosConfig();
                    config.ShowDialog();
                    MessageBox.Show("No se pudo establecer la conexiòn.");
                }
            }

            else
            {
                MessageBox.Show("No existe el archivo de configuraciòn en la ruta " + path + "\nConsulte con el administrador.");                
            }                
        }

        //VARIABLES PARA FACTURACION ELECTRONICA
        public static int iTipoAmbiente;
        public static int iTipoEmision;
        public static int iTipoCertificado;

        public static string sNumeroRucEmisor;
        public static string sWebServiceEnvioPruebas;
        public static string sWebServiceConsultaPruebas;
        public static string sWebServiceEnvioProduccion;
        public static string sWebServiceConsultaProduccion;
        public static string sRutaCertificado;
        public static string sClaveCertificado;
        public static string sCorreoSmtp;
        public static string sCorreoEmisor;
        public static string sClaveCorreoEmisor;
        public static string sCorreoCopia;
        public static string sCorreoConsumidorFinal;
        public static string sCorreoAmbientePruebas;
        public static int iCorreoPuerto;
        public static int iManejaSSL;

        //VERSION DEL PRODUCTO
        public static string sVersionProducto = "2.1";

        //VARIABLE PARA ACTIVAR TECLADO VIRTUAL
        public static int iActivaTeclado;

        //VARIABLE PARA ACTIVAR LA VERSION DEMO
        public static int iVersionDemo;
        //-----------------------------------------------

        //VARIABLE PARA ACTIVAR EL DISEÑO DE MESAS
        public static int iDisenioMesas;

        //VARIABLE PARA ACTIVAR RISE
        public static int iManejaRise;

        //VARIABLE PARA ACTIVAR RISE
        public static string sUrlContabilidad;
        public static string sUrlReportes;

        //VARIABLE PARA SABER SI COBRAR CON O SIN IMPUESTOS LOS PRODUCTOS
        public static int iCobrarConSinProductos;

        //VARIABLE PARA SABER SI EMITE NOTA DE VENTA O FACTURA
        public static int iDescuentaIva;

        //VARIABLE PARA ACTIVAR Y DESACTIVAR VISTA PREVIA DE REPORTES
        public static int iVistaPreviaImpresiones;

        //VARIABLE PARA SELECCIONAR LAS OPCIONES DEL LOGIN
        public static int iUsuarioLogin;

        //VARIABLE PARA HABILITAR O INHABILITAR LAS DESCARGAS DE RECETA
        public static int iUsarReceta;

        //VARIABLE PARA EL ID DE MODIFICADOR
        public static int iIdProductoModificador;

        //VARIABLE PARA EL ID DE DOMICILIO
        public static int iIdProductoDomicilio;

        //VARIABLE PARA EL ID DE NUEVO ITEM
        public static int iIdProductoNuevoItem;

        //VARIABLE PARA HABILITAR NOTA DE VENTA
        public static int iSeleccionarNotaVenta;

        //VARIABLE PARA VER SI MANEJA NOMINA
        public static int iManejaNomina;

        //VARIABLE PARA MANEJO DE ALMUERZOS
        public static int iManejaAlmuerzos;

        //VARIABLE PARA EL NUMERO DE PERSONAS DEFAULT
        public static int iNumeroPersonasDefault;

        //VARIABLE PARA LA EJECUCION DE LA IMPRESION
        public static int iEjecutarImpresion;

        //VARIABLE PARA PERMITIR ABRIR EL CAJÓN DE DINERO
        public static int iPermitirAbrirCajon;

        //VARIBALE QUE PERMITE APLICAR RECARGO A TARJETAS
        public static int iAplicaRecargoTarjeta;
        public static decimal dbPorcentajeRecargoTarjeta;
        public static int iComprobanteNotaEntrega;
        public static int iHabilitaOpciones;
        public static string sCorreoElectronicoDefault;

        public static int iCortar = 0;
        public static int iIdPersonaFacturador;
        public static string iIdentificacionFacturador;

        public static int iIdProductoAnular;
        public static double dValorProductoAnular;
        public static string sCodigoModificador;
        public static string sLogo;

        public static int iArregloMenuFila;
        public static int iArregloMenuColumna;

        public static int iBanderaNumeroMesa = 1;
        public static float iTamañoLetraMesa;
        public static int iBanderaCerrarVentana = 0;

        public static int iHabilitarDecimal;
        public static int iManejaNotaVenta;

        public static int iAnchoPantalla;
        public static int iLargoPantalla;

        public static double dCambioPantalla= 0;
        public static double dValorFacturado;

        //VARIABLE PARA GUARDAR LA ETIQUETA DE USUARIO EN EL SISTEMA
        public static string sEtiqueta;
        public static string sEtiquetaAdministrador;

        //SECCION DE DATATABLES
        public static string[,] sDetallesItems = new string[100, 100];
        //public static string[,] sContarDinero = new string[12, 3];
        public static string[,] sContarDinero = { { "1", "0", "0.00" }, { "2", "0", "0.00" }, { "5", "0", "0.00" }, { "10", "0", "0.00" }, { "20", "0", "0.00" }, { "50", "0", "0.00" }, { "100", "0", "0.00" }, { "1 Ctvo.", "0", "0.00" }, { "5 Ctvos.", "0", "0.00" }, { "10 Ctvos.", "0", "0.00" }, { "25 Ctvos.", "0", "0.00" }, { "50 Ctvos.", "0", "0.00" } };
        public static DataTable dtMonedasCierre;

        public static int iContadorDetalle = 0;
        public static int iContadorDetalleMximoX = 100;
        public static int iContadorDetalleMximoY = 100;

        public static int iVerCaja;
        public static int iCgTipoUnidad = 6142;

        public static string sNombreUsuario = "";
        //public static string sNombreUsuarioAdministracion;
        public static string sNombreTerminal;
        public static string sEstadoUsuario;
        public static string sNombreCajeroDefault;
        public static string sCierreCajero;
        public static DateTime sFechaSistema;

        public static int iPuedeCobrar;
        public static int iFacturacionElectronica;

        public static int iCgLocalidad;
        public static int iIdMesero;
        public static int iIdCajeroDefault;
        public static int iFormatoFactura;
        public static int iFormatoPrecuenta;
        public static int iImprimirCocina;
        public static int iIdTerminal;
        public static int iImprimirDatosFactura;
        public static string sCiudadDefault;
        public static int iSeleccionMesero;
        public static int iManejaServicioOrden;
        public static int iIdImpresoraReportes;

        //public static int iConsumidorFinal;
        public static int iIdVendedor;
        public static int iManejaJornada;
        public static int iMostrarJornada = 0;
        public static int iJornadaRecuperada;

        public static int iUnidadCompraConsumo = 6142;
        public static int iIdPersonaMovimiento;

        public static string sPasswordAdmin;

        public static string[] sDatosMaximo = new string[5];
        //public static string[] sDatosMaximoAdministracion = new string[5];

        public static int iModoDebug = 0;
        //VARIABLES PARA LA CONSULTA EN LA TABLA POS_ORIGEN_ORDEN
        public static int iIdOrigenOrden;
        public static string sDescripcionOrigenOrden;
        public static int iGeneraFactura;
        public static int iIdPersonaOrigenOrden;
        public static int iIdPosModoDelivery;
        public static int iPresentaOpcionDelivery;
        public static string sCodigoAsignadoOrigenOrden;
        public static string sIdentificacion;

        public static string sIdGrid;
        public static string sFormaPagoGrid;

        //VARIABLES GLOABLES PARA EVITAR REPETIR 

        public static int iBanderaReabrir = 0;

        public static string SQLBDATOS;
        public static string SQLCONEXION;
        public static string SQLSERVIDOR;
        public static string SQLDNS;

        public static int iIdEmpresa;
        public static int iCgEmpresa;
        public static int iIdPersona;
        public static int iIdLocalidad;
        public static int iCgMotivoDespacho;
        public static string sPuntoPartida = "Matriz Quito";
        public static int iCgCiudadEntrega = 0;
        public static string sDireccionEntrega = "Matriz Quito";
        public static int iCgEstadoDespacho = 6970;
        public static int iMoneda;
        public static int iIdFormularioSri = 19;
        public static double valorDescuento = 0;
        public static string sMotivoDescuento = "";
        public static int iBanderaCliente = 0;
        public static int iDomicilioEspeciales = 0;
        public static int iModoDelivery = 0;
        public static double dDescuentoEmpleados = 25;
        public static double dPorcentajeEmpleados = 0;
        public static int iNuevoNumeroPersonas = 0;
        public static string iCodigoAreaTelefono = "02";

        public static int iIdPosCierreCajero;
        public static string sFechaAperturaCajero;
        public static string sEstadoCajero;
        public static int iJornadaCajero;

        public static int iIdProduto = 0;

        public static int iCuentaDiaria=0;

        public static string sMotivoProductoCancelado;
        public static int iBanderaCortesia = 0;

        //VARIABLES SOLO PARA USAR CON EFECTIVO o DINERO ELECTRONICO
        public static int iEfectivo = 1;
        public static int iDineroElectronico = 11;

        public static Double dPropinas = 0;

        public static string sMotivoCortesia;
        public static int iOrigenOrden;

        public static int iIdPosCierreCaja;

        //Arreglos para guardar nombre de productos
        public static int iCuenta = 0;
        public static string[] sNombreProductos;
        public static string[] sCantidadProductos;
        public static double[] dPreciosProductos;

        //Variables para recuperar datos de la orden
        public static int iNumeroDeOrden;
        public static int iIdCabPedido;
        public static int iIdPosMesa;
        public static string sNombreMesa;
        public static string sNombreCajero;
        public static string sCorreoElectronico;
        public static string sNOmbreOrigenOrden;
        public static string sFechaOrden;
        public static int iNumeroPersonas;

        //VARIABLE PARA GUARDAR EL NUMERO DE ORDEN PADRE
        public static string sREFERENCIA_ORDEN = null;
        //VARIABLE PARA GUARDAR EL NOMBRE DE LA MESA
        public static string sNOMBREMESA = null;
        //VARIABLE PARA GUARDAR LA IDENTIFICACION DEL CLIENTE
        public static string sIDENTIFICACION = null;
        //VARIABLE PARA GUARDAR EL TIPO DE ORDEN
        public static string sIDTIPOSORDEN = null;
        public static string sTIPOSORDEN = null;
        //VARIABLE PARA GUARDAR EL ID DEL PAGO
        public static int iIdPago = 0;
        //VARIABLE PARA GUARDAR LA POSICION DEL VECTOR ORDEN PARA GUARDAR LA POSICION
        public static int iPosicionOrden = 0;
        //FUNCION PÀRA GUARDAR EL ID PERSONA
        public static string sIDPERSONA = null;
        //VARIABLE PARA ABRIR EL FACTURADOR
        public static int iVERIFICADOR=0;
        //VARIABLE PARA ALMACENAR LA JORNADA
        public static int iJORNADA = 0;
        //VARIABLE PARA GUARDAR TEMPORALMENTE EL ID DE LA MESA
        public static int iIDMESA = 0;
               
        //Arreglo para guardar el cambio
        public static double[,] dbCambio = new double[100, 100];

        //Bandera para controlar el estado de la mesa
        public static int iBaderaColorMesa = 0;

        //
        public static double[,] dbTotalDescuento = new double[100, 2];
        //Controlador para Descuento
        public static int iControlaorDescuento = 0;

        //nuevo insertar
        //VARIABLES PARA MANEJO DE TRANSACCION
        public const int G_INICIA_TRANSACCION = 1;
        public const int G_TERMINA_TRANSACCION = 2;
        public const int G_REVERSA_TRANSACCION = 3;

        public static string G_st_query = "";
        public static string GFun_St_Saca_Campo = "";
        public static string GFun_In_Mensaje = "";
        public static string G_st_fecha = "";
        public static string G_st_mensaje = "";

        //===================================================================================
        //===================================================================================
        //Arreglo para guardar datos del cliente
        public static string[,] sClientes = new string[100, 10];
        public static int CLI_CODIGO = 0;
        public static int CLI_CEDULA = 1;
        public static int CLI_NOMBRE_CLIENTE = 2;
        public static int CLI_TELEFONO = 3;
        public static int CLI_CIUDAD = 4;
        public static int CLI_SECTOR = 5;
        public static int CLI_CALLE_PRINCIPAL = 6;
        public static int CLI_CALLE_SECUNDARIA = 7;
        public static int CLI_NUMERACION = 8;
        public static int CLI_REFERENCIA = 9;


        //===================================================================================
        //===================================================================================
        //CODIGO NATIVO DE ESTA SECCION PARA ABAJO
        //===================================================================================
        //===================================================================================

        //Variables para controlar el acceso del cajero
        public static int CAJERO_ID;
        public static int CAJERO_NOMBRE = 1;

        //Variables para controlar los descuentos
        public static double dbValorPorcentaje = 0;
        public static double dbDescuento = 0;

        //Controlador para motivo cortesía
        public static int iControladroMotivoCortesia = 0;

        //Arreglo para guardar el motivo de la cancelación de un producto
        public static string[,] sMotivoCancelacion = new string[100, 2];
        //Controlador para motivo de cancelación
        public static int iControladorMotivoCancelacion = 0;

        //Arreglo para guardar las propinas
        public static double[,] dTotalDePropinas = new double[1000, 2];
        //Controlador para propinas
        public static int iControladorPropinas = 0;

        //Arreglo para guardar el valor de órdenes canceladas
        public static string[,] sTotalOrdenesCanceladas = new string[100, 2];
        //Controlador para órdenes canceladas
        public static int iControladorOrdenesCanceladas = 0;


        //Arreglo para guardar el valor del pago parcial
        public static string[,] dPagoParcial = new string[100, 2];
        //Controlador para Pago Parcial
        public static int icontroladorPagoParcial = 0;


        //ARREGLO PARA GUARDAR EL NÚMERO DE ORDEN DE ÓRDENES HIJAS
        public static int[] numeroOrdenHija = new int[100];

        //Tabla para ver si es hija
        public static string[,] tablaHijas = new string[100, 2];

        //Arreglo para guardar los items de cortesía
        public static string[,] sProductosCortesias = new string[100, 4];
        //Controlador para el número de productos de cortesía
        public static int iControladorCortesias = 0;

        //Arreglos para guardar Produtos Cancelados
        public static string[,] sProductosCancelados = new string[100, 100];
        //Controlador para el numero de productos cancelados
        public static int iControladorProductos = 0;

        //Definición de Constantes\\
        //Constantes Mesas
        public static int NUMERO_MESAS_LARGO = 6;
        public static int NUMERO_MESAS_ANCHO;
        //Constantes Orden

        //ESTA VARIABLE LA REUTILIZAMOS PARA RENOMBRAR LA MESA CUANDO SE LA DIVIDE
        //========================================================================
        public static int ORD_NOMBRE_CLIENTE = 11;
        //========================================================================
        
        public static int ORD_TELEFONO_CLIENTE = 12;
        public static int ORD_SECTOR_CLIENTE = 13;
        public static int ORD_CALLE_PRINCIPAL_CLIENTE = 14;
        public static int ORD_CALLE_SECUNDARIO_CLIENTE = 15;
        public static int ORD_CALLE_NUMERO = 16;
        public static int ORD_REFERENCIA_CLIENTE = 17;
        public static int ORD_RECARGO = 18;
        public static int ORD_ID_ORDEN_PADRE = 19;
        public static int ORD_CIUDAD_CLIENTE = 20;
        public static int ORD_MAIL_CLIENTE = 21;

        //Constantes Detalle Orden
        public static int DET_ORDEN_ID_ORDEN = 0;
        public static int DET_ORDEN_CANTIDAD = 1;
        public static int DET_ORDEN_NOMBRE_PRODUCTO = 2;
        public static int DET_ORDEN_VALOR_PRODUCTO = 3;
        public static int DET_ORDEN_VALOR_TOTAL = 4;
        public static int DET_ORDEN_ID_DETALLE_ORDEN = 5;
        public static int DET_ORDEN_ID_PRODUCTO = 6;

        //Constantes Pagos
        public static int PAGOS_ID_PAGOS = 0;
        public static int PAGOS_ID_ORDEN = 1;
        public static int PAGOS_FECHA = 2;
        public static int PAGOS_TOTAL = 3;

        //Constantes Detalle Pagos
        public static int DETALLEPAGO_ID = 0;
        public static int DETALLEPAGO_ID_PAGO = 1;
        public static int DETALLEPAGO_ORIGEN = 2;
        public static int DETALLEPAGO_VALOR = 3;

        //Constante del largo del grid en el conteo resumen
        public static int LARGO_GRID = 4;

        //Constantes Formas Pago
        public static int FORMASPAGO_ID = 0;
        public static int FORMASPAGO_DESCRIPCION = 1;

        //Variable para controlar el codigo de reabrir Mesas
        public static int contadorDeLasMesas = 1;
        //Variables para los totales de cuentas canceladas
        public static int TotalCuentasCanceladas = 0;
        public static int iTotalCuentasMesa = 0;
        public static float fTotalValorMesa = 0;
        public static int iTotalCuentasDomicilio = 0;
        public static int iTotalCuentasLlevar = 0;
        public static int iTotalCuentasCanjes = 0;
        public static int iTotalCuentasConsumoEmpleados = 0;
        public static int iTotalCuentasCortesias = 0;
        public static int iTotalCuentasFuncionarios = 0;
        public static int iContadorPersonas = 0;
        public static int iTotalCuentasMenuExpress = 0;


        //Tabla Mesas
        public static string[,] tablaMesas = new string[30, 2];

        //Boton Global
        public static Button botonGlobal1;
        public static Button botonGlobal2;

        //Validar Mesa
        public static bool mesausada;

        //Control De Mesas
        public static Button controlMesa;

        //Datos Domicilio
        public static string[,] DatosDomicilio = new string[,]{
                { "1111", "Diaz", "Juan","juan@gmail.com","1873826475","la gasca","234","Alejandro de valdez","La gascaSector","estadio"},
                { "2222", "Aguilar", "Luis","luis@gmail.com","9483774657","Amaguaña","884","Valdez de Alejandro","La gascaSector","estadio"},
                { "3333", "Muñoz", "Juan","juan@gmail.com","1873826475","la gasca","234","Alejandro de valdez","La gascaSector","estadio"},
                { "4444", "Correa", "Luis","luis@gmail.com","9483774657","Amaguaña","884","Valdez de Alejandro","La gascaSector","estadio"}
            };

        //Dirección de Clientes
        public static string cedula;
        public static string cliente;
        public static string telefonoCliente;
        public static string sectorCliente;
        public static string callePrincipal;
        public static string calleSecundaria;
        public static string numeroCliente;
        public static string referenciaCliente;


        //Estado
        public static int ayudaOrden = 0;
        public static string EstadodeOrden;

        //entrada y cierre de caja
        public static string[] entradaSalida;

        public static string entrada = "";
        public static string salida = "";
        public static string horaEntrada = "";
        public static string horaSalida = "";
        public static string fecha = "";

        //Resumen de cierre de caja
        public static string direccion2 = "República del Salvador";
        public static string jornada = "DIURNA";

        public static double totalEfectivo = 0;

        //Iva y Recargo
        public static double iva;
        public static double ice;
        public static double servicio;
        public static double descuento_empleados;
        public static int iLeerMesero;
        public static int iImprimeOrden;
        public static int iManejaServicio;
        public static double motorizado = 1.50;

        public static double factorPrecio = 1;

        //Nuevo número de Personas
        public static string nuevoPersonas = "";
        public static string nPersonas = "";
        //Nueva Orden
        public static string nuevaOrden = "";

        //Variables Local Impresion Datos
        #region Variables Local Impresion Datos
        public static string local;
        public static string direccion;
        public static string telefono1;
        public static string telefono2;
        public static string nombreMesero;
        #endregion


        ///clientes
        public static string Cli_telefono = "";
        public static string Cli_cedula = "";
        public static string Cli_sector = "";
        public static string Cli_nombre = "";
        public static string Cli_callePrincipal = "";
        public static string Cli_secundaria = "";
        public static string Cli_referencias = "";
        public static string Cli_numero = "";
        public static string Cli_apellido = "";
        public static string Cli_correo = "";
        public static string Cli_diudad = "";


        //Control indice detalle pedido
        public static int maximodetallePedido = 0;
        //Control indice detallePagos
        public static int maximodetallePagos = 0;
        //pagos


        //=======================================================================
        //MATRIZ PAGOS CON SUS CONSTANTES
        public static string[,] pagos = new string[100, 4];



        //=======================================================================
        //MATRIZ DETALLE_PAGO CON SUS CONSTANTES
        public static string[,] detallePago = new string[100, 4];
        //public int 


        //Domicilio (Fernando 10/03/2018)
        //Valor de movilizacion a domicilio
        public static string valorMovilizacion = "1.50";
        public static string domicilio = "";
        public static string interseccion = "";
        public static string referencias = "";
        public static string numero = "";
        public static string sector = "";

        //Nuevo número de Personas


        public static double G_Dbl_total;
        public static double G_Dbl_abono;
        public static double G_Dbl_saldo;

        //Areglo para guardar las formas de pago
        public static string[,] formasPago;

        //bandera
        //public static int bandera;
        //public static int ayudante;


        //número de Orden
        public static int nPedido = 0;
        public static int nOrden = 1000;
        public static int b = 1;
        public static int controlPedido = 0;
        public static int bandera2 = 0;

        //Última Orden
        public static int ultimaOrden;

        public static int filas = 2, columnas = 10;


    }
}
