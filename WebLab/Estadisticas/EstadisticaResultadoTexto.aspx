<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="EstadisticaResultadoTexto.aspx.cs" Inherits="WebLab.Estadisticas.EstadisticaResultadoTexto" %>
 
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>



<asp:Content ID="content3" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript"> 
      

	$(function() {
		$("#<%=txtFechaDesde.ClientID %>").datepicker({
		    maxDate: 0,
		    minDate: null,

			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

	$(function() {
	    $("#<%=txtFechaHasta.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,

			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});
 
     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
  
 

   
    </asp:Content>
 
<asp:Content ID="content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    <div align="left" style="width: 1200px" class="form-inline"  >
        
                <div   class="panel panel-success">
   <div class="panel-heading">  <h3>RESULTADOS PARA ESTADISTICAS</h3>
        </div>
			 
				<div class="panel-body">	<b> Efector: <asp:Label ID="lblEfector" runat="server" Text="Label"></asp:Label></b>
                    <br />	
					<b> Servicio: </b>
                            <asp:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" class="form-control input-sm"
                                AutoPostBack="True" 
                                onselectedindexchanged="ddlServicio_SelectedIndexChanged1">
                            </asp:DropDownList>
                                        
                                    <br />

                    	<b> Tipo de Datos: </b>
                            <asp:DropDownList ID="ddlTipoDatos" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" class="form-control input-sm"
                                 >
                                <asp:ListItem>Paciente</asp:ListItem>
                                <asp:ListItem>No Paciente</asp:ListItem>
                            </asp:DropDownList>
                                        
                                    <br />
						 
                        <b> Fecha Desde: </b>   
						 
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px"  /> 
                          
                                    <br />
                          <b> Fecha Hasta: </b> 
                    <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"  /> 
					  
        <br />
       <b> Area:  </b> <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="True" class="form-control input-sm"
                                                                    onselectedindexchanged="ddlArea_SelectedIndexChanged">
                                                                </asp:DropDownList>

                    <br />

      <b>Practica: </b>    <asp:DropDownList ID="ddlItem" runat="server" class="form-control input-sm">
                                                                </asp:DropDownList>  
                    <anthem:Button ID="btnAgregarItem" runat="server" Text="Agregar" CssClass="btn btn-default" Width="100px" OnClick="btnAgregarItem_Click" ToolTip="Agregar Determinacion" />
                     <%-- <anthem:ImageButton ID="btnAgregarItem" runat="server" ImageUrl="~/App_Themes/default/images/añadir.jpg" onclick="btnAgregarItem_Click" ToolTip="Agregar Determinacion" />--%>
        <br />
         <p><b>Practicas a incluir:</b></p>
                                                                <anthem:ListBox ID="lstItem" runat="server" class="form-control input-sm"  Height="200px" SelectionMode="Multiple" Width="350px" Font-Size="12pt">
                                                                </anthem:ListBox>



                    <%--<anthem:ImageButton ID="btnSacarItem" runat="server" ImageUrl="~/App_Themes/default/images/sacar.jpg" onclick="btnSacarItem_Click" ToolTip="Sacar Determinacion" />--%>
                      <anthem:Button ID="btnSacarItem" runat="server" Text="Excluir" CssClass="btn btn-default" Width="100px" OnClick="btnSacarItem_Click" ToolTip="Sacar Determinacion" />


        </div>

     <div class="panel-footer">	
         
                                      <asp:Button ID="btnExcel" runat="server"   Text="Exportar Excel Detalle"  CssClass="btn btn-success" Width="180px" OnClick="btnExcel_Click"/>
           	
            <asp:Label ID="lblError" runat="server" Text="Label" Visible="false"></asp:Label>     
           	
         </div>
    </div> </div>
</asp:Content>
