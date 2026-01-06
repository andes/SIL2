<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalisisEdit.aspx.cs" Inherits="WebLab.Resultados.AnalisisEdit" MasterPageFile="~/Site2.Master"  %>


<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
       <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      
 <%--  	 <script type="text/javascript" src="../script/Mascara.js"></script>--%>
     <script type="text/javascript" src="../script/ValidaFecha.js"></script>                
     <script src="jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>  
     <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
     
     <script type="text/javascript">
       


         function enterToTab(pEvent) {///solo para internet explorer
             if (window.event) // IE
             {
                 if (window.event.keyCode == 13) {
                     if (pEvent.srcElement.id.indexOf('Codigo_') == 0) {
                         window.event.keyCode = 9;
                     }
                 }
             }

         }

         function Enter(field, event) {
             var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
             if (keyCode == 13) {
                 var i;
                 for (i = 0; i < field.form.elements.length; i++)
                     if (field == field.form.elements[i])
                         break;
                 i = (i + 1) % field.form.elements.length;
                 field.form.elements[i].focus();
                 return false;
             }
             else
                 return true;

         }

  </script>  
  
  


   
    </asp:Content>




 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">   
     <input id="hidToken" type="hidden" runat="server" />   
     <div  style="width: 750px" class="form-inline" >
      <div class="panel panel-default" runat="server"  >
                    <div class="panel-heading">
                   <h5>    <strong> <asp:Label ID="lblProtocolo" runat="server" Text="Label"></asp:Label></strong> </h5>
    
  </div>
                    <div class="panel-body">
                         <asp:CustomValidator ID="cvValidacionInput" runat="server" 
                                                ErrorMessage="Debe completar al menos un analisis" 
                                    ValidationGroup="0" Font-Size="8pt" onservervalidate="cvValidacionInput_ServerValidate" 
                                             ></asp:CustomValidator>
<asp:Panel  runat="server" ID="pnlMuestra" Width="100%" Visible="false">
					 
                          Muestra: &nbsp; &nbsp; 
                            <anthem:TextBox ID="txtCodigoMuestra" Width="50px"   class="form-control input-sm" runat="server"  ontextchanged="txtCodigoMuestra_TextChanged" AutoCallBack="true"></anthem:TextBox> 
                            <anthem:DropDownList ID="ddlMuestra" runat="server" AutoCallBack="true" Width="250px"  onselectedindexchanged="ddlMuestra_SelectedIndexChanged"   class="form-control input-sm" >
                            </anthem:DropDownList>
                            <anthem:RangeValidator ID="rvMuestra" runat="server"     ErrorMessage="Muestra" 
                                ControlToValidate="ddlMuestra" Enabled="False" MaximumValue="9999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                            
                            </asp:Panel>
                        <br />

                        <table  width="600px" align="left">
				 
					 
					
					<tr>
						<td colspan="2"  >
						
					
                          
                            <div id="tab1" style="height: 230px">
                   

                                 <table style="width:600px;">
                                <tr>
                                    <td>
                                  <table cellpadding="1" cellspacing="0" >
                    <tr >
                        <td style="width: 10px; height: 13px">
                        </td>
                        <td style="width: 50px;" class="tituloCelda">
                            Codigo</td>
                        <td style="width: 350px;"  class="tituloCelda">
                            Descripcion</td>
                        <td style="width: 80px; " class="tituloCelda">
                            S/ Muestra</td>                    
                        <td style="width: 18px;" class="tituloCelda">
                        </td>
                    </tr>
                </table>
                
                <div  onkeydown="enterToTab(event)" style="width:540px;height:160pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                       
                    <table  class="mytablaIngreso"  border="0" id="tabla" cellpadding="0" cellspacing="0" >
	  	                <tbody id="Datos" >
		                    
		                </tbody>
	                </table>
	                
                    <input type="hidden" runat="server" name="TxtCantidadFilas" id="TxtCantidadFilas" value="0" />
                </div>
                      <div> 
                                             
                            
						
                                           <%-- <asp:CustomValidator ID="cvValidacionInput" runat="server" 
                                                ErrorMessage="Debe completar al menos un analisis" 
                                    ValidationGroup="0" Font-Size="8pt" onservervalidate="cvValidacionInput_ServerValidate" 
                                             ></asp:CustomValidator>--%>
                                                 
                                </div>                 
                                      </td>
                                    <td style="vertical-align: top">
                                       
                                       <fieldset id="Fieldset3" title="Analisis" style="width:95%;text-align:left;">
                                    
                                       <table>   
                                         <tr><td class="myLabelIzquierda" >  <anthem:DropDownList ID="ddlRutina" runat="server" AutoCallBack="True"                                                
                                                
                                class="form-control input-sm" TabIndex="20" Width="150px"
                                onselectedindexchanged="ddlRutina_SelectedIndexChanged">
                                          </anthem:DropDownList>
                                          <input id="Button2" type="button" value="Agregar" 
                                onclick="AgregarDeLaListaRutina();" class="btn btn-info"
                                 style="width: 100px" />
                                </td></tr>
                                       
                                       <tr><td class="myLabelIzquierda" >	<anthem:DropDownList ID="ddlItem" runat="server" AutoCallBack="True" 
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" 
                                                TextDuringCallBack="Cargando ..."  Width="150px"
                                class="form-control input-sm" TabIndex="20">
                                            </anthem:DropDownList>	                                                   
                            <input id="Button1" type="button" value="Agregar" 
                                onclick="AgregarDeLaLista();" 
                               class="btn btn-info" style="width: 100px" /></td>
                                       
                                       </tr>
                                      
                                       </table>
                                       </fieldset>
                                     
                                                
                                           &nbsp;</td>

                                </tr>
                                </table>
                                <input type="hidden" runat="server" name="TxtDatosCargados" id="TxtDatosCargados" value="" />                                
                                   <input type="hidden" runat="server" name="TxtDatos" id="TxtDatos" value="" />                                
                <input id="txtTareas" name="txtTareas" runat="server" type="hidden"  />
                            </div>
                          
                          
                        </td>
					</tr>
					
																					
						
						
						
				
						
					 
						
					
						
						
						
					<tr>
						
						<td align="left" >
						
                                             <anthem:TextBox ID="txtCodigo" runat="server" BorderColor="White" ForeColor="White" 
                                BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>
                                              <anthem:TextBox ID="txtCodigosRutina"  runat="server" BorderColor="White" 
                                ForeColor="White" BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>
                                                 
                        </td>
						
						<td  align="left">
						
                                           </td>
						
					</tr>
				
						
					
						
						
						
						
						
						</table>

                        </div>
          
                               <div class="panel-footer">	
                                               
		
			  <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                     HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                     ValidationGroup="0" ShowSummary="False" />			
                                        
                                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0"  Width="100px" CssClass ="btn btn-primary"
                                                onclick="btnGuardar_Click"    TabIndex="24" />

                                               
                                   </div>
          </div>
     
                                           
      
