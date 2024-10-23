<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="CasoResultado2.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoResultado2" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    

         <style type="text/css">  
        body  
        {  
            font-family: Arial;  
            font-size: 10pt;  
        }  
        .table  
        {  
            /*border: 1px solid #ccc;  
            border-collapse: collapse;  
            width: 200px;*/  
        }  
        .table th  
        {  
            background-color: #F7F7F7;  
            color: #333;  
            font-weight: bold;  
        }  
        .table th, .table td  
        {  
            padding: 5px;  
            border: 1px solid #ccc;  
        }  
    
    * {box-sizing: border-box}
body {font-family: "Lato", sans-serif;}

/* Style the tab */
.tab {
  float: left;
  border: 1px solid #ccc;
  background-color: #f1f1f1;
  width: 30%;
  height: 300px;
}

/* Style the buttons inside the tab */
.tab button {
  display: block;
  background-color: inherit;
  color: black;
  padding: 22px 16px;
  width: 100%;
  border: none;
  outline: none;
  text-align: left;
  cursor: pointer;
  transition: 0.3s;
  font-size: 17px;
}

/* Change background color of buttons on hover */
.tab button:hover {
  background-color: #ddd;
}

/* Create an active/current "tab button" class */
.tab button.active {
  background-color: #ccc;
}

/* Style the tab content */
.tabcontent {
  float: left;
  padding: 0px 12px;
  border: 1px solid #ccc;
  width: 70%;
  border-left: none;
  height: 300px;
}
      </style>
      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
    <link rel="stylesheet" href="https://unpkg.com/materialize-stepper@3.1.0/dist/css/mstepper.min.css" />
  <script type="text/javascript" src="../script/ValidaFecha.js"></script>        
     
<script type="text/javascript">
 <%-- $(function() {

                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                      
                        $("#tabContainer").tabs({ selected: currTab });
  });--%>

    function openCity(evt, cityName) {
        var i, tabcontent, tablinks;
        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }
        document.getElementById(cityName).style.display = "block";
        evt.currentTarget.className += " active";
    }

    // Get the element with id="defaultOpen" and click on it
    document.getElementById("defaultOpen").click();
     
</script>
   
  
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
      <input runat="server"  type="hidden" id="id"/>
                             <input runat="server"  type="hidden" id="Desde"/>
<div class="tab">
  <button class="tablinks" onclick="openCity(event, 'Muestras')" id="defaultOpen">Muestras</button>
  <button class="tablinks" onclick="openCity(event, 'Metodos')">Metodos</button>
  <button class="tablinks" onclick="openCity(event, 'Resultados')">Resultados</button>
</div>
<div id="Muestras" class="tabcontent">
     <table  width="100%" >
				
			 
					 
         	<tr>
						<td   >
                            &nbsp;</td>
						<td align="right">
                               <asp:LinkButton ID="ValidaMuestra" runat="server" Text="Valida Muestras" Width="20px" OnClick="ValidaMuestra_Click" ValidationGroup="0"  ></asp:LinkButton>
