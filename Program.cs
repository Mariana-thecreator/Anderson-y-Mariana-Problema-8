using System;

class Program
{
    // Arreglo de cursos (nodos)
    static string[] cursos = new string[50];

    // Matriz de adyacencia (conflictos)
    static int[,] matriz = new int[50, 50];

    // Arreglo de colores (horarios)
    static int[] colores = new int[50];

    // Cantidad actual de cursos
    static int cantidadCursos = 0;

    static void Main(string[] args)
    {
        int opcion;

        do
        {
            Console.WriteLine("\n===== SISTEMA DE HORARIOS =====");
            Console.WriteLine("1. Agregar curso");
            Console.WriteLine("2. Mostrar cursos");
            Console.WriteLine("3. Agregar conflicto");
            Console.WriteLine("4. Mostrar matriz");
            Console.WriteLine("5. Generar horarios");
            Console.WriteLine("6. Mostrar horarios");
            Console.WriteLine("7. Salir");
            Console.Write("Seleccione una opcion: ");

            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Console.Write("Ingrese el nombre del curso: ");
                    string nombre = Console.ReadLine();
                    AgregarCurso(nombre);
                    break;

                case 2:
                    MostrarCursos();
                    break;

                case 3:
                    Console.Write("Ingrese el primer curso: ");
                    string c1 = Console.ReadLine();

                    Console.Write("Ingrese el segundo curso: ");
                    string c2 = Console.ReadLine();

                    AgregarConflicto(c1, c2);
                    break;

                case 4:
                    MostrarMatriz();
                    break;

                case 5:
                    GenerarHorarios();
                    break;

                case 6:
                    MostrarHorarios();
                    break;

                case 7:
                    Console.WriteLine("Saliendo del sistema...");
                    break;

                default:
                    Console.WriteLine("Opcion invalida.");
                    break;
            }

        } while (opcion != 7);
    }

    // Buscar curso
    static int BuscarCurso(string nombre)
    {
        for (int i = 0; i < cantidadCursos; i++)
        {
            if (cursos[i] == nombre)
            {
                return i;
            }
        }

        return -1;
    }

    // Agregar curso
    static void AgregarCurso(string nombre)
    {
        if (BuscarCurso(nombre) != -1)
        {
            Console.WriteLine("El curso ya existe.");
            return;
        }

        cursos[cantidadCursos] = nombre;

        colores[cantidadCursos] = -1;

        cantidadCursos++;

        Console.WriteLine("Curso agregado correctamente.");
    }

    // Mostrar cursos
    static void MostrarCursos()
    {
        Console.WriteLine("\nLISTA DE CURSOS:");

        for (int i = 0; i < cantidadCursos; i++)
        {
            Console.WriteLine((i + 1) + ". " + cursos[i]);
        }
    }

    // Agregar conflicto
    static void AgregarConflicto(string curso1, string curso2)
    {
        int i = BuscarCurso(curso1);
        int j = BuscarCurso(curso2);

        if (i == -1 || j == -1)
        {
            Console.WriteLine("Uno o ambos cursos no existen.");
            return;
        }

        matriz[i, j] = 1;
        matriz[j, i] = 1;

        Console.WriteLine("Conflicto agregado correctamente.");
    }

    // Mostrar matriz
    static void MostrarMatriz()
    {
        Console.WriteLine("\nMATRIZ DE ADYACENCIA:");

        Console.Write("      ");

        for (int i = 0; i < cantidadCursos; i++)
        {
            Console.Write(cursos[i] + " ");
        }

        Console.WriteLine();

        for (int i = 0; i < cantidadCursos; i++)
        {
            Console.Write(cursos[i] + " ");

            for (int j = 0; j < cantidadCursos; j++)
            {
                Console.Write("   " + matriz[i, j]);
            }

            Console.WriteLine();
        }
    }

    // Generar horarios con coloreado greedy
    static void GenerarHorarios()
    {
        colores[0] = 0;

        bool[] disponible = new bool[50];

        for (int i = 1; i < cantidadCursos; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                disponible[j] = true;
            }

            for (int j = 0; j < cantidadCursos; j++)
            {
                if (matriz[i, j] == 1 && colores[j] != -1)
                {
                    disponible[colores[j]] = false;
                }
            }

            int color;

            for (color = 0; color < cantidadCursos; color++)
            {
                if (disponible[color])
                {
                    break;
                }
            }

            colores[i] = color;
        }

        Console.WriteLine("Horarios generados correctamente.");
    }

    // Mostrar horarios
    static void MostrarHorarios()
    {
        string[] horarios =
        {
            "7:00 AM",
            "9:00 AM",
            "11:00 AM",
            "1:00 PM",
            "3:00 PM",
            "5:00 PM"
        };

        Console.WriteLine("\nHORARIOS ASIGNADOS:");

        for (int i = 0; i < cantidadCursos; i++)
        {
            Console.WriteLine(cursos[i] + " -> " + horarios[colores[i]]);
        }
    }
}