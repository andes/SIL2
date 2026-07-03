<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteInsumo.aspx.cs" Inherits="WebLab.Estadisticas.ReporteInsumo" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
     

  
    </asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
      <div align="left" style="width: 1100px" class="form-inline"  >
   <div class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title">  Estadistica de Tiempo Sin Insumos </h3>
                       
                        </div>

				<div class="panel-body">

      <table  align="left" width="100%" >
					
					
					<tr>
						 <td class="myLabelIzquierda">Efector:&nbsp;</td>
						<td>
                            <anthem:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" AutoPostBack="True"  >
                            </anthem:DropDownList>
            <asp:Button ID="btnBuscar" runat="server"   CssClass="btn btn-primary" Width="150px"
                Text="Buscar" ValidationGroup="0" OnClick="btnBuscar_Click" />

                                            </td>
					</tr>

          	</table>         
           
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
          
            
         </div>
      
                 
       <div class="panel-footer"> 
            <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
                		<div style="border: 1px solid #999999; height: 450px; width:1000px; overflow: scroll; background-color: #EFEFEF;"> 
      <asp:GridView 
        ID="gvSinInsumo"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table"
        EmptyDataText="No hay registros">

     <Columns>

    <asp:BoundField 
        DataField="codigo"
        HeaderText="Código" />

    <asp:BoundField 
        DataField="nombre"
        HeaderText="Determinación" />


    <asp:BoundField 
        DataField="Efector"
        HeaderText="Efector" />

    <asp:BoundField 
        DataField="InicioSinInsumo"
        HeaderText="Inicio sin insumo"
        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

    <asp:BoundField 
        DataField="FinSinInsumo"
        HeaderText="Fin sin insumo"
        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

    <asp:BoundField 
        DataField="DiasSinInsumo"
        HeaderText="Días sin insumo" />

</Columns>

    </asp:GridView>
        
               
              
        </div>  

       </div>
          </div>
          </div>
        
</asp:Content>