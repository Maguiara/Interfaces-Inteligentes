# Práctica Cardboard — Interfaces Inteligentes

## Experiencia de realidad virtual en un bosque

Esta práctica desarrolla una pequeña aplicación de realidad virtual para Android con Unity y Google Cardboard. El jugador explora un bosque de ambiente oscuro, se desplaza mediante la mirada y un toque en pantalla, recoge cristales situados sobre pilares y utiliza un pedestal para recuperarlos.

El trabajo parte de la escena `HelloCardboard` del SDK y adapta su configuración, su entorno y sus interacciones a las mecánicas del enunciado.

## Ejercicio 1. Configuración de Cardboard y generación del APK

**Enunciado:** Crear un proyecto con Cardboard, experimentar con su funcionamiento y generar un APK para Android.

**Archivos y configuración:** `Packages/manifest.json`, `Assets/XR`, `CardboardStartup.cs` y las plantillas de Gradle de `Assets/Plugins/Android`.

**Descripción de lo realizado:**

Se ha creado el proyecto con Unity 6.6 (`6000.6.3f1`) y Universal Render Pipeline (URP). Se ha instalado Google Cardboard XR Plugin desde su repositorio Git y se ha importado la muestra Hello Cardboard, versión 1.36.0.

La escena de trabajo se conserva en:

```text
Assets/Samples/Google Cardboard XR Plugin for Unity/1.36.0/Hello Cardboard/Scenes/HelloCardboard.unity
```

Para generar la aplicación se ha instalado Android Build Support, con Android SDK, NDK y OpenJDK. En XR Plug-in Management se ha asignado Cardboard a la plataforma Android y se ha activado `Initialize XR on Startup`.

La configuración utilizada incluye arquitectura ARM64, Android 8.0 como versión mínima (API 26), el nuevo Input System, orientación horizontal fija y OpenGLES3. El punto de entrada de Android es `Activity`, con `GameActivity` desactivado. El nivel de API objetivo queda seleccionado automáticamente por el SDK instalado.

También se han activado las plantillas personalizadas de Gradle y se han añadido las dependencias Android requeridas por Cardboard, entre ellas AppCompat, Google Play Services Vision, Material Components y Protobuf. En `gradleTemplate.properties` se ha habilitado AndroidX y Jetifier.

La cámara del jugador mantiene el componente de seguimiento que recibe la orientación del dispositivo cuando Cardboard está activo. El objeto `inicarScript` contiene `CardboardStartup`, encargado de los parámetros del visor y de los controles propios de Cardboard. Este componente complementa la configuración del cargador XR; no la sustituye.

### Corrección de CardboardStartup

Se ha aplicado la modificación solicitada para evitar ejecutar `Api.UpdateScreenParams()` dentro del editor:

```csharp
#if !UNITY_EDITOR
    Api.UpdateScreenParams();
#endif
```

La llamada permanece dentro de su función original y se incluye en la compilación Android.

## Ejercicio 2. Escenario, movimiento y recogida con la mirada

**Enunciado:** Crear una escena con terreno y objetos de la Asset Store, permitir el desplazamiento del jugador y recoger objetos mediante la vista al acercarse a ellos.

**Archivos:** `TapMovement.cs`, `GazeCollector.cs`, `ReticleMovementFeedback.cs`, `ReticleTarget.cs`, `playermovement.cs` y `VegetationScatterWindow.cs`.

**Descripción de lo realizado:**

### Escenario y ambientación

Se ha diseñado un valle con montañas, vegetación, rocas y un camino de exploración. El entorno combina un Terrain con piezas de suelo colocadas manualmente, como `Ground_01` y `Ground_03`. Estas superficies utilizan colliders para que puedan detectarse tanto el apoyo del jugador como los destinos de desplazamiento.

La vegetación se ha distribuido inicialmente con el pincel de árboles del Terrain. Para colocar plantas sobre las piezas de suelo independientes se ha incorporado la herramienta de editor `VegetationScatterWindow`. Esta genera posiciones aleatorias, lanza rayos hacia los suelos seleccionados de la capa `Walkable` y sitúa los prefabs sobre su superficie. Permite variar la escala, la rotación, la separación mínima y la pendiente aceptada, además de deshacer la generación.

Se ha trabajado la iluminación con tonos fríos y niebla para crear un ambiente oscuro y limitar la visibilidad del fondo. Los materiales de los paquetes se han adaptado a URP. En el caso de los cristales se ha importado el parche `TranslucentCrystals_URP_Patch_Unity6`, necesario para conservar su shader específico.

