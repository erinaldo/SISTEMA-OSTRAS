using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Bodega
{
    public partial class frmBodegaDefinicionCorta : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();

        string sSql;
        DataTable dtConsulta;
        bool bRespuesta;

        public frmBodegaDefinicionCorta()
        {
            InitializeComponent();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBodega.CurrentRow.Cells[0].Value.ToString() != "")
                    MessageBox.Show("No se puede eliminar el registro", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    if (MessageBox.Show("Desea eliminar la línea...?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        dgvBodega.Rows.Remove(dgvBodega.CurrentRow);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No hay Productos para eliminar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            cargarCombos(0, "A");
            dgvBodega.Rows.Add("","","",estado.DisplayMember);
            this.dgvBodega.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvUserDetails_RowPostPaint);
        }

        //Función para cargar el combo del datagridview
        private void cargarCombos(int iBandera, string sEstado)
        {
            estado.Items.Clear();
            estado.Items.Add("Activo");
            estado.Items.Add("Eliminado");
            estado.DisplayMember = "Activo";

            #region recuperar datos del combo
            if (iBandera == 1)
            {
                if (sEstado == "A")
                    estado.DisplayMember = "Activo";
                else
                    estado.DisplayMember = "Eliminado";
            }
            #endregion


        }

        //Función para enumerar las filas del grid
        private void dgvUserDetails_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvBodega.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void frmBodegaDefinicionCorta_Load(object sender, EventArgs e)
        {
            llenarGrid(0);
        }

        //Función para llenar el grid
        private void llenarGrid(int iBandera)
        {
            try
            {
                dgvBodega.Rows.Clear();
                sSql = "";
                sSql += "Select correlativo, codigo, valor_texto, estado  From tp_codigos Where TABLA = 'SYS$00019' ";
                if (iBandera == 0)
                {
                    sSql += " AND ESTADO = 'A' ";
                }
                
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        for(int i=0;i<dtConsulta.Rows.Count;i++)
                        {
                            string sEstado = dtConsulta.Rows[i].ItemArray[3].ToString();
 
                            i = dgvBodega.Rows.Add();

                            cargarCombos(1,sEstado);
                            dgvBodega.Rows[i].Cells[0].Value = dtConsulta.Rows[i].ItemArray[0].ToString();
                            dgvBodega.Rows[i].Cells[1].Value = dtConsulta.Rows[i].ItemArray[1].ToString();
                            dgvBodega.Rows[i].Cells[2].Value = dtConsulta.Rows[i].ItemArray[2].ToString();
                            dgvBodega.Rows[i].Cells[3].Value = estado.DisplayMember;
                           
                        }

                        this.dgvBodega.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvUserDetails_RowPostPaint);
                    }
                }
                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }

            }
            catch (Exception exc)
            {
                catchMensaje.LblMensaje.Text = exc.ToString();
                catchMensaje.ShowDialog();
            }
            
        }

        private void rbtnActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnActivos.Checked == true)
                llenarGrid(0);
            else
                llenarGrid(1);
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (comprobarCodigo() == true)
            {
                if (verificarIngresoCodigo() == true)
                {
                    if (verificarNuevosRegistros() == false)
                    {
                        if (MessageBox.Show("¿Desea Grabar..?", "Contabilidad", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            grabarRegistros();
                            if (rbtnActivos.Checked == false)
                                rbtnActivos.Checked = true;
                        }

                    }

                    else
                        MessageBox.Show("No existen columnas modificadas a grabar", "Contabilidad", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    MessageBox.Show("Por favor, ingrese un código","Contabilidad",MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }
            else
                MessageBox.Show("El código ingresado ya existe","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        //Función para comprobar si ya existe el código ingresado
        private bool comprobarCodigo()
        {
            int iBandera = 0;

            if (dgvBodega.Rows.Count > 0)
            {
                for (int i = 0; i < dgvBodega.Rows.Count; i++)
                {
                    if (dgvBodega.Rows[i].Cells[0].Value == null || dgvBodega.Rows[i].Cells[0].Value.ToString() == "")
                    {
                        sSql = "Select codigo, estado  From tp_codigos Where TABLA = 'SYS$00019'  AND Codigo = '"
                            + dgvBodega.Rows[i].Cells[1].Value + "' ";

                        DataTable dtAyuda = new DataTable();
                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtAyuda, sSql);
                        if (bRespuesta == true)
                            if (dtAyuda.Rows.Count > 0)
                            {
                                iBandera = 1;
                                break;
                            }    
                    }
                }
            }
            

            if (iBandera == 1) return false; else return true;
        }

        //Función para verificar nuevos registros
        private bool verificarNuevosRegistros()
        {
            int iBandera = 0;

            for (int i = 0; i < dgvBodega.Rows.Count; i++)
            {
                
                if (dgvBodega.Rows[i].Cells[0].Value.ToString() == "")
                {
                    iBandera = 1;
                    break;
                }

                if (dgvBodega.Rows[i].Cells[3].Value.ToString() == "Eliminado")
                {
                    iBandera = 1;
                    break;
                }

            }

                if (iBandera == 1) return false; else return true;
        }

        //Función para grabar los registros
        private void grabarRegistros()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                    goto reversa;
                else
                {
                    for (int i = 0; i < dgvBodega.Rows.Count; i++)
                    {
                        if (dgvBodega.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            string sEstado;
                            if (dgvBodega.Rows[i].Cells[3].Value.ToString() == "Activo")
                                sEstado = "A";
                            else
                                sEstado = "E";

                            sSql = "Insert Into tp_codigos(Tabla,codigo,valor_texto,valor_fecha,valor_numero,estado,login,fecha, "+
                                    "numero_replica_trigger, numero_control_replica ) "+
                                    "Values ('SYS$00019','"+dgvBodega.Rows[i].Cells[1].Value.ToString()
                                            +"','"+dgvBodega.Rows[i].Cells[2].Value.ToString()+"', NULL ,0, '"+sEstado+"','', GetDate(),0,0)";

                            if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                                goto reversa;

                        }

                        if (dgvBodega.Rows[i].Cells[0].Value.ToString() != "")
                        {
                            string sEstado;
                            if (dgvBodega.Rows[i].Cells[3].Value.ToString() == "Activo")
                                sEstado = "A";
                            else
                                sEstado = "E";

                            if (modificarRegistro(Convert.ToInt32(dgvBodega.Rows[i].Cells[0].Value.ToString()), sEstado) == true)
                            {
                                sSql = "Update tp_codigos Set Tabla   =  'SYS$00019', CODIGO =  '"+dgvBodega.Rows[i].Cells[1].Value.ToString()+"',  "+
                                        "VALOR_TEXTO ='"+dgvBodega.Rows[i].Cells[2].Value.ToString()+"', VALOR_FECHA =  NULL ,  "+
                                        "VALOR_NUMERO = 0,  ESTADO ='"+sEstado+"', LOGIN  = '' Where CORRELATIVO = "+dgvBodega.Rows[i].Cells[0].Value.ToString()+"";

                                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                                    goto reversa;

                            }

                        }

                    }

                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                    MessageBox.Show("Operación Exitosa", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    goto fin;

                }
            }
            catch (Exception exc)
            { 
            
            }


            #region Funciones de ayuda

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                MessageBox.Show("Ocurrió un problema al grabar los registros", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        fin:
            {
                llenarGrid(0);
            }

            #endregion


        }

        //Función para verificar el cambio
        private bool modificarRegistro(int iCorrelativo, string sEstado)
        {
            int iBandera = 0;
            sSql = "select * from tp_codigos where tabla = 'SYS$00019' and correlativo = "+iCorrelativo+" and estado = '"+sEstado+"' ";
            DataTable dtAyuda = new DataTable();
            dtAyuda.Clear();
            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtAyuda,sSql);
            if (bRespuesta == true)
            {
                if (dtAyuda.Rows.Count > 0)
                    iBandera = 1;
            }

            if (iBandera == 1) return false; else return true;

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Función para verificar que no haya códigos vacios
        private bool verificarIngresoCodigo()
        {
            int iBandera = 0;

            for (int i = 0; i < dgvBodega.Rows.Count; i++)
            {
                if (dgvBodega.Rows[i].Cells[1].Value.ToString() == "")
                {
                    iBandera = 1;
                    break;
                }
            }

                if (iBandera == 1) return false; else return true;

        }


        //Fin de la clase
    }
}
