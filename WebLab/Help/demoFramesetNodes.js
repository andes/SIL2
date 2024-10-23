//
// Copyright (c) 2006 by Conor O'Mahony.
// For enquiries, please email GubuSoft@GubuSoft.com.
// Please keep all copyright notices below.
// Original author of TreeView script is Marcelino Martins.
//
// This document includes the TreeView script.
// The TreeView script can be found at http://www.TreeView.net.
// The script is Copyright (c) 2006 by Conor O'Mahony.
//
// You can find general instructions for this file at www.treeview.net. 
//

// Configures whether the names of the nodes are links (or whether only the icons
// are links).
USETEXTLINKS = 1

// Configures whether the tree is fully open upon loading of the page, or whether
// only the root node is visible.
STARTALLOPEN = 0

// Specify if the images are in a subdirectory;
ICONPATH = ''


foldersTree = gFld("<i>LIS</i>", "Documentos/principal.htm")
  foldersTree.treeID = "Frameset"

  aux1 = insFld(foldersTree, gFld("Capitulo 1 - Introducción",  "Documentos/descripcion del LIS.htm"))

    //aux2 = insFld(aux1, gFld("United States", "http://www.treeview.net/treemenu/demopics/beenthere_america.gif"))  
      insDoc(aux1, gLnk("R", "Descripcion del LIS", "Documentos/descripcion del LIS.htm"))
      insDoc(aux1, gLnk("R", "Requisitos del LIS", "Documentos/Requisitos del lis.htm"))
      insDoc(aux1, gLnk("R", "Acceso al LIS", "Documentos/Acceso al lis.htm"))            

//  aux1 = insFld(foldersTree, gFld("Capitulo 2 - Usuarios del Sistema", "Documentos/Tipos de usuario y permisos de acceso.htm"))
//      insDoc(aux1, gLnk("R", "Usuarios y Permisos", "Documentos/Tipos de usuario y permisos de acceso.htm"))      
//      insDoc(aux1, gLnk("R", "Entrar al LIS", "Documentos/Acceso al lis.htm"))
//      insDoc(aux1, gLnk("R", "Acceso externo", "Documentos/ACCESO A RESULTADOS.htm"))
      
    
     
  aux1 = insFld(foldersTree, gFld("Capitulo 2 - Configuracion del Sistema", "Documentos/Configuracion DEL LIS.htm"))
      /*insDoc(aux1, gLnk("R", "Introducción", "Documentos/Configuracion DEL LIS.htm"))               


      aux2 = insFld(aux1, gFld("Items a Configurar",  "javascript:parent.op()"))*/
      
      insDoc(aux1, gLnk("R", "Agenda", "Documentos/Agenda.htm"))
      insDoc(aux1, gLnk("R", "Analisis", "Documentos/ANALISIS.htm"))
      insDoc(aux1, gLnk("R", "Antibióticos", "Documentos/Antibioticos.htm"))
      insDoc(aux1, gLnk("R", "Areas", "Documentos/Área.htm"))
      insDoc(aux1, gLnk("R", "Formulas y Controles", "Documentos/Formulas y Controles.htm"))
      insDoc(aux1, gLnk("R", "Microrganismos", "Documentos/Microorganismo.htm"))
      insDoc(aux1, gLnk("R", "Hojas de Trabajo", "Documentos/Hoja de Trabajo.htm"))
      insDoc(aux1, gLnk("R", "Médicos solicitantes", "Documentos/Medico.htm"))
      insDoc(aux1, gLnk("R", "Metodos", "Documentos/Métodos.htm"))
      insDoc(aux1, gLnk("R", "Observaciones Predefinidas", "Documentos/Observaciones.htm"))
      insDoc(aux1, gLnk("R", "Orden de Impresion", "Documentos/Orden de Impresión.htm"))
      insDoc(aux1, gLnk("R", "Pefil de Antibióticos", "Documentos/Perfil de ANTIBIOTICOS.htm"))
      insDoc(aux1, gLnk("R", "Parametros del LIS", "Documentos/Parámetros del Sistema.htm"))
      insDoc(aux1, gLnk("R", "Recomendaciones", "Documentos/Recomendaciones.htm"))
      insDoc(aux1, gLnk("R", "Rutinas", "Documentos/Rutinas.htm"))
      insDoc(aux1, gLnk("R", "Servicios", "Documentos/Servicios.htm"))
      insDoc(aux1, gLnk("R", "Tipo de muestras", "Documentos/Tipo de Muestra.htm"))
      insDoc(aux1, gLnk("R", "Unidad de Medida", "Documentos/Unidad de Medida.htm"))
      insDoc(aux1, gLnk("R", "Unificar Pacientes", "Documentos/UNIFICAR PACIENTES.htm"))
      insDoc(aux1, gLnk("R", "Usuarios del sistema", "Documentos/Usuarios.htm"))
       
       
       
       
    aux1 = insFld(foldersTree, gFld("Capitulo 3- Operacion del Sistema", "Documentos/Operación del LIS.htm"))
        aux2 = insFld(aux1, gFld("PreAnalitica", "Documentos/Carga de REsultados.htm"))
            insDoc(aux2, gLnk("R", "Recepcion del Paciente", "Documentos/recepcion del paciente.htm"))            
            insDoc(aux2, gLnk("R", "Acceder a Protocolos", "Documentos/Lista de Protocolos.htm"))

            aux3 = insFld(aux2, gFld("Módulo de Turnos", "javascript:parent.op()"))
            insDoc(aux3, gLnk("R", "Asignación", "Documentos/Generación de turnos.htm"))
            insDoc(aux3, gLnk("R", "Recepción de Pacientes", "Documentos/turno Recepción de Pacientes.htm"))

        aux4 = insFld(aux1, gFld("Analitica", "Documentos/Carga de REsultados.htm"))
            insDoc(aux4, gLnk("R", "Informes", "Documentos/informes.htm"))    
      
            aux5 = insFld(aux4, gFld("Carga de Resultados", "Documentos/Carga de REsultados.htm"))
            insDoc(aux5, gLnk("R", "Por Protocolo", "Documentos/Carga por Lista de Protocolos.htm"))
            insDoc(aux5, gLnk("R", "Por Hoja de Trabajo", "Documentos/Carga por Hoja de Trabajo.htm"))
            insDoc(aux5, gLnk("R", "Por Analisis", "Documentos/Carga Por Analisis.htm"))
    
    
            insDoc(aux4, gLnk("R", "Control de Resultados", "Documentos/CONTROL DE RESULTADOS.htm"))

            insDoc(aux4, gLnk("R", "Validacion", "Documentos/Validación.htm"))
            insDoc(aux4, gLnk("R", "Módulo de Derivaciones", "Documentos/Derivaciones.htm"))                

         aux6 = insFld(aux1, gFld("PostAnalitica", "Documentos/Carga de REsultados.htm"))       
            insDoc(aux6, gLnk("R", "Impresión de Resultados", "Documentos/Impresión de Resultados.htm"))            
     
