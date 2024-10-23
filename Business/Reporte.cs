using System;



using System.Drawing;
using System.Drawing.Printing;
using System.Collections;

namespace Business
{


    public class Reporte
    {
        ArrayList headerLines = new ArrayList();
        ArrayList subHeaderLines = new ArrayList();
        ArrayList subHeaderLinesNegrita = new ArrayList();
        ArrayList subCodigoBarras = new ArrayList();
        ArrayList totales = new ArrayList();
        ArrayList footerLines = new ArrayList();
        private Image headerImage = null;

        int count = 1;

        int maxChar = 35;
        int maxCharDescription = 20;

        int imageHeight = 0;
        
        float leftMargin = 14;
        //float topMargin = 2;

        string fontName = "Verdana";
        
        int fontSize =9;
        int fontSizeSubLinea = 8;
        int fontSizeCodigoBarras2 = 10;
        
        Font printFont = null;
        Font printFontSubLinea = null;
        Font printFontCodigoBarras = null;
        SolidBrush myBrush = new SolidBrush(Color.Black);

        Graphics gfx = null;

        string line = null;

        public Reporte()
        {

        }

        public Image HeaderImage
        {
            get { return headerImage; }
            set { if (headerImage != value) headerImage = value; }
        }

        public int MaxChar
        {
            get { return maxChar; }
            set { if (value != maxChar) maxChar = value; }
        }

