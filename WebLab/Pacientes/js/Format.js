function formatearFecha(object) {

    var checkstr = "0123456789";
    var DateField = object; //eval('document.' + form + '.' + nameField);
    var Datevalue = "";
    var DateTemp = "";
    var seperator = "/";
    var day;
    var month;
    var year;
    var leap = 0;
    var err = 0;
    var i;
    err = 0;
    DateValue = DateField.value;
    /* Delete all chars except 0..9 */
    for (i = 0; i < DateValue.length; i++) {
        if (checkstr.indexOf(DateValue.substr(i, 1)) >= 0) {
            DateTemp = DateTemp + DateValue.substr(i, 1);
        }
    }
    DateValue = DateTemp;
    /* Always change date to 8 digits - string*/
    /* if year is entered as 2-digit / always assume 20xx */
    if (DateValue.length == 6) {
        var tempInt = parseInt(DateValue.substr(4, 2));
        if (tempInt > 50) {
            tempInt = 19;
        }
        else {
            tempInt = 20;
        }
        DateValue = DateValue.substr(0, 4) + tempInt + DateValue.substr(4, 2);
    }
    if (DateValue.length != 8) {
        err = 19;
    }
    /* year is wrong if year = 0000 */
    year = DateValue.substr(4, 4);
    if (year == 0) {
        err = 20;
    }
    /* Validation of month*/
    month = DateValue.substr(2, 2);
    if ((month < 1) || (month > 12)) {
        err = 21;
    }
    /* Validation of day*/
    day = DateValue.substr(0, 2);
    if (day < 1) {
        err = 22;
    }
    /* Validation leap-year / february / day */
    if ((year % 4 == 0) || (year % 100 == 0) || (year % 400 == 0)) {
        leap = 1;
    }
    if ((month == 2) && (leap == 1) && (day > 29)) {
        err = 23;
    }
    if ((month == 2) && (leap != 1) && (day > 28)) {
        err = 24;
    }
    /* Validation of other months */
    if ((day > 31) && ((month == "01") || (month == "03") || (month == "05") || (month == "07") || (month == "08") || (month == "10") || (month == "12"))) {
        err = 25;
    }
    if ((day > 30) && ((month == "04") || (month == "06") || (month == "09") || (month == "11"))) {
        err = 26;
    }
    /* if 00 ist entered, no error, deleting the entry */
    if ((day == 0) && (month == 0) && (year == 00)) {
        err = 0; day = ""; month = ""; year = ""; seperator = "";
    }
    /* if no error, write the completed date to Input-Field (e.g. 13.12.2001) */
    if (err == 0) {
        DateField.value = day + seperator + month + seperator + year;
        DateField.style.background = "#FFF";
    }
    /* Error-message if err != 0 */
    else {
        DateField.style.background = "#FF6666";
        DateField.select();
        DateField.focus();
    }
}

function formatearFechaDesde(object) {

    var checkstr = "0123456789";
    var DateField = object; //eval('document.' + form + '.' + nameField);
    var Datevalue = "";
    var DateTemp = "";
    var seperator = "/";
    var day;
    var month;
    var year;
    var leap = 0;
    var err = 0;
    var i;
    err = 0;
    DateValue = DateField.value;
    /* Delete all chars except 0..9 */
    for (i = 0; i < DateValue.length; i++) {
        if (checkstr.indexOf(DateValue.substr(i, 1)) >= 0) {
            DateTemp = DateTemp + DateValue.substr(i, 1);
        }
    }
    DateValue = DateTemp;

    /*Lucas: Si solo se ingresa el año, lo convierto */
    if (DateValue.length == 2) {
        var tempInt = DateValue;
        if (tempInt > 50) {
            tempInt = 19;
        }
        else {
            tempInt = 20;
        }
        DateValue = "0101" + tempInt + DateValue;
    }
    
    if (DateValue.length == 4) {
        DateValue = "0101" + DateValue;
    }
    
    /* Always change date to 8 digits - string*/
    /* if year is entered as 2-digit / always assume 20xx */
    
    if (DateValue.length == 6) {
        var tempInt = parseInt(DateValue.substr(4, 2));
        if (tempInt > 50) {
            tempInt = 19;
        }
        else {
            tempInt = 20;
        }
        DateValue = DateValue.substr(0, 4) + tempInt + DateValue.substr(4, 2);
    }
    if (DateValue.length != 8) {
        err = 19;
    }
    /* year is wrong if year = 0000 */
    year = DateValue.substr(4, 4);
    if (year == 0) {
        err = 20;
    }
    /* Validation of month*/
    month = DateValue.substr(2, 2);
    if ((month < 1) || (month > 12)) {
        err = 21;
    }
    /* Validation of day*/
    day = DateValue.substr(0, 2);
    if (day < 1) {
        err = 22;
    }
    /* Validation leap-year / february / day */
    if ((year % 4 == 0) || (year % 100 == 0) || (year % 400 == 0)) {
        leap = 1;
    }
    if ((month == 2) && (leap == 1) && (day > 29)) {
        err = 23;
    }
    if ((month == 2) && (leap != 1) && (day > 28)) {
        err = 24;
    }
    /* Validation of other months */
    if ((day > 31) && ((month == "01") || (month == "03") || (month == "05") || (month == "07") || (month == "08") || (month == "10") || (month == "12"))) {
        err = 25;
    }
    if ((day > 30) && ((month == "04") || (month == "06") || (month == "09") || (month == "11"))) {
        err = 26;
    }
    /* if 00 ist entered, no error, deleting the entry */
    if ((day == 0) && (month == 0) && (year == 00)) {
        err = 0; day = ""; month = ""; year = ""; seperator = "";
    }
    /* if no error, write the completed date to Input-Field (e.g. 13.12.2001) */
    if (err == 0) {
        DateField.value = day + seperator + month + seperator + year;
        DateField.style.background = "#FFF";
    }
    /* Error-message if err != 0 */
    else {
        DateField.style.background = "#FF6666";
        DateField.select();
        DateField.focus();
    }
}

