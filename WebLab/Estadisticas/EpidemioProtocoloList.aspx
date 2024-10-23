<%@ Page Title="" Language="C#" MasterPageFile="~/Epi1.Master" AutoEventWireup="true" CodeBehind="EpidemioProtocoloList.aspx.cs" Inherits="WebLab.Estadisticas.EpidemioProtocoloList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--     
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  



  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
     
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   --%>
    
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    <div align="left" style="width: 1350px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h2 >Fichas Clinicas - Covid19</h2>
                        <asp:Button ID="btnNuevo" runat="server" Text="Agregar" PostBackUrl="~/Estadisticas/EpidemioProtococolo.aspx" />
                          <asp:Button ID="btnExcel" runat="server" Text="Descargar Excel" OnClick="btnExcel_Click" />
                        <p >
                              <div style="overflow-y:auto;width:1300px; height:500px;">  
                            <asp:GridView ID="GridView1" runat="server" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Editar">
                                          <ItemTemplate>
                                           <asp:HyperLink   ID="HyperLink1" runat="server" CssClass="green"    NavigateUrl='<%# Generateurl((string)Eval("protocolo"))%>'>Editar</asp:HyperLink>
                                              </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="protocolo" HeaderText="protocolo">
                                    <ItemStyle Width="20px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Viaje_Z_Riesgo_14_dias_previos" HeaderText="Viaje Zona Riesgo">
                                    <ItemStyle Width="20px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Lugar" HeaderText="Lugar Viaje" />
                                    <asp:BoundField DataField="Fecha_ing" HeaderText="Fecha Ingreso" />
                                    <asp:BoundField DataField="Medio_trans" HeaderText="Medio transporte" />
                                    <asp:BoundField DataField="Contacto_con_personas_con_IRA_14_dias_previos" HeaderText="Contacto con personas con IRA">
                                    <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Entorno_familiar_IRA" HeaderText="Entorno familiar " />
                                    <asp:BoundField DataField="Entorno_asistencial_IRA" HeaderText="Entorno Asistencial" />
                                    <asp:BoundField DataField="Otro_entorno_IRA" HeaderText="Otro entorno" />
                                    <asp:BoundField DataField="Contacto_con_Confirmados_Covid" HeaderText="Contacto con Confirmados Covid">
                                    <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Entorno_Covid_familiar" HeaderText="Entorno_Covid_familiar" />
                                    <asp:BoundField DataField="Entorno_Covid_asistencial" HeaderText="Entorno_Covid_asistencial" />
                                    <asp:BoundField DataField="Otro_entorno_Covid" HeaderText="Otro_entorno_Covid" />
                                    <asp:BoundField DataField="Fecha_Inicio_sintomas" HeaderText="Fecha_Inicio_sintomas" />
                                    <asp:BoundField DataField="Fecha_Primera_consulta" HeaderText="Fecha_Primera_consulta" />
                                    <asp:BoundField DataField="Internacion" HeaderText="Internacion" />
                                    <asp:BoundField DataField="Fecha_Internacion" HeaderText="Fecha_Internacion" />
                                    <asp:BoundField DataField="UTI" HeaderText="UTI" />
                                    <asp:BoundField DataField="Fecha_Int_UTI" HeaderText="Fecha_Int_UTI" />
                                    <asp:BoundField DataField="ARM" HeaderText="ARM" />
                                    <asp:BoundField DataField="Dx_ingreso" HeaderText="Dx_ingreso" />
                                    <asp:BoundField DataField="Dx_Otros" HeaderText="Dx_Otros" />
                                    <asp:BoundField DataField="Comorbilidades" HeaderText="Comorbilidades" />
                                    <asp:BoundField DataField="Com_Enf_Resp" HeaderText="Com_Enf_Resp" />
                                    <asp:BoundField DataField="Com_Enf_Neuro" HeaderText="Com_Enf_Neuro" />
                                    <asp:BoundField DataField="Com_Inmunosupresion" HeaderText="Com_Inmunosupresion" />
                                    <asp:BoundField DataField="Com_Enf_cardio" HeaderText="Com_Enf_cardio" />
                                    <asp:BoundField DataField="Com_HTA" HeaderText="Com_HTA" />
                                    <asp:BoundField DataField="Com_Renal" HeaderText="Com_Renal" />
                                    <asp:BoundField DataField="Com_Hepatica" HeaderText="Com_Hepatica" />
                                    <asp:BoundField DataField="Com_DBT" HeaderText="Com_DBT" />
                                    <asp:BoundField DataField="Com_Desnutricion" HeaderText="Com_Desnutricion" />
                                    <asp:BoundField DataField="Com_IMC_Mayor_30" HeaderText="Com_IMC_Mayor_30" />
                                    <asp:BoundField DataField="Com_Perinatales" HeaderText="Com_Perinatales" />
                                    <asp:BoundField DataField="Com_Tto_Atb_Previo" HeaderText="Com_Tto_Atb_Previo" />
                                    <asp:BoundField DataField="Com_Fecha_Tto_Atb_Previo" HeaderText="Com_Fecha_Tto_Atb_Previo" />
                                    <asp:BoundField DataField="Com_Tto_antiviral_previo" HeaderText="Com_Tto_antiviral_previo" />
                                    <asp:BoundField DataField="Com_Fecha_Tto_antiviral_previo" HeaderText="Com_Fecha_Tto_antiviral_previo" />
                                    <asp:BoundField DataField="Com_Antigripal_2020" HeaderText="Com_Antigripal_2020" />
                                </Columns>
                            </asp:GridView>
                                  </div>
                        </p>
                        </div>

				
        </div>
    </div>
</asp:Content>
