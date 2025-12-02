using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Interfaz para el Repositorio de Clientes.
    /// Hereda los métodos comunes de IRepoBase.
    /// </summary>
    public interface IRepoClientes : IRepoBase<Cliente>
    {
        // --- MÉTODOS ESPECÍFICOS QUE DEBES AGREGAR ---

        /// <summary>
        /// Crea y agrega un nuevo cliente.
        /// </summary>
        void Agregar(string nombre, string apellido, string telefono, string correo,
                       string generoTexto, DateTime fechaNacimiento);

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// </summary>
        void Modificar(int id, string campo, string valor);
        
        /// <summary>
        /// Especificación para actualizar solamente los datos adicionales del cliente.
        /// </summary>
        void ActualizarDatosAdicionales(int id, string generoTexto, DateTime fechaNacimiento);

    }
}