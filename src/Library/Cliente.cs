using Library;

using System;
using System.Collections.Generic;

public class Cliente
{
    public int Id { get; private set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string Genero { get; set; }
    public DateTime FechaNacimiento { get; set; }

    public List<Interaccion> Interacciones { get; set; } = new List<Interaccion>();
    public List<Etiqueta> Etiquetas { get; set; } = new List<Etiqueta>();
    
    public List<Venta> Ventas { get; private set; } = new List<Venta>();
    
    public Usuario VendedorAsignado { get; private set; }

    
    public Cliente(int id, string nombre, string apellido, string telefono, 
        string correo, string genero, DateTime fechaNacimiento)
    {
        this.Id = id;
        this.Nombre = nombre;
        this.Apellido = apellido;
        this.Telefono = telefono;
        this.Correo = correo;
        this.Genero = genero;
        this.FechaNacimiento = fechaNacimiento;
    }
    
 
    public void AsignarVendedor(Usuario nuevoVendedor)
    {
        if (nuevoVendedor != null && nuevoVendedor.Rol == RolUsuario.Vendedor)
        {
            this.VendedorAsignado = nuevoVendedor;
        }
    }
    
    public void AgregarVenta(Venta venta)
    {
        this.Ventas.Add(venta);
    }
}