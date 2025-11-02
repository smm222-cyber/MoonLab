# Guía: Sistema de Minijuego de Conectar Cables

Esta guía te explica cómo configurar el sistema completo para que un objeto abra el minijuego de conectar cables cuando el jugador presiona E.

---

## 📋 Componentes Necesarios

### Scripts:
- `ConnectMiniGameTrigger.cs` - En el objeto que se puede interactuar
- `ConnectingController.cs` - En el contenedor del minijuego
- `ConnectablePoint.cs` - En cada punto/círculo que se puede conectar
- `ConnectionLine.cs` - Se crea automáticamente para las líneas

### En la Escena:
1. Un objeto interactivo (ej: caja de herramientas, panel eléctrico)
2. Un Empty/Canvas con el minijuego (puntos + fondo)
3. EventSystem (debe existir en la escena)

---

## 🔧 Paso 1: Configurar el Objeto Interactivo

### 1.1 Crear el objeto
- Crea un GameObject en la escena (ej: "Panel_Electrico")
- Añádele un `Collider2D` (BoxCollider2D o CircleCollider2D)
- Marca el collider como **Trigger** ✓

### 1.2 Añadir el script ConnectMiniGameTrigger
1. Selecciona el objeto
2. Add Component → `ConnectMiniGameTrigger`
3. Configura los campos:

```
┌─────────────────────────────────────────┐
│ ConnectMiniGameTrigger                  │
├─────────────────────────────────────────┤
│ UI                                      │
│ ├─ Interact UI: [Arrastra aquí tu      │
│ │                icono de "Press E"]   │
│                                         │
│ Requisitos de Misión                   │
│ ├─ Required Mission: "Buscar cables"   │
│    (dejar vacío si no requiere misión) │
│                                         │
│ Canvas del Minijuego                   │
│ ├─ Connect MiniGame Canvas:            │
│    [Arrastra el Empty del minijuego]   │
│                                         │
│ Cámara                                 │
│ ├─ Camera Follow:                      │
│    [Arrastra la Main Camera aquí]      │
│    (para que deje de seguir al jugador)│
│                                         │
│ Feedback                               │
│ ├─ Message When Locked:                │
│    "Necesito encontrar los cables      │
│     primero"                           │
└─────────────────────────────────────────┘
```

### 1.3 Configurar Layer
- Asigna el objeto a un layer que el jugador pueda detectar (ej: "Interactable")
- Tu script `InteractPlayerItem` debe tener ese layer en su máscara

---

## 🎮 Paso 2: Configurar el Prefab del Minijuego

### 2.1 Crear el Prefab (Solo una vez)

**Si ya tienes el prefab creado, salta al paso 2.3**

**⚠️ IMPORTANTE: Usa un Canvas UI para que siempre se vea en la misma posición**

1. Crea un Canvas (si no existe):
   - GameObject → UI → Canvas
   - En el Canvas, configura:
     - Render Mode: **Screen Space - Overlay**
     - ✓ Esto hace que siempre se vea en pantalla

2. Crea esta jerarquía como hijo del Canvas:

```
Canvas (Screen Space - Overlay)
└─ MiniJuego_Cables (Empty o Panel)
   ├─ ConnectingController (script aquí)
   ├─ Fondo_Cables (Image UI con sprite de fondo)
   ├─ Punto1 (Image UI - círculo con Collider2D)
   │  ├─ ConnectablePoint (script)
   │  └─ Highlight (Image UI de color opcional)
   ├─ Punto2 (Image UI - círculo con Collider2D)
   │  └─ ...
   ├─ Punto3 (Image UI - círculo con Collider2D)
   │  └─ ...
   └─ Punto4 (Image UI - círculo con Collider2D)
      └─ ...
```

**O si prefieres seguir con Sprites en mundo:**

```
MiniJuego_Cables (Empty)
├─ ConnectingController (script aquí)
├─ Fondo_Cables (Sprite con imagen de cables)
├─ Punto1 (Círculo Sprite con Collider2D)
│  ├─ ConnectablePoint (script)
│  └─ Highlight (círculo de color opcional)
├─ Punto2 (Círculo Sprite con Collider2D)
│  └─ ...
├─ Punto3 (Círculo Sprite con Collider2D)
│  └─ ...
└─ Punto4 (Círculo Sprite con Collider2D)
   └─ ...
```
**(Pero tendrás que posicionar el Empty cerca de donde esté la cámara)**

2. Arrastra el Empty completo a la carpeta `Assets/03_Prefabs` (o donde guardes prefabs)
3. Se creará un prefab azul en el Project
4. Ahora puedes **eliminar** el Empty de la Hierarchy (el prefab ya está guardado)

### 2.2 Configurar el Prefab

