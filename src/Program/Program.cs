using Library;
using System;

public class Program
{
    private static Fachada fachada;

    public static void Main(string[] args)
    {
        Console.WriteLine("--- Iniciando Demo del CRM ---");

        // --- "Composition Root" ---
        var repoClientes = new RepoClientes();
        var repoEtiquetas = new RepoEtiquetas();
        var repoUsuarios = new RepoUsuarios();
        var repoVentas = new RepoVentas();

        // Se "Inyectan" las dependencias en el constructor de la Fachada.
        fachada = new Fachada(repoClientes, repoEtiquetas, repoUsuarios, repoVentas);
        // -----------------------------

        DemoGestionUsuarios();
        DemoClientesYAsignacion();
        DemoInteraccionesYVentas();
        DemoReportesYDashboard();

        Console.WriteLine("\n--- Fin de la Demo ---");
    }

    private static void DemoGestionUsuarios()
    {
        Console.WriteLine("\n--- 1. Demo Gestión de Usuarios ---");
        
        // --- LLAMADAS A 'CrearUsuario' ACTUALIZADAS ---
        fachada.CrearUsuario("admin_global", Rol.Administrador);
        fachada.CrearUsuario("vendedor_juan", Rol.Vendedor);
        fachada.CrearUsuario("vendedor_maria", Rol.Vendedor);
        Console.WriteLine("Usuarios 'admin_global', 'vendedor_juan' y 'vendedor_maria' creados.");

        int idMaria = 3; 
        fachada.SuspenderUsuario(idMaria);
        Console.WriteLine("Usuario 'vendedor_maria' (ID 3) suspendido.");

        var usuarios = fachada.VerTodosLosUsuarios();
        foreach (var u in usuarios)
        {
            Console.WriteLine($"  - [Usuario] ID: {u.Id}, Nombre: {u.NombreUsuario}, Rol: {u.Rol}, Estado: {u.Estado}");
        }
    }

    private static void DemoClientesYAsignacion()
    {
        Console.WriteLine("\n--- 2. Demo Clientes y Asignación de Vendedor ---");
        
        fachada.CrearCliente("Cliente A", "Gomez", "099123456", "clienteA@mail.com", "Masculino", new DateTime(1990, 5, 15));
        fachada.CrearCliente("Cliente B", "Perez", "098765432", "clienteB@mail.com", "Femenino", new DateTime(1985, 10, 20));
        Console.WriteLine("Clientes 'Cliente A' (ID 1) y 'Cliente B' (ID 2) creados.");

        int idVendedorJuan = 2; 
        int idVendedorMaria = 3; 
        int idClienteA = 1;
        int idClienteB = 2;

        Console.WriteLine($"Asignando Cliente A (ID {idClienteA}) a Vendedor Juan (ID {idVendedorJuan})...");
        fachada.AsignarClienteVendedor(idClienteA, idVendedorJuan);

        Console.WriteLine($"Intentando asignar Cliente B (ID {idClienteB}) a Vendedora Maria (ID {idVendedorMaria}, Suspendida)...");
        fachada.AsignarClienteVendedor(idClienteB, idVendedorMaria); 

        var clienteA = fachada.BuscarCliente(idClienteA);
        var clienteB = fachada.BuscarCliente(idClienteB);
        
        Console.WriteLine($"  -> Vendedor de Cliente A: {clienteA.VendedorAsignado?.NombreUsuario ?? "N/A"}"); 
        Console.WriteLine($"  -> Vendedor de Cliente B: {clienteB.VendedorAsignado?.NombreUsuario ?? "N/A"}"); 
    }

    private static void DemoInteraccionesYVentas()
    {
        Console.WriteLine("\n--- 3. Demo Interacciones y Ventas (Fusión) ---");
        int idClienteA = 1;

        fachada.RegistrarLlamada(idClienteA, DateTime.Now.AddDays(-5), "Consulta inicial", "Entrante");
        fachada.RegistrarCotizacion(idClienteA, "Cotización CRM", 2500, "Licencia Plus"); 
        fachada.RegistrarReunion(idClienteA, DateTime.Now.AddDays(3), "Demo Producto", "Oficina Cliente"); 
        Console.WriteLine("Interacciones (Llamada, Cotización, Reunión) registradas para Cliente A.");

        Console.WriteLine("Registrando ventas...");
        
        fachada.RegistrarVenta("Consultoría", 400.50f, DateTime.Now.AddDays(-2));
        
        fachada.RegistrarVenta(idClienteA, "Licencia CRM", 2500f);

        fachada.RegistrarVenta("Soporte", 150f, DateTime.Now.AddDays(-1));
        
        fachada.RegistrarVenta("Hardware Antiguo", 1000f, DateTime.Now.AddDays(-40));
        
        DateTime inicio = DateTime.Now.AddDays(-30);
        DateTime fin = DateTime.Now;
        float totalGlobal = fachada.CalcularTotalVentas(inicio, fin); 
        
        Console.WriteLine($"  -> Total de Ventas GLOBALES (últimos 30 días): ${totalGlobal:F2}"); 

        var clienteA = fachada.BuscarCliente(idClienteA);
        Console.WriteLine($"  -> Ventas registradas específicas para Cliente A: {clienteA.Ventas.Count}"); 
    }
    
    private static void DemoReportesYDashboard()
    {
        Console.WriteLine("\n--- 4. Demo Reportes y Dashboard (Fusión) ---");

        var resumen = fachada.ObtenerResumenDashboard(); 
        
        Console.WriteLine($"  [Dashboard: Clientes Totales] -> {resumen.TotalClientes}"); 
        
        Console.WriteLine("  [Dashboard: Interacciones Recientes (Top 5)]");
        foreach (var inter in resumen.InteraccionesRecientes)
        {
            Console.WriteLine($"    - {inter.Fecha.ToShortDateString()}: {inter.Tema}");
        }
        
        Console.WriteLine("  [Dashboard: Reuniones Próximas]");
        if (resumen.ReunionesProximas.Count > 0)
        {
            foreach (var reunion in resumen.ReunionesProximas)
            {
                Console.WriteLine($"    - {reunion.Fecha.ToShortDateString()}: {reunion.Tema} en {reunion.Lugar}");
            }
        }
        else
        {
            Console.WriteLine("    - No hay reuniones próximas.");
        }

        var inactivos = fachada.ObtenerClientesInactivos(30); 
        Console.WriteLine($"\n  [Reporte: Clientes Inactivos (30 días)]");
        foreach (var c in inactivos)
        {
            Console.WriteLine($"    - ID: {c.Id}, Nombre: {c.Nombre}"); 
        }
    }
}