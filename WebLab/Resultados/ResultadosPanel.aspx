<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadosPanel.aspx.cs" Inherits="WebLab.Resultados.ResultadosPanel" MasterPageFile="~/Resultados/SitePE.Master" %>
<%@ Register Src="~/Services/ObrasSociales.ascx" TagName="OSociales" TagPrefix="uc1" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

     <link rel="shortcut icon" href="App_Themes/default/images/favicon.ico"/>

 
      <link rel="stylesheet"  href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
<script src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script src='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
     <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>

    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      
   
      
  
    </asp:Content>


<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    
    
     <div  >
         <h4>Bienvenido al Panel de Resultados del Sistema Centralizado de Laboratorios de la Provincia de Neuquen</h4>
       <p> Recuerde que la Ley 2611, en su artículo 4°, inciso K, señala: “Los
pacientes tienen derecho a la intimidad y confidencialidad de los datos. En
correspondencia con este derecho el servidor público debe indefectiblemente
guardar y preservar el secreto profesional”</p>
        
                  <div class="panel panel-danger" id="pnlAntigeno" runat="server" >


           <div class="panel-heading"><h4>Antigenos Covid-19</h4>
               </div>
                     <div class="panel-body">
                               <asp:LinkButton PostBackUrl="../Protocolos/DefaultEfector.aspx?idServicio=3&idUrgencia=0" runat="server" id="ingresoTest">Ingreso de Test Realizado</asp:LinkButton>
                       
                         
                               <asp:LinkButton PostBackUrl="../Protocolos/ProtocoloList.aspx?idServicio=3&tipo=Lista" Visible="false" runat="server" id="listaTest">Listado de test Realizado</asp:LinkButton>
                         

               </div>
                      </div>                           

               
            <div class="panel panel-default">


           <div class="panel-heading">


                         
                             <asp:LinkButton PostBackUrl="../Informes/historiaClinicafiltro.aspx?Tipo=PacienteValidado&Desde=Consulta" runat="server" id="Consulta">Consulta de Resultados</asp:LinkButton>
                             

                     <br />

                
               


                                              

                        
                             
                             <asp:LinkButton PostBackUrl="../CasoFiliacion/CasoListResultado.aspx" runat="server" id="Histo">Informes de Médula Osea</asp:LinkButton>
               <br />
                <asp:LinkButton PostBackUrl="ResultadosPanel.aspx?Consulta=1" runat="server" id="LinkButton1">Ver mis últimas consultas</asp:LinkButton>
                 <br />
               <asp:GridView ID="GridView1" runat="server" Visible="False" CssClass="table table-bordered bs-table" ></asp:GridView>  
               
                 
   </div>

                     
                            
                       

                     </div>
          

                        
         </div>

    
                   
        
    </asp:Content>