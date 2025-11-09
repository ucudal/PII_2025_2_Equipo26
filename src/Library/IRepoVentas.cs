using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define el contrato para el Repositorio de Ventas Generales.
    /// Hereda la funcionalidad base de IRepositorio.
    /// </summary>
    
    // 1. Ahora hereda de IRepositorio<Venta>
    //    (Aunque IRepositorio tiene 'Buscar' y 'Eliminar' y esta
    //     interfaz no los usa explícitamente, está bien.
    //     Una alternativa sería hacer una IRepoVentas : IRepositorioLectura<Venta>)
    //    Pero para mantenerlo simple, esto funciona.
    public interface IRepoVentas : IRepositorio<Venta>
    {
        /// <summary>
        /// Agrega una nueva venta general.
        /// </summary>
        Venta Agregar(string producto, float importe, DateTime fecha);
        
    }
}