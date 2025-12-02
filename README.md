# Universidad Católica del Uruguay

### Programación II - Proyecto CRM (2025)

## 📋 Descripción del Proyecto

Este proyecto consiste en el diseño y desarrollo de un sistema de **Gestión de Relaciones con el Cliente (CRM)** implementado como un **Chatbot en Discord** 🤖.

El objetivo principal es facilitar la gestión de carteras de clientes, permitiendo a vendedores y administradores registrar interacciones, realizar seguimientos de ventas y obtener métricas clave, todo a través de una interfaz conversacional sencilla.

El sistema ha sido construido bajo estrictos estándares de **Programación Orientada a Objetos (POO)**, aplicando principios **SOLID** (especialmente SRP y OCP) y patrones de diseño como **Expert**, **Fachada (Facade)**, **Repositorio** y **Polimorfismo**.

---

## 🚀 Funcionalidades Principales

* **Gestión de Clientes:** Alta, baja, modificación y búsqueda avanzada de clientes.
* **Registro de Interacciones:** Historial unificado de llamadas, reuniones, correos y mensajes.
* **Seguimiento Comercial:** Registro de ventas y cotizaciones.
* **Organización:** Sistema de etiquetas personalizables para segmentar clientes.
* **Reportes y Métricas:** Dashboard con resumen de actividad, cálculo de ventas por período y detección de clientes inactivos.
* **Administración:** Gestión de usuarios del sistema (vendedores/admins) con roles y permisos.

---

## 🛠️ Tecnologías Utilizadas

* **Lenguaje:** C# (.NET 8.0)
* **Plataforma de Bot:** Discord.Net
* **Gestión de Versiones:** Git & GitHub
* **Gestión de Tareas:** Trello
* **Testing:** NUnit

---

## 🤖 Lista de Comandos del Bot

A continuación se detallan los comandos disponibles para interactuar con el CRM.

### 👥 Gestión de Clientes
| Comando | Descripción | Ejemplo de Uso |
| :--- | :--- | :--- |
| `!crear_cliente` | Crea un nuevo cliente en el sistema. | `!crear_cliente "Juan" "Perez" "099123456" "juan@mail.com"` |
| `!modificar_cliente` | Modifica los datos de un cliente existente. | `!modificar_cliente "1" "Juan Mod" "Perez" ...` |
| `!actualizar_datos` | Agrega género y fecha de nacimiento. | `!actualizar_datos "1" "Masculino" "15/05/1990"` |
| `!eliminar_cliente` | Elimina un cliente de la base de datos. | `!eliminar_cliente "1"` |
| `!buscar_cliente` | Busca clientes por nombre, teléfono o mail. | `!buscar_cliente "Perez"` |
| `!ver_clientes` | Muestra la lista de todos los clientes. | `!ver_clientes` |
| `!reasignar_cliente` | Asigna un cliente a otro vendedor. | `!reasignar_cliente "1" "5"` (ID Cliente, ID Vendedor) |

### 📞 Interacciones y Notas
| Comando | Descripción | Ejemplo de Uso |
| :--- | :--- | :--- |
| `!registrar_llamada` | Registra una llamada telefónica. | `!registrar_llamada "1" "10/11/2023" "Venta" "Entrante"` |
| `!registrar_reunion` | Registra una reunión presencial o virtual. | `!registrar_reunion "1" "12/11/2023" "Presupuesto" "Oficina"` |
| `!registrar_mensaje` | Registra un mensaje (WhatsApp/SMS). | `!registrar_mensaje "1" "12/11/2023" "Consulta" "Texto..."` |
| `!registrar_correo` | Registra un correo electrónico. | `!registrar_correo "1" "12/11/2023" "Factura" "Asunto..."` |
| `!agregar_nota` | Agrega una nota a una interacción pasada. | `!agregar_nota "1" "2" "Cliente muy interesado"` |
| `!ver_interacciones` | Muestra el historial de un cliente. | `!ver_interacciones "1"` |

### 🏷️ Etiquetas y Organización
| Comando | Descripción | Ejemplo de Uso |
| :--- | :--- | :--- |
| `!crear_etiqueta` | Crea un nuevo tipo de etiqueta. | `!crear_etiqueta "VIP"` |
| `!asignar_etiqueta` | Asigna una etiqueta a un cliente. | `!asignar_etiqueta "1" "VIP"` |

