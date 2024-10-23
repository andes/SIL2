<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Capacita.aspx.cs" Inherits="WebLab.Capacita.Capacita" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div   class="panel panel-default">
   <div class="panel-heading"> 
     Capacitacion SIL
        </div>
         	<div class="panel-body" > 
                    <div   class="panel panel-default" style="width:20%; height:350px">
                        <div class="panel-heading"> 
                          <h3>Ingreso de Protocolo</h3>
                       </div>
         	           <div class="panel-body" > 
                
   
                                <video   src="INGRESOMUESTRA.mp4" type="video/mp4" width="98%" height="200px" controls   poster="..\App_Themes\default\images\logo.png">
                            Your browser does not support the HTML5 Video element.
                        </video>
                    </div> 
                         </div>
                            
       <div   class="panel panel-default" style="width:20%; height:350px">
                        <div class="panel-heading"> 
                           <h3>Conexión MetroLab CM200 y CM250</h3>
                       </div>
         	           <div class="panel-body" > 
                       
                
        <video   src="conexionMetrolabQuimica.mp4" type="video/mp4" width="98%" height="200px"  controls   poster="..\App_Themes\default\images\logo.png">
    Your browser does not support the HTML5 Video element.
</video>
  </div> 
                         </div>
        </div>
                       </div>
       </div>
           
           
       
</asp:Content>
