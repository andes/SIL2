<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeticionEdit.aspx.cs" Inherits="WebLab.PeticionElectronica.PeticionEdit"  MasterPageFile="~/Site2.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

 <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     <link rel="stylesheet" type="text/css" href="../App_Themes/default/style.css" />
    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  
    <script type="text/javascript"      src="../script/jquery.min.js"></script> 
    <script type="text/javascript"      src="../script/jquery-ui.min.js"></script>     
   	<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <script src="../script/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>         
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 

      
<script type="text/javascript">

    $(function () {
        $("#tabContainer").tabs();
        $("#tabContainer").tabs({ selected: 0 });
    });


    function printdiv(printpage) {
        var ficha = document.getElementById(printpage);
        var ventimp = window.open(' ', 'popimpr');
        ventimp.document.write(ficha.innerHTML);
        ventimp.document.close();
        ventimp.print();
        ventimp.close();
    }           
</script> 
   
   
     
     <style type="text/css">
         .auto-style1 {
             border-style: none;
             font-size: 8pt;
             font-family: Arial, Helvetica, sans-serif;
             background-color: #F3F3F3;
             color: #333333;
             font-weight: normal;
             width: 133px;
         }
     </style>
   
   
     
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
<input id="hdidPaciente" type="hidden" runat="server" />
    <div align="left" style="width: 900px">            
        <asp:ValidationSummary ID="VSLaboratorio" runat="server" DisplayMode="List" HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" ShowSummary="False" ValidationGroup="0" />
        <asp:ValidationSummary ID="VSMicrobiologia" runat="server" DisplayMode="List" HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" ShowSummary="False" ValidationGroup="1" />   
         <p class="h2_alt">   SISTEMA INFORMATICO PROVINCIAL DE LABORATORIO</p>
       	<table class="mytable1" width="100%" >
            <tr>
            <td class="myLabelIzquierdaFondo">Apellido:</td>
                <td  >
        <asp:Label ID="lblApellido" runat="server" Text="Label" CssClass="myLabelIzquierdaGde"></asp:Label>
                </td>
                <td class="myLabelIzquierdaFondo">
                    Nombre:</td>
                <td  >
        <asp:Label ID="lblNombre" runat="server" Text="Label" CssClass="myLabelIzquierdaGde"></asp:Label>
                </td>
                <td class="myLabelIzquierdaFondo">
                    DU:</td>
                <td  >
        <asp:Label ID="lblDU" runat="server" Text="Label" CssClass="myLabelIzquierdaGde"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="myLabelIzquierdaFondo">
                    Sexo:</td>
                <td class="myLabelIzquierda">
        <asp:Label ID="lblSexo" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>
                </td>
                <td class="myLabelIzquierdaFondo">
                    Fecha Nac.:</td>
                <td class="style16">
        <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>
                </td>
                <td class="myLabelIzquierdaFondo">
                    Usuario:</td>
                <td class="myLabelIzquierda">
                <asp:Label ID="lblSolicitante" Visible="true" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>
   <%--     <asp:Label ID="lblEstado" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>--%>
   
                    <%-- <asp:Label ID="lblNumeroPeticion" runat="server" Text="Label" CssClass="myLabelRojo" 
            Font-Bold="True"></asp:Label>--%>
  
                </td>
            </tr>
            </table>
      
         <div> &nbsp;
                  <asp:Label ID="lblAlerta" runat="server" Text="Label" CssClass="myLabelRojo" ></asp:Label>
        
             </div>
     <table class="mytable1" width="100%" >
            <tr>
                <td class="auto-style1" style="vertical-align: top">
                    Origen/Sector:   
                <asp:RangeValidator ID="rvOrigenLaboratorio" runat="server"  ControlToValidate="ddlOrigen" ErrorMessage="Origen" MaximumValue="999999"  MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                <asp:RangeValidator ID="rvOrigenMicrobiologia" runat="server" ControlToValidate="ddlOrigen" ErrorMessage="Origen" MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="1">*</asp:RangeValidator>
                <asp:RangeValidator ID="rvSectorServicioLaboratorio" runat="server" ControlToValidate="ddlSectorServicio" ErrorMessage="Sector" MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                <asp:RangeValidator ID="rvSectorServicioMicrobiologia" runat="server" ControlToValidate="ddlSectorServicio" ErrorMessage="Sector" MaximumValue="999999"  MinimumValue="1" Type="Integer" ValidationGroup="1">*</asp:RangeValidator>
                </td>
                <td colspan="2"  >
                    <asp:DropDownList ID="ddlOrigen" runat="server"  
                                ToolTip="Seleccione el origen" CssClass="ddlBordeAzul">
                            </asp:DropDownList>                                        
                                        <asp:DropDownList ID="ddlSectorServicio" CssClass="ddlBordeAzul" runat="server" TabIndex="1" Width="200px">
                                        </asp:DropDownList>

                </td>
                <td class="myLabelIzquierdaFondo" align="right">
                   Ubicación(Sala/Cama/Box): &nbsp;</td>
                <td  >
                    <asp:TextBox ID="txtSala" CssClass="myTexto" runat="server" Width="50px" TabIndex="2"></asp:TextBox>
                    &nbsp;<asp:TextBox ID="txtCama" CssClass="myTexto" runat="server" Width="50px" TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style1" style="vertical-align: top">
                    Fecha Pedido:</td>
                <td>
        <asp:Label ID="lblFecha" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>
        <asp:Label ID="lblHora" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>
                </td>
                <td class="myLabelIzquierdaFondo" align="right">
                    Médico Solicitante: &nbsp;</td>
                <td colspan="2">
                 <asp:DropDownList ID="ddlEspecialista" runat="server" CssClass="ddlBordeAzul" ToolTip="Seleccione el médico solicitante" TabIndex="4" ></asp:DropDownList>                                        
        
                </td>
            </tr>
            <tr>
            <td class="auto-style1" style="vertical-align: top">Observaciones:
         
                </td>
                 <td colspan="4">          
                     <asp:TextBox ID="txtObservaciones" runat="server" Width="80%" TabIndex="5" Rows="1" TextMode="Password"></asp:TextBox>                                           
        <div class="myLabelLitlle">*no ingrese pruebas en las observaciones.</div>
                </td>
            </tr>
            </table>&nbsp;
    <table>
     <tr>
