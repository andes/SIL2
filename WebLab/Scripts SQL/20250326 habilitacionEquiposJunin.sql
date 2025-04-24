
---Permisos autoanalizadores junin

 delete from LAB_EfectorEquipo
 where idefector=71
 and idMenuEquipo=2320
 --238- Config. Sysmex XP300    
 
 if not exists (select 1 from LAB_EfectorEquipo where idefector=71 and idMenuEquipo=238)
 insert into LAB_EfectorEquipo
 select 71 as idEfector , idMenuEquipo, descripcion, habilitado
 from LAB_EfectorEquipo where idefector=3
  and idMenuEquipo=238
 ------
 select * from LAB_EfectorEquipo where idefector=71

 go 