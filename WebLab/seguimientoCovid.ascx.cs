using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab
{
    public partial class seguimientoCovid : System.Web.UI.UserControl
    {

        Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        { 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
                  
                     

               

        }

       


        public void MostrarSeguimiento()
        {
           

            //DataTable dtMuestrasTotal = MostrarTotales();
            //gvSeguimientoTotal.DataSource = dtMuestrasTotal;
            //gvSeguimientoTotal.DataBind();

         

            //DataTable dtincidencias = MostrarIncidencias();

            //gvSeguimientoIncidencias.DataSource = dtincidencias;
            //gvSeguimientoIncidencias.DataBind();

        }
//        private DataTable MostrarIncidencias()
//        {


//            string m_strSQL = @" select   'Incidencias'  , sum (cantidad) as cantidad
//from
//(
//select   count (*) as cantidad from LAB_IndicenciaRecepcion as i where i.fecha>='20200301'
 
//union
//select count (*) as cantidad
//from lab_protocolo p
//inner join LAB_ProtocoloIncidenciaCalidad as i on i.idProtocolo= p.idprotocolo
 
 
//inner join lab_detalleprotocolo as D on D.idprotocolo= P.idprotocolo 
//inner JOIN LAB_ITEM IT ON IT.idItem = D.iditem and IT.codigo='9122' 
 
//)x


//";




//            DataSet Ds = new DataSet();
//            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
//            adapter.Fill(Ds);

//            return Ds.Tables[0];
//        }

        private DataTable MostrarTotales(Item oItem)
        {



                string m_strSQL = @"select  'TOTAL PROTOCOLOS' as total, count (*) as cantidad, 1 b
 from
(
select  distinct
ir.idprotocolo
 from		
LAB_protocolo as IR
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo
where  
(DP.idsubitem= '" + oItem.IdItem.ToString() + @"' )  
and  ir.baja=0 
and ir.idEfector =" + oUser.IdEfector.IdEfector.ToString() +@"
 
) x
  
  union  
  select  'TOTAL PACIENTES' as total, count (*) AS CANTIDAD , 2 b
 from
(
select  distinct
ir.idpaciente
 from		
LAB_protocolo as IR
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo 
 
where  
(DP.idsubitem= '" + oItem.IdItem.ToString() + @"' )  
and  ir.baja=0  and ir.idPaciente>-1
and ir.idEfector =" + oUser.IdEfector.IdEfector.ToString() + @"
 
) x
UNION 
 select  'TOTAL NO PACIENTES (INVEST.)' as total, count (*) AS CANTIDAD, 3 b
 from
(
select  distinct
ir.idPROTOCOLO
 from		
LAB_protocolo as IR
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo 
 
where  
(DP.idsubitem= '" + oItem.IdItem.ToString() + @"' )  
and  ir.baja=0  and ir.idPaciente=-1
and ir.idEfector =" + oUser.IdEfector.IdEfector.ToString() + @"
 
) x

union 

 select   'TOTAL INCIDENCIAS' as total  , sum (cantidad) as cantidad, 4 b
from
(
select   count (*) as cantidad from LAB_IndicenciaRecepcion as i where i.fecha>='20200301'
 
union
select count (*) as cantidad
from lab_protocolo p
inner join LAB_ProtocoloIncidenciaCalidad as i on i.idProtocolo= p.idprotocolo  
inner join lab_detalleprotocolo as D on D.idprotocolo= P.idprotocolo  
where DP.idsubitem= '" + oItem.IdItem.ToString() + @"'  and ir.idEfector =" + oUser.IdEfector.IdEfector.ToString() + @"
)x
 order by b

";

             


            DataSet Ds = new DataSet();

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        private int VerificaPermisos(string sObjeto)
        {
            int i_permiso = 0;

            Utility oUtil = new Utility();
            i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
            return i_permiso;

        }
        protected void btnSeguimiento_Click(object sender, EventArgs e)
        {
            Response.Redirect("Seguimiento.aspx", false);
        }
        private DataTable MostrarDatos(Item oItem)
        {

            DateTime fecha1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                DateTime fecha2 = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(1);
             
            string m_strSQL = @"SELECT CANTIDAD,  [CoVid-19], B FROM(
select count (*) as cantidad, [CoVid-19],
CASE WHEN [CoVid-19] = 'EN PROCESO' THEN '1'
else '2'  
END AS B
from 
(
select  
case when  Dp.idUsuarioValida>0 then 'CON RESULTADOS' else
	case when Dp.idUsuarioPreValida>0 then 'CON RESULTADOS SIN CONFIRMACION' ELSE 'EN PROCESO' END
 end   AS [CoVid-19]
from		LAB_protocolo as IR
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo   
where  Dp.idsubitem= '" + oItem.IdItem.ToString() + @"'
 and ir.idEfector =" + oUser.IdEfector.IdEfector.ToString() + @" 
and  ir.baja=0 
 
)x
group by [CoVid-19]  
)Y


UNION

select   count (*) as cantidad , 'PROTOCOLOS DE HOY' as [CoVid-19]  , 4 AS B
from LAB_pROTOCOLO as P
inner join lab_detalleprotocolo as D on D.idprotocolo= P.idprotocolo  
where P.idefector=" + oUser.IdEfector.IdEfector.ToString() + @" and D.idsubitem= '" + oItem.IdItem.ToString() + @"' and P.BAJA=0 AND P.fecha>=GETDATE()-1
  
UNION
select  
count (*) as cantidad , 'RESULTADOS DE HOY' as [CoVid-19]  , 5 AS B   
 from		
LAB_protocolo as IR
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo 
 
where 
IR.idefector=" + oUser.IdEfector.IdEfector.ToString() + @" and  Dp.idsubitem= '" + oItem.IdItem.ToString() + @"' and
      dp.fechavalida >= '" + fecha1.ToString("yyyyMMdd") + "'  AND dp.fechavalida < '" + fecha2.ToString("yyyyMMdd") + @"'
            and  ir.baja=0 
UNION


 select  sum (cantidad) as cantidad,   'INCIDENCIAS DE HOY' as [CoVid-19]  ,6 AS B
from
(
select   count (*) as cantidad from LAB_IndicenciaRecepcion as i where i.fecha>=GETDATE()-1 
union
select count (*) as cantidad
from lab_protocolo p
inner join LAB_ProtocoloIncidenciaCalidad as i on i.idProtocolo= p.idprotocolo  
inner join lab_detalleprotocolo as D on D.idprotocolo= P.idprotocolo  
where 
P.idefector=" + oUser.IdEfector.IdEfector.ToString() + @" and D.idsubitem= '" + oItem.IdItem.ToString() + @"' and
P.fecha>=GETDATE()-1
 
 
)Y
ORDER BY B  ";


////            string m_strSQL = @" 

////select count (*) as cantidad, [CoVid-19],
////CASE WHEN [CoVid-19] = 'EN PROCESO' THEN '3'
////else '2'  
////END AS B
////from 
////(
////select  

////case when  Dp.idUsuarioValida>0 then 'CON RESULTADOS' else
////	case when Dp.idUsuarioPreValida>0 then 'CON RESULTADOS SIN CONFIRMACION' ELSE 'EN PROCESO' END
//// end   AS [CoVid-19]
//// from		
////LAB_protocolo as IR
////inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo
////inner JOIN LAB_ITEM IT ON IT.idItem = DP.iditem and IT.codigo='9122' 
 
////where  
////(IT.codigo= '9122')  
////and  ir.baja=0 
 
////)x
////group by [CoVid-19] order by b desc ";


             

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
          
            return Ds.Tables[0];
        }
        private DataTable MostrarCaracter(Item oItem)
        {

            Configuracion oCon = new Configuracion();
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);


            string m_strSQL = @"   
 select Caracter,   cantidad ,cast((cantidad*100)/
 (select count (*)
 from		LAB_protocolo as IR
 inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo and DP.idsubitem= '" + oItem.IdItem.ToString() + @"'   
 and IR.idEfector= " + oUser.IdEfector.IdEfector.ToString() + @"
) as float)as porcent
 
 from 
 (
select  Caracter, count (*) as cantidad
from 
(
select  C.nombre as Caracter
 from		LAB_protocolo as IR
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo   
 inner join lab_caracter  C on C.idcaracter= ir.idcaracter
where  
(DP.idsubitem= '" + oItem.IdItem.ToString() + @"')
and IR.idEfector= " + oUser.IdEfector.IdEfector.ToString() +@"
and  ir.baja=0 
 
)x
group by caracter
 
  )y
  order by cantidad desc 
 ";




            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        protected void bntResultados_Click(object sender, EventArgs e)
        {
           
            Response.Redirect("Estadisticas/Reporteporresultadocovid.aspx",false);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Estadisticas/epidemioprotocololist.aspx", false);

        }

        protected void btnFacturacion_Click(object sender, EventArgs e)
        {

        }

        protected void btnSeguimiento0_Click(object sender, EventArgs e)
        {
            Response.Redirect("SeguimientoResultados.aspx", false);
        }

        protected void btnSeguimiento1_Click(object sender, EventArgs e)
        {
            Response.Redirect("SeguimientoIncidencias.aspx", false);
        }

        protected void btnVerAcumulados_Click(object sender, EventArgs e)
        {


            //Configuracion oCon = new Configuracion();
            //oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Item));
            crit.Add(Expression.Eq("Codigo", oC.CodigoCovid));
            //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
            Item oItem = (Item)crit.UniqueResult();

            if (oItem != null)
            {
                DataTable dtMuestrasTotal = MostrarTotales(oItem);
                gvSeguimientoTotal.DataSource = dtMuestrasTotal;
                gvSeguimientoTotal.DataBind();
                gvSeguimientoTotal.UpdateAfterCallBack = true;

                DataTable dtMuestras1 = MostrarCaracter(oItem);


                gvSeguimiento0.DataSource = dtMuestras1;
                gvSeguimiento0.DataBind();
                gvSeguimiento0.UpdateAfterCallBack = true;
            }


        }

        protected void btnEstado_Click(object sender, EventArgs e)
        {
            //Configuracion oCon = new Configuracion();
            //oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Item));
            crit.Add(Expression.Eq("Codigo", oC.CodigoCovid));
            //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
            Item oItem = (Item)crit.UniqueResult();

            if (oItem != null)
            {
                DataTable dtMuestras = MostrarDatos(oItem);


                gvSeguimiento.DataSource = dtMuestras;
                gvSeguimiento.DataBind();
                gvSeguimiento.UpdateAfterCallBack = true;
            }
        }

        protected void btnResumenSemanal_Click(object sender, EventArgs e)
        {
            Response.Redirect("SeguimientoResultadosSemanal.aspx", false);
        }
    }
}