using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define el contrato para el Repositorio de Ventas Generales.
    /// Hereda la funcionalidad base de IRepoBase.
    /// </summary>
    
    // 1. Ahora hereda de IRepoBase<Venta>
    //    (Aunque IRepoBase tiene 'Buscar' y 'Eliminar' y esta
    //     interfaz no los usa explícitamente, está bien.
    //     Una alternativa sería hacer una IRepoVentas : IRepoLectura<Venta>)
    //    Pero para mantenerlo simple, esto funciona.
    public interface IRepoVentas : IRepoBase<Venta>
    {
        /// <summary>
        /// Agrega una nueva venta general.
        /// </summary>
        Venta Agregar(string producto, float importe, DateTime fecha);
        
    }
}