</td>
						
					</tr>
				
			 
					 
         	<tr>
						<td   >
                        <h4>    <asp:Label ID="Label1" runat="server" Text="Tipo de Caso"></asp:Label></h4>
                        </td>
						<td align="left" >
                         <h4>      <asp:Label ID="lblTipoCaso" runat="server" 
                                 Font-Bold="False"></asp:Label> </h4> </td>
						
					</tr>
					<tr>
						<td   ><h4>Titulo del caso:</h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="1" ToolTip="Ingrese el titulo"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td   ><h4>Entidad solicitante:<asp:RequiredFieldValidator ID="rfv0" runat="server" ControlToValidate="txtSolicitante" ErrorMessage="*" ValidationGroup="0"></asp:RequiredFieldValidator>
                            </h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtSolicitante" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="2"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td   ><h4>Autos:</h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtAutos" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="3"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td  ><h4>Objetivo del análisis:<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtObjetivo" ErrorMessage="Objeto" ValidationGroup="0"></asp:RequiredFieldValidator>
                            </h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtObjetivo" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="4">Determinación de vínculo de parentesco.</asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >&nbsp;</td>
						<td align="left" >
                           
                        </td>
						
					</tr>
					<tr>
						<td colspan="2"   ><h4>Muestras analizadas</h4>

						</td>
						
					</tr>
					<tr>
						<td colspan="2">   
						
                                 <asp:DataList ID="dlForense" runat="server"    
        CellSpacing="3" RepeatLayout="Table">  
        <ItemTemplate>  
            <table class="table">  
                <tr> 
                     
                    <th colspan="4">  
                        Muestra:
                        <b>  
                            <%# Eval("muestra") %></b>:  <%# Eval("muestraDetalle") %>  
                    </th>  
               
                </tr>  
                
                <tr>  
                    <td>  
                       Cadena de Custodia:
                    </td>  
                    <td>  
                     <%# Eval("numeroOrigen")%>  
                    </td>  
                      <td  >  
                     Protocolo:</td>
                        <td  >  
                    <%# Eval("Protocolo")%>   
                    </td>  
                </tr>  
               
            </table>  
        </ItemTemplate>  
    </asp:DataList> 
                        

                             <asp:DataList ID="dlMuestras" runat="server"    
        CellSpacing="3" RepeatLayout="Table" >  
        <ItemTemplate>  
            <table class="table">  
                <tr>  
                    <th colspan="3">  
                        <b>  
                            <%# Eval("Persona") %></b>
                        <asp:LinkButton PostBackUrl= '<%# "../Protocolos/ProtocoloEditForense.aspx?idServicio=6&Desde=Caso&Operacion=Modifica&idProtocolo=" +  Eval("idProtocolo")%>' ID="Editar" runat="server" Text="" Width="20px"  >
                                             <span class="glyphicon glyphicon-pencil">Editar</span></asp:LinkButton>
                                                                         
                                                                
                    <td rowspan="5">
                          
                      <div class="thumbnail" >
<asp:Image ID="Image1" runat="server" ImageUrl='<%# "../imagen.ashx?idPaciente="+ Eval("idPaciente") %>' /> 
                             <asp:Image ID="img" runat="server"            />   
                             </div>
                     
                    </td>
                </tr>  
                <tr>  
                    <td colspan="3">  
                        <%# Eval("Parentesco") %> 
                      
                    </td>  
                  
                </tr>  
                <tr>  
                    <td>  
                         <%# Eval("numerodocumento")%>  
                    </td>  
                    <td>  
                      Protocolo Nro:  <%# Eval("Protocolo")%>  
                    </td>  
                        <td colspan="1">  
                      Fecha y Lugar de toma:  <%# Eval("fechatoma")%>  ,   <%# Eval("lugar")%>  
                    </td>  
                </tr>  
               <tr>  
                    <td colspan="3">  
                      
                        Muestra:    <%# Eval("Muestra") %>
                    </td>  
                </tr>  
            </table>  
        </ItemTemplate>  
    </asp:DataList> 
                            

                            <asp:Button ID="btnMuestras" runat="server" Text="Agregar/Quitar Muestras" OnClick="btnMuestras_Click" CssClass="btn btn-success" Width="240px" />
                                <br />

                                </td>
						
					</tr>

         <tr>
						<td colspan="2">   
                          
                            </td>
             </tr>

         </table>
        </div>

<div id="Metodos" class="tabcontent">
  <h3>Paris</h3>
  <p>Paris is the capital of France.</p> 
</div>

<div id="Resultados" class="tabcontent">
  <h3>Tokyo</h3>
  <p>Tokyo is the capital of Japan.</p>
</div>
 
      
    
    <br /> <asp:HiddenField runat="server" ID="HFCurrTabIndex" Value="0"   /> 
    <br /> <br /> 
   <div class="form-group"  >
   <div class="panel panel-default" style="width:80%">
                    <div class="panel-heading">
                        <h3 class="panel-title">INFORME DE RESULTADOS <asp:Label ID="lblNumero" runat="server" Text=" CASO NRO."></asp:Label> <asp:Label ID="lblTitulo" runat="server" 
                                 Font-Bold="False"></asp:Label></h3>
                        <!-- Horizontal Steppers -->
                     
<!-- /.Horizontal Steppers -->
                        
                        </div>
       	<div class="panel-body">	
 <span class="glyphicon glyphicon-ok" id="icobasefa" runat="server"> </span>
                  	<span class="glyphicon glyphicon-ok" id="icoresultado" runat="server"> </span>
   <span class="glyphicon glyphicon-ok" id="icomuestra" runat="server"> </span> <span class="glyphicon glyphicon-ok" id="icobibliografia" runat="server"> </span>
