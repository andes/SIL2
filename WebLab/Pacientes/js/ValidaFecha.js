// Archivo JScript

function AnioBisiesto(anio) {
return (((anio % 4 == 0) && (anio % 100 != 0)) || (anio % 400 == 0)) ? 1 : 0;
}

function esDigito(sChr){
var sCod = sChr.charCodeAt(0);
return ((sCod > 47) && (sCod < 58));
}

function valSep(oTxt){
var bOk = false;
bOk = bOk || ((oTxt.value.charAt(2) == "-") && (oTxt.value.charAt(5) == "-"));
bOk = bOk || ((oTxt.value.charAt(2) == "/") && (oTxt.value.charAt(5) == "/"));
return bOk;
}

function finMes(oTxt){
var nMes = parseInt(oTxt.value.substr(3, 2), 10);
var nAno = parseInt(oTxt.value.substr(6, 4), 10);
var nRes = 0;
switch (nMes){
case 1: nRes = 31; break;
case 2:{
    if (AnioBisiesto(nAno)) {
        nRes = 29; }
    else {
        nRes = 28; }
    break;
    }
case 3: nRes = 31; break;
case 4: nRes = 30; break;
case 5: nRes = 31; break;
case 6: nRes = 30; break;
case 7: nRes = 31; break;
case 8: nRes = 31; break;
case 9: nRes = 30; break;
case 10: nRes = 31; break;
case 11: nRes = 30; break;
case 12: nRes = 31; break;
}
return nRes;
}

function valDia(oTxt){
var bOk = false;
var nDia = parseInt(oTxt.value.substr(0, 2), 10);
bOk = bOk || ((nDia >= 1) && (nDia <= finMes(oTxt)));
return bOk;
}

function valMes(oTxt){
var bOk = false;
var nMes = parseInt(oTxt.value.substr(3, 2), 10);
bOk = bOk || ((nMes >= 1) && (nMes <= 12));
return bOk;
}

function valAno(oTxt){
var bOk = true;
var nAno = oTxt.value.substr(6);
bOk = bOk && ((nAno.length == 2) || (nAno.length == 4));
if (bOk){
for (var i = 0; i < nAno.length; i++){
bOk = bOk && esDigito(nAno.charAt(i));
}
}
if (nAno.length == 4){
bOk = bOk && (nAno >= 1900)
}
return bOk;
}

/*function valFecha(oTxt){
var bOk = true;
if (oTxt.value != ""){
bOk = bOk && (valAno(oTxt));
bOk = bOk && (valMes(oTxt));
bOk = bOk && (valDia(oTxt));
bOk = bOk && (valSep(oTxt));
if (!bOk){
alert("Fecha inválida");
oTxt.value = "";
oTxt.focus();
}
}
}*/

function valFecha(oTxt)
{
    var bOk = true;
    if (oTxt.value != "")
    {
        if (oTxt.value.length == 5)
        {
            var fecha_actual = new Date();
            oTxt.value = oTxt.value + "/" + fecha_actual.getFullYear();
        }
        bOk = bOk && (valAno(oTxt));
        bOk = bOk && (valMes(oTxt));
        bOk = bOk && (valDia(oTxt));
        bOk = bOk && (valSep(oTxt));
        if (!bOk)
        {
            alert("Fecha inválida");
            oTxt.value = "";
            oTxt.focus();
        }
    }
}

function valHora(oTxt)
{
    var bOk = true;
    if (oTxt.value != "")
    {
        var nHs = parseInt(oTxt.value.substr(0, 2), 10);
        var nMin = parseInt(oTxt.value.substr(3, 2), 10);
        bOk = bOk && (nHs <= 23) && (nMin <= 59) && (oTxt.value.length == 5);
        if (!bOk)
        {
            alert("Hora inválida");
            oTxt.value = "";
            oTxt.focus();
        }
    }
}

function valNumero(oTxt)
{
    if (oTxt.value != "")
    {
        if (!IsNumeric(oTxt.value))
        {
            alert("Ingrese Número");
            oTxt.value = "";
            oTxt.focus();
        }
    }
}

function IsNumeric(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;
   var Comas = 0;

   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
         
      if (Char == ".")
         {
         Comas = Comas + 1;
         }
      }
   
   if (Comas > 1)
     {
     IsNumber = false;
     }
   
   return IsNumber;   
}

