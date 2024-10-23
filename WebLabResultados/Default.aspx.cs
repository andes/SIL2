using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLabResultados
{
   public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    Business.Utility oUtil = new Business.Utility();
                    string param = Request["de"].ToString();  //va a produccion www.saludnqn.gob.ar/ressil/default.aspx?de=228n396277 ==>encriptado
                                                              //string param = oUtil.Encrypt("228n396277");
                                                              //Request["EF"]
                                                              //      Request["idProtocolo"];

                    param = param.Replace(' ', '+').Replace('-', '+').Replace('_', '/').PadRight(4 * ((param.Length + 3) / 4), '=');
                    string deco = oUtil.DecryptarNet(param,"SIL",256);
                    string[] ar = deco.Split('n');
                    string idEfe = ar[0];
                    string idpro = ar[1];

                    //Context.Items.Add("EF", idEfe); // Request["EF"].ToString()); ///Laboratorio central

                    //Context.Items.Add("idProtocolo", idpro); // "3455"); // Request["idProtocolo"].ToString());
                    //                                         //Context.Items.Add("idProtocolo", "3455"); // "3455"); // Request["idProtocolo"].ToString());

                    //Server.Transfer("Resultados/ResultadoView.aspx");

                    Response.Redirect("Resultados/ResultadoView.aspx?de="+param);

                }
                catch (Exception ex)
                {
                    string exception = "";
                    //while (ex != null)
                    //{
                    exception = ex.Message ;
                    lblError.Text = exception  ;
                    //}
                }
            }
        }
    }
}