<td align="left" style="vertical-align: top">

<div id="tabContainer"  style="width:100%; z-index:200; position:relative; top: 0px; left: 0px;" >  
    <ul style="font-size: small">         
        <li><a href="#tab1">Nueva Petición</a></li>
     <%--   <li><a href="#tab3">Microbiologia</a></li>--%>
        <li><a href="#tab2">Peticiones del Paciente</a></li> 
        
    </ul>      
                          		      
<div id="tab1" class="tab_content" style="width: 95%;height:450px;" >
                   <table width="95%" class="myLabelIzquierda" >
        <tr>
        <td><asp:DropDownList ID="ddlRutina" CssClass="ddlBordeAzul" runat="server" 
                TabIndex="6" 
                onselectedindexchanged="ddlRutina_SelectedIndexChanged" 
                AutoPostBack="True" ></asp:DropDownList> &nbsp;&nbsp; <anthem:LinkButton Visible="false" AutoCallBack="True" ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                  >Marcar todas</anthem:LinkButton>&nbsp;
                                            <anthem:LinkButton Visible="false" AutoCallBack="True" ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                 >Desmarcar todas</anthem:LinkButton>

        </td>
    
       <td></td>   
        </tr>                   
    
     <tr>
    <td style="vertical-align: top" class="myLabelIzquierda">               
        <div title="Detalle de Petición Electrónica" id="divLaboratorio" style="border: 1px solid #C0C0C0; color: #FFFFFF;height:380px;   width:600px; overflow: scroll;"  align="left" >
        <asp:DataList ID="DataList1" runat="server" width="99%" RepeatColumns="1" RepeatDirection="Horizontal"  onitemdatabound="DataList1_ItemDataBound"  CellPadding="0"  TabIndex="7">                                            
<ItemTemplate>                                                                            
<div class="h1_alt"> <anthem:CheckBox ID="CheckBox1"  OnCheckedChanged="CheckBox1_CheckedChanged" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "area") %>' AutoCallBack="True" /><asp:Label Visible="false"  Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "area") %>' ID="lblArea" >   </asp:Label> </div>

<asp:Label Visible="false" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "idArea") %>' ID="lblIdArea">
    </asp:Label>    
<div  style=" width:100%;" >
            <anthem:CheckBoxList ID="chkAnalisis" runat="server"  Width="100%" AutoCallBack="True" CssClass="myLabelIzquierda"
                   onselectedindexchanged="chkAnalisis_SelectedIndexChanged" RepeatColumns="3" RepeatLayout="Table" RepeatDirection="Vertical" CellPadding="1" CellSpacing="5" TextAlign="Right" >
            </anthem:CheckBoxList>
