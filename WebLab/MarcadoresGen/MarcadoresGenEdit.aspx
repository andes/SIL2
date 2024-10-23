<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarcadoresGenEdit.aspx.cs" Inherits="WebLab.MarcadoresGen.MarcadoresGenEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %> 

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<title>LABORATORIO</title>
<link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />


     <script src="../script/jquery.min.js" type="text/javascript"></script>  
                  <script src="../script/jquery-ui.min.js" type="text/javascript"></script> 
    <link rel="stylesheet" type="text/css" href ="../script/moverfilas/moverfilas.css" />
<script type="text/javascript" src="../script/moverfilas/codigo.js"></script>
<script type="text/javascript">
    $(function() {

                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                      
                        $("#tabContainer").tabs({ selected: currTab });
             });
</script>
  

    <style type="text/css">
        .style1
        {
            font-size: 10pt;
            font-family: Calibri;
            background-color: #FFFFFF;
            color: #333333;
            font-weight: bold;
            width: 189px;
        }
        .style2
        {
            width: 501px;
        }
    </style>
  

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">       
    <br />   &nbsp;
     <asp:HiddenField runat="server" ID="HFCurrTabIndex"   /> 

  <div align="center" style="width:600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">CONFIGURACION DE TIPO DE MARCADOR</h3>
                        </div>

				<div class="panel-body">	
  
    

    
    <table style="width: 100%"  >
<tr><td>
						   Nombre de tipo marcador:</td>
						<td>
                                            <asp:TextBox ID="txtCodigo" runat="server" class="form-control input-sm" Width="264px" 
                                                MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCodigoHT" runat="server" 
                                                ControlToValidate="txtCodigo" ErrorMessage="Nombre de tipo marcador" 
                                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                                            </td>
						<td>
                            &nbsp;</td>
						
					</tr>
	 
            	<tr>
						<td   >
                                            Marcador:</td>
						<td>
                            <anthem:TextBox ID="txtNombre" runat="server"    class="form-control input-sm"           Width="196px" AutoCallBack="True" 
                                 TabIndex="6"   MaxLength="50"></anthem:TextBox>
                                        
  <input id="btnAgregar" type="button" value="Agregar a Lista"  onclick="CrearFila()"   style="width: 150px" /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            &nbsp;</td>
						<td>
                            <asp:CustomValidator ID="cvAnalisis" runat="server" 
                                ErrorMessage="Debe completar al menos un Analisis" 
                                onservervalidate="cvAnalisis_ServerValidate" ValidationGroup="0" 
                                Font-Size="8pt">Debe completar al menos un Analisis</asp:CustomValidator>
                                        <anthem:Label ID="lblMensaje" runat="server" ForeColor="#FF3300">&nbsp;&nbsp;
                            </anthem:Label>
                        </td>
						
					</tr>
					<tr>
						<td  colspan="2" >
                                            <table id="Table1"  
        style="font-size:.9em; margin-left:1%; background-color: #DFE7F7;" cellpadding="0" cellspacing="0" 
        align="center">
<tr>
<td style="width:40px;">&nbsp</td>
    
    <td style="width:500px;"  align="center">Marcador</td>
    <td width="20px" >&nbsp</td>
</tr>


</table>
<table summary="Tabla editable para sumar filas y columnas" id="tabla"  
        style="font-size:.9em; margin-left:1%; " cellpadding="0" cellspacing="0" 
        align="center">

<tbody id="cuerpo">

</tbody>
</table>


						</td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            <anthem:TextBox ID="txtNombreAnalisis" runat="server" CssClass="myTexto" 
                                BorderColor="White" ForeColor="White" 
                               ></anthem:TextBox>     
                        </td>
						<td>
                            &nbsp;</td>
						
					</tr>
        </table>
                        

 
        
        </div>
                    
        <div class="panel-footer">
 <table width="100%">
					<tr>
						<td align="left" >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="MarcadoresGenList.aspx" CausesValidation="False" 
                                                ToolTip="Regresa a la pagina anterior" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>     
                        </td>
						
						<td align="right" >
    <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Width="80px"  Text="Guardar" 
        onclick="btnGuardar_Click" ValidationGroup="0" />  
                        </td>
						
					</tr>
					<tr>
						<td align="right" colspan="2" >
                            <asp:ValidationSummary ID="vsHojaTrabajo" runat="server" 
                                HeaderText="Debe completar los siguientes datos requeridos:" 
                                ShowMessageBox="True" ShowSummary="False" ValidationGroup="0" />
                        </td>
						
					</tr>
