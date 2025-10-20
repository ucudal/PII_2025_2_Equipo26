using Library;

using System;

public class Cotizacion : Interaccion
{
    public double Monto { get; private set; }
    public string Detalle { get; private set; }
    
    public Cotizacion(string tema, double monto, string detalle) 
        : base(DateTime.Now, tema)
    {
        this.Monto = monto;
        this.Detalle = detalle;
    }
}