using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Oficina
{
    public partial class frmClaveAdministrador : MaterialForm
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        string sSql;
        bool bRespuesta;
        bool bActualizar;
        DataTable dtConsulta;
        int iIdParametro;

        public frmClaveAdministrador()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //CONSULTAR EL REGISTRO 
        private void consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select id_pos_parametro_localidad, clave_acceso_admin" + Environment.NewLine;
                sSql = sSql + "from pos_parametro_localidad" + Environment.NewLine;
                sSql = sSql + "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdParametro = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                        txtClave.Text = dtConsulta.Rows[0].ItemArray[1].ToString();
                        bActualizar = true;
                        txtClave.Focus();
                    }

                    else
                    {
                        bActualizar = false;
                        txtClave.Focus();
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowInTaskbar = false;
                    catchMensaje.ShowDialog();
                    goto fin;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                goto fin;
            }

        fin: { }
        }

        //FUNCION PARA ACTUALIZAR LA CLAVE DE ADMINISTRACION
        private void actualizarClave()
        {
            try
            {
                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                    //limpiar();
                    goto fin;
                }

                //INSTRUCCIÓN SQL PARA ACTUALIZAR
                sSql = "";
                sSql = sSql + "update pos_parametro_localidad set" + Environment.NewLine;
                sSql = sSql + "clave_acceso_admin = '" + txtClave.Text.Trim() + "'" + Environment.NewLine;
                sSql = sSql + "where id_pos_parametro_localidad = " + iIdParametro;

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowInTaskbar = false;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //SI SE EJECUTA TODO REALIZA EL COMMIT
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                Program.sPasswordAdmin = txtClave.Text.Trim();
                ok.LblMensaje.Text = "Clave de administrador modificada éxitosamente.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
                consultarRegistro();
                goto fin;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
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

        private void frmClaveAdministrador_Load(object sender, EventArgs e)
        {
            consultarRegistro();
        }

        private void chkVerClave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVerClave.Checked == true)
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtClave.Text == "")
            {
                ok.LblMensaje.Text = "Favor ingrese una clave para la administración.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }

            else
            {
                if (bActualizar == true)
                {
                    //ENVIAR A ACTUALIZAR
                    actualizarClave();
                }

                else
                {
                    //ENVIAR A INSERTAR
                }
            }
        }
    }
}
