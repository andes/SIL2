<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseGenMenu.aspx.cs" Inherits="WebLab.CasoFiliacion.BaseGenMenu" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
    

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />



</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
<div    class="form-inline"  >
        <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Base de Datos Frecuencias Alélicas</h3>
                        </div>
       	<div class="panel-body">	
               
 <asp:Button ID="btnCargarTablas" runat="server" Text="Cargar Nuevas Tablas" ValidationGroup="0" 
                                              CssClass="btn btn-success" Width="200px"  TabIndex="1" OnClick="btnCargarTablas_Click" />
                       
                                    &nbsp;
                              
                               <asp:Button ID="btnImportar" runat="server" Text="Importar desde Resultados" ValidationGroup="0" 
                                              CssClass="btn btn-success" Width="200px"   TabIndex="0" OnClick="btnImportar_Click" />
           
                  &nbsp;
                               <asp:Button ID="btnResultados" runat="server" Text="Resultados" ValidationGroup="0" 
                                              CssClass="btn btn-danger" Width="200px"   TabIndex="0" OnClick="btnResultados_Click" />
           

               
          
          
           
                 
  
               </div>

               <div class="panel-footer">		
                        <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
                   <asp:GridView ID="gvBaseGen" runat="server"  
                EmptyDataText="No se encontraron datos en la base" AutoGenerateColumns="False" DataKeyNames="idProtocolo" OnRowCommand="gvBaseGen_RowCommand" OnRowDataBound="gvBaseGen_RowDataBound">
                       <Columns>
                           <asp:BoundField HeaderText="Numero" DataField="numero" />
                           <asp:BoundField HeaderText="Persona" DataField="paciente" />
                           <asp:BoundField HeaderText="Edad" DataField="edad" />
                             <asp:BoundField DataField="sexo" HeaderText="Sexo" />
                             <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                       
                                                                              <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                             <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
                       </Columns>
             

         </asp:GridView> 
            </div>
         
          
       </div>
  </div>
           
    
 </asp:Content>