</div>
                 

<script language="javascript" type="text/javascript">

    var contadorfilas = 0;
    InicioPagina();

    document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value = contadorfilas;

    function VerificaLargo(source, arguments) {
        var Observacion = arguments.Value.toString().length;
        //   alert(Observacion);
        if (Observacion > 4000)
            arguments.IsValid = false;
        else
            arguments.IsValid = true;    //Si llego hasta aqui entonces la validación fue exitosa        
    }



    function InicioPagina() {
        if (document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosCargados").ClientID %>').value == "") {///protocolo nuevo
            CrearFila(true);
        }
        else {        ///modificacion de protocolo
            AgregarCargados();
        }

    }


    function NuevaFila() {
        Grilla = document.getElementById('Datos');


        fila = document.createElement('tr');
        fila.id = 'cod_' + contadorfilas;
        fila.name = 'cod_' + contadorfilas;

        celda1 = document.createElement('td');
        oNroFila = document.createElement('input');
        oNroFila.type = 'text';
        oNroFila.readOnly = true;

        oNroFila.runat = 'server';
        oNroFila.name = 'NroFila_' + contadorfilas;
        oNroFila.id = 'NroFila_' + contadorfilas;
        //oNroFila.onfocus= function() {PasarFoco(this)}
        oNroFila.className = 'fila';
        celda1.appendChild(oNroFila);
        fila.appendChild(celda1);

        celda2 = document.createElement('td');
        oCodigo = document.createElement('input');

        oCodigo.type = 'text';
        oCodigo.runat = 'server';
        oCodigo.name = 'Codigo_' + contadorfilas;
        oCodigo.id = 'Codigo_' + contadorfilas;
        oCodigo.className = 'codigo';
        oCodigo.onblur = function () {
            CargarTarea(this);
        };

        oCodigo.setAttribute("onkeypress", "javascript:return Enter(this, event)");
        //oCodigo onkeypress = function(){ return Enter(this, event) };
        //oCodigo.setAttribute( = function () { alert('hola'); if (event.keyCode == 13) event.keyCode = 9; };
        //oCodigo.onchange = function () {CargarDatos()};
        celda2.appendChild(oCodigo);
        fila.appendChild(celda2);

        celda3 = document.createElement('td');
        oTarea = document.createElement('input');
        oTarea.type = 'text';
        oTarea.readOnly = true;
        oTarea.runat = 'server';
        oTarea.name = 'Tarea_' + contadorfilas;
        oTarea.id = 'Tarea_' + contadorfilas;
        oTarea.className = 'descripcion';
        oTarea.onchange = function () { CargarDatos() };
        celda3.appendChild(oTarea);
        fila.appendChild(celda3);

        celda4 = document.createElement('td');
        oDesde = document.createElement('input');
        oDesde.type = 'checkbox';
        oDesde.runat = 'server';



        oDesde.name = 'Desde_' + contadorfilas;
        oDesde.id = 'Desde_' + contadorfilas;
        oDesde.alt = "Sin muestra";

        oDesde.className = 'muestra';
        oDesde.onblur = function () { CargarDatos(); };

        celda4.appendChild(oDesde);
        fila.appendChild(celda4);



        celda6 = document.createElement('td');
        oBoton = document.createElement('input');
        oBoton.className = 'boton';
        oBoton.name = 'boton_' + contadorfilas;
        oBoton.type = 'button';
        oBoton.value = 'X';
        oBoton.onclick = function () { borrarfila(this) };
        celda6.appendChild(oBoton);
        fila.appendChild(celda6);

        Grilla.appendChild(fila);
        contadorfilas = contadorfilas + 1;
    }


    function CrearFila(validar) {
        var valFila = contadorfilas - 1;
        if (UltimaFilaCompleta(valFila, validar)) {

            NuevaFila();

            valFila = contadorfilas - 1;
            document.getElementById('NroFila_' + valFila).value = contadorfilas;

            if (contadorfilas > 1) {
                var filaAnt = contadorfilas - 2;

            }

            document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value = contadorfilas;
            document.getElementById('Codigo_' + valFila).focus();
        }
    }


    function UltimaFilaCompleta(fila, validar) {
        if ((fila >= 0) && (validar)) {
            var cod = document.getElementById('Codigo_' + fila);
            if (cod.value == '') {

                return false;
            }

        }

        return true;
    }

    function CargarDatos() {
        var str = '';
        for (var i = 0; i < contadorfilas; i++) {
            var nroFila = document.getElementById('NroFila_' + i);
            var cod = document.getElementById('Codigo_' + i);
            var tarea = document.getElementById('Tarea_' + i);
            var desde = document.getElementById('Desde_' + i);
            if (cod.value != '')
                str = str + nroFila.value + '#' + cod.value + '#' + tarea.value + '#' + desde.checked + '@';
        }
        document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;


    }

    function PasarFoco(Fila) {
        var fila = Fila.id.substr(8);
        document.getElementById('Codigo_' + fila).focus();
    }

    function CargarTarea(codigo) {
        var nroFila = codigo.name.replace('Codigo_', '');
        var tarea = document.getElementById('Tarea_' + nroFila);
        var sinMu = document.getElementById('Desde_' + nroFila);


        var lista = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtTareas").ClientID %>').value;

        if (codigo.value == '') {
            tarea.value = '';
        }
        else {


            if (verificarRepetidos(codigo, tarea)) {
                var i = lista.indexOf('#' + codigo.value + '#', 0);
                if (i < 0) {
                    codigo.value = '';
                    tarea.value = '';
                    alert('El codigo de analisis no existe o no es válido.');
                    document.getElementById('Codigo_' + nroFila).focus();

                }
                else {

                    if (!verificaDisponible(codigo)) {

                        alert('El codigo ' + codigo.value + ' no está disponible. Verifique con al administrador del sistema.');
                        codigo.value = '';
                        tarea.value = '';
                        document.getElementById('Codigo_' + nroFila).focus();
                    }
                    else {
                        var j = lista.indexOf('@', i);
                        i = lista.indexOf('#', i + 1) + 1;

                        //                        alert(i);alert(j);
                        tarea.value = lista.substring(i, j).replace('#True', '').replace('#False', '');

                        //  sinMu.checked= sinMuestra;
                        CargarDatos();
                        CrearFila(true);
                    }

                }
            }

        }
    }


    function verificaDisponible(objCodigo) {
        var devolver = true;
        var esnuevo = true;
        var listaDatos = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosCargados").ClientID %>').value;


        var sTabla1 = listaDatos.split(';');
        for (var i = 0; i < (sTabla1.length); i++) {
            var sItem = sTabla1[i].split('#');
            var valorCodigo = sItem[0];
            if (valorCodigo == objCodigo.value) {
                esnuevo = false; break;
            }
        }

        if (esnuevo) {         //no esta el codigo
            var listaItem = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtTareas").ClientID %>').value;
            var sTabla = listaItem.split('@');
            for (var i = 0; i < (sTabla.length - 1); i++) {
                var sFila = sTabla[i].split('#');
                if (sFila[1] != "") {
                    if (objCodigo.value == sFila[1]) {
                        if (sFila[3] == "False")// campo que indica si está disponible
                        {
                            devolver = false;
                            break;
                        }
                    }
                }
            }
        }
        return devolver;
    }


    function verificarRepetidos(objCodigo, objTarea) {
        ///Verifica si ya fue cargado en el txtDatos
        var devolver = true;
        var codigo = objCodigo.value;
        if (objTarea.value == '') {
            var listaExistente = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtDatos").ClientID %>').value;
            var cantidad = 1;
            var sTabla = listaExistente.split('@');
            for (var i = 0; i < (sTabla.length - 1); i++) {
                var sFila = sTabla[i].split('#');
                if (sFila[1] != "") {
                    if (codigo == sFila[1]) cantidad += 1;
                }

            }

            if (cantidad > 1) {
                objCodigo.value = '';
                objTarea.value = '';
                alert('El código ' + codigo + ' ya fue cargado. No se admiten analisis repetidos.');
                objCodigo.focus();
                devolver = false;
            }
            else
                devolver = true;
            ///Fin Verifica si ya fue cargado en el txtDatos
        }
        else
            devolver = true;

        return devolver;
    }


    function borrarfila(obj) {
        if (contadorfilas > 1) {
            var delRow = obj.parentNode.parentNode;
            var tbl = delRow.parentNode.parentNode;
            var rIndex = delRow.sectionRowIndex;
            Grilla = document.getElementById('Datos');
            Grilla.parentNode.deleteRow(rIndex);
            //alert('entra aca');
            OrdenarDatos();

            contadorfilas = contadorfilas - 1;
        }
        else {

            var cod = document.getElementById('Codigo_0').value = '';
            var tarea = document.getElementById('Tarea_0').value = '';
            var desde = document.getElementById('Desde_0').value = '';

            CargarDatos();
        }
    }



    function OrdenarDatos() {
        var pos = 0;
        var str = '';

        for (var i = 0; i < contadorfilas; i++) {
            var nroFila = document.getElementById('NroFila_' + i);

            if (nroFila != null) {
                nroFila.name = 'NroFila_' + pos;
                nroFila.id = 'NroFila_' + pos;
                nroFila.value = pos + 1;
                var cod = document.getElementById('Codigo_' + i);
                cod.name = 'Codigo_' + pos;
                cod.id = 'Codigo_' + pos;
                var tarea = document.getElementById('Tarea_' + i);
                tarea.name = 'Tarea_' + pos;
                tarea.id = 'Tarea_' + pos;
                var desde = document.getElementById('Desde_' + i);
                desde.name = 'Desde_' + pos;
                desde.id = 'Desde_' + pos;

                pos = pos + 1;
                str = str + nroFila.value + '#' + cod.value + '#' + tarea.value + '#' + desde.value + '@';
            }
        }
        document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;

    }

    function AgregarDeLaLista() {
        var elvalor = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigo").ClientID %>').value;
       
        if (elvalor != '') {
            var con = contadorfilas - 1;
            if (UltimaFilaCompleta(con, true)) {
                NuevaFila();
            }
            document.getElementById('Codigo_' + con).value = elvalor;
            CargarTarea(document.getElementById('Codigo_' + con));

            OrdenarDatos();
        }
    }


    function AgregarDeLaListaRutina() {
        var elvalor = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigosRutina").ClientID %>').value;
        if (elvalor != '') {
            var sTabla = elvalor.split(';');
            for (var i = 0; i < (sTabla.length); i++) {

                var valorCodigo = sTabla[i];
                var con = contadorfilas - 1;

                document.getElementById('Codigo_' + con).value = valorCodigo;
                CargarTarea(document.getElementById('Codigo_' + con));

            }

        }
    }


    function AgregarCargados() {
        //    alert('entra');
        CrearFila(true);
        var elvalor = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosCargados").ClientID %>').value;
        console.table(elvalor);
        if (elvalor != '') {
            var sTabla = elvalor.split(';');
            for (var i = 0; i < (sTabla.length); i++) {
                var sItem = sTabla[i].split('#');

                var valorCodigo = sItem[0];
                var sinMuestra = true;
                if (sItem[1] == 'No') sinMuestra = true;
                else sinMuestra = false;

                var con = contadorfilas - 1;
                document.getElementById('Codigo_' + con).value = valorCodigo;

                CargarTarea(document.getElementById('Codigo_' + con));
                var desde = document.getElementById('Desde_' + con);
                var boton = document.getElementById('boton_' + con);


                if (sItem[2] == 'True')
                    document.getElementById('Codigo_' + con).className = 'codigoConResultado';
                desde.checked = sinMuestra;


            }
        }
    }
        
        



    </script>
   </asp:Content>