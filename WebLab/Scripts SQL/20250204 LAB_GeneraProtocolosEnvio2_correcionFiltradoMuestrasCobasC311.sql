/* Control de cambios
- Cobas: corrección filtrado por tipo de muestra.
- Búsqueda de Pacientes por filtro ampliado. Corrección de error.
- Desvalidacion de resultados. Corrección de error.

*/



alter PROCEDURE [dbo].[LAB_GeneraProtocolosEnvio2]
@Equipo varchar(50),
@Param varchar(500),	
@TipoMuestra varchar(50),
@Prefijo varchar(10),
@estado varchar(2)
as

--exec [LAB_GeneraProtocolosEnvio2] 'CobasC311',' P.baja=0 and P.idEfector=2 and P.Fecha>=''20250128'' AND P.fecha<=''20250131'' AND P.idSector in (38,68,48)','Orina','', 0
/*
13-12-2023. Agrego filtro de que no se envien determinaciones sin muestra
04.11.2023. Mejora de numero de protocolo
31.01.2025. Mejora en filro de tipo de muestra de cobasc311
*/
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--exec [LAB_GeneraProtocolosEnvio2] 'Metrolab', ' P.Fecha>=''20121213'' AND P.fecha<=''20121214'' AND P.idSector in (4,10,14,5,6,3,13,2,15,18,9,8,7,17,16,11,12,19)  ','Sangre', '','0'
declare @ssql varchar(max)

declare @condicion varchar (max)
declare @tabla varchar(max)


set @condicion= @Param + ' and DP.trajoMuestra=''Si'''


