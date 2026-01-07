<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TurnosEdit2.aspx.cs" Inherits="WebLab.Turnos.TurnosEdit2" MasterPageFile="~/Site1.Master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">


    <link rel="stylesheet" href="../App_Themes/default/style.css" />
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-ui.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'></script>
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>' />


    <script type="text/javascript">

        $(function () {
            $("#tabContainer").tabs();
            $("#tabContainer").tabs({ selected: 0 });
        });


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
    
    
    
    <style type="text/css">
        .auto-style3 {
            width: 738px;
        }
        .auto-style6 {
            font-size: 10pt;
            font-family: Calibri;
            background-color: #FFFFFF;
            color: #333333;
            font-weight: bold;
            height: 11px;
            width: 120px;
        }
        .auto-style7 {
            width: 120px;
        }
        .auto-style8 {
            width: 4px;
        }
        .auto-style9 {
            font-size: 10pt;
            font-family: Calibri;
            background-color: #FFFFFF;
            color: #333333;
            font-weight: bold;
            height: 11px;
            width: 332px;
        }
        .auto-style10 {
            height: 11px;
            width: 264px;
        }
    </style>
    
    
    
    </asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3><span class="label label-default">
        <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
    </span></h3>
    <br />
    <div align="left" style="width: 950px;" class="form-inline">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">


                    <asp:Label ID="lblPaciente" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="12pt" ForeColor="Black" Text="26982063 - PINTOS BEATRIZ CAROLINA"></asp:Label>
                    <br />
                    Fecha de Nacimiento:    
                    <asp:Label ID="lblFechaNacimiento" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="9pt" ForeColor="#333333"
                        Text="Label"></asp:Label>
                    <br />
                    <asp:Label ID="lblIdPaciente" runat="server" Text="Label"
                        Visible="False"></asp:Label>
                    Sexo:
                    <asp:Label ID="lblSexo" runat="server"
                        Text="Label" Font-Bold="True" Font-Names="Arial"
                        Font-Size="9pt" ForeColor="#333333"></asp:Label>
                    <br />
                    <asp:Label ID="lblAlerta" runat="server" CssClass="myLabelRojo" Text="Label"
                        Visible="False"></asp:Label>

                    <br />

                    <label>
                        Obra social:
                    </label>
                    <asp:Label ID="lblObraSocial" runat="server" Text="Label"></asp:Label>


                    <asp:LinkButton ID="btnSelObraSocial" runat="server" Visible="false" ToolTip="Cambiar O.S" OnClientClick="SelObraSocial(); return false;" OnClick="btnSelObraSocial_Click" Width="40px" CausesValidation="False">
                                             <span class="glyphicon glyphicon-search"></span></asp:LinkButton>
                    <anthem:Label ID="lblAlertaObraSocial" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC3300" Text="Label" Visible="false"></anthem:Label>

                    <br />
                    <label>Teléfono Contacto</label>:
                                                        <asp:TextBox ID="txtTelefono" runat="server" class="form-control input-sm" Width="200px"></asp:TextBox>
                    <br />
                </h3>
            </div>

            <div class="panel-body">

                <input id="hidToken" type="hidden" runat="server" />
                <table>

                    <tr>
                        <td class="style2">&nbsp;</td>
                        <td class="style10">Tipo de Servicio:</td>
                        <td class="style11">

                            <asp:Label ID="lblTipoServicio" runat="server" Text="Label"></asp:Label>

                            <asp:Label ID="lblIdTipoServicio" runat="server" Text="Label" Visible="False"></asp:Label>

                        </td>
                        <td class="style8">&nbsp;</td>
                        <td class="style9">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="style2">&nbsp;</td>
                        <td class="style10">Fecha</td>
                        <td class="style11">

                            <asp:Label ID="lblFecha" runat="server" Text="Label" Font-Bold="True" Font-Size="9pt"></asp:Label>

                            &nbsp;:
                                       
                                            <asp:Label ID="lblHora" runat="server" Text="Label" Font-Bold="True" Font-Size="9pt"></asp:Label>

                        </td>
                        <td class="style8">&nbsp;</td>
                        <td class="style9">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="style2">&nbsp;</td>
                        <td class="style10">Sector:</td>
                        <td class="style11">

                            <asp:DropDownList ID="ddlSectorServicio" runat="server" class="form-control input-sm"
                                TabIndex="13" Width="200px">
                            </asp:DropDownList>

                            <asp:RangeValidator ID="rvSectorServicio" runat="server"
                                ControlToValidate="ddlSectorServicio" ErrorMessage="Sector" MaximumValue="999999"
                                MinimumValue="1" Type="Integer" ValidationGroup="0">Dato requerido</asp:RangeValidator>

                        </td>
                        <td class="myLabelIzquierda"></td>
                        <td class="style9">

                            <%--  <asp:DropDownList ID="ddlEspecialista" runat="server" class="form-control input-sm"
                                                TabIndex="13" Width="200px">
                                                <asp:ListItem Value="1">No tiene</asp:ListItem>
                                            </asp:DropDownList>--%>
                            <br />


                        </td>
                        <td class="style3">&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="style2">&nbsp;</td>
                        <td class="style10">Médico Solicitante (MP):</td>
                        <td class="style11">

                            <anthem:TextBox ToolTip="Ingrese la mátricula" ID="txtEspecialista" Width="80px" TabIndex="5" class="form-control input-sm" runat="server" AutoCallBack="true" OnTextChanged="txtEspecialista_TextChanged"></anthem:TextBox>
                            <anthem:DropDownList ID="ddlEspecialista" runat="server" AutoCallBack="true"
                                TabIndex="6" class="form-control input-sm" Width="280px">
                            </anthem:DropDownList>


                            <asp:LinkButton ID="LinkButton1" Width="60px" CssClass="btn btn-primary" runat="server" OnClientClick="SelMedico(); return false;" OnClick="Button1_Click"> <span class="glyphicon glyphicon-zoom-in"></span></asp:LinkButton>
                            <anthem:Label ID="lblErrorMedico" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC3300" Text="Label" Visible="False"></anthem:Label>
                            <asp:HiddenField ID="hf_selMedico" runat="server" Value="" />


                        </td>
                        <td class="myLabelIzquierda">&nbsp;</td>
                        <td class="style9">&nbsp;</td>
                        <td class="style3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style2">&nbsp;</td>
                        <td class="style10" colspan="4">

                            <asp:CustomValidator ID="cvValidaPracticas" runat="server"
                                ErrorMessage="CustomValidator"
                                OnServerValidate="cvValidaPracticas_ServerValidate" ValidationGroup="0"></asp:CustomValidator>



                            <br />
                        </td>

                        <td align="left" class="style3">&nbsp;</td>

                    </tr>







                    <tr>



                        <td class="style2">&nbsp;</td>



                        <td colspan="4">

                            <div id="tabContainer" style="width: 100%; z-index: 100; position: relative;">
                                <ul class="nav nav-pills">
                                    <li><a href="#tab1">Analisis</a></li>
                                    <li><a href="#tab2">Diagnósticos</a></li>


                                </ul>

                                <div id="tab1" class="tab_content">

                                    <table width="100%">
                                        <tr>

                                            <td style="vertical-align: top">

                                                <table cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 10px; height: 13px"></td>
                                                        <td style="width: 50px;" class="tituloCelda">Codigo</td>
                                                        <td style="width: 350px;" class="tituloCelda">Descripcion</td>
                                                        <%--   <td style="width: 80px; " class="tituloCelda">
                                Sin Muestra</td>      --%>
                                                    </tr>
                                                </table>
                                                <div onkeydown="enterToTab(event)" style="width: 470px; height: 180pt; overflow: scroll; border: 1px solid #CCCCCC;">

                                                    <table class="mytablaIngreso" border="0" id="tabla" cellpadding="0" cellspacing="0">
                                                        <tbody id="Datos">
                                                        </tbody>
                                                    </table>

                                                    <input type="hidden" runat="server" name="TxtCantidadFilas" id="TxtCantidadFilas" value="0" />
                                                </div>


                                            </td>

                                            <td colspan="3" style="vertical-align: top">






                                                <anthem:DropDownList ID="ddlRutina" runat="server" AutoCallBack="True"
                                                    TextDuringCallBack="Cargando ..."
                                                    class="form-control input-sm" TabIndex="20"
                                                    OnSelectedIndexChanged="ddlRutina_SelectedIndexChanged" ToolTip="Rutina">
                                                </anthem:DropDownList>
                                                <br />
                                                <input id="Button2" type="button" value="Agregar Rutina"
                                                    onclick="AgregarDeLaListaRutina();"
                                                    title="Agrega la rutina seleccionada a la lista" style="width: 160px" />

                                                <br />
                                                <anthem:DropDownList ID="ddlItem" runat="server" AutoCallBack="True"
                                                    OnSelectedIndexChanged="ddlItem_SelectedIndexChanged"
                                                    TextDuringCallBack="Cargando  ..." Width="150px"
                                                    class="form-control input-sm" TabIndex="20" ToolTip="Analisis">
                                                </anthem:DropDownList>
                                                <br />
                                                <input id="Button1" type="button" value="Agregar Analisis"
                                                    onclick="AgregarDeLaLista();" style="width: 160px"
                                                    title="Agrega el analisis seleccionado a la lista" />



                                            </td>
                                        </tr>
                                    </table>
                                </div>





                                              	
                            <div id="tab2" class="tab_content">
					<table width="100%">
					<tr>
						
						<td style="vertical-align: top" align="right">
						
						    &nbsp;</td>
						
						<td   colspan="3" style="vertical-align: top">
						
						       <fieldset id="Fieldset2"  
                                >
                                <legend class="myLabelIzquierda">Diagnósticos Presuntivos - Codificación CIE 10</legend>
                             
                                    <table class="auto-style3">
                                         <tr>
                                          <td class="myLabelIzquierda" colspan="3"  >
                                                 Codigo: &nbsp; &nbsp;
                                                 <anthem:TextBox ID="txtCodigoDiagnostico" runat="server" AutoCallBack="True" 
                                                     CssClass="form-control input-sm" ontextchanged="txtCodigoDiagnostico_TextChanged" Height="22px"></anthem:TextBox>
                                                 <br />
                                                 Nombre: &nbsp;<anthem:TextBox ID="txtNombreDiagnostico" runat="server" AutoCallBack="True" 
                                                     CssClass="form-control input-sm" ontextchanged="txtNombreDiagnostico_TextChanged" 
                                                   Width="268px"></anthem:TextBox>
                                           
                                                 <anthem:Button ID="btnBusquedaDiagnostico" runat="server" CssClass="btn btn-primary" onclick="btnBusquedaDiagnostico_Click" Text="Buscar" Width="90px" />                                                 
                                                
                                                  <anthem:Button ID="btnBusquedaFrecuente" runat="server" CssClass="btn btn-danger" onclick="btnBusquedaFrecuente_Click" Text="Ver Frecuentes" Width="120px" /></td>                                     
                                         </tr>
                                         <tr>
                                           <td class="auto-style9" colspan="3" >
                                               Diagnosticos encontrados</td>
                                                                                
                                         </tr>
                                         <tr>
                                           <td class="myLabelIzquierda" colspan="2"  >
                                               <anthem:ListBox ID="lstDiagnosticos" runat="server" AutoCallBack="True" 
                                                   CssClass="form-control input-sm" Height="150px" Width="800px">
                                               </anthem:ListBox>
                                             </td>
                                             <td class="auto-style7">
                                                 <anthem:ImageButton ID="btnAgregarDiagnostico" runat="server" 
                                                     ImageUrl="~/App_Themes/default/images/añadir.jpg" 
                                                     onclick="btnAgregarDiagnostico_Click1" 
                                                     ToolTip="Agregar a la lista de Diagnosticos del Paciente" /><br />
                                                    
                                                 <anthem:ImageButton ID="btnSacarDiagnostico" runat="server" 
                                                     ImageUrl="~/App_Themes/default/images/sacar.jpg" 
                                                     onclick="btnSacarDiagnostico_Click"
                                                        ToolTip="Quitar diagnóstico de la lista de Diagnosticos del Paciente"
                                                      />
                                             </td>                                         
                                                                           
                                         </tr>
                                         <tr>
                                           <td class="myLabelIzquierda" colspan="3"  >
                                                 Diagnosticos del Paciente<br />
                                                 <anthem:ListBox ID="lstDiagnosticosFinal" runat="server" CssClass="form-control input-sm" 
                                                     Height="150px" Width="800px" SelectionMode="Multiple" 
                                                     ToolTip="Sacar de la lista de Diagnosticos del Paciente">
                                                 </anthem:ListBox>
                                             </td>
                                                                               
                                                                              
                                         </tr>
                                      </table>

                            
                                 </fieldset>
           
              </td>
						
						<td style="vertical-align: top">
						
						    &nbsp;</td>
						
					</tr>
					
				
					</table>
					   </div>
					  </div>
                                            
                                            </td>
						
						
						
						<td class="style3">
                                  


                    </tr>




                    <tr>



                        <td class="style2">&nbsp;</td>



                        <td colspan="4">

                            <input type="hidden" runat="server" name="TxtDatos" id="TxtDatos" value="" />
                            <input id="txtTareas" name="txtTareas" runat="server" type="hidden" />
                            &nbsp;   
                            <asp:CheckBox ID="chkImprimir" runat="server" CssClass="myLabel"
                                Text="Imprimir comprobante" />
                            <input type="hidden" runat="server" name="CodOS" id="CodOS" value="0" />

                        </td>



                        <td class="style3">
                            <div style="width: 95%" align="left">
                                <anthem:TextBox
                                    ID="txtCodigo" runat="server" BorderColor="White" ForeColor="White"
                                    BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>
                                <anthem:TextBox
                                    ID="txtCodigosRutina" runat="server" BorderColor="White"
                                    ForeColor="White" BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>
                            </div>
                            &nbsp;</td>



                    </tr>



                </table>

            </div>

            <div class="panel-footer">
                <table width="100%">
                    <tr>





                        <td align="left">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-primary"
                                OnClick="btnCancelar_Click" Width="100px" TabIndex="24"
                                CausesValidation="False" />
                        </td>



                        <td align="right">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0"
                                OnClick="btnGuardar_Click" CssClass="btn btn-primary" Width="100px" TabIndex="24"
                                ToolTip="Guarda los datos cargados para el turno" />



                        </td>



                    </tr>






                    <tr>



                        <td class="style2">&nbsp;</td>



                        <td colspan="4">
                            <asp:LinkButton CssClass="myLink" Visible="false" ID="lnkReimprimirComprobante" OnClick="lnkReimprimirComprobante_Click" runat="server">Reimprimir Comprobante</asp:LinkButton></td>



                        <td class="style3">&nbsp;</td>



                    </tr>


                    <tr>



                        <td class="style2">&nbsp;</td>



                        <td colspan="4">


                            <asp:Panel ID="pnlImpresora" runat="server" Style="border: 1px solid #C0C0C0; width: 100%; background-color: #EFEFEF;">


                                <table width="720px" align="center">
                                    <tr>
                                        <td class="myLabelIzquierda" align="left" style="width: 140px; background-color: #EFEFEF;">Impresora del sistema: </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlImpresora" runat="server" CssClass="myList">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                </table>

                            </asp:Panel>
                        </td>



                        <td class="style3">&nbsp;</td>



                    </tr>









                </table>





            </div>


        </div>


        <script language="javascript" type="text/javascript">
            var contadorfilas = parseInt(document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value);
            InicioPagina();
            document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value = contadorfilas;
            //function VerificaLargo (source, arguments)
            //    {                
            //    var Observacion = arguments.Value.toString().length;
            // //   alert(Observacion);
            //    if (Observacion>4000 )
            //        arguments.IsValid=false;    
            //    else   
            //        arguments.IsValid=true;    //Si llego hasta aqui entonces la validación fue exitosa        
            //}








            function InicioPagina() {
                const esPostBack = esPostBackSelMedico();
                const TxtDatos = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value;
                const validatorSpan = esPostBackValidacion();

                //  console.log("TxtDatos", TxtDatos);
                //console.log("esPostBack", esPostBack);
               // console.log("validatorSpan", validatorSpan);

                if (!esPostBack && !validatorSpan) {
                    if (TxtDatos == "") {
                        CrearFila(true);
                    }
                    else {
                        CargarDetalles(); OrdenarDatos();
                    }
                } else {
                    CargarDetalles(true);
                    OrdenarDatos();
                }
            }

            function NuevaFila(indice) {
                var filaIndex = (typeof indice !== 'undefined') ? indice : contadorfilas;
                Grilla = document.getElementById('Datos');


                fila = document.createElement('tr');
                fila.id = 'cod_' + filaIndex;
                fila.name = 'cod_' + filaIndex;

                celda1 = document.createElement('td');
                oNroFila = document.createElement('input');
                oNroFila.type = 'text';
                oNroFila.readOnly = true;

                oNroFila.runat = 'server';
                oNroFila.name = 'NroFila_' + filaIndex;
                oNroFila.id = 'NroFila_' + filaIndex;
                //oNroFila.onfocus= function() {PasarFoco(this)}
                oNroFila.className = 'fila';
                celda1.appendChild(oNroFila);
                fila.appendChild(celda1);

                celda2 = document.createElement('td');
                oCodigo = document.createElement('input');
                oCodigo.type = 'text';
                oCodigo.runat = 'server';
                oCodigo.name = 'Codigo_' + filaIndex;
                oCodigo.id = 'Codigo_' + filaIndex;
                oCodigo.className = 'codigo';

                oCodigo.onblur = function () { CargarTarea(this); };
                oCodigo.setAttribute("onkeypress", "javascript:return Enter(this, event)");
                //oCodigo.onchange = function () {CargarDatos()};
                celda2.appendChild(oCodigo);
                fila.appendChild(celda2);

                celda3 = document.createElement('td');
                oTarea = document.createElement('input');
                oTarea.type = 'text';
                oTarea.readOnly = true;
                oTarea.runat = 'server';
                oTarea.name = 'Tarea_' + filaIndex;
                oTarea.id = 'Tarea_' + filaIndex;
                oTarea.className = 'descripcion';
                oTarea.onchange = function () { CargarDatos() };
                celda3.appendChild(oTarea);
                fila.appendChild(celda3);

                //    	 


                celda6 = document.createElement('td');
                oBoton = document.createElement('input');
                oBoton.className = 'boton';
                oBoton.type = 'button';
                oBoton.value = 'X';
                oBoton.onclick = function () { borrarfila(this) };
                celda6.appendChild(oBoton);
                fila.appendChild(celda6);

                Grilla.appendChild(fila);
                filaIndex = filaIndex + 1;

                return filaIndex;
            }

            function CrearFila(validar, esPostBack = false, indice = 0) {
                if (esPostBack) {
                    var valFila = indice;
                    NuevaFila(valFila);
                    document.getElementById('NroFila_' + valFila).value = valFila + 1;
                    document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value = valFila + 1;
                    document.getElementById('Codigo_' + valFila).focus();
                } else {
                    var valFila = contadorfilas - 1;
                    if (UltimaFilaCompleta(valFila, validar)) {
                        contadorfilas = NuevaFila();
                        valFila = contadorfilas - 1;
                        document.getElementById('NroFila_' + valFila).value = contadorfilas;
                        document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value = contadorfilas;
                        document.getElementById('Codigo_' + valFila).focus();
                    }
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
                    //console.log("-------------");
                    //console.log("Nueva fila, i", i);
                    //console.log("Nueva fila, contador filas", contadorfilas);


                    var nroFila = document.getElementById('NroFila_' + i);
                    //console.log("nroFila", nroFila);
                    var cod = document.getElementById('Codigo_' + i);
                    //console.log("cod", cod);
                    var tarea = document.getElementById('Tarea_' + i);
                    //console.log("tarea", tarea);
                    //console.log("-------------");

                    if (cod.value != '')
                        str = str + nroFila.value + '#' + cod.value + '#' + tarea.value + '@';
                    //		        
                }

                document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;


            }

            function PasarFoco(Fila) {
                var fila = Fila.id.substr(8);

                document.getElementById('Codigo_' + fila).focus();
            }

            function CargarTarea(codigo) {

                //console.log("CargarTarea : codigo", codigo)
                var nroFila = codigo.name.replace('Codigo_', '');
                var tarea = document.getElementById('Tarea_' + nroFila);

                var lista = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtTareas").ClientID %>').value;

                //         

                if (codigo.value == '') {
                    tarea.value = '';
                }
                else {
                    if (verificarRepetidos(codigo, tarea)) {
                        var i = lista.indexOf('#' + codigo.value + '#', 0);

                        if (i < 0) {
                            codigo.value = '';
                            tarea.value = '';
                            alert('El codigo de tarea no existe.');
                            document.getElementById('Codigo_' + nroFila).focus();
                        }
                        else {
                            var j = lista.indexOf('@', i);
                            i = lista.indexOf('#', i + 1) + 1;
                            tarea.value = lista.substring(i, j);
                            CargarDatos();
                            CrearFila(true);
                        }
                    }
                }
            }

            function borrarfila(obj) {
                if (contadorfilas > 1) {
                    var delRow = obj.parentNode.parentNode;
                    var tbl = delRow.parentNode.parentNode;
                    var rIndex = delRow.sectionRowIndex;
                    Grilla = document.getElementById('Datos');
                    Grilla.parentNode.deleteRow(rIndex);

                    OrdenarDatos();

                    contadorfilas = contadorfilas - 1;
                }
                else {

                    var cod = document.getElementById('Codigo_0').value = '';
                    var tarea = document.getElementById('Tarea_0').value = '';


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


                        pos = pos + 1;


                        str = str + nroFila.value + '#' + cod.value + '#' + tarea.value + '@';
                    }
                }


                document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;

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
                            if (codigo == sFila[1])
                                cantidad += 1;
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

            function AgregarDeLaLista() {
                var elvalor = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigo").ClientID %>').value;
                if (elvalor != '') {
                    var con = contadorfilas - 1;
                    if (UltimaFilaCompleta(con, true)) {
                        NuevaFila();
                    }
                    document.getElementById('Codigo_' + con).value = elvalor;
                    CargarTarea(document.getElementById('Codigo_' + con));
                    //                CargarDatos();
                    //                NuevaFila();
                    OrdenarDatos();
                }
            }


            function AgregarDeLaListaRutina() {
                var elvalor = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigosRutina").ClientID %>').value;
                //alert(elvalor);
                if (elvalor != '') {
                    var sTabla = elvalor.split(';');
                    for (var i = 0; i < (sTabla.length); i++) {
                        var valorCodigo = sTabla[i];
                        var con = contadorfilas - 1;
                        if (UltimaFilaCompleta(con, true)) {
                            NuevaFila();
                        }
                        document.getElementById('Codigo_' + con).value = valorCodigo;
                        CargarTarea(document.getElementById('Codigo_' + con));
                        //                    CargarDatos();
                        //                    NuevaFila();
                    }
                    OrdenarDatos();
                }
            }


            function CargarDetalles(esPostBack = false) {
                var str = '';
                var detalles = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value
                var sTabla = detalles.split('@');

                for (var i = 0; i < (sTabla.length - 1); i++) {
                    var sFila = sTabla[i].split('#');

                    if (sFila[1] != "") {
                        CrearFila(false, esPostBack, i);
                        var nroFila = document.getElementById('NroFila_' + i);
                        console.log("nrofila", nroFila);
                        nroFila.value = sFila[0];
                        var cod = document.getElementById('Codigo_' + i);
                        cod.value = sFila[1];
                        var tarea = document.getElementById('Tarea_' + i);
                        tarea.value = sFila[2];

                        str = str + nroFila.value + '#' + cod.value + '#' + tarea.value + '@';
                    }
                }
                //if (esPostBack && i == (sTabla.length - 1)) {
                //    CrearFila(false, esPostBack, i-1);
                //}
                CrearFila(false, esPostBack, contadorfilas - 1);

                document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;


            }

            function SelMedico() {
                var abrioPopUp = document.getElementById("<%= hf_selMedico.ClientID %>").value = 'Si';
                var dom = document.domain;
                var domArray = dom.split('.');
                for (var i = domArray.length - 1; i >= 0; i--) {
                    try {
                        var dom = '';
                        for (var j = domArray.length - 1; j >= i; j--) {
                            dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                        }
                        document.domain = dom;
                        break;
                    } catch (E) {
                    }
                }


                var $this = $(this);
                $('<iframe src="../Protocolos/MedicoSel.aspx?id=' + idPaciente + '" />').dialog({
                    title: 'Buscar Médico',
                    autoOpen: true,
                    width: 620,
                    height: 400,
                    modal: true,
                    resizable: false,
                    autoResize: true,
                    open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },

                    buttons: {
                        'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.LinkButton1))%>; }
                    },
                    overlay: {
                        opacity: 0.5,
                        background: "black"
                    }
                }).width(600);
            }

            var idPaciente = $("#<%= lblIdPaciente.ID %>").val();
            function SelObraSocial() {
                var dom = document.domain;
                var domArray = dom.split('.');
                for (var i = domArray.length - 1; i >= 0; i--) {
                    try {
                        var dom = '';
                        for (var j = domArray.length - 1; j >= i; j--) {
                            dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                        }
                        document.domain = dom;
                        break;
                    } catch (E) {
                    }
                }


                var $this = $(this);
                $('<iframe src="../Protocolos/ObraSocialSel.aspx?idPaciente=' + idPaciente + '" />').dialog({
                    title: 'Financiador del Protocolo',
                    autoOpen: true,
                    width: 550,
                    height: 400,
                    modal: true,
                    resizable: false,
                    autoResize: true,
                    buttons: {
                        'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnSelObraSocial))%>; }
                    },
                    overlay: {
                        opacity: 0.5,
                        background: "black"
                    }
                }).width(600);
            }
            function Button1_onclick() {

            }
            function CargarDetallesDinamico() {
                var detalles = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value
            }
            function esPostBackSelMedico() {

                var abrioPopUp = document.getElementById("<%= hf_selMedico.ClientID %>").value;
                if (abrioPopUp == 'Si') {
                    // despues de verificar que ingreso lo vuelvo a un estado vacio
                    abrioPopUp = "";
                    return true;
                }
                else {
                    return false;
                }
            }

            function esPostBackValidacion() {
                var validatorSpan = document.getElementById('<%= cvValidaPracticas.ClientID %>');
                var mensaje = validatorSpan.innerText;
                if (mensaje != '') {
                    return true;
                } else {
                    return false;
                }
            }
        </script>
    </div>
</asp:Content>
