<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit2.aspx.cs" Inherits="WebLab.AutoAnalizador.Incca.ConfiguracionEdit2" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
    <style type="text/css">
        .myTexto
        {}
    </style>



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
  
		<div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading"> 
    <h3 class="panel-title"> EQUIPO INCAA - Diconex</h3>
                        </div>

				<div class="panel-body">
		
<table align="center" width="600">
 
<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Area:</td>
						<td style="width: 475px">
                            <anthem:DropDownList ID="ddlArea" runat="server" CssClass="form-control input-sm"
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged"></anthem:DropDownList>
                                        
                        </td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Análisis del LIS:</td>
						<td style="width: 475px">
                                        
                            <anthem:DropDownList ID="ddlItem" runat="server" CssClass="form-control input-sm"></anthem:DropDownList>
                                        
                            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                ControlToValidate="ddlItem" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px; vertical-align: top;" >
                                            Método del Equipo:<asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                runat="server" ControlToValidate="txtOrden" ErrorMessage="*" 
                                ValidationGroup="0"></asp:RequiredFieldValidator>
                                        
                                                </td>
						<td class="myLabelIzquierda" style="width: 475px">
                            <asp:TextBox ID="txtOrden" runat="server" CssClass="form-control input-sm" Width="200px" 
                                MaxLength="50"></asp:TextBox>    <br />
                                                &nbsp;(hasta 50 caracteres)
                                        
                                                </td>
						
					</tr>
  	<tr>
						<td class="myLabelIzquierda" style="vertical-align: top; width: 115px;" >
                                            Prefijo especial:</td>
						<td class="myLabelIzquierda"  style="width: 475px">
                            <asp:TextBox ID="txtPrefijo" runat="server" CssClass="form-control input-sm" Width="100px" 
                                MaxLength="50"></asp:TextBox>
                                        <br />
                                                &nbsp;(Sólo para casos como Glucemia Post ingesta, Glucemia Post Almuerzo, etc.)</td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
						<td style="width: 475px">
                                              <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                                  onclick="btnGuardar_Click2"  CssClass="btn btn-primary" Width="100px" ValidationGroup="0" />
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
                                              <asp:Label ID="lblMensajeValidacion" runat="server" 
                                                  ForeColor="#FF3300"></asp:Label>
                        </td>
						
					</tr>
				 
					</table>	 
		
		</div>			
       	<div class="panel-footer">
               			<div style="border: 1px solid #999999; height: 500px; width:610px; overflow: scroll;">
                                <asp:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table"  AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idInccaItem" onrowcommand="gvLista_RowCommand" 
                                    onrowdatabound="gvLista_RowDataBound" 
                                    EmptyDataText="No se configuraron prácticas para el equipo" 
                                    EnableModelValidation="True">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="idIncca" HeaderText="ID en Equipo" />
                                       
                                           
                                         <asp:BoundField DataField="prefijo" HeaderText="Prefijo" />
                                       
                                           
                                         <asp:TemplateField HeaderText="Habilitado">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkStatus" runat="server" 
                            AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged"
                            Checked='<%# Convert.ToBoolean(Eval("Habilitado")) %>'
                            />
                    </ItemTemplate>                    
                </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" 
                                                                                ImageUrl="../../App_Themes/default/images/eliminar.jpg" 
                                                                                OnClientClick="return PreguntoEliminar();" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                                    </asp:TemplateField>
                                    </Columns>
                                    
                                    <HeaderStyle BackColor="#999999" />
                                </asp:GridView>
					</div>
               </div>	
       </div>
            </div>      
                            
                      
 <script language="javascript" type="text/javascript">

    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de eliminar?'))
        return true;
    else
        return false;
    }
    </script>
 </asp:Content>
