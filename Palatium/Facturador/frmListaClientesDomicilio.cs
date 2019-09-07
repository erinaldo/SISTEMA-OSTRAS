using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Facturador
{
    public partial class frmListaClientesDomicilio : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        DataTable dtConsulta = new DataTable();
        string sSql;
        bool bRespuesta;

        public frmListaClientesDomicilio(DataTable dtDatos)
        {
            this.dtConsulta = dtDatos;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            dgvDatos.DataSource = dtConsulta;

            dgvDatos.Columns[1].Width = 100;
            dgvDatos.Columns[2].Width = 180;
            dgvDatos.Columns[3].Width = 180;
            dgvDatos.Columns[4].Width = 180;
            dgvDatos.Columns[5].Width = 240;
            dgvDatos.Columns[6].Width = 100;

            dgvDatos.Rows[0].Selected = true;
            dgvDatos.CurrentCell = dgvDatos.Rows[0].Cells[1];
        }

        //FUNCION PARA RECUPERAR DATOS
        private void recuperarDatos()
        {
            try
            {
                dgvDatos.Columns[0].Visible = true;
                sSql = "";
                sSql = sSql + "select identificacion, nombres, apellidos," + Environment.NewLine;
                sSql = sSql + "correo_electronico, codigo_alterno, id_persona" + Environment.NewLine;
                sSql = sSql + "from tp_personas" + Environment.NewLine;
                sSql = sSql + "where id_persona = " + Convert.ToInt32(dgvDatos.CurrentRow.Cells[0].Value.ToString()) + Environment.NewLine;
                sSql = sSql + "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        Program.sIDPERSONA = dgvDatos.CurrentRow.Cells[0].Value.ToString();
                        Domicilio frmDomicilio = new Domicilio();
                        frmDomicilio.dtConsulta = dtConsulta;
                        frmDomicilio.txtNumeroTelefono.Text = dgvDatos.CurrentRow.Cells[6].Value.ToString();
                        frmDomicilio.ShowDialog();
                        dgvDatos.Columns[0].Visible = false;
                        this.DialogResult = DialogResult.OK;                        
                        goto fin;
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto fin;
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

        #endregion

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListaClientesDomicilio_Load(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            recuperarDatos();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            recuperarDatos();
        }

        private void frmListaClientesDomicilio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
