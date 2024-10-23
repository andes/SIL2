<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoSecuenciacion.aspx.cs"
     Inherits="WebLab.SeguimientoSecuenciacion" MasterPageFile="~/Site1.Master" %>
  <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     
      <script type="text/javascript" src='<%= ResolveUrl("script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("script/jquery-ui.css") %>'  />

      
   	 
     <script type="text/javascript" src="script/ValidaFecha.js"></script>        
       

     <script type="text/javascript">                    
                                  
          
            
         $(function() {
             $("#<%=txtFechaDesde.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: 'App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
         });

              
         $(function() {
             $("#<%=txtFechaHasta.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: 'App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
         });

         
$(function () {
    $("#tabContainer").tabs();
    var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
    $("#tabContainer").tabs({ selected: currTab });
});
          </script>

   </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
    <asp:HiddenField runat="server" ID="HFCurrTabIndex"  /> 
     
       <div align="left" style="width: 1450px" class="form-inline"  >
  <div   class="panel panel-success">
   <div class="panel-heading">  <h3>VIGILANCIA GENOMICA (COVID-19)</h3>
        </div>
         	<div class="panel-body" >  
            <asp:Label ID="lblError" runat="server" Text="Label" Visible="false"></asp:Label>     
                   <div class="form-group" >
                   <strong>    Fecha Desde:  </strong>
                     <input id="txtFechaDesde" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

                  <strong>  Fecha Hasta: </strong>  <input id="txtFechaHasta" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

</div> 
                 <br />
                       <div class="form-group" >
                       <strong>      Caracter: </strong><asp:DropDownList ID="ddlCaracter" runat="server" 
                                ToolTip="Seleccione el caracter" TabIndex="1" class="form-control input-sm">
                            </asp:DropDownList>
                     </div>
                   <div class="form-group" >
                       <strong>      Resultado: </strong><asp:DropDownList ID="ddlResultado" runat="server" 
                                ToolTip="Seleccione el resultado" TabIndex="1" class="form-control input-sm">
                            </asp:DropDownList>
                     </div>

                  
                        <div class="form-group" > 
                        <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"  CssClass="btn btn-success" Width="100px"/>
                             <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Exportar Excel Detalle"  CssClass="btn btn-success" Width="180px"/>
           </div>
                            <br />
                                        

           
               
           
                  
                 
    
       </div>
      	<div class="panel-footer" > 

              <div id="tabContainer">  
                           
                             <ul>
    <li><a href="#tab0"><b>Resumen</b></a></li>  
    <li><a href="#tab1"><b>Detalle</b></a></li>                              
     
</ul>               
                  <div  id="tab0" >  
        <asp:GridView ID="gvResumen"  CssClass="table table-bordered bs-table"  runat="server" Width="500px" Font-Size="10pt"   Font-Names="Verdana"  >
                  </asp:GridView>
                    </div>
                      <div  id="tab1" >  
            <h4  >  <asp:Label ID="lblCantidad" runat="server" Text=""></asp:Label></h4>
              <div style="overflow-y:auto;width:1300px; height:800px;">  
        <asp:GridView ID="GridResultados"  CssClass="table table-bordered bs-table"  runat="server" Width="600px" Font-Size="8pt" RowDataBound="GridView1_RowDataBound" Font-Names="Verdana" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound1">
            <Columns>
                
                
              <%--  <asp:BoundField DataField="Ciudad Domicilio" HeaderText="Ciudad Domicilio" />
                <asp:BoundField DataField="Provincia Domicilio" HeaderText="Provincia Domicilio" />
                <asp:BoundField DataField="Pais" HeaderText="Pais" />--%>
                <asp:BoundField DataField="Fecha Registro" HeaderText="Fecha Registro" />
                <asp:BoundField DataField="Protocolo" HeaderText="Nro." />
                <asp:BoundField DataField="linaje" HeaderText="Linaje de SARS-CoV-2" >
                <ItemStyle Font-Bold="True" ForeColor="#CC3300" />
                </asp:BoundField>
                <asp:BoundField DataField="Efector Procedencia" HeaderText="Efector Procedencia" />
             
                <asp:BoundField DataField="codigoPais" HeaderText="Codigo PAIS" />
                  <asp:BoundField DataField="eventoSisa" HeaderText="Evento SISA" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellidos" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombres" />
                <asp:BoundField DataField="Tipo Doc." HeaderText="Tipo Doc." />
                <asp:BoundField DataField="Nro. Documento" HeaderText="Nro. Doc." />
                <asp:BoundField DataField="Fecha Nacimiento" HeaderText="Fecha Nac." />
                <asp:BoundField DataField="Edad" HeaderText="Edad" />
                <asp:BoundField DataField="amd" HeaderText="amd" />
                <asp:BoundField DataField="Sexo" HeaderText="S" />
                
                
                <asp:BoundField DataField="ciudad" HeaderText="Ciudad"  />
                <asp:BoundField DataField="Amb/Int." HeaderText="A/I" />
                  
                <asp:BoundField DataField="F. Toma Muestra" HeaderText="F.Toma" />

                <asp:BoundField DataField="CAL RED (RDRP)" HeaderText="CAL RED (RDRP)" />
                   <asp:BoundField DataField="QUASAR 670 (N)" HeaderText="QUASAR 670 (N)" />
     
                 <asp:BoundField DataField="FAM (E, N1)" HeaderText="FAM" />
               <asp:BoundField DataField="FIS" HeaderText="FIS" />
                                      <asp:BoundField DataField="fuc" HeaderText="FUC" />
     
               <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
            </Columns> 
                  </asp:GridView>

                  </div>

                          </div>
                  </div>
    <br />
       
    </div>
     </div>
           </div>
    </asp:Content>