/*
insert license info here
*/
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using NHibernate.Expression;
using MathParser;
using System.IO;

namespace Business.Data.Laboratorio
{
    [Serializable]
    public sealed class Placa : Business.BaseDataAccess
    {

        #region Private Members
        private bool m_isChanged;

        private int m_idPlaca;
        private DateTime m_fecha;
        private string m_operador;
        private string m_equipo;
        private DateTime m_fechaRegistro;
        private int m_idUsuarioRegistro;
        private bool m_baja;
        private string m_estado;

        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public Placa()
        {
            m_idPlaca = 0;
            m_fecha = DateTime.MinValue;
            m_operador = String.Empty;
            m_equipo = String.Empty;
            m_fechaRegistro = DateTime.MinValue;
            m_idUsuarioRegistro = 0;
            m_estado = String.Empty;
        }

        #endregion // End of Default ( Empty ) Class Constuctor

        #region Required Fields Only Constructor
        /// <summary>
        /// required (not null) fields only constructor
        /// </summary>
        public Placa(
            int idPlaca,
            DateTime fecha,
            string operador,
            string equipo,
            DateTime fechaRegistro,
            int idUsuarioRegistro,
            string estado)
                : this()
        {
            m_idPlaca = idPlaca;
            m_fecha = fecha;
            m_operador = operador;
            m_equipo = equipo;
            m_fechaRegistro = fechaRegistro;
            m_idUsuarioRegistro = idUsuarioRegistro;
            m_estado = estado;
        }


        #endregion // End Required Fields Only Constructor

        #region Public Properties

        public int IdPlaca
        {
            get { return m_idPlaca; }
            set
            {
                m_isChanged |= (m_idPlaca != value);
                m_idPlaca = value;
            }

        }

        public DateTime Fecha
        {
            get { return m_fecha; }
            set
            {
                m_isChanged |= (m_fecha != value);
                m_fecha = value;
            }

        }

        public String Operador
        {
            get { return m_operador; }
            set
            {
                m_isChanged |= (m_operador != value);
                m_operador = value;
            }

        }

        public String Estado
        {
            get { return m_estado; }
            set
            {
                m_isChanged |= (m_estado != value);
                m_estado = value;
            }

        }

        public String Equipo
        {
            get { return m_equipo; }
            set
            {
                m_isChanged |= (m_equipo != value);
                m_equipo = value;
            }

        }

        public DateTime FechaRegistro
        {
            get { return m_fechaRegistro; }
            set
            {
                m_isChanged |= (m_fechaRegistro != value);
                m_fechaRegistro = value;
            }

        }

        public int IdUsuarioRegistro
        {
            get { return m_idUsuarioRegistro; }
            set
            {
                m_isChanged |= (m_idUsuarioRegistro != value);
                m_idUsuarioRegistro = value;
            }

        }

        public bool Baja
        {
            get { return m_baja; }
            set
            {
                m_isChanged |= (m_baja != value);
                m_baja = value;
            }
        }

        public bool IsChanged
        {
            get { return m_isChanged; }
        }

        public void BorrarDetalle()
        {
            DetalleProtocoloPlaca oDetalle = new DetalleProtocoloPlaca();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocoloPlaca));
            crit.Add(Expression.Eq("IdPlaca", this.IdPlaca));



            IList items = crit.List();

