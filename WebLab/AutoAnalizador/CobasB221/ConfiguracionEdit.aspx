<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit.aspx.cs" Inherits="WebLab.AutoAnalizador.CobasB221.ConfiguracionEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
  
	<div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading"> 
    <h3 class="panel-title">Configuración SIL - EQUIPO COBAS b 221</h3>
                        </div>

				<div class="panel-body">
		
<table align="center" width="600" >

<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Area:</td>
						<td style="width: 475px">
                            <anthem:DropDownList ID="ddlArea" runat="server" class="form-control input-sm"
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged"></anthem:DropDownList>
                                        
                        </td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Análisis del LIS:</td>
						<td style="width: 475px">
                                        
                            <anthem:DropDownList ID="ddlItem" runat="server" class="form-control input-sm"
                                
                               >
                            </anthem:DropDownList>
                                        
                            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                ControlToValidate="ddlItem" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            ID en Equipo:</td>
						<td style="width: 475px">
                            <asp:DropDownList ID="ddlItemEquipo" class="form-control input-sm" runat="server" Width="150px">
                                <asp:ListItem Selected="True" Value="0">Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">pH</asp:ListItem>
                                <asp:ListItem Value="2">H+</asp:ListItem>
                                <asp:ListItem Value="3">PO2</asp:ListItem>
                                <asp:ListItem Value="4">PCO2</asp:ListItem>
                                <asp:ListItem Value="5">Hct</asp:ListItem>
                                <asp:ListItem Value="6">Na</asp:ListItem>
                                <asp:ListItem Value="7">K</asp:ListItem>
                                <asp:ListItem Value="8">Ca</asp:ListItem>
                                <asp:ListItem Value="9">Cl</asp:ListItem>
                                <asp:ListItem Value="10">tHb</asp:ListItem>
                                <asp:ListItem Value="11">SO2</asp:ListItem>
                                <asp:ListItem Value="12">O2Hb</asp:ListItem>
                                <asp:ListItem Value="13">COHb</asp:ListItem>
                                <asp:ListItem Value="14">MetHb</asp:ListItem>
                                <asp:ListItem Value="15">HHb</asp:ListItem>
                                <asp:ListItem Value="16">Bili</asp:ListItem>
                                <asp:ListItem Value="17">Glu</asp:ListItem>
                                <asp:ListItem Value="18">Lac</asp:ListItem>
                                <asp:ListItem Value="19">Urea</asp:ListItem>
                                <asp:ListItem Value="20">BUN</asp:ListItem>
                                <asp:ListItem Value="21">Baro</asp:ListItem>                                                                
                                <asp:ListItem Value="555">---Parametros calculados---</asp:ListItem>                                                                
                                <asp:ListItem Value="22">cHCO3</asp:ListItem>                                                                
                                <asp:ListItem Value="23">ctCO2(P)</asp:ListItem>                                                                
                                 <asp:ListItem Value="24">BE</asp:ListItem>                                                                
                                <asp:ListItem Value="25">BE(act)</asp:ListItem>                                                                
                                 <asp:ListItem Value="26">BEecf</asp:ListItem>                                                                
                                <asp:ListItem Value="27">BB</asp:ListItem>                                                                
                                 <asp:ListItem Value="28">SO2(c)</asp:ListItem>                                                                
                                <asp:ListItem Value="29">P50</asp:ListItem>                                                                
                                 <asp:ListItem Value="30">ctO2</asp:ListItem>                                                                
                                <asp:ListItem Value="31">ctCO2(B)</asp:ListItem>                                                                
                                 <asp:ListItem Value="32">pHst</asp:ListItem>                                                                
                                <asp:ListItem Value="33">cHCO3st</asp:ListItem>                                                                
                                <asp:ListItem Value="34">PAO2</asp:ListItem>                                                                
                                 <asp:ListItem Value="35">AaDO2</asp:ListItem>                                                                
                                <asp:ListItem Value="36">a/AO2</asp:ListItem>                                                                
                                 <asp:ListItem Value="37">avDO2</asp:ListItem>                                                                
                                <asp:ListItem Value="38">RI</asp:ListItem>                                                                
                                 <asp:ListItem Value="39">Qs/Qt</asp:ListItem>                                                                
                                <asp:ListItem Value="40">niCa</asp:ListItem>                                                                
                                 <asp:ListItem Value="41">AG</asp:ListItem>                                                                
                                <asp:ListItem Value="42">pHt</asp:ListItem>                                                                
                                 <asp:ListItem Value="43">cHt</asp:ListItem>                                                                
                                <asp:ListItem Value="44">PCO2t</asp:ListItem>                                                                
                                 <asp:ListItem Value="45">PO2t</asp:ListItem>                                                                
                                <asp:ListItem Value="46">PAO2t</asp:ListItem>                                                                
                                 <asp:ListItem Value="47">AaDO2t</asp:ListItem>                                                                
                                <asp:ListItem Value="48">a/AO2t</asp:ListItem>                                                                
                                 <asp:ListItem Value="49">Rit</asp:ListItem>                                                                
                                <asp:ListItem Value="50">Hct(c)</asp:ListItem>                                                                
                                 <asp:ListItem Value="51">MCHC</asp:ListItem>                                                                
                                <asp:ListItem Value="52">Osm</asp:ListItem>                                                                
                                 <asp:ListItem Value="53">OER</asp:ListItem>                                                                
                                <asp:ListItem Value="54">BO2</asp:ListItem>                                                                
                                 <asp:ListItem Value="55">BUN</asp:ListItem>                                                                
                                <asp:ListItem Value="56">QT</asp:ListItem>                                                                
                                 <asp:ListItem Value="57">C</asp:ListItem>                                                                
                                <asp:ListItem Value="58">PFIndex</asp:ListItem>     
                                 <asp:ListItem Value="59">FO2Hb</asp:ListItem>                                                                
                                
                            </asp:DropDownList>
                                        
                                                <asp:RangeValidator ID="RangeValidator2" runat="server" 
                                ControlToValidate="ddlItemEquipo" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                                                </td>
						
					</tr>
    <%--<asp:BoundField DataField="tipoMuestra" HeaderText="Tipo de Muestra" />
                                        <asp:BoundField DataField="prefijo" HeaderText="Prefijo Especial" />--%>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
						<td style="width: 475px">
                                              <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                                  onclick="btnGuardar_Click2"  Width="100px"  CssClass="btn btn-primary" ValidationGroup="0" />
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
                                              <asp:Label ID="lblMensajeValidacion" runat="server" 
                                                  ForeColor="#FF3300"></asp:Label>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
                                           <hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
					
						<div style="border: 1px solid #999999; height: 600px; width:610px; overflow: scroll;">
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idCobasb221Item" onrowcommand="gvLista_RowCommand" 
                                    onrowdatabound="gvLista_RowDataBound"  CssClass="table table-bordered bs-table"
                                    EmptyDataText="No se configuraron prácticas para el equipo">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="idCobas" HeaderText="ID en Equipo" />
                                       
                                           
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
					
					    </td>
						
					</tr>
					</table>
                   

		
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
