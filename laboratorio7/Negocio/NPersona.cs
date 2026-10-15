using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Datos;
using Entidades;

namespace Negocio
{
    public class NPersona
    {
        private readonly DPersona dPersona = new DPersona();

        public List<Persona> Listar()
        {
            return dPersona.Listar();
        }

        public List<Persona> Buscar(string? texto)
        {
            texto = texto?.Trim();
            if (string.IsNullOrEmpty(texto))
                return dPersona.Listar();

            return dPersona.Buscar(texto);
        }

        public int Registrar(Persona persona)
        {
            Validar(persona);

            try
            {
                return dPersona.Insertar(persona);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new ArgumentException($"Ya existe una persona registrada con el DNI {persona.DNI}.");
            }
        }

        public void Actualizar(Persona persona)
        {
            if (persona.IdPersona <= 0)
                throw new ArgumentException("Seleccione una persona para actualizar.");

            Validar(persona);

            try
            {
                if (!dPersona.Actualizar(persona))
                    throw new ArgumentException("La persona no existe o fue eliminada.");
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new ArgumentException($"Ya existe otra persona registrada con el DNI {persona.DNI}.");
            }
        }

        public void Eliminar(int idPersona)
        {
            if (idPersona <= 0)
                throw new ArgumentException("Seleccione una persona para eliminar.");

            if (!dPersona.Eliminar(idPersona))
                throw new ArgumentException("La persona no existe o ya fue eliminada.");
        }

        private static void Validar(Persona persona)
        {
            persona.Nombres = persona.Nombres?.Trim() ?? string.Empty;
            persona.Apellidos = persona.Apellidos?.Trim() ?? string.Empty;
            persona.DNI = persona.DNI?.Trim() ?? string.Empty;
            persona.Email = string.IsNullOrWhiteSpace(persona.Email) ? null : persona.Email.Trim();

            if (persona.Nombres.Length == 0)
                throw new ArgumentException("El campo Nombres es obligatorio.");

            if (persona.Apellidos.Length == 0)
                throw new ArgumentException("El campo Apellidos es obligatorio.");

            if (!Regex.IsMatch(persona.DNI, @"^\d{8}$"))
                throw new ArgumentException("El DNI debe tener exactamente 8 dígitos.");

            if (persona.Email != null && !Regex.IsMatch(persona.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El formato del Email no es válido.");
        }
    }
}
