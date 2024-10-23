<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoResultados.aspx.cs"
     Inherits="WebLab.SeguimientoResultados" MasterPageFile="~/Site1.Master" %>
  <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     
      <script type="text/javascript" src='<%= ResolveUrl("script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("script/jquery-ui.css") %>'  />

      
   	 
     <script type="text/javascript" src="script/ValidaFecha.js"></script>        
      <style type="text/css">
          .style3
          {
              font-size: 10pt;
              font-family: Calibri;
              color: #333333;
              font-weight: bold;
              border-style: none;
              background-color: #FFFFFF;
          }
          .style4
          {
              width: 96%;
          }
      </style>

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
          </script>

   </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    

     
       <div align="left" style="width: 100%" class="form-inline"  >
  <div   class="panel panel-success">
   <div class="panel-heading">  <h3>Coronavirus (COVID-19):Resultados informados</h3>
        </div>
         	<div class="panel-body" >  
            <asp:Label ID="lblError" runat="server" Text="Label" Visible="false"></asp:Label>     
                   <div class="form-group" >
                   <strong>    Fecha Validación Desde:  </strong>
                     <input id="txtFechaDesde" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

                  <strong>  Fecha Validación Hasta: </strong>  <input id="txtFechaHasta" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
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
                        <strong>      Tipo: </strong><asp:DropDownList ID="ddlTipo" runat="server" 
                                ToolTip="Seleccione el resultado" TabIndex="1" class="form-control input-sm">
                            <asp:ListItem>Validado</asp:ListItem>
                            <asp:ListItem>Prevalidado</asp:ListItem>
                            </asp:DropDownList>
                       </div>
                        <div class="form-group" > 
                        <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"  CssClass="btn btn-success" Width="100px"/>
                             <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Exportar Excel"  CssClass="btn btn-success" Width="150px"/>
         
             </div>
                 
             
         <%--   <asp:RadioButtonList ID="rdbOpcion" runat="server"    RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbOpcion_SelectedIndexChanged" AutoPostBack="True">
                  <asp:ListItem Selected="True"  Value="0">Muestras Ingresadas</asp:ListItem>
                 <asp:ListItem Value="3">Muestras Pendientes de Resultados</asp:ListItem>
                  <asp:ListItem Value="4">Muestras Positivas</asp:ListItem>
                  <asp:ListItem Value="5">Muestras Procesadas</asp:ListItem>
                <asp:ListItem Value="1">Pacientes Analizados</asp:ListItem>
                <asp:ListItem Value="2">Pacientes Positivos CoVid-19</asp:ListItem>
            </asp:RadioButtonList>--%>
                 
    
       </div>
      	<div class="panel-footer" > 
            <h3  >  <asp:Label ID="lblCantidad" runat="server" Text=""></asp:Label></h3>
              <div style="overflow-y:auto;width:1300px; height:800px;">  
        <asp:GridView ID="GridResultados"  CssClass="table table-bordered bs-table"  runat="server" Width="600px" Font-Size="8pt" RowDataBound="GridView1_RowDataBound" Font-Names="Verdana" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound1">
            <Columns>
                <asp:BoundField DataField="Fecha Registro" HeaderText="Fecha Registro" />
                <asp:BoundField DataField="Protocolo" HeaderText="Nro." />
                <asp:BoundField DataField="Origen" HeaderText="Origen" />
                <asp:BoundField DataField="Efector Procedencia" HeaderText="Efector Procedencia" />
             
                <asp:BoundField DataField="Caracter" HeaderText="Caracter" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellidos" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombres" />
                <asp:BoundField DataField="Tipo Doc." HeaderText="Tipo Doc." />
                <asp:BoundField DataField="Nro. Documento" HeaderText="Nro. Doc." />
                <asp:BoundField DataField="Fecha Nacimiento" HeaderText="Fecha Nac." />
                <asp:BoundField DataField="Edad" HeaderText="Edad" />
                <asp:BoundField DataField="amd" HeaderText="amd" />
                <asp:BoundField DataField="Sexo" HeaderText="S" />
                
                
              <%--  <asp:BoundField DataField="Ciudad Domicilio" HeaderText="Ciudad Domicilio" />
                <asp:BoundField DataField="Provincia Domicilio" HeaderText="Provincia Domicilio" />
                <asp:BoundField DataField="Pais" HeaderText="Pais" />--%>
                <asp:BoundField DataField="Amb/Int." HeaderText="A/I" />
                   <asp:BoundField DataField="Solicitante" HeaderText="Solicitante" />
                <asp:BoundField DataField="F. Toma Muestra" HeaderText="F.Toma" />

                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                   <asp:BoundField DataField="Hisopado" HeaderText="Hisopado" />
     
                
               <asp:BoundField DataField="Obra Social" HeaderText="Obra Social" />
                                      <asp:BoundField DataField="F. Resultado" HeaderText="F. Resultado" />
     
                <asp:BoundField DataField="CoVid-19" HeaderText="CoVid-19" />
               <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
            </Columns> 
                  </asp:GridView>

                  </div>
    <br />
       
    </div>
     </div>
           </div>
    </asp:Content>