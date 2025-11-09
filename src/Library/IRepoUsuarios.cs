namespace Library
{
    /// <summary>
    /// Interfaz específica para el Repositorio de Usuarios.
    /// Hereda de IRepositorio<Usuario>
    /// Esencial para la Inversión de Dependencias (DIP).
    /// </summary>
    public interface IRepoUsuarios : IRepositorio<Usuario>
    {
        // (Tu compañero/a "Nahuel" agregará la firma de 'CrearUsuario' aquí)
    }
}