<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="seguimientoCovid.ascx.cs" Inherits="WebLab.seguimientoCovid" %>

<style>.btn-sq-lg {
  width: 150px !important;
  height: 150px !important;
}

.btn-sq {
  width: 100px !important;
  height: 100px !important;
  font-size: 10px;
}

.btn-sq-sm {
  width: 50px !important;
  height: 50px !important;
  font-size: 10px;
}

.btn-sq-xs {
  width: 25px !important;
  height: 25px !important;
  padding:2px;
}
</style>
 
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
 


<div  id="pnlSeguimiento" runat="server">
               <div   class="panel panel-default">
   <div class="panel-heading"> 
       Panel Seguimiento Coronavirus (COVID-19) - Vigilancia Genómica
        </div>
         	<div class="panel-body" >  
                        <div class="row">
        <div class="col-lg-12">
        
          <p>
            <a href="Seguimiento.aspx" class="btn btn-sq btn-danger">
                <i class="fa fa-search fa-5x"></i><br/>
               DIARIO
            </a>
            <a href="SeguimientoResultados.aspx" class="btn btn-sq btn-success">
              <i class="fa fa-user fa-5x"></i><br/>
             RESULTADOS
            </a>
          
               <a href="SeguimientoIncidencias.aspx" class="btn btn-sq btn-info">
              <i class="fa fa-bars fa-5x"></i><br/>
              INCIDENCIAS
            </a>
               
             
               <a href="SeguimientoTestAntigenos.aspx" class="btn btn-sq btn-primary">
              <i class="fa fa-bars fa-5x"></i><br/>
               ANTIGENOS
            </a> </P>
            </DIV>
<div class="col-lg-12">
        
          <p>
             <a href="SeguimientodERIVACIONES.aspx" class="btn btn-sq btn-warning">
              <i class="fa fa-check fa-5x"></i><br/>
              DERIVACIONES
            </a>
              <a href="Estadisticas/ReportePorResultadoCovid.aspx" class="btn btn-sq btn-warning">
              <i class="fa fa-bars fa-5x"></i><br/>
              ESTADISTICAS  <br>RESULTADOS
            </a>
                 <a href="SeleccionSecuenciacion.aspx" class="btn btn-sq btn-primary">
              <i class="fa fa-bars fa-5x"></i><br/>
              POSITIVOS  <br>CON CT
            </a>
                <a href="SeguimientoSecuenciacion.aspx" class="btn btn-sq btn-success">
              <i class="fa fa-bars fa-5x"></i><br/>
              VIGILANCIA  <br>GENÓMICA
            </a>    
          </p>
            </div>     
                               </div> 


            <p><br /></p>
                                                 <asp:Button ID="btnResumenSemanal" runat="server" Text="RESUMEN PERIODICO"  Width="220px"  OnClick="btnResumenSemanal_Click"  />
        
                  <p><br /></p>
                                <anthem:Button ID="btnEstado" runat="server" Text="VER INDICADORES SITUACION" AutoUpdateAfterCallBack="True" OnClick="btnEstado_Click" Width="220px" />
         <p><br /></p>
                    <anthem:GridView ID="gvSeguimiento" runat="server" AutoGenerateColumns="False"   CellPadding="20" CellSpacing="10" ShowHeader="False" BorderStyle="None" CssClass="table table-bordered bs-table" >
                            <Columns>
                                <asp:BoundField DataField="CoVid-19" HeaderText="CoVid-19">
                                <ItemStyle Width="300px" Height="30px" CssClass="label label-default" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                <ItemStyle Width="50px" Height="30px" CssClass="label label-danger"  />
                                     
                                </asp:BoundField>
                            </Columns>
                              <RowStyle Height="25px" />
                            
                        </anthem:GridView>
                
                         
 </div>
                   <div class="panel-footer" > 
             

                          <div   class="panel panel-default">
   <div class="panel-heading"> 
        <anthem:Button ID="btnVerAcumulados" runat="server" Text="VER TOTALES ACUMULADOS" AutoUpdateAfterCallBack="True" OnClick="btnVerAcumulados_Click" Width="200px" />
        </div>
         	<div class="panel-body" >  
                      <anthem:GridView ID="gvSeguimientoTotal" runat="server" AutoGenerateColumns="False" Width="350px" CellPadding="20" CellSpacing="10" ShowHeader="False" BorderStyle="None">
                            <Columns>
                                <asp:BoundField DataField="TOTAL" HeaderText="CoVid-19">
                                <ItemStyle Width="350px"  CssClass="label label-default"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                <ItemStyle Width="50px"   CssClass="label label-danger" />
                                     
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle CssClass="label label-danger" />
                            <RowStyle Height="25px" />
                        </anthem:GridView>

                 </div>
                              </div>
                              
                        
           <div class="row">
               <div class="col-lg-6">   
<anthem:GridView ID="gvSeguimiento0" runat="server" AutoGenerateColumns="False" Width="350px" CellPadding="20" CellSpacing="10" ShowHeader="False" BorderStyle="None">
                            <Columns>
                                <asp:BoundField DataField="Caracter" HeaderText="CoVid-19">
                                <ItemStyle Width="300px"  CssClass="label label-default"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                <ItemStyle Width="50px" CssClass="label label-danger" />
                                     
                                </asp:BoundField>
                            </Columns>
                            <RowStyle Height="20px" />
                        </anthem:GridView>
                   </div>
        <div class="col-lg-6">                
 
            </div>

               </div>            
                           <br />
                        <br />
                        
                        
                       

                    </div>
                 
            </div>

    </div>