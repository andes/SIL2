﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebLab.CasoFiliacion.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="js/jquery-1.12.0.js"></script>
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
	<script type="text/javascript" src="js/editor.js"></script>	
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
	<link rel="stylesheet" type="text/css" href="css/editor.css">
	<script type="text/javascript">
		$(document).ready(function(){
			$('#txt-content').Editor();

			$('#txt-content').Editor('setText', ['<p style="color:red;">Hola</p>']);

			
		});	
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="container">
		<div class="row">
			<div class="col-sm-8">
				<form action="prueba.php" method="post" id="frm-test">
					<div class="form-group">
						<textarea id="txt-content" name="txt-content"></textarea>
					</div>
					<input type="submit" class="btn btn-default" id="btn-enviar" value="Mostrar Resultado">
				</form>
			</div>
		</div>
		<div class="row">
			<div class="col-sm-8">
				<div id="texto" style="border:1px solid #000; padding:10px;margin-top:20px;">
					
				</div>
			</div>
		</div>
	</div>
    </div>
    </form>
</body>
</html>
