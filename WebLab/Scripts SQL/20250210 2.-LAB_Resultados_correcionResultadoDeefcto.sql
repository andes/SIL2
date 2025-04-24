
alter PROCEDURE [dbo].[LAB_Resultados]
	
	@idProtocolo int,
	 @FiltroBusqueda nvarchar(max)

/*07.09.2023. Nuevo SP que reemplaza la vista vta_LAB_resultados para optimizar la carga y valdiacion de protocolos por vista de protocolos*/
/*10.02.2025. Correccion de obtencion de resultado por defector. Se obtiene desde lab_item efector*/
AS
BEGIN


declare @ssql varchar(max)
 
SELECT     TOP (100) PERCENT I2.nombre AS grupo, DP.idSubItem AS idItem, I.nombre AS item, DP.trajoMuestra, DP.resultadoCar, DP.resultadoNum, DP.conResultado, 
                      DP.observaciones, DP.idProtocolo, I.idCategoria, I.idTipoResultado, 
                      case when dp.idUsuarioValida >0 then DP.unidadMedida else  UM.nombre end AS UnidadMedida, 
                      CASE WHEN DP.idusuariovalida > 0 or DP.idUsuarioValidaObservacion>0 or D.idusuarioRegistro>0 THEN 2 ELSE 
                        CASE WHEN DP.idUsuarioControl > 0 THEN 1 ELSE 0 END 
                       END AS estado, DP.valorReferencia, DP.metodo, 
                      CASE WHEN uValida.FirmaValidacion = '' THEN uValida.Apellido + ' ' + uValida.Nombre ELSE uValida.FirmaValidacion END AS userValida, 
                      uCarga.apellido + ' ' + uCarga.nombre AS userCarga, I.idArea,
                      
                       case when dp.idUsuarioValida>0 then DP.formatoValida else  I.formatoDecimal end as formatoDecimal,
                      
                       case when dp.idUsuarioValida=0 then 			                       
						CASE I.formatoDecimal WHEN 0 THEN CAST(resultadonum AS int) END 
					   else 
						CASE DP.formatoValida WHEN 0 THEN CAST(resultadonum AS int) END 
					   end
					   AS formato0, 
					   
					   
					   case when dp.idUsuarioValida=0 then 			                       
						  CASE I.formatoDecimal WHEN 1 THEN CAST(resultadonum AS decimal(18, 1)) END
					   else 
						  CASE DP.formatoValida WHEN 1 THEN CAST(resultadonum AS decimal(18, 1)) END 
					   end
					   AS formato1, 					   
					   
                  
                       case when dp.idUsuarioValida=0 then 			                       
						  CASE I.formatoDecimal WHEN 2 THEN CAST(resultadonum AS decimal(18, 2)) END
					   else 
						  CASE DP.formatoValida WHEN 2 THEN CAST(resultadonum AS decimal(18, 2)) END
					   end
					   AS formato2, 	
					   
					   case when dp.idUsuarioValida =0 then 			                       						   
						CASE I.formatoDecimal WHEN 3 THEN CAST(resultadonum AS decimal(18, 3)) END
					   else 
						  CASE DP.formatoValida WHEN  3 THEN CAST(resultadonum AS decimal(18, 3)) END
					   end					  
						AS formato3, 
						
						 case when dp.idUsuarioValida=0 then 
                      CASE I.formatoDecimal WHEN 4 THEN CAST(resultadonum AS decimal(18, 4)) END 
                       else 
						  CASE DP.formatoValida WHEN 4 THEN CAST(resultadonum AS decimal(18, 4)) END
					   end					  
						AS formato4, 
                      
                      --CASE WHEN I.idresultadopordefecto = 0 THEN I.resultadoDefecto ELSE CAST(i.idresultadopordefecto AS varchar) END AS resultadoDefecto, 
					  ie.resultadodefecto AS resultadoDefecto, 
					  DP.idDetalleProtocolo, 
                      uControl.apellido + ' ' + uControl.nombre AS userControl, A.ordenImpresion AS ordenArea, I2.ordenImpresion AS orden, case when I.codificaHiv=0 then I2.codificaHiv else I.codificaHiv end as codificaHiv, I2.codigo, 
                      CASE WHEN DP.idUsuarioValidaObservacion > 0 THEN 2 ELSE 0 END AS estadoObservacion, A.nombre AS area, DP.idItem as idPractica, I.informable as informable
into #protocolo
FROM         dbo.LAB_DetalleProtocolo AS DP INNER JOIN
                      dbo.LAB_Item AS I ON DP.idSubItem = I.idItem INNER JOIN
                      dbo.LAB_Item AS I2 ON DP.idItem = I2.idItem inner join
					  dbo.lab_itemefector as IE on Ie.idefector=DP.idEfector and ie.iditem= DP.idSubItem
					  LEFT OUTER JOIN
                      dbo.Sys_Usuario AS uValida ON DP.idUsuarioValida = uValida.idUsuario LEFT OUTER JOIN
                      dbo.Sys_Usuario AS uCarga ON uCarga.idUsuario = DP.idUsuarioResultado LEFT OUTER JOIN
                      dbo.Sys_Usuario AS uControl ON uControl.idUsuario = DP.idUsuarioControl LEFT OUTER JOIN
                      dbo.LAB_UnidadMedida AS UM ON I.idUnidadMedida = UM.idUnidadMedida INNER JOIN
                      dbo.LAB_Area AS A ON I.idArea = A.idArea LEFT OUTER JOIN
					  LAB_Derivacion as D on D.idDetalleProtocolo= DP.idDetalleProtocolo
                    where DP.idProtocolo= @idprotocolo
ORDER BY I2.codigo
 

select @ssql=N'select grupo,item, iditem, resultadoNum, ResultadoCar, observaciones,idCategoria,
idTipoResultado, UnidadMedida, Estado, Metodo, valorReferencia, '''' as MaximoReferencia, '''' as observacionReferencia ,
userCarga, trajoMuestra ,'''' as tipoValorReferencia, conresultado, 
formatoDecimal,  formato0,  formato1, formato2, formato3,  formato4 , resultadoDefecto, userControl, iddetalleProtocolo, codificaHiv, userValida,estadoObservacion 
from #protocolo
where '+ @FiltroBusqueda
exec (@ssql)

drop table #protocolo

end

 
  