# 🎬 ESCENA FINAL SIMPLE - Solo Efecto Visual

## ✅ ¿QUÉ ES ESTO?

Una escena que muestra texto con un efecto bonito de máquina de escribir (typewriter).
**NO usa contadores**, es solo para probar el efecto visual.

---

## 🎮 PASO A PASO

### PASO 1: Crear la Escena

1. **File** → **New Scene**
2. **File** → **Save As**
3. Nombre: `EndingScene` (o el que prefieras)
4. Guardar en: `Assets/01_Scenes/`

---

### PASO 2: Crear el Canvas

1. Click derecho en **Hierarchy** → **UI** → **Canvas**
2. Configurar el Canvas:
   - **Render Mode:** Screen Space - Overlay
   - **Canvas Scaler:**
     - UI Scale Mode: Scale With Screen Size
     - Reference Resolution: 1920 x 1080

---

### PASO 3: Crear el Fondo Negro

1. Click derecho en **Canvas** → **UI** → **Panel**
2. Renombrar a: `BackgroundPanel`
3. Configurar:
   - **Anchor Presets:** Stretch (ambos ejes)
   - **Left, Right, Top, Bottom:** todos en 0
   - **Color:** Negro (R:0, G:0, B:0, A:255)

---

### PASO 4: Crear el Texto

1. Click derecho en **Canvas** → **UI** → **Text - TextMeshPro**
   - (Si sale ventana, importa TMP Essentials)
2. Renombrar a: `GuionText`
3. Configurar **Rect Transform:**
   - **Anchor:** Center-Middle
   - **Width:** 1200
   - **Height:** 800
   - **Pos X:** 0, **Pos Y:** 0

4. Configurar **TextMeshPro:**
   - **Font Size:** 36
   - **Color:** Blanco
   - **Alignment:** Center + Middle (horizontalmente y verticalmente)
   - **Wrapping:** Enabled
   - **Line Spacing:** 1.5

---

### PASO 5: Agregar el Script

1. Selecciona el **Canvas** en Hierarchy
2. **Add Component** → Buscar: `EndingSceneSimple`
3. Configurar en el Inspector:

#### **UI References:**
- **Guion Text:** Arrastra `GuionText` desde Hierarchy

#### **Ending Text:**
- Escribe aquí el texto que quieres mostrar
- Puedes usar `\n` para saltos de línea
- Ejemplo:
  ```
  Bienvenido al final...\n\nEste es un ejemplo de texto con efecto bonito.\n\n¡Gracias por jugar!
  ```

#### **Settings:**
- **Text Speed:** 0.05 (más bajo = más lento)
  - Prueba: 0.03 (rápido), 0.05 (normal), 0.08 (lento)
- **Skip Text With Click:** ✅ (permite saltar con click)

---

## 🎮 CÓMO PROBAR

1. Asegúrate de guardar la escena (Ctrl+S)
2. Dale **Play ▶️** en Unity
3. El texto aparecerá letra por letra
4. **Haz click** para mostrar todo el texto inmediatamente
5. **Haz click de nuevo** después de terminar (aparece mensaje en Console)

---

## 🎨 ESTRUCTURA FINAL

```
Hierarchy:
  ├── Canvas
  │   ├── BackgroundPanel (Panel negro)
  │   ├── GuionText (TextMeshPro)
  │   └── EndingSceneSimple (Script - en el Canvas)
  └── EventSystem
```

---

## 🎯 PARA CARGAR ESTA ESCENA DESDE EL JUEGO

Cuando quieras mostrar esta escena desde cualquier script:

```csharp
using UnityEngine.SceneManagement;

// En cualquier script
SceneManager.LoadScene("EndingScene");
```

**IMPORTANTE:** Agrega la escena a Build Settings:
- **File** → **Build Settings**
- Arrastra `EndingScene` a la lista
- Ya está lista para usar

---

## 🎨 PERSONALIZACIÓN

### Cambiar el Texto
- Selecciona el Canvas
- En Inspector, busca el componente `Ending Scene Simple`
- Edita el campo **Texto Final**

### Cambiar la Velocidad
- Ajusta **Text Speed**:
  - 0.03 = Rápido
  - 0.05 = Normal
  - 0.08 = Lento

### Cambiar el Color del Fondo
- Selecciona `BackgroundPanel`
- En Inspector → **Image** → **Color**
- Elige el color que quieras

### Cambiar el Color del Texto
- Selecciona `GuionText`
- En Inspector → **TextMeshPro** → **Vertex Color**
- Elige el color que quieras

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### ❌ El texto no aparece
- ✅ Verifica que arrastraste `GuionText` al Inspector del Canvas
- ✅ Verifica que el texto sea visible (blanco sobre negro)
- ✅ Verifica que escribiste algo en el campo "Texto Final"

### ❌ No puedo hacer click para saltar
- ✅ Verifica que "Skip Text With Click" esté activado (✅)

### ❌ El texto va muy rápido/lento
- ✅ Ajusta "Text Speed" en el Inspector
- ✅ Prueba valores entre 0.03 y 0.08

### ❌ Aparece error en Console
- Si dice "GuionText no está asignado":
  - Selecciona Canvas
  - Arrastra GuionText al campo correspondiente en Inspector

---

## ✅ CHECKLIST RÁPIDO

- [ ] Escena creada
- [ ] Canvas creado
- [ ] Panel negro creado
- [ ] GuionText creado (TextMeshPro)
- [ ] Script `EndingSceneSimple` agregado al Canvas
- [ ] GuionText asignado en el Inspector
- [ ] Texto personalizado escrito
- [ ] Probado en Play Mode

---

## 💡 PRÓXIMOS PASOS

Una vez que tengas este efecto funcionando, puedes:

1. **Agregar música de fondo:**
   - Agregar Audio Source al Canvas
   - Asignar un AudioClip
   - Play On Awake

2. **Integrar con contadores:**
   - Usar el script `EndingSceneController` en lugar de `EndingSceneSimple`
   - Seguir las instrucciones en `INSTRUCCIONES_ESCENA_FINAL.md`

3. **Agregar más efectos:**
   - Fade in/out al entrar
   - Partículas sutiles
   - Animaciones adicionales

---

**¡Eso es todo!** Ahora tienes una escena simple con efecto de texto bonito. 🎉

**Pruébala y después puedes integrarla con el sistema de contadores si quieres.** 😊