</div>
                                                 
    
               
   </ItemTemplate>

                                            </asp:DataList>
         
        </div>    
        &nbsp;</td>


        <td style="vertical-align: top"><b>Prácticas Seleccionadas</b>
           <anthem:ListBox ID="lstSeleccion" runat="server" CssClass="myList" Width="200px" 
                Height="300px" SelectionMode="Multiple"></anthem:ListBox>
            <br />
           <asp:LinkButton ID="imgBorrarSeleccionLaboratorio" CssClass="myLittleLink" ToolTip="Borrar Selección"  onclick="imgBorrarSeleccionLaboratorio_Click"  runat="server">Borrar Prácticas</asp:LinkButton>
           
            <br />
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Peticion" 
            CssClass="myButton"  Width="160px" 
            ValidationGroup="1" Visible="False" onclick="btnGuardar_Click" />

            <br />

        <asp:Button ID="btnGuardarEnviar" runat="server" Text="ENVIAR PETICION" CssClass="myButtonRojo" Width="200px" Height="30px" ValidationGroup="0" 
            onclick="btnGuardarEnviar_Click" TabIndex="8" />     

        </td>
        </tr>

        <tr>
        <td>
            
  
  
<%--<anthem:TextBox ID="txtSeleccion" runat="server" Font-Bold="True" CssClass="myLabelRojo" Font-Size="10pt" Rows="2" TextMode="MultiLine" Width="600px" BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True"></anthem:TextBox>--%>
        <img src="../App_Themes/default/images/imprimir.jpg"  onclick="printdiv('divLaboratorio');"/><anthem:Label  ID="lblIdSeleccion" runat="server" Font-Bold="True" ForeColor="#0000CC" Visible="False"></anthem:Label>
<asp:RequiredFieldValidator ID="rfSeleccionLaboratorio" runat="server"  
                ControlToValidate="lstSeleccion" ErrorMessage="Seleccionar al menos una prueba" 
                ValidationGroup="0">*</asp:RequiredFieldValidator>

         
 
                                      
                </td>
            <td>

                &nbsp;</td>
        </tr>           
                </table>
                                            </div>

                                            <div id="tab2" class="tab_content"  style="width: 100%;height:350px;">

                                            <table style="width: 95%;">
      
        <tr>
            <td>
                
              <div align="left" id="divLista"  style="border: 1px solid #C0C0C0; overflow:scroll; overflow-x:hidden; height:340px; background-color: #F8F8F8; ">
                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idPeticion" 
                         EmptyDataText="No hay peticiones generadas" 
                    Width="780px" onrowcommand="gvLista_RowCommand" 
                    onrowdatabound="gvLista_RowDataBound" Font-Names="Arial" 
                    Font-Size="9pt">
                    <Columns>
                            <asp:BoundField DataField="idPeticion" HeaderText="Peticion" >
                            <ItemStyle Width="10%"  CssClass="myLabelIzquierda" HorizontalAlign="Left" VerticalAlign="Top"/>
                               
                        </asp:BoundField>
                          <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                            <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>
                         <asp:BoundField DataField="origen" HeaderText="Origen" >
                           <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>
                       <asp:BoundField DataField="sector" HeaderText="Sector" >
                            <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="solicitante" HeaderText="Solicitante" >
                            <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>   
                        <asp:BoundField DataField="usuario" HeaderText="Grabado por" >
                            <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>
                          <asp:BoundField DataField="item"  HeaderText="Prácticas Solicitadas" >
                            <ItemStyle Width="25%" CssClass="myLittleLink2"  HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="observaciones" HeaderText="Observaciones" >
                            <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="protocolo" HeaderText="Protocolo" >
                            <ItemStyle Width="5%"  CssClass="myLabelIzquierda" HorizontalAlign="Left" VerticalAlign="Top"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="estado" HeaderText="Estado" >
                              <ItemStyle  CssClass="myLabelIzquierda" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                          
                          <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="Imprimir" runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg"  CommandName="Imprimir"  />
                            </ItemTemplate>                          
                            <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" VerticalAlign="Top" />                          
                        </asp:TemplateField>
                           <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton  ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" VerticalAlign="Top"  />
                          
                        </asp:TemplateField>
                        
                     
                    </Columns>
                    <HeaderStyle 
                        HorizontalAlign="Left" />
                </asp:GridView>
               </div>
            </td>
           
        </tr>
      
    </table>


    
                                            </div>


                                            


