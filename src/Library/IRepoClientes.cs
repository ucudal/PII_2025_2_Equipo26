using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define el contrato para el Repositorio de Clientes.
    /// Hereda la funcionalidad base de IRepositorio.
    /// </summary>
    public interface IRepoClientes : IRepositorio<Cliente>
    {
        /// <summary>
        /// Crea y agrega un nuevo cliente.
        /// </summary>
        void Agregar(string nombre, string apellido, string telefono, string correo,
            string genero, DateTime fechaNacimiento);

        /// <summary>
        /// Modifica un cliente existente.
        /// </summary>
        void Modificar(int id, string nombre, string apellido, string telefono, 
            string correo, string genero, DateTime fechaNacimiento);

        /// <summary>
        /// Busca clientes por un término de texto.
        /// </summary>
        List<Cliente> BuscarPorTermino(string termino);

        // HEMOS ELIMINADO: Buscar(int), ObtenerTodos(), Eliminar(int)
    }
}