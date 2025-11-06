namespace Library
{
    /// <summary>
    /// Interfaz específica para el Repositorio de Clientes.
    /// Hereda de IRepositorio<Cliente>
    /// Esencial para la Inversión de Dependencias (DIP).
    /// </summary>
    public interface IRepoClientes : IRepositorio<Cliente>
    {
        // (Tu compañero/a "Nahuel" agregará la firma de 'CrearCliente' aquí)
    }
}