</table>
                            </div>
      </div>


  
 <input type="hidden" runat="server" name="TxtDatos" id="TxtDatos" value="" />  

  </div>
      
  <script language="javascript" type="text/javascript">
CargarDetalle();
 // var contadorfilas = 0; 
   var tab;
            var filas;	




  function NoExiste( cod)
  {
    var devolver= true;
    var codigoNuevo= cod;
        if (codigoNuevo=='')
        {
          devolver= false; 
        }
        else
        {
             	         	    
    	    tab = document.getElementById('cuerpo');
            filas = tab.getElementsByTagName('tr');
            for (i=0; ele = filas[i]; i++)
            {  
                var codigo=ele.getElementsByTagName('input')[0].value;        
 		        if (codigo==codigoNuevo)
 		        {
 		        alert('El item ya existe en la lista');
		          devolver= false; break;
		        }
            }     	     	
         }     
            return devolver;
  }
  
  function CrearFila()
  {
       
    

      var textoNuevo = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtNombre").ClientID %>').value;


        NuevaFila( textoNuevo);
       
  }
  
  function NuevaFila( tex)
        {
   if (NoExiste(tex))
        { 
      //  alert('entra');
            Grilla = document.getElementById('cuerpo');
         
         
             
            fila = document.createElement('tr');
           // fila.id = 'cod_'+contadorfilas;
            //fila.name='cod_'+contadorfilas;
            
        
            celdaflecha1= document.createElement('td');    
            celdaflecha1.className= "orden";
                ////crea la primera flecha
                  oFlecha1= document.createElement('img');
                  oFlecha1.name= "pru";
                  oFlecha1.src="../script/moverfilas/arriba.gif";
                  oFlecha1.alt="subir fila";
                ////crea la segunda flecha
                  oFlecha2= document.createElement('img');            
                  oFlecha2.src="../script/moverfilas/abajo.gif";
                  oFlecha2.alt="bajas fila";                                  
            
            celdaflecha1.appendChild(oFlecha1);
            celdaflecha2= document.createElement('td');    
            celdaflecha2.className= "orden";
            celdaflecha2.appendChild(oFlecha2);
            fila.appendChild(celdaflecha1);
            fila.appendChild(celdaflecha2);
            
            ///////////////////////////////////        	
            celdaCodigo = document.createElement('td');   
                ///celda para el codigo
                oCodigo = document.createElement('input');
                oCodigo.type = 'text';                        
                oCodigo.runat = 'server';
                oCodigo.width = "500px";
                celdaCodigo.width = "500px";
                //oCodigo.name = 'Codigo_'+contadorfilas;
                //oCodigo.id = 'Codigo_'+contadorfilas;
                oCodigo.readOnly=true;
                //oCodigo.className = 'codigoAnalisis';
                //celdaCodigo.className = 'codigoAnalisis';
                oCodigo.value= tex;
            
            celdaCodigo.appendChild(oCodigo);
            fila.appendChild(celdaCodigo);
        			
            
            
            
                 celda6 = document.createElement('td');  
                  celda6.className= "orden";
                  celda6.width="60px";
          oBoton= document.createElement('img');
              //    oBoton.name= "pru";
                  oBoton.src="../script/moverfilas/eliminar.gif";
                  oBoton.alt="eliminar fila";
            oBoton.onclick = function () {borrarfila(this,'cuerpo')};
            celda6.appendChild(oBoton);
            fila.appendChild(celda6);
        	
            Grilla.appendChild(fila);
       //     contadorfilas = contadorfilas + 1;
          //  alert(contadorfilas);
            iniciarTabla('cuerpo');
            CargarDatos();
            }
           
        }
        
        
        
        
        
        
        
        function CargarDatos()
        {
            var str = '';            
            var tab;
            var filas;	   	     
    	    
    	    tab = document.getElementById('cuerpo');
            filas = tab.getElementsByTagName('tr');
            for (i=0; ele = filas[i]; i++)
            {  
                var codigo = ele.getElementsByTagName('input')[0].value;
              


                if (codigo != '')
                    str = str + codigo + '#' + codigo + '@';
            }     	     	     
	         document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;	   	        
        }
        
        function CargarDetalle()
{ 
	var datos= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value ;	   	        
	
    if (datos!="")
    {      
            var sTab = datos.split('@');
                                    
	        for (var i=0; i<(sTab.length-1); i++)
	        {//alert(sTab[i]);
	            var sFi = sTab[i].split('#');
	            //alert(sFi[0]);
	            if  (sFi[0]!="")
	            {
	            
	         
	            NuevaFila(sFi[0] );	            
		       
		      
		        }
	        }
	        }

}
       
</script> 
 
 </asp:Content>


