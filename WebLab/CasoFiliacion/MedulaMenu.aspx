<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MedulaMenu.aspx.cs" Inherits="WebLab.CasoFiliacion.MedulaMenu" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO CENTRAL</title>    
    

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
    <br /> <br /> <br /> 
    <table align="center" style="width: 1000px;">
        <tr>
            <td>
<div align="center" style="width:520px" class="form-inline"  >
        <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Gestión de Casos de Histocompatibilidad</h3>
                        </div>
       	<div class="panel-body">	
               <table>
                   <tr>
                       <td></td>
                       <td   valign="top">
                                  <asp:Button ID="btnNuevoCaso" runat="server" Text="Nuevo Caso" ValidationGroup="0" 
                                              CssClass="btn btn-success" Width="150px" Height="49px"  TabIndex="0" OnClick="btnNuevoCaso_Click" />
           

                 <br />  <br />
         <asp:Button ID="btnNuevoCaso0" runat="server" Text="Ver Casos" ValidationGroup="0" 
                                              CssClass="btn btn-danger" Width="150px" Height="49px"  TabIndex="1" OnClick="btnNuevoCaso0_Click" />
               <br />
                       </td>
                   </tr>
                   
               </table>
          
               <br />
                 
  
               </div>
       </div>
  </div>
            </td>
        </tr>
    </table>
    
 </asp:Content>


