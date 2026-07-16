using System.Data;
using System.Web.UI.WebControls;

namespace Business.Helpers
{
    public static class ComboCache
    {
        public static void CargarCombo(
            DropDownList ddl,
            string cacheKey,
            string sql,
            string valueField,
            string textField, string conexion,
            string textoInicial = null,
            string valorInicial = "0"
            )
        {
            DataTable dt = CatalogoCache.GetDataTable(cacheKey, sql, conexion);

            ddl.DataSource = dt;
            ddl.DataValueField = valueField;
            ddl.DataTextField = textField;
            ddl.DataBind();

            if (!string.IsNullOrEmpty(textoInicial))
                ddl.Items.Insert(0, new ListItem(textoInicial, valorInicial));
        }
    }
}