<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="WebLab.Estadisticas.Principal" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <table style="width: 50%;" align="center">
        <tr>
            <td>
                  <div class="services_block">
					                  <img src="../App_Themes/default/principal/images/estadistica3.jpg" alt="" width="48" height="48" border="0" class="icon_left" title="" />
					                  <div class="services_details">
                     <h3>INFORMES ESTADISTICOS</h3>
                     <br />
                     <div align="left">
                         <ul>
                             <li class="myLink"><a href="GeneralFiltro.aspx">Estadistica General</a></li>
                             <li class="myLink"><a href="PorResultado.aspx">Estadisticas de Resultados</a></li>
                             <%--<li class="myLink"><a href="#">Conteo por Efector Solicitante</a></li>
                             <li class="myLink"><a href="#">Conteo por Analisis discriminado por Resultados</a></li>
                             <li class="myLink"><a href="#">Protocolos por Médico Solicitante</a></li>--%>
                         </ul>
                    </div>
                  </div>
</div>
            </td>
           
        </tr>
        </table>
    <link rel="stylesheet" type="text/css" href="../App_Themes/default/principal/style.css" />
 <%--  <form id="form1" runat="server">--%>
                                   
                    <h2>&nbsp;</h2>
                    
    </div>
  <%--  </form> 
   --%>
    
    </asp:Content>


