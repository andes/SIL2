<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoItemEditExterno.aspx.cs" Inherits="WebLab.Resultados.ResultadoItemEditExterno" MasterPageFile="~/PeticionElectronica/SitePE.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

      
  

                   <script type="text/javascript">

                       function Enter(field, event) {
                           var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                           if (keyCode == 13) {
                               var i;
                               for (i = 0; i < field.form.elements.length; i++)
                                   if (field == field.form.elements[i])
                                       break;
                               i = (i + 1) % field.form.elements.length;
                               field.form.elements[i].focus();
                               return false;
                           }
                           else
                               return true;

                       }


function enterToTab(pEvent) {
        if (window.event.keyCode == 13  )
        {     
          
            window.event.keyCode = 9; 
            
        }
    }


  </script>  
  
   


   
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
     <div align="left" style="width: 1400px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
   
                   <div class="row">




                <div class="col-sm-6"> <h3  >   <asp:Label ID="lblTitulo"   runat="server" Text="CARGA DE RESULTADOS" ></asp:Label> </h3> 
                        	<asp:ImageButton ImageUrl="~/App_Themes/default/images/actualizar.gif"  ID="btnActualizar"  runat="server"  ToolTip="Ctrl+F4"   onclick="btnActualizarPracticas_Click"
                        ></asp:ImageButton> 
                         <h3>   <asp:Label ID="lblItem" runat="server"  ></asp:Label>
                           
                        </h3>
                    
                        <INPUT TYPE="button" accesskey="m" title="Alt+Shift+M"  runat="server" NAME="marcar" id="lnkMarcar" VALUE="Marcar todos" onClick="seleccionar_todo()" class="btn btn-primary" style="width:140px;">
                  <INPUT TYPE="button" accesskey="z" title="Alt+Shift+Z" runat="server" NAME="desmarcar" id="lnkDesmarcar"  VALUE="Desmarcar todos" onClick="desmarcar_todo()" class="btn btn-primary" style="width:140px;">
                    </div>    
                        <div class="col-sm-6">
                            <h3>
                                <asp:Label ID="lblProcesado" runat="server" Text="Procesado en este paso " Visible="false"></asp:Label></h3>
                             <asp:GridView ID="gvResumen" runat="server" Visible="False"  CssClass="table table-bordered bs-table" OnRowDataBound="gvResumen_RowDataBound" ShowFooter="True" >
                                 <FooterStyle Font-Bold="True" />
                             </asp:GridView>
                             <h4>  <asp:Label ID="lblMensajeSISA" ForeColor="Red" runat="server" Text="  " Visible="false"></asp:Label></h4>
                          
                       
                         
					 </div>
                             <asp:CustomValidator ID="cvValidaControles" runat="server" 
                                ErrorMessage="CustomValidator" Font-Size="9pt" 
                                onservervalidate="cvValidaControles_ServerValidate" 
                                ValidationGroup="0"></asp:CustomValidator>          &nbsp; &nbsp;&nbsp; &nbsp;                  
                                <asp:Button ID="btnAceptarValorFueraLimite" Width="200px" CssClass="btn btn-danger"  onclick="btnAceptarValorFueraLimite_Click" runat="server" Text="Aceptar valor fuera de límite" Visible="false" />  
                            <asp:Label ID="lblIdValorFueraLimite" Visible="false" runat="server" Text="0"></asp:Label>
                              <asp:Label ID="lblIdValorFueraLimite1" Visible="false" runat="server" Text="0"></asp:Label>
                                   </div>
                       </div> 
                               		 
                      
          				<div class="panel-body">	

                           <asp:Panel onkeydown="enterToTab(event)"  ID="Panel1" BackColor="#F2F2F2" runat="server" ScrollBars="Vertical" Width="100%" Height="600px" BorderStyle="Solid" BorderWidth="1" BorderColor="#CCCCCC">                                                                                                                
                                               <asp:Table ID="tContenido" 
                                            
                   
                                                   Runat="server" CellPadding="0" CellSpacing="0" CssClass="table table-bordered bs-table" 
                                                   Width="100%" ></asp:Table></asp:Panel>
                        </div>
                               
          <div class="panel-footer">	           
						                        
                                                  
                                                
    <asp:HyperLink accesskey="r" title="Alt+Shift+R" ID="hypRegresar" runat="server" CssClass="myLink">Regresar</asp:HyperLink>    
                                         
                                 
           <asp:Button ID="btnValidarPendiente"   onclick="btnValidarPendiente_Click" Visible="false"  AccessKey="P" runat="server" CssClass="btn btn-success" Text="Validar pendiente"  ToolTip="Alt+Shift+P:Validar solo lo pendiente"
                                 ValidationGroup="0" Width="140px" TabIndex="600" />  &nbsp;&nbsp;&nbsp;
    
                            <asp:Button accesskey="s" title="Alt+Shift+S" ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Guardar" 
                                onclick="btnGuardar_Click" ValidationGroup="0" Width="140px"/> &nbsp;&nbsp;&nbsp;

             <%--    <asp:Button AccessKey="D" ID="btnDesValidar" Visible="true" runat="server" class="btn btn-warning" Text="Desvalidar"   OnClientClick="return PreguntoConfirmar();"
                                onclick="btnDesValidar_Click" ValidationGroup="0" TabIndex="600"  Width="100px" ToolTip="Alt+Shift+D:Desvalida lo validado por el usuario actual"/>
                    --%>
                                         </div>
          </div>


                           
                        </div>
                        <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
					   <script type="text/javascript">

                       

					       function seleccionar_todo() {//Funcion que permite seleccionar todos los checkbox 

					           form = document.forms[0];

					           for (i = 0; i < form.elements.length; i++) {
					               if (form.elements[i].type == "checkbox") {					            
					                       form.elements[i].checked = 1;
					               }
					           }
					       }

					       function desmarcar_todo() {//Funcion que permite desmarcar todos los checkbox

					           form = document.forms[0];

					           for (i = 0; i < form.elements.length; i++) {
					               if (form.elements[i].type == "checkbox") {					                   
					                       form.elements[i].checked = 0;
					               }
					           }
					       }
					       function AntecedenteAnalisisView(idAnalisis, idPaciente, ancho, alto) {
					           var dom = document.domain;
					           var domArray = dom.split('.');
					           for (var i = domArray.length - 1; i >= 0; i--) {
					               try {
					                   var dom = '';
					                   for (var j = domArray.length - 1; j >= i; j--) {
					                       dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
					                   }
					                   document.domain = dom;
					                   break;
					               } catch (E) {
					               }
					           }


					           var $this = $(this);

					           $('<iframe src="AntecedentesAnalisisView.aspx?idAnalisis=' + idAnalisis + '&idPaciente=' + idPaciente + '" />').dialog({
					               title: 'Evolucion',
					               autoOpen: true,
					               width: ancho,
					               height: alto,
					               modal: true,
					               resizable: false,
					               autoResize: true,
					               overlay: {
					                   opacity: 0.5,
					                   background: "black"
					               }
					           }).width(800);
					       }


                           function ObservacionEdit(idDetalle,idTipoServicio,operacion) {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }

        //var operacion='Valida';
        var $this = $(this);         
                               $('<iframe src="ObservacionesEditExt.aspx?idDetalleProtocolo=' + idDetalle + '&idTipoServicio=' + idTipoServicio + '&Operacion=' + operacion + '" />').dialog({
            title: 'Observaciones',
            autoOpen: true,
            width: 500,
            height: 440,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(510);
    }

     function PredefinidoSelect(idDetalle, operacion) {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }

      
        var $this = $(this);         
        $('<iframe src="PredefinidoSelect.aspx?idDetalleProtocolo=' + idDetalle +'&Operacion='+operacion+'" />').dialog({        
            title: 'Resultados',
            autoOpen: true,
            width: 500,
            height: 440,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(510);
    }


 
 function editDiagnostico(idProtocolo) {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }


        var $this = $(this);
        $('<iframe src="DiagnosticoEdit.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Diagnostico del Paciente',
            autoOpen: true,
            width: 750,
            height: 475,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(765);
    }
 
					       function PreguntoConfirmar() {
					           if (confirm('¿Está seguro de desvalidar lo seleccionado?'))
					               return true;
					           else
					               return false;
					       }
</script>				
 
</asp:Content>