<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeleccionSecuenciacion.aspx.cs"
     Inherits="WebLab.SeleccionSecuenciacion" MasterPageFile="~/Site1.Master" %>
  <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     
      <script type="text/javascript" src='<%= ResolveUrl("script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("script/jquery-ui.css") %>'  />

      
   	 
     <script type="text/javascript" src="script/ValidaFecha.js"></script>        
       

     <script type="text/javascript">                    
                                  
          
            
         $(function() {
             $("#<%=txtFechaDesde.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: 'App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
         });

              
         $(function() {
             $("#<%=txtFechaHasta.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: 'App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
         });

         
$(function () {
    $("#tabContainer").tabs();
    var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
    $("#tabContainer").tabs({ selected: currTab });
});
          </script>

   </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
    <asp:HiddenField runat="server" ID="HFCurrTabIndex"  /> 
     
       <div align="left" style="width: 1450px" class="form-inline"  >
  <div   class="panel panel-success">
   <div class="panel-heading">  <h3>SELECCION SECUENCIACION (COVID-19)</h3>
        </div>
         	<div class="panel-body" >  
            <asp:Label ID="lblError" runat="server" Text="Label" Visible="false"></asp:Label>     
                   <div class="form-group" >
                   <strong>    Fecha Desde:  </strong>
                     <input id="txtFechaDesde" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

                  <strong>  Fecha Hasta: </strong>  <input id="txtFechaHasta" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

</div> 
                 <br />

                  
                        <div class="form-group" > 
                        <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"  CssClass="btn btn-success" Width="100px"/>
                             <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Exportar Excel Detalle"  CssClass="btn btn-success" Width="180px"/>
           </div>
                            <br />
                                        

           
               
           
                  
                 
    
       </div>
      	<div class="panel-footer" > 

                 <h4  >  <asp:Label ID="lblCantidad" runat="server" Text=""></asp:Label></h4>
        <asp:GridView ID="gvResumen"  CssClass="table table-bordered bs-table"  runat="server" Width="100%" Font-Size="8pt"   Font-Names="Verdana"  >
                  </asp:GridView>
                    
         

       
     </div>
           </div>
           </div>
    </asp:Content>