<span class="glyphicon glyphicon-ok" id="icometodo" runat="server"> </span><span class="glyphicon glyphicon-ok" id="icomarcador" runat="server"> </span>
		


 <%--   <div id="tab1" class="tab_content" style="border: 1px solid #C0C0C0">--%>
        

              <div id="tab4"   style="border: 1px solid #C0C0C0; ">


                   <div class="form-group">
    <label for="exampleInputEmail1">Método de extracción:</label>
    <asp:TextBox ID="txtMetodo" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="8" MaxLength="500" TextMode="MultiLine">Papel de filtro lavado (1)</asp:TextBox>
  </div>


     <table width="100%" >
					<tr>
						<td colspan="2"><h3> Métodos:<asp:LinkButton ID="Valida1" runat="server" Text="Validar" Width="20px" OnClick="Valida1_Click" ValidationGroup="1"  ></asp:LinkButton>
                            </h3></td>
						
						
						
					</tr>
					<tr>
						<td> <h4>Método de extracción:</h4></td>
						
						<td>
                           
                            </td>
						
					</tr>
					<tr>
						<td> 
                            <h4>Cuantificación:</h4>
                        </td>
						
						<td align="left">
                            <asp:TextBox ID="txtCuantificacion" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="9" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                            </td>
						
					</tr>
					<tr>
						<td><h4> Amplificación:</h4></td>
						
							<td align="left">
                            <asp:TextBox ID="txtAmplificacion" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="10" ToolTip="Amplificación" MaxLength="500" TextMode="MultiLine">PowerPlex Fusion(Promega) (2)</asp:TextBox>
                            </td>
						
					</tr>
					<tr>
						<td><h4>Análisis de fragmentos:</h4></td>
						
							<td align="left">
                            

                            <asp:TextBox ID="txtAnalisis" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="11" ToolTip="Análisis de fragmentos" MaxLength="500" TextMode="MultiLine">Equipo ABI Prism 310</asp:TextBox>
                               

                        </td>
						
					</tr>
					<tr>
						<td>
                          <h4>  Software de análisis:</h4></td>
						
							<td align="left">
                            <asp:TextBox ID="txtSoftware" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="12" ToolTip="Software de análisis" MaxLength="500" TextMode="MultiLine">GeneMapper ID-X 1.4</asp:TextBox>
                            </td>
						
					</tr>
					<tr>
						<td><h4>Análisis estadístico: </h4></td>
						
							<td align="left">
                            <asp:TextBox ID="txtEstadistico" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="13" ToolTip="Análisis estadístico" MaxLength="500" TextMode="MultiLine">Software Familias versión 3.1.8 (3)</asp:TextBox>
                            </td>
						
					</tr>
					<tr>
						<td><h4> Marco de estudio estadístico:</h4> </td>
						
							<td align="left">
                            <asp:TextBox ID="txtMarcoEstudio" runat="server" class="form-control input-sm" Width="90%"
                                TabIndex="14" ToolTip="Marco de estudio estadístico" TextMode="MultiLine" Rows="8" MaxLength="5000">Hipótesis planteadas:
A.- ..... es el padre biológico de .............. siendo .............. la madre.
B.- El padre biológico de ............. es un individuo al azar de la población, no relacionado con ................., siendo ................ la madre. </asp:TextBox>
                            </td>
						
					</tr>
					
				
         </table>
                         </div>
              <div id="tab3"   style="border: 1px solid #C0C0C0">
     <table align="center" width="100%">
					<tr>
						<td><h4>Resultados</h4></td>
						
						<td>
                           
                            <asp:LinkButton ID="Valida2" runat="server" Text="Validar" Width="20px" OnClick="Valida2_Click" ValidationGroup="2"  ></asp:LinkButton>
                           
                            </td>
						
					</tr>
					<tr>
						<td colspan="2">
                             <asp:TextBox ID="txtResultado" runat="server" class="form-control input-sm" Width="95%" 
                               TextMode="MultiLine" Rows="3" TabIndex="5" ToolTip="Ingrese el resultado">Ver tabla 1. Se observan incompatibilidades en los siguientes marcadores: </asp:TextBox>

                            <br />
                        
                            


						</td>
						
						
						
					</tr>
				

					<tr>
						<td>
                            <h4>
                                 Conclusiones:</h4>

                           
                        </td>
						
						<td>&nbsp;</td>
						
					</tr>
					<tr>
						<td colspan="2">
                            <asp:TextBox ID="txtConclusion" runat="server" class="form-control input-sm" Width="95%" 
                                TabIndex="7" TextMode="MultiLine" Rows="16">Los resultados obtenidos son compatibles con la existencia de vínculo biológico de paternidad del Sr. ....... respecto de......., siendo ...... la madre biológica. El Índice de Paternidad (IP) obtenido es ....... Este número indica que es ............. veces más probable obtener este resultado si........... es el padre biológico de ....., que si lo fuera cualquier otro hombre no relacionado y tomado al azar de la población.

