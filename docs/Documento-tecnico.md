# Sistema de Horarios Académicos con Grafos

## Integrantes

- Anderson León Giraldo
- Mariana López Díaz

---

# 1. Descripción del problema y motivación

Este proyecto busca resolver el problema de asignación de horarios académicos evitando conflictos entre cursos.

Los conflictos pueden presentarse cuando dos cursos comparten el mismo profesor, el mismo grupo de estudiantes o el mismo salón, por lo que no pueden ser asignados al mismo bloque horario.

Para solucionar este problema se utilizó un grafo no dirigido y un algoritmo de coloreado greedy, permitiendo organizar los horarios de forma sencilla y eficiente.

La motivación principal del proyecto fue aplicar estructuras de datos vistas en clase, especialmente grafos y matrices de adyacencia, en una situación real relacionada con la organización académica.

---

# 2. Modelado del problema

## Nodos

Cada nodo del grafo representa un curso.

Ejemplos:

- Programación
- Física
- Matemáticas
- Inglés

---

## Aristas

Cada arista representa un conflicto entre dos cursos.

Por ejemplo:

- Programación ↔ Física
- Física ↔ Inglés

Esto significa que esos cursos no pueden compartir el mismo horario.

---

## Dirección del grafo

El grafo utilizado es no dirigido.

Esto se debe a que si un curso entra en conflicto con otro, el conflicto ocurre en ambos sentidos.

Ejemplo:

Si Programación tiene conflicto con Física, entonces Física también tiene conflicto con Programación.

---

## Pesos

El grafo no utiliza pesos, ya que únicamente se necesita representar si existe o no un conflicto entre cursos.

---

## Representación gráfica

Ejemplo de grafo:

```txt
Programación ---- Física
      |
      |
Matemáticas

Física ---- Inglés
```

---

# 3. Estructuras de datos utilizadas

## Matriz de adyacencia

Se utilizó una matriz de adyacencia para representar el grafo.

La matriz almacena valores de 0 y 1:

- 0 → No existe conflicto
- 1 → Existe conflicto

Ejemplo:

|               | Programación | Física | Matemáticas |
|---------------|-------------|---------|--------------|
| Programación  | 0           | 1       | 1            |
| Física        | 1           | 0       | 0            |
| Matemáticas   | 1           | 0       | 0            |

La matriz de adyacencia fue elegida porque permite verificar conflictos de manera rápida y es sencilla de implementar.

---

## Arreglo de cursos

Se utilizó un arreglo de cadenas para almacenar los nombres de los cursos.

```csharp
string[] cursos = new string[50];
```

---

## Arreglo de colores

Se utilizó un arreglo para almacenar los bloques horarios asignados a cada curso.

```csharp
int[] colores = new int[50];
```

Cada color representa un bloque horario diferente.

---

# 4. Operaciones implementadas

| Operación | Descripción | Complejidad |
| Agregar curso | Inserta un nuevo curso | O(1) |
| Buscar curso | Busca un curso en el arreglo | O(n) |
| Eliminar curso | Elimina un curso y reorganiza la matriz | O(n²) |
| Agregar conflicto | Agrega una arista entre dos cursos | O(1) |
| Eliminar conflicto | Elimina una arista existente | O(1) |
| Mostrar matriz | Recorre toda la matriz de adyacencia | O(n²) |
| Generar horarios | Aplica coloreado greedy | O(n²) |
| Validar horarios | Verifica conflictos entre horarios | O(n²) |

---

## Explicación del coloreado greedy

El algoritmo greedy asigna el primer bloque horario disponible a cada curso sin generar conflictos con sus vecinos.

Cada color representa un horario diferente.

El algoritmo no busca la solución perfecta global, sino una solución válida y rápida.

---

# 5. Casos de prueba

## Archivo de entrada

```txt
Programacion,Fisica
Programacion,Matematicas
Fisica,Ingles
```

---

## Resultado esperado

| Curso | Horario |
| Programación | 7:00 AM |
| Física | 9:00 AM |
| Matemáticas | 9:00 AM |
| Inglés | 7:00 AM |

---

## Validación

El sistema verifica que dos cursos con conflicto no compartan el mismo bloque horario.

---

# 6. Diagramas e imágenes

En esta sección se incluirán:

- Diagramas del grafo.
- Flujo del algoritmo greedy.
- Capturas del programa funcionando.
- Ejemplos de la matriz de adyacencia.

---

# 7. Instrucciones para ejecutar el código

## Requisitos

- Visual Studio Code
- .NET SDK instalado
- Extensión de C# para Visual Studio Code

---

## Pasos para ejecutar

1. Abrir la carpeta del proyecto en Visual Studio Code.

2. Abrir una terminal en la carpeta `src`.

3. Ejecutar el siguiente comando:

```bash
dotnet run
```

4. Para cargar un archivo de prueba utilizar la opción:

```txt
1
```

5. Ingresar la ruta del archivo:

```txt
../data/prueba1.txt
```

---

# 8. Limitaciones de la solución y posibles mejoras

## Limitaciones

- El sistema tiene un límite máximo de 50 cursos.
- La interfaz funciona únicamente por consola.
- El algoritmo greedy no garantiza usar el mínimo número posible de horarios.

---

## Posibles mejoras

- Implementar una interfaz gráfica.
- Permitir guardar horarios automáticamente.
- Utilizar listas de adyacencia.
- Implementar algoritmos más avanzados de optimización.
- Exportar horarios a archivos externos.

---