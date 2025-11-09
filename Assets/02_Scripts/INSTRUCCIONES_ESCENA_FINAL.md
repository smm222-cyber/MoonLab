# 🎬 ESCENA DE FINAL - GUÍA DE CONFIGURACIÓN

## ✅ ¿QUÉ ES ESTO?

Una escena de final que muestra diferentes guiones dependiendo de las elecciones del jugador durante el juego.

### 🎯 Finales Posibles:
1. **Final "Circo":** Si preguntó más sobre los demás personajes
2. **Final "Yo":** Si habló más sobre sí mismo (o empate)

---

## 🎮 PASO A PASO - CONFIGURACIÓN

### PASO 1: Crear la Nueva Escena

1. En Unity, ve a **File** → **New Scene**
2. Guarda la escena: **File** → **Save As**
3. Nombre sugerido: `EndingScene`
4. Guárdala en: `Assets/01_Scenes/`

---

### PASO 2: Configurar el Canvas

#### 2.1 Crear Canvas
1. Click derecho en **Hierarchy** → **UI** → **Canvas**
2. Unity creará automáticamente: `Canvas` y `EventSystem`

#### 2.2 Configurar Canvas
1. Selecciona `Canvas` en Hierarchy
2. En el Inspector:
   - **Render Mode:** Screen Space - Overlay
   - **Canvas Scaler:**
     - UI Scale Mode: **Scale With Screen Size**
     - Reference Resolution: **1920 x 1080**
     - Match: 0.5 (medio entre Width y Height)

---

### PASO 3: Crear el Panel Negro de Fondo

1. Click derecho en `Canvas` → **UI** → **Panel**
2. Renombrar a: `BackgroundPanel`
3. Configurar:
   - **Anchor:** Stretch (ambos ejes)
   - **Left, Right, Top, Bottom:** todos en **0**
   - **Color:** Negro puro (R:0, G:0, B:0, A:255)

---

### PASO 4: Crear el Texto del Guion

1. Click derecho en `Canvas` → **UI** → **Text - TextMeshPro**
2. Renombrar a: `GuionText`
3. Configurar **Rect Transform:**
   - **Anchor:** Center-Middle
   - **Width:** 1200
   - **Height:** 800
   - **Pos X:** 0
   - **Pos Y:** 0

4. Configurar **TextMeshPro:**
   - **Font Size:** 32
   - **Color:** Blanco
   - **Alignment:** Center + Middle
   - **Wrapping:** Enabled
   - **Overflow:** Overflow
   - **Line Spacing:** 1.5
   - **Paragraph Spacing:** 20

---

### PASO 5: Configurar el Script EndingSceneController

1. Selecciona `Canvas` en Hierarchy
2. **Add Component** → Buscar `EndingSceneController`
3. Configurar en el Inspector:

#### **Referencias UI:**
- **Background Panel:** Arrastra `BackgroundPanel`
- **Guion Text:** Arrastra `GuionText`

#### **Configuración de Texto:**
- **Typing Speed:** 30 (caracteres por segundo)
- **Wait Time After Text:** 3 (segundos)

#### **Guiones de Finales:**
Los textos por defecto están bien, pero puedes personalizarlos:

**Ending Circo** (si preguntó más sobre otros):
```
Tu curiosidad por conocer a los demás del circo te ha llevado a crear lazos increíbles.

Cada personaje te confió sus historias, sus sueños y sus miedos.

Ahora eres parte importante de esta gran familia circense.

El circo no es solo un espectáculo... es un hogar.
```

**Ending Yo** (si habló más sobre sí mismo o empate):
```
Al compartir tu historia con todos, encontraste tu lugar en el circo.

Cada personaje te escuchó, te comprendió y te ayudó a crecer.

Descubriste que al abrirte a los demás, también te conoces mejor a ti mismo.

Esta es tu historia... y apenas comienza.
```

#### **Opciones:**
- **Next Scene Name:** Dejar vacío para salir del juego, o poner nombre de escena (ej: "MenuPrincipal")
- **Continue Key:** Space (tecla para continuar)

---

### PASO 6: Agregar la Escena al Build Settings

1. Ve a **File** → **Build Settings**
2. Arrastra la escena `EndingScene` a la lista de **Scenes In Build**
3. Asegúrate de que tenga un índice asignado

---

## 🎮 CÓMO LLAMAR A LA ESCENA DE FINAL

