
 ---Depuracion de datos.
 delete from LAB_Impresora where nombre like 'Microsoft Print to PDF'
 delete from  lab_ProtocoloEtiqueta

-- select * from lab_auditoriaprotocolo where idUsuario=0
GO
 


 
---Correccion para que imprima determinaciones incluidas en otras
ALTER VIEW [dbo].[vta_LAB_GeneraCodigoBarras]
AS
SELECT DISTINCT 
                      P.idProtocolo, I.idArea,
					case when I.imprimeMuestra= 1 then convert(varchar,numero)+isnull(M.codigo,'') 
					else convert(varchar,numero) end AS numeroP, P.idEfectorSolicitante, P.idOrigen, P.idPrioridad, P.idSector, 
					P.idTipoServicio, 
                      CASE WHEN I.etiquetaAdicional = 1 
						THEN substring (I.nombre,0,20) 
						ELSE 
							CASE WHEN I2.etiquetaAdicional = 1 
							THEN substring (I2.nombre,0,20) 
							ELSE 
						substring(A.nombre,0,20)
						 END
						end AS area,
						
						P.fecha, O.nombre AS Origen, SS.prefijo AS Sector, P.numeroOrigen, P.numeroOrigen2,
                      0 as numeroDocumento, I.codificaHiv as apellido , P.sexo, CONVERT(varchar, P.edad) 
                      + '' + CASE P.unidadEdad WHEN 0 THEN 'a' WHEN 1 THEN 'm' WHEN 2 THEN 'd' END AS edad,
					  --DP.conResultado, 
					  P.numero, P.numeroDiario, P.numeroTipoServicio, 
                      P.numeroSector, case when I.codificaHiv=0 then 'FALSE' ELSE 'TRUE' end AS pacientecodificado 
					  
FROM         dbo.LAB_Protocolo AS P INNER JOIN
                      dbo.LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo and P.idEfector = DP.idEfector 
					  INNER JOIN    dbo.LAB_Item AS I ON DP.idItem = I.idItem 
					  INNER JOIN    dbo.LAB_Item AS I2 ON DP.idsubItem = I2.idItem 
					  Left JOIN
					  lab_muestra as M on M.idMuestra= I.idMuestra inner join
                      dbo.LAB_Area AS A ON I.idArea = A.idArea INNER JOIN
                      dbo.LAB_Origen AS O ON P.idOrigen = O.idOrigen INNER JOIN
                      dbo.LAB_SectorServicio AS SS ON P.idSector = SS.idSectorServicio 
Where P.baja=0 and DP.trajoMuestra='Si'

go
go
/****** Object:  StoredProcedure [dbo].[LAB_GetGeneraEtiqueta]    Script Date: 14/03/2025 17:55:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*

PROCEDIMIENTO LLAMADO DESDE EL PROGRAMA LOCAL DESDE EL EFECTOR PARA GENERAR EL DISEÑO DE LA ETIQUETA  DE SU PROTOCOLO CARGADO Y ENVIARLO A SU IMPREOSRA LOCAL
Este procedimiento toma como dato de entrada la etiqueta que se ingresó en LAB_ProtocoloEtiqueta desde la carga o edicion de protocolo
Deja como salida datos para la etiqueta impresa en lab_ProtocoloEtiquetaImpresa para ser consumida por el cliente impresor.


10-09-2024. Se agrega codigo de efector de letras en la impresion
07-03-2025. Se corrige error cuando se ingresa caracteres no numericos en numero de identificacion adicional
07-03-2025. Se corrige para que se imprima numero de origen
18-03-2025. Se habilita para que se impriman muestras del modulo no pacientes

------datos de prueba------------
select top 10 * from sys_paciente order by idpaciente desc
update sys_paciente set numeroadic='15263-785A' where idpaciente=51048

select * from lab_Protocolo where idefector= 55 and idprotocolo=100125625
select * from lab_item where iditem in (
select * from lab_detalleprotocolo where idprotocolo=100003995)

update lab_detalleprotocolo
set trajomuestra='No'
where --idsubitem in (2235,2274,2285)
 idprotocolo=100003995

select * from lab_ProtocoloEtiqueta
idProtocoloEtiqueta idProtocolo idArea      idItem      impresora                                                                                                                                              fechaRegistro           idEFector
------------------- ----------- ----------- ----------- ------------------------------------------------------------------------------------------------------------------------------------------------------ ----------------------- -----------
23693               100048511   0           0           Microsoft Print to PDF                                                                                                                                 2024-01-01 19:25:40.953 185

185|Microsoft Print to PDF
desde API se invoca 185|5465465
select *  
--delete
from LAB_ProtocoloEtiqueta
where idefector=55


insert into lab_ProtocoloEtiqueta
values --(100141787,-1,0,'Microsoft Print to PDF',getdate(),55),
-- (100141787,11,0,'Microsoft Print to PDF',getdate(),55)
 (100141794,-1,0,'Microsoft Print to PDF',getdate(),55),---no paciente
 (100141794,19,0,'Microsoft Print to PDF',getdate(),55)---no paciente
-- (100082667,45,0,'Microsoft Print to PDF',getdate(),6)

exec LAB_GetGeneraEtiqueta 55,'Microsoft Print to PDF'
go
select * 
--delete
from LAB_ProtocoloEtiquetaimpresa



------fin datos de prueba------------
*/
 
