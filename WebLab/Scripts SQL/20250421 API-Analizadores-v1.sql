
--Caro: crear un nueva tabla para que allí quede el dato final a enviar al equipo.
CREATE TABLE [dbo].[LAB_TempProtocoloEquipo](
	[numeroProtocolo] [varchar](50) NOT NULL,
	[tipoMuestra] [varchar](50) NOT NULL,
	[iditem] [varchar](500) NULL,
	[paciente] [varchar](200) NOT NULL,
	[anioNacimiento] [varchar](6) NOT NULL,
	[sexo] [varchar](1) NOT NULL,
	[sectorSolicitante] [varchar](150) NOT NULL,
	[medicoSolicitante] [varchar](150) NOT NULL,
	[urgente] [varchar](50) NOT NULL,
	[idMuestra] [int] NOT NULL,
	[equipo] [varchar](50) NOT NULL,
	[idEfector] [int] not NULL
 )

if (select count(*) from sys.all_objects o
inner join sys.all_columns c on o.object_id= c.object_id
where o.name = 'LAB_TempProtocoloEnvio'
and c.name = 'enviado') = 0
begin 
	/* Se agrega columna 'enviado' en LAB_TempProtocoloEnvio y por defecto que tenga el valor 0 */
	Alter table LAB_TempProtocoloEnvio add enviado bit 
	Alter table LAB_TempProtocoloEnvio add  CONSTRAINT [DF_LAB_TempProtocoloEnvio_enviado]  DEFAULT ((0)) FOR [enviado]
end


if (Select count(*) from WEBAPIProcedimiento where Procedimiento = 'LAB_GetTempProtocoloEnvio' ) = 0
  Insert into WEBAPIProcedimiento values ('LAB_GetTempProtocoloEnvio')

if (Select count(*) from WebAPIConsultas where Nombre = 'GetCantidadTempProtocoloEnvio' ) = 0
Insert into WebAPIConsultas(Nombre, Consulta) 
values ('GetCantidadTempProtocoloEnvio',
'SELECT Count(*) as Cantidad FROM LAB_TempProtocoloEnvio WHERE equipo = @equipo AND idEfector = @idEfector AND isnull(enviado,0) = 0')

GO


-- // Procedimiento //

CREATE PROCEDURE LAB_GetTempProtocoloEnvio (
@analizador varchar(20), --equipo autonalizador
@idEfector int, ---id de efector-laboratorio que envia
@numeroProtocolo int = 0 --- OPCIONAL  numero de protocolo
)
AS
BEGIN

 declare @cantidadregistros int  
 declare @idTempProtocoloEnvio int
 set @cantidadregistros = 0

	SET NOCOUNT ON;
	BEGIN TRY
		---Siempre habrá un unico registro para los filtros enviados para el equipo y efector
		delete from LAB_TempProtocoloEquipo where idEfector=@idEfector and equipo=@analizador
		 
		IF(@numeroProtocolo <> 0) 

		 /* Vane: (19/03/2025) Esta segunda parte la vemos después por lo tanto no sufrio modificaciones */
		 
		 /*Caro: (13/03/2025) Debería traer los datos desde la tablas nucleo ya que si se implementa que el equipo pida 
		 al sil por numero de protocolo la gente del laboratorio no utilizará la opcion de enviar protocolos 
		 desde el sil; ya que la comunicacion sería automatica, y por ende no se grabaran en LAB_TempProtocoloEnvio
		 El query debería ser:			
			insert into LAB_TempProtocoloEquipo
			Select *----datos a definir que va a recibir el autoanalizador
			from lab_protocolo ....
			inner join lab_detalleprotocolo  ....
			inner join lab_cobasC311....

		Otra opcion es invocar desde acá al SP LAB_GeneraProtocolosEnvio2 que llama el SIL para llenar 
		la LAB_TempProtocoloEnvio solo con ese numero de protocolo como parametro.		

		Lo vemos juntas como armar segun el equipo.
		 */
		 
		 SELECT TOP 1 * FROM LAB_TempProtocoloEnvio (nolock)  
		 WHERE equipo = @analizador AND idEfector = @idEfector AND ISNULL(enviado,0) = 0 AND numeroProtocolo = @numeroProtocolo
	ELSE
		
		begin

		/*Caro: (13/03/2025) sugiero que no haya 2 SP (2 api) para invocar desde la interface; porque puede pasar que el equipo
		reciba el dato y luego cuando invoque la segunda API se caiga la conexion o pase algo que haga que no pueda
		ejecutarse; quedando inconsistente la transaccion.

		Entonces acá cambié para que en una tabla final LAB_TempProtocoloEquipo se grabe lo que se envia al equipo
		y dsps se grabar en esa tabla se hace el update de la marca enviado de LAB_TempProtocoloEnvio en este mismo SP.

		Aca va el codigo
		*/
			
			SELECT TOP 1  @idTempProtocoloEnvio=idTempProtocoloEnvio	
			FROM LAB_TempProtocoloEnvio (nolock) 			
			WHERE equipo = @analizador AND idEfector = @idEfector AND ISNULL(enviado,0) = 0 
			
			insert into LAB_TempProtocoloEquipo
			SELECT  numeroProtocolo, tipoMuestra, iditem,paciente, anioNacimiento,
			sexo, sectorSolicitante,medicoSolicitante, urgente, idMuestra,
			equipo, idEfector
			----Caro: especificar los campos
			FROM LAB_TempProtocoloEnvio (nolock) 			
			WHERE idTempProtocoloEnvio = @idTempProtocoloEnvio

			select @cantidadregistros = @@ROWCOUNT				

			IF @cantidadregistros > 0 
			  UPDATE LAB_TempProtocoloEnvio set enviado = 1 where idTempProtocoloEnvio=@idTempProtocoloEnvio
	
		
		end

		---Caro: salida del SP para consumir en la interface.
		---Siempre habrá un unico registro para los filtros enviados para el equipo y efector
		select numeroProtocolo, tipoMuestra, iditem,paciente, anioNacimiento, sexo, sectorSolicitante,medicoSolicitante, urgente, idMuestra, equipo, idEfector from LAB_TempProtocoloEquipo WHERE equipo = @analizador AND idEfector = @idEfector 
	END TRY
	BEGIN CATCH
	/*Se crea mensaje de error para la tabla Temp_Mensaje*/
	DECLARE @mensajeCompleto NVARCHAR(MAX);
	SET @mensajeCompleto = 
		'Error: ' + ERROR_MESSAGE() + 
		' | Procedimiento: ' + ISNULL(ERROR_PROCEDURE(), 'N/A') + 
		' | Línea: ' + CAST(ERROR_LINE() AS VARCHAR) +
		' | Analizador: ' + @analizador ;

		 INSERT INTO Temp_Mensaje (mensaje, fechaRegistro, idEfector)
        VALUES ( @mensajeCompleto, GETDATE() , @idEfector);
    END CATCH
   END