function formatearFechaHasta(object) {

    var checkstr = "0123456789";
    var DateField = object; //eval('document.' + form + '.' + nameField);
    var Datevalue = "";
    var DateTemp = "";
    var seperator = "/";
    var day;
    var month;
    var year;
    var leap = 0;
    var err = 0;
    var i;
    err = 0;
    DateValue = DateField.value;
    /* Delete all chars except 0..9 */
    for (i = 0; i < DateValue.length; i++) {
        if (checkstr.indexOf(DateValue.substr(i, 1)) >= 0) {
            DateTemp = DateTemp + DateValue.substr(i, 1);
        }
    }
    DateValue = DateTemp;

    /*Lucas: Si solo se ingresa el año, lo convierto */
    if (DateValue.length == 2) {
        var tempInt = DateValue;
        if (tempInt > 50) {
            tempInt = 19;
        }
        else {
            tempInt = 20;
        }
        DateValue = "3112" + tempInt + DateValue;
    }

    if (DateValue.length == 4) {
        DateValue = "3112" + DateValue;
    }

    /* Always change date to 8 digits - string*/
    /* if year is entered as 2-digit / always assume 20xx */

    if (DateValue.length == 6) {
        var tempInt = parseInt(DateValue.substr(4, 2));
        if (tempInt > 50) {
            tempInt = 19;
        }
        else {
            tempInt = 20;
        }
        DateValue = DateValue.substr(0, 4) + tempInt + DateValue.substr(4, 2);
    }
    if (DateValue.length != 8) {
        err = 19;
    }
    /* year is wrong if year = 0000 */
    year = DateValue.substr(4, 4);
    if (year == 0) {
        err = 20;
    }
    /* Validation of month*/
    month = DateValue.substr(2, 2);
    if ((month < 1) || (month > 12)) {
        err = 21;
    }
    /* Validation of day*/
    day = DateValue.substr(0, 2);
    if (day < 1) {
        err = 22;
    }
    /* Validation leap-year / february / day */
    if ((year % 4 == 0) || (year % 100 == 0) || (year % 400 == 0)) {
        leap = 1;
    }
    if ((month == 2) && (leap == 1) && (day > 29)) {
        err = 23;
    }
    if ((month == 2) && (leap != 1) && (day > 28)) {
        err = 24;
    }
    /* Validation of other months */
    if ((day > 31) && ((month == "01") || (month == "03") || (month == "05") || (month == "07") || (month == "08") || (month == "10") || (month == "12"))) {
        err = 25;
    }
    if ((day > 30) && ((month == "04") || (month == "06") || (month == "09") || (month == "11"))) {
        err = 26;
    }
    /* if 00 ist entered, no error, deleting the entry */
    if ((day == 0) && (month == 0) && (year == 00)) {
        err = 0; day = ""; month = ""; year = ""; seperator = "";
    }
    /* if no error, write the completed date to Input-Field (e.g. 13.12.2001) */
    if (err == 0) {
        DateField.value = day + seperator + month + seperator + year;
        DateField.style.background = "#FFF";
    }
    /* Error-message if err != 0 */
    else {
        DateField.style.background = "#FF6666";
        DateField.select();
        DateField.focus();
    }
}


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