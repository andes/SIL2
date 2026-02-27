using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab
{
	public partial class select2 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				Utility oUtil = new Utility();

				string m_ssql = @" SELECT idEfector, nombre FROM Sys_Efector  (nolock) ORDER BY nombre ";
				oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
				oUtil.CargarCombo(ddlEfector3, m_ssql, "idEfector", "nombre");
				oUtil.CargarListBox(ddlEfector2, m_ssql, "idEfector", "nombre");
			}
		}
		
	}
}