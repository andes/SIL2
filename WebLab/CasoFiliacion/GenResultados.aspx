<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenResultados.aspx.cs" Inherits="WebLab.CasoFiliacion.GenResultados" MasterPageFile="~/Site1.Master" %>

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
    <br /> <input runat="server"  type="hidden" id="id"/>
                             <input runat="server"  type="hidden" id="Desde"/>
    <br /> <br /> 
   
   <div class="panel panel-default" style="width:80%">
                    <div class="panel-heading">
                        <h3 class="panel-title">Base de Datos Frecuencias Alélicas. Generar Resultados de Análisis</h3>
                        </div>
       	<div class="panel-body">	
     
          <asp:Button ID="btnGenerarProceso" runat="server"    CssClass="btn btn-danger" Width="200px"
                Text="Correr Proceso" OnClick="btnGenerarProceso_Click" />
     
               <br />
                 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idProceso"  
                EmptyDataText="No se encontraron resultados generados" OnRowCommand="gvLista_RowCommand" OnRowDataBound="gvLista_RowDataBound">
            <Columns>
                <asp:BoundField DataField="fecha"   HeaderText="Fecha Resultados" >
                
                </asp:BoundField>
             
          
                <asp:BoundField DataField="cantidad" HeaderText="Cantidad Muestras Procesadas">
               
                </asp:BoundField>
             
               <asp:TemplateField HeaderText="Resultado">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Resultados" runat="server" Text="" Width="20px"  >
                                             Visualizar</asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>
         
                        
            </Columns>
             

         </asp:GridView>
           
            
           
         </div>
        
           
     
            
           
        
     
            
           
            
             
         
     
 
       <div class="panel-footer">		
         

            
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            


               </div>
    </div>
      
 
</asp:Content>


