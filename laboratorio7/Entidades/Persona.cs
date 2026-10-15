namespace Entidades
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool Eliminado { get; set; }
    }
}
