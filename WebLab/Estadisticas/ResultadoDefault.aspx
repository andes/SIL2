<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoDefault.aspx.cs" Inherits="WebLab.Estadisticas.ResultadoDefault" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
 
       
     
 
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>REPORTES ESTADISTICOS POR RESULTADO</h4>
                        </div>

				<div class="panel-body">	
     <div class="row">
        <div class="col-lg-4">

                     

                            <asp:ImageButton ID="imgResultadoPredefinido" runat="server" 
                                ImageUrl="~/App_Themes/default/principal/images/estadistica3.jpg"
                                onclick="imgResultadoPredefinido_Click" />
                                        
                               
				<strong>	<h4 >Estadisticas por Resultado Predefinido</h4></strong>
						

                            
                               <p>Muestra para una practica nomenclada la cantidad de casos obtenidos para cada resultado predefinido.
                           Por ejemplo, la cantidad de casos por resultado de Chagas, HIV, ToxoPlasmosis, etc.&nbsp;</p>
                               <a href="ReportePorResultado.aspx" class="btn btn-primary" style="width:100px;" >Ingresar</a>

                                             
                          </div>  
                    
                         <div class="col-lg-4">
                            <asp:ImageButton ID="imgResultadoNumerico" runat="server" 
                               ImageUrl="~/App_Themes/default/principal/images/estadistica3.jpg"
                                onclick="imgResultadoNumerico_Click" />                                        
                               
							<strong>	<h4 >	Estadisticas por Resultado Numericos</h4></strong>
							 
                        <p>    Muestra para una practica nomenclada de tipo Numerica; la cantidad de casos obtenidos para un rango de valores, diferenciados por grupos etareos.&nbsp;</p>
                            <a href="ReportePorResultadoNum.aspx" class="btn btn-primary" style="width:100px;">Ingresar</a>


                       </div>
                        
                    	      <div class="col-lg-4">

                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                ImageUrl="~/App_Themes/default/principal/images/estadistica3.jpg"
                                onclick="imgResultadoPredefinido_Click" />
                                        
                               
				<strong>	<h4 >Estadisticas por Resultado de Texto</h4></strong>
						

                            
                               <p>Muestra una lista de determinaciones y su valor obtenido para el texto de busqueda ingresado. 
                          </p>
                               <a href="ReportePorResultadoTexto.aspx" class="btn btn-primary" style="width:100px;" >Ingresar</a>

                                             
                          </div>  
					  					
 </div>
    </div>
        </div>


</asp:Content>