1. **Doble clic** en el prefab en el Project para editarlo (modo Prefab)
2. Configura todos los scripts como se explica abajo
3. Guarda el prefab (Ctrl+S o se guarda automáticamente)
4. Sal del modo Prefab

### 2.3 Usar el Prefab en tu Escena

**Ya tienes el prefab creado, ahora úsalo así:**

1. **Arrastra el prefab a la Hierarchy**
   - Desde la carpeta Project, arrastra el prefab `MiniJuego_Cables` a la Hierarchy
   - Puede ir como hijo del Canvas o en la raíz de la escena

2. **⚠️ MUY IMPORTANTE: Desactívalo**
   - Selecciona el prefab en la Hierarchy
   - En el Inspector, **desactiva el checkbox** (☐) al lado del nombre
   - Debe quedar en gris/desactivado
   - Si no lo desactivas, se mostrará desde el inicio del juego

3. **Arrastra la instancia al Trigger**
   - Selecciona tu objeto interactivo (ej: Panel_Electrico)
   - En el componente `ConnectMiniGameTrigger`
   - Arrastra la **instancia desactivada** desde la Hierarchy al campo `Connect MiniGame Canvas`
   - ⚠️ NO arrastres el prefab desde Project, arrastra la instancia desde Hierarchy

**Resumen visual:**
```
Hierarchy:
├─ EventSystem
├─ Main Camera
├─ Player
├─ Panel_Electrico (objeto interactivo)
│  └─ ConnectMiniGameTrigger
│      └─ Connect MiniGame Canvas: [MiniJuego_Cables] ← arrastra esto
└─ MiniJuego_Cables ☐ (DESACTIVADO, en gris)
    ├─ Puntos...
    └─ Fondo...
```

**¿Por qué así?**
- ✅ El prefab está listo en la escena pero invisible
- ✅ El Trigger solo lo activa cuando el jugador presiona E
- ✅ Al completar, se desactiva automáticamente
- ✅ Puedes tener varios objetos interactivos que usen el mismo minijuego

---

### 2.4 (Alternativa Avanzada) Múltiples Instancias

Si necesitas que diferentes objetos abran diferentes minijuegos:

1. **Duplica el prefab en la Hierarchy**
   - Arrastra el prefab dos veces (o más)
   - Renombra: `MiniJuego_Cables_1`, `MiniJuego_Cables_2`, etc.
   - **Desactiva ambos** (☐)

2. **Edita cada instancia**
   - Puedes cambiar el fondo, mover puntos, etc.
   - Los cambios solo afectan esa instancia

3. **Asigna cada uno a un objeto diferente**
   - Panel_Electrico_1 → usa MiniJuego_Cables_1
   - Panel_Electrico_2 → usa MiniJuego_Cables_2

**O mejor: Crea Prefab Variants** (ver sección al final de la guía)

---

**⚠️ RECORDATORIO CLAVE**: 
- El prefab/instancia en la Hierarchy DEBE estar **desactivado** (☐)
- Si se ve en la escena al iniciar el juego, es porque olvidaste desactivarlo

### 2.2 Configurar ConnectingController

1. Selecciona el Empty `MiniJuego_Cables`
2. Add Component → `ConnectingController`
3. Configura:

```
┌─────────────────────────────────────────┐
│ ConnectingController                    │
├─────────────────────────────────────────┤
│ Input / Layers                          │
│ ├─ Node Layer Mask: [Selecciona el     │
│    layer de tus puntos, ej: "Default"] │
│                                         │
│ Linea / Visual                          │
│ ├─ Line Material: [Material de línea]  │
│ ├─ Line Width: 0.05                    │
│                                         │
│ Completion                              │
│ ├─ Required Connections: 4             │
│    (número de conexiones para ganar)   │
│ ├─ Auto Close On Complete: ✓           │
│ ├─ Mission To Complete:                │
│    "Reparar cables"                    │
│    (opcional, misión a completar)      │
└─────────────────────────────────────────┘
```

---

## ⚫ Paso 3: Configurar cada Punto

### 3.1 Crear un punto
1. GameObject → 2D Object → Sprites → Circle (o usa tu propio sprite)
2. Renómbralo (ej: "Punto_A1")
3. Ponlo como hijo del Empty del minijuego

### 3.2 Añadir Collider
1. Add Component → `Circle Collider 2D`
2. Ajusta el radio para que cubra el círculo
3. **Deja Trigger DESACTIVADO** ☐

### 3.3 Añadir ConnectablePoint
1. Add Component → `ConnectablePoint`
2. Configura:

```
┌─────────────────────────────────────────┐
│ ConnectablePoint                        │
├─────────────────────────────────────────┤
│ ├─ Point Id: "A1"                       │
│    (identificador único del punto)      │
│                                         │
│ ├─ Anchor: [Deja vacío o arrastra      │
│    el mismo punto]                      │
│                                         │
│ ├─ Highlight Visual: [Crea un hijo     │
│    con sprite de color y arrástralo]   │
│                                         │
│ ├─ Allowed Target Id: "B1"             │
│    (ID del punto al que DEBE            │
│     conectarse, vacío = cualquiera)    │
└─────────────────────────────────────────┘
```

