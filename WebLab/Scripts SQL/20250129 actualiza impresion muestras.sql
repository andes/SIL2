/*control de cambios
- Recepción de FNE interoperando con ANDES
- Generación de todas las etiquetas en la carga de protocolos.
*/
/*
select * from lab_item where imprimeMuestra=1
and baja=0
*/
go
update
lab_item set imprimeMuestra=0
where imprimeMuestra=1
and baja=0