### Movimiento mediante mirada y tap

`TapMovement.cs` lanza un raycast desde la cámara hacia el centro de la imagen. El jugador señala una superficie y confirma el desplazamiento con un toque en pantalla. El destino debe pertenecer a la capa `Walkable`, estar dentro del alcance configurado y cumplir el límite de pendiente.

Antes del teletransporte se utiliza `Physics.OverlapCapsule()` para comprobar que el jugador cabe en el destino sin solaparse con otros colliders. El `CharacterController` gestiona la gravedad y el apoyo sobre el suelo.

| Parámetro | Valor de referencia |
|---|---|
| Alcance máximo de movimiento | 8 metros |
| Pendiente máxima | 35 grados |
| Confirmación en Android | Toque en pantalla |
| Confirmación en el editor | Clic izquierdo |

La selección del destino depende de la orientación de la cámara, no de la posición del dedo sobre la pantalla.

En la versión de `TapMovement.cs` guardada en este proyecto se valida el destino y el primer impacto del rayo, pero no se realiza un barrido del volumen del jugador a lo largo de todo el recorrido. Por ello, el teletransporte puede salvar obstáculos que no bloqueen la mirada ni ocupen el destino.

### Recogida de cristales

Los cristales se han colocado sobre pilares y disponen de un collider en su raíz y de la capa `Collectible`. `GazeCollector.cs`, situado en `Main Camera`, detecta el primer collider hacia el que mira el jugador.

Para recoger un cristal es necesario mirarlo durante cinco segundos seguidos desde una distancia máxima de tres metros medida hasta el impacto del raycast. Al apartar la mirada, cambiar de objetivo o salir del alcance, el tiempo se reinicia. Los colliders intermedios bloquean la selección.

Cuando se completa la interacción, el cristal se añade a `collectedObjects` y se oculta mediante `SetActive(false)`. No se destruye, ya que debe poder recuperarse en el ejercicio siguiente.

### Información visual del retículo

`ReticleMovementFeedback.cs` consulta la misma función de validación que utiliza `TapMovement` y cambia el color del retículo:

- **Verde:** el destino de desplazamiento es válido.
- **Rojo:** no se permite el desplazamiento hacia ese punto.

El componente original `CardboardReticlePointer` amplía el retículo cuando apunta a las capas interactivas configuradas. `ReticleTarget.cs` recibe sus eventos sin activar una recogida por clic: la recolección continúa dependiendo de los cinco segundos de mirada.

La expansión identifica un objetivo interactivo, pero no implica por sí sola que esté dentro de la distancia de recogida. El color sigue representando la posibilidad de desplazarse.

### Controles de prueba en el editor

El archivo `playermovement.cs` contiene la clase `EditorPlayerControls`, utilizada para orientar la cámara con el botón derecho del ratón y facilitar las pruebas sin un dispositivo Android. Su lógica está protegida con `#if UNITY_EDITOR` y no controla al jugador en el APK.

El proyecto conserva también `GazeMovement.cs`, correspondiente a una alternativa inicial de avance al mirar hacia abajo. Esta alternativa se ha sustituido por el movimiento mediante taps y debe permanecer desactivada.

## Ejercicio 3. Pedestal de recuperación

**Enunciado:** Elegir un objeto que permita recuperar los recolectables y hacer que, al seleccionarlo con la vista, se dirijan hacia el jugador.

**Archivos:** `RecallPedestal.cs` y `GazeCollector.cs`.

**Descripción de lo realizado:**

Se ha creado un pedestal de recuperación, distinguible por su cristal azul. El componente `RecallPedestal` se sitúa en la raíz y dispone de una referencia a `puntoSalida`, colocado sobre el pedestal.

Al mirarlo durante cinco segundos desde el alcance de interacción, `GazeCollector` solicita la recuperación. La acción solo comienza si existen cristales recogidos y no hay otra recuperación en curso.

Los cristales reaparecen en el punto de salida y se desplazan mediante una corrutina, con interpolación suavizada y un arco vertical. Sus posiciones de llegada se distribuyen delante del jugador y se fijan al activar la acción.

| Parámetro | Valor de referencia |
|---|---|
| Duración del vuelo | 2,5 segundos |
| Radio de llegada | 1,5 metros |
| Altura del arco | 1 metro |

Durante el vuelo se desactivan los colliders de los cristales. Al terminar se restauran, los objetos permanecen visibles y pueden recogerse de nuevo. La lista de objetos recogidos se vacía al iniciar la recuperación.