            foreach (DetalleProtocoloPlaca oDet in items)
            {
                oDet.Delete();
            }
        }

        public object getCeldas()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            string m_strSQL = "";
            switch (this.Equipo)
            { case "Promega":
                    { 
                    m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
 WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [AB2],[AB3],[AB4], [AB5],[AB6],[AB7], [AB8],[AB9],[AB10], [AB11],[AB12],
[CD1], [CD2],[CD3],[CD4], [CD5],[CD6],[CD7], [CD8],[CD9],[CD10], [CD11],[CD12],
[EF1], [EF2],[EF3],[EF4], [EF5],[EF6],[EF7], [EF8],[EF9],[EF10], [EF11],[EF12],
[GH1], [GH2],[GH3],[GH4], [GH5],[GH6],[GH7], [GH8],[GH9],[GH10], [GH11],[GH12] 
 


)) AS Child
"; break; }
                case "Alplex":
                    {   m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [A2],[A3],[A4], [A5],[A6],[A7], [A8],[A9],[A10], [A11],[A12],
[B1], [B2],[B3],[B4], [B5],[B6],[B7], [B8],[B9],[B10], [B11],[B12],
[C1], [C2],[C3],[C4], [C5],[C6],[C7], [C8],[C9],[C10], [C11],[C12],
[D1], [D2],[D3],[D4], [D5],[D6],[D7], [D8],[D9],[D10], [D11],[D12],
[E1], [E2],[E3],[E4], [E5],[E6],[E7], [E8],[E9],[E10], [E11],[E12],
[F1], [F2],[F3],[F4], [F5],[F6],[F7], [F8],[F9],[F10], [F11],[F12],
[G1], [G2],[G3],[G4], [G5],[G6],[G7], [G8],[G9],[G10], [G11],[G12],
[H1], [H2],[H3],[H4], [H5],[H6],[H7], [H8],[H9],[H10], [H11],[H12]


)) AS Child
";
            } break;

                case "Alplex7V":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d (nolock)
  inner join LAB_Placa as Pl (nolock) on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE (nolock) ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p (nolock) ON P.IDPROTOCOLO= DE.IDPROTOCOLO
WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [A2],[A3],[A4], [A5],[A6],[A7], [A8],[A9],[A10], [A11],[A12],
[B1], [B2],[B3],[B4], [B5],[B6],[B7], [B8],[B9],[B10], [B11],[B12],
[C1], [C2],[C3],[C4], [C5],[C6],[C7], [C8],[C9],[C10], [C11],[C12],
[D1], [D2],[D3],[D4], [D5],[D6],[D7], [D8],[D9],[D10], [D11],[D12],
[E1], [E2],[E3],[E4], [E5],[E6],[E7], [E8],[E9],[E10], [E11],[E12],
[F1], [F2],[F3],[F4], [F5],[F6],[F7], [F8],[F9],[F10], [F11],[F12],
[G1], [G2],[G3],[G4], [G5],[G6],[G7], [G8],[G9],[G10], [G11],[G12],
[H1], [H2],[H3],[H4], [H5],[H6],[H7], [H8],[H9],[H10], [H11],[H12]


)) AS Child
";
                    }
                    break;

                case "Promega-30M":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
 WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [ABC1],[ABC2],[ABC3], [ABC4],[ABC5],[ABC6], [ABC7],[ABC8],[ABC9], [ABC10],[ABC11],[ABC12],
[D123], [D456],[D789],[D101112], 
[E123], [E456],[E789],[E101112],
[FGH1], [FGH2],[FGH3],[FGH4], [FGH5],[FGH6],[FGH7], [FGH8],[FGH9],[FGH10], [FGH11],[FGH12] 
 


)) AS Child
";
                    }
                    break;
                          case "PromegaM":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
 WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [ABCD1],[ABCD2],[ABCD3], [ABCD4],[ABCD5],[ABCD6], [ABCD7],[ABCD8],[ABCD9], [ABCD10],[ABCD11],[ABCD12],
