using Microsoft.Data.SqlClient;

namespace Datos
{
    public class Conexion
    {
        private readonly string connectionString = @"Data Source=LAB1506-11\SQLEXPRESS01;Initial Catalog=BD_Laboratorio7;Integrated Security=True;TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }
    }
}
