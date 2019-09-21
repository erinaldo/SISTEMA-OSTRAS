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
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
        }

        private void btnPuntoVenta_MouseEnter(object sender, EventArgs e)
        {
            btnPuntoVenta.BackColor = Color.FromArgb(255, 128, 255);
        }

        private void btnPuntoVenta_MouseLeave(object sender, EventArgs e)
        {
            btnPuntoVenta.BackColor = Color.FromArgb(255, 192, 255);
        }

        private void btnConfiguracion_MouseLeave(object sender, EventArgs e)
        {
            btnConfiguracion.BackColor = Color.FromArgb(192, 255, 192);
        }

        private void btnConfiguracion_MouseEnter(object sender, EventArgs e)
        {
            btnConfiguracion.BackColor = Color.FromArgb(128, 255, 128);
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            //this.ActiveControl = this;
        }

        private void btnPuntoVenta_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //Login.frmLoginPuntoVenta login = new Login.frmLoginPuntoVenta();
            //login.ShowDialog();

            //if (login.DialogResult == DialogResult.OK)
            //{
            //    Palatium.Menu.frmNuevoMenuPos ver = new Menu.frmNuevoMenuPos();
            //    ver.ShowDialog();
            //    this.Close();
            //}

            //else
            //{
            //    this.Show();
            //}
        }
    }
}
