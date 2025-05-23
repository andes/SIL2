﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inscripcion.aspx.cs" Inherits="WebLab.website.jornadas.inscripcion" %>

<!DOCTYPE html>

<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
    <title>X Jornadas Bioquímicas Provinciales</title>
<link href="css/bootstrap.min.css" rel="stylesheet" />
<link href="css/style.css?v=1.0" rel="stylesheet" type="text/css">
<link rel="stylesheet" href="css/font-awesome.min.css">
<script src="js/jquery.min.js"></script>
<script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
<link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900' rel='stylesheet' type='text/css'>
    <script src='https://www.google.com/recaptcha/api.js'></script>


   
</head>
<body>
<div class="header">
  <div class="container"><a class="navbar-brand" href="index.html"><h3>X Jornadas Bioquímicas Provinciales</h3></a>
            <div class="menu">
                <a class="toggleMenu" href="#"><img src="images/nav_icon.png" alt="" /> </a>
                <ul class="nav" id="nav">
                    <li class="current"><a href="index.html">Jornadas</a></li>
                    <li><a href="temario.html">Programa</a></li>
                    <li><a href="inscripcion.aspx">Inscripciones</a></li>                   
                    <li><a href="auspiciantes.html">Auspiciantes</a></li>
                    <li><a href="services.html">Expositores</a></li>
                    <!--<li><a href="index.html">Certificados</a></li>--> 
                    <div class="clear"></div>
                        
                </ul>
      <script type="text/javascript" src="js/responsive-nav.js"></script> 
    </div>
  </div>
</div>
<!--<div class="about">
  <div class="container">
    <section class="title-section">
      <h1 class="title-header">Contact Us</h1>
    </section>
  </div>
</div>-->
<div class="contact">
   
  <div class="container">
      <h2>Inscripción</h2>
    <%--<div class="row contact_top">
      <div class="col-md-4 contact_details">
        <h5>Dirección:</h5>
        <div class="contact_address">Gregorio Martínez 65, (8300) Neuquén - Argentina	</div>
      </div>
     
      <div class="col-md-4 contact_details">
        <h5>Email :</h5>
        <div class="contact_mail"> labcen@yahoo.com.ar</div>
      </div>
    </div>--%>
    <div class="contact_bottom">    
       
        <form id="form1" runat="server" method="post">
            

        <div id="container">         

          

            

              
                 <div style=" padding-top: 10px;
	
    padding-bottom:20px; padding-right:10px;">
                    <h3><span class="green"><strong></strong></span></h3>
   <h4>Las inscripciones se cerraron.</h4>
                     
             

      
                
                       
    
  
        </div> <!--! end container -->
     </form>
        
    
       
    </div>
  </div>
</div>
   <div class="footer">
            <div class="footer_bottom">
             
              
              <div class="follow-us">
                
                  <a class="fa fa-facebook social-icon" href="http://www.facebook.com/sharer.php?u=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank" rel="nofollow" title="Compartir en Facebook"></a>
               

                  <a class="fa fa-twitter social-icon" href="http://twitter.com/home?status=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank"  rel="nofollow" title="Compartir en Twitter"></a>
                  <a class="fa fa-linkedin social-icon" href="http://www.linkedin.com/shareArticle?url=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank" rel="nofollow" title="Compartir en Linkedin"></a>
                  <a class="fa fa-google-plus social-icon" href="https://plus.google.com/share?url=http://laboratoriocentral.neuquen.gob.ar/website/jornadas/index.html" target="_blank" rel="nofollow" title="Compartir en Google+"></a>
                
                      
</div>
                <div class="copy">
                    <p>Copyright &copy; 2018 <a target="_blank" href="http://laboratoriocentral.neuquen.gob.ar" rel="nofollow">Laboratorio Central</a></p>
                </div>
                
              
</div>
            <a href="http://www.saludneuquen.gov.ar/" target="_blank">
                <img src="../images/logo_ministerio.png" />
            </a>
          
</div>
     <script type="text/javascript">
        // Get your number elements
        var numberElements = document.getElementsByClassName("number");
        // Loop through each one
        for (var i = 0; i < numberElements.length; i++) {
            // Get your current element
            numberElements[i].type = 'number';
        }
</script>
<script src="js/bootstrap.min.js"></script> 
<script src="js/jquery.flexslider.js"></script>
</body>
</html>
