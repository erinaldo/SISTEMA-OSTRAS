using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Comida_Rapida
{
    public partial class frmComandaComidaRapida : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        string sSql;

        DataTable dtConsulta;
        DataTable dtFamilia;
        DataTable dtItems;

        bool bRespuesta;

        //CONTROLES PARA EL ARREGLO DINÁMICO
        Panel[,] pnlFamilias = new Panel[3,2];
        Label[,] lblFamilias = new Label[3,2];
        Button[,] btnProductos = new Button[3, 3];

        //VARIAABLES PARA EL ARREGLO DINAMICO
        int iCoordenadaXPanel;
        int iCoordenadaYPanel;
        int iCoordenadaXBotones;
        int iCoordenadaYBotones;
        int iCoordenadaXEtiqueta;
        int iCoordenadaYEtiqueta;
        int iPosicionX;
        int iPosicionY;
        int iContadorItems;

        int uno;
        int dos;
        int tres;
        //int cuatro;

        public frmComandaComidaRapida()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA ENCERAR LAS VARIABLES
        private void encerarVariables()
        {            
            iCoordenadaXBotones = 0;
            iCoordenadaYBotones = 28;
            iCoordenadaXEtiqueta = 0;
            iCoordenadaYEtiqueta = 0;
        }

        //CREAR RANDOM COLORES
        private void generarColores()
        {
            Random rand = new Random();
            uno = rand.Next(0, 255);
            dos = rand.Next(0, 255);
            tres = rand.Next(0, 255);
            //cuatro = rand.Next(0, 255);
        }

        //CONSULTAR LAS FAMILIAS
        private void cargarFamilias()
        {
            try
            {
                iCoordenadaXPanel = 0;
                iCoordenadaYPanel = 0;
                iPosicionX = 0;
                iPosicionY = 0;

                sSql = "";
                sSql += "select top 6 P.id_producto, P.codigo, NP.nombre" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "where P.nivel = 2" + Environment.NewLine;
                sSql += "and P.menu_pos = 1";

                dtFamilia = new DataTable();
                dtFamilia.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtFamilia, sSql);

                if (bRespuesta == true)
                {
                    for (int i = 0; i < dtFamilia.Rows.Count; i++)
                    {
                        crearControles(Convert.ToInt32(dtFamilia.Rows[i][0].ToString()), dtFamilia.Rows[i][2].ToString().ToUpper());
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR LOS CONTROLES
        private void crearControles(int iIdProducto_P, string sNombreProducto_P)
        {
            try
            {
                sSql = "";
                sSql += "select top 9 P.id_producto, P.codigo, NP.nombre" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and NP.estado = 'A'" + Environment.NewLine;
                sSql += "where P.id_producto_padre = " + iIdProducto_P;

                dtItems = new DataTable();
                dtItems.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtItems, sSql);

                if (bRespuesta == true)
                {
                    encerarVariables();

                    if (dtItems.Rows.Count == 0)
                    {
                        return;
                    }

                    generarColores();
                    pnlFamilias[iPosicionX, iPosicionY] = new Panel();
                    pnlFamilias[iPosicionX, iPosicionY].Location = new Point(iCoordenadaXPanel, iCoordenadaYPanel);
                    pnlFamilias[iPosicionX, iPosicionY].Size = new Size(300, 270);
                    pnlFamilias[iPosicionX, iPosicionY].BackColor = Color.White;

                    lblFamilias[iPosicionX, iPosicionY] = new Label();
                    lblFamilias[iPosicionX, iPosicionY].BackColor = Color.Black;
                    lblFamilias[iPosicionX, iPosicionY].Font = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    lblFamilias[iPosicionX, iPosicionY].ForeColor = Color.White;
                    lblFamilias[iPosicionX, iPosicionY].Location = new Point(iCoordenadaXEtiqueta, iCoordenadaYEtiqueta);
                    lblFamilias[iPosicionX, iPosicionY].Size = new Size(300, 25);
                    lblFamilias[iPosicionX, iPosicionY].Text = sNombreProducto_P;
                    lblFamilias[iPosicionX, iPosicionY].TextAlign = ContentAlignment.MiddleCenter;                    

                    //AGREGAR LABEL AL PANEL
                    pnlFamilias[iPosicionX, iPosicionY].Controls.Add(lblFamilias[iPosicionX, iPosicionY]);

                    iContadorItems = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            btnProductos[i, j] = new Button();
                            btnProductos[i, j].Tag = dtItems.Rows[iContadorItems][0].ToString();
                            btnProductos[i, j].Text = dtItems.Rows[iContadorItems][2].ToString();
                            btnProductos[i, j].Size = new Size(100, 80);
                            btnProductos[i, j].Location = new Point(iCoordenadaXBotones, iCoordenadaYBotones);
                            pnlFamilias[iPosicionX, iPosicionY].Controls.Add(btnProductos[i, j]);
                            generarColores();
                            btnProductos[i, j].BackColor = Color.FromArgb(uno, dos, tres);

                            iContadorItems++;
                            iCoordenadaXBotones += 100;

                            if (iContadorItems == dtItems.Rows.Count)
                            {
                                break;
                            }

                            if (j + 1 == 3)
                            {
                                iCoordenadaXBotones = 0;
                                iCoordenadaYBotones += 80;
                            }
                        }

                        if (iContadorItems == dtItems.Rows.Count)
                        {
                            break;
                        }
                    }

                    pnlComanda.Controls.Add(pnlFamilias[iPosicionX, iPosicionY]);

                    iPosicionX++;
                    iCoordenadaXPanel += 306;
                    iCoordenadaXEtiqueta += 306;                    

                    if (iPosicionX == 3)
                    {
                        iCoordenadaXPanel = 0;
                        iCoordenadaYPanel += 274;

                        iCoordenadaXEtiqueta = 0;
                        iCoordenadaYEtiqueta += 274;

                        iPosicionX = 0;
                        iPosicionY++;                        
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
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

        private void frmComandaComidaRapida_Load(object sender, EventArgs e)
        {
            cargarFamilias();
        }
    }
}
