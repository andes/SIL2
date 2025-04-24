
--select * from LAB_ResultadoItem
/*Se agrega marca de resultado por defecto por efector*/
alter table LAB_ResultadoItem add resultadoDefecto bit 
go
update LAB_ResultadoItem
set resultadoDefecto=0
go


---si tiene una valor a nivel de efector va ese sino busca desde lab_item
alter table LAB_ItemEfector add resultadoDefecto varchar(200)--es para todos excepto para predefinidos
go
update LAB_ItemEfector
set resultadoDefecto=''

 --Permiso al perfil Tecnico sobre impresion de etiquetas
 --select * from sys_perfil ---tecnico =7
 --select * from sys_menu where objeto like '%etiquet%' --203
 --select * from sys_permiso where idperfil=7 and idMenu=203
 insert into sys_permiso
 select 227, 7, 203, '2' 
 ---Depuracion de tablas
 go
 delete from LAB_AuditoriaProtocolo
 where idUsuario=2
 go
 delete from LAB_AuditoriaItem
 where idUsuario=2