[EFGH1], [EFGH2],[EFGH3],[EFGH4], [EFGH5],[EFGH6],[EFGH7], [EFGH8],[EFGH9],[EFGH10], [EFGH11],[EFGH12] 
 


)) AS Child
"; break;
                    }

            }
            DataSet Ds = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
        public object getCeldasArchivo()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            string m_strSQL = "";
            switch (this.Equipo)
            {
                case "Promega":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
 WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [AB2],[AB3],[AB4], [AB5],[AB6],[AB7], [AB8],[AB9],[AB10], [AB11],[AB12],
[CD1], [CD2],[CD3],[CD4], [CD5],[CD6],[CD7], [CD8],[CD9],[CD10], [CD11],[CD12],
[EF1], [EF2],[EF3],[EF4], [EF5],[EF6],[EF7], [EF8],[EF9],[EF10], [EF11],[EF12],
[GH1], [GH2],[GH3],[GH4], [GH5],[GH6],[GH7], [GH8],[GH9],[GH10], [GH11],[GH12] 
 


)) AS Child
"; break;
                    }
                case "Alplex":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, convert(varchar,p.NUMERO)+ ' ' +substring(replace (replace (Ef.nombre,'H.',''), 'HOSPITAL',''),0,13) + ' ' + 
  case when P.fechaInicioSintomas<>'19000101' then 'FIS:' +convert(varchar, P.fechaInicioSintomas,103) else 'Sin FIS' end
   + ' ' + isnull( 
(select 'Cal='+SUBSTRING(valorct,1,3)  from LAB_ResultadoTemp where   tipoCT like 'Cal%' and idPlaca= " + this.IdPlaca.ToString() + @"
 and  celda=substring (d.NamespaceID,1,1)+ (case when len(d.NamespaceID)=2 then '0'+substring(d.NamespaceID,2,1) else substring(d.NamespaceID,2,2) end ))
 ,'')
   + ' ' + isnull( 
   (select 'FAM='+SUBSTRING(valorct,1,3)  from LAB_ResultadoTemp where   tipoCT like 'FAM%' and idPlaca=" + this.IdPlaca.ToString() + @"
 and  celda=substring (d.NamespaceID,1,1)+ (case when len(d.NamespaceID)=2 then '0'+substring(d.NamespaceID,2,1) else substring(d.NamespaceID,2,2) end ))
   ,'')
  + ' ' +  isnull( (select 'HEX='+SUBSTRING(valorct,1,4)  from LAB_ResultadoTemp where   tipoCT like 'HEX%' and idPlaca= " + this.IdPlaca.ToString() + @"
 and  celda=substring (d.NamespaceID,1,1)+ (case when len(d.NamespaceID)=2 then '0'+substring(d.NamespaceID,2,1) else substring(d.NamespaceID,2,2) end ))
 ,'')
  + ' ' +  isnull((select 'QUA='+SUBSTRING(valorct,1,4)  from LAB_ResultadoTemp where   tipoCT like 'QUA%' and idPlaca=" + this.IdPlaca.ToString() + @"
 and  celda=substring (d.NamespaceID,1,1)+ (case when len(d.NamespaceID)=2 then '0'+substring(d.NamespaceID,2,1) else substring(d.NamespaceID,2,2) end ))
  ,'')

    + ' ' + isnull( 
(select 'N='+SUBSTRING(valorct,1,3)  from LAB_ResultadoTemp where   tipoCT ='N' and idPlaca= " + this.IdPlaca.ToString() + @"
 and  celda=d.NamespaceID)
 ,'')
   + ' ' + isnull( 
   (select 'E='+SUBSTRING(valorct,1,3)  from LAB_ResultadoTemp where   tipoCT ='E' and idPlaca=" + this.IdPlaca.ToString() + @"
  and  celda=d.NamespaceID)
   ,'')
  + ' ' +  isnull( (select 'RdRp='+SUBSTRING(valorct,1,3)  from LAB_ResultadoTemp where   tipoCT='RdRp' and idPlaca= " + this.IdPlaca.ToString() + @"
  and  celda=d.NamespaceID)
 ,'')
  + ' ' +  isnull((select 'CI='+SUBSTRING(valorct,1,3)  from LAB_ResultadoTemp where   tipoCT ='CI' and idPlaca=" + this.IdPlaca.ToString() + @"
  and  celda=d.NamespaceID)
  ,'')
  + ' ..... ' + 
   
 (select CASE  WHEN COUNT(*) =4 THEN 'SE DETECTA'
 WHEN  COUNT(*) =1  THEN 'NO SE DETECTA'
 WHEN  (COUNT(*) >1 and  COUNT(*) <4 )   THEN 'INDETERMINADO' 
 WHEN   COUNT(*) =0  THEN '' END 
     from LAB_ResultadoTemp where valorct<>'NaN' AND tipoct in ('Cal Red 610', 'FAM','HEX', 'Quasar 670') AND  idPlaca= " + this.IdPlaca.ToString() + @"
 and  celda=substring (d.NamespaceID,1,1)+ (case when len(d.NamespaceID)=2 then '0'+substring(d.NamespaceID,2,1) else substring(d.NamespaceID,2,2) end ))
 
 +
   
 (select CASE  WHEN COUNT(*) =4 THEN 'SE DETECTA'
 WHEN  COUNT(*) =1  THEN 'NO SE DETECTA'
 WHEN  (COUNT(*) >1 and  COUNT(*) <4 )   THEN 'INDETERMINADO' 
 WHEN   COUNT(*) =0  THEN '' END 
     from LAB_ResultadoTemp where SUBSTRING(valorct,1,3)<>'Und' AND tipoct in ('N', 'E','CI', 'RdRp') AND  idPlaca= " + this.IdPlaca.ToString() + @"
 and  celda= d.NamespaceID  )
 

  as numero
 , d.NamespaceID  
       
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca

INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
  inner join sys_efector  Ef on Ef.idEfector= P.idEfectorSolicitante
WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [A2],[A3],[A4], [A5],[A6],[A7], [A8],[A9],[A10], [A11],[A12],
[B1], [B2],[B3],[B4], [B5],[B6],[B7], [B8],[B9],[B10], [B11],[B12],
[C1], [C2],[C3],[C4], [C5],[C6],[C7], [C8],[C9],[C10], [C11],[C12],
[D1], [D2],[D3],[D4], [D5],[D6],[D7], [D8],[D9],[D10], [D11],[D12],
[E1], [E2],[E3],[E4], [E5],[E6],[E7], [E8],[E9],[E10], [E11],[E12],
[F1], [F2],[F3],[F4], [F5],[F6],[F7], [F8],[F9],[F10], [F11],[F12],
[G1], [G2],[G3],[G4], [G5],[G6],[G7], [G8],[G9],[G10], [G11],[G12],
[H1], [H2],[H3],[H4], [H5],[H6],[H7], [H8],[H9],[H10], [H11],[H12]


)) AS Child
";
                    }
                    break;

                case "Promega-30M":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
 WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [ABC1],[ABC2],[ABC3], [ABC4],[ABC5],[ABC6], [ABC7],[ABC8],[ABC9], [ABC10],[ABC11],[ABC12],
