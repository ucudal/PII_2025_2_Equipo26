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

    public string Genero { get; set; }
    public DateTime FechaNacimiento { get; set; }

    public List<Interaccion> Interacciones { get; private set; }
    public List<Etiqueta> Etiquetas { get; private set; }
    public List<Venta> Ventas { get; private set; }

    // Constructor
    public Cliente(int id, string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Correo = correo;
        Genero = genero;
        FechaNacimiento = fechaNacimiento;
        
        Interacciones = new List<Interaccion>();
        Etiquetas = new List<Etiqueta>();
        Ventas = new List<Venta>();
    }
    
    public void AgregarVenta(Venta venta)
    {
        this.Ventas.Add(venta);
    }
}
