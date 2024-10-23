<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportaDatos.aspx.cs" Inherits="WebLab.Seroteca.ImportaDatos" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                    

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
   <div  style="width: 900px" class="form-inline" >
           <div class="panel panel-default" runat="server" > 
                    <div class="panel-heading"> <h3 class="panel-title"><b>Etiquetas Seroteca</b> </h3>

                        </div>
                   <div class="panel-body">
     <table width="100%"
         >
     <tr>

         <td colspan="2">


            <div>
          

                Archivo con etiquetas:
               <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm" Width="500px" />
                <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="150px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />

            </div>
         
            <div>
                <strong>El archivo debe ser de tipo cvs</strong>
               <asp:Label ID="estatus" runat="server" 
                    Style="color:red"></asp:Label>
            </div>
            <hr />
                </td></tr>
                <tr>
              

                <td align="left">  
                  <div class="myLabelIzquierda" > Seleccionar: <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton> <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
            
                   

                    </div> 
        </td>
                <td align="right">   
                     <label>Impresora:</label> <asp:DropDownList ID="ddlImpresora" runat="server" class="form-control input-sm">
                      </asp:DropDownList><asp:Button ID="btnImprimir" runat="server" onclick="btnGuardar_Click"   CssClass="btn btn-primary" Width="150px"
                Text="Imprimir Etiquetas" />
        </td>
                </tr> </table>
                        </div>
                    <div class="panel-footer">
                		<div style="border: 1px solid #999999; height: 450px; width:850px; overflow: scroll; background-color: #EFEFEF;"> 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idMindrayResultado" CssClass="mytable2" Font-Names="Arial" Font-Size="8pt"
                EmptyDataText="No se encontraron resultados para incorporar">
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="numero"   HeaderText="Numero" >
                <ItemStyle Width="5%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
                <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="apellido" HeaderText="Apellidos y Nombres">
                <ItemStyle Width="30%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha Transfusion" >
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="transfusion" HeaderText="Transfusion">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                
                 
                
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>
         </div>
                        </div>
      


    

   

        </div>

        
    </div>
</asp:Content>