El vuelo es una animación de posición y no resuelve colisiones con el escenario. Por este motivo, el pedestal debe colocarse en una zona despejada.

## Capas y componentes principales

| Elemento | Capa o configuración | Función |
|---|---|---|
| Suelos transitables | `Walkable`, con collider | Destinos del teletransporte |
| Cristales recolectables | `Collectible`, collider no trigger | Selección y recogida por mirada |
| Cristal del pedestal | `Recall`, collider no trigger | Identificación visual del recuperador |
| Bases, árboles y rocas | Fuera de `Walkable`, con colliders cuando corresponda | Decoración y obstáculos |
| Player | `CharacterController` y `TapMovement` | Movimiento y apoyo sobre el suelo |
| Main Camera | Seguimiento de pose y `GazeCollector` | Orientación y selección |
| Retículo | Componentes Cardboard y `ReticleMovementFeedback` | Información visual |

Para los árboles pintados con el Terrain se han revisado los colliders de los prefabs y la opción `Enable Tree Colliders`. Se ha utilizado una cápsula para representar el tronco, evitando Mesh Colliders en los árboles del pincel.

## Recursos utilizados

| Recurso | Uso en la práctica |
|---|---|
| Google Cardboard XR Plugin y Hello Cardboard | Base de la aplicación, seguimiento y retículo |
| Simple Nature Pack | Vegetación, rocas y piezas de suelo |
| Simple Modular Pillars — Knightferret Studios | Pilares de recogida y pedestal |
| Translucent Crystals — SineVFX | Cristales recolectables y cristal del recuperador |

Los recursos externos mantienen sus respectivas licencias. La configuración del SDK se ha realizado tomando como referencia la [guía oficial de Google Cardboard para Unity](https://developers.google.com/cardboard/develop/unity/quickstart?hl=es-419).

## Ejecución y compilación

1. Abrir el proyecto con Unity `6000.6.3f1` y disponer del módulo Android Build Support con sus herramientas.
2. Abrir `HelloCardboard.unity` desde la ruta indicada y comprobar las referencias de cámara, capas y punto de salida del pedestal.
3. Para las pruebas del editor, utilizar el botón derecho y el ratón para mirar y el clic izquierdo para confirmar el teletransporte. WASD está disponible en el control de pruebas; no reproduce las comprobaciones de colisión del movimiento por taps.
4. En Build Profiles, activar Android e incluir la escena de la práctica como primera escena habilitada.
5. Comprobar Cardboard XR Plugin e inicialización al arrancar en la pestaña Android. Mantener `Activity`, orientación horizontal y OpenGLES3.
6. Desactivar `Build App Bundle` y `Export Project`, y pulsar `Build` para generar un APK.
7. Instalar el APK en un Android compatible y abrirlo con el dispositivo en horizontal. La aplicación debe mostrar la vista estereoscópica y responder a la rotación del teléfono.

Las compilaciones se encuentran en `Builds/`. Debe entregarse el APK que corresponda a la versión final validada, junto con los scripts y este README. Las carpetas `BackUpThisFolder_ButDontShipItWithYourGame` generadas por Unity no son necesarias para instalar la aplicación.

## Incidencias y comprobaciones

Durante el desarrollo se han abordado las siguientes incidencias:

- **Ruta con caracteres no ASCII:** se trasladó el proyecto a una ruta sin la `ñ` de `Cuarto-año` para permitir la compilación Android.
- **Materiales rosas:** se convirtieron los materiales compatibles a URP y se utilizó el parche específico de Unity 6 para los cristales.
- **Vegetación enterrada:** se separó la altura del Terrain de las piezas de suelo y se añadió la herramienta de distribución sobre mallas.
- **Ausencia de seguimiento y vista estereoscópica:** se revisó la asignación del cargador Cardboard en Android y la inicialización XR.
- **Tema AppCompat no encontrado:** se incorporaron las dependencias y propiedades de Gradle requeridas por Cardboard.
- **Imagen girada y renderizado incompleto en el móvil:** se cambió a orientación horizontal fija y OpenGLES3. La validación de estos últimos ajustes debe realizarse sobre el APK actualizado en el dispositivo.

La recogida y la recuperación se han comprobado durante las pruebas de desarrollo en el editor. También se han generado APK y realizado pruebas en Android que permitieron detectar los problemas de inicialización y renderizado descritos. Este documento no da por validada la última compilación en móvil sin repetir esa prueba.

## Demostración de la aplicación

![Demostración de la práctica Cardboard](gifs/gifcardborad.gif)
