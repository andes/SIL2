/*control de cambios
Hoja de Trabajo. Correccion en impresion para separar el numero de origen del numero de protocolo.
*/
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
alter PROCEDURE [dbo].[LAB_GeneraHT]
	@fechaDesde varchar(10),
	@fechaHasta varchar(10),
	@idHojaTrabajo int,
	@idEfectorSolicitante int,
	@idOrigen int,
	@idPrioridad int,
	@idSector varchar(800)    ,
	@estado varchar(1), --- si es 0 todos ; si es 1 solo pendiente
	@numeroDesde int,
	@numeroHasta int   ,
	@desdeUltimoNumero int ,@tipoMuestra varchar (800),
		@idCaracter  varchar(100)  

AS
BEGIN
---20.03.2025. Correccion para separar el numero de origen con guion.
	SET NOCOUNT ON;

--exec LAB_GeneraHT '20250313','20250313',231,0,0,1,'',0,'','',0,0,''

--DECLARE @TableItemProtocolo AS TABLE( idProtocolo int)
create table #TableItemHT (idItem int, textoImprimir nvarchar(max), orden int)
create table #TableFinal (idProtocolo int,
idItem int, cantidad varchar(max), textoImprimir varchar(max), 
idhojaTrabajo int, orden int, antecedente bit, paciente varchar(100)
,fila int  
 --idPaciente int,
 --idPrioridad int,
 --idOrigen int ,
 --idEfectorSolicitante int,
 --letra  varchar(50) collate database_default,
 --numero  int ,
 --numeroOrigen varchar(50) collate database_default,
 --fecha datetime ,
 --edad int ,
 --unidadEdad int ,
 --sexo nvarchar(1) collate database_default,
 --idMuestra int,
 -- medico varchar(100) collate database_default
 )


create table #TableItemProtocolo
( idProtocolo int,
paciente varchar(100), idSubItem int)
create table #TableItemProtocoloAux
( idProtocolo int,paciente varchar(100), idSubItem int)

declare @idItemPivot int
declare @Protocolo int
declare @existe int
declare @texto nvarchar(max)
declare @ssql nvarchar(max)
declare @scondicion nvarchar(max)
declare @orden int
declare @resultado varchar(50)
declare @conAntecedente bit
declare @cantidadLineaAdicional int
declare @ultimoNumeroListado int
declare @idMuestra int
declare @buscaMedico bit
declare @medico nvarchar(100)

declare @contadorProtocolo int

declare @idEfector int

--select * from LAB_hojaTrabajo

select @idEfector=idEfector,@conAntecedente=imprimirAntecedente, @cantidadLineaAdicional= cantidadLineaAdicional from LAB_hojaTrabajo where idHojaTrabajo=@idHojaTrabajo

