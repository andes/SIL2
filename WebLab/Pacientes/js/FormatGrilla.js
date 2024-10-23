//---------------------------------------------------------------------------­----------
//Efectos sobre grilla/celda
//---------------------------------------------------------------------------­----------

function grillaMouseOver(src, classOver) {
  if (!src.contains(event.fromElement)) {
    src.style.cursor = 'hand';
    src.className = classOver;
  }

}

function grillaMouseOut(src, classIn) {
  if (!src.contains(event.toElement)) {
    src.style.cursor = 'default';
    src.className = classIn;
  }
}