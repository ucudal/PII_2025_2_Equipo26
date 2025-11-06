namespace Library
{
    /// <summary>
    /// Interfaz específica para el Repositorio de Ventas.
    /// Hereda de IRepositorio<Venta>
    /// Esencial para la Inversión de Dependencias (DIP).
    /// </summary>
    public interface IRepoVentas : IRepositorio<Venta>
    {
        // (Tu compañero "Nahuel" agregará la firma de 'CrearVenta' aquí)
    }
}