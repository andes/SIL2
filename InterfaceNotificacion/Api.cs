using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InterfaceNotificacion
{
    public class Api
    {
        private NpgsqlConnection cnn;
        private Api()
        {
            cnn = new NpgsqlConnection();
            cnn.ConnectionString = "Server=66.97.36.127;Port=5433;Database=glymsapi;User Id=postgres;Password=mama;";
        }
        private static Api instance = null;
        public static Api Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Api();
                }
                return instance;
            }
        }

        public Pendiente GetSISAEVENTO()
        {
            
            var cmd = @"SELECT * FROM public.sisa_evento where idevento='' or ideventomuestra='' or resultado='' limit 1";
            try
            {
                List<Object[]> result = Query(cmd);
                if (result.Count > 0)
                {
                    Pendiente p = new Pendiente()
                    {
                        protocolo = result[0][0].ToString(),
                        idevento = result[0][1].ToString(),
                        ideventomuestra = result[0][2].ToString(),
                        tipo = int.Parse(result[0][3].ToString()),
                        clasificacionmanual = int.Parse(result[0][4].ToString()),
                        resultado = result[0][5].ToString()
                    };
                    return p;
                }
            }
            catch (Exception ex)
            {
                ELog.save(this, "Error: " + ex.Message);
                //MessageBox.Show("Error: " + ex.Message, "Error Api", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
             return null;
        }

        public int GetCantidadSISAEVENTO()
        {

            var cmd = @"SELECT count(*) as cantidad FROM public.sisa_evento where idevento='' or ideventomuestra='' or resultado='' ";
            try
            {
                List<Object[]> result = Query(cmd);

                return result.Count-1;

            }
            catch (Exception ex)
            {
                ELog.save(this, "Error: " + ex.Message);
                //MessageBox.Show("Error: " + ex.Message, "Error Api", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }

        private void Connect()
        {
            try
            {
                cnn.Open();
            }
            catch
            {
                throw new NotSupportedException("Error de conexion");
            }
            finally
            {
                cnn.Close();
            }
        }

        private List<Object[]> Query(string command)
        {
            try
            {
                NpgsqlDataReader dr;
                NpgsqlCommand cmd = new NpgsqlCommand(command, cnn);

                cnn.Open();
                dr = cmd.ExecuteReader();
                List<object[]> result = new List<object[]>();
                while (dr.Read())
                {
                    object[] resutado = new Object[dr.FieldCount];
                    dr.GetValues(resutado);
                    result.Add(resutado);
                }

                return result;
            }
            catch
            {
                throw new NotSupportedException("Error obteniendo datos");
            }
            finally
            {
                cnn.Close();
            }
        }

        
        public void ActualizarEventoPendiente(string protocolo, string idevento)
        {
            try
            {   
                NpgsqlCommand cmd = new NpgsqlCommand("update sisa_evento set \"idevento\" = :idEvento where \"protocolo\" = '" + protocolo + "' ;", cnn);
                cnn.Open();
                cmd.Parameters.Add(new NpgsqlParameter("idEvento", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters[0].Value = idevento;              
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

            catch (Exception ex)
            {
                ELog.save(this, "Error: " + ex.Message);
                
            }
        }

        public void ActualizarMuestraPendiente(string protocolo, string idEventoMuestraDevSISA)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("update sisa_evento set \"ideventomuestra\" = :idEventoMuestraDevSISA where \"protocolo\" = '" + protocolo + "' ;", cnn);
                cnn.Open();
                cmd.Parameters.Add(new NpgsqlParameter("idEventoMuestraDevSISA", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters[0].Value = idEventoMuestraDevSISA;
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

            catch (Exception ex)
            {
                ELog.save(this, "Error: " + ex.Message);
            }
        }

        internal void ActualizarEventoPendienteTotal(string protocolo, string v)
        {
            try
            {
                
                NpgsqlCommand cmd = new NpgsqlCommand("update sisa_evento set \"idevento\" = :idevento, \"ideventomuestra\" = :ideventomuestra, \"resultado\" = :res where \"protocolo\" = '" + protocolo + "' ;", cnn);
                cnn.Open();
                cmd.Parameters.Add(new NpgsqlParameter("idevento", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters.Add(new NpgsqlParameter("ideventomuestra", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters.Add(new NpgsqlParameter("res", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters[0].Value = v;
                cmd.Parameters[1].Value = v;
                cmd.Parameters[2].Value = v;
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

            catch (Exception ex)
            {
                ELog.save(this, "Error: " + ex.Message);
            }
        }

        public void ActualizarResultadoPendiente(string protocolo, string res)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("update sisa_evento set \"resultado\" = :res where \"protocolo\" = '" + protocolo + "' ;", cnn);
                cnn.Open();
                cmd.Parameters.Add(new NpgsqlParameter("res", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters[0].Value = res;
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

            catch (Exception ex)
            {
                ELog.save(this, "Error: " + ex.Message);
            }
        }

        public Protocolo GetProtocolo(string numero)
        {
            string NumeroProtocolo = numero.ToUpper();
            var cmd = @"select * from public.sisa_protocolo('" + NumeroProtocolo + @"')";
            try
            {
                List<Object[]> result = Query(cmd);
                if (result.Count > 0)
                {
                    Protocolo p = new Protocolo()
                    {
                        protocolo = result[0][0].ToString(),
                        tipo_documento = result[0][1].ToString(),
                        numero_documento = result[0][2].ToString(),
                        sexo = result[0][3].ToString(),
                        fecha_nacimiento = DateTime.Parse(result[0][4].ToString()),
                        fecha_pedido = DateTime.Parse(result[0][5].ToString()),
                        fecha_toma_muestra = DateTime.Parse(result[0][6].ToString()),
                        establecimiento_toma_muestra = result[0][7].ToString(),
                        resultado = result[0][8].ToString(),
                        fecha_resultado = DateTime.Parse(result[0][9].ToString())
                    };
                    return p;
                }
            }
            catch (Exception ex)
            {
                ELog.save(this, "Error: " + ex.Message);
            }
            return null;
        }
    }
}
