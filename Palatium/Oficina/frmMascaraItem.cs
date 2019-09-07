﻿using MaterialSkin.Controls;
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
    public partial class frmMascaraItem : MaterialForm
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

        DataTable dtConsulta;

        string sSql;

        bool bRespuesta;

        int iIdMascaraItem;
        public frmMascaraItem()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL DATAGRIDVIEW
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql = sSql + "select id_pos_mascara_item as 'ID', codigo as CÓDIGO, descripcion as DESCRIPCION," + Environment.NewLine;
                sSql = sSql + "case estado when 'A' then 'ACTIVO' else 'INACTIVO' end as ESTADO" + Environment.NewLine;
                sSql = sSql + "from pos_mascara_item" + Environment.NewLine;
                sSql = sSql + "where estado = 'A'" + Environment.NewLine;

                if (iOp == 1)
                {
                    sSql = sSql + "and (codigo like '%" + txtBuscar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql = sSql + "or descripcion like '%" + txtBuscar.Text.Trim() + "%')";
                }

                sSql = sSql + "order by id_pos_mascara_item";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    dgvDatos.DataSource = dtConsulta;
                    dgvDatos.Columns[1].Width = 60;
                    dgvDatos.Columns[2].Width = 185;
                    dgvDatos.Columns[3].Width = 60;
                    dgvDatos.Columns[0].Visible = false;
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }

                lblRegistros.Text = dgvDatos.Rows.Count.ToString() + " Registros Encontrados";
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            grupoDatos.Enabled = false;
            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtEstado.Text = "ACTIVO";

            llenarGrid(0);
            btnAgregar.Text = "Nuevo";
            btnEliminar.Enabled = false;
            txtCodigo.Enabled = true;
            txtBuscar.Focus();
        }

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    limpiar();
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "insert into pos_mascara_item (" + Environment.NewLine;
                sSql = sSql + "codigo, descripcion, estado, fecha_ingreso," + Environment.NewLine;
                sSql = sSql + "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql = sSql + "values(" + Environment.NewLine;
                sSql = sSql + "'" + txtCodigo.Text.Trim() + "', '" + txtDescripcion.Text.Trim() + "'," + Environment.NewLine;
                sSql = sSql + "'A', GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql = sSql + "'" + Program.sDatosMaximo[1] + "')";

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //SI SE EJECUTA TODO REALIZA EL COMMIT
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro ingresado éxitosamente.";
                ok.ShowDialog();
                limpiar();
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }

            reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
            }

            fin: { }
        }

        //FUNCION PARA ACTUALIZAR EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    limpiar();
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "update pos_mascara_item set" + Environment.NewLine;
                sSql = sSql + "descripcion = '" + txtDescripcion.Text.Trim() + "'" + Environment.NewLine;
                sSql = sSql + "where id_pos_mascara_item = " + iIdMascaraItem + Environment.NewLine;
                sSql = sSql + "and estado = 'A'";


                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //SI SE EJECUTA TODO REALIZA EL COMMIT
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro actualizado éxitosamente.";
                ok.ShowDialog();
                limpiar();
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
            }

        fin: { }
        }

        //FUNCION PARA ELIMINAR EN LA BASE DE DATOS
        private void eliminarRegistro()
        {
            try
            {
                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    limpiar();
                    goto fin;
                }

                sSql = "";
                sSql = sSql + "update pos_mascara_item set" + Environment.NewLine;
                sSql = sSql + "estado = 'E'," + Environment.NewLine;
                sSql = sSql + "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql = sSql + "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql = sSql + "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql = sSql + "where id_pos_mascara_item = " + iIdMascaraItem;


                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //SI SE EJECUTA TODO REALIZA EL COMMIT
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro eliminado éxitosamente.";
                ok.ShowDialog();
                limpiar();
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
            }

        fin: { }
        }

        //FUNCION PARA CONSULTAR SI EL REGISTRO YA SE UTILIZÓ EN UNA TRANSACCION
        private int consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select count(*) cuenta" + Environment.NewLine;
                sSql = sSql + "from cv403_det_pedidos" + Environment.NewLine;
                sSql = sSql + "where id_pos_mascara_item = " + iIdMascaraItem + Environment.NewLine;
                sSql = sSql + "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return -1;
                }
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return -1;
            }
        }

        #endregion

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (btnAgregar.Text == "Nuevo")
            {
                grupoDatos.Enabled = true;
                txtCodigo.Focus();
                btnAgregar.Text = "Guardar";
            }

            else
            {
                if (txtCodigo.Text.Trim() == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese el código del registro.";
                    ok.ShowDialog();
                }

                else if (txtDescripcion.Text.Trim() == "Favor ingrese la descripción del registro.")
                {
                    ok.LblMensaje.Text = "";
                    ok.ShowDialog();
                }

                else
                {
                    if (btnAgregar.Text == "Guardar")
                    {
                        insertarRegistro();
                    }

                    else if (btnAgregar.Text == "Actualizar")
                    {
                        actualizarRegistro();
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void frmMascaraItem_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            iIdMascaraItem = Convert.ToInt32(dgvDatos.CurrentRow.Cells[0].Value);
            txtCodigo.Text = dgvDatos.CurrentRow.Cells[1].Value.ToString();
            txtDescripcion.Text = dgvDatos.CurrentRow.Cells[2].Value.ToString();
            txtEstado.Text = dgvDatos.CurrentRow.Cells[3].Value.ToString();
            txtCodigo.Enabled = false;
            btnAgregar.Text = "Actualizar";
            btnEliminar.Enabled = true;
            grupoDatos.Enabled = true;
            txtDescripcion.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Trim() == "")
            {
                llenarGrid(0);
            }

            else
            {
                llenarGrid(1);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (iIdMascaraItem == 0)
            {
                ok.LblMensaje.Text = "No hay un registro seleccionado para eliminar.";
                ok.ShowDialog();
            }

            else
            {
                if (consultarRegistro() > 0)
                {
                    ok.LblMensaje.Text = "Existen registros relacionados con la selección. No puede eliminar el registro.";
                    ok.ShowDialog();
                }

                else if (consultarRegistro() == -1)
                {
                    ok.LblMensaje.Text = "Existe un error al consultar si el registro se utilizó en otros registros.";
                    ok.ShowDialog();
                }

                else
                {
                    SiNo.LblMensaje.Text = "¿Está seguro que desea eliminar el registro seleccionado?";
                    SiNo.ShowDialog();

                    if (SiNo.DialogResult == DialogResult.OK)
                    {
                        eliminarRegistro();
                    }
                }
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtBuscar.Text.Trim() == "")
                {
                    llenarGrid(0);
                }

                else
                {
                    llenarGrid(1);
                }
            }
        }
    }
}
