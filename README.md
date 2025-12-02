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

### 📋 Tabla de Historias de Usuario y Comandos

| 📖 Historia de Usuario | 🛠️ Comandos Necesarios | 💻 Ejemplo de Ejecución (Copiar y Pegar) |
| :--- | :--- | :--- |
| Como usuario quiero crear un nuevo cliente con su información básica: nombre, apellido, teléfono y correo electrónico... | `!crear_cliente` | `!crear_cliente "Juan" "Perez" "099123456" "juan@mail.com"` |
| Como usuario quiero modificar la información de un cliente existente, para mantenerla actualizada. | `!crear_cliente`<br>`!modificar_cliente` | `!crear_cliente "Juan" "Perez" "099123456" "juan@mail.com"`<br>`!modificar_cliente 1 "Juan" "Lopez" "099888777" "nuevo@mail.com" "Masculino" "01/01/1990"` |
| Como usuario quiero eliminar un cliente, para mantener limpia la base de datos. | `!crear_cliente`<br>`!eliminar_cliente` | `!crear_cliente "Borrar" "Este" "099000000" "borrar@mail.com"`<br>`!eliminar_cliente 1` |
| Como usuario quiero buscar clientes por nombre, apellido, teléfono o correo electrónico... | `!crear_cliente`<br>`!buscar_cliente` | `!crear_cliente "Maria" "Busqueda" "098111222" "maria@mail.com"`<br>`!buscar_cliente "Maria"` |
| Como usuario quiero ver una lista de todos mis clientes, para tener una vista general de mi cartera. | `!crear_cliente`<br>`!ver_clientes` | `!crear_cliente "Cliente1" "Test" "099111111" "c1@mail.com"`<br>`!ver_clientes` |
| Como usuario quiero registrar llamadas enviadas o recibidas de clientes... | `!crear_cliente`<br>`!registrar_llamada` | `!crear_cliente "Ana" "Llamadas" "099222333" "ana@mail.com"`<br>`!registrar_llamada 1 "22/11/2023" "Consulta de precio" "Entrante"` |
| Como usuario quiero registrar reuniones con los clientes... | `!crear_cliente`<br>`!registrar_reunion` | `!crear_cliente "Carlos" "Reunion" "099333444" "carlos@mail.com"`<br>`!registrar_reunion 1 "25/12/2023" "Oficina Central" "Firma contrato"` |
| Como usuario quiero registrar mensajes enviados a o recibidos de los clientes... | `!crear_cliente`<br>`!registrar_mensaje` | `!crear_cliente "Sofia" "Mensaje" "099555666" "sofia@mail.com"`<br>`!registrar_mensaje 1 "23/11/2023" "Coordinar hora" "Hola, paso a las 5pm"` |
| Como usuario quiero registrar correos electrónicos enviados a o recibidos de los clientes... | `!crear_cliente`<br>`!registrar_correo` | `!crear_cliente "Pedro" "Correo" "099777888" "pedro@mail.com"`<br>`!registrar_correo 1 "23/11/2023" "Presupuesto PDF" "Adjunto lo solicitado"` |
| Como usuario quiero agregar notas o comentarios a las llamadas, reuniones, mensajes... | `!crear_cliente`<br>`!registrar_llamada`<br>`!ver_interacciones`<br>`!agregar_nota` | `!crear_cliente "Lucas" "Notas" "091000111" "lucas@mail.com"`<br>`!registrar_llamada 1 "24/11/2023" "Reclamo" "Entrante"`<br>`!ver_interacciones 1`<br>`!agregar_nota 1 "El cliente estaba molesto"` |
| Como usuario quiero registrar otros datos de los clientes como género y fecha de nacimiento... | `!crear_cliente`<br>`!registrar_datos_cliente` | `!crear_cliente "Lucia" "Datos" "092222333" "lucia@mail.com"`<br>`!registrar_datos_cliente 1 "Femenino" "15/05/1995"` |
| Como usuario quiero poder definir etiquetas para poder organizar y segmentar a mis clientes. | `!crear_etiqueta` | `!crear_etiqueta "VIP"` |
| Como usuario quiero poder agregar una etiqueta a un cliente... | `!crear_cliente`<br>`!crear_etiqueta`<br>`!asignar_etiqueta` | `!crear_cliente "Marcos" "Etiqueta" "093333444" "marcos@mail.com"`<br>`!crear_etiqueta "Deudor"`<br>`!asignar_etiqueta 1 "Deudor"` |
| Como usuario quiero poder registrar una venta a un cliente... | `!crear_cliente`<br>`!registrar_venta` | `!crear_cliente "Cliente" "Comprador" "094444555" "compra@mail.com"`<br>`!registrar_venta 1 "Laptop Gamer" 2500` |
| Como usuario quiero poder registrar que le envié una cotización a un cliente... | `!crear_cliente`<br>`!registrar_cotizacion` | `!crear_cliente "Prospecto" "Nuevo" "095555666" "pros@mail.com"`<br>`!registrar_cotizacion 1 "Servicio Mensual" 500` |
| Como usuario quiero ver todas las interacciones de un cliente... | `!crear_cliente`<br>`!registrar_llamada`<br>`!ver_interacciones` | `!crear_cliente "Historial" "Full" "096666777" "hist@mail.com"`<br>`!registrar_llamada 1 "20/11/2023" "Intro" "Saliente"`<br>`!ver_interacciones 1` |
| Cómo usuario quiero saber los clientes que hace cierto tiempo que no tengo ninguna interacción... | `!crear_cliente`<br>`!clientes_inactivos` | `!crear_cliente "Inactivo" "Test" "097777888" "inactivo@mail.com"`<br>`!clientes_inactivos 30` |
| Como usuario quiero saber los clientes que se pusieron en contacto conmigo y no les contesté... | `!crear_cliente`<br>`!registrar_mensaje`<br>`!ver_interacciones` | `!crear_cliente "Sin" "Respuesta" "098888999" "sinresp@mail.com"`<br>`!registrar_mensaje 1 "01/12/2023" "Ayuda" "Necesito soporte (Entrante)"`<br>`!ver_interacciones 1` |
| Como administrador quiero crear, suspender o eliminar usuarios... | `!admin crear_usuario`<br>`!admin suspender_usuario`<br>`!admin eliminar_usuario` | `!admin crear_usuario @UsuarioDiscord "Vendedor"`<br>`!admin suspender_usuario 1`<br>`!admin eliminar_usuario 1` |
| Como vendedor, quiero poder asignar un cliente a otro vendedor... | `!admin crear_usuario`<br>`!crear_cliente`<br>`!asignar_vendedor` | *(Requiere mencionar a otro usuario)*<br>`!admin crear_usuario @Compañero "Vendedor"`<br>`!crear_cliente "Cliente" "Transferible" "099999999" "transf@mail.com"`<br>`!asignar_vendedor 1 @Compañero` |
| Como usuario quiero saber el total de ventas de un periodo dado... | `!crear_cliente`<br>`!registrar_venta`<br>`!total_ventas` | `!crear_cliente "Comprador" "Total" "091111222" "total@mail.com"`<br>`!registrar_venta 1 "Item 1" 1000`<br>`!registrar_venta 1 "Item 2" 2000`<br>`!total_ventas "01/01/2020" "31/12/2025"` |
| Como usuario quiero ver un panel con clientes totales, interacciones recientes y reuniones próximas... | `!crear_cliente`<br>`!registrar_reunion`<br>`!dashboard` | `!crear_cliente "Demo" "Dashboard" "092222333" "dash@mail.com"`<br>`!registrar_reunion 1 "30/12/2025" "Zoom" "Cierre de año"`<br>`!dashboard` |

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