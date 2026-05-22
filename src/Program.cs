using System;
using System.IO;

class Program
{
    // ==============================
    // VARIABLES GLOBALES
    // ==============================

    // Arreglo de cursos (nodos)
    static string[] cursos = new string[50];

    // Matriz de adyacencia (conflictos)
    static int[,] matriz = new int[50, 50];

    // Arreglo de colores (bloques horarios)
    static int[] colores = new int[50];

    // Cantidad actual de cursos
    static int cantidadCursos = 0;

    // Bandera para saber si los horarios ya fueron generados
    static bool horariosGenerados = false;

    // Bloques horarios disponibles
    static string[] bloques =
    {
        "7:00 AM",
        "9:00 AM",
        "11:00 AM",
        "1:00 PM",
        "3:00 PM",
        "5:00 PM",
        "7:00 PM",
        "9:00 PM"
    };

    // ==============================
    // MAIN
    // ==============================

    static void Main(string[] args)
    {
        int opcion;

        // Inicializar colores en -1
        for (int i = 0; i < 50; i++)
        {
            colores[i] = -1;
        }

        do
        {
            Console.WriteLine("\n====================================");
            Console.WriteLine(" SISTEMA DE HORARIOS ACADÉMICOS ");
            Console.WriteLine("====================================");
            Console.WriteLine("1.  Cargar cursos desde archivo");
            Console.WriteLine("2.  Agregar curso");
            Console.WriteLine("3.  Eliminar curso");
            Console.WriteLine("4.  Mostrar cursos");
            Console.WriteLine("5.  Agregar conflicto");
            Console.WriteLine("6.  Eliminar conflicto");
            Console.WriteLine("7.  Mostrar matriz de adyacencia");
            Console.WriteLine("8.  Generar horarios (Greedy)");
            Console.WriteLine("9.  Validar horarios");
            Console.WriteLine("10. Mostrar matriz horaria");
            Console.WriteLine("11. Salir");

            Console.Write("\nSeleccione una opción: ");

            // CORRECCIÓN 1: Validar entrada no numérica
            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
                Console.WriteLine("\nEntrada inválida. Ingrese un número.");
                continue;
            }

            switch (opcion)
            {
                case 1:
                    Console.Write("\nIngrese el nombre del archivo: ");
                    string archivo = Console.ReadLine();
                    CargarArchivo(archivo);
                    break;

                case 2:
                    Console.Write("\nIngrese el nombre del curso: ");
                    string nombreCurso = Console.ReadLine();
                    AgregarCurso(nombreCurso);
                    break;

                case 3:
                    Console.Write("\nIngrese el curso a eliminar: ");
                    string cursoEliminar = Console.ReadLine();
                    EliminarCurso(cursoEliminar);
                    break;

                case 4:
                    MostrarCursos();
                    break;

                case 5:
                    Console.Write("\nIngrese el primer curso: ");
                    string curso1 = Console.ReadLine();
                    Console.Write("Ingrese el segundo curso: ");
                    string curso2 = Console.ReadLine();
                    AgregarConflicto(curso1, curso2);
                    break;

                case 6:
                    Console.Write("\nIngrese el primer curso: ");
                    string cursoA = Console.ReadLine();
                    Console.Write("Ingrese el segundo curso: ");
                    string cursoB = Console.ReadLine();
                    EliminarConflicto(cursoA, cursoB);
                    break;

                case 7:
                    MostrarMatriz();
                    break;

                case 8:
                    GenerarHorarios();
                    break;

                case 9:
                    ValidarHorarios();
                    break;

                case 10:
                    MostrarMatrizHoraria();
                    break;

                case 11:
                    Console.WriteLine("\nSaliendo del sistema...");
                    break;

                default:
                    Console.WriteLine("\nOpción inválida.");
                    break;
            }

        } while (opcion != 11);
    }

    // ==============================
    // BUSCAR CURSO
    // ==============================

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

    // ==============================
    // AGREGAR CURSO
    // ==============================

    static void AgregarCurso(string nombre)
    {
        if (cantidadCursos >= 50)
        {
            Console.WriteLine("\nNo se pueden agregar más cursos (límite: 50).");
            return;
        }

        if (BuscarCurso(nombre) != -1)
        {
            Console.WriteLine("\nEl curso ya existe.");
            return;
        }

        cursos[cantidadCursos] = nombre;
        colores[cantidadCursos] = -1;
        cantidadCursos++;

        // Al agregar un curso, los horarios ya no son válidos
        horariosGenerados = false;

        Console.WriteLine("\nCurso agregado correctamente.");
    }

    // ==============================
    // ELIMINAR CURSO
    // ==============================

    static void EliminarCurso(string nombre)
    {
        int pos = BuscarCurso(nombre);

        if (pos == -1)
        {
            Console.WriteLine("\nEl curso no existe.");
            return;
        }

        // Mover cursos y colores
        for (int i = pos; i < cantidadCursos - 1; i++)
        {
            cursos[i] = cursos[i + 1];
            colores[i] = colores[i + 1];
        }

        // Mover filas de la matriz
        for (int i = pos; i < cantidadCursos - 1; i++)
        {
            for (int j = 0; j < cantidadCursos; j++)
            {
                matriz[i, j] = matriz[i + 1, j];
            }
        }

        // CORRECCIÓN 2: Mover columnas con límite correcto (cantidadCursos - 1)
        for (int j = pos; j < cantidadCursos - 1; j++)
        {
            for (int i = 0; i < cantidadCursos - 1; i++)
            {
                matriz[i, j] = matriz[i, j + 1];
            }
        }

        // Limpiar última fila y columna que quedó sucia
        for (int k = 0; k < cantidadCursos; k++)
        {
            matriz[cantidadCursos - 1, k] = 0;
            matriz[k, cantidadCursos - 1] = 0;
        }

        cantidadCursos--;

        // Al eliminar un curso, los horarios ya no son válidos
        horariosGenerados = false;

        Console.WriteLine("\nCurso eliminado correctamente.");
    }

    // ==============================
    // MOSTRAR CURSOS
    // ==============================

    static void MostrarCursos()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("\nNo hay cursos registrados.");
            return;
        }

        Console.WriteLine("\n===== LISTA DE CURSOS =====");

        for (int i = 0; i < cantidadCursos; i++)
        {
            Console.WriteLine((i + 1) + ". " + cursos[i]);
        }
    }

    // ==============================
    // AGREGAR CONFLICTO
    // ==============================

    static void AgregarConflicto(string curso1, string curso2)
    {
        int i = BuscarCurso(curso1);
        int j = BuscarCurso(curso2);

        // Validar existencia
        if (i == -1 || j == -1)
        {
            Console.WriteLine("\nUno o ambos cursos no existen.");
            return;
        }

        // Validar mismo curso
        if (i == j)
        {
            Console.WriteLine("\nUn curso no puede tener conflicto consigo mismo.");
            return;
        }

        // Validar conflicto repetido
        if (matriz[i, j] == 1)
        {
            Console.WriteLine("\nEse conflicto ya existe.");
            return;
        }

        // Grafo NO dirigido
        matriz[i, j] = 1;
        matriz[j, i] = 1;

        // Al agregar conflicto, los horarios ya no son válidos
        horariosGenerados = false;

        Console.WriteLine("\nConflicto agregado correctamente.");
    }

    // ==============================
    // ELIMINAR CONFLICTO
    // ==============================

    static void EliminarConflicto(string curso1, string curso2)
    {
        int i = BuscarCurso(curso1);
        int j = BuscarCurso(curso2);

        if (i == -1 || j == -1)
        {
            Console.WriteLine("\nUno o ambos cursos no existen.");
            return;
        }

        if (matriz[i, j] == 0)
        {
            Console.WriteLine("\nEse conflicto no existe.");
            return;
        }

        matriz[i, j] = 0;
        matriz[j, i] = 0;

        // Al eliminar conflicto, los horarios ya no son válidos
        horariosGenerados = false;

        Console.WriteLine("\nConflicto eliminado correctamente.");
    }

    // ==============================
    // MOSTRAR MATRIZ DE ADYACENCIA
    // ==============================

    static void MostrarMatriz()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("\nNo hay cursos registrados.");
            return;
        }

        Console.WriteLine("\n===== MATRIZ DE ADYACENCIA =====\n");

        Console.Write("".PadRight(15));

        // Encabezados
        for (int i = 0; i < cantidadCursos; i++)
        {
            Console.Write(cursos[i].PadRight(15));
        }

        Console.WriteLine();

        // Filas
        for (int i = 0; i < cantidadCursos; i++)
        {
            Console.Write(cursos[i].PadRight(15));

            for (int j = 0; j < cantidadCursos; j++)
            {
                Console.Write(matriz[i, j].ToString().PadRight(15));
            }

            Console.WriteLine();
        }
    }

    // ==============================
    // COLOREADO GREEDY
    // Asigna bloques horarios sin conflictos
    // ==============================

    static void GenerarHorarios()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("\nNo hay cursos registrados.");
            return;
        }

        // Reiniciar colores
        for (int i = 0; i < cantidadCursos; i++)
        {
            colores[i] = -1;
        }

        // Primer nodo recibe el primer bloque
        colores[0] = 0;

        // Arreglo de disponibilidad de bloques
        bool[] disponible = new bool[50];

        // Recorrer nodos restantes
        for (int i = 1; i < cantidadCursos; i++)
        {
            // Todos los bloques disponibles inicialmente
            for (int j = 0; j < 50; j++)
            {
                disponible[j] = true;
            }

            // Marcar bloques usados por vecinos con conflicto
            for (int j = 0; j < cantidadCursos; j++)
            {
                if (matriz[i, j] == 1 && colores[j] != -1)
                {
                    disponible[colores[j]] = false;
                }
            }

            // Buscar el primer bloque libre
            int color;
            for (color = 0; color < cantidadCursos; color++)
            {
                if (disponible[color])
                {
                    break;
                }
            }

            // Asignar bloque horario
            colores[i] = color;
        }

        // CORRECCIÓN 3: Verificar que los colores asignados tienen bloque horario disponible
        int maxColor = 0;
        for (int i = 0; i < cantidadCursos; i++)
        {
            if (colores[i] > maxColor)
            {
                maxColor = colores[i];
            }
        }

        if (maxColor >= bloques.Length)
        {
            Console.WriteLine("\nAdvertencia: Se necesitan " + (maxColor + 1) + " bloques horarios.");
            Console.WriteLine("El sistema solo tiene " + bloques.Length + " bloques disponibles.");
            Console.WriteLine("Considere reducir los conflictos o agregar más bloques.");
        }

        horariosGenerados = true;

        Console.WriteLine("\nHorarios generados correctamente.");
    }

    // ==============================
    // VALIDAR HORARIOS
    // Verifica que el horario sea libre de conflictos
    // ==============================

    static void ValidarHorarios()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("\nNo hay cursos registrados.");
            return;
        }

        // CORRECCIÓN 4: Verificar que los horarios fueron generados
        if (!horariosGenerados)
        {
            Console.WriteLine("\nPrimero debe generar los horarios (opción 8).");
            return;
        }

        bool valido = true;

        for (int i = 0; i < cantidadCursos; i++)
        {
            for (int j = i + 1; j < cantidadCursos; j++)
            {
                if (matriz[i, j] == 1 && colores[i] == colores[j])
                {
                    Console.WriteLine("\nConflicto encontrado entre: " + cursos[i] + " y " + cursos[j]);
                    valido = false;
                }
            }
        }

        if (valido)
        {
            Console.WriteLine("\nEl horario es válido y libre de conflictos.");
        }
    }

    // ==============================
    // MOSTRAR MATRIZ HORARIA FINAL
    // ==============================

    static void MostrarMatrizHoraria()
    {
        if (cantidadCursos == 0)
        {
            Console.WriteLine("\nNo hay cursos registrados.");
            return;
        }

        // CORRECCIÓN 5: Verificar que los horarios fueron generados
        if (!horariosGenerados)
        {
            Console.WriteLine("\nPrimero debe generar los horarios (opción 8).");
            return;
        }

        Console.WriteLine("\n===== MATRIZ HORARIA FINAL =====\n");
        Console.WriteLine("CURSO".PadRight(25) + "BLOQUE HORARIO");
        Console.WriteLine(new string('-', 45));

        for (int i = 0; i < cantidadCursos; i++)
        {
            string bloque;

            // CORRECCIÓN 6: Manejar más bloques de los disponibles en el arreglo
            if (colores[i] < bloques.Length)
            {
                bloque = bloques[colores[i]];
            }
            else
            {
                bloque = "Bloque extra " + (colores[i] + 1);
            }

            Console.WriteLine(cursos[i].PadRight(25) + bloque);
        }
    }

    // ==============================
    // CARGAR ARCHIVO
    // Formato esperado por línea: Curso1,Curso2
    // ==============================

    static void CargarArchivo(string nombreArchivo)
    {
        if (!File.Exists(nombreArchivo))
        {
            Console.WriteLine("\nEl archivo no existe.");
            return;
        }

        string[] lineas = File.ReadAllLines(nombreArchivo);
        int lineasProcesadas = 0;

        foreach (string linea in lineas)
        {
            // Ignorar líneas vacías
            if (string.IsNullOrWhiteSpace(linea))
            {
                continue;
            }

            string[] datos = linea.Split(',');

            if (datos.Length == 2)
            {
                string c1 = datos[0].Trim();
                string c2 = datos[1].Trim();

                // Agregar cursos si no existen
                if (BuscarCurso(c1) == -1)
                {
                    AgregarCurso(c1);
                }

                if (BuscarCurso(c2) == -1)
                {
                    AgregarCurso(c2);
                }

                // Agregar conflicto entre ellos
                AgregarConflicto(c1, c2);

                lineasProcesadas++;
            }
            else
            {
                Console.WriteLine("\nLínea con formato incorrecto ignorada: \"" + linea + "\"");
            }
        }

        Console.WriteLine("\nArchivo cargado correctamente. Líneas procesadas: " + lineasProcesadas);
    }
}