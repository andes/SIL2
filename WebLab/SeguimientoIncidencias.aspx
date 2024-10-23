<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoIncidencias.aspx.cs"
     Inherits="WebLab.SeguimientoIncidencias" MasterPageFile="~/Site1.Master" %>
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
  <div   class="panel panel-info">
   <div class="panel-heading">  <h3>Coronavirus (COVID-19):Muestras con incidencias</h3>
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
                       <strong>      Tipo Incidencia: </strong><asp:DropDownList ID="ddlTipo" runat="server" 
                                ToolTip="Seleccione" TabIndex="1" class="form-control input-sm">
                           <asp:ListItem Value="0">Todas</asp:ListItem>
                           <asp:ListItem>Prerecepcion</asp:ListItem>
                           <asp:ListItem>De Protocolo</asp:ListItem>
                            </asp:DropDownList>
                     </div>
                        <div class="form-group" > 
                        <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"  CssClass="btn btn-info" Width="100px"/>
                             <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Exportar Excel"  CssClass="btn btn-info" Width="150px"/>
         
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
        <asp:GridView ID="GridIncidencias"  CssClass="table table-bordered bs-table"  runat="server" Width="600px" Font-Size="8pt" RowDataBound="GridView1_RowDataBound" Font-Names="Verdana" OnRowDataBound="GridView1_RowDataBound1">
                  </asp:GridView>

                  </div>
    <br />
       
    </div>
     </div>
           </div>
    </asp:Content>