# 🐛 Diagnóstico: No puedo conectar los puntos

## ✅ Checklist de Configuración

### 1. En Unity Editor - ANTES de ejecutar:

#### Para cada punto (Círculo/Cuadrado):
- [ ] Tiene un `Collider2D` (CircleCollider2D o BoxCollider2D)
- [ ] El collider **NO** está marcado como Trigger (☐)
- [ ] Tiene el script `ConnectablePoint` añadido
- [ ] El `Point Id` está configurado (ej: "A1", "B1", etc.)

#### En el ConnectingController (Empty padre):
- [ ] Tiene el script `ConnectingController`
- [ ] El campo `Node Layer Mask` está configurado (NO debe estar en "Nothing")
  - Selecciona el layer donde están tus puntos (probablemente "Default")
- [ ] Tiene un Material asignado en `Line Material` (opcional pero recomendado)
- [ ] `Required Connections` está configurado (número de pares a conectar)

#### En la Cámara:
- [ ] La Main Camera tiene el tag "MainCamera"
- [ ] La cámara puede ver la posición de tus puntos

---

## 🔍 Pasos para Diagnosticar

### 1. Verifica la Consola al INICIAR el juego:

Cuando el minijuego se active, deberías ver:
```
[ConnectingController] Encontrados X puntos conectables
```

**Si dice "0 puntos":**
- ✗ Los puntos NO son hijos del Empty que tiene el ConnectingController
- ✗ O los puntos no tienen el script `ConnectablePoint`

**Si muestra warnings sobre Colliders:**
- ✗ Algún punto no tiene Collider2D (añádelo)
- ✗ O tiene el Collider marcado como Trigger (desmárcalo ☐)

---

### 2. HAZ UN CLICK en cualquier parte del minijuego:

Deberías ver en la consola:
```
[ConnectingController] Click en posición mundo: (x, y)
```

**Si NO aparece este mensaje:**
- ✗ El minijuego NO está activo
- ✗ O hay otro objeto capturando los clicks (UI encima)

---

### 3. HAZ CLICK exactamente ENCIMA de un punto:

**Si detecta el collider, verás:**
```
[ConnectingController] Detectado collider: NombreDelPunto
[ConnectingController] Iniciando conexión desde: A1
```

**Si sale "No se detectó ningún collider":**
- ✗ El `Node Layer Mask` está mal configurado
- ✗ O los puntos están en un layer diferente al configurado
- ✗ O el click no está realmente sobre el punto

**Si detecta el collider pero dice "no tiene ConnectablePoint":**
- ✗ El script `ConnectablePoint` no está en el GameObject del círculo
- ✗ O está en un hijo y no en el mismo objeto del Collider

**Si dice "ya tiene una conexión":**
- ✓ Está funcionando, pero ese punto ya está conectado
- Intenta con otro punto

---

### 4. ARRASTRA hacia otro punto y SUELTA:

**Si es exitoso, verás:**
```
[ConnectingController] Soltado en: B1
[ConnectingController] ✓ Conexión válida: A1 <-> B1
```

**Si dice "No puedes conectar un punto consigo mismo":**
- ✗ Soltaste en el mismo punto donde empezaste
- Arrastra más lejos

**Si dice "no permite conectar" con un allowedTargetId:**
- ⚠️ Los IDs no coinciden
- Verifica que `A1.allowedTargetId = "B1"` y `B1.allowedTargetId = "A1"`
- O deja `allowedTargetId` vacío para permitir cualquier conexión

**Si dice "Soltado fuera de un punto":**
- ✗ No soltaste encima de un punto válido
- Arrastra exactamente hasta otro círculo

---

## 🔧 Soluciones Comunes

### Problema: "No se detectó ningún collider"

**Solución 1: Configurar Layer Mask**
1. Selecciona el Empty con `ConnectingController`
2. En el Inspector, busca `Node Layer Mask`
3. Haz clic en el dropdown
4. Marca el layer donde están tus puntos (probablemente "Default")

**Solución 2: Verificar layers de los puntos**
1. Selecciona un punto (círculo)
2. En el Inspector, arriba, verás el Layer (ej: "Default")
3. Asegúrate de que TODOS los puntos estén en el mismo layer
4. Ese layer debe estar marcado en el `Node Layer Mask`

### Problema: "El collider está marcado como Trigger"

1. Selecciona cada punto
2. En el componente `Circle Collider 2D` o `Box Collider 2D`
3. **Desmarca** el checkbox "Is Trigger" (☐)
4. El collider debe ser sólido, NO trigger

### Problema: Los puntos están muy lejos/no se ven

1. Posiciona la cámara en `(0, 0, -10)`
2. Posiciona los puntos alrededor de `(0, 0, 0)`
3. O ajusta la posición de la cámara para que vea los puntos

### Problema: "no permite conectar" - IDs incorrectos

**Opción A: Sin restricciones (cualquier punto con cualquiera)**
- Deja el campo `Allowed Target Id` **vacío** en TODOS los puntos

**Opción B: Con restricciones (pares específicos)**
- Punto A1: `allowedTargetId = "B1"`
- Punto B1: `allowedTargetId = "A1"`
- Punto A2: `allowedTargetId = "B2"`
- Punto B2: `allowedTargetId = "A2"`
- etc.

---

## 📸 Configuración de Ejemplo

```
MiniJuego_Cables (Empty)
├─ ConnectingController (script)
│  ├─ Node Layer Mask: Default ✓
│  ├─ Line Material: [Material]
│  ├─ Required Connections: 2
│  └─ Auto Close On Complete: ✓
│
├─ Punto_A1 (Sprite Circle)
│  ├─ Circle Collider 2D
│  │  └─ Is Trigger: ☐ (DESMARCADO)
│  └─ ConnectablePoint
│      ├─ Point Id: "A1"
│      └─ Allowed Target Id: "B1"
│
├─ Punto_B1 (Sprite Circle)
│  ├─ Circle Collider 2D
│  │  └─ Is Trigger: ☐ (DESMARCADO)
│  └─ ConnectablePoint
│      ├─ Point Id: "B1"
│      └─ Allowed Target Id: "A1"
│
└─ ... más puntos
```

---

## 🎮 Flujo de Prueba

1. **Ejecuta el juego**
2. **Lee la consola** → debería decir "Encontrados X puntos"
3. **Presiona E** en el objeto interactivo → el minijuego aparece
4. **Haz click en un círculo** → consola debería decir "Iniciando conexión desde..."
5. **Arrastra hasta otro círculo** → debería verse una línea amarilla siguiendo el mouse
6. **Suelta** → consola debería decir "✓ Conexión válida"
7. **Repite** hasta completar todas las conexiones
8. Debería cerrarse automáticamente

---

## ⚠️ Último Recurso: Proyecto de Prueba Mínimo

Si nada funciona, crea un proyecto de prueba:

1. Crea un Empty llamado "Test_Minijuego"
2. Añádele el script `ConnectingController`
3. Configura `Node Layer Mask: Default`
4. Crea 2 círculos como hijos:
   - GameObject → 2D Object → Sprites → Circle
   - Renombra: "CirculoA" y "CirculoB"
5. A cada círculo:
   - Add Component → Circle Collider 2D (NO trigger)
   - Add Component → ConnectablePoint
   - CirculoA: `pointId = "A"`, `allowedTargetId = ""` (vacío)
   - CirculoB: `pointId = "B"`, `allowedTargetId = ""` (vacío)
6. Posiciona los círculos separados (ej: (-2, 0) y (2, 0))
7. Activa el Empty
8. Ejecuta y prueba

Si esto funciona, compara con tu configuración original para ver qué falta.
