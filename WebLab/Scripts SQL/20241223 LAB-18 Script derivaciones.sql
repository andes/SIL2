if (select count(*) from sys.all_objects o
	inner join sys.all_columns c on o.object_id= c.object_id
	where o.name = 'LAB_Derivacion' and c.name = 'idLote') = 0
begin 
	/* Se agrega columna 'idlote' en LAB_Derivacion */
	alter table LAB_Derivacion add idLote int
	end
GO
/******************************** ESTADOS DE DERIVACIONES  *****************************************************/
if( select count(*) from sys.all_objects o where o.name = 'LAB_DerivacionEstado') = 0
BEGIN
	CREATE TABLE [dbo].[LAB_DerivacionEstado](
	[idEstado] [int] IDENTITY(0,1) NOT NULL,
	[descripcion] varchar(max) NOT NULL,
	[baja] [bit] NOT NULL,
	CONSTRAINT [PK_LAB_DerivacionEstado] PRIMARY KEY CLUSTERED
	(
	[idEstado] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[LAB_DerivacionEstado] ADD CONSTRAINT [DF_LAB_DerivacionEstado_baja] DEFAULT ((0)) FOR [baja]

	INSERT INTO LAB_DerivacionEstado (descripcion) VALUES('Pendiente de derivar')
	INSERT INTO LAB_DerivacionEstado (descripcion) VALUES('Enviado')
	INSERT INTO LAB_DerivacionEstado (descripcion) VALUES('No Enviado')
	INSERT INTO LAB_DerivacionEstado (descripcion) VALUES('Recibido')
	INSERT INTO LAB_DerivacionEstado (descripcion) VALUES('Pendiente para enviar')
end
GO
/******************************** LOTES PARA LAS DERIVACIONES *****************************************************/
if( select count(*) from sys.all_objects o where o.name = 'LAB_LoteDerivacion') = 0
BEGIN
	CREATE TABLE [dbo].[LAB_LoteDerivacion](
	[idLoteDerivacion] [int] IDENTITY(1,1) NOT NULL,
	[idEfectorOrigen] [int] NOT NULL,
	[idEfectorDestino] [int] NOT NULL,
	[estado] [int] NOT NULL,
	[fechaRegistro] [datetime] NOT NULL,
	[fechaEnvio] [datetime] NOT NULL,
	[fechaIngreso] [datetime] NOT NULL,
	[baja] [bit] NOT NULL,
	[idUsuarioRegistro] [int] NULL,
	[idUsuarioEnvio] [int] NULL,
	[idUsuarioRecepcion] [int] NULL,
	[observacion] varchar(max) NULL,
	CONSTRAINT [PK_LAB_Lote] PRIMARY KEY CLUSTERED
	(
	[idLoteDerivacion] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] 
end

/******************************** ESTADOS DE DERIVACIONES DE LOTE *****************************************************/
if( select count(*) from sys.all_objects o where o.name = 'LAB_LoteDerivacionEstado') = 0
BEGIN
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[LAB_LoteDerivacionEstado](
	[idEstado] [int] IDENTITY(1,1) NOT NULL,
	[nombre] varchar(max) NOT NULL,
	[baja] [bit] NULL,
	CONSTRAINT [PK_LAB_LoteDerivacionEstado] PRIMARY KEY CLUSTERED
	(
	[idEstado] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] 
	ALTER TABLE [dbo].[LAB_LoteDerivacionEstado] ADD CONSTRAINT [DF_LAB_LoteDerivacionEstado_baja] DEFAULT ((0)) FOR [baja]
	ALTER TABLE [dbo].[LAB_LoteDerivacion] WITH CHECK ADD
	CONSTRAINT [FK_LAB_LoteDerivacion_LAB_LoteDerivacionEstado] FOREIGN KEY([estado])
	REFERENCES [dbo].[LAB_LoteDerivacionEstado] ([idEstado])
	ALTER TABLE [dbo].[LAB_LoteDerivacion] CHECK CONSTRAINT [FK_LAB_LoteDerivacion_LAB_LoteDerivacionEstado]


	INSERT INTO LAB_LoteDerivacionEstado (nombre)
	VALUES
	('Creado'),
	('Derivado'),
	('Cancelado'),
	('Recibido'),
	('Ingresado'),
	('Completado')
END

/******************************** AUDITORIA DE LOTE *****************************************************/
if( select count(*) from sys.all_objects o 
where o.name = 'LAB_AuditoriaLote') = 0
BEGIN
	SET ANSI_NULLS OFF
	SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[LAB_AuditoriaLote](
	[idAuditoriaLote] [int] IDENTITY(1,1) NOT NULL,
	[idLote] [int] NOT NULL,
	[fecha] [datetime] NOT NULL,
	[hora] nvarchar(max) NOT NULL,
	[accion] nvarchar(max) NOT NULL,
	[analisis] nvarchar(max) NOT NULL,
	[valor] nvarchar(max) NOT NULL,
	[valorAnterior] nvarchar(max) NOT NULL,
	[idUsuario] [int] NOT NULL,
	PRIMARY KEY CLUSTERED
	(
	[idAuditoriaLote] ASC,
	[idLote] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[LAB_AuditoriaLote] ADD CONSTRAINT [DF_LAB_AuditoriaLote_fecha] DEFAULT (((1)/(1))/(1900)) FOR [fecha]
	ALTER TABLE [dbo].[LAB_AuditoriaLote] ADD CONSTRAINT [DF_LAB_AuditoriaLote_hora] DEFAULT ('') FOR [hora]
	ALTER TABLE [dbo].[LAB_AuditoriaLote] ADD CONSTRAINT [DF_LAB_AuditoriaLote_accion] DEFAULT ('') FOR [accion]
	ALTER TABLE [dbo].[LAB_AuditoriaLote] ADD CONSTRAINT [DF_LAB_AuditoriaLote_analisis] DEFAULT ('') FOR [analisis]
	ALTER TABLE [dbo].[LAB_AuditoriaLote] ADD CONSTRAINT [DF_LAB_AuditoriaLote_valor] DEFAULT ('') FOR [valor]
	ALTER TABLE [dbo].[LAB_AuditoriaLote] ADD CONSTRAINT [DF_LAB_AuditoriaLote_valorAnterior] DEFAULT ('') FOR [valorAnterior]
	ALTER TABLE [dbo].[LAB_AuditoriaLote] ADD CONSTRAINT [DF_LAB_AuditoriaLote_username] DEFAULT ((0)) FOR [idUsuario]
END

ALTER VIEW vta_LAB_Derivaciones
AS
SELECT DISTINCT
TOP (100) PERCENT P.edad, P.unidadEdad, P.sexo, P.numero, P.fecha, P.idEspecialistaSolicitante AS especialista, CASE WHEN Pac.idPaciente = - 1 THEN 0 ELSE CASE WHEN Pac.idestado = 2 THEN '' ELSE CONVERT(varchar,
Pac.numeroDocumento) END END AS dni, CASE WHEN Pac.idPaciente = - 1 THEN P.descripcionProducto ELSE Pac.apellido END AS apellido, Pac.nombre, P.idProtocolo, I.descripcion AS determinacion,
dbo.Sys_Efector.nombre AS efectorDerivacion, P.idOrigen, P.idPrioridad, P.idEfector, P.idTipoServicio, I.idArea, I.idItem, DP.idDetalleProtocolo, CASE WHEN D .estado IS NULL THEN 0 ELSE d .estado END AS estado,
U.apellido + ' ' + SUBSTRING(U.nombre, 0, 5) AS username, D.observacion, D.resultado, D.idUsuarioResultado, D.fechaResultado, D.estado AS estadoDerivacion, P.Especialista AS solicitante, CONVERT(varchar(10),
Pac.fechaNacimiento, 103) AS fechaNacimiento, D.idEfectorDerivacion, D.idLote
FROM dbo.LAB_Protocolo AS P INNER JOIN
dbo.LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo INNER JOIN
dbo.LAB_Derivacion AS D ON D.idDetalleProtocolo = DP.idDetalleProtocolo INNER JOIN
dbo.Sys_Usuario AS U ON U.idUsuario = D.idUsuarioRegistro INNER JOIN
dbo.Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente INNER JOIN
dbo.LAB_Item AS I ON DP.idItem = I.idItem INNER JOIN
dbo.Sys_Efector ON D.idEfectorDerivacion = dbo.Sys_Efector.idEfector
WHERE (P.baja = 0)
GROUP BY P.observacion, Pac.idEstado, Pac.numeroAdic, P.edad, P.sexo, P.idProtocolo, P.fecha, P.idEspecialistaSolicitante, Pac.numeroDocumento, Pac.apellido, Pac.nombre, P.idProtocolo, I.descripcion, dbo.Sys_Efector.nombre,
P.numero, P.numeroDiario, P.idOrigen, P.idPrioridad, D.idEfectorDerivacion, P.idTipoServicio, I.idArea, I.idItem, DP.idDetalleProtocolo, U.apellido, U.nombre, D.observacion, D.estado, P.unidadEdad, D.resultado,
D.idUsuarioResultado, D.fechaResultado, P.Especialista, Pac.fechaNacimiento, P.idEfector, Pac.idPaciente, P.descripcionProducto, P.numeroOrigen, D.idLote
GO