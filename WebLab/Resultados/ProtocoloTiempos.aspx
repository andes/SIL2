
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ProtocoloTiempos.aspx.cs"
    Inherits="WebLab.Resultados.ProtocoloTiempos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tiempos del protocolo</title>

    <style type="text/css">
      body {
    font-family: Arial, sans-serif;
    font-size: 13px;
    background: #ffffff;
    margin: 0;
    padding: 10px;
    color: #333;
}

      .contenedor {
    background: white;
    border: none;
    padding: 10px;
    max-width: 100%;
    margin: 0;
}

        .titulo {
            font-size: 18px;
            font-weight: bold;
            color: #285580;
            margin-bottom: 5px;
        }

        .subtitulo {
            color: #777;
            margin-bottom: 20px;
        }

        .fila {
            display: flex;
            justify-content: space-between;
            padding: 9px 0;
            border-bottom: 1px solid #eee;
        }

        .etiqueta {
            color: #666;
        }

        .valor {
            font-weight: bold;
        }

        .tiempo-total {
            margin-top: 18px;
            padding: 15px;
            background: #eef4fa;
            border-radius: 5px;
            text-align: center;
        }

        .tiempo-total .numero {
            font-size: 22px;
            font-weight: bold;
            color: #285580;
            margin-top: 6px;
        }

        .seccion {
            margin-top: 25px;
            font-weight: bold;
            color: #285580;
            border-bottom: 1px solid #ddd;
            padding-bottom: 8px;
            margin-bottom: 10px;
        }

        .tabla {
            width: 100%;
            border-collapse: collapse;
        }

        .tabla th {
            background: #f2f4f6;
            text-align: left;
            padding: 9px;
            border-bottom: 1px solid #ddd;
        }

        .tabla td {
            padding: 9px;
            border-bottom: 1px solid #eee;
        }

        .pie {
            margin-top: 20px;
            text-align: right;
        }

        .btn {
            background: #285580;
            color: white;
            border: none;
            border-radius: 4px;
            padding: 9px 22px;
            cursor: pointer;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

    <div class="contenedor">

        

        <div class="titulo">
            Protocolo N.º
            <asp:Label ID="lblNumero" runat="server" />
        </div>

        <div class="fila">
            <span class="etiqueta">Fecha de ingreso</span>
            <asp:Label ID="lblFechaIngreso"
                runat="server" CssClass="valor" />
        </div>

        <div class="fila">
            <span class="etiqueta">Finalización general</span>
            <asp:Label ID="lblFechaFin"
                runat="server" CssClass="valor" />
        </div>

        <div class="fila">
            <span class="etiqueta">Estado</span>
            <asp:Label ID="lblEstado"
                runat="server" CssClass="valor" />
        </div>

        <div class="tiempo-total">
            <div>Tiempo total transcurrido</div>
            <div class="numero">
                <asp:Label ID="lblTiempoTotal" runat="server" />
            </div>
        </div>

        <asp:Panel ID="pnlAreas" runat="server"
            Visible="false">

            <div class="seccion">Cierres por área</div>

            <asp:GridView ID="gvAreas"
                runat="server"
                AutoGenerateColumns="false"
                CssClass="tabla"
                GridLines="None"
                ShowHeader="true">

                <Columns>
                    <asp:BoundField
                        DataField="nombreArea"
                        HeaderText="Área" />

                    <asp:BoundField
                        DataField="fechaTermina"
                        HeaderText="Finalización"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                    <asp:BoundField
                        DataField="tiempo"
                        HeaderText="Tiempo transcurrido" />
                </Columns>

            </asp:GridView>

        </asp:Panel>

  

    </div>

</form>
</body>
</html>
