using Library;
using System;
using System.Collections.Generic;

/// <summary>
/// ... (tus comentarios XML están bien) ...
/// </summary>
public class RepoClientes : Repositorio<Cliente>
{
    // APLICAR PATRÓN CREATOR (Feedback: "los repositorios deberían crear las instancias")

    // ANTES (Incorrecto - 8 argumentos):
    // public Cliente CrearCliente(int id, string nombre, string apellido, string email, string telefono, string correo, string genero, DateTime fechaNacimiento)

    // DESPUÉS (Correcto - 7 argumentos):
    public Cliente CrearCliente(string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
    {
        var nuevoCliente = new Cliente(nombre, apellido, telefono, correo, genero, fechaNacimiento);
        this.Agregar(nuevoCliente);
        return nuevoCliente;
    }
}