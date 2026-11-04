
<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="IncidenciaEdit.ascx.cs"
    Inherits="WebLab.Calidad.IncidenciaEdit" %>

<asp:TreeView
    ID="TreeView2"
    runat="server"
    ImageSet="Arrows"
    ShowLines="True"
    EnableViewState="false"
    ExpandDepth="0"
    ShowCheckBoxes="None"
    CssClass="tree-incidencias">

    <NodeStyle
        Font-Names="Verdana"
        Font-Size="9pt"
        ForeColor="Black"
        Height="25px"
        HorizontalPadding="5px"
        VerticalPadding="2px" />

    <HoverNodeStyle
        Font-Underline="True"
        ForeColor="#5555DD" />

    <SelectedNodeStyle
        Font-Underline="False"
        ForeColor="#5555DD" />

</asp:TreeView>
