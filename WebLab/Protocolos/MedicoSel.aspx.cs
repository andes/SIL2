using Business.Data;
using Business.Data.Laboratorio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebLab.Protocolos
{
    public partial class MedicoSel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["matricula"] = null;
                Session["apellidoNombre"] = null;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ///Buscar especilista
                string apellido = txtApellido.Text;
                string nombre = txtNombre.Text;
                string s_urlWFC = oCon.UrlMatriculacion;
                string s_url = s_urlWFC + "nombre=" + nombre + "&apellido=" + apellido;// + "&codigoProfesion=1 ";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(s_url);
                HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
               
                Stream st = ws1.GetResponseStream();
                StreamReader sr = new StreamReader(st);

                string s = sr.ReadToEnd();
                if (s != "0")
                {

                    //List<ProtocoloEdit2.ProfesionalMatriculado> pro = jsonSerializer.Deserialize<List<ProtocoloEdit2.ProfesionalMatriculado>>(s);

                    DataTable t = GetDataTableMatriculaciones(s); //GetJSONToDataTableUsingMethod(s);
                    gvMedico.DataSource = t;
                    gvMedico.DataBind();
                }
            }
            catch (Exception ex)
            {
                
            }
 

        }
        public static DataTable GetJSONToDataTableUsingMethod(string JSONData)
        {
            DataTable dtUsingMethodReturn = new DataTable();
            string[] jsonStringArray = Regex.Split(JSONData.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx).Replace("\"", "").Trim();
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dtUsingMethodReturn.Columns.Add(AddColumnName);
            }
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] RowData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dtUsingMethodReturn.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx).Replace("\"", "").Trim();
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dtUsingMethodReturn.Rows.Add(nr);
            }
            return dtUsingMethodReturn;
        }
        protected void gvMedico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdModificar = (LinkButton)e.Row.Cells[3].Controls[1];
                    CmdModificar.CommandArgument = this.gvMedico.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdModificar.CommandName = "Seleccionar";
                    CmdModificar.ToolTip = "Seleccionar";

                    // Valor adicional (Nombre y apellido)
                    DataRow rowData = ((DataRowView)e.Row.DataItem).Row;
                    CmdModificar.Attributes["nombre"] = rowData.ItemArray[0].ToString();
                    CmdModificar.Attributes["apellido"] = rowData.ItemArray[1].ToString();

                }
            }
        }

        protected void gvMedico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName== "Seleccionar")
            {
                Session["matricula"] = e.CommandArgument.ToString();
                LinkButton boton = (LinkButton)e.CommandSource;
                Session["apellidoNombre"] = boton.Attributes["apellido"] + " " + boton.Attributes["nombre"];
            }
        }

        private static DataTable GetDataTableMatriculaciones(string json)
        {
            //Pasa de JSON al tipo de objeto ProfesionalMatriculado
            List<Protocolos.ProtocoloEdit2.ProfesionalMatriculado>  personas = JsonConvert.DeserializeObject<List<Protocolos.ProtocoloEdit2.ProfesionalMatriculado>>(json);
            DataTable dt = new DataTable();
           
            if (personas.Count > 0)
            {
                //Guardo solo en la tabla aquellos datos que necesito
                dt.Columns.Add("nombre");
                dt.Columns.Add("apellido");
                dt.Columns.Add("titulo");
                dt.Columns.Add("matriculaNumero");

                foreach (Protocolos.ProtocoloEdit2.ProfesionalMatriculado persona in personas)
                {
                    foreach (Protocolos.ProtocoloEdit2.Profesiones prof in persona.profesiones)
                    {
                        foreach (Protocolos.ProtocoloEdit2.Matricula mat in prof.matriculacion)
                        {
                            if (DateTime.Compare(mat.fin, DateTime.Now) > 0) //Solo agrega las matriculas no vencidas
                            {
                                dt.Rows.Add(persona.nombre, persona.apellido, prof.titulo, mat.matriculaNumero);
                            }
                        }
                    }
                }
            }
            return dt;
        }


    }
}