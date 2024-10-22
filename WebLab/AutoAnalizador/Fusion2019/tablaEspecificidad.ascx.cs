using Business;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebLab.AutoAnalizador.Fusion2019
{
    public partial class tablaEspecificidad : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

     

        public void CargarGrillaProtocolo(string s_nroProtocolo, int tipo)
        {

            //Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            //oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id));
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            CrystalReportSource oCr = new CrystalReportSource();

            ParameterDiscreteValue nroProtocolo = new ParameterDiscreteValue();
            nroProtocolo.Value = s_nroProtocolo;

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;

         

            if (tipo == 1)
            {
                oCr.Report.FileName = "especificidad.rpt";
                oCr.ReportDocument.SetDataSource(LeerDatosProtocolos(s_nroProtocolo));
            }
            else
            {
                oCr.Report.FileName = "especificidad2.rpt";
                oCr.ReportDocument.SetDataSource(LeerDatosProtocolos2(s_nroProtocolo));
            }
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(nroProtocolo);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(encabezado3);
            oCr.DataBind();                  

            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Especifidad_"+tipo.ToString()+"_"+ s_nroProtocolo );
            //GridView1.DataSource = LeerDatosProtocolos(s_nroProtocolo);
            //GridView1.DataBind();
            //Exportar(s_nroProtocolo,"1");

        }

    
        private object LeerDatosProtocolos(string s_nroProtocolo)
        {
           
            string m_strSQL = @" SELECT tipovalor as [tipo], [Anti HLA-A], [Anti HLA-B],[Anti HLA-C], U.firmaValidacion as firma
FROM
(      SELECT valor, tipovalor, idsubitem, case  tipoValor
when 'Positivo Fuerte' then 1
when 'Positivo Moderado' then 2
when 'Positivo Debil' then 3
when 'Negativo' then 4
end as orden, idusuariovalida
    FROM LAB_ProtocoloLuminex where idProtocolo=" + s_nroProtocolo + @"
) AS SourceTable PIVOT(max(valor) FOR idsubitem IN([Anti HLA-A], [Anti HLA-B],[Anti HLA-C])) AS PivotTable
left join sys_usuario U on U.idusuario= idusuariovalida
order by orden  ";
        

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];

        }


     


        private object LeerDatosProtocolos2(string s_nroProtocolo)
        {
             
            string m_strSQL = @" SELECT tipovalor as [tipo], [Anti HLA-DR], [Anti HLA-DQ],[Anti HLA-DP], U.firmaValidacion as firma
FROM
(
      SELECT valor, tipovalor, idsubitem, case  tipoValor
when 'Positivo Fuerte' then 1
when 'Positivo Moderado' then 2
when 'Positivo Debil' then 3
when 'Negativo' then 4
end as orden, idusuariovalida
    FROM LAB_ProtocoloLuminex where  idProtocolo=" + s_nroProtocolo + @"
) AS SourceTable PIVOT(max(valor) FOR idsubitem IN([Anti HLA-DR], [Anti HLA-DQ],[Anti HLA-DP])) AS PivotTable
left join sys_usuario U on U.idusuario= idusuariovalida
                                                         order by orden";


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];

        }
    }
}