[D123], [D456],[D789],[D101112], 
[E123], [E456],[E789],[E101112],
[FGH1], [FGH2],[FGH3],[FGH4], [FGH5],[FGH6],[FGH7], [FGH8],[FGH9],[FGH10], [FGH11],[FGH12] 
 


)) AS Child
"; break;
                    }

            }
            DataSet Ds = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
        public object getCeldasResultado()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            string m_strSQL = "";
            switch (this.Equipo)
            {
                case "Promega":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, convert(varchar,p.NUMERO)+ ' ' +substring(Ef.nombre,0,13)  + ' ' + 
  case when P.fechaInicioSintomas<>'19000101' then 'FIS:' +convert(varchar, P.fechaInicioSintomas,103) else 'Sin FIS' end  
   + ' ' + isnull( (select 'N1='+case when resultadocar='' then '....' else resultadocar  end  from LAB_DETALLEPROTOCOLO where   iditem=3179 and idProtocolo= P.IDPROTOCOLO),'')
   + ' ' + isnull( (select 'RP='+resultadocar from LAB_DETALLEPROTOCOLO where   iditem=3181 and idProtocolo= P.IDPROTOCOLO),'')
     + ' ' + substring (de.resultadocar,0,15) 
 as numero
  , d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
 inner join sys_efector  Ef on Ef.idEfector= P.idEfectorSolicitante
 WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [AB2],[AB3],[AB4], [AB5],[AB6],[AB7], [AB8],[AB9],[AB10], [AB11],[AB12],
[CD1], [CD2],[CD3],[CD4], [CD5],[CD6],[CD7], [CD8],[CD9],[CD10], [CD11],[CD12],
[EF1], [EF2],[EF3],[EF4], [EF5],[EF6],[EF7], [EF8],[EF9],[EF10], [EF11],[EF12],
[GH1], [GH2],[GH3],[GH4], [GH5],[GH6],[GH7], [GH8],[GH9],[GH10], [GH11],[GH12] 
 


)) AS Child
"; break;
                    }
                case "Alplex":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, convert(varchar,p.NUMERO)+ ' ' +substring(replace(Ef.nombre,'H.',''),0,10) + ' ' + 
  case when P.fechaInicioSintomas<>'19000101' then 'FIS:' +convert(varchar, P.fechaInicioSintomas,103) else 'Sin FIS' end
   + ' ' + isnull( (select 'Cal='+resultadocar from LAB_DETALLEPROTOCOLO m where   exists (select * from lab_item i where codigo='CAL' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'')
   + ' ' + isnull( (select 'FAM='+resultadocar from LAB_DETALLEPROTOCOLO m  where   exists (select * from lab_item i where codigo='FAM' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'')
   + ' ' +  isnull((select 'HEX='+resultadocar from LAB_DETALLEPROTOCOLO m where    exists (select * from lab_item i where codigo='HEX' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'')
 + ' ' +  isnull((select 'QUA='+resultadocar + '.............' from LAB_DETALLEPROTOCOLO m where     exists (select * from lab_item i where codigo='QUA' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'')
 + ' ' + isnull( (select 'N='+resultadocar from LAB_DETALLEPROTOCOLO m where   exists (select * from lab_item i where codigo='N' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'')
   + ' ' + isnull( (select 'RdR='+resultadocar from LAB_DETALLEPROTOCOLO m  where   exists (select * from lab_item i where codigo='RdR' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'')
   + ' ' +  isnull((select 'E='+resultadocar from LAB_DETALLEPROTOCOLO m where    exists (select * from lab_item i where codigo='E' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'')
 + ' ' +  isnull((select 'CI='+resultadocar+ '...............' from LAB_DETALLEPROTOCOLO m where     exists (select * from lab_item i where codigo='CI' and i.iditem=m.iditem) and idProtocolo= P.IDPROTOCOLO),'') 
  + ' ' + 
 isnull( substring( De.resultadoCar,0,14),'')  as numero, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca

INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
  inner join sys_efector  Ef on Ef.idEfector= P.idEfectorSolicitante
WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [A2],[A3],[A4], [A5],[A6],[A7], [A8],[A9],[A10], [A11],[A12],
[B1], [B2],[B3],[B4], [B5],[B6],[B7], [B8],[B9],[B10], [B11],[B12],
[C1], [C2],[C3],[C4], [C5],[C6],[C7], [C8],[C9],[C10], [C11],[C12],
[D1], [D2],[D3],[D4], [D5],[D6],[D7], [D8],[D9],[D10], [D11],[D12],
[E1], [E2],[E3],[E4], [E5],[E6],[E7], [E8],[E9],[E10], [E11],[E12],
[F1], [F2],[F3],[F4], [F5],[F6],[F7], [F8],[F9],[F10], [F11],[F12],
[G1], [G2],[G3],[G4], [G5],[G6],[G7], [G8],[G9],[G10], [G11],[G12],
[H1], [H2],[H3],[H4], [H5],[H6],[H7], [H8],[H9],[H10], [H11],[H12]


)) AS Child

";
                    }
                    break;

                case "Promega-30M":
                    {
                        m_strSQL = @"SELECT    Child.* FROM (
  SELECT pl.idPlaca,  pl.fecha, pl.operador, p.NUMERO, d.NamespaceID 
  FROM LAB_DetalleProtocoloPLACA  AS d
  inner join LAB_Placa as Pl on Pl.idPlaca = d.idplaca
INNER JOIN LAB_DETALLEPROTOCOLO AS DE ON D.idDetalleProtocolo=DE.idDetalleProtocolo
INNER JOIN LAB_PROTOCOLO AS p ON P.IDPROTOCOLO= DE.IDPROTOCOLO
 WHERE pl.IDPLACA=" + this.IdPlaca.ToString() + @"
  
  )
  pvt PIVOT (MAX(NUMERO) 
FOR NamespaceID IN (  [ABC1],[ABC2],[ABC3], [ABC4],[ABC5],[ABC6], [ABC7],[ABC8],[ABC9], [ABC10],[ABC11],[ABC12],
[D123], [D456],[D789],[D101112], 
[E123], [E456],[E789],[E101112],
[FGH1], [FGH2],[FGH3],[FGH4], [FGH5],[FGH6],[FGH7], [FGH8],[FGH9],[FGH10], [FGH11],[FGH12] 
 


)) AS Child
"; break;
                    }

            }
            DataSet Ds = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
        public void GrabarAuditoria(string m_accion, int m_idusuario, string valor)
        {
            AuditoriaPlaca oRegistro = new AuditoriaPlaca();
            oRegistro.Accion = m_accion;
            oRegistro.Fecha = DateTime.Now;
            oRegistro.Hora = DateTime.Now.ToLongTimeString();
            oRegistro.IdPlaca = this.IdPlaca;
            oRegistro.Valor = valor;
            oRegistro.IdUsuario = m_idusuario;
            oRegistro.Save();
        
    }
    }
}

        #endregion


