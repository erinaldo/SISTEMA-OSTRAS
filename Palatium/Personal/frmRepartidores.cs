using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Personal
{
    public partial class frmRepartidores : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

        Clases.ClaseValidarCaracteres caracteres = new Clases.ClaseValidarCaracteres();

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdRepartidor;
        int iIdPersona;

        string sSql;

        public frmRepartidores()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //LIPIAR LAS CAJAS DE TEXTO
        private void limpiarTodo()
        {
            dbAyudaEmpleados.txtDatos.Text = "";
            dbAyudaEmpleados.txtIdentificacion.Text = "";
            dbAyudaEmpleados.iId = 0;
            txtBuscar.Clear();
            txtCodigo.Clear();
            txtDescripcion.Clear();            
            txtClave.Clear();
            chkMostrar.Checked = false;
            cmbEstado.SelectedIndex = 0;
            iIdRepartidor = 0;
            llenarGrid(0);
        }

        //Función para comprobar si hay un código repetido
        private bool comprobarCodigo()
        {
            int iBandera = 0;
            for (int i = 0; i < dgvDatos.Rows.Count; i++)
            {
                if (txtCodigo.Text == dgvDatos.Rows[i].Cells[2].Value.ToString())
                {
                    iBandera = 1;
                    break;
                }
            }

            if (iBandera == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }        

        //CONSULTAR REGISTRO DBAYUDA
        private void consultarRegistroLlenar()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select id_persona, identificacion, apellidos + ' ' + nombres" + Environment.NewLine;
                sSql = sSql + "from tp_personas" + Environment.NewLine;
                sSql = sSql + "where id_persona = " + iIdPersona + Environment.NewLine;
                sSql = sSql + "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    dbAyudaEmpleados.iId = Convert.ToInt32(iIdPersona);
                    dbAyudaEmpleados.txtIdentificacion.Text = dtConsulta.Rows[0].ItemArray[1].ToString().Trim();
                    dbAyudaEmpleados.txtDatos.Text = dtConsulta.Rows[0].ItemArray[2].ToString().Trim();
                }

                else
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al cargar datos del dbAyuda.";
                    ok.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ok.LblMensaje.Text = ex.ToString();
                ok.ShowDialog();
            }
        }

        //Función para llenar las sentencias del dbAyuda
        private void llenarSentencia()
        {
            try
            {
                sSql = "";
                sSql += "select id_persona, identificacion, apellidos + ' ' + isnull(nombres, '') nombres " + Environment.NewLine;
                sSql += "from tp_personas" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;

                if (Program.iManejaNomina == 1)
                {
                    sSql += "and estaenroldepagos = 1" + Environment.NewLine;
                }

                dbAyudaEmpleados.Ver(sSql, "identificacion", 0, 1, 2);
            }

            catch (Exception ex)
            {
                ok.LblMensaje.Text = ex.ToString();
                ok.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int iBandera)
        {
            try
            {
                dgvDatos.Rows.Clear();

                sSql = "";
                sSql = sSql + "select id_pos_repartidor,isnull(PER.id_persona,0), REP.codigo as Código," + Environment.NewLine;
                sSql = sSql + "REP.descripcion as Descripcion, isnull(REP.claveacceso, 0) as Clave_Acceso," + Environment.NewLine;
                sSql = sSql + "REP.estado as Estado, isnull(PER.identificacion,' '), (PER.apellidos + ' ' + PER.nombres) 'Nombre del Cajero'" + Environment.NewLine;
                sSql = sSql + "from tp_personas PER right outer join pos_repartidor REP on REP.id_persona =PER.id_persona" + Environment.NewLine;
                sSql = sSql + "where REP.estado ='A'" + Environment.NewLine;


                if (iBandera != 0)
                {
                    sSql = sSql + "and REP.descripcion like '%" + txtBuscar.Text.Trim() + "%'";
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {

                    if (dtConsulta.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            int iIdRepartidor_P = Convert.ToInt32(dtConsulta.Rows[i].ItemArray[0].ToString());
                            int iIdPersona_P = Convert.ToInt32(dtConsulta.Rows[i].ItemArray[1].ToString());
                            string sCodigo_P = dtConsulta.Rows[i].ItemArray[2].ToString();
                            string sDescripcion_P = dtConsulta.Rows[i].ItemArray[3].ToString();
                            int iClaveAcceso_P = Convert.ToInt32(dtConsulta.Rows[i].ItemArray[4].ToString());
                            string sEstado_P = dtConsulta.Rows[i].ItemArray[5].ToString();
                            string sIdentificacion_P = dtConsulta.Rows[i].ItemArray[6].ToString();
                            string sNombre_P = dtConsulta.Rows[i].ItemArray[7].ToString().Trim();

                            dgvDatos.Rows.Add(iIdRepartidor_P, iIdPersona_P, sCodigo_P, sDescripcion_P, iClaveAcceso_P, sEstado_P, sIdentificacion_P, sNombre_P);
                        }
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

        //FUNCION PARA INSERTAR EL REGISTRO
        private void insertarRegistro()
        {
            try
            {
                //INICIA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "insert into pos_repartidor (" + Environment.NewLine;
                sSql = sSql + "id_persona, codigo, descripcion, claveacceso, estado," + Environment.NewLine;
                sSql = sSql + "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql = sSql + "values(" + Environment.NewLine;
                sSql = sSql + dbAyudaEmpleados.iId + ", '" + txtCodigo.Text.ToString().Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'" + txtDescripcion.Text.ToString().Trim() + "', '" + txtClave.Text.ToString().Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'A', GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                //EJECUTA LA INSTRUCCION SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro ingresado éxitosamente";
                ok.ShowDialog();
                btnNuevo.Text = "Nuevo";
                grupoDatos.Enabled = false;
                limpiarTodo();
                goto fin;

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


        //FUNCION PARA ACTUALIZAR EL REGISTRO
        private void actualizarRegistro()
        {
            try
            {
                //INICIA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "update pos_repartidor set" + Environment.NewLine;
                sSql = sSql + "id_persona = " + dbAyudaEmpleados.iId + "," + Environment.NewLine;
                sSql = sSql + "codigo = '" + txtCodigo.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "descripcion = '" + txtDescripcion.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "claveacceso = '" + txtClave.Text + "'" + Environment.NewLine;
                sSql = sSql + "where id_pos_repartidor = " + iIdRepartidor;

                //EJECUTA LA INSTRUCCION SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro actualizado éxitosamente.";
                ok.ShowDialog();
                btnNuevo.Text = "Nuevo";
                grupoDatos.Enabled = false;
                limpiarTodo();
                goto fin;

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

        //FUNCION PARA ELIMINAR EL REGISTRO
        private void eliminarRegistro()
        {
            try
            {
                //INICIA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "update pos_repartidor set" + Environment.NewLine;
                sSql = sSql + "estado = 'E'," + Environment.NewLine;
                sSql = sSql + "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql = sSql + "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql = sSql + "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql = sSql + "where id_pos_repartidor = " + iIdRepartidor;

                //EJECUTA LA INSTRUCCION SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro eliminado éxitosamente.";
                ok.ShowDialog();
                btnNuevo.Text = "Nuevo";
                grupoDatos.Enabled = false;
                limpiarTodo();
                goto fin;

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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            grupoDatos.Enabled = false;
            btnNuevo.Text = "Nuevo";
            limpiarTodo();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text == "")
            {
                limpiarTodo();
                llenarGrid(0);
            }

            else
            {
                limpiarTodo();
                llenarGrid(1);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            //SI EL BOTON ESTA EN OPCION NUEVO
            if (btnNuevo.Text == "Nuevo")
            {
                limpiarTodo();
                grupoDatos.Enabled = true;
                btnNuevo.Text = "Guardar";
                txtCodigo.Focus();
            }

            else
            {
                if ((txtCodigo.Text == "") && (txtDescripcion.Text == ""))
                {
                    ok.LblMensaje.Text = "Debe rellenar todos los campos obligatorios.";
                    ok.ShowDialog();
                    txtCodigo.Focus();
                }

                else if (txtCodigo.Text == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese el código del cajero.";
                    ok.ShowDialog();
                    txtCodigo.Focus();
                }

                else if (txtDescripcion.Text == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese la descripción del cajero.";
                    ok.ShowDialog();
                    txtDescripcion.Focus();
                }

                //else if (iIdPersona == 0)
                //{
                //    ok.LblMensaje.Text = "Favor seleccione el empleado a asociar.";
                //    ok.ShowDialog();
                //}

                else
                {

                    if (btnNuevo.Text == "Guardar")
                    {
                        if (comprobarCodigo() == true)
                        {
                            insertarRegistro();
                        }

                        else
                        {
                            ok.LblMensaje.Text = "Ya existe un registro con el código ingresado.";
                            ok.ShowDialog();
                            txtCodigo.Clear();
                            txtCodigo.Focus();
                        }
                    }

                    else if (btnNuevo.Text == "Actualizar")
                    {
                        actualizarRegistro();
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (iIdRepartidor == 0)
            {
                ok.LblMensaje.Text = "No ha seleccionado un registro para verificar la eliminación.";
                ok.ShowDialog();
            }

            else
            {
                SiNo.LblMensaje.Text = "Esta seguro que desea dar de baja el registro?";
                SiNo.ShowDialog();

                if (SiNo.DialogResult == DialogResult.OK)
                {
                    eliminarRegistro();
                }
            }
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBuscar_Click(sender, e);
            }
            else
            {
                caracteres.soloNumeros(e);
            }
        }

        private void chkMostrar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrar.Checked == true)
            {
                txtClave.PasswordChar = '\0';
                txtClave.Focus();
            }
            else
            {
                txtClave.PasswordChar = '*';
                txtClave.Focus();
            }

            txtClave.SelectionStart = txtClave.Text.Trim().Length;
        }

        private void frmRepartidores_Load(object sender, EventArgs e)
        {
            llenarGrid(0);
            llenarSentencia();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            grupoDatos.Enabled = true;
            txtCodigo.Enabled = false;
            btnNuevo.Text = "Actualizar";

            iIdRepartidor = Convert.ToInt32(dgvDatos.CurrentRow.Cells[0].Value.ToString());
            iIdPersona = Convert.ToInt32(dgvDatos.CurrentRow.Cells[1].Value.ToString());
            
            consultarRegistroLlenar();

            txtCodigo.Text = dgvDatos.CurrentRow.Cells[2].Value.ToString();
            txtDescripcion.Text = dgvDatos.CurrentRow.Cells[3].Value.ToString();
            txtClave.Text = dgvDatos.CurrentRow.Cells[4].Value.ToString();

            if (dgvDatos.CurrentRow.Cells[5].Value.ToString() == "A")
            {
                cmbEstado.SelectedIndex = 0;
            }
            else
            {
                cmbEstado.SelectedIndex = 1;
            }

            txtDescripcion.Focus();
        }
    }
}