set @scondicion='P.idEfector='+convert(varchar,@idEfector)+' and  P.baja=0  and (P.fecha>='''+@fechaDesde+''') and (P.fecha<='''+@fechaHasta+''')'
set @scondicion=@scondicion + ' and DHT.idHojaTrabajo ='+ convert(varchar, @idHojaTrabajo) +' and   (DP.trajoMuestra = ''Si'') '

if @estado=1
set @scondicion=@scondicion + ' and DP.conresultado=0  and P.estado<2 '

if @idEfectorSolicitante<>0
	set @scondicion=@scondicion + ' and P.idEfectorSolicitante=' +  convert(varchar,@idEfectorSolicitante)
if @idOrigen<>0
	set @scondicion=@scondicion + ' and P.idOrigen=' +  convert(varchar,@idOrigen)
if @idPrioridad<>0
	set @scondicion=@scondicion + ' and P.idPrioridad=' +  convert(varchar,@idPrioridad)
if @idSector<>''
	set @scondicion=@scondicion + ' and P.idSector in (' +  @idSector + ')'

declare @TipoNumeracionProtocolo int

Select top 1 @TipoNumeracionProtocolo= tiponumeracionProtocolo from LAB_Configuracion

if @numeroDesde>0
	begin
		if @TipoNumeracionProtocolo=0 
			set @scondicion=@scondicion + ' and P.numero>=' + convert(varchar, @numeroDesde)
		if @TipoNumeracionProtocolo=1 
			set @scondicion=@scondicion + ' and P.numeroDiario>=' +  convert(varchar, @numeroDesde)
		if @TipoNumeracionProtocolo=2 
			set @scondicion=@scondicion + ' and P.numeroSector>=' +convert(varchar, @numeroDesde)
	    if @TipoNumeracionProtocolo=3 
			set @scondicion=@scondicion + ' and P.numeroTipoServicio>=' +convert(varchar, @numeroDesde)
	end


if @numeroHasta>0
	begin
		if @TipoNumeracionProtocolo=0 
			set @scondicion=@scondicion + ' and P.numero<=' + convert(varchar, @numeroHasta)
		if @TipoNumeracionProtocolo=1 
			set @scondicion=@scondicion + ' and P.numeroDiario<=' +  convert(varchar, @numeroHasta)
		if @TipoNumeracionProtocolo=2 
			set @scondicion=@scondicion + ' and P.numeroSector<=' + convert(varchar, @numeroHasta)
		if @TipoNumeracionProtocolo=3 
			set @scondicion=@scondicion + ' and P.numeroTipoServicio<=' + convert(varchar, @numeroHasta)
	end

--Desde aca filtra por el ultimo numero listado

set @ultimoNumeroListado=0

-------------se agrega el id del ultimo protocolo listado para recordar
select @conAntecedente=imprimirAntecedente, @cantidadLineaAdicional= cantidadLineaAdicional,
@buscaMedico=imprimirMedico,
@ultimoNumeroListado= idUltimoProtocoloListado from LAB_hojaTrabajo where idHojaTrabajo=@idHojaTrabajo
------------------------


set @scondicion=@scondicion +'  and P.baja=0  and (P.fecha>='''+@fechaDesde+''') and (P.fecha<='''+@fechaHasta+''')'
set @scondicion=@scondicion + ' and DHT.idHojaTrabajo ='+ convert(varchar, @idHojaTrabajo) +' and   (DP.trajoMuestra = ''Si'') '

-------------se agrega el id del ultimo protocolo listado para recordar
if @desdeUltimoNumero=1 and @ultimoNumeroListado>0
	set @scondicion=@scondicion+' and P.idProtocolo> ' + convert (varchar,@ultimoNumeroListado)
------------------------------------------------------------------------------


if @idCaracter<>''
	set @scondicion=@scondicion + ' and P.idCaracter in (' +  @idCaracter + ')'


---pone en una tabla temporal los item de la hoja de trabajo
insert into #TableItemHT    
SELECT  idItem, textoImprimir, idDETALLEHojaTrabajo as orden 
FROM dbo.LAB_DetalleHojaTrabajo 
WHERE   idHojaTrabajo =@idHojaTrabajo 
order by idDETALLEHojaTrabajo
--------------------------
set @ssql='insert into #TableItemProtocoloAux  
		SELECT DISTINCT DP.idProtocolo, SUBSTRING(Pac.apellido + '' '' + Pac.nombre, 0, 14) as paciente, DP.idSubItem 
		FROM         dbo.LAB_DetalleProtocolo AS DP with (nolock)
		INNER JOIN	LAB_Protocolo as P  with (nolock) on P.idProtocolo = DP.idProtocolo 
		inner join  LAB_DetalleHojaTrabajo AS DHT  with (nolock) ON DP.idSubItem = DHT.idItem
		INNER JOIN   dbo.Sys_Paciente AS Pac  with (nolock) ON Pac.idPaciente = P.idPaciente
		WHERE   '	+ @scondicion

print (@ssql)
exec (@ssql)

declare @Paciente varchar(100)



create table #TableNumeroFilaProtocolo(orden int , idProtocolo int)

insert into #TableNumeroFilaProtocolo
--select  ROW_NUMBER(), idProtocolo from #TableItemProtocoloAux group by idProtocolo
Select distinct
       ROW_NUMBER() OVER (ORDER BY idProtocolo ) as orden,
       s1.* 
from 
     (select distinct idProtocolo from  #TableItemProtocoloAux group by idProtocolo) s1
--group by idProtocolo
order by 
       idProtocolo 

--recorre por cada item y se fija si está en cada protocolo; si no está lo pone igual; pero con un XXX
WHILE (Select count (*) from #TableItemHT)>0
	BEGIN
		SELECT top 1 @idItemPivot=idItem, @texto=textoImprimir, @orden=orden  FROM #TableItemHT 
		
		insert into #TableItemProtocolo select * from #TableItemProtocoloAux
	

		WHILE (Select count (*) from #TableItemProtocolo  )>0
			begin
				set @existe=0
				
				--select top 1 @Protocolo=idProtocolo, @Paciente=paciente from #TableItemProtocolo 					

				--select top 1 @existe=idSubitem from LAB_detalleProtocolo where idProtocolo=@Protocolo and idSubitem=@idItemPivot

				select top 1 @Protocolo=idProtocolo, @Paciente=paciente from #TableItemProtocolo 					
				select top 1 @existe=idSubitem from #TableItemProtocolo where idProtocolo=@Protocolo and idSubitem=@idItemPivot
				select  @contadorProtocolo= orden from  #TableNumeroFilaProtocolo where idProtocolo=@Protocolo
				if @existe=0										
						insert into #TableFinal values (@Protocolo,@idItemPivot,'xxx',@texto,@idHojaTrabajo,@orden, 0,@Paciente,@contadorProtocolo)																	
				else
					begin
						insert into #TableFinal values (@Protocolo,@idItemPivot,'___',@texto,@idHojaTrabajo,@orden,0,@Paciente,@contadorProtocolo)
						---buscar antecedente: ultimo resultado obtenido por el paciente.
						if @conAntecedente=1
							begin
								set @resultado= dbo.BuscarUltimoResultado(@Protocolo,@idItemPivot)						
								---print @resultado
								insert into #TableFinal values (@Protocolo,@idItemPivot,@resultado,@texto,@idHojaTrabajo,@orden,1,@Paciente,@contadorProtocolo)
							end
					end
				delete from #TableItemProtocolo where  idProtocolo= @Protocolo

			end

delete  FROM #TableItemHT  where idItem=@idItemPivot
end



--declare @ssql varchar(max)
declare @ordenProtocolo varchar (100)
declare @letra varchar(20)
declare @numero varchar(20)

select @ordenProtocolo= case Con.tipoNumeracionProtocolo
			 when 0 then 'P.numero'
			 when 1 then 'P.fecha,P.numerodiario'
			 when 2 then 'P.prefijosector,P.numerosector'
			 when 3 then 'letra, P.numeroTipoServicio'
			 end 
	from  LAB_Configuracion  as Con 


select @letra= case Con.tipoNumeracionProtocolo
			 when 0 then '''A'''
			 when 1 then 'P.fecha'
			 when 2 then 'P.prefijosector'
			 when 3 then '''A'''
			 end 
	from  LAB_Configuracion  as Con 


select @numero= case Con.tipoNumeracionProtocolo
			  when 0 then 'P.numero'
			 when 1 then 'P.numerodiario'
			 when 2 then 'P.numerosector'
			 when 3 then 'P.numeroTipoServicio'
			 end 
	from  LAB_Configuracion  as Con 

--  select * from #TableFinal
--  select * from #TableFinal

--case when Tf.antecedente=0 then  dbo.NumeroProtocolo(P.idProtocolo) else   dbo.NumeroProtocolo(P.idProtocolo)+''-Antecedente'' end as numero ,
create table #TableFinalAdicional (fila nvarchar(4), numero int, idHojaTrabajo int)

declare @i as int
set @i=1
while (@i<=@cantidadLineaAdicional)
	begin
		insert into #TableFinalAdicional (fila,numero,idHojaTrabajo) values ('zzz', @i+9999999 ,@idHojaTrabajo)
		set @i=@i+1
	end 


	 
	set @ssql= ' SELECT   TOP (100) PERCENT '+@letra +' as letra,'+ @numero +' as numero ,
	tf.orden, Tf.cantidad, case when Tf.antecedente=0 then ''0'' else ''1'' end as antecedente,
	CASE WHEN HT.imprimirprioridad = 1 THEN ''-'' + SUBSTRING(Pri.nombre, 1, 1) ELSE '''' END AS prioridad, 
	CASE WHEN HT.imprimirOrigen = 1 THEN ''-'' + SUBSTRING(O.nombre, 1, 1) ELSE '''' END AS origen, 
	CASE WHEN HT.imprimirApellidoNombre = 1 OR HT.imprimirEdad = 1 OR HT.imprimirSexo = 1 THEN 1 ELSE 0 END AS datosPaciente, 
	CASE WHEN HT.imprimirEdad = 1 THEN ''-'' + CONVERT(varchar(50), P.edad) ELSE '''' END AS edad,
	CASE WHEN HT.imprimirSexo = 1 THEN ''-'' + P.sexo ELSE '''' END AS sexo, 
	CASE WHEN HT.imprimirApellidoNombre = 1 THEN ''-'' + TF.Paciente ELSE '''' END AS paciente, 
	TF.textoImprimir AS item, A.nombre AS area, TF.fila AS ordenProtocolo, CONVERT(VARCHAR(10), P.fecha, 103) AS fecha, HT.responsable, 
	HT.idHojaTrabajo, HT.codigo AS codigoHT, HT.textoInferiorDerecha, HT.textoInferiorIzquierda, '' '' as medico,
	case when  P.numeroOrigen<>'''' then ''-''+ P.numeroOrigen else '''' end as numeroOrigen, '' '' as muestra
	FROM         #TableFinal AS TF 
	INNER JOIN   dbo.LAB_Protocolo AS P  with (nolock) ON TF.idProtocolo = P.idProtocolo  
	INNER JOIN   dbo.LAB_HojaTrabajo AS HT  with (nolock) ON HT.idHojaTrabajo = TF.idHojaTrabajo
	INNER JOIN   dbo.LAB_Area AS A  with (nolock) ON A.idArea = HT.idArea 
	INNER JOIN   dbo.LAB_Prioridad AS Pri  with (nolock) ON Pri.idPrioridad = P.idPrioridad 
	INNER JOIN   dbo.LAB_Origen AS O  with (nolock) ON O.idOrigen = P.idOrigen '
	

	 

