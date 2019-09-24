using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Inicio
{
    public partial class frmMenuTab : Form, IForm
    {
        VentanasMensajes.frmMensajeNuevoSiNo SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();

        Clases.ClaseEtiquetaUsuario etiqueta = new Clases.ClaseEtiquetaUsuario();
        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();

        private Form activeForm = null;

        int iVerFormulario = 0;
        public frmMenuTab()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA ABRIR EL FORMULARIO HIJO
        private void abrirFormularioHijo(Form frmHijo, int iOp)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = frmHijo;
            frmHijo.TopLevel = false;
            frmHijo.FormBorderStyle = FormBorderStyle.None;
            frmHijo.Dock = DockStyle.Fill;

            if (iOp == 1)
            {
                frmHijo.MdiParent = this;
            }

            pnlContenedor.Controls.Add(frmHijo);
            pnlContenedor.Tag = frmHijo;
            frmHijo.BringToFront();
            
            if (iOp == 1)
            {
                frmHijo.Show(this);
            }

            else
            {
                frmHijo.Show();
            }
        }

        //FUNCION PARA MOSTRAR LOS BOTONES Y OCULTAR
        public void cambioEstado(bool ok)
        {
            btnRestaurante.Visible = ok;
            btnComedor.Visible = ok;
            btnSincronizarSRI.Visible = ok;
            btnUtilitarios.Visible = ok;
            btnRestaurante.Visible = ok;
            btnReportes.Visible = ok;
            btnCerrarSesion.Visible = ok;
            //btnInicio.Visible = !ok;
        }

        #endregion

        private void btnRestaurante_Click(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmInicioRestaurante(), 0);
            btnRestaurante.BackColor = Color.FromArgb(0, 192, 0);
            btnComedor.BackColor = Color.Blue;
            btnUtilitarios.BackColor = Color.Blue;
            btnSincronizarSRI.BackColor = Color.Blue;
            btnReportes.BackColor = Color.Blue;
        }

        private void btnUtilitarios_Click(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmUtilitarios(), 0);
            btnRestaurante.BackColor = Color.Blue;
            btnComedor.BackColor = Color.Blue;
            btnUtilitarios.BackColor = Color.FromArgb(0, 192, 0);
            btnSincronizarSRI.BackColor = Color.Blue;
            btnReportes.BackColor = Color.Blue;
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            if (iVerFormulario == 1)
            {
                abrirFormularioHijo(new Inicio.frmInicioPrograma(), 0);
                btnRestaurante.BackColor = Color.Blue;
                btnComedor.BackColor = Color.Blue;
                btnUtilitarios.BackColor = Color.Blue;
                btnSincronizarSRI.BackColor = Color.Blue;
                btnReportes.BackColor = Color.Blue;
            }

            else
            {
                Inicio.frmIniciarSesion sesion = new frmIniciarSesion();
                sesion.ShowDialog();

                if (sesion.DialogResult == DialogResult.OK)
                {
                    sesion.Close();
                    etiqueta.crearEtiquetaUsuario();
                    lblEtiqueta.Text = Program.sEtiqueta;
                    iVerFormulario = 1;
                    cambioEstado(true);
                }
            }
        }

        private void frmMenuTab_Load(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmInicioPrograma(), 0);
            //Inicio.frmInicioPrograma ini = new Inicio.frmInicioPrograma();
            //pnlContenedor.Controls.Add(ini);
            //ini.Show(this);
        }

        private void btnComedor_Click(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmInicioComedores(), 0);
            btnRestaurante.BackColor = Color.Blue;
            btnComedor.BackColor = Color.FromArgb(0, 192, 0);
            btnUtilitarios.BackColor = Color.Blue;
            btnSincronizarSRI.BackColor = Color.Blue;
            btnReportes.BackColor = Color.Blue;
        }

        private void btnSincronizarSRI_Click(object sender, EventArgs e)
        {
            btnRestaurante.BackColor = Color.Blue;
            btnComedor.BackColor = Color.Blue;
            btnUtilitarios.BackColor = Color.Blue;
            btnSincronizarSRI.BackColor = Color.FromArgb(0, 192, 0);
            btnReportes.BackColor = Color.Blue;
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            btnRestaurante.BackColor = Color.Blue;
            btnComedor.BackColor = Color.Blue;
            btnUtilitarios.BackColor = Color.Blue;
            btnSincronizarSRI.BackColor = Color.Blue;
            btnReportes.BackColor = Color.FromArgb(0, 192, 0);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            SiNo.lblMensaje.Text = "¿Está seguro que desea cerrar la aplicación?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
            {
                Application.Exit();
            }  
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            SiNo.lblMensaje.Text = "¿Está seguro que desea cerrar su sesión?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
            {
                iVerFormulario = 0;
                abrirFormularioHijo(new Inicio.frmInicioPrograma(), 0);
                btnRestaurante.BackColor = Color.Blue;
                btnComedor.BackColor = Color.Blue;
                btnUtilitarios.BackColor = Color.Blue;
                btnSincronizarSRI.BackColor = Color.Blue;
                btnReportes.BackColor = Color.Blue;
                lblEtiqueta.Text = "DESCONECTADO";
                cambioEstado(false);
            }
        }

        private void frmMenuTab_KeyDown(object sender, KeyEventArgs e)
        {
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
    }
}
