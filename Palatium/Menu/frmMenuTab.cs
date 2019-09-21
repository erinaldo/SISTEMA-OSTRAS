using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Menu
{
    public partial class frmMenuTab : Form
    {
        private Form activeForm = null;

        public frmMenuTab()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA ABRIR EL FORMULARIO HIJO
        private void abrirFormularioHijo(Form frmHijo)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = frmHijo;
            frmHijo.TopLevel = false;
            frmHijo.FormBorderStyle = FormBorderStyle.None;
            frmHijo.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(frmHijo);
            pnlContenedor.Tag = frmHijo;
            frmHijo.BringToFront();
            frmHijo.Show();
        }

        #endregion

        private void btnRestaurante_Click(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmInicioRestaurante());
            btnRestaurante.BackColor = Color.FromArgb(0, 192, 0);
            btnComedor.BackColor = Color.Blue;
            btnUtilitarios.BackColor = Color.Blue;
            btnSincronizarSRI.BackColor = Color.Blue;
            btnReportes.BackColor = Color.Blue;
        }

        private void btnUtilitarios_Click(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmUtilitarios());
            btnRestaurante.BackColor = Color.Blue;
            btnComedor.BackColor = Color.Blue;
            btnUtilitarios.BackColor = Color.FromArgb(0, 192, 0);
            btnSincronizarSRI.BackColor = Color.Blue;
            btnReportes.BackColor = Color.Blue;
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmInicioPrograma());
            btnRestaurante.BackColor = Color.Blue;
            btnComedor.BackColor = Color.Blue;
            btnUtilitarios.BackColor = Color.Blue;
            btnSincronizarSRI.BackColor = Color.Blue;
            btnReportes.BackColor = Color.Blue;
        }

        private void frmMenuTab_Load(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmInicioPrograma());
        }

        private void btnComedor_Click(object sender, EventArgs e)
        {
            abrirFormularioHijo(new Inicio.frmInicioComedores());
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
            //this.Close();
            Application.Exit();
        }
    }
}
