/* *** código para la página moverfilas.html *** */
var tab;
var filas;

function iniciarTabla(cuerpo) {
  tab = document.getElementById(cuerpo);
  filas = tab.getElementsByTagName('tr');
  for (i=0; ele = filas[i]; i++) {
   // alert(ele.getElementsByTagName('text'));
    ele.getElementsByTagName('img')[0].onclick = function() { mover(this,-1,cuerpo) }
    ele.getElementsByTagName('img')[1].onclick = function() { mover(this,1,cuerpo) }
    ele.getElementsByTagName('img')[2].onclick = function() { borrarfila(this,cuerpo) }

  }
  mostrarOcultar(cuerpo);
      
}

// Ocultar imagen subir en primera fila y bajar en última fila. Mostrar el resto de imágenes
function mostrarOcultar(cuerpo) {
  for (i=0; ele = filas[i]; i++) {
    ele.getElementsByTagName('img')[0].style.visibility = (i==0) ? 'hidden' : 'visible';
    ele.getElementsByTagName('img')[1].style.visibility = (i==filas.length-1) ? 'hidden' : 'visible';
  }
  if (cuerpo=='cuerpo')  
  CargarDatos();
  else
  CargarDatosDiagrama();
}

function mover(obj,num,cuerpo) {
  fila = obj.parentNode.parentNode;
  for (i=0; ele = tab.getElementsByTagName('tr')[i]; i++)
    if (ele == fila) {numFila=i; break}
  copia = filas[numFila].cloneNode(true);
  // Añadir evento onclick a las imágenes
  copia.getElementsByTagName('img')[0].onclick = function() { mover(this,-1,cuerpo); }
  copia.getElementsByTagName('img')[1].onclick = function() { mover(this,1,cuerpo); }
  copia.getElementsByTagName('img')[2].onclick = function() { borrarfila(this,cuerpo); }

  tab.removeChild(filas[numFila]);
  numFila += num;
  if (numFila > filas.length-1) 
    tab.appendChild(copia);
  else
    tab.insertBefore(copia,filas[numFila]);
  mostrarOcultar(cuerpo);
}

 function borrarfila(obj,cuerpo)
        {

	            var delRow = obj.parentNode.parentNode;
	            var tbl = delRow.parentNode.parentNode;
	            var rIndex = delRow.sectionRowIndex;
	            Grilla = document.getElementById(cuerpo); 
	            Grilla.parentNode.deleteRow(rIndex);
	             if (cuerpo=='cuerpo')  
  CargarDatos();
  else
  CargarDatosDiagrama();
	         
}	         


  