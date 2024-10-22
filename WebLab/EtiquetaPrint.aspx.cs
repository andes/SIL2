using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab
{
    public partial class EtiquetaPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {    
            //string mensagito = @"S0001912345678912345PINTOS BEATRIZ CAROLINA      X18011979f033departamento       D200920120.78.5";
            /////Decofificar mensagito phoresys por posicion de ocupacion
            //string s_Prueba = mensagito.Substring(0, 1);
            //string s_NumeroMuestra = mensagito.Substring(1, 4);
            //string s_NumeroDocumento = mensagito.Substring(5, 15);
            //string s_Paciente = mensagito.Substring(20, 30);
            //string s_FechaNacimiento = mensagito.Substring(50, 8);
            //string s_Sexo = mensagito.Substring(58, 1);
            //string s_Edad = mensagito.Substring(59, 3);
            //string s_Domicilio = mensagito.Substring(62, 20);
            //string s_FechaProtocolo = mensagito.Substring(82, 8);
            //string s_Resultado = mensagito.Substring(90,5);
            ///////Lo que sigue son textos libres de extension 30 caracteres


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Imprimir("PDFCreator");
        }

        private void Imprimir(string s_impresora)
        {

            // aca se controla si la linea supera los 35 caracteres (no seria mi caso) debo controlar que no supere antes
            Business. ticket = new Business.Reporte();
            
//ticket.HeaderImage = "C:\imagen.jpg"; //esta propiedad no es obligatoria

        


            ////////////////////////////////
ticket.AddHeaderLine("PEREZ JUAN CARLOS");
ticket.AddSubHeaderLine("M  27896785  09/09/2012" );
ticket.AddSubHeaderLine("28A  AMBULATORIO   MG");
ticket.AddSubHeaderLine("HEMATOLOGIA");
            
            //ticket.AddItem("1", "Articulo Prueba", "15.00" );

//El metodo AddSubHeaderLine es lo mismo al de AddHeaderLine con la diferencia
//de que al final de cada linea agrega una linea punteada "=========="
ticket.AddCodigoBarras("123456", "Ccode39M43");
ticket.AddFooterLine("123456");
//El metodo AddItem requeire 3 parametros, el primero es cantidad, el segundo es la descripcion
//del producto y el tercero es el precio


//ticket.AddTotal("", "" );//Ponemos un total en blanco que sirve de espacio

//ticket.AddFooterLine("GRACIAS POR TU VISITA" );

//Y por ultimo llamamos al metodo PrintTicket para imprimir el ticket, este metodo necesita un
//parametro de tipo string que debe de ser el nombre de la impresora.
ticket.PrintTicket(s_impresora, "Code39-Digits"); 
        }
    }
}