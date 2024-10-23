<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FiliacionMenu.aspx.cs" Inherits="WebLab.CasoFiliacion.FiliacionMenu" MasterPageFile="~/Site1.Master" %>

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
     
    <table align="center"  >
        <tr>
            <td>
<div align="center"  class="form-inline"  >
        <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Gestión de Casos de Genética Forense</h3>
                        </div>
       	<div class="panel-body">	
               <table>
                   <tr>
                       
                       <td   valign="top">
                           <asp:Label ID="lblMensaje" runat="server" Text="Label" Visible="false" ForeColor="Red"></asp:Label>
                           <br />
                                  <asp:Button ID="btnNuevoCaso" runat="server" Text="Nuevo Caso" ValidationGroup="0" 
                                              CssClass="btn btn-danger" Width="150px" Height="49px"  TabIndex="0" OnClick="btnNuevoCaso_Click" />
           

                 <br />  <br />
         <asp:Button ID="btnNuevoCaso0" runat="server" Text="Ver Casos" ValidationGroup="0" 
                                              CssClass="btn btn-danger" Width="150px" Height="49px"  TabIndex="1" OnClick="btnNuevoCaso0_Click" />
               <br />
                       </td>
                       <td   valign="top">
                                 &nbsp;   &nbsp;   &nbsp;   &nbsp;
                       </td>
                       <td   valign="top">
                              
                    <%--     <div class="row" style="width:250px;">        
                           	 <div class="thumbnail" >
                                    <h4>Procedimiento Ingreso Filiación</h4>
<a href="proceso caso filiacion.gif" target="_blank">
<asp:Image ID="Image1" runat="server" ImageUrl="~/CasoFiliacion/proceso caso filiacion.gif" /> </a>
                             
                             </div>
                                  </div> --%>


                       </td>



                   </tr>
                   
        
                   
                   </table>
          
               <br />
                 
  
               </div>


            <div class="panel-footer">
               

            </div>
       </div>
  </div>
            </td>
        </tr>
    </table>
    
 </asp:Content>


