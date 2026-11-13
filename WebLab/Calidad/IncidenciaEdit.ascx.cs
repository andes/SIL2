using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using Business;
using Business.Data.Laboratorio;
using NHibernate.Expression;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Web.Caching;
namespace WebLab.Calidad
{
    public partial class IncidenciaEdit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CargarIncidencias();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }


        private DataTable ObtenerCatalogoIncidencias()
        {
            const string cacheKey = "IncidenciasCalidad_Tipo2";

            DataTable dt = HttpRuntime.Cache[cacheKey] as DataTable;

            if (dt != null)
                return dt;

            string sql = @"
        SELECT
            idIncidenciaCalidad,
            nombre,
            idIncidenciaSuperior
        FROM LAB_IncidenciaCalidad WITH (NOLOCK)
        WHERE idTipoIncidencia = 2
          AND baja = 0
        ORDER BY nombre";

            dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["SIL_ReadOnly"]
                    .ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dt);
                }
            }

            // La tabla se comparte solo para lectura.
            HttpRuntime.Cache.Insert(
                cacheKey,
                dt,
                null,
                DateTime.Now.AddMinutes(60),
                Cache.NoSlidingExpiration
            );

            return dt;
        }


        private void CargarIncidencias()
        {
            TreeView2.Nodes.Clear(); // Evita acumular nodos
            DataTable dt = ObtenerCatalogoIncidencias();

            TreeView2.Nodes.Clear();

            Dictionary<int, TreeNode> nodos =
                new Dictionary<int, TreeNode>();

            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["idIncidenciaCalidad"]);

                TreeNode nodo = new TreeNode();
                nodo.Text = Convert.ToString(row["nombre"]);
                nodo.Value = id.ToString();
                nodo.ShowCheckBox = true;
                nodo.SelectAction = TreeNodeSelectAction.None;

                nodos.Add(id, nodo);
            }

            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["idIncidenciaCalidad"]);

                int idSuperior =
                    row["idIncidenciaSuperior"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(row["idIncidenciaSuperior"]);

                TreeNode nodo = nodos[id];

                if (idSuperior > 0 &&
                    idSuperior != id &&
                    nodos.ContainsKey(idSuperior))
                {
                    nodos[idSuperior].ChildNodes.Add(nodo);
                }
                else
                {
                    TreeView2.Nodes.Add(nodo);
                }
            }

            foreach (TreeNode nodo in TreeView2.Nodes)
            {
                ConfigurarNodo(nodo);
            }
        }


        private void ConfigurarNodo(TreeNode nodo)
        {
            bool tieneHijos = nodo.ChildNodes.Count > 0;

            // Los nodos superiores son solo agrupadores.
            nodo.ShowCheckBox = !tieneHijos;

            nodo.SelectAction = tieneHijos
                ? TreeNodeSelectAction.Expand
                : TreeNodeSelectAction.None;

            nodo.Expanded = false;

            // Aplicar la misma regla a todos los niveles.
            foreach (TreeNode hijo in nodo.ChildNodes)
            {
                ConfigurarNodo(hijo);
            }
        }


        public HashSet<int> MostrarDatosdelProtocolo(int idProtocolo)
        {
            ISession session = NHibernateHttpModule.CurrentSession;

            ICriteria crit = session.CreateCriteria(
                typeof(ProtocoloIncidenciaCalidad));

            crit.Add(Expression.Eq("IdProtocolo", idProtocolo));

            IList items = crit.List();

            HashSet<int> incidencias = new HashSet<int>();

            foreach (ProtocoloIncidenciaCalidad oDet in items)
            {
                incidencias.Add(oDet.IdIncidenciaCalidad);
            }

            foreach (TreeNode nodo in TreeView2.Nodes)
            {
                MarcarIncidencias(nodo, incidencias);
            }

            return incidencias;
        }


        //public void MostrarDatosdelProtocolo(int p)
        //{
        //    ISession m_session = NHibernateHttpModule.CurrentSession;

        //    ICriteria crit = m_session.CreateCriteria(
        //        typeof(ProtocoloIncidenciaCalidad));

        //    crit.Add(Expression.Eq("IdProtocolo", p));

        //    IList items = crit.List();

        //    // Guardar los ID de las incidencias registradas.
        //    HashSet<int> incidencias = new HashSet<int>();

        //    foreach (ProtocoloIncidenciaCalidad oDet in items)
        //    {
        //        incidencias.Add(oDet.IdIncidenciaCalidad);
        //    }

        //    // Recorrer el árbol una sola vez.
        //    foreach (TreeNode nodo in TreeView2.Nodes)
        //    {
        //        MarcarIncidencias(nodo, incidencias);
        //    }
        //}

        private void MarcarIncidencias(
            TreeNode nodo,
            HashSet<int> incidencias)
        {
            int idIncidencia;

            if (int.TryParse(nodo.Value, out idIncidencia))
            {
                nodo.Checked = incidencias.Contains(idIncidencia);

                // Expandir los padres de las incidencias seleccionadas.
                if (nodo.Checked)
                {
                    TreeNode padre = nodo.Parent;

                    while (padre != null)
                    {
                        padre.Expanded = true;
                        padre = padre.Parent;
                    }
                }
            }

            // Recorrer los nodos hijos.
            foreach (TreeNode hijo in nodo.ChildNodes)
            {
                MarcarIncidencias(hijo, incidencias);
            }
        }


        //public void MostrarDatosdelProtocolo(int p)
        //{

        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloIncidenciaCalidad));
        //    crit.Add(Expression.Eq("IdProtocolo", p));

        //    IList items = crit.List();

        //    foreach (ProtocoloIncidenciaCalidad oDet in items)
        //    {
        //        for (int i = 0; i < TreeView2.Nodes.Count; i++)
        //        {

        //            if (TreeView2.Nodes[i].Value == oDet.IdIncidenciaCalidad.ToString())
        //            {
        //                TreeView2.Nodes[i].Checked = true;

        //            }
        //            for (int j = 0; j < TreeView2.Nodes[i].ChildNodes.Count; j++)
        //            {
        //                if (TreeView2.Nodes[i].ChildNodes[j].Value == oDet.IdIncidenciaCalidad.ToString())
        //                {
        //                    TreeView2.Nodes[i].ChildNodes[j].Checked = true;
        //                    TreeView2.Nodes[i].ExpandAll();
        //                }
        //            }
        //        }
        //    }
        //}

        private void ObtenerIncidenciasSeleccionadas(
            TreeNode nodo,
            Dictionary<int, string> seleccionadas)
        {
            int idIncidencia;

            if (nodo.Checked &&
                nodo.ShowCheckBox != false &&
                int.TryParse(nodo.Value, out idIncidencia))
            {
                if (!seleccionadas.ContainsKey(idIncidencia))
                {
                    seleccionadas.Add(idIncidencia, nodo.Text);
                }
            }

            foreach (TreeNode hijo in nodo.ChildNodes)
            {
                ObtenerIncidenciasSeleccionadas(hijo, seleccionadas);
            }
        }
        private string ObtenerNombreIncidencia(int idIncidencia)
        {
            IncidenciaCalidad oInci = new IncidenciaCalidad();

            oInci = (IncidenciaCalidad)oInci.Get(
                typeof(IncidenciaCalidad),
                idIncidencia
            );

            return oInci != null
                ? oInci.Nombre
                : idIncidencia.ToString();
        }
        public void GuardarProtocoloIncidencia(Protocolo oRegistro)
        {
            if (oRegistro == null)
                throw new ArgumentNullException("oRegistro");

            int idProtocolo = oRegistro.IdProtocolo;
            int idEfector = oRegistro.IdEfector.IdEfector;
            int idUsuario = Convert.ToInt32(Session["idUsuario"]);

            ISession m_session = NHibernateHttpModule.CurrentSession;

            // Obtener incidencias existentes del protocolo.
            ICriteria crit = m_session.CreateCriteria(
                typeof(ProtocoloIncidenciaCalidad));

            crit.Add(Expression.Eq("IdProtocolo", idProtocolo));

            IList existentes = crit.List();

            // Obtener incidencias seleccionadas en el TreeView.
            Dictionary<int, string> seleccionadas =
                new Dictionary<int, string>();

            foreach (TreeNode nodo in TreeView2.Nodes)
            {
                ObtenerIncidenciasSeleccionadas(nodo, seleccionadas);
            }

            // IDs existentes para búsquedas rápidas.
            HashSet<int> idsExistentes = new HashSet<int>();

            foreach (ProtocoloIncidenciaCalidad oDet in existentes)
            {
                idsExistentes.Add(oDet.IdIncidenciaCalidad);
            }

            // 1. Eliminar solamente las incidencias desmarcadas.
            foreach (ProtocoloIncidenciaCalidad oDet in existentes)
            {
                int id = oDet.IdIncidenciaCalidad;

                if (!seleccionadas.ContainsKey(id))
                {
                    string nombre = ObtenerNombreIncidencia(id);

                    oDet.Delete();

                    oRegistro.GrabarAuditoriaDetalleProtocolo(
                        "Elimina",
                        idUsuario,
                        "Incidencia",
                        nombre
                    );
                }
            }

            // 2. Insertar solamente las incidencias nuevas.
            foreach (KeyValuePair<int, string> item in seleccionadas)
            {
                if (!idsExistentes.Contains(item.Key))
                {
                    ProtocoloIncidenciaCalidad oDet =
                        new ProtocoloIncidenciaCalidad();

                    oDet.IdProtocolo = idProtocolo;
                    oDet.IdEfector = idEfector;
                    oDet.IdIncidenciaCalidad = item.Key;

                    oDet.Save();

                    oRegistro.GrabarAuditoriaDetalleProtocolo(
                        "Registra",
                        idUsuario,
                        "Incidencia",
                        item.Value
                    );
                }
            }
        }


        //public void GuardarProtocoloIncidencia(Protocolo oRegistro)
        //{
        //    //// borra los detalles para el existente y los crea de nuevo
        //    EliminarProtocoloIncidencia(oRegistro);
        //    for (int i = 0; i < TreeView2.Nodes.Count; i++)
        //    {
        //        if (TreeView2.Nodes[i].Checked)
        //        {
        //            ProtocoloIncidenciaCalidad oDet = new ProtocoloIncidenciaCalidad();
        //            oDet.IdProtocolo = oRegistro.IdProtocolo;
        //           oDet.IdEfector = oRegistro.IdEfector.IdEfector;

        //            oDet.IdIncidenciaCalidad = int.Parse(TreeView2.Nodes[i].Value); // int.Parse(TreeView1.CheckedNodes[i].Value);
        //            oDet.Save();
        //            oRegistro.GrabarAuditoriaDetalleProtocolo("Registra", int.Parse(Session["idUsuario"].ToString()), "Incidencia", TreeView2.Nodes[i].Text);
        //        }
        //        for (int j = 0; j < TreeView2.Nodes[i].ChildNodes.Count; j++)
        //        {
        //            if (TreeView2.Nodes[i].ChildNodes[j].Checked)
        //            {
        //                ProtocoloIncidenciaCalidad oDet = new ProtocoloIncidenciaCalidad();
        //                oDet.IdProtocolo = oRegistro.IdProtocolo;
        //                oDet.IdEfector = oRegistro.IdEfector.IdEfector;
        //                oDet.IdIncidenciaCalidad = int.Parse(TreeView2.Nodes[i].ChildNodes[j].Value); // int.Parse(TreeView1.CheckedNodes[i].Value);
        //                oDet.Save();
        //                oRegistro.GrabarAuditoriaDetalleProtocolo("Registra", int.Parse(Session["idUsuario"].ToString()), "Incidencia", TreeView2.Nodes[i].ChildNodes[j].Text);
        //            }
        //        }
        //    }


        //}




        public void EliminarProtocoloIncidencia(Protocolo oRegistro)
        {
            //// borra los detalles para el existente y los crea de nuevo
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloIncidenciaCalidad));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro.IdProtocolo));
            IList items = crit.List();

            foreach (ProtocoloIncidenciaCalidad oDet in items)
            {
                IncidenciaCalidad oInci = new IncidenciaCalidad();
                oInci = (IncidenciaCalidad)oInci.Get(typeof(IncidenciaCalidad), oDet.IdIncidenciaCalidad);
                if (oInci != null) oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Incidencia", oInci.Nombre);

                oDet.Delete();
            }

        }
    }
}