using Library;
using System;
using System.Collections.Generic;

// Esta clase representa a un cliente en el sistema.
// Contiene todos los datos personales y el historial de interacciones.
public class Cliente
{
    // --- Propiedades del Cliente ---
    // (Datos que definen al cliente)

    // Identificador único del cliente (ej: cédula o ID interno).
    // El 'private set' significa que solo se puede poner un valor
    // cuando creamos el cliente (en el constructor).
    public int Id { get; private set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string Genero { get; set; }
    public DateTime FechaNacimiento { get; set; }
    
    // --- Relaciones y Listas ---
    // (Cosas asociadas al cliente)

    // Lista de todas las interacciones (llamadas, reuniones, etc.)
    // que hemos tenido con este cliente.
    public List<Interaccion> Interacciones { get; set; } = new List<Interaccion>();

    // Lista de etiquetas para clasificar al cliente (ej: "VIP", "Nuevo", "Interesado").
    public List<Etiqueta> Etiquetas { get; set; } = new List<Etiqueta>();
    
    // Historial de ventas cerradas con este cliente.
    public List<Venta> Ventas { get; private set; } = new List<Venta>();

    // El usuario (vendedor) que está a cargo de este cliente.
    public Usuario VendedorAsignado { get; private set; }

    // --- Constructor ---
    // Es la "receta" para crear un nuevo objeto Cliente.
    // Pide los datos básicos obligatorios.
    public Cliente(int id, string nombre, string apellido, string telefono, 
        string correo, string genero, DateTime fechaNacimiento)
    {
        // 'this.Id' se refiere a la propiedad de la clase (arriba)
        // 'id' se refiere al valor que nos pasaron al llamarlo.
        this.Id = id;
        this.Nombre = nombre;
        this.Apellido = apellido;
        this.Telefono = telefono;
        this.Correo = correo;
        this.Genero = genero;
        this.FechaNacimiento = fechaNacimiento;
    }
    
     // --- Métodos ---

    // Esta función permite cambiar el vendedor asignado al cliente.
    public void AsignarVendedor(Usuario nuevoVendedor)
    {
        // Se asegura que el nuevo vendedor exista (no sea null)
        // y que realmente tenga el rol de "Vendedor".
        if (nuevoVendedor != null && nuevoVendedor.Rol == RolUsuario.Vendedor)
        {
            this.VendedorAsignado = nuevoVendedor;
        }
    }

    // Esta función agrega una nueva venta al historial del cliente.
    public void AgregarVenta(Venta venta)
    {
        this.Ventas.Add(venta);
    }
}