<div runat="server" visible="false"  id="tab3" class="tab_content" style="width: 100%;height:500px;">
<table width="95%" class="myLabelIzquierda">
<tr>
<td ><asp:DropDownList ID="ddlMuestra" Visible="false" runat="server"></asp:DropDownList> </td>
<td align="right"><img src="../App_Themes/default/images/imprimir.jpg"  onclick="printdiv('divMicrobiologia');"/></td>    
</tr>    
<tr>
    <td colspan="2" style="vertical-align: top" class="myLabelIzquierda">      
        <div title="Detalle de Petición Electrónica Microbiología" id="divMicrobiologia"  style="border: 1px solid  #C0C0C0; color: #FFFFFF;height:400px;width:100%; overflow: scroll;"  align="left" >
        <asp:DataList ID="datalistMicrobiologia" runat="server" width="100%" RepeatColumns="1" RepeatDirection="Horizontal"  onitemdatabound="datalistMicrobiologia_ItemDataBound" 
            CellPadding="0" CellSpacing="0">                                            
<ItemTemplate>                              
 <div style="background-color:#4A7058;font-family:Tahoma,Arial,sans-serif;font-size:11px;height:20px;">
  &nbsp; &nbsp; <b><%# DataBinder.Eval(Container.DataItem, "area") %></b>   <asp:Label Visible="false" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "idArea") %>' ID="lblIdArea"> </asp:Label>    
   </div>  
<div  style="border: 1px solid #999999; color: #000000;width: 100%; " >
            <anthem:CheckBoxList ID="chkAnalisisMicrobiologia" runat="server"  CssClass="myLabelIzquierda" RepeatColumns="2" Width="100%" AutoCallBack="True"  onselectedindexchanged="chkAnalisisMicrobiologia_SelectedIndexChanged">
            </anthem:CheckBoxList>
            </div>                                                         
               
   </ItemTemplate>

                                            </asp:DataList></div>   
                                            
                                            </td> 
                                            </tr>
    
                                            <tr>
                              <td align="left" style="vertical-align: top">
<anthem:TextBox ID="txtSeleccionMicrobiologia" runat="server" Font-Bold="True"   CssClass="myLabelRojo" Font-Size="10pt" Rows="1" TextMode="MultiLine" Width="80%" BorderColor="#333333" BackColor="#E2E2E2" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True"></anthem:TextBox>
<anthem:Label ID="lblIdSeleccionMicrobiologia" runat="server" Font-Bold="True" ForeColor="#0000CC" Visible="False"></anthem:Label>                 
<asp:LinkButton ID="imgBorrarSeleccionMicrobiologia" CssClass="myLittleLink" ToolTip="Borrar Selección" onclick="imgBorrarSeleccionMicrobiologia_Click"  runat="server">Borrar Prácticas</asp:LinkButton>
<anthem:RequiredFieldValidator ID="rfSeleccionMicrobiologia" ControlToValidate="txtSeleccionMicrobiologia" ValidationGroup="1" runat="server" ErrorMessage="Debe seleccionar una practica"></anthem:RequiredFieldValidator> 
<br />

        <asp:Button ID="btnGuardarMicrobiologia" runat="server" Text="Enviar Petición Microbiología" 
            CssClass="myButton" Width="200px" ValidationGroup="1" 
            onclick="btnGuardarMicrobiologia_Click" />       <br />                             
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                                    ControlToValidate="txtSeleccionMicrobiologia" ErrorMessage="CustomValidator" ValidationGroup="1"
                                                    onservervalidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                                            
            </td>
            <td align="right" style="vertical-align: top">
                <asp:CheckBox ID="chkIgnorarValidacionMicrobiologia" runat="server" 
                                                    AutoPostBack="True" 
                                                    oncheckedchanged="chkIgnorarValidacionMicrobiologia_CheckedChanged" 
                                                    Text="Ignorar mensaje y guardar" 
                    Visible="False" />
                <asp:RadioButtonList Visible="false" onselectedindexchanged="rdbTipoPlanilla_SelectedIndexChanged" CssClass="myLabelIzquierda" ID="rdbTipoPlanilla" runat="server"  RepeatDirection="Horizontal" AutoPostBack="True">
                                   <asp:ListItem Selected="True"  Value="0">Perfiles</asp:ListItem>
                                   <asp:ListItem Value="1">Completo</asp:ListItem>
                               </asp:RadioButtonList>
                               
                               
            </td>
            
            </tr>
                           
                           </table>
                                            </div>



            </div>                             
                                            
</td>
    </tr>
    
       
            <tr>
    <td style="vertical-align: top" class="myLabelIzquierda">
        
    </td>
           </tr>
      </table>      
      <input id="hidToken" type="hidden" runat="server" />
  
   
        <script language="javascript" type="text/javascript">

            function PreguntoEliminar() {
                if (confirm('¿Está seguro de eliminar la petición?'))
                    return true;
                else
                    return false;
            }
    </script>
    </asp:Content>

