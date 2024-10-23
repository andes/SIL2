<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistorialPorUsuario.aspx.cs" Inherits="WebLab.Informes.HistorialPorUsuario" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript">


          $(function () {
              $("#<%=txtFechaDesde.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });

          $(function () {
              $("#<%=txtFechaHasta.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });


          $(function () {
              $("#<%=txtFechaNac.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    <br />
<br />
<div align="left">
      <table  width="1100px"  
                     style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: normal; color: #000000" 
                     cellpadding="1" cellspacing="1" >
<tr><td>


				 <table  width="1000px" align="center" cellpadding="1" cellspacing="1" class="myTabla"  style="width: 750px" >
					<tr>
						<td align="right" >
                            Historial de consultas realizadas</td>
						<td >&nbsp;</td>
					</tr>
					<tr>
						<td ><hr /></td>
						<td >&nbsp;</td>
					</tr>
					<tr>
						<td style="vertical-align: top">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="idPaciente" Font-Size="9pt" 
                                                Width="100%" CellPadding="2" 
                                ForeColor="Black" PageSize="20" 
                                
                                EmptyDataText="No se encontraron pacientes con resultados validados para los parametros de busqueda ingresados" 
                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="3px" 
                                GridLines="Horizontal" Font-Bold="False">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" 
                Font-Size="8pt" />
            <Columns>
                <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="60%" />
                </asp:BoundField>
                 <asp:BoundField DataField="protocolo" HeaderText="Protocolo">
                     <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:BoundField HeaderText="Fecha y hora de accceso" />
                 <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/zoom.png" 
                            CommandName="Editar" ToolTip="Visualizar laboratorios" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle ForeColor="#CC3300" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#3A93D2" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                Font-Names="Arial" Font-Size="8pt" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                   
                        <br />
                            &nbsp;select distinct top 20 Pac.numeroDocumento as dni, Pac.apellido + &#39; &#39; + 
                            Pac.nombre as paciente , P.numero, A.fecha from LAB_AuditoriaProtocolo as A 
                            inner join lab_protocolo as P on P.idprotocolo= A.idProtocolo inner join 
                            Sys_Paciente as Pac on Pac.idPaciente=P.idpaciente where accion =&#39;Consulta&#39; and 
                            A.idUsuario=1080 order by fecha desc</td>
						
						<td style="vertical-align: top">
                            &nbsp;</td>
						
					</tr>
					</table>
						




</td></tr>
   
 </table>
   
 </div>
 </asp:Content>