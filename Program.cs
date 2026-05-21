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
            Console.WriteLine("\n===== SISTEMA DE HORARIOS ACADEMICOS =====");
            Console.WriteLine("1. Cargar cursos y conflictos desde archivo");
            Console.WriteLine("2. Agregar un curso");
            Console.WriteLine("3. Eliminar un curso");
            Console.WriteLine("4. Agregar un conflicto");
            Console.WriteLine("5. Eliminar un conflicto");
            Console.WriteLine("6. Asignar bloques horarios (Coloreado Greedy)");
            Console.WriteLine("7. Validar horario (Libre de conflictos)");
            Console.WriteLine("8. Mostrar la matriz horaria final");
            Console.WriteLine("9. Salir");
            Console.Write("Seleccione una operacion: ");
            
            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
            }

            switch (opcion)
            {
                case 1:
                    Console.Write("Ingrese la ruta del archivo (ej. datos.txt): ");
                    string ruta = Console.ReadLine();
                    CargarDesdeArchivo(ruta);
                    break;

                case 2:
                    Console.Write("Ingrese el nombre del curso: ");
                    string nombreAgregar = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nombreAgregar))
                        AgregarCurso(nombreAgregar);
                    break;

                case 3:
                    Console.Write("Ingrese el nombre del curso a eliminar: ");
                    string nombreEliminar = Console.ReadLine();
                    EliminarCurso(nombreEliminar);
                    break;

                case 4:
                    Console.Write("Ingrese el primer curso: ");
                    string c1Agregar = Console.ReadLine();
                    Console.Write("Ingrese el segundo curso: ");
                    string c2Agregar = Console.ReadLine();
                    AgregarConflicto(c1Agregar, c2Agregar);
                    break;

                case 5:
                    Console.Write("Ingrese el primer curso: ");
                    string c1Eliminar = Console.ReadLine();
                    Console.Write("Ingrese el segundo curso: ");
                    string c2Eliminar = Console.ReadLine();
                    EliminarConflicto(c1Eliminar, c2Eliminar);
                    break;

                case 6:
                    GenerarHorarios();
                    break;

                case 7:
                    ValidarHorario();
                    break;

                case 8:
                    MostrarMatrizHorariaFinal();
                    break;

                case 9:
                    Console.WriteLine("Saliendo del sistema...");
                    break;

                default:
                    Console.WriteLine("Opcion invalida.");
                    break;
            }

        } while (opcion != 9);
    }

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

    static void CargarDesdeArchivo(string ruta)
    {
        if (!File.Exists(ruta))
        {
            Console.WriteLine("El archivo no existe.");
            return;
        }

        string[] lineas = File.ReadAllLines(ruta);
        foreach (string linea in lineas)
        {
            if (string.IsNullOrWhiteSpace(linea)) continue;

            string[] partes = linea.Split(',');
            
            if (partes.Length == 1)
            {
                AgregarCurso(partes[0].Trim());
            }
            else if (partes.Length == 2)
            {
                AgregarConflicto(partes[0].Trim(), partes[1].Trim());
            }
        }
        Console.WriteLine("Datos cargados correctamente desde el archivo.");
    }

    static void AgregarCurso(string nombre)
    {
        if (BuscarCurso(nombre) != -1)
        {
            Console.WriteLine("El curso ya existe.");
            return;
        }

        if (cantidadCursos >= 50)
        {
            Console.WriteLine("Capacidad maxima alcanzada.");
            return;
        }

        cursos[cantidadCursos] = nombre;
        colores[cantidadCursos] = -1;
        cantidadCursos++;

        Console.WriteLine("Curso agregado correctamente.");
    }

    static void EliminarCurso(string nombre)
    {
        int index = BuscarCurso(nombre);

        if (index == -1)
        {
            Console.WriteLine("El curso no existe.");
            return;
        }

        for (int i = index; i < cantidadCursos - 1; i++)
        {
            cursos[i] = cursos[i + 1];
            colores[i] = colores[i + 1];
        }

        for (int i = index; i < cantidadCursos - 1; i++)
        {
            for (int j = 0; j < cantidadCursos; j++)
            {
                matriz[i, j] = matriz[i + 1, j];
            }
        }

        for (int j = index; j < cantidadCursos - 1; j++)
        {
            for (int i = 0; i < cantidadCursos; i++)
            {
                matriz[i, j] = matriz[i, j + 1];
            }
        }

        for (int i = 0; i < cantidadCursos; i++)
        {
            matriz[cantidadCursos - 1, i] = 0;
            matriz[i, cantidadCursos - 1] = 0;
        }

        cantidadCursos--;
        Console.WriteLine("Curso eliminado y memoria reorganizada correctamente.");
    }

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

    static void EliminarConflicto(string curso1, string curso2)
    {
        int i = BuscarCurso(curso1);
        int j = BuscarCurso(curso2);

        if (i == -1 || j == -1)
        {
            Console.WriteLine("Uno o ambos cursos no existen.");
            return;
        }

        matriz[i, j] = 0;
        matriz[j, i] = 0;

        Console.WriteLine("Conflicto eliminado correctamente.");
    }

    static void GenerarHorarios()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("No hay cursos para asignar horarios.");
            return;
        }

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

        Console.WriteLine("Horarios asignados correctamente usando Coloreado Greedy.");
    }

    static void ValidarHorario()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("No hay datos para validar.");
            return;
        }

        bool hayError = false;
        for (int i = 0; i < cantidadCursos; i++)
        {
            if (colores[i] == -1)
            {
                Console.WriteLine($"!ADVERTENCIA! {cursos[i]} no tiene horario asignado.");
                hayError = true;
                continue;
            }

            for (int j = i + 1; j < cantidadCursos; j++)
            {
                if (matriz[i, j] == 1 && colores[i] == colores[j])
                {
                    Console.WriteLine($"!ERROR! Conflicto detectado entre {cursos[i]} y {cursos[j]} (Ambos tienen el horario {colores[i]}).");
                    hayError = true;
                }
            }
        }

        if (!hayError)
        {
            Console.WriteLine("Validacion exitosa: El horario esta 100% libre de conflictos.");
        }
    }

    static void MostrarMatrizHorariaFinal()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("No hay cursos registrados.");
            return;
        }

        string[] horarios =
        {
            "Lunes 7:00 AM - 9:00 AM", "Lunes 9:00 AM - 11:00 AM",
            "Martes 7:00 AM - 9:00 AM", "Martes 9:00 AM - 11:00 AM",
            "Miercoles 7:00 AM - 9:00 AM", "Jueves 7:00 AM - 9:00 AM"
        };

        Console.WriteLine("\n===== MATRIZ HORARIA FINAL =====");

        int maxColor = -1;
        for (int i = 0; i < cantidadCursos; i++)
        {
            if (colores[i] > maxColor) maxColor = colores[i];
        }

        if (maxColor == -1)
        {
            Console.WriteLine("Aun no se han generado los horarios. Ejecute la opcion 6 primero.");
            return;
        }

        for (int c = 0; c <= maxColor; c++)
        {
            string bloque = c < horarios.Length ? horarios[c] : $"Horario Extra {c}";
            Console.WriteLine($"\n[{bloque}]");

            bool hayCursos = false;
            for (int i = 0; i < cantidadCursos; i++)
            {
                if (colores[i] == c)
                {
                    Console.WriteLine($"  - {cursos[i]}");
                    hayCursos = true;
                }
            }
            if (!hayCursos) Console.WriteLine("  (Sin asignaciones)");
        }
    }
}