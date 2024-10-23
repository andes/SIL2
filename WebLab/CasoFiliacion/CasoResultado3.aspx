<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CasoResultado3.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoResultado3" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <style>
        
/*  bhoechie tab */
div.bhoechie-tab-container{
  z-index: 10;
  background-color: #ffffff;
  padding: 0 !important;
  border-radius: 4px;
  -moz-border-radius: 4px;
  border:1px solid #ddd;
  margin-top: 20px;
  margin-left: 20px;
  -webkit-box-shadow: 0 6px 12px rgba(0,0,0,.175);
  box-shadow: 0 6px 12px rgba(0,0,0,.175);
  -moz-box-shadow: 0 6px 12px rgba(0,0,0,.175);
  background-clip: padding-box;
  opacity: 0.97;
  filter: alpha(opacity=97);
}
div.bhoechie-tab-menu{
  padding-right: 0;
  padding-left: 0px;
  padding-bottom: 0;
}
div.bhoechie-tab-menu div.list-group{
  margin-bottom: 0;
}
div.bhoechie-tab-menu div.list-group>a{
  margin-bottom: 0;
}
div.bhoechie-tab-menu div.list-group>a .glyphicon,
div.bhoechie-tab-menu div.list-group>a .fa {
  color: #5A55A3;
}
div.bhoechie-tab-menu div.list-group>a:first-child{
  border-top-right-radius: 0;
  -moz-border-top-right-radius: 0;
}
div.bhoechie-tab-menu div.list-group>a:last-child{
  border-bottom-right-radius: 0;
  -moz-border-bottom-right-radius: 0;
}
div.bhoechie-tab-menu div.list-group>a.active,
div.bhoechie-tab-menu div.list-group>a.active .glyphicon,
div.bhoechie-tab-menu div.list-group>a.active .fa{
  background-color: #5A55A3;
  background-image: #55e03d;/*#5A55A3*/
  color: #ffffff;
}
div.bhoechie-tab-menu div.list-group>a.active:after{
  content: '';
  position: absolute;
  left: 100%;
  top: 50%;
  margin-top: -13px;
  border-left:  0;
  border-bottom: 13px solid transparent;
  border-top: 13px solid transparent;
  border-left: 10px solid #5A55A3;
}

div.bhoechie-tab-content{
  background-color: #ffffff;
  /* border: 1px solid #eeeeee; */
  padding-left: 250px;
  padding-top: 10px;
}

