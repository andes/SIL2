<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportarMarcadores.aspx.cs" Inherits="WebLab.CasoFiliacion.ImportarMarcadores" MasterPageFile="~/Site1.Master" %>

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
    <asp:HiddenField runat="server" ID="HFCurrTabIndex" Value="0"   /> <input runat="server"  type="hidden" id="id"/>
                             <input runat="server"  type="hidden" id="Desde"/>
   
 
   <div class="panel panel-default" style="width:80%">
                    <div class="panel-heading">
                        <h3 class="panel-title">Base de Datos Frecuencias Alélicas. Importar Marcadores</h3>
                        </div>
       	<div class="panel-body">	

                  		

	


   
             
                  <asp:Panel runat="server" ID="pnlMarcadoresFiliacion" Visible="true">
                  <h3>Importar tablas</h3>
               <div> Archivo Genotipos:   <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm"  />  <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="200px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />

               

               </div>
                <p>El archivo debe ser de tipo cvs o txt</p>
             
                            <div>

                            <asp:GridView ID="gvTablaForense" runat="server" Font-Names="Verdana" Font-Size="12pt" EmptyDataText="No se encontraron datos incorporables"></asp:GridView>
                             

                             </div>
                              <hr />
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar a Base de Datos" 
                                                onclick="btnAgregar_Click" CssClass="btn btn-success" Width="200px" />

                       

                      <br />
                      <asp:GridView ID="gvBaseGen1" runat="server" EmptyDataText="No se encontraron datos en la base">
                      </asp:GridView>

                       

</asp:Panel>
            
               </div>
         
     
   
       <div class="panel-footer">		
           
           <asp:Label ID="estatus1" runat="server" 
                    Style="color:red"></asp:Label>
           

               <asp:Label ID="estatus" runat="server" Style="color: #0000FF"></asp:Label>


         <asp:GridView ID="gvBaseGen" runat="server"  
                EmptyDataText="No se encontraron datos en la base">
             

         </asp:GridView>
         


         
                                          
                                        
                     



               </div>
    </div>
       
    <script type="text/javascript">

        function readFile(input) {

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var filePreview = document.createElement('img');
                    filePreview.id = 'file-preview';
                    //e.target.result contents the base64 data from the image uploaded
                    filePreview.src = e.target.result;
                    console.log(e.target.result);

                    var previewZone = document.getElementById('file-preview-zone');
                    previewZone.appendChild(filePreview);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }

        var fileUpload = document.getElementById('trepadorFoto');
        fileUpload.onchange = function (e) {
            readFile(e.srcElement);
        }


      </script>
 
</asp:Content>


