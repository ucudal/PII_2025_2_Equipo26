using Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    /// <summary>
    /// Administra la colección de objetos <see cref="Cliente"/>.
    /// Hereda la lógica común de <see cref="RepoBase{T}"/>.
    /// </summary>
    public class RepoClientes : RepoBase<Cliente>, IRepoClientes
    {
        // --- Campos Privados ---
        // 'private List<Cliente> _clientes' HA SIDO ELIMINADO
        // 'private int _nextId' HA SIDO ELIMINADO
        // (Ambos son heredados como 'protected _items' y 'protected _nextId')

        // --- Métodos Públicos (Operaciones CRUD) ---

        /// <summary>
        /// Crea y agrega un nuevo cliente a la lista (Operación Create).
        /// Implementa el patrón Creator (GRASP): RepoClientes tiene la información necesaria
        /// para instanciar objetos Cliente y agregarlos a su colección.
        /// </summary>
        public void Agregar(string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
        {
            if (nombre == null || nombre == "") throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            if (apellido == null || apellido == "") throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            if (telefono == null || telefono == "") throw new ArgumentException("El teléfono no puede ser nulo o vacío.", nameof(telefono));
            if (correo == null || correo == "") throw new ArgumentException("El correo no puede ser nulo o vacío.", nameof(correo));
            if (genero == null || genero == "") throw new ArgumentException("El género no puede ser nulo o vacío.", nameof(genero));

            var nuevoCliente = new Cliente(
                nombre, 
                apellido, 
                telefono, 
                correo,
                genero, 
                fechaNacimiento 
            );
            
            // --- CORRECCIÓN ---
            // En lugar de 'this._items.Add(nuevoCliente)', llamamos al método 'Agregar'
            // de la clase base, que se encarga de asignar el ID y añadirlo a la lista.
            base.Agregar(nuevoCliente);
        }

        // --- 'Buscar(int id)' HA SIDO ELIMINADO (Heredado) ---

        /// <summary>
        /// Actualiza los datos de un cliente existente (Operación Update).
        /// </summary>
        public void Modificar(int id, string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
        {
            if (nombre == null || nombre == "") throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            if (apellido == null || apellido == "") throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            if (telefono == null || telefono == "") throw new ArgumentException("El teléfono no puede ser nulo o vacío.", nameof(telefono));
            if (correo == null || correo == "") throw new ArgumentException("El correo no puede ser nulo o vacío.", nameof(correo));
            if (genero == null || genero == "") throw new ArgumentException("El género no puede ser nulo o vacío.", nameof(genero));

            var cliente = this.Buscar(id); // Llama a Buscar() heredado
            
            if (cliente != null)
            {
                cliente.Nombre = nombre;
                cliente.Apellido = apellido;
                cliente.Telefono = telefono;
                cliente.Correo = correo;
                cliente.Genero = genero; 
                cliente.FechaNacimiento = fechaNacimiento; 
            }
        }
        
        /// <summary>
        /// Busca clientes que coincidan con un término (Método específico de Cliente).
        /// </summary>
        public List<Cliente> BuscarPorTermino(string termino)
        {
            var resultados = new List<Cliente>();

            foreach (var cliente in this._items)
            {
                if (cliente.Coincide(termino))
                {
                    resultados.Add(cliente);
                }
            }
            return resultados;
        }
    
    }
}