using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;

namespace WebLab
{
    public partial class comparaString : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //TextBox1.Text = "1696345";
            //HFIdPaciente.Value = "1696345";

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("PeticionElectronica/PeticionList.aspx", false);
            //Response.Redirect("PeticionElectronica/PeticionEdit.aspx?idPaciente=1619852&Modifica=0&idTipoServicio=1&master=1", false);
            string root = TextBox1.Text;
            string root2 = TextBox2.Text;
            //bool areEqual = String.Compare(root, root2, StringComparison.OrdinalIgnoreCase);
            //TextBox3.Text = areEqual.ToString();

        //    int com=String.Compare(TextBox1.Text, TextBox2.Text);
        //TextBox3.Text = com.ToString();
            //if (String.Compare(TextBox1.Text, TextBox2.Text) == 0)
            //{
                
            //    //igualitos
            //}
            //else
            //{
            //    //no son igualitos
            //}
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("PeticionElectronica/PeticionEdit.aspx?idPaciente=1619852&Modifica=0&idTipoServicio=1&master=1", false);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("PeticionElectronica/PeticionEdit.aspx?idPaciente=1619852&Modifica=0&idTipoServicio=1&master=1", false);
        }

        protected void Button1_Click2(object sender, EventArgs e)
        {
            Utility oi = new Utility();
            TextBox2.Text = oi.SacaComillas(TextBox1.Text);
        }
    }
}