Luego del análisis de las muestras se excluye el vínculo de paternidad entre ....................... y .................., siendo el mismo, hijo de ........ </asp:TextBox>
                            <%# Eval("numeroOrigen")%>
                            </td>
						
					</tr>
					<tr>
						<td class="auto-style1"></td>
						
						<td class="auto-style1"></td>
						
					</tr>

         </table>
                  </div>
                     
              <div id="tab6"  >
                   
                        
                            <asp:TextBox ID="txtBibliografia" runat="server" class="form-control input-sm" Width="90%"
                                TabIndex="15" ToolTip="BIBLIOGRAFIA" TextMode="MultiLine" Rows="12">•	Frecuencias alélicas: Update of an on-line autosomal STR and Y-STR reference database of Argentina. Evguenia Alechine, Miguel Marino, Andrea Sala, Maria Cecilia Bobillo, Mariela Caputo and Daniel Corach. Forensic Science International: Genetics Supplement Series. Volume 2, Issue 1 2009, 382- 383.
•	Pautas y normas mínimas aprobadas por la Sociedad Argentina de Genética Forense para los informes de paternidad mediante análisis de ADN.
•	(1) Optimizing direct amplification of forensic commercial kits for STR Determination M. Caputo, M.C. Bobillo, A. Sala, D. Corach. Journal of Forensic and Legal Medicine 47 (2017) 17-23.
•	(2) TECHNICAL MANUAL-PowerPlex® Fusion System. Instructions for Use of Products DC2402 and DC2408
•	(3)Manual for Familias 3. Daniel Kling, Petter Mostad, Thore Egeland. 2014 </asp:TextBox>
                            
                            <asp:LinkButton ID="Valida3" runat="server" Text="Validar" Width="20px" OnClick="Valida3_Click" ValidationGroup="3"  ></asp:LinkButton>
                            
              </div>
       
              <div id="tab2"     style="border: 1px solid #C0C0C0"> 
                   <asp:Label ID="estatus" runat="server"   Style="color: #0000FF"></asp:Label>
                  <div id="divInputLoad">
                                <div id="divFileUpload">
                     <h3> Adjuntar imagen:</h3>
                       <asp:FileUpload Width="400px" ID="trepadorFoto" runat="server" accept="image/*"   class="form-control input-sm" onchange="readFile(this)"  />  
                       <asp:RegularExpressionValidator ID="RegExValFileUploadFileType" runat="server"
                        ControlToValidate="trepadorFoto"
                        ErrorMessage="el archivo debe ser en formato jpg " Font-Bold="True"
                        Font-Size="Medium"
                        ValidationExpression="(.*?)\.(jpg|JPG)$" ValidationGroup="0" Enabled="False"></asp:RegularExpressionValidator>

                                    </div>                     
             

                                <div id="file-preview-zone">
                           
                                    </div>
          
                                <div>
                                <anthem:Image ID="Image1" runat="server" Visible="False"  />

                                    <anthem:Button ID="btnBorrarImg" CssClass="btn btn-primary" runat="server" OnClick="btnBorrarImg_Click" Text="Borrar Img." Visible="False" Width="100px" AutoUpdateAfterCallBack="True" CausesValidation="False" />
            </div>
               </div>    

          
              <hr />
                  <asp:Panel runat="server" ID="pnlMarcadoresFiliacion" Visible="true">
                  <h3>Importar tablas</h3>
                  <table>
                      <tr>
                          <td> Archivo Genotipos:</td>
                          <td>  <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm"  /></td>
                          <td> <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="200px" 
                    Text="Procesar Archivo" OnClick="subir_Click" /></td>
                      </tr>
                      <tr>
                          <td colspan="3">
                              <br />
                              </td>
                          </tr>
                      <tr>
                          <td>  Archivo LR:</td>
                          <td> <asp:FileUpload ID="trepador2" runat="server" class="form-control input-sm"  /></td>
                          <td>  <asp:Button  CssClass="btn btn-primary" ID="subir0" runat="server" Width="200px" 
                    Text="Procesar Archivo" OnClick="subir0_Click" /></td>
                      </tr>
                      <tr>
                          <td colspan="3">
  <div>
                <p>El archivo debe ser de tipo cvs o txt</p>
               <asp:Label ID="estatus1" runat="server" 
                    Style="color:red"></asp:Label>
            </div>
                            <div>

                            <asp:GridView ID="gvTablaForense" runat="server" Font-Names="Verdana" Font-Size="12pt" EmptyDataText="No se encontraron datos de los protocolos del caso"></asp:GridView>
                                <asp:Button ID="btnBorrarTablaMarcadores" Visible="false" runat="server" CssClass="btn btn-danger" Text="Borrar Tabla" OnClick="btnBorrarTablaMarcadores_Click" Width="150px" />
                             </div>
                              <hr />
                          </td>
                      </tr>
                      <tr>
                            <td colspan="3">
                                <h4> TOTAL:</h4> <asp:TextBox ID="txTotalLR" class="form-control input-sm" runat="server" Text="" MaxLength="500" TabIndex="6"></asp:TextBox>
                               <h4> Probabilidad:</h4> <asp:TextBox ID="txtProbabilidad" class="form-control input-sm" runat="server" Text=">99,9999%" MaxLength="500" TabIndex="6"></asp:TextBox>
						
                          </td>
                      </tr>
                  </table>
