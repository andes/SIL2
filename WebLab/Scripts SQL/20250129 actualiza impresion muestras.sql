/*control de cambios
- Recepci�n de FNE interoperando con ANDES
- Generaci�n de todas las etiquetas en la carga de protocolos.
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