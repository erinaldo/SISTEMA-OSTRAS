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
    public partial class frmVentasPorMesero : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();

        DataTable dtConsulta;
        string sSql;
        bool bRespuesta = false;

        int iOp;
        string sFecha;
        Double dSuma;

        public frmVentasPorMesero(string sFecha)
        {   
            this.sFecha = sFecha;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        private void llenarGrid()
        {
            try
            {
                dSuma = 0;

                sSql = "";
                sSql = sSql + "select mesero MESERO, " + Environment.NewLine;
                sSql = sSql + "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva + valor_otro)), 10,2)) TOTAL" + Environment.NewLine;
                sSql = sSql + "from pos_vw_factura" + Environment.NewLine;
                sSql = sSql + "where fecha_factura = '" + sFecha + "'" + Environment.NewLine;
                sSql = sSql + "and id_pos_jornada = " + Program.iJornadaRecuperada + Environment.NewLine;
                sSql = sSql + "group by mesero" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        dgvDatos.DataSource = dtConsulta;

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            dSuma = dSuma + Convert.ToDouble(dtConsulta.Rows[i][1].ToString());
                        }

                        dgvDatos.Columns[0].Width = 250;
                        dgvDatos.Columns[1].Width = 100;
                        dgvDatos.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        txtTotal.Text = dSuma.ToString("N2");
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

        #endregion

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ok.LblMensaje.Text = "No hay registrios para imprimir.";
                ok.ShowDialog();
            }

            else
            {
                ReportesTextbox.frmVentasMesero mesero = new ReportesTextbox.frmVentasMesero(sFecha, 1);
                mesero.ShowDialog();
            }
        }

        private void frmVentasPorMesero_Load(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void frmVentasPorMesero_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }        
    }
}
