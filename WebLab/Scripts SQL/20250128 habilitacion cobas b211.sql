
select * from sys_menu where idmenu=239
update sys_menu set habilitado=1, idMenuSuperior=217 where idmenu=239
 GO
DELETE FROM sys_usuario where idUsuario=65
go

if not exists(select top 1 * from LAB_EfectorEquipo where idEfector=2)
begin
insert into LAB_EfectorEquipo
select 2 as idefector, idMenuEquipo, descripcion, habilitado from LAB_EfectorEquipo
where idEfector=70


insert into LAB_EfectorEquipo
select top 1 2 as idefector, 239 as idMenuEquipo,'Config. Cobas b 221' descripcion, 1 habilitado from LAB_EfectorEquipo
end
 
 select  * from LAB_EfectorEquipo where idEfector=2