        public int MaxCharDescription
        {
            get { return maxCharDescription; }
            set { if (value != maxCharDescription) maxCharDescription = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { if (value != fontSize) fontSize = value; }
        }

        public int FontSizeCodigoBarras
        {
            get { return fontSizeCodigoBarras2; }
            set { if (value != fontSizeCodigoBarras2) fontSizeCodigoBarras2 = value; }
        }
        public string FontName
        {
            get { return fontName; }
            set { if (value != fontName) fontName = value; }
        }

        public void AddHeaderLine(string line)
        {
            headerLines.Add(line);
        }

        public void AddSubHeaderLine(string line)
        {
            subHeaderLines.Add(line);
        }
        public void AddSubHeaderLineNegrita(string line)
        {
            subHeaderLinesNegrita.Add(line.ToUpper());
        }
        public void AddCodigoBarras(string line, string fuenteBarCode)
        {
            switch (fuenteBarCode)
            {
                case "CCode39": line = "*" + line + "*"; break;
                case "Ccode39M43":
                    {
                        string cVerificador = ValidateMod43(line);
                     line = "*" + line  + cVerificador + "*"; break;
                }
            }
            subCodigoBarras.Add(line);
        }


        //public void AddItem(string cantidad, string item, string price)
        //{
        //    OrderItem newItem = new OrderItem('?');
        //    items.Add(newItem.GenerateItem(cantidad, item, price));
        //}

        public void AddTotal(string name, string price)
        {
            OrderTotal newTotal = new OrderTotal('?');
            totales.Add(newTotal.GenerateTotal(name, price));
        }

        public void AddFooterLine(string line)
        {
            footerLines.Add(line);
        }

        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < maxChar; x++)
                dotted += "=";
            return dotted;
        }

        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }

        public void PrintTicket(string impresora, string fuenteBarCode)
        {
            printFont = new Font(fontName, fontSize, FontStyle.Bold);
            fuenteBarCode="CCode39";
            printFontCodigoBarras = new Font(fuenteBarCode, fontSizeCodigoBarras2, FontStyle.Regular);
            printFontSubLinea = new Font(fontName, fontSizeSubLinea, FontStyle.Regular);
            PrintDocument pr = new PrintDocument();
            pr.PrinterSettings.PrinterName = impresora;
            pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            pr.Print();
        }

        private string ValidateMod43(string barcode)
        {
            int subtotal = 0;
            const string charSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";

            foreach (char c in barcode)

                subtotal += charSet.IndexOf(c);

            return charSet[subtotal % 43].ToString();

        }

        private void pr_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            gfx = e.Graphics;

        //    DrawImage();
            DrawHeader();
            DrawSubHeader();
            DrawSubHeaderNegrita();
            DrawCodigoBarras();
                      //DrawItems();
           // DrawTotales();
            DrawFooter();

            //if (headerImage != null)
            //{
            //    HeaderImage.Dispose();
            //    headerImage.Dispose();
            //}
        }

        private void DrawSubHeaderNegrita()
        {
            foreach (string header in subHeaderLinesNegrita)
            {
                Font printFont_Area= new Font(fontName, 7, FontStyle.Bold);
                line = header;
                gfx.DrawString(line, printFont_Area, myBrush, leftMargin, YPosition(1), new StringFormat());

                count++;
                //  }
            }
            //DrawEspacio();
        }

        private float YPosition(float topMargin)
        {
            return topMargin + (count *  printFontSubLinea.GetHeight(gfx) + imageHeight);
            //return topMargin + (count * float.Parse("8.5") + imageHeight);
        }

        private void DrawImage()
        {
            if (headerImage != null)
            {
                try
                {
                    gfx.DrawImage(headerImage, new Point((int)leftMargin, (int)YPosition(1)));
                    double height = ((double)headerImage.Height / 58) * 15;
                    imageHeight = (int)Math.Round(height) + 3;
                }
                catch (Exception)
                {
                }
            }
        }

        private void DrawHeader()
        {
            foreach (string header in headerLines)
            {
                
                {
                   
                    line = header;

                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(1), new StringFormat());

                    count++;
                }
            }
         //   DrawEspacio();
        }


        private void DrawSubHeader()
        {
            foreach (string header in subHeaderLines)
            {
               
                    line = header;
                    gfx.DrawString(line, printFontSubLinea, myBrush, leftMargin, YPosition(1), new StringFormat());

                    count++;
              //  }
            }
            DrawEspacio();
        }

        private void DrawCodigoBarras()
        {
            foreach (string subHeader in subCodigoBarras)
            {
                //if (subHeader.Length > maxChar)
                //{
                //    int currentChar = 0;
                //    int subHeaderLenght = subHeader.Length;

                //    while (subHeaderLenght > maxChar)
                //    {

                //        line = subHeader;
                //        gfx.DrawString(line.Substring(currentChar, maxChar), printFontCodigoBarras, myBrush, leftMargin, YPosition(2), new StringFormat());

                //        count++;
                //        currentChar += maxChar;
                //        subHeaderLenght -= maxChar;
                //    }
                //    line = subHeader;
                //    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(2), new StringFormat());
                //    count++;
                //}
                //else
               // {
                    line = subHeader;

                    gfx.DrawString(line, printFontCodigoBarras, myBrush, leftMargin, YPosition(2), new StringFormat());

                    count++;

                    // line = DottedLine();

                    //gfx.DrawString(line, printFontCodigoBarras, myBrush, leftMargin, YPosition(), new StringFormat());

                    //count++;
                //}
            }
            //DrawEspacio();
        }

        //private void DrawSubHeader()
        //{
        //    foreach (string subHeader in subHeaderLines)
        //    {
        //        if (subHeader.Length > maxChar)
        //        {
        //            int currentChar = 0;
        //            int subHeaderLenght = subHeader.Length;

        //            while (subHeaderLenght > maxChar)
        //            {
                   
        //                line = subHeader;
        //                gfx.DrawString(line.Substring(currentChar, maxChar), printFontCodigoBarras, myBrush, leftMargin, YPosition(), new StringFormat());

        //                count++;
        //                currentChar += maxChar;
        //                subHeaderLenght -= maxChar;
        //            }
        //            line = subHeader;
        //            gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
        //            count++;
        //        }
        //        else
        //        {
        //            line = subHeader;

        //            gfx.DrawString(line, printFontCodigoBarras, myBrush, leftMargin, YPosition(), new StringFormat());

        //            count++;

        //           // line = DottedLine();

        //            //gfx.DrawString(line, printFontCodigoBarras, myBrush, leftMargin, YPosition(), new StringFormat());

        //            //count++;
        //        }
        //    }
        //    DrawEspacio();
        //}

        //private void DrawItems()
        //{
        //    OrderItem ordIt = new OrderItem('?');

        //    gfx.DrawString("CANT  DESCRIPCION           IMPORTE", printFont, myBrush, leftMargin, YPosition(), new StringFormat());

        //    count++;
        //    DrawEspacio();

        //    foreach (string item in items)
        //    {
        //        line = ordIt.GetItemCantidad(item);

        //        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

        //        line = ordIt.GetItemPrice(item);
        //        line = AlignRightText(line.Length) + line;

        //        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

        //        string name = ordIt.GetItemName(item);

        //        leftMargin = 0;
        //        if (name.Length > maxCharDescription)
        //        {
        //            int currentChar = 0;
        //            int itemLenght = name.Length;

        //            while (itemLenght > maxCharDescription)
        //            {
        //                line = ordIt.GetItemName(item);
        //                gfx.DrawString("      " + line.Substring(currentChar, maxCharDescription), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

        //                count++;
        //                currentChar += maxCharDescription;
        //                itemLenght -= maxCharDescription;
        //            }

        //            line = ordIt.GetItemName(item);
        //            gfx.DrawString("      " + line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
        //            count++;
        //        }
        //        else
        //        {
        //            gfx.DrawString("      " + ordIt.GetItemName(item), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

        //            count++;
        //        }
        //    }

        //    leftMargin = 0;
        //    DrawEspacio();
        //    line = DottedLine();

        //    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

        //    count++;
        //    DrawEspacio();
        //}

        //private void DrawTotales()
        //{
        //    OrderTotal ordTot = new OrderTotal('?');

        //    foreach (string total in totales)
        //    {
        //        line = ordTot.GetTotalCantidad(total);
        //        line = AlignRightText(line.Length) + line;

        //        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
        //        leftMargin = 0;

        //        line = "      " + ordTot.GetTotalName(total);
        //        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
        //        count++;
        //    }
        //    leftMargin = 0;
        //    DrawEspacio();
        //    DrawEspacio();
        //}

        private void DrawFooter()
        {
            foreach (string footer in footerLines)
            {
                
                    line = footer;
                    Font printFont_Numero = new Font(fontName, 8, FontStyle.Bold);
                    //gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(8), new StringFormat());
                    gfx.DrawString(line, printFont_Numero, myBrush, leftMargin, YPosition(8), new StringFormat());

                    count++;
              //  }
            }
       //     leftMargin = 0;
           // DrawEspacio();
        }

        private void DrawEspacio()
        {
            line = "";
            Font printFontLineaEspacio = new Font(fontName, 7, FontStyle.Regular);
            gfx.DrawString(line, printFontLineaEspacio, myBrush, leftMargin, YPosition(1), new StringFormat());

            count++;
        }

       
    }

 
    
}