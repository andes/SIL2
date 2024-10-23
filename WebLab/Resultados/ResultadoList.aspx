<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoList.aspx.cs" Inherits="WebLab.Resultados.ResultadoList" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
 
   <link href="jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                   <script type="text/javascript">                     

             $(function() {
                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                        $("#tabContainer").tabs({ selected: currTab });
             });

                   
      
                  </script> 

  
    <style type="text/css">
        .style1
        {
        }
        </style>

    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
    <br />  &nbsp;    
    
<div align="left" style="width: 1000px">
   <asp:HiddenField runat="server" ID="HFIdPaciente" /> 
                 <table  width="1000px"                    
                     
                     style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: normal; color: #000000" 
                     cellpadding="1" cellspacing="1" align="center">
					
					
					<tr>
						<td   
                            
                            
                            style="font-family: Arial; font-size: 14px; font-weight: bold; vertical-align: top;" 
                            colspan="3">
                            Resultados del Paciente</td>
					</tr>
						
					
					
					<tr>
						<td   
                            
                            
                            style="font-family: Arial; font-size: 14px; font-weight: bold; vertical-align: top;" 
                            colspan="3">
                            <hr /></td>
					</tr>
						
					
					
					<tr>
						<td   
                            
                            
                            
                            colspan="3">
                            <table >
                                <tr>
                                    <td class="myLabelIzquierda">
                                        Paciente:</td>
                                    <td class="style1" colspan="8">
                            <asp:Label ID="lblPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt"></asp:Label>
                            <asp:Label ID="lblCodigoPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="myLabelIzquierda">
                                        DU:</td>
                                    <td class="myLabelIzquierda">
                            <asp:Label ID="lblDni" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt"></asp:Label>
                                    </td>
                                    <td class="myLabelIzquierda">
                                        &nbsp;&nbsp;&nbsp; &nbsp;</td>
                                    <td class="myLabelIzquierda">
                                        Sexo: 
                            <asp:Label ID="lblSexo" runat="server" Text="Label" CssClass="myLabel" ></asp:Label>
                                    </td>
                                    <td class="myLabelIzquierda" align="left">
                                        &nbsp;&nbsp; &nbsp;</td>
                                    <td class="myLabelIzquierda" align="left">
                                        Fecha Nacimiento: <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                                    </td>
                                    <td            class="myLabelIzquierda"   >
                                        &nbsp;&nbsp; &nbsp;</td>
                                    <td            class="myLabelIzquierda"   >
                                        Edad: <asp:Label ID="lblEdad" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>   
                                    </td>
                                    <td align="left">
                                        &nbsp;</td>
                                </tr>
                                </table>
                        </td>
					</tr>
						<tr>
                        <td colspan="3">&nbsp;</td>
                        </tr>
					
					
					
						
						<tr>
                        <td colspan="3">
                                      
                                      <%--  <div  style="width:400px;height:350pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> --%>
                                            <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                            DataKeyNames="idProtocolo" onrowcommand="gvLista_RowCommand" 
                                            onrowdatabound="gvLista_RowDataBound" CellPadding="3" 
                                            HorizontalAlign="Left" Font-Size="8pt" BorderColor="#3A93D2" BorderStyle="Solid" 
                                            BorderWidth="1px" GridLines="None" Font-Bold="False" ForeColor="#333333" 
                                                Width="400px" >
                                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" 
                                            Font-Size="8pt" />

                                            <Columns>
                                            <asp:BoundField DataField="numero" HeaderText="Protocolo" />
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                                             <asp:BoundField DataField="item" HeaderText="Detalle">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle Width="30%" HorizontalAlign="Center" CssClass="myLittleLink2" />
                                                                    </asp:BoundField>
                                            <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:ImageButton ID="Pdf" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                                            CommandName="Pdf" />
                                            </ItemTemplate>

                                            <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="estado" Visible="False" />

                                            </Columns>
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" 
                                            Font-Names="Arial" Font-Size="8pt" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
                                            </asp:GridView>
                                   <%--  </div>--%>


                                        </td>
                        </tr>
					
					
					
						
						<tr>
						
					
					
					
						
						<td   align="center" style="vertical-align: top">
                            <table style="width:180px;">
                                <tr>
                                   <td align="left">
    <asp:HyperLink ID="hypRegresar" AccessKey="r" ToolTip="Alt+Shift+R" runat="server" CssClass="myLink">Regresar</asp:HyperLink>
    <br />  &nbsp;    
    <br />  &nbsp;
      <asp:Label CssClass="myLabelGris" ID="lblCantidadRegistros" runat="server" Text="Label"></asp:Label>
                                      </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                      
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
						
					
					
					
						
						<td   align="left" style="vertical-align: top">
                            &nbsp;</td>
						
						<td style="vertical-align: top">
                            
                            </td>
						
					</tr>
											
					
					
					
						
						</table>						
 </div></div>
 <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">




    var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();

  
  function AntecedenteView(idAnalisis, idPaciente) {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }


        var $this = $(this);        

        $('<iframe src="AntecedentesAnalisisView.aspx?idAnalisis=' +  idAnalisis+ '&idPaciente='+idPaciente+'" />').dialog({
            title: 'Evolucion',
            autoOpen: true,
            width:790,
            height: 420,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(800);
    }
    function editPeticion() {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }


        var $this = $(this);
        $('<iframe src="../PeticionElectronica/PeticionEdit.aspx?idPaciente=' + idPaciente + '&Modifica=0&idTipoServicio=1" />').dialog({
            title: 'Nueva Peticion',
            autoOpen: true,
            width: 980,
            height: 600,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(980);
    }


    </script>

</asp:Content>