if @cantidadLineaAdicional>0
set @ssql=@ssql+ ' 
union 


select Ad.fila as letra, Ad.numero as numero, tf.orden as orden, ''___'' as cantidad, ''0'' as antecedente, '''' as prioridad, '''' as origen,0 as datosPaciente,
'''' as edad, '''' as sexo, '''' as paciente,TF.textoImprimir AS item, A.nombre AS area, Ad.numero AS ordenProtocolo, CONVERT(VARCHAR(10), P.fecha, 103) AS fecha, HT.responsable, 
HT.idHojaTrabajo, HT.codigo AS codigoHT, HT.textoInferiorDerecha, HT.textoInferiorIzquierda, '' '' as medico, P.numeroOrigen as numeroOrigen, '' '' as muestra
FROM         #TableFinal AS TF 
inner join #TableFinalAdicional as Ad on Ad.idhojaTrabajo= Tf.idhojatrabajo
INNER JOIN   dbo.LAB_Protocolo AS P  with (nolock) ON TF.idProtocolo = P.idProtocolo  
INNER JOIN   dbo.LAB_HojaTrabajo AS HT  with (nolock) ON HT.idHojaTrabajo = TF.idHojaTrabajo
INNER JOIN   dbo.LAB_Area AS A  with (nolock) ON A.idArea = HT.idArea 
INNER JOIN   dbo.LAB_Prioridad AS Pri  with (nolock) ON Pri.idPrioridad = P.idPrioridad 
INNER JOIN   dbo.LAB_Origen AS O  with (nolock) ON O.idOrigen = P.idOrigen 

order by ' + @ordenProtocolo

print (@ssql)
exec (@ssql)



drop table  #TableItemHT
drop table #TableFinal 
drop table  #TableItemProtocolo
drop table  #TableItemProtocoloAux
 
drop table  #TableFinalAdicional 

END


   

 go
alter PROCEDURE [dbo].[LAB_GeneraHTconResultados]
	@fechaDesde varchar(10),
	@fechaHasta varchar(10),
	@idHojaTrabajo int,
	@idEfectorSolicitante int,
	@idOrigen int,
	@idPrioridad int,
	@idSector varchar(800)    ,
	@estado varchar(1), --- si es 0 todos ; si es 1 solo pendiente
	@numeroDesde int,
	@numeroHasta int   ,
	@desdeUltimoNumero int ,@tipoMuestra varchar (800),
		@idCaracter  varchar(100)  

AS
BEGIN

--exec LAB_GeneraHTconResultados  '20211001','20211031',46,0,1,1,'',0, 0,0, 0,0,''
---20.03.2025. Correccion para separar el numero de origen con guion.
	SET NOCOUNT ON;

declare @hiv bit
set @hiv=0
select @hiv=codificaHiv from LAB_DetalleHojaTrabajo as dHT
inner join LAB_Item as i on dHT.idItem= i.idItem
where   idHojaTrabajo=@idHojaTrabajo and codificaHiv=1

--DECLARE @TableItemProtocolo AS TABLE( idProtocolo int)
create table #TableItemHT (idItem int, textoImprimir nvarchar(max), orden int)

create table #TableFinal 
(idProtocolo int,
 idItem int ,
 cantidad varchar(max)  ,
 textoImprimir varchar(max)  ,
 idhojaTrabajo int , 
 orden int ,
 antecedente bit,
 paciente varchar(100)  , 
 fila int ,
 idPaciente int,
 idPrioridad int,
 idOrigen int ,
 idEfectorSolicitante int,
 letra  varchar(50)  ,
 numero  int ,
 numeroOrigen varchar(50)  ,
 fecha datetime ,
 edad int ,
 unidadEdad int ,
 sexo nvarchar(1)  ,
 idMuestra int,
 medico varchar(100)  )
 

create table #TableItemProtocolo( idProtocolo int,paciente varchar(100)  , idSubItem int,idPaciente int,
idPrioridad int, idOrigen int, idEfectorSolicitante int, letra  varchar(50)  , numero  int,numeroOrigen varchar(50)  ,
fecha datetime, edad int, unidadEdad int, sexo nvarchar(1)  , idMuestra int, valor varchar(500), medico varchar(100)  )

create table #TableItemProtocoloAux ( idProtocolo int,paciente varchar(100)  , idSubItem int,idPaciente int,
idPrioridad int, idOrigen int, idEfectorSolicitante int, letra  varchar(50)  , numero  int,numeroOrigen varchar(50)  ,
fecha datetime, edad int, unidadEdad int, sexo nvarchar(1)  , idMuestra int, valor varchar(500) , medico varchar(100) )

declare @idItemPivot int
declare @Protocolo int
declare @existe int
declare @texto nvarchar(max)
declare @ssql nvarchar(max)
declare @scondicion nvarchar(max)
declare @orden int
declare @resultado varchar(50)
declare @conAntecedente bit
declare @cantidadLineaAdicional int

declare @contadorProtocolo int
declare @ultimoNumeroListado int
set @ultimoNumeroListado=0
----------------------
declare @ordenProtocolo varchar (100)
declare @letra varchar(50)
declare @numero varchar(100)
declare @Paciente varchar(100)
declare @idPaciente int
declare @idPrioridad1 int
declare @idOrigen1 int
declare @idEfectorSolicitante1 int
declare @numeroOrigen varchar(50)
declare @fecha datetime
declare @edad int 
declare @unidadEdad int
declare @sexo nvarchar(1)
declare @valor varchar(500)
declare @idMuestra int
-------------------

declare @buscaMedico bit
declare @medico nvarchar(100)
 

declare @idEfector int

--select * from LAB_hojaTrabajo

select @idEfector=idEfector, @conAntecedente=imprimirAntecedente, @cantidadLineaAdicional= cantidadLineaAdicional ,
@buscaMedico=imprimirMedico,
@ultimoNumeroListado= idUltimoProtocoloListado from LAB_hojaTrabajo where idHojaTrabajo=@idHojaTrabajo

set @scondicion= 'P.idEfector='+convert(varchar,@idEfector)+' AND   P.baja=0  and (P.fecha>='''+@fechaDesde+''') and (P.fecha<='''+@fechaHasta+''')'
set @scondicion=@scondicion + ' and DHT.idHojaTrabajo ='+ convert(varchar, @idHojaTrabajo) +' and   (DP.trajoMuestra = ''Si'') '

-------------se agrega el id del ultimo protocolo listado para recordar
if @desdeUltimoNumero=1 and @ultimoNumeroListado>0
	set @scondicion=@scondicion+' and P.idProtocolo> ' + convert (varchar,@ultimoNumeroListado)
------------------------------------------------------------------------------

if @estado=1
set @scondicion=@scondicion + ' and DP.conresultado=0 and P.estado<>2 '

if @idEfectorSolicitante<>0
	set @scondicion=@scondicion + ' and P.idEfectorSolicitante=' +  convert(varchar,@idEfectorSolicitante)
if @idOrigen<>0
	set @scondicion=@scondicion + ' and P.idOrigen=' +  convert(varchar,@idOrigen)
if @idPrioridad<>0
	set @scondicion=@scondicion + ' and P.idPrioridad=' +  convert(varchar,@idPrioridad)
if @idSector<>''
	set @scondicion=@scondicion + ' and P.idSector in (' +  @idSector + ')'

declare @TipoNumeracionProtocolo int

Select top 1 @TipoNumeracionProtocolo= tiponumeracionProtocolo,
@letra= case tipoNumeracionProtocolo
			 when 0 then '''A'''
			 when 1 then 'P.fecha'
			 when 2 then 'P.prefijosector'
			 when 3 then '''A'''
			 end ,
			 @numero= case tipoNumeracionProtocolo
			  when 0 then 'P.numero'
			 when 1 then 'P.numerodiario'
			 when 2 then 'P.numerosector'
			 when 3 then 'P.numeroTipoServicio'
			 end 
 from LAB_Configuracion

if @numeroDesde>0
	begin
		if @TipoNumeracionProtocolo=0 
			set @scondicion=@scondicion + ' and P.numero>=' + convert(varchar, @numeroDesde)
		if @TipoNumeracionProtocolo=1 
			set @scondicion=@scondicion + ' and P.numeroDiario>=' +  convert(varchar, @numeroDesde)
		if @TipoNumeracionProtocolo=2 
			set @scondicion=@scondicion + ' and P.numeroSector>=' +convert(varchar, @numeroDesde)
	    if @TipoNumeracionProtocolo=3 
			set @scondicion=@scondicion + ' and P.numeroTipoServicio>=' +convert(varchar, @numeroDesde)
	end


if @numeroHasta>0
	begin
		if @TipoNumeracionProtocolo=0 
			set @scondicion=@scondicion + ' and P.numero<=' + convert(varchar, @numeroHasta)
		if @TipoNumeracionProtocolo=1 
			set @scondicion=@scondicion + ' and P.numeroDiario<=' +  convert(varchar, @numeroHasta)
		if @TipoNumeracionProtocolo=2 
			set @scondicion=@scondicion + ' and P.numeroSector<=' + convert(varchar, @numeroHasta)
		if @TipoNumeracionProtocolo=3 
			set @scondicion=@scondicion + ' and P.numeroTipoServicio<=' + convert(varchar, @numeroHasta)
	end


---pone en una tabla temporal los item de la hoja de trabajo
insert into #TableItemHT    
SELECT  idItem, textoImprimir, idDETALLEHojaTrabajo as orden 
FROM dbo.LAB_DetalleHojaTrabajo 
WHERE   idHojaTrabajo =@idHojaTrabajo 
order by idDETALLEHojaTrabajo
--------------------------

set @ssql='insert into #TableItemProtocoloAux  
		SELECT DISTINCT DP.idProtocolo,'''' as paciente,DP.idSubItem as idSubItem, P.idPaciente as idPaciente,
		P.idPrioridad as idPrioridad, P.idOrigen as idOrigen, P.idEfectorSolicitante as idEfectorSolicitante,
		'+ @letra +' as letra,
		'+@numero+' as numero, P.numeroOrigen as numeroOrigen, P.fecha as fecha, P.edad as edad, P.unidadEdad as unidadEdad, P.sexo as sexo,
		P.idMuestra,
		case when conResultado=1 then case when resultadoCar<>'''' then substring(resultadoCar,0,10) else 
		case I.formatoDecimal
			when 0 then convert(varchar(50),convert(int,resultadoNum) )
			when 1 then convert(varchar(50),convert(decimal(18,1), resultadoNum))
			when 2 then convert(varchar(50),convert(decimal (18,2),resultadoNum))
			when 3 then convert(varchar(50),convert(decimal(18,3),resultadoNum))
			when 4 then convert(varchar(50),convert (decimal(18,4), resultadoNum))
		end
		end else ''____'' end as valor, P.especialista as medico
		FROM         dbo.LAB_DetalleProtocolo AS DP with (nolock) 
		INNER JOIN dbo.LAB_item as I with (nolock) on i.iditem= DP.idsubitem
		INNER JOIN	LAB_Protocolo as P with (nolock) on P.idProtocolo = DP.idProtocolo 
		inner join  LAB_DetalleHojaTrabajo AS DHT with (nolock)  ON DP.idSubItem = DHT.idItem
	
		WHERE   '	+ @scondicion

print (@ssql)
exec (@ssql)


----------------------
create table #TableNumeroFilaProtocolo(orden int , idProtocolo int)

insert into #TableNumeroFilaProtocolo
--select  ROW_NUMBER(), idProtocolo from #TableItemProtocoloAux group by idProtocolo
Select distinct
       ROW_NUMBER() OVER (ORDER BY idProtocolo ) as orden,
       s1.* 
from 
     (select distinct idProtocolo from  #TableItemProtocoloAux group by idProtocolo) s1
--group by idProtocolo
order by 
     idProtocolo 
--recorre por cada item y se fija si está en cada protocolo; si no está lo pone igual; pero con un XXX
WHILE (Select count (*) from #TableItemHT)>0
	BEGIN
		SELECT top 1 @idItemPivot=idItem, @texto=textoImprimir, @orden=orden  FROM #TableItemHT 
		
		insert into #TableItemProtocolo select * from #TableItemProtocoloAux
	

		WHILE (Select count (*) from #TableItemProtocolo  )>0
			begin
				set @existe=0
				
				--select top 1 @Protocolo=idProtocolo, @Paciente=paciente from #TableItemProtocolo 					

				--select top 1 @existe=idSubitem from LAB_detalleProtocolo where idProtocolo=@Protocolo and idSubitem=@idItemPivot

			--	select top 1 @Protocolo=idProtocolo, @Paciente=paciente from #TableItemProtocolo 					
				select top 1 @Protocolo=idProtocolo,@Paciente=dbo.BuscarPaciente(idpaciente,@hiv), @idPaciente=idPaciente,
				@idPrioridad1=idPrioridad, @idOrigen1=idOrigen, @idEfectorSolicitante1=idEfectorSolicitante, @letra=letra,
				@numero=numero,@numeroOrigen=numeroOrigen,@fecha=fecha,@edad=edad,@unidadEdad=unidadEdad, @sexo=sexo, @idMuestra=idMuestra,
				@medico=case when @buscaMedico= 1 then  medico else  null end   
				from #TableItemProtocolo 					
				
				 
				select top 1 @existe=idSubitem, @valor= valor from #TableItemProtocolo where idProtocolo=@Protocolo and idSubitem=@idItemPivot
				select  @contadorProtocolo= orden from  #TableNumeroFilaProtocolo where idProtocolo=@Protocolo
				if @existe=0										
				insert into #TableFinal
						values (@Protocolo,@idItemPivot,'xxx',@texto,@idHojaTrabajo,@orden, 0,@Paciente,@contadorProtocolo,																						
						@idPaciente,@idPrioridad1, @idOrigen1, @idEfectorSolicitante1, @letra, @numero,@numeroOrigen ,
						@fecha, @edad, @unidadEdad, @sexo, @idMuestra,@medico)
				else
					begin
						--insert into #TableFinal values (@Protocolo,@idItemPivot,@valor,@texto,@idHojaTrabajo,@orden,0,@Paciente,@contadorProtocolo)
						
						insert into #TableFinal
						values (@Protocolo,@idItemPivot,@valor,@texto,@idHojaTrabajo,@orden, 0,@Paciente,@contadorProtocolo,																						
						@idPaciente,@idPrioridad1, @idOrigen1, @idEfectorSolicitante1, @letra, @numero,@numeroOrigen ,
						@fecha, @edad, @unidadEdad, @sexo, @idMuestra,@medico)
					end
				delete from #TableItemProtocolo where  idProtocolo= @Protocolo

			end

delete  FROM #TableItemHT  where idItem=@idItemPivot
end


 

select @ordenProtocolo= case Con.tipoNumeracionProtocolo
			 when 0 then 'P.numero'
			 when 1 then 'P.fecha,P.numerodiario'
			 when 2 then 'P.prefijosector,P.numerosector'
			 when 3 then 'letra, P.numeroTipoServicio'
			 end 
	from  LAB_Configuracion  as Con 


select @letra= case Con.tipoNumeracionProtocolo
			 when 0 then '''A'''
			 when 1 then 'P.fecha'
			 when 2 then 'P.prefijosector'
			 when 3 then '''A'''
			 end 
	from  LAB_Configuracion  as Con 


select @numero= case Con.tipoNumeracionProtocolo
			  when 0 then 'P.numero'
			 when 1 then 'P.numerodiario'
			 when 2 then 'P.numerosector'
			 when 3 then 'P.numeroTipoServicio'
			 end 
	from  LAB_Configuracion  as Con 
 

--case when Tf.antecedente=0 then  dbo.NumeroProtocolo(P.idProtocolo) else   dbo.NumeroProtocolo(P.idProtocolo)+''-Antecedente'' end as numero ,
create table #TableFinalAdicional (fila nvarchar(4), numero int, idHojaTrabajo int)

declare @i as int
set @i=1
while (@i<=@cantidadLineaAdicional)
	begin
		insert into #TableFinalAdicional (fila,numero,idHojaTrabajo) values ('zzz', @i+9999999 ,@idHojaTrabajo)
		set @i=@i+1
	end 



	--set @ssql= ' SELECT   TOP (100) PERCENT '+@letra +' as letra,'+ @numero +' as numero ,
	--tf.orden, Tf.cantidad, case when Tf.antecedente=0 then ''0'' else ''1'' end as antecedente,
	--CASE WHEN HT.imprimirprioridad = 1 THEN ''-'' + SUBSTRING(Pri.nombre, 1, 1) ELSE '''' END AS prioridad, 
	--CASE WHEN HT.imprimirOrigen = 1 THEN ''-'' + SUBSTRING(O.nombre, 1, 1) ELSE '''' END AS origen, 
	--CASE WHEN HT.imprimirApellidoNombre = 1 OR HT.imprimirEdad = 1 OR HT.imprimirSexo = 1 THEN 1 ELSE 0 END AS datosPaciente, 
	--CASE WHEN HT.imprimirEdad = 1 THEN ''-'' + CONVERT(varchar(50), P.edad) ELSE '''' END AS edad,
	--CASE WHEN HT.imprimirSexo = 1 THEN ''-'' + P.sexo ELSE '''' END AS sexo, 
	--CASE WHEN HT.imprimirApellidoNombre = 1 THEN ''-'' + TF.Paciente ELSE '''' END AS paciente, 
	--TF.textoImprimir AS item, A.nombre AS area, TF.fila AS ordenProtocolo, CONVERT(VARCHAR(10), P.fecha, 103) AS fecha, HT.responsable, 
	--HT.idHojaTrabajo, HT.codigo AS codigoHT, HT.textoInferiorDerecha, HT.textoInferiorIzquierda,  medico, P.numeroOrigen as numeroOrigen, '' '' as muestra
	--FROM         #TableFinal AS TF 
	--INNER JOIN   dbo.LAB_Protocolo AS P ON TF.idProtocolo = P.idProtocolo  
	--INNER JOIN   dbo.LAB_HojaTrabajo AS HT ON HT.idHojaTrabajo = TF.idHojaTrabajo
	--INNER JOIN   dbo.LAB_Area AS A ON A.idArea = HT.idArea 
	--INNER JOIN   dbo.LAB_Prioridad AS Pri ON Pri.idPrioridad = P.idPrioridad 
	--INNER JOIN   dbo.LAB_Origen AS O ON O.idOrigen = P.idOrigen 
	  --'  

	 
	set @ssql= ' SELECT   TOP (100) PERCENT '+@letra +' as letra,'+ @numero +' as numero ,
	tf.orden, Tf.cantidad, case when Tf.antecedente=0 then ''0'' else ''1'' end as antecedente,
	CASE WHEN HT.imprimirprioridad = 1 THEN ''-'' + SUBSTRING(Pri.nombre, 1, 1) ELSE '''' END AS prioridad, 
	CASE WHEN HT.imprimirOrigen = 1 THEN ''-'' + SUBSTRING(O.nombre, 1, 1) ELSE '''' END AS origen, 
	CASE WHEN HT.imprimirApellidoNombre = 1 OR HT.imprimirEdad = 1 OR HT.imprimirSexo = 1 THEN 1 ELSE 0 END AS datosPaciente, 
	CASE WHEN HT.imprimirEdad = 1 THEN ''-'' + CONVERT(varchar(50), P.edad) ELSE '''' END AS edad,
	CASE WHEN HT.imprimirSexo = 1 THEN ''-'' + P.sexo ELSE '''' END AS sexo, 
	CASE WHEN HT.imprimirApellidoNombre = 1 THEN ''-'' + TF.Paciente ELSE '''' END AS paciente, 
	TF.textoImprimir AS item, A.nombre AS area, TF.fila AS ordenProtocolo, CONVERT(VARCHAR(10), P.fecha, 103) AS fecha, HT.responsable, 
	HT.idHojaTrabajo, HT.codigo AS codigoHT, HT.textoInferiorDerecha, HT.textoInferiorIzquierda, '' '' as medico,
	case when  P.numeroOrigen<>'''' then ''-''+ P.numeroOrigen else '''' end as numeroOrigen, '' '' as muestra
	FROM         #TableFinal AS TF 
	INNER JOIN   dbo.LAB_Protocolo AS P with (nolock) ON TF.idProtocolo = P.idProtocolo  
	INNER JOIN   dbo.LAB_HojaTrabajo AS HT with (nolock)  ON HT.idHojaTrabajo = TF.idHojaTrabajo
	INNER JOIN   dbo.LAB_Area AS A with (nolock)  ON A.idArea = HT.idArea 
	INNER JOIN   dbo.LAB_Prioridad AS Pri with (nolock)  ON Pri.idPrioridad = P.idPrioridad 
	INNER JOIN   dbo.LAB_Origen AS O with (nolock)  ON O.idOrigen = P.idOrigen '
	

	 
 

if @cantidadLineaAdicional>0
set @ssql=@ssql+ ' 
union 


select Ad.fila as letra, Ad.numero as numero, tf.orden as orden, ''___'' as cantidad, ''0'' as antecedente, '''' as prioridad, '''' as origen,0 as datosPaciente,
'''' as edad, '''' as sexo, '''' as paciente,TF.textoImprimir AS item, A.nombre AS area, Ad.numero AS ordenProtocolo, CONVERT(VARCHAR(10), P.fecha, 103) AS fecha, HT.responsable, 
HT.idHojaTrabajo, HT.codigo AS codigoHT, HT.textoInferiorDerecha, HT.textoInferiorIzquierda, '' '' as medico, P.numeroOrigen as numeroOrigen, '' '' as muestra
FROM         #TableFinal AS TF 
inner join #TableFinalAdicional as Ad on Ad.idhojaTrabajo= Tf.idhojatrabajo
INNER JOIN   dbo.LAB_Protocolo AS P ON TF.idProtocolo = P.idProtocolo  
INNER JOIN   dbo.LAB_HojaTrabajo AS HT ON HT.idHojaTrabajo = TF.idHojaTrabajo
INNER JOIN   dbo.LAB_Area AS A ON A.idArea = HT.idArea 
INNER JOIN   dbo.LAB_Prioridad AS Pri ON Pri.idPrioridad = P.idPrioridad 
INNER JOIN   dbo.LAB_Origen AS O ON O.idOrigen = P.idOrigen 

order by ' + @ordenProtocolo

print (@ssql)
exec (@ssql)


 
 /*
drop table  #TableItemHT
drop table #TableFinal 
drop table  #TableItemProtocolo
drop table  #TableItemProtocoloAux

drop table  #TableNumeroFilaProtocolo
drop table  #TableFinalAdicional 
*/
END




