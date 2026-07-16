using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace Business.Helpers
{
    public static class CatalogoCache
    {
        private const int MinutosCache = 60;

        /// <summary>
        /// Obtiene un DataTable desde Cache o desde SQL.
        /// </summary>
        public static DataTable GetDataTable(string cacheKey, string sql, string conexion)
        {
            Cache cache = HttpRuntime.Cache;

            DataTable dt = cache[cacheKey] as DataTable;

            if (dt != null)
                return dt;

            dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(              conexion))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                da.Fill(dt);
            }

            cache.Insert(
                cacheKey,
                dt,
                null,
                DateTime.Now.AddMinutes(MinutosCache),
                Cache.NoSlidingExpiration);

            return dt;
        }

        /// <summary>
        /// Elimina un catálogo del cache.
        /// </summary>
        public static void Remove(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        /// <summary>
        /// Limpia todos los catálogos.
        /// </summary>
        public static void ClearCatalogos()
        {
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                string key = enumerator.Key.ToString();

                if (key.StartsWith("CAT_"))
                    HttpRuntime.Cache.Remove(key);
            }
        }
    }
}