if @Equipo='Mindray'
	begin
		set @condicion= @Param --+ ' and MI.tipoMuestra='''+@TipoMuestra +''''


		if @estado<>'2'
			begin
				set @ssql= 'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo,	' + @estado + ' as estado
				FROM         LAB_Protocolo AS P  with (nolock) 
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP with (nolock)
				INNER JOIN   LAB_MindrayItem AS MI with (nolock) ON DP.idSubItem = MI.idItem 
				where MI.habilitado=1 and  MI.prefijo= ''' + @Prefijo + '''  and MI.tipoMuestra='''+ @TipoMuestra+''' 
				and DP.enviado='+ @estado +') and ' + @condicion + ' ORDER BY numero '
			end
		else
			begin
				set @ssql= 'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then  '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo, ''0'' as estado
				FROM         LAB_Protocolo AS P  with (nolock)
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP with (nolock)
				INNER JOIN   LAB_MindrayItem AS MI ON DP.idSubItem = MI.idItem 
				where MI.habilitado=1 and  MI.prefijo= ''' + @Prefijo + '''  and MI.tipoMuestra='''+ @TipoMuestra+''' 
				and DP.enviado=0) and ' + @condicion +
				'union ' +
				'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then  '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo, ''1'' as estado
				FROM         LAB_Protocolo AS P  with (nolock)
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP with (nolock)
				INNER JOIN   LAB_MindrayItem AS MI with (nolock) ON DP.idSubItem = MI.idItem  
				where MI.habilitado=1 and  MI.prefijo= ''' + @Prefijo + '''  and MI.tipoMuestra='''+ @TipoMuestra+''' 
				and DP.enviado=1) and ' + @condicion + ' ORDER BY numero '	
			end		   
	end --- fin de mindray


if @Equipo='Metrolab' OR @Equipo='Miura'  OR @Equipo='Incca' 
begin
			set @condicion= @Param --+ ' and MI.tipoMuestra='''+@TipoMuestra +''''
		--set @condicion =@condicion+  ' and MI.habilitado=1 and MI.Prefijo='''+@Prefijo +''''
		
		if @TipoMuestra<>'0'		
			set @condicion =@condicion+  ' and  I.idMuestra='+@TipoMuestra 

		if @Equipo='Metrolab' set @tabla='LAB_MetrolabItem' 	
		if @Equipo='Miura' set @tabla='LAB_MiuraItem' 	
		if @Equipo='Incca' set @tabla='LAB_InccaItem' 	
		 IF @Equipo = 'CobasC311'
            SET @tabla = 'LAB_CobasC311';
		if @estado<>'2'
			begin
				--set @ssql= 'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				--case when Pac.idEstado=2 then  '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				--convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				--P.sexo as sexo,	' + @estado + ' as estado
				--FROM         LAB_Protocolo AS P  
				--inner join LAB_Origen AS O on O.idOrigen= P.idOrigen 
				--INNER JOIN   Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente  				
				--WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				--(SELECT   DP.idProtocolo
				--FROM         LAB_DetalleProtocolo AS DP 
				--INNER JOIN   '+@tabla+' AS MI ON DP.idSubItem = MI.idItem 
				--where MI.habilitado=1 and  MI.prefijo= ''' + @Prefijo + ''' and DP.enviado='+ @estado +') 
				--and ' + @condicion + ' ORDER BY numero '
				if @estado='99'---filtra las deterinaciones que no tienen resultados: no controla estado del protocolo ni si la determinacion fue enviada ono
				
				
				set @ssql=';with protocoloCT as (SELECT  
				 distinct P.idProtocolo
							FROM         LAB_Protocolo AS P with (nolock)
							inner join LAB_DetalleProtocolo AS DP with (nolock) on p.idprotocolo= DP.idProtocolo
							INNER JOIN   '+@tabla+' AS MI with (nolock) ON DP.idSubItem = MI.idItem 
							inner join lab_item as I with (nolock) on i.idItem= Mi.idItem
							where MI.habilitado=1 and  MI.prefijo=''' + @Prefijo + ''' and DP.conResultado=0 and  ' + @condicion + ' 
			)

			SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
							case when Pac.idEstado=2 then  '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
							convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
							P.sexo as sexo,	P.estado as estado
							FROM         LAB_Protocolo AS P  with (nolock)
							inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
							INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  		
							inner join protocoloCT as CT  on Ct.idprotocolo= P.idProtocolo '
				else
				set @ssql=';with protocoloCT as (SELECT  
     distinct P.idProtocolo
				FROM         LAB_Protocolo AS P with (nolock)
				inner join LAB_DetalleProtocolo AS DP with (nolock) on p.idprotocolo= DP.idProtocolo
				INNER JOIN   '+@tabla+' AS MI with (nolock) ON DP.idSubItem = MI.idItem 
				inner join lab_item as I with (nolock) on i.idItem= Mi.idItem
				where MI.habilitado=1 and  MI.prefijo=''' + @Prefijo + ''' and DP.enviado='+ @estado +'
				and  ' + @condicion + ' 
)

SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then  '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo,	' + @estado + ' as estado
				FROM         LAB_Protocolo AS P  with (nolock)
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  		
				inner join protocoloCT as CT with (nolock) on Ct.idprotocolo= P.idProtocolo '
			end
		else
			begin
				set @ssql= 'SELECT  P.idProtocolo,P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then  '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo, ''0'' as estado
				FROM         LAB_Protocolo AS P  with (nolock)
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP with (nolock)
				INNER JOIN   '+@tabla+' AS MI ON DP.idSubItem = MI.idItem 
				where MI.habilitado=1 and  MI.prefijo= ''' + @Prefijo + ''' and DP.enviado in (0)) and ' + @condicion +
				'union ' +
				'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then  '''' else Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo, ''1'' as estado
				FROM         LAB_Protocolo AS P  with (nolock)
				inner join LAB_Origen AS O on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP 
				INNER JOIN   '+@tabla+' AS MI ON DP.idSubItem = MI.idItem  
				where MI.habilitado=1 and  MI.prefijo= ''' + @Prefijo + ''' and DP.enviado=1) and ' + @condicion + ' ORDER BY numero '	

			
			end		
end
	



if @Equipo='SysmexXS1000' set @tabla='LAB_SysmexItem'
if @Equipo='SysmexXT1800' set @tabla='LAB_SysmexItemXT1800'	
if @Equipo='CobasC311' set @tabla='LAB_CobasC311'
if @Equipo='Stago' set @tabla='LAB_STACompactItem' 	

