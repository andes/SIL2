<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportaDatos.aspx.cs" Inherits="WebLab.AutoAnalizador.ImportaDatos" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                    

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
      <div align="left" style="width: 1100px" class="form-inline"  >
   <div class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title">  Incorporar Resultados </h3>
                        <h4>  <asp:Label ID="lblEquipo" runat="server" Text="Label"></asp:Label></h4>
                        </div>

				<div class="panel-body">

     
            <p><strong>Paso 1.-</strong> Haga clic en "Examinar" para seleccionar el archivo con los resultados del equipo                 </p> 
            <p><strong>Paso 2.-</strong> Haga clic en "Procesar Archivo". Al terminar se mostraran los protocolos cuyos numeros se corresponden en el sistema.</p> 
            <p><strong>Paso 3.-</strong> Seleccione los protocolos cuyos resultados desea guardar y hacer clic en "Guardar Resultados".</p>
             

                Elegir el archivo con resultados:
               <asp:FileUpload ID="trepador" runat="server" CssClass="form-control input-sm"  Width="500px" />
                <asp:Button CssClass="btn btn-primary"   ID="subir" runat="server" Width="150px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />
            
         
            <div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>
            
                  <div class="myLabelIzquierda" > Seleccionar: <asp:LinkButton  ID="lnkMarcar" runat="server" CssClass="form-control input-sm"  onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="form-control input-sm"  onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton>
                    <br />
                    </div> 
           <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
         <br />
                    </div>
       <div class="panel-footer">
                		<div style="border: 1px solid #999999; height: 450px; width:1000px; overflow: scroll; background-color: #EFEFEF;"> 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idProtocolo" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="11pt"
                EmptyDataText="No se encontraron resultados para incorporar" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="numero"   HeaderText="Protocolo" >
                <ItemStyle Width="5%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="origen" HeaderText="Origen">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="edad" HeaderText="Edad">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="sexo" HeaderText="Sexo">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                
                <asp:BoundField DataField="estado" HeaderText="Estado Protocolo">
                <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                
                 
                
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>
        
               
              
        </div>  
            <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click"  CssClass="btn btn-primary" Width="150px"
                Text="Guardar Resultados" />

       </div>
          </div>
          </div>
        
</asp:Content>