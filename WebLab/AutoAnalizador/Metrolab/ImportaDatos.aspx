<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportaDatos.aspx.cs" Inherits="WebLab.AutoAnalizador.Metrolab.ImportaDatos" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                    

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
<br />&nbsp;
     <div>
     <table  width="1000px">
     <tr><td>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
         <td colspan="2">
<div class="mytituloTabla"            >
                Incorporar Resultados METROLAB </div>
             <hr />
            <div class="myLabelIzquierda2">
            Paso 1.- Haga clic en "Examinar" para seleccionar el archivo con los resultados del equipo                 <br />
            Paso 2.- Haga clic en "Procesar Archivo". Al terminar se mostraran los protocolos cuyos numeros se corresponden en el sistema.<br />
            Paso 3.- Seleccione los protocolos cuyos resultados desea guardar y hacer clic en "Guardar Resultados".
            
            <hr />
                Elegir el archivo con resultados:
               <asp:FileUpload ID="trepador" runat="server" CssClass="myButton" Width="500px" />
                <asp:Button CssClass="myButtonRojo" ID="subir" runat="server" Width="150px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />
            </div>
         
            <div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>
            <hr />
                </td></tr>
                <tr>
                <td>   &nbsp;</td>
                <td align="left">  
                  <div class="myLabelIzquierda" > Seleccionar: <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton>
                    <br />
                    </div> 
        </td>
                <td align="right">    <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
        </td>
                </tr>
                <tr>
                <td>   &nbsp;</td>
                <td colspan="2">  
                		<div style="border: 1px solid #999999; height: 450px; width:1000px; overflow: scroll; background-color: #EFEFEF;"> 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idProtocolo" CssClass="mytable2" Font-Names="Arial" Font-Size="8pt"
                EmptyDataText="No se encontraron resultados para incorporar" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="numero"   HeaderText="Protocolo" >
                <ItemStyle Width="5%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="origen" HeaderText="Origen">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="edad" HeaderText="Edad">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="sexo" HeaderText="Sexo">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                
                <asp:BoundField DataField="estado" HeaderText="Estado Protocolo">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                
                 
                
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>
         </div>
        </td>
                </tr>
                <tr>
                <td>   &nbsp;</td>
                <td colspan="2" align="right">  
                <hr />
            <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click"  CssClass="myButton" Width="150px"
                Text="Guardar Resultados" />
        </td>
                </tr>
     </table>

     <br /> &nbsp; <br /> &nbsp;
        </div>

        
</asp:Content>