</asp:Panel>
              </div>
               
         
              <div id="tab5"  >
                  <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idProtocolo"  
                EmptyDataText="No se encontraron resultados para incorporar" >
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="numero"   HeaderText="Protocolo" >
                <ItemStyle Width="5%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
             
          
                <asp:BoundField DataField="pacientep" HeaderText="Apellidos y Nombres">
                <ItemStyle Width="30%" />
                </asp:BoundField>
             
                <asp:BoundField DataField="edad" HeaderText="Edad">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="sexo" HeaderText="Sexo">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
              
                   <%-- <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                                               
                                                                            
                                                                           

                                                                             <asp:LinkButton ID="Adjuntar" runat="server" Text="" Width="20px" >
                                             <span class="glyphicon glyphicon-paperclip"></span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
                        --%>
         
                        
                <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                <asp:BoundField DataField="Caso" HeaderText="Caso" />
         
                        
                <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
         
                        
            </Columns>
             

             <SelectedRowStyle BackColor="#CC3300" />
             

         </asp:GridView>
                    <hr />
            <asp:Button ID="btnAgregar" runat="server"    CssClass="btn btn-success" Width="200px"
                Text="Agregar a Base de Datos" OnClick="btnAgregar_Click" />
     
            
           
            <asp:Button ID="btnExcluir" runat="server"    CssClass="btn btn-danger" Width="200px"
                Text="Excluir Marcadores" OnClick="btnExcluir_Click" />
     
            
              </div>
     
 <%-- </div>--%>
               </div>
       <div class="panel-footer">		
             <table width="100%">
                 	<tr>
						<td colspan="2">
                            <asp:Label ID="lblUsuario" runat="server" Text="Label" Visible="False"></asp:Label>
                        </td>
						
					</tr>
					<tr>
						<td   colspan="2">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td align="left">
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                OnClick="lnkRegresar_Click">Regresar</asp:LinkButton>
                                        
                        </td>
						
						<td align="right">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los datos marcados como requeridos:" 
                                                ShowMessageBox="True" ValidationGroup="0" ShowSummary="False" />
                                            <asp:Button ID="btnValidar" runat="server" Text="Validar" ValidationGroup="0" 
                                                onclick="btnValidar_Click" CssClass="btn btn-success" Width="80px"  TabIndex="15" />
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" 
                                                onclick="btnGuardar_Click" CssClass="btn btn-success" Width="80px"  TabIndex="16" />
                                   <asp:LinkButton ID="imgPdf" runat="server" CssClass="btn btn-info" Text="Buscar" OnClick="imgPdf_Click" Width="200px" > <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar Resultados</asp:LinkButton>

                                        
                        </td>
						
					</tr>
					</table>




               </div>
    
        </div>  
    </div>
    <script type="text/javascript">
      
 
        function readFile(input) {

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var filePreview = document.createElement('img');
                    filePreview.id = 'file-preview';
                    //e.target.result contents the base64 data from the image uploaded
                    filePreview.src = e.target.result;
                    console.log(e.target.result);

                    var previewZone = document.getElementById('file-preview-zone');
                    previewZone.appendChild(filePreview);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }

        var fileUpload = document.getElementById('trepadorFoto');
        fileUpload.onchange = function (e) {
            readFile(e.srcElement);
        }



      </script>
 
   

</asp:Content>


