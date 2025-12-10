using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Data.SqlClient;
using System.Data;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Drawing;

namespace WebLab.Resultados
{
    public partial class ATBValida : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (!Page.IsPostBack)
            {
                string s_iditem = Request["idItem"].ToString();
                string s_idProtocolo = Request["idProtocolo"].ToString();
                string s_idGermen = Request["idGermen"].ToString();
                string s_idMetodo= Request["idMetodo"].ToString();
                string s_numeroAislamiento = Request["numeroAislamiento"].ToString();
                switch (int.Parse(s_idMetodo))
                {
                    case 0: lblMetodo.Text = "Disco"; break;
                    case 1: lblMetodo.Text = "CIM"; break;
                    case 2: lblMetodo.Text = "eTest"; break;
                }
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));
                lblProtocolo.Text = oProtocolo.GetNumero();

                Germen oGermen = new Germen();
                oGermen = (Germen)oGermen.Get(typeof(Germen), int.Parse(s_idGermen));

                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(s_iditem));

                lblGermen.Text = s_numeroAislamiento + " " + oGermen.Nombre;
                lblPractica.Text = oItem.Nombre;

              


                CargarPerfilAntibiotico();
                CargarListaAntibiotico();
                CargarMecanismos(oProtocolo, oGermen, oItem, int.Parse(s_idMetodo));
                ActualizarVistaAntibiograma();

            }
          
        }

        private void CargarMecanismos(Protocolo oProtocolo, Germen oGermen, Item oItem, int v)
        {
            Utility oUtil = new Utility();
            string m_ssql = @"SELECT idMecanismoResistencia, sigla as nombre FROM LAB_MecanismoResistencia with (nolock)  order by nombre";
            oUtil.CargarCheckBox(chkMecanismoResistencia, m_ssql, "idMecanismoResistencia", "nombre");

            for (int i = 0; i < chkMecanismoResistencia.Items.Count; i++)
            {

                chkMecanismoResistencia.Items[i].Selected = false;
               
                }
                


                ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloAtbMecanismo));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            crit.Add(Expression.Eq("IdMetodologia", v));
            crit.Add(Expression.Eq("IdGermen", oGermen));
            crit.Add(Expression.Eq("IdItem", oItem.IdItem));
             
            IList lista = crit.List();
            foreach (ProtocoloAtbMecanismo oRegistro in lista)
            {
                int mec1 = oRegistro.IdMecanismoResistencia.IdMecanismoResistencia;
                for (int i = 0; i < chkMecanismoResistencia.Items.Count; i++)
                {
                   
                    int mec2 = int.Parse(chkMecanismoResistencia.Items[i].Value);
                    if (mec1 == mec2)
                    {
                        chkMecanismoResistencia.Items[i].Selected = true;
                        break;
                    }

                

                }

            }
        }



        private void CargarPerfilAntibiotico()
        {
            Utility oUtil = new Utility();
            ///Carga los perfiles de  Antibioticos
            string m_ssql = @" SELECT DISTINCT PA.idPerfilAntibiotico, PA.nombre
FROM         LAB_PerfilAntibiotico AS PA with (nolock) INNER JOIN
                      LAB_DetallePerfilAntibiotico AS DPA with (nolock) ON PA.idPerfilAntibiotico = DPA.idPerfilAntibiotico INNER JOIN
                      LAB_Antibiotico AS A with (nolock) ON DPA.idAntibiotico = A.idAntibiotico
WHERE     (PA.baja = 0)
ORDER BY PA.nombre";
            oUtil.CargarCombo(ddlPerfilAntibiotico, m_ssql, "idPerfilAntibiotico", "nombre");
            //ddlPerfilAntibiotico.Items.Insert(0, new ListItem("--SELECCIONE PERFIL ANTIBIOTICOS--", "0"));
            ddlPerfilAntibiotico.Items.Insert(0, new ListItem("--TODOS LOS ANTIBIOTICOS--", "0"));
            //////////////////////////////                              
        }
        private void ActualizarVistaAntibiograma()
        {
      


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_GetAntibiogramaEditar]";

            int s_iditem = int.Parse(Request["idItem"].ToString());
            int s_idProtocolo =int.Parse( Request["idProtocolo"].ToString());
            int s_idGermen = int.Parse(Request["idGermen"].ToString());
            int s_idMetodo = int.Parse(Request["idMetodo"].ToString());
            int s_numeroAislamiento = int.Parse(Request["numeroAislamiento"].ToString());
         
            /*
             * store nuevo: parametros int grupo, int subgrupo, int etiologia, int anio, string semana, string codigo, string sexo, int id_establecimiento, string usuario_sivila
             */

            cmd.Parameters.Add("@idItem", SqlDbType.Int); cmd.Parameters["@idItem"].Value = s_iditem;
            cmd.Parameters.Add("@idProtocolo", SqlDbType.Int); cmd.Parameters["@idProtocolo"].Value = s_idProtocolo;
            cmd.Parameters.Add("@idGermen", SqlDbType.Int); cmd.Parameters["@idGermen"].Value = s_idGermen;
            cmd.Parameters.Add("@idMetodo", SqlDbType.Int); cmd.Parameters["@idMetodo"].Value = s_idMetodo;
            cmd.Parameters.Add("@numeroAislamiento", SqlDbType.Int); cmd.Parameters["@numeroAislamiento"].Value = s_numeroAislamiento;
          

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
        
            gvAntiobiograma.DataSource = Ds.Tables[0];
            gvAntiobiograma.DataBind();

   
            foreach (GridViewRow row in gvAntiobiograma.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (row.Cells[6].FindControl("lblEstado") as Label);
                    if (lbl!=null)
                    {
                        Antibiograma oAtb = new Antibiograma();
                        oAtb = (Antibiograma)oAtb.Get(typeof(Antibiograma), int.Parse(gvAntiobiograma.DataKeys[row.RowIndex].Value.ToString()));

                        if (oAtb.IdUsuarioRegistro == 0) //enviado por el equipo
                        {
                            //lblPersona.Text = "AUTOMÁTICO: " + oDetalle.FechaResultado.ToShortDateString() + " - " + oDetalle.FechaResultado.ToShortTimeString();
                            lbl.Text = "AUTOMATICO " + oAtb.FechaRegistro.ToShortDateString() + " - " + oAtb.FechaRegistro.ToShortTimeString();
                            lbl.ForeColor = Color.Red;
                        }
                        else
                        {
                            Usuario oUser = new Usuario();

                            oUser = (Usuario)oUser.Get(typeof(Usuario), oAtb.IdUsuarioRegistro);
                            lbl.Text = "Carg.: " + oUser.Apellido + " " + oUser.Nombre + " " + oAtb.FechaRegistro.ToShortDateString() + " " + oAtb.FechaRegistro.ToShortTimeString(); ;
                            lbl.ForeColor = Color.Black;

                        }
                        if (oAtb.IdUsuarioValida > 0) //enviado por el equipo
                        {
                            Usuario oUser = new Usuario();

                            oUser = (Usuario)oUser.Get(typeof(Usuario), oAtb.IdUsuarioValida);
                            lbl.Text = "Val.: "+ oUser.FirmaValidacion + " " + oAtb.FechaValida.ToShortDateString() + " " + oAtb.FechaValida.ToShortTimeString();
                            lbl.ForeColor = Color.Blue;
                        }
                        lbl.Font.Size = FontUnit.Point(7);
                        lbl.Font.Italic = true;
                       
                    }
                }
            }




        }
        protected void ddlPerfilAntibiotico_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarListaAntibiotico();
           
        }


        private void CargarListaAntibiotico()
        {
            Utility oUtil = new Utility();
            ///Carga los antibioticos para la solapa Antibiograma

            string m_ssql = "";
            if (ddlPerfilAntibiotico.SelectedValue == "0") ///Todos los antibioticos            
                m_ssql = " SELECT idAntibiotico , descripcion FROM LAB_Antibiotico with (nolock) where baja=0 order by descripcion ";
            
            else            
                m_ssql = @" SELECT DISTINCT A.idAntibiotico, A.descripcion
FROM         LAB_PerfilAntibiotico AS PA with (nolock) INNER JOIN
                      LAB_DetallePerfilAntibiotico AS DPA with (nolock) ON PA.idPerfilAntibiotico = DPA.idPerfilAntibiotico INNER JOIN
                      LAB_Antibiotico AS A with (nolock) ON DPA.idAntibiotico = A.idAntibiotico
WHERE     (PA.idPerfilAntibiotico = " + ddlPerfilAntibiotico.SelectedValue + ")  ORDER BY A.descripcion";
            

            oUtil.CargarCombo(ddlAntibiotico, m_ssql, "idAntibiotico", "descripcion");
            ddlAntibiotico.Items.Insert(0, new ListItem("--Seleccione--", "0"));
        }
        protected void btnGuardarAntibiograma_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
               
                    GuardarAntibiograma();
                    ddlPerfilAntibiotico.SelectedValue = "0";
                    ddlAntibiotico.SelectedValue = "0";
                    ddlResultado.SelectedValue = "0";
                    txtValor.Text = "";
                    ActualizarVistaAntibiograma();
               
            }
        }

       

        private void GuardarAntibiograma()
        {
            string s_iditem = Request["idItem"].ToString();
            string s_idProtocolo = Request["idProtocolo"].ToString();
            string s_idGermen = Request["idGermen"].ToString();
            string s_idMetodo = Request["idMetodo"].ToString();
            string s_numeroAislamiento = Request["numeroAislamiento"].ToString();

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));

            Germen oGermen = new Germen();
            oGermen = (Germen)oGermen.Get(typeof(Germen), int.Parse(s_idGermen));

            Usuario oUser = new Usuario();
           // if (Session["idUsuarioValida"]!=null)
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
          //  else
           //     oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

         

            Antibiograma oRegistro = new Antibiograma();
            oRegistro.IdProtocolo = oProtocolo;
            oRegistro.IdEfector = oProtocolo.IdEfector;
            oRegistro.IdGermen = oGermen;
            oRegistro.NumeroAislamiento = int.Parse( s_numeroAislamiento);
            oRegistro.IdItem = int.Parse(s_iditem);

            Antibiotico oAntibiotico = new Antibiotico();
            oAntibiotico = (Antibiotico)oAntibiotico.Get(typeof(Antibiotico), int.Parse(ddlAntibiotico.SelectedValue));
            oRegistro.IdAntibiotico = oAntibiotico;


            oRegistro.IdMetodologia = int.Parse(s_idMetodo);
            oRegistro.Resultado = ddlResultado.SelectedValue;                     
            
            oRegistro.Valor = txtValor.Text;
         

            oRegistro.IdUsuarioRegistro = oUser.IdUsuario;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.IdUsuarioValida = oUser.IdUsuario;
            oRegistro.FechaValida = DateTime.Parse("01/01/1900");
            oRegistro.Save();                                

            oProtocolo.GrabarAuditoriaDetalleProtocolo("Alta", oUser.IdUsuario, "ATB " + oRegistro.NumeroAislamiento.ToString()+ " " +  oRegistro.IdGermen.Nombre +  " ("+lblMetodo.Text +") - " + oRegistro.IdAntibiotico.Descripcion, oRegistro.Resultado + " " + oRegistro.Valor);


               
        }

        protected void ddlPerfilAntibiotico_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CargarListaAntibiotico();
            ddlAntibiotico.UpdateAfterCallBack = true;
        }

        protected void gvAntiobiograma_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName=="Eliminar")
            {EliminarAntibiotico(e.CommandArgument.ToString());
                ActualizarVistaAntibiograma();
            }
            
        }

        private void EliminarAntibiotico(string anti)
        {
            Antibiograma oRegistro = new Antibiograma();
            oRegistro = (Antibiograma)oRegistro.Get(typeof(Antibiograma), int.Parse(anti));


            if (oRegistro != null)
            {
                oRegistro.Delete();
                oRegistro.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "ATB: " + oRegistro.IdGermen.Nombre + "(" + lblMetodo.Text + ") - " + oRegistro.IdAntibiotico.Descripcion, oRegistro.Resultado + " " + oRegistro.Valor);
            }
           
            
        }

       
      

        protected void gvAntiobiograma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[7].Controls[1];
                CmdModificar.CommandArgument = this.gvAntiobiograma.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Eliminar";
                CmdModificar.ToolTip = "Eliminar";

            
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
                      //////////////////
          string s_iditem = Request["idItem"].ToString();
            string s_idProtocolo = Request["idProtocolo"].ToString();
            string s_idGermen = Request["idGermen"].ToString();

            string s_idMetodo = Request["idMetodo"].ToString();

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));

            Germen oGermen = new Germen();
            oGermen = (Germen)oGermen.Get(typeof(Germen), int.Parse(s_idGermen));


            Antibiotico oAntibiotico = new Antibiotico();
            oAntibiotico = (Antibiotico)oAntibiotico.Get(typeof(Antibiotico), int.Parse(ddlAntibiotico.SelectedValue));

            ////////////////


            Antibiograma oATB = new Antibiograma();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Antibiograma));

         
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            crit.Add(Expression.Eq("IdMetodologia", int.Parse(s_idMetodo)));
            crit.Add(Expression.Eq("IdGermen", oGermen));
            crit.Add(Expression.Eq("IdItem", int.Parse(s_iditem)));

            crit.Add(Expression.Eq("IdAntibiotico", oAntibiotico));
              IList lista = crit.List();
            if (lista.Count > 0)
                 args.IsValid=false;
            else
                  args.IsValid=true;


              
        }

        protected void btnValidarATB_Click(object sender, EventArgs e)
        {


            ValidarATB(true);
            ActualizarVistaAntibiograma();
        }

        private void ValidarATB(bool accion)
        {
            string operacion;
            if (accion) operacion = "Valida"; else operacion = "Desvalida";
            string s_idProtocolo = Request["idProtocolo"].ToString();
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));

            foreach (GridViewRow row in gvAntiobiograma.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[2].FindControl("chkValidar") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Antibiograma oAtb = new Antibiograma();
                        oAtb = (Antibiograma)oAtb.Get(typeof(Antibiograma), int.Parse(gvAntiobiograma.DataKeys[row.RowIndex].Value.ToString()));
                        if (accion == true) //Validar
                        {                        
                            oAtb.FechaValida = DateTime.Now;
                            oAtb.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                        }
                        else //Desvalidar
                        {
                            
                            oAtb.FechaValida = DateTime.Parse("01/01/1900");
                            oAtb.IdUsuarioValida = 0;
                        }
                        oAtb.Save();

                        /// graba auditoria
                       
                        
                        oProtocolo.GrabarAuditoriaDetalleProtocolo(operacion, oAtb.IdUsuarioValida, "ATB " + oAtb.NumeroAislamiento.ToString() + " " + oAtb.IdGermen.Nombre + " (" + oAtb.IdMetodologia.ToString() + ") - " + oAtb.IdAntibiotico.Descripcion, oAtb.Resultado);                   
                        /// fin de grabar auditoria
                    }

                    CheckBox chkRow2 = (row.Cells[4].FindControl("chkValidarValor") as CheckBox);
                    if (chkRow2.Checked)
                    {
                        Antibiograma oAtb = new Antibiograma();
                        oAtb = (Antibiograma)oAtb.Get(typeof(Antibiograma), int.Parse(gvAntiobiograma.DataKeys[row.RowIndex].Value.ToString()));
                        oAtb.ValidaValor = accion;   //este campo define si valida o desvalida                                                                     
                        oAtb.Save();
                    }

                }
            }
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);

        }

        private void MarcarSeleccionados(bool p)
        {

            foreach (GridViewRow row in gvAntiobiograma.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("chkValidar")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("chkValidar"))).Checked = p;
            }
          
        }

        protected void lnkDesMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        protected void btnDesValidar_Click(object sender, EventArgs e)
        {
            ValidarATB(false);
            ActualizarVistaAntibiograma();
        }

        protected void btnValidarMecanismo_Click(object sender, EventArgs e)
        {
            try
            {
                //int mecSel = 0;
                //for (int i = 0; i < chkMecanismoResistencia.Items.Count; i++)
                //{
                //    if (chkMecanismoResistencia.Items[i].Selected)
                //        mecSel += 1;
                //}
                //if (mecSel > 0)//si hay al menos uno seleccionado hago algo sino no.
                //{
                string s_iditem = Request["idItem"].ToString();
                string s_idProtocolo = Request["idProtocolo"].ToString();
                string s_idGermen = Request["idGermen"].ToString();
                string s_idMetodo = Request["idMetodo"].ToString();
                string s_numeroAislamiento = Request["numeroAislamiento"].ToString();

                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));

                Germen oGermen = new Germen();
                oGermen = (Germen)oGermen.Get(typeof(Germen), int.Parse(s_idGermen));
                if ((oProtocolo!=null) && (oGermen!= null))
                    GuardarMecanismo(oProtocolo, oGermen, int.Parse(s_idMetodo), int.Parse(s_iditem));

                lblMensajeMecanismo.Visible = true;
                lblMensajeMecanismo.Text = "Mecanismos actualizados";
               }
            catch (Exception ex)
            {                
                lblMensajeMecanismo.Visible = true;
                lblMensajeMecanismo.Text = "Ha ocurrido un error. Avise al administrador";
            }
        }

        private void GuardarMecanismo(Protocolo oProtocolo, Germen oGermen, int idmetodologia, int iditem)
        {
            try
            { 
            Usuario oUser = new Usuario();
            // if (Session["idUsuarioValida"]!=null)
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
            if (oUser != null)
            {             

                for (int i = 0; i < chkMecanismoResistencia.Items.Count; i++)
                {
                   

                        MecanismoResistencia oM = new MecanismoResistencia();
                        oM = (MecanismoResistencia)oM.Get(typeof(MecanismoResistencia), int.Parse(chkMecanismoResistencia.Items[i].Value));
                        ///elimina mecanismos para el atb 
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit2 = m_session.CreateCriteria(typeof(ProtocoloAtbMecanismo));
                        crit2.Add(Expression.Eq("IdProtocolo", oProtocolo));
                        crit2.Add(Expression.Eq("IdGermen", oGermen));
                        crit2.Add(Expression.Eq("IdItem", iditem));
                        crit2.Add(Expression.Eq("IdMetodologia", idmetodologia));
                        crit2.Add(Expression.Eq("IdMecanismoResistencia", oM));

                        IList lista2 = crit2.List();

                        if ((chkMecanismoResistencia.Items[i].Selected) && (lista2.Count == 0))//esta seleccionado y no no existe lo agrego                                                
                        {
                            ProtocoloAtbMecanismo oRegistro = new ProtocoloAtbMecanismo();
                            oRegistro.IdProtocolo = oProtocolo;
                            oRegistro.IdGermen = oGermen;
                            oRegistro.IdMetodologia = idmetodologia;
                            oRegistro.IdItem = iditem;
                            oRegistro.IdMecanismoResistencia = oM;
                            oRegistro.Save();
                            oProtocolo.GrabarAuditoriaDetalleProtocolo("Graba", oUser.IdUsuario, "ATB: " + oGermen.Nombre + "- Mecanismo", oM.Nombre);
                        }
                            if ((!chkMecanismoResistencia.Items[i].Selected) && (lista2.Count > 0))//no esta seleccionado y no no existe no hago nada, pero si existe lo borro ya que no esta seleccionado                                             
                            {
                                foreach (ProtocoloAtbMecanismo oRegistro2 in lista2)
                                {
                                    oRegistro2.Delete();
                                    oRegistro2.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(oUser.IdUsuario.ToString()), "ATB: " + oRegistro2.IdGermen.Nombre + "- Mecanismo", oM.Nombre);

                                }
                            }
                     
                    }

                
            }
            }
            catch (Exception ex)
            {
                lblMensajeMecanismo.Visible = true;
                lblMensajeMecanismo.Text = "Ha ocurrido un error. Avise al administrador";
            }
        }

    }
}