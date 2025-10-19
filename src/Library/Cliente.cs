namespace Library;

using System.Collections.Generic;

public class Cliente
{
    // Propiedades
    public int Id { get; private set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }

    public List<Interaccion> Interacciones { get; private set; }
    public List<Etiqueta> Etiquetas { get; private set; }
    public List<Venta> Ventas { get; private set; }

    // Constructor
    public Cliente(int id, string nombre, string apellido, string telefono, string correo)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Correo = correo;
        
        Interacciones = new List<Interaccion>();
        Etiquetas = new List<Etiqueta>();
        Ventas = new List<Venta>();
    }
}