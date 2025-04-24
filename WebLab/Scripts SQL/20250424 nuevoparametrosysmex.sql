/*control de cambios
-Conexion Sysmex Kx21N. Se cambia parametro RDW_CV por RDW-CV y RDW_SD por RDW-SD.
*/



select * from LAB_SysmexItemKX21N
where idsysmex ='RDW_CV'

update LAB_SysmexItemKX21N
set idsysmex='RDW-CV'
where idsysmex ='RDW_CV'
 
 update LAB_SysmexItemKX21N
set idsysmex='RDW-SD'
where idsysmex ='RDW_SD'
 go
 ---habilitacion Sysmex KX21N SMA
 if not exists (select 1 from LAB_EfectorEquipo where idefector=70 and descripcion like '%Config. Sysmex KX21N%')
 insert into LAB_EfectorEquipo
 select 70 as idEfector , idMenuEquipo, descripcion, habilitado
  from LAB_EfectorEquipo where descripcion like '%Config. Sysmex KX21N%' and idefector=5
 
 
 select * from LAB_EfectorEquipo where idefector in (70)