### Opción 1: Desde otro script
```csharp
using UnityEngine.SceneManagement;

// Al completar el juego
SceneManager.LoadScene("EndingScene");
```

### Opción 2: Usando el método estático
```csharp
// Llama directamente al método del EndingSceneController
EndingSceneController.LoadEndingScene("EndingScene");
```

### Opción 3: Botón en UI
1. Crea un botón en tu última escena
2. En el Inspector del botón → **OnClick()**
3. Arrastra un GameObject que tenga un script
4. Selecciona una función que llame a `LoadEndingScene()`

---

## 🎨 ESTRUCTURA FINAL EN HIERARCHY

```
Hierarchy:
  ├── Canvas
  │   ├── BackgroundPanel (Panel negro)
  │   ├── GuionText (TextMeshPro - guion)
  │   └── EndingSceneController (Script)
  └── EventSystem
```

---

## 🎯 CÓMO FUNCIONA

### Al Cargar la Escena:

1. **Lee los contadores:**
   - Cuántas veces eligió "Saber sobre el circo"
   - Cuántas veces eligió "Saber sobre mí"

2. **Determina el final:**
   - Si circo > yo → Muestra `endingCirco`
   - Si yo >= circo → Muestra `endingYo` (incluye empate)

3. **Muestra el texto:**
   - Letra por letra (efecto máquina de escribir)
   - Velocidad configurable

4. **Espera:**
   - Presionar **ESPACIO** para continuar/salir

---

## 🎬 EFECTOS VISUALES ADICIONALES (OPCIONAL)

### Agregar Fade In
1. Crea un nuevo Panel negro sobre todo
2. Agrégale un componente `CanvasGroup`
3. Crea un script que anime el `alpha` de 1 a 0

### Agregar Música
1. En el `Canvas`, agregar `Audio Source`
2. Asignar música de final
3. **Play On Awake:** ✅
4. **Loop:** ✅ (si quieres que se repita)

### Agregar Partículas
1. Click derecho en Hierarchy → **Effects** → **Particle System**
2. Configura partículas sutiles (estrellas, luces, etc.)

---

## 📝 EJEMPLO DE USO

### En tu última escena del juego:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompletionTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // El jugador llegó al final
            Debug.Log("¡Juego completado! Cargando final...");
            
            // Cargar escena de final
            SceneManager.LoadScene("EndingScene");
        }
    }
}
```

---

## 🐛 TROUBLESHOOTING

### El texto no aparece
- ✅ Verifica que `GuionText` esté asignado en el Inspector
- ✅ Verifica que el color del texto sea blanco (no transparente)
- ✅ Verifica que `BackgroundPanel` esté activo

### Los contadores están en 0
- ✅ Asegúrate de que `ChoiceCounterManager` exista en las escenas anteriores
- ✅ Verifica que `DontDestroyOnLoad` esté funcionando
- ✅ Revisa la Console por errores

### La escena no carga
- ✅ Verifica que la escena esté en **Build Settings**
- ✅ Verifica que el nombre coincida exactamente

### El texto va muy rápido/lento
- Ajusta **Typing Speed** en el Inspector
- Valores sugeridos: 20-50 caracteres por segundo

---

## 🎨 PERSONALIZACIÓN AVANZADA

### Cambiar los Textos
Edita los campos de texto en el Inspector del `EndingSceneController`

### Agregar más finales
Modifica el script `EndingSceneController.cs` para agregar condiciones adicionales

### Cambiar el fondo
Cambia el color del `BackgroundPanel` o agrega una imagen de fondo

---

## ✅ CHECKLIST

- [ ] Escena creada y guardada como "EndingScene"
- [ ] Canvas configurado (Screen Space - Overlay)
- [ ] Panel negro creado como hijo del Canvas
- [ ] GuionText creado como hijo del Panel
- [ ] Script `EndingSceneController` agregado al Canvas
- [ ] Referencia GuionText asignada en el Inspector
- [ ] Texto personalizado en el campo "Texto Final"
- [ ] Velocidad ajustada a tu gusto (Text Speed)
- [ ] Probado en Play Mode ▶️

---

**¡Listo!** Ahora tienes una escena de final profesional que se adapta a las elecciones del jugador. 🎉

## 🎮 PARA PROBAR:

1. Crea el `ChoiceCounterManager` en tu escena principal
2. Juega y elige opciones
3. Llama a la escena `EndingScene`
4. Verás el final correspondiente a tus elecciones

---

¿Necesitas ayuda con algo específico de la configuración? 😊
