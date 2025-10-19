namespace Library;

using System;

public class Venta
{
    public int Id { get; private set; }
    public string Producto { get; set; }
    public float Importe { get; set; }
    public DateTime Fecha { get; set; }

    // Constructor
    public Venta(int id, string producto, float importe, DateTime fecha)
    {
        Id = id;
        Producto = producto;
        Importe = importe;
        Fecha = fecha;
    }
}