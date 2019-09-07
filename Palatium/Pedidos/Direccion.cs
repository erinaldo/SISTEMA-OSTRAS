using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium
{
    public partial class Direccion : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        Clases.ClaseValidarRUC validarRuc = new Clases.ClaseValidarRUC();
        ValidarCedula validarCedula = new ValidarCedula();
        
        bool bRespuesta = false;

        DataTable dtConsulta;

        string sSql;
        string sIdPersona;
        string sTabla;
        string sCampo;
        string sIdentificacionRespaldo;

        int iIdPersona;
        int iIdTelefono;
        int iIdDireccion;
        int iTercerDigito;
        int iIdTipoPersona;
        int iIdTipoIdentificacion;
        int iBandera;
        
        long iMaximo;

        public Direccion(string sIdPersona)
        {
            this.sIdPersona = sIdPersona;
            this.iIdPersona = Convert.ToInt32(sIdPersona);
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //CONSULTAR DATOS EN LA BASE
        private void consultarRegistro()
        {
            try
            {
                sIdentificacionRespaldo = txtIdentificacion.Text.Trim();

                sSql = "";
                sSql = sSql + "SELECT TP.id_persona, TP.identificacion, TP.nombres, TP.apellidos, TP.correo_electronico," + Environment.NewLine;
                sSql = sSql + "TD.direccion, TD.calle_principal, TD.numero_vivienda, TD.calle_interseccion, TD.referencia," + Environment.NewLine;
                sSql = sSql + "isnull(TT.domicilio, TT.oficina) domicilio, TT.celular" + Environment.NewLine;
                sSql = sSql + "FROM dbo.tp_personas TP" + Environment.NewLine;
                sSql = sSql + "LEFT OUTER JOIN dbo.tp_direcciones TD ON TP.id_persona = TD.id_persona" + Environment.NewLine;
                sSql = sSql + "and TP.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and TD.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "LEFT OUTER JOIN dbo.tp_telefonos TT ON TP.id_persona = TT.id_persona" + Environment.NewLine;
                sSql = sSql + "and TT.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "WHERE  TP.identificacion = '" + txtIdentificacion.Text.Trim() + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdPersona = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                        txtNombres.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                        txtApellidos.Text = dtConsulta.Rows[0].ItemArray[3].ToString();
                        txtMail.Text = dtConsulta.Rows[0].ItemArray[4].ToString();
                        txtSector.Text = dtConsulta.Rows[0].ItemArray[5].ToString();
                        txtPrincipal.Text = dtConsulta.Rows[0].ItemArray[6].ToString();
                        txtNumeracion.Text = dtConsulta.Rows[0].ItemArray[7].ToString();
                        txtSecundaria.Text = dtConsulta.Rows[0].ItemArray[8].ToString();
                        txtReferencia.Text = dtConsulta.Rows[0].ItemArray[9].ToString();
                        //sCiudad = dtConsulta.Rows[0].ItemArray[8].ToString();

                        if (dtConsulta.Rows[0].ItemArray[10].ToString() == "")
                        {
                            txtTelefono.Text = dtConsulta.Rows[0].ItemArray[11].ToString();
                        }

                        else if (dtConsulta.Rows[0].ItemArray[11].ToString() == "")
                        {
                            txtTelefono.Text = dtConsulta.Rows[0].ItemArray[10].ToString();
                        }

                        else
                        {
                            txtTelefono.Text = dtConsulta.Rows[0].ItemArray[10].ToString();
                        }

                        txtApellidos.Focus();

                        //btnGuardar.Enabled = true;
                        //btnGuardar.Focus();
                    }

                    else
                    {
                        iIdPersona = 0;
                        txtIdentificacion.Clear();
                        txtApellidos.Clear();
                        txtNombres.Clear();
                        txtTelefono.Clear();
                        txtSector.Clear();
                        txtPrincipal.Clear();
                        txtNumeracion.Clear();
                        txtSecundaria.Clear();
                        txtReferencia.Clear();
                        txtIdentificacion.Focus();
                    }

                    //btnEditar.Visible = true;
                    goto fin;
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

        fin:
            { }
        }


        //FUNCION PARA VALIDAR LA CEDULA O RUC
        private void validarIdentificacion(int iOp)
        {
            try
            {
                iBandera = 0;

                if (txtIdentificacion.Text.Length >= 10)
                {
                    iTercerDigito = Convert.ToInt32(txtIdentificacion.Text.Substring(2, 1));
                }
                else
                {
                    goto mensaje;
                }

                if (txtIdentificacion.Text.Length == 10)
                {
                    if (validarCedula.validarCedulaConsulta(txtIdentificacion.Text.Trim()) == "SI")
                    {
                        //CONSULTAR EN LA BASE DE DATOS
                        iIdTipoPersona = 2447;
                        iIdTipoIdentificacion = 178;

                        if (iOp == 1)
                        {
                            consultarRegistro();
                        }

                        iBandera = 1;
                        goto fin;
                    }

                    else
                    {
                        goto mensaje;
                    }
                }

                else if (txtIdentificacion.Text.Length == 13)
                {
                    if (iTercerDigito == 9)
                    {
                        if (validarRuc.validarRucPrivado(txtIdentificacion.Text.Trim()) == true)
                        {
                            //CONSULTAR EN LA BASE DE DATOS
                            iIdTipoPersona = 2448;
                            iIdTipoIdentificacion = 179;

                            if (iOp == 1)
                            {
                                consultarRegistro();
                            }

                            iBandera = 1;
                            goto fin;
                        }

                        else
                        {
                            goto mensaje;
                        }

                    }

                    else if (iTercerDigito == 6)
                    {
                        if (validarRuc.validarRucPublico(txtIdentificacion.Text.Trim()) == true)
                        {
                            //CONSULTAR EN LA BASE DE DATOS
                            iIdTipoPersona = 2448;
                            iIdTipoIdentificacion = 179;

                            if (iOp == 1)
                            {
                                consultarRegistro();
                            }

                            iBandera = 1;
                            goto fin;
                        }

                        else
                        {
                            goto mensaje;
                        }
                    }

                    else if ((iTercerDigito <= 5) || (iTercerDigito >= 0))
                    {
                        if (validarRuc.validarRucNatural(txtIdentificacion.Text.Trim()) == true)
                        {
                            //CONSULTAR EN LA BASE DE DATOS
                            iIdTipoPersona = 2447;
                            iIdTipoIdentificacion = 179;

                            if (iOp == 1)
                            {
                                consultarRegistro();
                            }

                            iBandera = 1;
                            goto fin;
                        }

                        else
                        {
                            goto mensaje;
                        }
                    }

                    else
                    {
                        goto mensaje;
                    }
                }

                else
                {
                    goto mensaje;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }

        mensaje:
            {
                ok.LblMensaje.Text = "El número de identificación ingresado es incorrecto.";
                ok.ShowDialog();
                //txtIdentificacion.Clear();
                txtIdentificacion.Text = sIdentificacionRespaldo;
                txtIdentificacion.Focus();
            }
        fin:
            { }
        }


        //VERIFICAR SI LA CADENA ES UN NUMERO O UN STRING
        public bool esNumero(object Expression)
        {

            bool isNum;

            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

            return isNum;

        }

        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                //SE INICIA LA TRANSACCIÓN
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al realizar la transacción. Por favor comuníquese con el administrador en caso de continuar con el inconveniente.";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                    //Limpiar();
                    goto fin;
                }

                else
                {
                    //ACTUALIZAR EN LA TABLA TP_PERSONAS
                    sSql = "";
                    sSql = sSql + "update tp_personas set" + Environment.NewLine;
                    sSql = sSql + "apellidos = '" + txtApellidos.Text.Trim() + "'," + Environment.NewLine;
                    sSql = sSql + "nombres = '" + txtNombres.Text.Trim() + "'," + Environment.NewLine;
                    sSql = sSql + "codigo_alterno = '" + txtTelefono.Text.Trim()  + "'," + Environment.NewLine;
                    sSql = sSql + "correo_electronico = '" + txtMail.Text.Trim() + "'," + Environment.NewLine;
                    sSql = sSql + "cg_tipo_persona = " + iIdTipoPersona + "," + Environment.NewLine;
                    sSql = sSql + "cg_tipo_identificacion = " + iIdTipoPersona + Environment.NewLine;
                    sSql = sSql + "where id_persona = " + iIdPersona + Environment.NewLine;
                    sSql = sSql + "and estado = 'A'";

                    //EJECUTAMOS LA INSTRUCCIÒN SQL 
                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    //BUSCAMOS EL ID DE LA DIRECCION
                    //==================================================================================================================

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    sSql = "";
                    sSql = sSql + "select top 1 correlativo" + Environment.NewLine;
                    sSql = sSql + "from tp_direcciones" + Environment.NewLine;
                    sSql = sSql + "where id_persona = " + iIdPersona + Environment.NewLine;
                    sSql = sSql + "and estado = 'A'";

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count != 0)
                        {
                            iIdDireccion = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());


                            //ACTUALIZAMOS LA TABLA DE DIRECCIONES
                            //=================================================================================================================
                            sSql = "";
                            sSql = sSql + "update tp_direcciones set" + Environment.NewLine;
                            sSql = sSql + "direccion = '" + txtSector.Text.Trim() + "'," + Environment.NewLine;
                            sSql = sSql + "calle_principal = '" + txtPrincipal.Text.Trim() + "'," + Environment.NewLine;
                            sSql = sSql + "calle_interseccion = '" + txtSecundaria.Text.Trim() + "'," + Environment.NewLine;
                            sSql = sSql + "numero_vivienda = '" + txtNumeracion.Text.Trim() + "'," + Environment.NewLine;
                            sSql = sSql + "referencia = '" + txtReferencia.Text.Trim() + "'" + Environment.NewLine;
                            sSql = sSql + "where correlativo = " + iIdDireccion;

                            //EJECUTAMOS LA INSTRUCCIÒN SQL 
                            if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                            {
                                catchMensaje.LblMensaje.Text = sSql;
                                catchMensaje.ShowDialog();
                                goto reversa;
                            }
                        }
                    }

                    else
                    {
                        goto reversa;
                    }
               

                    //BUSCAMOS EL ID DE TELEFONOS   
                    //==================================================================================================================

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    sSql = "";
                    sSql = sSql + "select top 1 correlativo" + Environment.NewLine;
                    sSql = sSql + "from tp_telefonos" + Environment.NewLine;
                    sSql = sSql + "where id_persona = " + iIdPersona + Environment.NewLine;
                    sSql = sSql + "and estado = 'A'";

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count != 0)
                        {
                            iIdTelefono = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());


                            //ACTUALIZAMOS LA TABLA DE TELEFONOS
                            //=================================================================================================================
                            sSql = "";
                            sSql = sSql + "update tp_telefonos set" + Environment.NewLine;
                            sSql = sSql + "domicilio = '" + txtTelefono.Text.Trim() + "'" + Environment.NewLine;
                            sSql = sSql + "where correlativo = " + iIdTelefono;

                            //EJECUTAMOS LA INSTRUCCIÒN SQL 
                            if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                            {
                                catchMensaje.LblMensaje.Text = sSql;
                                catchMensaje.ShowDialog();
                                goto reversa;
                            }
                        }
                    }

                    else
                    {
                        goto reversa;
                    }

                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                    Program.sIDPERSONA = iIdPersona.ToString();
                    ok.LblMensaje.Text = "Su registro se ha actualizado con éxito";
                    ok.ShowDialog();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    goto fin;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
            }
        fin:
            { }
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                //SE INICIA LA TRANSACCIÓN
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al realizar la transacción. Por favor comuníquese con el administrador en caso de continuar con el inconveniente.";
                    ok.ShowDialog();
                    //Limpiar();
                    goto reversa;
                }

                else
                {
                    sSql = "";
                    sSql = sSql + "Insert Into tp_personas (" + Environment.NewLine;
                    sSql = sSql + "idempresa, Cg_Tipo_Persona, Cg_Pais_Residencia," + Environment.NewLine;
                    sSql = sSql + "Cg_Tipo_Identificacion, Identificacion, Nombres, Apellidos," + Environment.NewLine;
                    sSql = sSql + "Cliente, codigo_alterno, correo_electronico, estado, fecha_ingreso, Usuario_Ingreso," + Environment.NewLine;
                    sSql = sSql + "terminal_ingreso,contador, porcentaje_descuento," + Environment.NewLine;
                    sSql = sSql + "hacerlaretencionfuenteir,numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                    sSql = sSql + "Values (" + Environment.NewLine;
                    sSql = sSql + Program.iIdEmpresa + ", " + iIdTipoPersona + ", 2843, " + iIdTipoIdentificacion + ", '" + txtIdentificacion.Text.Trim() + "'," + Environment.NewLine;
                    sSql = sSql + "'" + txtNombres.Text.Trim() + "', '" + txtApellidos.Text.Trim() + "', 1," + Environment.NewLine;
                    sSql = sSql + "'" + txtTelefono.Text.Trim() + "', " + txtMail.Text.Trim() + ", 'A', GETDATE()," + Environment.NewLine;
                    sSql = sSql + "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "',0,0,0,0,0 )";

                    //EJECUTAMOS LA INSTRUCCIÒN SQL 
                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }


                    //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA TP_PERSONAS
                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    sTabla = "tp_personas";
                    sCampo = "id_persona";

                    iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                    if (iMaximo == -1)
                    {
                        ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                        ok.ShowDialog();
                        goto reversa;
                    }

                    else
                    {
                        iIdPersona = Convert.ToInt32(iMaximo);
                    }

                    //PARA INSERTAR LA DIRECCION
                    sSql = "";
                    sSql = sSql + "Insert Into  tp_direcciones (" + Environment.NewLine;
                    sSql = sSql + "id_persona, IdTipoEstablecimiento, Direccion, calle_principal," + Environment.NewLine;
                    sSql = sSql + "numero_vivienda, calle_interseccion, referencia," + Environment.NewLine;
                    sSql = sSql + "Cg_Localidad, Estado, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                    sSql = sSql + "fecha_ingreso, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                    sSql = sSql + "Values (" + Environment.NewLine;
                    sSql = sSql + iIdPersona + ", 1, '" + txtSector.Text.Trim() + "', '" + txtPrincipal.Text.Trim() + "'," + Environment.NewLine;
                    sSql = sSql + "'" + txtNumeracion.Text.Trim() + "','" + txtSecundaria.Text.Trim() + "'," + Environment.NewLine;
                    sSql = sSql + "'" + txtReferencia.Text.Trim() + "', " + Program.iCgLocalidad + "," + Environment.NewLine;
                    sSql = sSql + "'A','" + Program.sDatosMaximo[0] + "','" + Program.sDatosMaximo[1] + "'," + Environment.NewLine;
                    sSql = sSql + "GetDate(), 0, 0)";

                    //EJECUTAMOS LA INSTRUCCIÒN SQL 
                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    //PARA INSERTAR EL TELEFONO
                    sSql = "";
                    sSql = sSql + "Insert Into tp_telefonos (" + Environment.NewLine;
                    sSql = sSql + "id_persona, idTipoEstablecimiento, CODIGO_AREA," + Environment.NewLine;
                    sSql = sSql + "domicilio, Estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                    sSql = sSql + "terminal_ingreso, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                    sSql = sSql + "Values (" + Environment.NewLine;
                    sSql = sSql + iIdPersona + ", 1, '02','" + txtTelefono.Text.Trim() + "','A', GetDate()," + Environment.NewLine;
                    sSql = sSql + "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 0, 0)";

                    //EJECUTAMOS LA INSTRUCCIÒN SQL 
                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                    ok.LblMensaje.Text = "El registro se ha guardado con éxito.";
                    ok.ShowInTaskbar = false;
                    Program.sIDPERSONA = iIdPersona.ToString(); ;
                    ok.ShowDialog();

                    if (ok.DialogResult == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    goto fin;
                }
            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
            }
        fin: { }

        }

        #endregion

        private void Direccion_Load(object sender, EventArgs e)
        {
            Direccion cod = Owner as Direccion;
            Domicilio dm = Owner as Domicilio;
            txtNombres.Focus();
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
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
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Direccion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtIdentificacion.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese el número de identificación";
                ok.ShowDialog();

                txtIdentificacion.Focus();
                goto fin;
            }

            else
            {
                if (chkPasaporte.Checked == false)
                {
                    validarIdentificacion(0);

                    if (iBandera == 0)
                    {
                        goto fin;
                    }
                }

                else
                {
                    iIdTipoPersona = 2447;
                    iIdTipoIdentificacion = 180;
                }
            }

            if (txtApellidos.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese el apellido para el registro";
                ok.ShowDialog();
                txtApellidos.Focus();
            }
                
            else if (txtTelefono.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese el número telefónico para el registro";
                ok.ShowDialog();
                txtTelefono.Focus();
                //goto fin;
            }

            else if (txtMail.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese el correo electrónico para el registro";
                ok.ShowDialog();
                txtMail.Focus();
                //goto fin;
            }

            else if (txtSector.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese el sector para el registro del domicilio.";
                ok.ShowDialog();

                txtSector.Focus();
                //goto fin;
            }

            else if (txtPrincipal.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese la calle principal para el registro del domicilio.";
                ok.ShowDialog();

                txtPrincipal.Focus();
                //goto fin;
            }

            else if (txtNumeracion.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese la numeración para el registro del domicilio.";
                ok.ShowDialog();

                txtNumeracion.Focus();
                //goto fin;
            }

            else if (txtSecundaria.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese la calle secundaria para el registro del domicilio.";
                ok.ShowDialog();

                txtSecundaria.Focus();
                //goto fin;
            }

            else if (txtReferencia.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese la referencia para el registro del domicilio.";
                ok.ShowDialog();

                txtReferencia.Focus();
                //goto fin;
            }

            else
            {
                //INSERTAR NUEVO REGISTRO
                if (iIdPersona == 0)
                {
                    insertarRegistro();
                }

                //ACTUALIZAR EL REGISTRO
                else
                {
                    actualizarRegistro();
                }
            }

        fin:
            { }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtIdentificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtIdentificacion.Text != "")
                {
                    //AQUI INSTRUCCIONES PARA CONSULTAR Y VALIDAR LA CEDULA
                    if ((esNumero(txtIdentificacion.Text.Trim()) == true) && (chkPasaporte.Checked == false))
                    {
                        //INSTRUCCIONES PARA VALIDAR
                        validarIdentificacion(1);
                    }
                    else
                    {
                        //CONSULTAR EN LA BASE DE DATOS
                        iIdTipoPersona = 2447;
                        iIdTipoIdentificacion = 180;
                        consultarRegistro();
                    }
                }
            }
        }
    }
}
