alter  PROCEDURE [dbo].[LABAPI_GetProtocolos]
	@estado varchar(10),  -- validado o temporal
	@numeroDocumento int,-- si es validado busca normal, si es temporal busca en los datos del familiar.	
	@fechaNacimiento varchar(10),-- dato adicional para validar del paciente - no parentesco
	@apellido varchar(200),-- dato adicional para validar del paciente- no parentesco
	@fechaDesde varchar(10), --fecha de protocolo desde
	@fechaHasta varchar(10)--fecha de protocolo hasta	

AS
BEGIN

--select idpaciente, count(*) as can from lab_protocolo 
--where idTipoServicio=1
--group by idpaciente
--having    count(*) >1
--order by idPaciente desc
--select * from sys_paciente where idpaciente=2128
--select * from lab_protocolo where idprotocolo=100002558

--exec LABAPI_GetProtocolos 'Validado',70326808, '20240620', 'DEL PINO SOTO', '20200101','20250130'----falta tipo de muestra
---con hiv
---Validado|10780104|19530609|ROSSI|20200101|20241231 --- con as un protocolo
---Validado|33951577|19880728|BARRERE|20200101|20241231 --- con as un protocolo
---Validado|14709926|19630320|OLAVE HERMOSILLA|20200101|20241231 --- con antibiobiograma
---temporal|26982063|20240301|PRUEBA|20200101|20241231 --- con as un protocolo
create table #pacientes (idPaciente int)
if UPPER(@estado)='TEMPORAL'
	insert into #pacientes
	select   Pac.idPaciente
	from Sys_Paciente   as Pac with (nolock)
	left join Sys_Parentesco as Par with (nolock) on Pac.idPaciente= Par.idPaciente
	where idEstado=2
	and Par.numeroDocumento=@numeroDocumento
	and convert(varchar(10),pac.fechaNacimiento,112)=@fechaNacimiento
--	and pac.apellido =@apellido
	and replace (pac.apellido,' ','')=replace (@apellido,' ','')
if UPPER(@estado)='VALIDADO'
	insert into #pacientes
	select  idPaciente
	from Sys_Paciente  as Pac 
	where   idEstado<>2 
	and Pac.numeroDocumento= @numeroDocumento
 	and convert(varchar(10),pac.fechaNacimiento,112)=@fechaNacimiento
	--and pac.apellido=@apellido
	and replace (pac.apellido,' ','')=replace (@apellido,' ','')
	 
	  

	  --select * from #pacientes
	  --select distinct embarazada from lab_protocolo
;with Protocolos as (
select P.idProtocolo,
Pac.numeroDocumento as documento,
Pac.apellido,  Pac.nombre,
substring(Se.nombre,1,1)+ substring(Pac.nombre,1,2)+substring(Pac.apellido,1,2)+ convert(varchar(10),pac.fechaNacimiento,112)+
case when P.embarazada='S' then 'E' else '' end as codigoHIV,
Pac.fechanacimiento, Se.nombre as sexobiologico, 
P.numero, P.fecha, e.nombre as Laboratorio,
P.Especialista as medicoSolicitante, E1.nombre as efectorSolicitante, O.nombre as origen  , M.nombre as tipoMuestra
from Sys_Paciente  as Pac with (nolock)
inner join LAB_Protocolo as P with (nolock)
	on Pac.idPaciente= P.idPaciente
inner join Sys_Efector   as E with (nolock)
	on E.idEfector= P.idEfector
inner join LAB_Origen  as O with (nolock)
	on O.idOrigen= P.idOrigen
inner join sys_efector as E1 with (nolock)
	on E1.idEfector = P.idEfectorSolicitante
inner join #pacientes as pa 
	on pa.idPaciente= Pac.idPaciente
inner join Sys_Sexo as Se with (nolock)
	on Se.idSexo= Pac.idSexo
left join LAB_Muestra as M with (nolock)
	on M.idMuestra= P.idMuestra
where P.baja=0
and convert(varchar(10),P.fecha,112)  between @fechaDesde and @fechaHasta
and exists (select 1 from lab_detalleprotocolo Det with (nolock)
			where Det.idprotocolo= P.idProtocolo 
			and (idusuariovalida>0 or idUsuarioObservacion>0)
			and conResultado=1			)
)
 
select *
from  Protocolos 
--FOR JSON PATH , ROOT('PROTOCOLOS')
 
  
end 


 