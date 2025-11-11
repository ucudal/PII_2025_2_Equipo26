using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Interfaz para el Repositorio de Clientes.
    /// Hereda los métodos comunes de IRepositorio.
    /// </summary>
    public interface IRepoClientes : IRepositorio<Cliente>
    {
        // --- MÉTODOS ESPECÍFICOS QUE DEBES AGREGAR ---

        /// <summary>
        /// Crea y agrega un nuevo cliente.
        /// </summary>
        void Agregar(string nombre, string apellido, string telefono, string correo,
                       string genero, DateTime fechaNacimiento);

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// </summary>
        void Modificar(int id, string nombre, string apellido, string telefono,
                         string correo, string genero, DateTime fechaNacimiento);
        
        /// <summary>
        /// Busca clientes que coincidan con un término.
        /// </summary>
        List<Cliente> BuscarPorTermino(string termino);
    }
}