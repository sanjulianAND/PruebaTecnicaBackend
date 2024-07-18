namespace PruebaTecnicaBackend.Models
{
    public class Usuario
    {
        public int Identificador { get; set; }
        public string NombreUsuario { get; set; }
        public string Pass { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
