using Library;
using System;
using System.Collections.Generic;

/// <summary>
/// Representa un Usuario del sistema (ej. Vendedor o Administrador).
/// Implementa la interfaz IEntidad para ser gestionado por un Repositorio.
/// </summary>
public class Usuario : IEntidad
{
    // --- CORRECCIÓN ---
    // Se cambia 'private set' por 'set' para cumplir
    // con la interfaz IEntidad { get; set; }
    public int Id { get; set; }

    /// <summary>
    /// Nombre de login del usuario.
    /// </summary>
    public string NombreUsuario { get; private set; }

    /// <summary>
    /// Rol del usuario (Vendedor o Administrador).
    /// </summary>
    public Rol Rol { get; private set; }

    /// <summary>
    /// Estado actual del usuario (Activo o Suspendido).
    /// </summary>
    public Estado Estado { get; private set; }

    /// <summary>
    /// Constructor de la clase Usuario.
    /// </summary>
    /// <param name="nombreUsuario">Nombre de login.</param>
    /// <param name="rol">Rol del usuario.</param>
    public Usuario(string nombreUsuario, Rol rol)
    {
        this.NombreUsuario = nombreUsuario;
        this.Rol = rol;
        this.Estado = Estado.Activo; // Los usuarios se crean Activos por defecto
    }

    /// <summary>
    /// Cambia el estado del usuario a Suspendido.
    /// </summary>
    public void Suspender()
    {
        this.Estado = Estado.Suspendido;
    }

    /// <summary>
    /// Cambia el estado del usuario a Activo.
    /// </summary>
    public void Activar()
    {
        this.Estado = Estado.Activo;
    }
}