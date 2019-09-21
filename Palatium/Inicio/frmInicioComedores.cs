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
    public partial class frmInicioComedores : Form
    {
        public frmInicioComedores()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //INGRESAR EL CURSOR AL BOTON
        private void ingresaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.Black;
            btnProceso.BackColor = Color.LawnGreen;
        }

        //SALIR EL CURSOR DEL BOTON
        private void salidaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.White;
            btnProceso.BackColor = Color.Navy;
        }

        #endregion

        private void btnClienteEmpresarial_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnClienteEmpresarial);
        }

        private void btnClienteEmpresarial_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnClienteEmpresarial);
        }

        private void btnVentaExpress_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnVentaExpress);
        }

        private void btnVentaExpress_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnVentaExpress);
        }

        private void btnTarjetaAlmuerzo_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnTarjetaAlmuerzo);
        }

        private void btnTarjetaAlmuerzo_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnTarjetaAlmuerzo);
        }

        private void btnCobroAlmuerzos_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCobroAlmuerzos);
        }

        private void btnCobroAlmuerzos_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCobroAlmuerzos);
        }

        private void btnDatosClientes_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnDatosClientes);
        }

        private void btnDatosClientes_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnDatosClientes);
        }

        private void btnRevisar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnRevisar);
        }

        private void btnRevisar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnRevisar);
        }

        private void btnCancelar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCancelar);
        }

        private void btnCancelar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCancelar);
        }

        private void btnMovimientoCaja_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnMovimientoCaja);
        }

        private void btnMovimientoCaja_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnMovimientoCaja);
        }

        private void btnSalidaCajero_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnSalidaCajero);
        }

        private void btnSalidaCajero_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnSalidaCajero);
        }
    }
}