### 3.4 Repetir para todos los puntos

**Ejemplo de configuración:**
- Punto_A1: `pointId = "A1"`, `allowedTargetId = "B1"`
- Punto_B1: `pointId = "B1"`, `allowedTargetId = "A1"`
- Punto_A2: `pointId = "A2"`, `allowedTargetId = "B2"`
- Punto_B2: `pointId = "B2"`, `allowedTargetId = "A2"`

Esto hace que solo se puedan conectar pares correctos (A1↔B1, A2↔B2).

---

## 🎯 Paso 4: Configurar las Conexiones Válidas

### Opción 1: Sin restricciones
- Deja `allowedTargetId` vacío en todos los puntos
- Cualquier punto podrá conectarse con cualquier otro

### Opción 2: Conexiones específicas
- Define en cada punto su `allowedTargetId`
- Solo podrá conectarse con el punto que tenga ese ID

**Ejemplo: 4 cables que van de izquierda a derecha**
```
Izquierda          Derecha
[A1] ──────────→ [B1]
[A2] ──────────→ [B2]
[A3] ──────────→ [B3]
[A4] ──────────→ [B4]
```

Configuración:
- A1: `allowedTargetId = "B1"`
- B1: `allowedTargetId = "A1"`
- A2: `allowedTargetId = "B2"`
- B2: `allowedTargetId = "A2"`
- ... y así sucesivamente

---

## 🎨 Paso 5: Configurar Material de Línea (Opcional)

### 5.1 Crear Material
1. Clic derecho en Project → Create → Material
2. Renombrar: "LineMaterial"
3. Shader: Sprites/Default (o Unlit/Color)
4. Color: Amarillo (o el que prefieras)

### 5.2 Asignar a ConnectingController
- Arrastra el material al campo `Line Material` del `ConnectingController`

---

## 🎮 Flujo del Juego

### Al iniciar la escena:
1. El Empty del minijuego está **desactivado**
2. El jugador se mueve normalmente

### Cuando el jugador interactúa (presiona E):
1. `ConnectMiniGameTrigger` verifica si tiene la misión requerida
   - ✓ **SÍ tiene la misión**: Abre el minijuego
   - ✗ **NO tiene la misión**: Muestra mensaje de bloqueo

### Si abre el minijuego:
1. Se activa el Empty del minijuego
2. Se bloquea el movimiento del jugador
3. El jugador ve los puntos y el fondo

### Jugando:
1. Click en un punto → arrastra → suelta en otro punto
2. Se dibuja una línea conectando ambos
3. Si la conexión es válida (IDs coinciden), se mantiene
4. Si es inválida y `allowedTargetId` está definido, no se permite

### Al completar todas las conexiones:
1. `ConnectingController` detecta que se alcanzó `requiredConnections`
2. Espera 1 segundo
3. Completa la misión (si configuraste `missionToComplete`)
4. Cierra el minijuego automáticamente
5. Reactiva el movimiento del jugador

---

## ⚠️ Checklist de Configuración

Antes de probar, verifica:

- [ ] EventSystem existe en la escena
- [ ] El objeto interactivo tiene `ConnectMiniGameTrigger`
- [ ] El objeto interactivo tiene `Collider2D` marcado como Trigger
- [ ] El Empty del minijuego está **desactivado** al inicio
- [ ] El Empty tiene `ConnectingController` configurado
- [ ] Cada punto tiene `ConnectablePoint` y `Collider2D`
- [ ] Los `pointId` son únicos para cada punto
- [ ] Los `allowedTargetId` están bien emparejados (si usas restricciones)
- [ ] `requiredConnections` coincide con el número de pares a conectar
- [ ] El material de línea está asignado (opcional pero recomendado)

---

## 🐛 Problemas Comunes

### El jugador no puede interactuar
- ✓ Verifica que el objeto tiene un Collider2D marcado como Trigger
- ✓ Verifica que el layer del objeto está en la máscara de `InteractPlayerItem`
- ✓ Asegúrate de que el jugador tiene el script de interacción activo

### No se ve el minijuego al interactuar
- ✓ Verifica que asignaste el Empty en `Connect MiniGame Canvas`
- ✓ Comprueba que el jugador tiene la misión requerida (o deja el campo vacío)
- ✓ Mira la consola por errores

