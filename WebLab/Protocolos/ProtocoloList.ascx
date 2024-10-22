<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloList.ascx.cs" Inherits="WebLab.Protocolos.ProtocoloList1" %>
<%--<asp:Panel ID="pnlProtocolos" runat="server">--%>
                         
						 <div  style="overflow:scroll;overflow-x:hidden;width:310px;"  class="form-inline" > 
                             
                                 <asp:DataList ID="DataList1" runat="server"
                                                 onitemdatabound="DataList1_ItemDataBound"  
width="300px" CellPadding="4" ForeColor="#333333" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
>
                                               
                                            
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                              
                                                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                               
                                                <ItemTemplate>
                                                    <table width="100%" cellpadding="1" cellspacing="0">
                                                        <tr>                                                           
                                                            <td colspan="2" class="myLabelIzquierda">
                                                                
                                                                <asp:HyperLink ID="hplProtocoloEdit" NavigateUrl=<%# DataBinder.Eval(Container.DataItem, "idProtocolo")%>  runat="server"><b> <%# DataBinder.Eval(Container.DataItem, "numero") %></b></asp:HyperLink>
                                                             - 
                                                                <b><%# DataBinder.Eval(Container.DataItem, "paciente") %></b>

                                                             <%--   <a  href="ProtocoloEdit2.aspx?idProtocolo=<%# DataBinder.Eval(Container.DataItem, "idProtocolo")%>&Operacion=Modifica&idPaciente=<%# DataBinder.Eval(Container.DataItem, "idPaciente")%>" style="border-style: none"><img src="../App_Themes/default/images/zoom.png" /></a>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>                                                        
                                                            <td class="mylabel">
                                                                DU:<b><%# DataBinder.Eval(Container.DataItem, "numeroDocumento") %></b></td>                                                             
                                                            <td align="right"  class="mylabel">                                                                       
                                                            <asp:HyperLink ID="lnkNuevoProtocolo" Visible="false"  runat="server">Nuevo Protocolo</asp:HyperLink>                                                    
                                                            <asp:Label ID="lblidPaciente" runat="server" Visible="false" Text=<%# DataBinder.Eval(Container.DataItem, "idPaciente") %> ></asp:Label> 
                                                                  <%--<a id="nuevoLabo" runat="server" href='ProtocoloEdit2.aspx?idPaciente=<%# DataBinder.Eval(Container.DataItem, "idpaciente") %>&amp;Operacion=Alta&amp;idServicio=<%# DataBinder.Eval(Container.DataItem, "idTipoServicio") %>&amp;Urgencia=0'>
                                                                   
                                                                Nuevo Protocolo</a>                                                              --%>
                                                            </td>
                                                        </tr>       
                                                        <tr>
                                                        <td>  <asp:HyperLink ID="lnkMicrobiologia" Visible="false"  runat="server">Nuevo Microbiologia</asp:HyperLink></td>
                                                        <td> 
                                                        </td>
                                                        </tr>      <tr>
                                                        
                                                            <td class="mylabel" colspan="2">
                                                               <asp:Label ID="lblEfector" runat="server" ><%# DataBinder.Eval(Container.DataItem, "efector") %> </asp:Label>    
                                                            </td>
                                                           
                                                        </tr>                                              
                                                        <tr>
                                                        
                                                            <td class="mylabel" colspan="2">                                                                                                                     
                                                                <asp:Label ID="lblTipoMuestra" runat="server" >Tipo de Muestra:&nbsp;<b><%# DataBinder.Eval(Container.DataItem, "muestra") %></b> </asp:Label>                                                            
                                                               
                                                            </td>
                                                           
                                                        </tr>
                                                        <%-- <tr>
                                                        
                                                            <td   colspan="2">                                                                                                                     
                                                                <p>
                                                               <asp:Label ID="lblSISA" runat="server" >Nro. SISA:&nbsp; <span class="label label-danger"><%# DataBinder.Eval(Container.DataItem, "idCasoSISA") %></b>  </span></asp:Label>                                                           
                                                               </p>
                                                            </td>
                                                           
                                                        </tr>--%>
                                                                 <tr>
                                                        
                                                            <td class="mylabel" colspan="2">
                                                             Cargado por: <%# DataBinder.Eval(Container.DataItem, "username") %>  <%# DataBinder.Eval(Container.DataItem, "fechaRegistro")%>
                                                            </td>
                                                           
                                                        </tr>                                                                                                               
                                                    </table>   
                                                </ItemTemplate>

                                            </asp:DataList>
                                            <input id="HFidServicio" runat="server" type="hidden" />
                       
    <%--       </asp:Panel>--%>
                             </div>