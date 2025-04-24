/*control de cambios
- Recepción de FNE interoperando con ANDES
- Generación de todas las etiquetas en la carga de protocolos.
*/


/****** Object:  Table [dbo].[LAB_FichaElectronica]    Script Date: 12/12/2024 18:01:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--drop table [LAB_FichaRecepcion]
CREATE TABLE [dbo].[LAB_FichaRecepcion](
	[idFichaRecepcion] [int] IDENTITY(1,1) NOT NULL,
	[idEfector] int NOT NULL,---idEfector que recibe
	fecha date not null,
	idFicha varchar(500), --_id campo de andes
	tipoFicha varchar(100),--- tipo_ficha de andes
	solicitante varchar(200), --usuario de andes
	idEfectorSolicitante int, --OrganizacionNombre de andes
	identificadorlabo varchar(100),-- identificadorlabo de andes
	clasificacion varchar(100),-- clasificacion de andes
	analisis varchar(200),
	fechaSintoma date null,
	idCasoSnvs char(50) null,
	idTipoMuestra int null,
	[fechaRegistro] [datetime] NULL,
	[idusuarioregistro] int,
 CONSTRAINT [PK_LAB_FichaRecepcion] PRIMARY KEY CLUSTERED 
(
	[idFichaRecepcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
) ON [PRIMARY]
GO

/*
select * delete from LAB_FichaRecepcion
where idFichaRecepcion<6
select * from    Rel_andes
select *
--delete
from    Rel_andes
where tipo='Diagnostico'
and nombreAndes='Dengue'


select * from sys_cie10
where Nombre like '%dengue%'

*/
insert into Rel_andes values
('Dengue', null, 'A90',1,'Diagnostico')

insert into Rel_andes values
('Sospechoso', null, 'SOSPECHOSO',1,'Caracter')

insert into Rel_andes values
('', null, 'MEDICINA GENERAL',38,'Sector')

/*
select * from lab_item where codigo in ('4369','4367','4363')

select * from Rel_Andes
where idsil=614
select   * from Rel_andes
*/

if not exists(select 1 from    Rel_andes where tipo='Determinacion' and nombreAndes='Dengue')
insert into Rel_andes   
select 'Dengue', '', codigo,iditem ,  'Determinacion' from lab_item where codigo in ('4369','4367','4363')
go

 ---select * from 
 update Rel_Andes
 set idSIL=70
where idsil=614

---select  * from sys_menu where idMenu=2298
update 
sys_menu
set url='/Protocolos/DefaultFFEE2.aspx?idServicio=3&idUrgencia=0'
where idMenu=2298

/*
menu nuevo de recepcion delaboratorio

objeto: Recepcion desde FFEE
url: /Protocolos/DefaultFFEE2.aspx?idServicio=1&idUrgencia=0
menu superior: Recepcion
orden: 10
perfiles:2,6,7,8 

*/
/*
select *
--delete
from LAB_FichaRecepcion

select * from  LAB_ItemMuestra
select *
--delete
from   LAB_SysmexItemKX21N

select IdSectorUrgencia,* from LAB_Configuracion

select * from Rel_andes A with (nolock)
                   inner join LAB_ItemEfector I with(nolock) on A.idSIL = i.idItem 
                    inner join lab_item I2 on I2.iditem = I.iditem         
                    inner join lab_Area A2 on A2.idarea=I2.idarea
                    where A.tipo ='Determinacion'   and [nombreAndes]='covid19' and I.idEfector=40 and i.disponible=1  and A2.idtipoServicio=3

					select * from LAB_Item
					where codigo in ('9122','9080','9002','9001')

					*/

/*interoperabilidad dengue con sisa*/


select * from lab_item where codigo in ('4369','4367','4363')
/*
idItem      idEfector   codigo                                             nombre                                             descripcion                                                                                                                                                                                                                                                      tipo ordenImpresion requiereMuestra idArea      idEfectorDerivacion idUnidadMedida idCategoria idTipoResultado formatoDecimal valorMinimo                             valorMaximo                             idItemReferencia duracion    disponible baja  idUsuarioRegistro fechaRegistro           codigoNomenclador                                                                                                                                                                                                                                               idResultadoPorDefecto resultadoDefecto                                                                                                                                                                                         codificaHiv multiplicador isScreeening limiteTurnosDia etiquetaAdicional informable requiereCaracter idMuestra   imprimeMuestra
----------- ----------- -------------------------------------------------- -------------------------------------------------- ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ---- -------------- --------------- ----------- ------------------- -------------- ----------- --------------- -------------- --------------------------------------- --------------------------------------- ---------------- ----------- ---------- ----- ----------------- ----------------------- --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- --------------------- -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ----------- ------------- ------------ --------------- ----------------- ---------- ---------------- ----------- --------------
5887        227         4363                                               Dengue, Ac. IgM Anti-                              Dengue, Ac. IgM Anti-                                                                                                                                                                                                                                            P    28             N               45          227                 0              0           3               2              -1.0000                                 -1.0000                                 0                1           1          0     38                2024-01-12 09:19:40.000                                                                                                                                                                                                                                                                 59646                 Negativo                                                                                                                                                                                                 0           1             0            0               0                 1          0                168         0
5944        227         4369                                               Dengue, ARN [PCR]                                  Dengue. ARN                                                                                                                                                                                                                                                      P    0              N               37          227                 0              0           3               0              -1.0000                                 -1.0000                                 0                1           1          0     38                2024-08-05 15:32:07.000                                                                                                                                                                                                                                                                 100529                                                                                                                                                                                                                         0           1             0            0               0                 1          0                216         0
5951        227         4367                                               Dengue, Ag. NS1 [ELISA]                            Dengue, Ag. NS1                                                                                                                                                                                                                                                  P    29             N               45          227                 0              0           3               2              -1.0000                                 -1.0000                                 0                1           1          0     38                2024-08-13 14:11:21.000                                                                                                                                                                                                                                                                 0                                                                                                                                                                                                                              0           1             0            0               0                 1          0                168         0

(3 rows affected)
*/