set @condicion= @Param --+ '   and MI.habilitado=1 '
IF @Equipo='CobasC311'
BEGIN
	declare @condicion2 varchar(200)=''
	if @TipoMuestra<>'0'		
			set @condicion2 =@condicion2+  ' and  MI.tipoMuestra='''+@TipoMuestra + ''''

	if @estado<>'2'
			BEGIN
            
				set @ssql= 'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then '''' else   Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo,' + @estado + ' as estado
				FROM LAB_Protocolo AS P  with (nolock)
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP with (nolock) 
				INNER JOIN   '+ @tabla +' AS MI ON DP.idSubItem = MI.idItemSil ' +@condicion2+' 
				where MI.habilitado=1 --and  MI.prefijo= ''' + @Prefijo + ''' 
				and DP.enviado='+ @estado +') and ' + @condicion+ ' order by numero '
			END
            ELSE
            BEGIN
				set @ssql= 'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then '''' else   Pac.numerodocumento end as numeroDocumento,
				Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo, ''0'' as estado
				FROM         LAB_Protocolo AS P  with (nolock) 
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP  with (nolock)
				INNER JOIN   '+@tabla+' AS MI ON DP.idSubItem = MI.idItemSil' +@condicion2+' 
				where MI.habilitado=1 
				--and  MI.prefijo= ''' + @Prefijo + ''' 
				and DP.enviado=0) and ' + @condicion +
				'union ' +
				'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then '''' else   Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo, ''1'' as estado
				FROM         LAB_Protocolo AS P  with (nolock)
				inner join LAB_Origen AS O with (nolock) on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac with (nolock) ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP with (nolock)
				INNER JOIN   '+@tabla+' AS MI ON DP.idSubItem = MI.idItemSil ' +@condicion2+' 
				where MI.habilitado=1 
				--and  MI.prefijo= ''' + @Prefijo + ''' 
				and DP.enviado=1) and ' + @condicion + ' ORDER BY numero '	
            END
            
END

if @Equipo<>'Mindray' and @Equipo<>'Metrolab' and @Equipo<>'Miura' and @Equipo<>'CobasC311' and @Equipo<>'Incca'
begin
	if @estado<>'2'
			begin
				set @ssql= 'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2 then '''' else   Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo,' + @estado + ' as estado
				FROM         LAB_Protocolo AS P  
				inner join LAB_Origen AS O on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP 
				INNER JOIN   '+ @tabla +' AS MI ON DP.idSubItem = MI.idItem  
				where MI.habilitado=1  and DP.enviado='+ @estado +') and ' + @condicion+
				' order by numero '
			end
	else
			begin
				set @ssql= 'SELECT  P.idProtocolo, P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2  then '''' else   Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo,0 as estado
				FROM         LAB_Protocolo AS P  
				inner join LAB_Origen AS O on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP 
				INNER JOIN   '+ @tabla +' AS MI ON DP.idSubItem = MI.idItem 
				where MI.habilitado=1  and DP.enviado=0) and ' + @condicion + '
				union
				SELECT  P.idProtocolo,P.numero AS numero, convert(varchar(10),P.fecha,103) as fecha, 
				case when Pac.idEstado=2  '''' else   Pac.numerodocumento end as numeroDocumento, Pac.apellido + '' '' + Pac.nombre AS Paciente   , O.nombre as Origen,
				convert (varchar (10),P.edad) + case P.unidadedad when 0 then '' años '' when 1 then '' meses '' when 2 then '' días '' end as edad,
				P.sexo as sexo,1 as estado
				FROM         LAB_Protocolo AS P  
				inner join LAB_Origen AS O on O.idOrigen= P.idOrigen 
				INNER JOIN   Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente  				
				WHERE  P.estado<2 and P.baja = 0 and P.idProtocolo in 
				(SELECT   DP.idProtocolo
				FROM         LAB_DetalleProtocolo AS DP 
				INNER JOIN   '+ @tabla+' AS MI ON DP.idSubItem = MI.idItem 
				where MI.habilitado=1  and DP.enviado=1) and ' + @condicion+
				' order by numero '
			end		
	  
end


print (@ssql)
exec (@ssql)


END



 
  