//     aux2 = insFld(aux1, gFld("Presentación del Modelo",  "../Ayuda/MIII/Fase_4/Acciones/PRESENTACION_DEL_MODELO.htm"))
//      insDoc(aux2, gLnk("R", "Presentación", "UI_Tarea.aspx?id=20"))
//      
 
 
 
        


        insDoc(aux1, gLnk("R", "Módulo de Urgencias", "Documentos/Urgencias.htm"))
        insDoc(aux1, gLnk("R", "Módulo de Microbiología", "Documentos/Microbiología.htm")) 


//  aux1 = insFld(foldersTree, gFld("Estadisticas", "javascript:parent.op()"))

//     insDoc(aux1, gLnk("R", "Descripcion", "UI_Tarea.aspx?id=6"))  

    aux1 = insFld(foldersTree, gFld("Capitulo 4 - Estadísticas", "Documentos/Estadísticas.htm"))    
        insDoc(aux1, gLnk("R", "Generales", "Documentos/Estadisticas Generales.htm"))          
        insDoc(aux1, gLnk("R", "Por Resultados", "Documentos/Estadísticas Por Resultado.htm"))  
         insDoc(aux1, gLnk("R", "Turnos", "Documentos/Estadísticas de Turnos.htm"))

         aux1 = insFld(foldersTree, gFld("Control de Cambios", "Documentos/Control de Cambios v3.2012.htm"))
  aux1 = insFld(foldersTree, gFld("Anexo: Diagnósticos CIE10",  "Documentos/Cie 10.htm"))
  aux1 = insFld(foldersTree, gLnk("B", "Anexo: Manual Conexion Sysmex Serie XS/XT", "Documentos/Manuales/Manual de Configuracion Sysmex.pdf"))
  //aux1 = insFld(foldersTree, gLnk("B", "Anexo: Manual Conexion Sysmex KX21", "Documentos/Manuales/Manual de Configuracion Sysmex Serial.pdf"))
  aux1 = insFld(foldersTree, gLnk("B", "Anexo: Manual Conexion Metrolab", "Documentos/Manuales/Manual de Configuracion METROLAB.pdf"))
   
    //docAux = insDoc(aux1, gLnk("B", " Descargar",  "../Ayuda/AyudaP3TQ.chm"))
//    docAux.iconSrc = ICONPATH + "wht_toc2.gif"

  aux1 = insFld(foldersTree, gLnk("B","Descargar Version Imprimible",  "Ayuda Lis.pdf"))  
  
  
  
  
//aux1 = insFld(foldersTree, gFld("<font color=red>F</font><font color=blue>o</font><font color=pink>r</font><font color=green>m</font><font color=red>a</font><font color=blue>t</font><font color=brown>s</font>", "javascript:parent.op()"))
//    docAux = insDoc(aux1, gLnk("R", "<div class=specialClass>CSS Class</div>", "http://www.treeview.net/treemenu/demopics/beenthere_newyork.jpg"))

