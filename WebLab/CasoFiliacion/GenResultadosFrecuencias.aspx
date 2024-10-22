<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenResultadosFrecuencias.aspx.cs" Inherits="WebLab.CasoFiliacion.GenResultadosFrecuencias" MasterPageFile="~/Site1.Master" %>

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
 
   
  
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
   
   <div class="panel panel-default" style="width:100%">
                    <div class="panel-heading">
                        <h3 class="panel-title">Frecuencias Alélicas - Marcadores Autosómicos - Laboratorio Central</h3>
                        <br />
                       <h4> Proceso Nro.  <asp:Label ID="lblProceso" runat="server" Text="Label"></asp:Label></h4>
                        <br />
                        <asp:DropDownList ID="ddlMarcador" Width="150px"
                                ToolTip="Seleccione el marcador"   class="form-control input-sm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMarcador_SelectedIndexChanged"></asp:DropDownList>
                        </div>
       	<div class="panel-body">	
     <asp:GridView ID="gvLista" runat="server" 
                
                EmptyDataText="No se encontraron resultados generados" Width="100%">
             

         </asp:GridView>
         
               
                		<div  > 
         
         
     
            
           
         </div>
        
                
     
            
           
            
     
            
           
              </div>
             
         
     
 
       <div class="panel-footer">		
         <div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>
              <asp:Button ID="btnDescargar" runat="server"    CssClass="btn btn-danger" Width="200px"
                Text="Descargar"   />



               </div>
    </div>
      
 
</asp:Content>


