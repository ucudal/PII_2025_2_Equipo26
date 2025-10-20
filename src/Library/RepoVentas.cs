using Library;

using System;
using System.Collections.Generic;

public class RepoVentas
{
    private List<Venta> _ventas = new List<Venta>();
    private int _nextId = 1;

    public Venta Agregar(string producto, float importe, DateTime fecha)
    {
        var nuevaVenta = new Venta(_nextId++, producto, importe, fecha);
        _ventas.Add(nuevaVenta);
        return nuevaVenta;
    }

    public List<Venta> ObtenerTodas()
    {
        return _ventas;
    }
}