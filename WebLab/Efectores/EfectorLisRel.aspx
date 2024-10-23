<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EfectorLisRel.aspx.cs" Inherits="WebLab.Efectores.EfectorLisRel" MasterPageFile="~/Site1.Master" %>


<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">           
 
    	 <div align="left" style="width:800px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">Efectores Vinculados al Laboratorio</h3>
                        </div>

				<div class="panel-body">
                    
                    
                  
                     Efector:    <asp:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" AutoPostBack="True" OnSelectedIndexChanged="ddlEfector_SelectedIndexChanged" >
                            </asp:DropDownList>
                    <br />
                    Zona:    <asp:DropDownList ID="ddlZona" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione la zona" AutoPostBack="True" OnSelectedIndexChanged="ddlZona_SelectedIndexChanged"  >
                            </asp:DropDownList>
                    <br />
                    Que contenga: <anthem:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="400px" ></anthem:TextBox>
                    
                                    <anthem:Button ID="btnBuscar" runat="server"  
                                        Text="Buscar" CssClass="btn btn-primary" Width="100px" OnClick="btnBuscar_Click" />
		
                    <hr />
		

    
        <table>
            <tr>
             <td class="auto-style1">                                               
                                            
                                        <strong>      Efectores Encontrados</strong>
                 <br /><anthem:ListBox ID="lstEfector" runat="server" AutoCallBack="True" 
                                                     class="form-control input-sm"  Height="400px" Width="300px">
                                               </anthem:ListBox>
                                              
                                             </td>                                         
                                             <td style="vertical-align: top">
                                            
                                                 <br />
 <anthem:Button ID="btnAgregarEfector" runat="server" Text="Agregar efector" CssClass="btn btn-primary" Width="120px"
                                                      OnClick="btnAgregarEfector_Click" 
                                                     />
                                      	                 <br />                        
                                     <div>
                                                <br />
                                                 <anthem:Button ID="btnSacarEfector" runat="server" CssClass="btn btn-primary" Width="120px"
                                                      Text="Sacar Efector" OnClick="btnSacarEfector_Click"
                                                   />
                                    </div>
                                     
                                                 <br />
                                    <anthem:Button ID="btnSacarTodos" runat="server"  
                                        Text="Sacar Todos" CssClass="btn btn-primary" Width="120px" OnClick="btnSacarTodos_Click"   />
		

    
                                             </td>   
                                
                               
                                             <td class="auto-style1">
                                                 &nbsp;</td>                                         
                                             <td style="vertical-align: top">
                                                 &nbsp;</td>   
                                
                                
                                             <td class="auto-style1">
                                                 
                                          <strong>    Efectores Vinculados</strong>
                 		
         <anthem:Label ID="estatus" runat="server" 
                    Style="color: red"></anthem:Label>


                 <br />
                                                 <anthem:ListBox ID="lstEfectorVinculado" runat="server"    class="form-control input-sm"
                                                     Height="400px" Width="300px" SelectionMode="Multiple">
                                                 </anthem:ListBox></td>                                         
                                             <td style="vertical-align: top">
                                    
                                                 &nbsp;</td>   

                                    </tr>
            <tr>
             <td class="auto-style1">                                               
                                            
                                              &nbsp;</td>                                         
                                             <td style="vertical-align: top">
                                            
                                                 &nbsp;</td>   
                                
                               
                                             <td class="auto-style1">
                                                 &nbsp;</td>                                         
                                             <td style="vertical-align: top">
                                                 &nbsp;</td>   
                                
                                
                                             <td class="auto-style1">
                                                 
                                    <asp:Button ID="btnGuardar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Guardar" CssClass="btn btn-primary" Width="100px" />
		

                </td>                                         
                                             <td style="vertical-align: top">
                                    
                                                 &nbsp;</td>   

                                    </tr>
        </table>

        </div>


   </div>
             </div>

    
        
    <script type="text/javascript" language="javascript">
    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de eliminar el registro?'))
    return true;
    else
    return false;
    }
    </script>
</asp:Content>