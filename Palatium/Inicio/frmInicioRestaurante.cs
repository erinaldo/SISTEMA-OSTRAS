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
    public partial class frmInicioRestaurante : Form
    {
        public frmInicioRestaurante()
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

        private void btnMesas_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnMesas);
        }

        private void btnMesas_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnMesas);
        }

        private void btnLlevar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnLlevar);
        }

        private void btnLlevar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnLlevar);
        }

        private void btnDomicilios_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnDomicilios);
        }

        private void btnDomicilios_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnDomicilios);
        }

        private void btnCanjes_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCanjes);
        }

        private void btnCanjes_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCanjes);
        }

        private void btnCortesias_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCortesias);
        }

        private void btnCortesias_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCortesias);
        }

        private void btnFuncionarios_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnFuncionarios);
        }

        private void btnFuncionarios_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnFuncionarios);
        }

        private void btnConsumoEmpleados_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnConsumoEmpleados);
        }

        private void btnConsumoEmpleados_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnConsumoEmpleados);
        }

        private void btnDatosClientes_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnDatosClientes);
        }

        private void btnDatosClientes_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnDatosClientes);
        }
    }
}