ALTER PROCEDURE [dbo].[LAB_GetGeneraEtiqueta]
@idEfector int,
@impresora varchar(50) 

AS
BEGIN
	SET NOCOUNT ON;
 
declare @idprotocolo int,
	@idArea int,
	@idPaciente int, 
	@idTipoServicio int, 
	@ssql varchar(1000), 
	@codigoEfector varchar(5),
	@idItem int,
	@imprimeProtocoloFecha  bit,
              @imprimeProtocoloOrigen bit,
              @imprimeProtocoloSector bit,
              @imprimeProtocoloNumeroOrigen bit,
              @imprimePacienteNumeroDocumento bit,
              @imprimePacienteApellido bit,
              @imprimePacienteSexo bit,
              @imprimePacienteEdad bit, 
			  @tipoEtiqueta varchar(100), 
			  @FuenteBarCode varchar(100)

			
DELETE FROM lab_ProtocoloEtiquetaImpresa WHERE IDEFECTOR=@idEfector
---
select @tipoEtiqueta=tipoEtiqueta	
from lab_Configuracion with (nolock)
where  idEfector=@idefector
---
select @codigoEfector= case when len(idEfector2)>1 then idEfector2 else '' end 
from Sys_Efector with (nolock)
where	    idEfector=@idefector 

CREATE TABLE #etiquetas	(	 
	[idProtocolo] [int] NOT NULL,
	[idArea] [int] NOT NULL,
	[idItem] [int] NOT NULL,
	[impresora] [varchar](150) NOT NULL,
	[fechaRegistro] [datetime] NULL,
	[idEFector] [int] NULL)

	/*trabaja con las etiquetas generadas desde el protoclo en la tabla LAB_ProtocoloEtiqueta*/
	select @ssql=N'insert into #etiquetas	
	select  idProtocolo,idArea,idItem,	impresora,fechaRegistro,idEFector 	
	from LAB_ProtocoloEtiqueta 	
	where idEfector='+ convert(varchar,@idEfector)+
	' and impresora like ''%'+@impresora+'%'' order by idprotocolo, idarea'

