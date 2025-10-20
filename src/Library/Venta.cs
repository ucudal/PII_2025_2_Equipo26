using Library;

using System;

public class Venta
{
    public int Id { get; set; }
    public string Producto { get; set; }
    public float Importe { get; set; }
    public DateTime Fecha { get; set; }

    public Venta(int id, string producto, float importe, DateTime fecha)
    {
        this.Id = id;
        this.Producto = producto;
        this.Importe = importe;
        this.Fecha = fecha;
    }
}