### No puedo conectar los puntos (clic no funciona)
- ✓ Verifica que cada punto tiene `CircleCollider2D` o `BoxCollider2D` (NO marcado como Trigger ☐)
- ✓ Asegúrate de que el layer de los puntos está en `Node Layer Mask` del Controller (NO en "Nothing")
- ✓ Comprueba que la cámara es Main Camera (tag "MainCamera")
- ✓ Lee la consola - hay logs de debug que te dirán exactamente qué falta
- ✓ **Ver el archivo `DEBUG_CONEXIONES.md` para diagnóstico completo**

### Las líneas no se ven
- ✓ Asigna un material al `ConnectingController`
- ✓ Aumenta el `lineWidth` (prueba con 0.1 o más)
- ✓ Verifica que los puntos están en coordenadas de mundo visibles

### El minijuego no se cierra al completar
- ✓ Verifica que `requiredConnections` es correcto (número de pares)
- ✓ Asegúrate de que `autoCloseOnComplete` está activado
- ✓ Comprueba que las conexiones son válidas según los `allowedTargetId`

### Se conecta pero dice que falta completar
- ✓ Las conexiones solo cuentan si son válidas (IDs correctos)
- ✓ Verifica los `allowedTargetId` de cada punto
- ✓ Si quieres que cualquier conexión valga, deja `allowedTargetId` vacío

### La cámara sigue al jugador durante el minijuego
- ✓ Arrastra la Main Camera al campo `Camera Follow` del `ConnectMiniGameTrigger`
- ✓ La cámara debe tener el script `CameraFollow` con la opción `Is Following` activada

---

## 🎓 Ejemplo Completo

**Escenario**: Panel eléctrico con 2 cables a conectar

### Configuración:

**Panel_Electrico (GameObject)**
- BoxCollider2D (Trigger ✓)
- ConnectMiniGameTrigger
  - Required Mission: "Buscar herramienta"
  - Connect MiniGame Canvas: [Arrastra el **prefab** desde Project, o la instancia desde Hierarchy]
  - Message When Locked: "Necesito encontrar la herramienta eléctrica"

**MiniJuego_Cables (Prefab en Hierarchy, DESACTIVADO ☐)**
- O bien: Solo el prefab en Project (si usas instanciado automático)
- ConnectingController
  - Node Layer Mask: Default
  - Required Connections: 2
  - Auto Close On Complete: ✓
  - Mission To Complete: "Reparar panel"

**Punto_Rojo1** (hijo de MiniJuego_Cables)
- CircleCollider2D
- ConnectablePoint: `pointId = "Rojo1"`, `allowedTargetId = "Rojo2"`

**Punto_Rojo2** (hijo de MiniJuego_Cables)
- CircleCollider2D
- ConnectablePoint: `pointId = "Rojo2"`, `allowedTargetId = "Rojo1"`

**Punto_Azul1** (hijo de MiniJuego_Cables)
- CircleCollider2D
- ConnectablePoint: `pointId = "Azul1"`, `allowedTargetId = "Azul2"`

**Punto_Azul2** (hijo de MiniJuego_Cables)
- CircleCollider2D
- ConnectablePoint: `pointId = "Azul2"`, `allowedTargetId = "Azul1"`

---

## 📚 Resumen Rápido

1. **Objeto con `ConnectMiniGameTrigger`** → El que presionas E
2. **Prefab del minijuego** → Contiene todos los puntos y el Controller
3. **ConnectingController** → Maneja la lógica de conexiones
4. **ConnectablePoint en cada punto** → Define IDs y restricciones
5. **Collider2D en cada punto** → Para detectar clics

**Flujo**: E en objeto → Abre minijuego → Conecta puntos → Al completar → Cierra automático

---

## 🎨 Ventajas de Usar Prefab

✅ **Reutilizable**: Usa el mismo prefab en múltiples objetos/escenas
✅ **Fácil de actualizar**: Cambios en el prefab se aplican a todas las instancias
✅ **Múltiples niveles**: Crea variantes del prefab (Prefab Variant) para diferentes diseños:
   - `MiniJuego_Cables_Nivel1` (4 cables)
   - `MiniJuego_Cables_Nivel2` (6 cables)
   - `MiniJuego_Cables_Nivel3` (8 cables más complejos)

### Crear Variantes del Prefab

1. Click derecho en el prefab → Create → Prefab Variant
2. Renombra la variante (ej: "MiniJuego_Cables_Nivel2")
3. Doble clic para editarla
4. Cambia el sprite del fondo, reposiciona puntos, añade más puntos
5. Ajusta `requiredConnections` en el `ConnectingController`
6. Guarda

Ahora puedes usar diferentes variantes en diferentes objetos:
- Panel_Electrico_1 → usa MiniJuego_Cables_Nivel1
- Panel_Electrico_2 → usa MiniJuego_Cables_Nivel2
- etc.

---

¡Listo! Ahora puedes crear diferentes niveles con variantes del prefab y reutilizar el sistema en toda tu escena/proyecto.
