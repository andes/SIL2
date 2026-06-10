<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadosaSisa.aspx.cs" Inherits="WebLab.Estadisticas.ResultadosaSisa" MasterPageFile="~/Site1.Master" %>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   
     <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      
       <script type="text/javascript">     
         
        
         
            
         $(function() {
             $("#<%=txtFechaDesde.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
         });

               $(function() {
             $("#<%=txtFechaHasta.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
	        });
         
  </script>  
  
  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
   <div  style="width: 1150px" class="form-inline" >
           <div class="panel panel-success" runat="server" > 
                    <div class="panel-heading"> <h3 class="panel-title">RESULTADOS INFORMADOS A SISA</h3>

                        </div>
                   <div class="panel-body">
     <table width="1100px"
         >
     <tr>
  <td colspan="1">Efector:               </td>
  <td colspan="3">
<asp:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" >
                            </asp:DropDownList></td>

     </tr>
     <tr>

         <td >

 
          
                Fecha  Desde:</td>
         <td><input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm"  /> 
             </td>
                <td colspan="2">
                 Fecha  Hasta: <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm"  />
                    <asp:CustomValidator ID="cvValidaDatos" runat="server" ErrorMessage="CustomValidator" OnServerValidate="cvValidaDatos_ServerValidate"></asp:CustomValidator>
                    </td>
         </tr>
         <tr>
                 <td colspan="1">Practica: </td>
          <td colspan="3"><asp:DropDownList ID="ddlItem" runat="server" class="form-control input-sm"  AutoPostBack="True" Font-Bold="True" Font-Size="12pt" >
                </asp:DropDownList>
             &nbsp;<asp:RangeValidator ID="rvItem" runat="server" ControlToValidate="ddlItem" ErrorMessage="RangeValidator" MaximumValue="99999999" MinimumValue="1" Type="Integer" ValidationGroup="0">Debe seleccionar un evento</asp:RangeValidator>

         </td>
             </tr>
       
    <tr>
              <td colspan="1">
                <asp:Button ID="btnBuscar"  runat="server" Text="Buscar" OnClick="btnBuscar_Click"  CssClass="btn btn-success" Width="100px"  />
                </td>   
              <td colspan="3" align="right">
                <asp:Button ID="btnDescargarExcelControl"  runat="server" Text="Descargar Excel" OnClick="btnDescargarExcelControl_Click"  CssClass="btn btn-success" Width="150px"  />
                <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                
                <asp:Button ID="btnDescargarDetalle"
    runat="server"
    Text="Descargar detalle Pacientes"
    CssClass="btn btn-success"
    OnClick="btnDescargarDetalle_Click" Width="200px" />
                    </td>
        </tr>
         </table>
               
            </div>
           
                  
               

                 
                    <div class="panel-footer">
                         <asp:GridView ID="gvEstadistica"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="table table-bordered table-hover"
    ShowFooter="True"
    OnRowDataBound="gvEstadistica_RowDataBound" EmptyDataText="No hay datos para los filtros de busqueda ingresados.">

    <Columns>

        <asp:BoundField
            DataField="fechaEnvio"
            HeaderText="Fecha envío">
        </asp:BoundField>

        <asp:BoundField
            DataField="nombreEfector"
            HeaderText="Efector">
        </asp:BoundField>

        <asp:BoundField
            DataField="idItem"
            HeaderText="ID Item">
        </asp:BoundField>

        <asp:BoundField
            DataField="nombreItem"
            HeaderText="Práctica">
        </asp:BoundField>

        <asp:BoundField
            DataField="cantidadEnviados"
            HeaderText="Enviados"
            ItemStyle-HorizontalAlign="Right"
            FooterStyle-HorizontalAlign="Right">
        </asp:BoundField>

        <asp:BoundField
            DataField="pacientesUnicos"
            HeaderText="Pacientes"
            ItemStyle-HorizontalAlign="Right"
            FooterStyle-HorizontalAlign="Right">
        </asp:BoundField>

        <asp:BoundField
            DataField="protocolos"
            HeaderText="Protocolos"
            ItemStyle-HorizontalAlign="Right"
            FooterStyle-HorizontalAlign="Right">
        </asp:BoundField>

    </Columns>

    <FooterStyle
        BackColor="#D9EDF7"
        Font-Bold="true" />

    <HeaderStyle
        BackColor="#337AB7"
        ForeColor="White"
        Font-Bold="true" />

</asp:GridView>
                        </div>
      

        </div>

      </div>
     

</asp:Content>