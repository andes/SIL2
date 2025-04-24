
alter  PROCEDURE [dbo].[LAB_PostDatosEquipo]
@Equipo varchar(20),  --equipo autonalizador
@idEfector int=0, ---id de efector-laboratorio que envia
@numero int, --- numero de protocolo
@Prueba varchar(50),---codigo de determinacion del equipo
@valor varchar(200)--- valor informado

---exec [LAB_PostDatosEquipo]  'MINDRAY', 5, 0, 'hbc', '97'
AS 
/*
PROCEDIMIENTO LLAMADO DESDE LA API PARA INGRESAR DATOS DESDE EL EQUIPO

@Equipo ='SysmexXp300',
@numero =16
@Prueba='RBC'
@valorNum=4.36
@valorTexto=''
SysmexXp300|1542|RBC|4.36|
 
*/
 
BEGIN TRY 
	SET NOCOUNT ON;
	declare @valornum decimal (18,4) 
	Declare @idSubItem int=0, @idProtocolo int=0, @analisis varchar(100)
	declare @idTipoResultado int, @cantidadregistros int
	--registro log del mensaje que llega
	insert into temp_mensaje (mensaje, fecharegistro, idEfector)
	values (@Equipo+';'+convert(varchar,@idEfector)+';'+convert(varchar,@numero)+';'+@prueba+';'+ @valor, 	getdate(),@idEfector)

	if UPPER(@Equipo)='SYSMEX XP300'			
		select top 1 @idSubItem=idItem from LAB_SysmexItemXP300 with (NOLOCK) where habilitado=1 and idSysmex=@Prueba
	if UPPER(@Equipo)='COUNTERXS20' 
		select top 1 @idSubItem=idItem from LAB_CounterItem with (NOLOCK) where habilitado=1 and idCounter=@Prueba
	if UPPER(@Equipo)='COUNTER 19'						
		select top 1 @idSubItem=idItem from LAB_CounterItem with (NOLOCK) where habilitado=1 and idCounter=@Prueba
	if UPPER(@Equipo)='SYSMEX KX-21N'			
		select top 1 @idSubItem=idItem from LAB_SysmexItemKX21N with (NOLOCK) where habilitado=1 and idSysmex=@Prueba
	if UPPER(@Equipo)='MINDRAY'		
		select  top 1 @idSubItem=idItem from LAB_MindrayItem with (nolock)  where habilitado=1 and idmindray=@Prueba
	if UPPER(@Equipo)='SYSMEXXN550'		
		select  top 1 @idSubItem=idItem from LAB_SysmexItem with (nolock)  where habilitado=1 and idSysmex=@Prueba
	if UPPER(@Equipo)='COBASC311'				
		select  top 1 @idSubItem=idItemSIL from LAB_CobasC311 with (nolock)  where habilitado=1 and IDITEMCOBAS=@Prueba
	if UPPER(@Equipo)='COBASB221'		
		select  top 1 @idSubItem=idItem from LAB_CobasB221Item with (nolock)  where habilitado=1 and idCobas=@Prueba
		 
	select top 1 @idProtocolo=idprotocolo 
	from LAB_Protocolo (NOLOCK) 
	where 	idEfector=@idEfector and 
	numero=@numero 
	and baja=0 
	and estado<2		
	
	
	if @idSubItem>0 and @idProtocolo>0
		begin		
				select @cantidadregistros=0
				---ver el tipo de resultado de la determinacion
				select @idTipoResultado=idTipoResultado, @analisis=nombre 
				from lab_item with (NOLOCK) where idItem=@idSubItem	 

				if @idTipoResultado=1 ---numerico
				begin
					select 
						@valornum=						CASE formatodecimal
							WHEN 0 THEN cast(round(cast(@valor AS decimal) ,0) as int)
								WHEN 1 THEN CAST(@valor AS decimal(18, 1)) 
										WHEN 2 THEN CAST(@valor AS decimal(18, 2)) 
														WHEN 3 THEN CAST(@valor AS decimal(18, 3)) 
					   end 
					  from lab_item with (NOLOCK) where idItem=@idSubItem					   					                                            					   					    
				
			 
						Update  LAB_DetalleProtocolo 
						set resultadonum=@valorNum,					
							conresultado=1, 
							idUsuarioResultado=0, 
							enviado=2, 
							fechaResultado= getdate()  
						where	idUsuarioControl=0 
						and idUsuarioValida=0 			
						and idProtocolo=@idProtocolo
						and idSubItem=@idSubItem
						and idEfector=@idEfector

						select @cantidadregistros=@@ROWCOUNT
				end
				else
				begin
						 			   					                                            					   					    
				
			 
						Update  LAB_DetalleProtocolo 
						set resultadoCar=@valor	,			
							conresultado=1, 
							idUsuarioResultado=0, 
							enviado=2, 
							fechaResultado= getdate()  
						where	idUsuarioControl=0 
						and idUsuarioValida=0 			
						and idProtocolo=@idProtocolo
						and idSubItem=@idSubItem
						and idEfector=@idEfector

						select @cantidadregistros=@@ROWCOUNT
				end

				IF @@ROWCOUNT >0 
				begin
					Update  LAB_Protocolo 
					set estado=1  
					where estado=0 
					and idProtocolo=@idProtocolo
					and idEfector=@idEfector

					insert into LAB_AuditoriaProtocolo (idProtocolo,fecha, hora, accion, analisis, valor, valorAnterior, idUsuario)
					values (@idProtocolo, getdate(), right(convert(char(8),getDate(),8),8), 'Automático ' + @Equipo,@analisis, @valor, '', 0)
						 
					select 'OK' as Resultado
				end
				else
					select 'No OK 1' as Resultado
		 end
		 else
			select 'No OK 2' as Resultado	 		  
END TRY  
BEGIN CATCH 


	insert into temp_mensaje (mensaje, fecharegistro, idEfector)
	SELECT     
       ERROR_MESSAGE(),getdate(),@idEfector

	--   select * from temp_mensaje

	/*insert into temp_mensaje (mensaje, fecharegistro, idEfector)
	values ('Error:'+ERROR_MESSAGE() , getdate(),@idEfector)
	*/
END CATCH 

 
  