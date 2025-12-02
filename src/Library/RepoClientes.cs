using Library;
using System;
using System.Collections.Generic;

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
        public void Agregar(string nombre, string apellido, string telefono, string correo, string generoTexto, DateTime fechaNacimiento)
        {
            if (nombre == null || nombre == "") throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            if (apellido == null || apellido == "") throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            if (telefono == null || telefono == "") throw new ArgumentException("El teléfono no puede ser nulo o vacío.", nameof(telefono));
            if (correo == null || correo == "") throw new ArgumentException("El correo no puede ser nulo o vacío.", nameof(correo));
            if (generoTexto == null || generoTexto == "") throw new ArgumentException("El género no puede ser nulo o vacío.", nameof(generoTexto));

            var nuevoCliente = new Cliente(
                nombre, 
                apellido, 
                telefono, 
                correo,
                generoTexto, 
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
        public void Modificar(int id, string campo, string valor)
        {
            Cliente cliente = this.Buscar(id);
            
            if (cliente == null)
            {
                throw new KeyNotFoundException("No se encontró el cliente con ID " + id);
            }

            // 2. Normalizamos el campo a minúsculas para comparar fácil
            string campoNormalizado = campo.ToLower();

            // 3. Switch clásico
            switch (campoNormalizado)
            {
                case "nombre":
                if (valor == null || valor == "")
                 {
            throw new ArgumentException("El nombre no puede ser nulo.");
                }
                    cliente.Nombre = valor;
                    break;

                case "apellido":
                    cliente.Apellido = valor;
                    break;

                case "telefono":
                case "teléfono":
                    cliente.Telefono = valor;
                    break;

                case "correo":
                case "email":
                    cliente.Correo = valor;
                    break;

                case "genero":
                case "género":
                    try
                    {
                        // SIN OUT: Usamos Enum.Parse. Si falla, va al catch.
                        // El 'true' al final es para ignorar mayúsculas/minúsculas.
                        cliente.Genero = (GeneroCliente)Enum.Parse(typeof(GeneroCliente), valor, true);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("El género '" + valor + "' no es válido.");
                    }
                    break;

                case "fecha":
                case "nacimiento":
                    try
                    {
                        // SIN OUT: Usamos DateTime.Parse.
                        cliente.FechaNacimiento = DateTime.Parse(valor);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("La fecha '" + valor + "' no es válida. Use dd/mm/aaaa.");
                    }
                    break;

                default:
                    throw new ArgumentException("El campo '" + campo + "' no existe o no se puede editar.");
            }
        }
        /// <summary>
        /// Actualiza solo el género y la fecha de nacimiento de un cliente.
        /// Adhiere al SRP (Single Responsibility Principle) al tener un foco de cambio específico.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <param name="generoTexto">El género en formato string (ej: "Masculino").</param>
        /// <param name="fechaNacimiento">La fecha de nacimiento ya parseada.</param>
        public void ActualizarDatosAdicionales(int id, string generoTexto, DateTime fechaNacimiento)
        {
            if (generoTexto == null || generoTexto == "") throw new ArgumentException("El género no puede ser nulo o vacío.", nameof(generoTexto));

            // CONVERSIÓN DE TIPO (Programación Contra Especificaciones)
            GeneroCliente generoEnum;
            if (!Enum.TryParse(generoTexto, true, out generoEnum))
            {
                throw new ArgumentException($"El valor '{generoTexto}' no es un género válido.", nameof(generoTexto));
            }

            var cliente = this.Buscar(id); // Buscar() es heredado del genérico RepoBase<T>
            
            if (cliente != null)
            {
                // El objeto Cliente (Expert) es quien recibe los mensajes de actualización.
                cliente.Genero = generoEnum;
                cliente.FechaNacimiento = fechaNacimiento;
            }
            else
            {
                throw new KeyNotFoundException($"No se encontró el cliente con ID {id} para actualizar.");
            }
        }

    
    }
}