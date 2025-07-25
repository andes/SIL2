<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadosaSisa2.aspx.cs" Inherits="WebLab.Resultados.ResultadosaSisa2" MasterPageFile="~/Site1.Master" %>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
       <script type="text/javascript">     
         
        
         $(function () {
             
                 $("#tabContainer").tabs();                        
                $("#tabContainer").tabs({ selected: 0 });
             });                         
          
            
         $(function() {
             $("#<%=txtFechaDesde.ClientID %>").datepicker({
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
           <div class="panel panel-primary" runat="server" > 
                    <div class="panel-heading"> <h3 class="panel-title">RESULTADOS A SISA</h3>

                        </div>
                   <div class="panel-body">
     <table width="1100px"
         >
     <tr>

         <td colspan="2">


Efector:               <asp:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" >
                            </asp:DropDownList></td></tr>
     <tr>

         <td colspan="2">


            <div>
              
          
                <p>Fecha Validacion Desde:<input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                        style="width: 120px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm"  /></p>
                  <p> Evento SISA:  <asp:DropDownList ID="ddlItem" runat="server" class="form-control input-sm" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="True" Font-Bold="True" Font-Size="12pt" >
                </asp:DropDownList>

   Resultado: <asp:DropDownList ID="ddlResultado" runat="server" 
                                ToolTip="Seleccione el resultado a informar" TabIndex="1" class="form-control input-sm" Font-Bold="True" Font-Size="12pt" AutoPostBack="True" OnSelectedIndexChanged="ddlResultado_SelectedIndexChanged"  >
                            </asp:DropDownList>

                      <asp:RangeValidator ID="rvItem" runat="server" ControlToValidate="ddlItem" ErrorMessage="RangeValidator" MaximumValue="99999999" MinimumValue="1" Type="Integer" ValidationGroup="0">Debe seleccionar un evento</asp:RangeValidator>

                </p>
                <p> 
                    <asp:RadioButtonList ID="rdbEstado" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="0">Pendientes de Enviar</asp:ListItem>
                        <asp:ListItem Value="1">Enviados por Interoperabilidad</asp:ListItem>
                        <asp:ListItem Value="2">Excluidos de enviar</asp:ListItem>
                    </asp:RadioButtonList> </p>

     <asp:Button ID="btnBuscar" runat="server" onclick="btnBuscar_Click"   CssClass="btn btn-primary" Width="150px"
                Text="Buscar" ValidationGroup="0" />

               
                
                
            </div>
         
            <div>
                
               <asp:Label ID="estatus" runat="server" 
                    Style="color:red"></asp:Label>

                <asp:Button ID="btnDescargarExcelControl"  runat="server" Text="Descargar Excel Control" OnClick="btnDescargarExcelControl_Click"  CssClass="btn btn-success" Width="150px" Visible="False" />
            </div>
            <hr />
                </td></tr>
                <tr>
              

                <td align="left">  
                  <div  > Seleccionar: <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton> <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
            
                   

                    </div> 
        </td>
                <td align="right">   
                  
               

                    <input id="HdIdMuestra" type="hidden" runat="server" />
                  
                <input id="HdIdTipoMuestra" type="hidden" runat="server" />
                         <input id="HdIdPrueba" type="hidden" runat="server" />
                  
                <input id="HdIdTipoPrueba" type="hidden" runat="server" />
                     <input id="HdidResultadoSISA" type="hidden" runat="server" />
                           <input id="HdidEventoSISA" type="hidden" runat="server" />
                       <input id="HidItemSIL" type="hidden" runat="server" />

                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
   

     <asp:Button ID="btnSISA" runat="server" onclick="btnGuardar_Click"   CssClass="btn btn-primary" Width="150px"
                Text="Informar a SISA" Visible="False" />

   

        </td>
                </tr> </table>
                        </div>
                    <div class="panel-footer">
                		<div style="border: 1px solid #999999; height: 450px; width:1080px; overflow: scroll; background-color: #EFEFEF;"> 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"
                DataKeyNames="idDetalleProtocolo" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="10pt"
                EmptyDataText="No se encontraron resultados para incorporar"  Font-Bold="True">
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="numero"   HeaderText="Numero" >
               
                <ItemStyle Width="10%" />
               
                </asp:BoundField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
            
                <ItemStyle Width="10%" />
            
                </asp:BoundField>
                
                
                <asp:BoundField DataField="apellido" HeaderText="Apellido" >
                
                
                  <ItemStyle Width="15%" />
                </asp:BoundField>
                
                
                  <asp:BoundField DataField="nombre" HeaderText="Nombre"  >
                      
                <ItemStyle Width="20%" />
                </asp:BoundField>
                
                        
         
                        
                <asp:BoundField DataField="resultadoCar" HeaderText="Resultado SIL" >
                
                        
         
                        
                <ItemStyle Width="20%" />
                </asp:BoundField>
                
                        
         
                        
                <asp:BoundField DataField="fechatoma" HeaderText="Fecha Toma" >
                        
                <ItemStyle Width="10%" />
                </asp:BoundField>
                        
                <asp:BoundField DataField="nombreDet" HeaderText="Determinacion" >
                <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="IdMuestraSISA"  HeaderText="idM" >
            
                <ItemStyle Width="2%" />
            
                </asp:BoundField>
                <asp:BoundField DataField="idTipoMuestraSISA" HeaderText="idTM" >
            
                <ItemStyle Width="2%" />
            
                </asp:BoundField>               
                             
                <asp:BoundField DataField="IdPruebaSISA"  HeaderText="IdP" >
            
                <ItemStyle Width="2%" />
            
                </asp:BoundField>
                <asp:BoundField DataField="idTipoPruebaSISA" HeaderText="idTP" />
                  <asp:BoundField DataField="idResultadoSISA" HeaderText="idREs" />
                      
                  <asp:BoundField DataField="idEvento" HeaderText="idEv" />
                  <asp:BoundField DataField="idcasosisa" HeaderText="idCasoSisa" />
                <asp:BoundField DataField="iditem" HeaderText="idDetSIL" />                 
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>

         </div>
   
     <asp:Button ID="btnNoInformarSISA" runat="server"   CssClass="btn btn-danger" Width="250px"  OnClientClick="return PreguntoExcluir();"
                Text="Descartar Informar a SISA" OnClick="btnNoInformarSISA_Click" Visible="False" />

                        </div>
      

        </div>

     
        
    </div>

     <script language="javascript" type="text/javascript"> 
    function PreguntoExcluir()
    {
    if (confirm('¿Está seguro de excluir del envío?'))
    return true;
    else
        return false;

    }

        
    </script>

</asp:Content>