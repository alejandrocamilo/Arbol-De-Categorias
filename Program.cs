using System;
using System.Collections.Generic;

public class Producto
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }

    public Producto(string nombre, decimal precio)
    {
        Nombre = nombre;
        Precio = precio;
    }
}

public class Categoria
{
    public string Nombre { get; set; }
    private List<Producto> productos;
    public Dictionary<string, Categoria> subCategorias;

    public Categoria(string nombre)
    {
        Nombre = nombre;
        productos = new List<Producto>();
        subCategorias = new Dictionary<string, Categoria>();
    }

    public void AgregarProducto(Producto producto)
    {
        productos.Add(producto);
    }

    public void AgregarSubCategoria(string nombre, Categoria categoria)
    {
        subCategorias.Add(nombre, categoria);
    }

    public Producto BuscarProducto(string nombre)
    {
        foreach (var producto in productos)
        {
            if (producto.Nombre == nombre)
            {
                return producto;
            }
        }

        foreach (var subCategoria in subCategorias.Values)
        {
            var productoEnSubCategoria = subCategoria.BuscarProducto(nombre);
            if (productoEnSubCategoria != null)
            {
                return productoEnSubCategoria;
            }
        }

        return null;
    }
}

public class Catalogo
{
    private Categoria raiz;

    public Catalogo()
    {
        raiz = new Categoria("Root");
    }

    public void AgregarProducto(string categoria, Producto producto)
    {
        var categorias = categoria.Split('/');
        var currentNode = raiz;

        foreach (var cat in categorias)
        {
            if (!currentNode.subCategorias.ContainsKey(cat))
            {
                currentNode.subCategorias.Add(cat, new Categoria(cat));
            }
            currentNode = currentNode.subCategorias[cat];
        }

        currentNode.AgregarProducto(producto);
    }

    public Producto BuscarProducto(string nombre)
    {
        return raiz.BuscarProducto(nombre);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Catalogo catalogo = new Catalogo();

        while (true)
        {
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Agregar producto");
            Console.WriteLine("2. Buscar producto");
            Console.WriteLine("3. Salir");
            Console.WriteLine();
            Console.Write("Escriba la opcion: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.WriteLine("Ingrese el nombre del producto:");
                    string nombreProducto = Console.ReadLine();
                    Console.WriteLine("Ingrese el precio del producto:");
                    decimal precioProducto;
                    while (!decimal.TryParse(Console.ReadLine(), out precioProducto))
                    {
                        Console.WriteLine("Precio inválido. Intente nuevamente:");
                    }

                    Console.WriteLine("Ingrese la categoría del producto:");
                    string categoriaProducto = Console.ReadLine();

                    catalogo.AgregarProducto(categoriaProducto, new Producto(nombreProducto, precioProducto));
                    Console.WriteLine("Producto agregado con éxito.");
                    break;
                case "2":
                    Console.WriteLine("Ingrese el nombre del producto que desea buscar:");
                    string nombreBuscar = Console.ReadLine();
                    Producto productoBuscado = catalogo.BuscarProducto(nombreBuscar);
                    if (productoBuscado != null)
                    {
                        Console.WriteLine($"Producto encontrado: {productoBuscado.Nombre} - Precio: {productoBuscado.Precio}");
                    }
                    else
                    {
                        Console.WriteLine("Producto no encontrado.");
                    }
                    break;
                case "3":
                    Console.WriteLine("Saliendo del programa...");
                    return;
                default:
                    Console.WriteLine("Opción no válida. Intente nuevamente.");
                    break;
            }
        }
    }
}
