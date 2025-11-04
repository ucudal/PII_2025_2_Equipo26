using System.Collections.Generic;

public interface IEntidad
{
    int Id { get; set; }
}

public class Repositorio<T> where T : class, IEntidad
{
    protected readonly List<T> _elementos = new List<T>();
    private int _nextId = 1;

    public T Agregar(T elemento)
    {
        if (elemento.Id == 0) 
        {
            elemento.Id = this._nextId;
            this._nextId++;
        }
        this._elementos.Add(elemento);
        return elemento;
    }

    public T Buscar(int id)
    {

        foreach (T elemento in this._elementos)
        {
            if (elemento.Id == id)
            {
                return elemento;
            }
        }

        return null; 
    }

    public void Eliminar(int id)
    {
        T elemento = this.Buscar(id);
        if (elemento != null)
        {
            this._elementos.Remove(elemento);
        }
    }

    public IEnumerable<T> ObtenerTodos()
    {
        return this._elementos.AsReadOnly();
    }
}