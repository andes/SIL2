
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

-- Stored Procedure

-- =============================================
-- Author:		Carolina Pintos
-- Create date: 30/11/2010
-- Description:	Genera la hoja de trabajo
/*10.04.2025. 
	Se agrega filtro idEfector para que sea multiefector 
	Se agrega dato de diagnostico a pedido del coordinador

*/
-- =============================================
alter PROCEDURE [dbo].[LAB_AnalisisFueraHT]
	@fechaDesde varchar(10),
	@fechaHasta varchar(10),	
	@idEfectorSolicitante int,
	@idOrigen int,
	@idPrioridad int,
	@idSector varchar(800)  , 
	@numeroDesde int,
	@numeroHasta int,
	@listaItem varchar(max)     ,
	@estado varchar(1), --- si es 0 todos ; si es 1 solo pendiente de resultado
	@idEfector int  ---reporte multiefector

AS
BEGIN

SET NOCOUNT ON;


declare @ssql nvarchar(max)
declare @scondicion nvarchar(max)
declare @TipoNumeracionProtocolo int

Select top 1 @TipoNumeracionProtocolo= tiponumeracionProtocolo from LAB_Configuracion


set @scondicion=' WHERE   P.baja=0 and DP.conresultado=0 and (P.fecha>='''+@fechaDesde+''') and (P.fecha<='''+@fechaHasta+''')'
if @estado=1
set @scondicion=@scondicion + ' and DP.conresultado=0 '



if @numeroDesde>0  
	set @scondicion=@scondicion + ' and P.numero>=' + convert(varchar, @numeroDesde)  

if @numeroHasta>0	 
	set @scondicion=@scondicion + ' and P.numero<=' + convert(varchar, @numeroHasta)
	 



if @idEfectorSolicitante<>0
	set @scondicion=@scondicion + ' and P.idEfectorSolicitante=' +  convert(varchar,@idEfectorSolicitante)
if @idOrigen<>0
	set @scondicion=@scondicion + ' and P.idOrigen=' +  convert(varchar,@idOrigen)
if @idPrioridad<>0
	set @scondicion=@scondicion + ' and P.idPrioridad=' +  convert(varchar,@idPrioridad)
if @idSector<>''
	set @scondicion=@scondicion + ' and P.idSector in (' + @idSector + ')'

if @listaItem<>''
	set @scondicion=@scondicion + ' and DP.idItem in (' +  @listaItem + ')'
	
if @idEfector<>0
set @scondicion=@scondicion + ' and P.idEfector= '+ convert(varchar,@idEfector)

set @ssql='SELECT P.numero as numero, convert(varchar(10),P.fecha,103) as fecha,
			 Pac.apellido + ''  '' + Pac.nombre 
			 +
			+ '' - '' + isnull(STUFF ((SELECT DISTINCT ''-'' +  S.nombre
          FROM    Sys_CIE10 S  with (nolock)
		  inner join LAB_ProtocoloDiagnostico as PD with (nolock) on P.idProtocolo= Pd.idProtocolo and PD.idDiagnostico= S.id 		
          FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(MAX)''), 1, 1, ''''),''Sin Diag.'')
			 AS paciente,
			 
			 P.edad, P.unidadEdad,
				P.sexo, I.nombre as determinacion
				FROM        dbo.LAB_Protocolo AS P with (nolock) 
				INNER JOIN  dbo.LAB_DetalleProtocolo AS DP with (nolock) ON P.idProtocolo = DP.idProtocolo 
				INNER JOIN  dbo.Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente 
				INNER JOIN   dbo.LAB_Item AS I with (nolock) ON DP.idItem = I.idItem '
				+ @scondicion +
				' ORDER BY i.NOMBRE, P.fecha, numero '

--print (@ssql)
exec (@ssql)


END
 