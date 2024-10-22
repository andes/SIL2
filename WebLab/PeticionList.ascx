<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeticionList.ascx.cs" Inherits="WebLab.PeticionList" %>

                <div id="divPeticion" visible="false" runat="server"  style="width:310px;height:300px;overflow:scroll;overflow-x:hidden;" 
             align="right" > 
                  
             <br />

                        <asp:UpdatePanel runat="server" ID="TimedPanel" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
                </Triggers>
                <ContentTemplate>
                    <asp:Timer runat="server" ID="UpdateTimer" Interval="3000" OnTick="UpdateTimer_Tick" />
                   <table style="width:300px;overflow:scroll;overflow-x:hidden;" >
                       <tr><td align="left"><asp:Label ID="lblCantidad" CssClass="myLabelLitlle" runat="server" ></asp:Label>  </td>
                       <td> &nbsp;</td>
                       <td><asp:Label ID="lblActualizacion" CssClass="myLabelLitlle" runat="server" ></asp:Label>    </td></tr></table>  
                  

            
                <asp:DataList ID="DataList2" runat="server" onitemdatabound="DataList2_ItemDataBound"                                                 
width="290px" CellPadding="4" ForeColor="#333333" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
>
                                               
                                            
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                              
                                                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                               
                                            
<HeaderTemplate>
   PETICIONES 
   </HeaderTemplate>



                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>                                                           
                                                            <td  class="myLabelIzquierda"> 
                                                        PETICION NRO.&nbsp;         <%# DataBinder.Eval(Container.DataItem, "idPeticion")%> 
                                                            <br />                                                               
                                                              <b><%# DataBinder.Eval(Container.DataItem, "fechaHoraRegistro")%></b>    &nbsp;  &nbsp;  &nbsp;  &nbsp;
                                                                  <asp:HyperLink ID="hplPeticionEdit"  NavigateUrl=<%# DataBinder.Eval(Container.DataItem, "idPeticion")%>  runat="server"><b>Recepcionar</b></asp:HyperLink>                                                        
                                                            </td>
                                                        </tr>
                                                        <tr>                                                        
                                                            <td >
                                                           <b style="color: #FF0000; font-weight: bold; text-transform: uppercase"><%# DataBinder.Eval(Container.DataItem, "paciente")%>  </b></td>                                                             
                                                            
                                                        </tr> 
                                                          <tr>                                                        
                                                            <td >
                                                           <b>  DU:</b> <%# DataBinder.Eval(Container.DataItem, "DU")%>  </td>                                                             
                                                            
                                                        </tr>                                                        
                                                        <tr>
                                                        
                                                            <td >
                                                                
                                                           
                                                              <b>  Desde:  </b> <%# DataBinder.Eval(Container.DataItem, "origen")%> 
                                                            </td>
                                                            
                                                        </tr>
                                                                                                                                                          
                                                    </table>
                                                </ItemTemplate>

                                            </asp:DataList>
                                            </ContentTemplate>
</asp:UpdatePanel>
                                            </div>                             
