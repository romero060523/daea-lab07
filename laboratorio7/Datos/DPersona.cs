using System.Data;
using Microsoft.Data.SqlClient;
using Entidades;

namespace Datos
{
    public class DPersona
    {
        private readonly Conexion conexion = new Conexion();

        public List<Persona> Listar()
        {
            List<Persona> lista = new List<Persona>();

            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("sp_Persona_Listar", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(Mapear(dr));
            }

            return lista;
        }

        public List<Persona> Buscar(string texto)
        {
            List<Persona> lista = new List<Persona>();

            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("sp_Persona_Buscar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Texto", SqlDbType.NVarChar, 100).Value = texto;

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(Mapear(dr));
            }

            return lista;
        }

        public int Insertar(Persona persona)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("sp_Persona_Insertar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            AgregarParametros(cmd, persona);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Actualizar(Persona persona)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("sp_Persona_Actualizar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IdPersona", SqlDbType.Int).Value = persona.IdPersona;
            AgregarParametros(cmd, persona);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // Eliminación lógica: el SP solo marca Eliminado = 1
        public bool Eliminar(int idPersona)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            connection.Open();

            using SqlCommand cmd = new SqlCommand("sp_Persona_Eliminar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IdPersona", SqlDbType.Int).Value = idPersona;

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private static void AgregarParametros(SqlCommand cmd, Persona persona)
        {
            cmd.Parameters.Add("@Nombres", SqlDbType.NVarChar, 100).Value = persona.Nombres;
            cmd.Parameters.Add("@Apellidos", SqlDbType.NVarChar, 100).Value = persona.Apellidos;
            cmd.Parameters.Add("@DNI", SqlDbType.Char, 8).Value = persona.DNI;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = (object?)persona.Email ?? DBNull.Value;
        }

        private static Persona Mapear(SqlDataReader dr)
        {
            return new Persona
            {
                IdPersona = Convert.ToInt32(dr["IdPersona"]),
                Nombres = dr["Nombres"].ToString() ?? string.Empty,
                Apellidos = dr["Apellidos"].ToString() ?? string.Empty,
                DNI = dr["DNI"].ToString() ?? string.Empty,
                Email = dr["Email"] == DBNull.Value ? null : dr["Email"].ToString(),
                Eliminado = Convert.ToBoolean(dr["Eliminado"])
            };
        }
    }
}
