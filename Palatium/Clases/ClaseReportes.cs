using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palatium.Clases
{
    class ClaseReportes
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        string sSql;
        string sOrigenOrden;
        string sFecha;
        string sNombreProducto;
        string sTotal;
        string sTexto;

        DataTable dtConsulta;
        DataTable dtAyuda;

        bool bRespuesta;

        int iIdOrigenOrden;
        int iIdJornada;
        int iIdLocalidad;
        int iCuenta;        

        Decimal dbTotal;
        Decimal dbCantidadProductos;

        //FUNCION PARA CREAR EL REPORTE DETALLADO POR ORIGEN
        public string detalleVentasOrigen(string sFecha_P, int iIdLocalidad_P, int iIdJornada_P)
        {
            try
            {
                this.sFecha = sFecha_P;
                this.iIdLocalidad = iIdLocalidad_P;
                this.iIdJornada = iIdJornada_P;

                sTexto = "";

                sSql = "";
                sSql += "select id_pos_origen_orden, codigo, descripcion" + Environment.NewLine;
                sSql += "from pos_origen_orden" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "order by id_pos_origen_orden";

                dtAyuda = new DataTable();
                dtAyuda.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtAyuda, sSql);

                if (bRespuesta == false)
                {
                    return "ERROR";
                }

                if (dtAyuda.Rows.Count == 0)
                {
                    return "SIN INFORMACIÓN";
                }

                sTexto += "".PadLeft(40, '=') + Environment.NewLine;

                for (int i = 0; i < dtAyuda.Rows.Count; i++)
                {
                    iIdOrigenOrden = Convert.ToInt32(dtAyuda.Rows[i]["id_pos_origen_orden"].ToString());
                    sOrigenOrden = dtAyuda.Rows[i]["descripcion"].ToString().Trim().ToUpper();

                    sSql = "";
                    sSql += "select count(*) cuenta" + Environment.NewLine;
                    sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                    sSql += "where id_pos_origen_orden = " + iIdOrigenOrden + Environment.NewLine;
                    sSql += "and estado = 'A'" + Environment.NewLine;
                    sSql += "and estado_orden in ('Pagada', 'Cerrada')" + Environment.NewLine;
                    sSql += "and fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                    sSql += "and id_pos_jornada = " + iIdJornada + Environment.NewLine;
                    sSql += "and id_localidad = " + iIdLocalidad;

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == false)
                    {
                        return "ERROR";
                    }

                    iCuenta = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                    if (iCuenta > 0)
                    {
                        sTexto += "ORIGEN: " + sOrigenOrden + Environment.NewLine;
                        sTexto += "".PadLeft(40, '-') + Environment.NewLine;

                        sSql = "";
                        sSql += "select * from pos_vw_detallar_productos_origen_orden" + Environment.NewLine;
                        sSql += "where id_pos_origen_orden = " + iIdOrigenOrden + Environment.NewLine;
                        sSql += "and fecha_pedido = '" + sFecha + "'" + Environment.NewLine;
                        sSql += "and id_pos_jornada = " + iIdJornada + Environment.NewLine;
                        sSql += "and id_localidad = " + iIdLocalidad + Environment.NewLine;
                        sSql += "order by cantidad";

                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                        if (bRespuesta == false)
                        {
                            return "ERROR";
                        }

                        dbTotal = 0;

                        for (int j = 0; j < dtConsulta.Rows.Count; j++)
                        {
                            sNombreProducto = dtConsulta.Rows[j]["nombre"].ToString().Trim().ToUpper();
                            dbCantidadProductos = Convert.ToDecimal(dtConsulta.Rows[j]["cantidad"].ToString().Trim().ToUpper());
                            sTotal = dtConsulta.Rows[j]["total"].ToString().Trim();
                            dbTotal += Convert.ToDecimal(dtConsulta.Rows[j]["total"].ToString().Trim());

                            if (sNombreProducto.Length > 25)
                            {
                                sNombreProducto = sNombreProducto.Substring(0, 25);
                            }

                            sTexto += sNombreProducto.PadRight(25, ' ') + dbCantidadProductos.ToString("N0").PadLeft(5, ' ') + sTotal.PadLeft(10, ' ') + Environment.NewLine;
                        }

                        sTexto += "".PadLeft(40, '-') + Environment.NewLine;
                        sTexto += "TOTAL REPORTADO:".PadRight(30, ' ') + dbTotal.ToString("N2").PadLeft(10, ' ') + Environment.NewLine;
                        sTexto += "".PadLeft(40, '=') + Environment.NewLine;
                    }
                }
                
                return sTexto;
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }
    }
}