--	print @ssql
	exec (@ssql)
	--select * from #etiquetas
	/*Recorrer la temp #etiquetas*/
	while (select count(*) from #etiquetas )>0

	begin
		select top 1 @idprotocolo=idprotocolo, @idArea=idArea, @idItem=idItem from #etiquetas 

		select   @idPaciente=idPaciente, @idTipoServicio=idTipoServicio
		from lab_protocolo where idprotocolo=@idprotocolo
		if @idTipoServicio=5 
			select @idTipoServicio=3

			 select top 1  @imprimeProtocoloFecha = protocoloFecha,
             @imprimeProtocoloOrigen = protocoloorigen,
             @imprimeProtocoloSector =protocoloSector,
             @imprimeProtocoloNumeroOrigen = protocolonumeroorigen,
             @imprimePacienteNumeroDocumento =pacientenumerodocumento,
             @imprimePacienteApellido = pacienteapellido,
             @imprimePacienteSexo = pacientesexo,
             @imprimePacienteEdad = pacienteedad, 
			 @FuenteBarCode=fuente
			--select top 1* 
			 from lab_ConfiguracionCodigoBarra	with (nolock)
			 where idtiposervicio=@idtiposervicio and idEfector=@idefector
			  
		/*
		si idPacioente=-1 ==> no paciente
			 si idPaciente>0 ==> paciente
			
		*/
		--if @idPaciente>0
		--begin

			if @idArea>0  --Imprime etiqueta por area
				insert into lab_ProtocoloEtiquetaImpresa  (idProtocolo,	idArea,	numeroP,area,fecha,Origen,Sector,NumeroOrigen,
		NumeroDocumento,apellido,Sexo,edad,pacientecodificado,NumeroOrigen2, idEfector)
				SELECT  distinct [idProtocolo]      ,[idArea]      ,CONVERT(VARCHAR,[numeroP])      ,[area]
				,CONVERT (VARCHAR(10),[Fecha],103)      ,[Origen]      ,[Sector]      ,[NumeroOrigen]      ,
				[NumeroDocumento]      ,[apellido]      ,
				[Sexo]      ,isnull([edad],''),pacientecodificado,
				[NumeroOrigen2]  , @idEfector
				--select *
				FROM vta_LAB_GeneraCodigoBarras       A		 
			--	WHERE  				idProtocolo = 100048537            
				WHERE   idArea =@idArea AND 
				idProtocolo = @idprotocolo            
		
			else
			       
			--		if @idArea=-1 ---adicional            
                 	insert into lab_ProtocoloEtiquetaImpresa  (idProtocolo,	idArea,	numeroP,area,fecha,Origen,Sector,NumeroOrigen,
	NumeroDocumento,apellido,Sexo,edad,pacientecodificado,NumeroOrigen2,idEfector)
              SELECT  [idProtocolo]      ,[idArea]      ,CONVERT(VARCHAR,[numeroP])      ,[area]
            ,CONVERT (VARCHAR(10),[Fecha],103)      ,[Origen]      ,[Sector]      ,[NumeroOrigen]      ,
			[NumeroDocumento]      ,[apellido]      ,[Sexo]      ,isnull([edad],'') ,pacientecodificado,
            [NumeroOrigen2] , @idEfector
				
                FROM vta_LAB_GeneraCodigoBarrasGeneral                    
                WHERE    idProtocolo =@idprotocolo--100048537--


		if @idPaciente>0
		begin	
			
			update lab_ProtocoloEtiquetaImpresa
			set apellido = @codigoEfector+ ' - ' +
			case when Pei.pacientecodificado<>'TRUE'
				then  Pac.apellido + ' ' + Pac.nombre
			else
				substring (Pac.apellido,1,2) + ' ' + substring (Pac.nombre,1,2)
			end,
				NumeroDocumento = case when Pac.idEstado<>2 then convert(varchar,Pac.numeroDocumento) else isnull(Pac.numeroAdic,'') end 
			from lab_ProtocoloEtiquetaImpresa Pei
			inner join LAB_Protocolo P with (nolock) on Pei.idProtocolo= P.idProtocolo
			inner join sys_paciente Pac with (nolock)  on P.idPaciente= Pac.idPaciente

		end  
		else
		begin --no paciente

			update lab_ProtocoloEtiquetaImpresa
			set apellido = @codigoEfector+ ' - ' + substring ( P.descripcionProducto,1,20) 
			, NumeroDocumento = ''--isnull( Pei.numeroorigen ,Pei.numerop)  ---		 
			from lab_ProtocoloEtiquetaImpresa Pei
			inner join LAB_Protocolo P with (nolock) on Pei.idProtocolo= P.idProtocolo
		 
		end
		update lab_ProtocoloEtiquetaImpresa
		set  [Fecha]=case when @imprimeProtocoloFecha=0 then '' else [Fecha] end,
		Origen=case when @imprimeProtocoloOrigen=0 then '' else Origen end,
		Sector=case when @imprimeProtocoloOrigen=0 then '' else Sector end,
		NumeroOrigen=case when @imprimeProtocoloNumeroOrigen=0 then '' else NumeroOrigen end,
		NumeroDocumento=case when @imprimePacienteNumeroDocumento=0 then '' else NumeroDocumento end,
		Apellido=case when @imprimePacienteApellido=0 then @codigoEfector else Apellido end,
		Sexo=case when @imprimePacienteSexo=0 then '' else Sexo end,
		edad=case when @imprimePacienteEdad=0 then '' else Edad end,				
			tipoEtiqueta=@tipoEtiqueta,  FuenteBarCode=@FuenteBarCode
		
		
	 
	
		delete from #etiquetas where idprotocolo=@idprotocolo and  idArea=@idArea and idItem=@idItem
		---marcar como impreso==> saca de la lista de pendientes de imprimir
		delete from LAB_ProtocoloEtiqueta where  idProtocolo= @idprotocolo and idArea=@idArea and idItem=@idItem
	end
drop table #etiquetas
	  
--	 select * from lab_ProtocoloEtiquetaImpresa where idEfector=@idEfector
END
	

	 
	  