### 💰 Ventas y Reportes
| Comando | Descripción | Ejemplo de Uso |
| :--- | :--- | :--- |
| `!registrar_venta` | Registra una venta cerrada. | `!registrar_venta "1" "Laptop" "1500"` |
| `!registrar_cotizacion`| Registra una cotización enviada. | `!registrar_cotizacion "1" "Servicios IT" "500"` |
| `!reporte_ventas` | Calcula el total vendido en un rango de fechas.| `!reporte_ventas "01/01/2023" "31/12/2023"` |
| `!clientes_inactivos` | Muestra clientes sin actividad reciente. | `!clientes_inactivos "30"` (Días sin contacto) |
| `!dashboard` | Muestra un resumen general del CRM. | `!dashboard` |

### ⚙️ Administración de Usuarios
| Comando | Descripción | Ejemplo de Uso |
| :--- | :--- | :--- |
| `!crear_usuario` | Crea un nuevo usuario (Vendedor/Admin). | `!crear_usuario "vendedor1" "Vendedor"` |
| `!suspender_usuario` | Suspende el acceso a un usuario. | `!suspender_usuario "2"` |
| `!eliminar_usuario` | Elimina un usuario del sistema. | `!eliminar_usuario "2"` |
| `!ping` | Verifica si el bot está activo. | `!ping` |

---

## 📝 Notas del Equipo

### 🔗 Gestión del Proyecto
> **Trello del equipo:** [Ver Tablero en Trello](https://trello.com/invite/b/68d541045dec11ea36333a81/ATTI797df16772e564e85383eaf0f48f3f03D2B5D304/proyecto-programacion-f-naf)

### 🎯 Desafíos de la Entrega

1.  **Gestión del Proyecto y Control de Versiones:** Si bien la codificación en C# presentó sus propios desafíos técnicos, la mayor curva de aprendizaje y el aspecto más desgastante fue la coordinación del equipo a través de **Git** y la planificación detallada en **Trello**. Asegurarnos de que todos trabajáramos en ramas separadas, integráramos los cambios (merge) correctamente y mantuviéramos el Trello actualizado requirió más esfuerzo y comunicación de lo anticipado.
2.  **Aplicación Práctica de Principios:** Llevar los conceptos de **SRP** y **Expert** de la teoría a decisiones concretas en el código, como la creación de los Repositorios y `CRMPrinter`, fue un proceso iterativo que requirió varias refactorizaciones.

### 🧠 Aprendizajes Clave

* **Importancia de la Gestión:** Aprendimos que la codificación es solo una parte del desarrollo de software. La **planificación** (Trello) y la **colaboración** (Git) son cruciales para el éxito del equipo.
* **Diseño Consciente:** Nos dimos cuenta de que tomarnos el tiempo necesario para diseñar y codificar con calma, pensando en los principios, hizo que esa parte del proceso fuera menos estresante a largo plazo y resultó en un código más robusto.
* **Testing como Red de Seguridad:** Empezar a implementar los tests unitarios nos dio confianza y nos ayudó a validar nuestras decisiones de diseño.
* **Flujo de Trabajo con Git:** Practicar el ciclo de `checkout`, `fetch`, `merge`, `add`, `commit` y `push` en ramas personales nos enseñó un flujo de trabajo efectivo para equipos.
* **Modelado Visual:** Crear el diagrama UML nos ayudó a entender mejor la estructura general del proyecto y las relaciones entre clases antes de empezar a codificar a fondo.

### 📚 Recursos Valiosos

* **Documentación Oficial de .NET:** Para consultas específicas sobre C# y las bibliotecas.
* **Tutoriales de Git:** Diversos recursos online para entender mejor el flujo de trabajo con ramas y fusiones.
* **Ayuda de Trello:** Para optimizar el uso del tablero y sus funcionalidades.

---

> **Aclaración:** Se usaron dos cuentas de Git a nombre de Facundo debido al uso de distintos dispositivos durante el desarrollo, por ende, hay commits en el historial provenientes de ambas cuentas.