div.bhoechie-tab div.bhoechie-tab-content:not(.active){
  display: none;
}
    </style> 
    
     <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
     
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css" />
<script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>
<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
  <script type="text/javascript" src="../script/ValidaFecha.js"></script>  
  
    <script type="text/javascript">
    $(document).ready(function() {
    $("div.bhoechie-tab-menu>div.list-group>a").click(function(e) {
        e.preventDefault();
        $(this).siblings('a.active').removeClass("active");
        $(this).addClass("active");
        var index = $(this).index();
        $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
        $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");
    });
    });

      
            
         $(function() {
             $("#<%=txtFechaTransplante.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
	        });
	 

           

        </script>

     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <div  >
      
<input runat="server"  type="hidden" id="id"/><input runat="server"  type="hidden" id="Desde"/>

   <div class="panel panel-default">
          <div class="panel-heading">
                           <h3 class="panel-title">   <asp:Label ID="lblTipoCaso" runat="server" 
                                 Font-Bold="False"></asp:Label> <asp:Label ID="lblNumero" runat="server" Text=" CASO NRO."></asp:Label> <asp:Label ID="lblTitulo" runat="server" 
                                 Font-Bold="False"></asp:Label> </h3> 

          </div>
         <div class="panel-body">	

                             
                               
        <div class="col-lg-10 col-md-10 col-sm-2 col-xs-2 bhoechie-tab-container">
            <div class="col-lg-2 col-md-4 col-sm-4 col-xs-4 bhoechie-tab-menu">
              <div class="list-group">
                   <a href="#" class="list-group-item active text-center">
                 <span class="glyphicon glyphicon-ok" id="icoencabezado" runat="server"></span> <br/><br/>ENCABEZADO
                </a>
                <a href="#" class="list-group-item text-center">
                <span class="glyphicon glyphicon-ok" id="icomuestra" runat="server"> </span> <br/><br/>MUESTRAS
                </a>
                <a href="#" class="list-group-item text-center">
                 
<span class="glyphicon glyphicon-ok" id="icometodo" runat="server"> </span><br/><br/>METODOLOGIA
                </a>
                <a href="#" class="list-group-item text-center">
                  <span class="glyphicon glyphicon-ok" id="icoresultado" runat="server"> </span><br/><br/>RESULTADOS
                </a>
                   <a href="#" class="list-group-item text-center" runat="server" id="tituloMarcadores">
                  <span class="glyphicon glyphicon-ok" id="icomarcador" runat="server" > </span><br/><br/>MARCADORES
                </a>
                <a href="#" class="list-group-item text-center">
                	<span class="glyphicon glyphicon-ok" id="icoconclusiones" runat="server"> </span>  <br/><br/>CONCLUSIONES
                </a>
                <a href="#" class="list-group-item text-center" runat="server" id="tituloBibliografia">
                   <span class="glyphicon glyphicon-ok" id="icobibliografia" runat="server"> </span><br/><br/>BIBLIOGRAFIA
                </a>
                     <a href="#" class="list-group-item text-center" runat="server" id="tituloBaseFA">
                   <span class="glyphicon glyphicon-ok" id="icobasefa" runat="server"> </span><br/><br/>BASE FA
                </a>
              </div>
            </div>
            <div class="bhoechie-tab">
                <!-- Encabezado section -->
                <div class="bhoechie-tab-content active">
                     <div >
                         <%# Eval("muestraDetalle") %>
                                                  
                       
                         <div class="form-group">
                             <h5>Título del caso:</h5> 
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="1" ToolTip="Ingrese el titulo"></asp:TextBox>
                       </div>
                        
                        <div class="form-group"><h5>Entidad solicitante:<asp:RequiredFieldValidator ID="rfv0" runat="server" ControlToValidate="txtSolicitante" ErrorMessage="Solicitante" ValidationGroup="0" ToolTip="Solicitante">*</asp:RequiredFieldValidator>
                            </h5> 
                            <asp:TextBox ID="txtSolicitante" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="2"></asp:TextBox>
                      </div> 
					 <div class="form-group" runat="server" id="groupAutos"><h5>Autos:</h5> 
                            <asp:TextBox ID="txtAutos" runat="server" class="form-control input-sm" Width="60%" TextMode="MultiLine" Rows="3"
                                TabIndex="3"></asp:TextBox>
                       </div> 
					 <div class="form-group"><h5>Objetivo del análisis:<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtObjetivo" ErrorMessage="*" ValidationGroup="0">Objeto</asp:RequiredFieldValidator>
                            </h5> 
                            <asp:TextBox ID="txtObjetivo" runat="server" class="form-control input-sm" Width="60%" TextMode="MultiLine" Rows="3"
                                TabIndex="4">Determinación de vínculo de parentesco.</asp:TextBox>
                        </div>
                         <div class="form-group" runat="server" id="groupFechaTransplante" visible="false">                                     
        <h5>   Fecha Transplante: 
                                            </h5>
 <input id="txtFechaTransplante" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; position=absolute; z-index=0;"  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>
                          </div>
                         <div>
                               <h4>  <asp:LinkButton ID="lnkValidaEncabezado"  runat="server" Text="Valida Encabezado"   ValidationGroup="0" OnClick="lnkValidaEncabezado_Click"  ></asp:LinkButton></h4>
<asp:Label ID="lblUsuarioEncabezado" runat="server" ForeColor="Red" Text=""></asp:Label>

                         </div>
                        </div>
</div>

                   <!-- Muestras section -->
                         <div class="bhoechie-tab-content">
                               
                      
                                   <asp:Button ID="btnMuestras" runat="server" Text="Agregar/Quitar Muestras" OnClick="btnMuestras_Click" CssClass="btn btn-success" Width="240px" />
                        <div class="form-group">                           
                          
                              <asp:DataList ID="dlForense" runat="server"    
       RepeatLayout="Table">  
        <ItemTemplate>  
            <table class="table" width="80%">  
                <tr> 
                     
                    <th colspan="1">     <asp:LinkButton CssClass="btn btn-primary" Width="90px"
                         PostBackUrl= '<%# "../Protocolos/ProtocoloEditForense.aspx?idServicio=6&Desde=Caso"+ Eval("Desde")+"&Operacion=Modifica&idProtocolo=" +  Eval("idProtocolo")%>' ID="Editar" runat="server" Text=""  >
                                             <span class="glyphicon glyphicon-pencil">Editar</span></asp:LinkButton>
                        </th>
                        <th colspan="3">
                        Muestra:
                        <b>  
                            <%# Eval("muestra") %></b> &nbsp; <%# Eval("muestraDetalle") %>  </b>
                     

                         
                                                                         
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
                    <th colspan="3" >  
                         <asp:LinkButton CssClass="btn btn-primary" Width="40px" PostBackUrl= '<%# "../Protocolos/ProtocoloEditForense.aspx?idServicio=6&Desde=Caso"+ Eval("Desde")+"&Operacion=Modifica&idProtocolo=" +  Eval("idProtocolo")%>' ID="Editar" runat="server" Text=""  >
                                             <span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                        <b>  

                            <%# Eval("Persona") %></b>
                     
                                                                         
                                                                
                    <td rowspan="5">
                          
                      <div class="thumbnail" >
<asp:Image ID="Image1" runat="server" ImageUrl='<%# "../imagen.ashx?idPaciente="+ Eval("idPaciente") %>' /> 
                             <asp:Image ID="img" runat="server"            />   
                             </div>
                     
                    </td>
                </tr>  
                <tr>  
                    <td colspan="3">  
                     <b >   <%# Eval("Parentesco") %> </b>
                      
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

                                <asp:DataList ID="dlQuimerismo" runat="server" CellSpacing="3" RepeatLayout="Table" >  
        <ItemTemplate>  
    
                  <table class="table"  >  
                <tr> 
                     
                    <th colspan="2" >    
                        <asp:LinkButton PostBackUrl= '<%# "../Protocolos/ProtocoloEditForense.aspx?idServicio=6&Desde=Caso"+ Eval("Desde")+"&Operacion=Modifica&idProtocolo=" +  Eval("idProtocolo")%>' ID="Editar" runat="server" Text=""  >
                                             <span class="glyphicon glyphicon-pencil">Editar</span></asp:LinkButton> 
                        <b>   <%# Eval("Persona") %></b> 
                          
                    
                                                                         
                    </th>  
               
                </tr>  
                <tr>
                    <td>
                           <%# Eval("Parentesco")%>   
                        </td>
                    <td>
                                                    Muestra:
                        <b>  
                            <%# Eval("muestra") %></b>.   
                     
                            <%# Eval("muestraDetalle") %>
                         
                    </td>
                </tr>
                <tr>  
                    
                     
                      <td >  
                        Protocolo:  <%# Eval("Protocolo")%>   
         </td> 
                        <td >  
                       Fecha y Lugar de toma:  <%# Eval("fechatoma")%>  ,   <%# Eval("lugar")%>  
                    </td>  
                </tr>  
               
            </table>  
        </ItemTemplate>  
    </asp:DataList> 
                             

                                 <div class="form-group" runat="server" id="groupMuestras" visible="false">                                     
        <h5>   Observaciones Muestras: 
                                            </h5>
    <asp:TextBox ID="txtObservacionMuestras" runat="server" class="form-control input-sm" Width="600px" TextMode="MultiLine"
                                TabIndex="8" MaxLength="500" > </asp:TextBox>
                          </div>
                      
                              <br />
                            <div style="text-align:left;">  <h4>  <asp:LinkButton ID="ValidaMuestra" runat="server" Text="Valida Muestras"  OnClick="ValidaMuestra_Click" ValidationGroup="10"  ></asp:LinkButton></h4></div>
<asp:Label ID="lblUsuarioMuestras" ForeColor="Red" runat="server" Text=""></asp:Label>

                          
                      
                </div>
                                   
                        </div>
                <!-- Metodos section -->
                <div class="bhoechie-tab-content">
                 
                    
                                        <div class="form-group">                                     
        <h5>   Método de extracción:   
                                            </h5>

                                       <asp:Label ID="txtMetodoExtraccion" runat="server"  class="form-control"  Width="400px" Enabled="false" ></asp:Label>
                             <%--                <asp:DropDownList ID="ddlMetodoExtraccion"  runat="server" Width="400px"
                                class="form-control input-sm" TabIndex="8" ></asp:DropDownList>--%>
                                     
                                            <asp:CheckBoxList  ID="chkMetodoExtraccion" RepeatDirection="Vertical"  6 runat="server"    RepeatColumns="3"></asp:CheckBoxList>
                                             
                                         
  </div>

              <div class="form-group"><h5>  Amplificación:  
                  </h5>
                            <asp:TextBox ID="txtAmplificacion" runat="server" class="form-control input-sm" Width="400px" Enabled="false"
                                TabIndex="10" ToolTip="Amplificación" MaxLength="500"  > </asp:TextBox>
                      <asp:CheckBoxList  ID="chkAmplificacion" RepeatDirection="Vertical"    runat="server"    RepeatColumns="3"></asp:CheckBoxList>
                                             
                           </div>
                        	 <div class="form-group" id="groupCuantificacion" runat="server">
                     <h5>     Cuantificación: 

                                 </h5>
                        
                            <asp:TextBox ID="txtCuantificacion" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="9" MaxLength="500" ></asp:TextBox>
                          </div>
                  
					
					 <div class="form-group"><h5>Análisis de fragmentos:<asp:RequiredFieldValidator ID="rfv5" runat="server" ControlToValidate="txtAnalisis" ErrorMessage="Analisis" ValidationGroup="1" ToolTip=" ">*</asp:RequiredFieldValidator>
                            </h5> 
                            

                            <asp:TextBox ID="txtAnalisis" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="11" ToolTip="Análisis de fragmentos" MaxLength="500"  >Equipo ABI 3500</asp:TextBox>
                               
</div>
					 <div class="form-group">
                     <h5>      Software de análisis: <asp:RequiredFieldValidator ID="rfv6" runat="server" ControlToValidate="txtSoftware" ErrorMessage="Software" ValidationGroup="1">*</asp:RequiredFieldValidator>
                         </h5>
                            <asp:TextBox ID="txtSoftware" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="12" ToolTip="Software de análisis" MaxLength="500"  >GeneMapper ID-X 1.6</asp:TextBox>
                           </div>
					 <div class="form-group" id="groupAnalisisEstadistico" runat="server">
                      <h5>   Análisis estadístico:  <asp:RequiredFieldValidator ID="rfv7" runat="server" ControlToValidate="txtEstadistico" ErrorMessage="Analisis Estadistico" ValidationGroup="1" ToolTip=" ">*</asp:RequiredFieldValidator>
                         </h5>
                            <asp:TextBox ID="txtEstadistico" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="13" ToolTip="Análisis estadístico" MaxLength="500"  >Software Familias versión 3.1.8 (3)</asp:TextBox>
                          </div>
 <div class="form-group" id="groupMarcoEstudio" runat="server">
     <h5> Marco de estudio estadístico:</h5>  
                            <asp:TextBox ID="txtMarcoEstudio" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="14" ToolTip="Marco de estudio estadístico" TextMode="MultiLine" Rows="8" MaxLength="5000">Hipótesis planteadas:
A.- ..... es el padre biológico de .............. siendo .............. la madre.
B.- El padre biológico de ............. es un individuo al azar de la población, no relacionado con ................., siendo ................ la madre.


                            </asp:TextBox>
                        
                          
                      
                            </div>
                    <div class="form-group" id="groupLimiteDeteccion" runat="server" visible="false">
                      <h5>   Limite de detección:

                         </h5>
                            <asp:TextBox ID="txtLimiteDeteccion" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="13" ToolTip="Análisis estadístico" MaxLength="500"  ></asp:TextBox>
                          </div>

                      <div style="text-align:left;"> 
                            <h4>
                    <asp:LinkButton ID="ValidaMetodo" runat="server" Text="Valida Metodos" OnClick="Valida1_Click" ValidationGroup="1"  ></asp:LinkButton>
                                </h4>
                         </div>
                     <div class="form-group">
                       <br />
<asp:Label ID="lblUsuarioMetodos" ForeColor="Red" runat="server" Text=""></asp:Label>
</div>
                </div>
    
                <!-- Resultados section -->
             <div class="bhoechie-tab-content">
                 
                         <div class="form-group">
                             <h4>Resultados</h4><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtResultado" ErrorMessage="Resultados" ValidationGroup="4" ToolTip="Resultados">*</asp:RequiredFieldValidator>
                              <asp:TextBox ID="txtResultado" runat="server" class="form-control input-sm" Width="80%" 
                               TextMode="MultiLine" Rows="10" TabIndex="5" ToolTip="Ingrese el resultado">Ver tabla 1. Se observan incompatibilidades en los siguientes marcadores: 
</asp:TextBox>
                             </div>
                   <asp:Label ID="estatus" runat="server"   Style="color: #0000FF"></asp:Label>
                  <div id="divInputLoad">
                                <div id="divFileUpload">
                     <h5> Adjuntar imagen:</h5>
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

                                    <anthem:Button ID="btnBorrarImg" CssClass="btn btn-primary" runat="server" OnClick="btnBorrarImg_Click" Text="Borrar Img." Visible="False" Width="100px" AutoUpdateAfterCallBack="True" />
            </div>
               </div>    
                   <h4> <asp:LinkButton ID="ValidaResultado" runat="server" Text="Valida Resultados"   ValidationGroup="4" OnClick="LinkButton1_Click"  ></asp:LinkButton></h4>
                 <asp:Label ID="lblUsuarioResultados" ForeColor="Red" runat="server" Text=""></asp:Label>
                      
             
                          <br />
<asp:Label ID="Label1" ForeColor="Red" runat="server" Text=""></asp:Label>

</div>

                 <!-- Marcadores section -->  
                        
          <div class="bhoechie-tab-content" >
                       
                         <div class="form-group">

                             
   <div class="panel panel-danger">
          <div class="panel-heading">
                           <h3 class="panel-title"> Marcadores </h3> 

          </div>
         <div class="panel-body">	
                              
                             <div class="form-group">
                                     <h4>Archivo Genotipos:</h4>
                                   
                            
                             Seleccione tipo de marcador:<asp:DropDownList ID="ddlTipoMarcadorArchivo" Width="200px" class="form-control input-sm" runat="server"></asp:DropDownList>

 <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm"  Width="200px" />
                                                

                                         <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="200px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />

                                      <br />
                                     <br />

                                      
                                     </div>
                  
                          <div class="form-group" runat="server" ID="pnlMarcadoresFiliacion">  
                      
                          <h4>  Archivo LR:</h4>
                           <asp:FileUpload ID="trepador2" runat="server" class="form-control input-sm"  Width="400px"/> 
                            <asp:Button  CssClass="btn btn-primary" ID="subir0" runat="server" Width="200px" 
                    Text="Procesar Archivo" OnClick="subir0_Click" /> 
                      </div>
                           <div class="form-group">
  
               
               <asp:Label ID="estatus1" runat="server" 
                    Style="color:red"></asp:Label>
            </div>
                            <div>

                            <asp:GridView ID="gvTablaForense" runat="server" Font-Names="Verdana" Font-Size="12pt" EmptyDataText="No se encontraron datos de los protocolos del caso" Font-Bold="False"></asp:GridView>

                            <asp:GridView ID="gvTablaForense2" runat="server" Font-Names="Verdana" Font-Size="12pt" EmptyDataText="No se encontraron datos de los protocolos del caso" AutoGenerateColumns="False" DataKeyNames="idProtocoloSubitem" OnRowCommand="gvTablaForense2_RowCommand" OnRowDataBound="gvTablaForense2_RowDataBound">
                                <Columns>  <asp:TemplateField HeaderText="Seleccionar" >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                    <asp:BoundField DataField="protocolo" HeaderText="Protocolo" />
                                    <asp:BoundField DataField="subitem" HeaderText="Subitem" />
                                       
                                     <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
                                </Columns>
                                </asp:GridView>
                                <asp:Button ID="btnBorrarTablaMarcadores" Visible="false" runat="server" CssClass="btn btn-danger" Text="Borrar Tabla" OnClick="btnBorrarTablaMarcadores_Click" Width="150px" />

                             </div>
                              
                          </div>
       <div class="panel-footer">
                                 <div class="form-group" id="groupTotalProbabilidad" runat="server" ><hr />
                                <h4> TOTAL:</h4> <asp:TextBox ID="txTotalLR" class="form-control input-sm" runat="server" Text="" MaxLength="500" TabIndex="6" Width="150px"></asp:TextBox>
                               <h4> Probabilidad:</h4> <asp:TextBox ID="txtProbabilidad" class="form-control input-sm" runat="server" Text=">99,9999%" MaxLength="500" Width="150px" TabIndex="6"></asp:TextBox>
                                     </div>
					   

                            
                             <asp:DropDownList ID="ddlTipoMarcador" Width="200px" class="form-control input-sm" runat="server"></asp:DropDownList>
                             <asp:Button ID="btnDescargarMarcador"  Width="200px" CssClass="btn btn-success" runat="server" Text="Descargar Informe" OnClick="btnDescargar_Click" />
                        
                                 <br />

                            <asp:LinkButton ID="lnkValidarMarcadores" runat="server" Text="Valida Marcadores" OnClick="lnkValidarMarcadores_Click" ValidationGroup="4"  ></asp:LinkButton>

                      
             	</div>
                           <br />
               </div>     
                          <br />
<asp:Label ID="lblUsuarioMarcadores" ForeColor="Red" runat="server" Text=""></asp:Label>

                          
                      
                   
            </div>
                       
   </div>
                  <!-- CONCLUSIONES section -->
                   <div class="bhoechie-tab-content">
                    <div style="margin-left:10px;">
                    
                      
                         <div class="form-group">
                              <h4>
                                 Conclusiones:</h4><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtConclusion" ErrorMessage="Conclusiones" ValidationGroup="2" ToolTip="Conclusiones">*</asp:RequiredFieldValidator>
                         

                            <asp:TextBox ID="txtConclusion" runat="server" class="form-control input-sm" Width="90%" 
                                TabIndex="7" TextMode="MultiLine" Rows="20">Los resultados obtenidos son compatibles con la existencia de vínculo biológico de paternidad del Sr. ....... respecto de......., siendo ...... la madre biológica. El Índice de Paternidad (IP) obtenido es ....... Este número indica que es ............. veces más probable obtener este resultado si........... es el padre biológico de ....., que si lo fuera cualquier otro hombre no relacionado y tomado al azar de la población.

Luego del análisis de las muestras se excluye el vínculo de paternidad entre ....................... y .................., siendo el mismo, hijo de ........
                            </asp:TextBox>
                             

                          
                      
                             </div>
                            <div style="text-align:left;"> 
                 <h4>   <asp:LinkButton ID="ValidaConclusiones" runat="server" Text="Valida Conclusiones"  OnClick="Valida2_Click" ValidationGroup="2"  ></asp:LinkButton></h4> 
                                 <br />
<asp:Label ID="lblUsuarioConclusiones" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                    </div>
                </div>
                     <!-- Bibliografia section -->
                <div class="bhoechie-tab-content">
                     
                            <div class="form-group">
                      
                    <h4>
                                 Bibliografía:</h4>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtBibliografia" ValidationGroup="3"></asp:RequiredFieldValidator>
                        
                            <asp:TextBox ID="txtBibliografia" runat="server" class="form-control input-sm" Width="80%"
                                TabIndex="15" ToolTip="BIBLIOGRAFIA" TextMode="MultiLine" Rows="12">•	Frecuencias alélicas: Update of an on-line autosomal STR and Y-STR reference database of Argentina. Evguenia Alechine, Miguel Marino, Andrea Sala, Maria Cecilia Bobillo, Mariela Caputo and Daniel Corach. Forensic Science International: Genetics Supplement Series. Volume 2, Issue 1 2009, 382- 383.
•	Pautas y normas mínimas aprobadas por la Sociedad Argentina de Genética Forense para los informes de paternidad mediante análisis de ADN.
•	(1) Optimizing direct amplification of forensic commercial kits for STR Determination M. Caputo, M.C. Bobillo, A. Sala, D. Corach. Journal of Forensic and Legal Medicine 47 (2017) 17-23.
•	(2) TECHNICAL MANUAL-PowerPlex® Fusion System. Instructions for Use of Products DC2402 and DC2408
•	(3)Manual for Familias 3. Daniel Kling, Petter Mostad, Thore Egeland. 2014 
</asp:TextBox>
                            
                        

                          
                      
              </div>
             <h4>           <asp:LinkButton ID="ValidaBibliografia" runat="server" Text="Valida Bibliografia"  OnClick="Valida3_Click" ValidationGroup="3"  ></asp:LinkButton></h4>
                            
                                <br />
<asp:Label ID="lblUsuarioBibliografia" runat="server" ForeColor="Red"  Text=""></asp:Label>
                    
                </div>


           <!-- base FA section -->
                  <div class="bhoechie-tab-content">
                     
                      <h1  >Base FA</h1>
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
                   
                </div>
            </div>
        </div> 


         </div>

<div class="panel-footer">
           
                            <asp:Label ID="lblUsuario" runat="server" Text="Label" Visible="False"></asp:Label>
                        
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                OnClick="lnkRegresar_Click">Regresar</asp:LinkButton>
                       
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los datos marcados como requeridos:" 
                                                ShowMessageBox="True" ValidationGroup="0" ShowSummary="False" />
                                            <asp:Button ID="btnValidar" runat="server" Text="Generar Informe" ValidationGroup="0" 
                                                onclick="btnValidar_Click" Visible="false" CssClass="btn btn-success" Width="150px"  TabIndex="15" />
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" Visible="false"
                                                onclick="btnGuardar_Click" CssClass="btn btn-success" Width="150px"  TabIndex="16" />
                                   <asp:LinkButton ID="imgPdf" runat="server" CssClass="btn btn-info" Text="Descargar Resultado Final" OnClick="imgPdf_Click" Width="200px" />
                                           <asp:LinkButton ID="imgPdfPreeliminar" Visible="false" runat="server" CssClass="btn btn-info" Text="Descargar Resultado Preeliminar"   Width="200px" OnClick="imgPdfPreeliminar_Click" />

                                           <%# Eval("Persona") %>

                                        
                        
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