---Codigo: 4363      Dengue, Ac. IgM Anti-
if not exists (select 1 from LAB_ConfiguracionSISA where idItem=5887)
begin
insert into LAB_ConfiguracionSISA
select top 1 0 AS idCaracter, 109 as idEvento, 'Dengue' as nombreEvento, 104 as idClasificacionManual,
'Caso sospechoso' as nombreClasificacionManual,
107 as idGrupoEvento, 'Sindrome Febril Agudo Inespecífico (SFAI)' as nombreGrupoEvento,
5887 as idIdem,0 as alta, 1 as idMuestra, 3 as idTipoMuestra, 577 as idPrueba,484 as idTipoPrueba,
'20250101' as fechavigenciadesde,'19000101' as fechavigenciahasta,
0 as idOrigen,'N' as soloEmbarazada, 0 as idEfectorSolicitante,
0 as EdadDesde, 999 as EdadHasta
--select *
from LAB_ConfiguracionSISA
end
--select * from LAB_ConfiguracionSISA where idItem=5887

/*select * from LAB_ConfiguracionSISADetalle
select distinct resultado from LAB_ResultadoItem
 where iditem=5887
 */
 if not exists (select 1 from LAB_ConfiguracionSISADetalle where iditem=5887 and idResultadoSISA in (109,110,508))
 begin
 insert into LAB_ConfiguracionSISADetalle
 select 5887 as iditem, 'Positivo',109, 'Positivo' 

  insert into LAB_ConfiguracionSISADetalle
 select 5887 as iditem, 'Indeterminado',110, 'Indeterminado' 

 insert into LAB_ConfiguracionSISADetalle
 select 5887 as iditem, 'Negativo',508, 'Negativo' 
 end
 --select * from LAB_ConfiguracionSISADetalle  where iditem=5887


 /*
  4367      Dengue, Ag. NS1 [ELISA]  iditem=5951
 */

 if not exists (select 1 from LAB_ConfiguracionSISA where idItem=5951)
begin
insert into LAB_ConfiguracionSISA
select top 1 0 AS idCaracter, 109 as idEvento, 'Dengue' as nombreEvento, 104 as idClasificacionManual,
'Caso sospechoso' as nombreClasificacionManual,
107 as idGrupoEvento, 'Sindrome Febril Agudo Inespecífico (SFAI)' as nombreGrupoEvento,
5951 as idIdem,0 as alta, 1 as idMuestra, 3 as idTipoMuestra, 577 as idPrueba,484 as idTipoPrueba,
'20250101' as fechavigenciadesde,'19000101' as fechavigenciahasta,
0 as idOrigen,'N' as soloEmbarazada, 0 as idEfectorSolicitante,
0 as EdadDesde, 999 as EdadHasta
--select *
from LAB_ConfiguracionSISA
end
--select * from LAB_ConfiguracionSISA where idItem=5951



/*select * from LAB_ConfiguracionSISADetalle
select distinct resultado from LAB_ResultadoItem
 where iditem=5951
 */
 if not exists (select 1 from LAB_ConfiguracionSISADetalle where iditem=5951 and idResultadoSISA in (109,110,508))
 begin
 insert into LAB_ConfiguracionSISADetalle
 select 5951 as iditem, 'Positivo',109, 'Positivo' 

  insert into LAB_ConfiguracionSISADetalle
 select 5951 as iditem, 'Indeterminado',110, 'Indeterminado' 

 insert into LAB_ConfiguracionSISADetalle
 select 5951 as iditem, 'Negativo',508, 'Negativo' 
 end
 --select * from LAB_ConfiguracionSISADetalle
 
 --where iditem=5951


 --select nombreAndes, idsil from Rel_andes where tipo='Tipo Muestra'