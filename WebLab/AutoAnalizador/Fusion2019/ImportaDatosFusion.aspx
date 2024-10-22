<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportaDatosFusion.aspx.cs" Inherits="WebLab.AutoAnalizador.Fusion2019.ImportaDatosFusion" MasterPageFile="~/Site1.Master" %>

<%@ Register src="tablaEspecificidad.ascx" tagname="tablaEspecificidad" tagprefix="uc1" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                    

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
   <div  style="width: 700px" class="form-inline" >
           <div class="panel panel-default" runat="server" > 
                    <div class="panel-heading"> <h3 class="panel-title">Importación Fusion-Luminex</h3>

                        </div>
                   <div class="panel-body">
     <table width="100%"
         >
     <tr>

         <td colspan="2">


            <div>
          
                <br />
                Archivo:
               <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm" Width="400px" />
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
                  
               

     <asp:Button ID="btnImprimir" runat="server" onclick="btnGuardar_Click"   CssClass="btn btn-danger" Width="150px"
                Text="Guardar Resultados" />

   

        </td>
                </tr> </table>
                        </div>
                    <div class="panel-footer">
                		<div style="border: 1px solid #999999; height: 450px; width:650px; overflow: scroll; background-color: #EFEFEF;"> 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  Visible="false"
                DataKeyNames="idMindrayResultado" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="10pt"
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
                <ItemStyle Width="20%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField DataField="item" HeaderText="Determinacion" >
                <ItemStyle Width="20%" />
                </asp:BoundField>
                
                
                  <asp:TemplateField HeaderText="Resultado" >
                    <ItemTemplate>
                    <asp:TextBox ID="txtResultado"  Text='<%# Bind("apellido") %>' Width="350px" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                  
                </asp:TemplateField>
                
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>

                            <uc1:tablaEspecificidad ID="tablaEspecificidad1" runat="server" />

                            <asp:GridView ID="gvListaEspecificidad" runat="server" AutoGenerateColumns="False"  Visible="false"
                DataKeyNames="idMindrayResultado" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="10pt"
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
                <ItemStyle Width="20%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField DataField="apellido" HeaderText="" >
                <ItemStyle Width="20%" />
                </asp:BoundField>
                
                  <asp:BoundField DataField="dni" HeaderText="" >
                <ItemStyle Width="20%" />
                </asp:BoundField>
                  <asp:BoundField DataField="transfusion" HeaderText="" >
                <ItemStyle Width="20%" />
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