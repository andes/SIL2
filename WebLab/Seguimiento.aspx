<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seguimiento.aspx.cs"
     Inherits="WebLab.Seguimiento" MasterPageFile="~/Site1.Master" %>
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
  <div   class="panel panel-danger">
   <div class="panel-heading">  <h3>Panel Respiratorio:Seguimiento Muestras</h3>
        </div>
         	<div class="panel-body" >  
              <asp:Label ID="lblError" runat="server" Text="Label" Visible="false"></asp:Label>
                 
                   <div class="form-group" >

            <asp:CheckBoxList ID="chkItem" runat="server" RepeatDirection="Horizontal" Font-Size="14pt" OnSelectedIndexChanged="chkItem_SelectedIndexChanged" RepeatColumns="6"></asp:CheckBoxList>
                       </div>
                    <hr />
                   <div class="form-group" >
                   <strong>    Desde:  </strong>
                     <input id="txtFechaDesde" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

                  <strong>   Hasta: </strong>  <input id="txtFechaHasta" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

                        <asp:RadioButtonList ID="rdbOpcion" runat="server"    RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbOpcion_SelectedIndexChanged" AutoPostBack="True">
                  <asp:ListItem Selected="True"  Value="0">Muestras Ingresadas</asp:ListItem>
                 <asp:ListItem Value="3">Muestras Pendientes de Resultados</asp:ListItem>
                  <asp:ListItem Value="4">Muestras Positivas</asp:ListItem>
                  <asp:ListItem Value="5">Muestras Procesadas</asp:ListItem>
                <asp:ListItem Value="1">Pacientes Analizados</asp:ListItem>
                <asp:ListItem Value="2">Pacientes Positivos</asp:ListItem>
            </asp:RadioButtonList>
</div> 
                 
             <hr />
           
                 
    
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
                        <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"  CssClass="btn btn-danger" Width="100px"/>
                             <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Exportar Excel"  CssClass="btn btn-danger" Width="150px"/>
         
             </div>
                 
             
       </div>
      	<div class="panel-footer" > 
            <h3  >  <asp:Label ID="lblCantidad" runat="server" Text=""></asp:Label></h3>
              <div style="overflow-y:auto;width:1300px; height:1000px;">  
        <asp:GridView ID="GridView1"  CssClass="table table-bordered bs-table"  runat="server" Width="980px" Font-Size="8pt" OnRowDataBound="GridView1_RowDataBound" Font-Names="Verdana" AutoGenerateColumns="False">
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
                
                  <%-- <asp:BoundField DataField="Calle Domicilio" HeaderText="Calle Domicilio" />
                   <asp:BoundField DataField="Barrio Domicilio" HeaderText="Barrio Domicilio" />
                <asp:BoundField DataField="Ciudad Domicilio" HeaderText="Ciudad Domicilio" />
                <asp:BoundField DataField="Provincia Domicilio" HeaderText="Provincia Domicilio" />
                <asp:BoundField DataField="Pais" HeaderText="Pais" />--%>
                 <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                <asp:BoundField DataField="Amb/Int." HeaderText="A/I" />
                   <asp:BoundField DataField="Solicitante" HeaderText="Solicitante" />
                <asp:BoundField DataField="F. Toma Muestra" HeaderText="F.Toma" />

                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
       <asp:BoundField DataField="numeroOrigen2" HeaderText="Nro. Hisopado" />
                
               <asp:BoundField DataField="Obra Social" HeaderText="Obra Social" />
                                      <asp:BoundField DataField="F. Resultado" HeaderText="F. Resultado" />
     
                <asp:BoundField DataField="Resultado" HeaderText="Resultado" />
                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
            </Columns> 
                  </asp:GridView>

                     <asp:GridView ID="gvPacientes" Visible="false" CssClass="table table-bordered bs-table"  runat="server" Width="600px" Font-Size="8pt"  Font-Names="Verdana" AutoGenerateColumns="true">
             
                  </asp:GridView>
                  </div>
    <br />
       
    </div>
     </div>
           </div>
    </asp:Content>