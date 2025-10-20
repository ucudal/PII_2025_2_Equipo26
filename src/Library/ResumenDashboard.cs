namespace Library;

using System.Collections.Generic;

public class ResumenDashboard
{
    public int TotalClientes { get; set; }
    public List<Interaccion> InteraccionesRecientes { get; set; }
    public List<Reunion> ReunionesProximas { get; set; }
}