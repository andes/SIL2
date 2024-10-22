<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenMarcadoresEdit.aspx.cs" Inherits="WebLab.CasoFiliacion.GenMarcadoresEdit" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
  <script type="text/javascript" src="../script/ValidaFecha.js"></script>            
 
   <script type="text/javascript" >
    function onMouseOver(rowIndex) {
        var gv = document.getElementById("gvLista");
        var rowElement = gv.rows[rowIndex];
        rowElement.style.backgroundColor = "#c8e4b6";
        rowElement.cells[1].style.backgroundColor = "green";
    }

    function onMouseOut(rowIndex) {
        var gv = document.getElementById("gvLista");
        var rowElement = gv.rows[rowIndex];
        rowElement.style.backgroundColor = "#fff";
        rowElement.cells[1].style.backgroundColor = "#fff";
    }
</script>
  
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
     <asp:HiddenField runat="server" ID="HFCurrTabIndex" Value="0"   /> <input runat="server"  type="hidden" id="id"/>
                             <input runat="server"  type="hidden" id="Desde"/>
   
   
   <div class="panel panel-default" style="width:80%">
                    <div class="panel-heading">
                        <h3 class="panel-title">Base de Datos Frecuencias Alélicas. Importar Desde Resultados</h3>
                        </div>
       	<div class="panel-body">	
     
         
               
                  <div class="myLabelIzquierda" > Seleccionar: <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton>
                      <asp:CheckBox ID="chkExcluidos" runat="server" AutoPostBack="True" OnCheckedChanged="chkExcluidos_CheckedChanged" Text="Ver excluidos" />
                    <br />
                    </div> 
        
                		<div  > 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idProtocolo"  
                EmptyDataText="No se encontraron resultados para incorporar" OnRowDataBound="gvLista_RowDataBound">
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
             
          
                <asp:BoundField DataField="pacientep" HeaderText="Apellidos y Nombres">
                <ItemStyle Width="30%" />
                </asp:BoundField>
             
                <asp:BoundField DataField="edad" HeaderText="Edad">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="sexo" HeaderText="Sexo">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
              
                   <%-- <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                                               
                                                                            
                                                                           

                                                                             <asp:LinkButton ID="Adjuntar" runat="server" Text="" Width="20px" >
                                             <span class="glyphicon glyphicon-paperclip"></span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
                        --%>
         
                        
                <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" />
                <asp:BoundField DataField="Caso" HeaderText="Caso" />
         
                        
                <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
         
                        
            </Columns>
             

             <SelectedRowStyle BackColor="#CC3300" />
             

         </asp:GridView>
         </div>
        
                <hr />
            <asp:Button ID="btnAgregar" runat="server"    CssClass="btn btn-success" Width="200px"
                Text="Agregar a Base de Datos" OnClick="btnAgregar_Click" />
     
            
           
            <asp:Button ID="btnExcluir" runat="server"    CssClass="btn btn-danger" Width="200px"
                Text="Excluir Marcadores" OnClick="btnExcluir_Click" />
     
            
           
            <div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>
     
            
           
              </div>
             
         
     
 
       <div class="panel-footer">		
         




         <asp:GridView ID="gvBaseGen" runat="server"  
                EmptyDataText="No se encontraron datos en la base">
             

         </asp:GridView>
         




               </div>
    </div>
      
 
</asp:Content>


