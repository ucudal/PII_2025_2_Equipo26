using Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    /// <summary>
    /// Implementa el patrón "Repositorio" (Repository).
    /// Maneja la lista de ventas generales (no atadas a un cliente).
    /// Hereda la lógica común de <see cref="RepoBase{T}"/>.
    /// </summary>
    public class RepoVentas : RepoBase<Venta>, IRepoVentas
    {
        // --- Campos Privados ---
        // 'private List<Venta> _ventas' HA SIDO ELIMINADO
        // 'private int _nextId' HA SIDO ELIMINADO
        // (Ambos son heredados como 'protected _items' y 'protected _nextId')

        // --- Métodos Públicos ---

        /// <summary>
        /// Agrega una nueva venta a la lista (Operación Create).
        /// </summary>
        /// <param name="producto">Descripción del producto o servicio vendido.</param>
        /// <param name="importe">El monto o valor total de la venta.</param>
        /// <param name="fecha">La fecha y hora en que se registró la venta.</param>
        /// <returns>La <see cref="Venta"/> recién creada (con su ID asignado).</returns>
        public Venta Agregar(string producto, float importe, DateTime fecha)
        {
            var nuevaVenta = new Venta(this._nextId++, producto, importe, fecha);
            this._items.Add(nuevaVenta); // Usa la lista _items heredada
            return nuevaVenta;
        }

    }
}
