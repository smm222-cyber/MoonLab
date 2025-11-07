# 📊 SISTEMA DE CONTADORES DE OPCIONES - GUÍA RÁPIDA

## ✅ ¿QUÉ SE CREÓ?

Un sistema simple para contar cuántas veces el jugador elige cada **posición de opción** de forma GLOBAL.

### 🌟 IMPORTANTE - CONTADORES GLOBALES:
Este sistema cuenta por **POSICIÓN**, no por NPC ni por texto:
- **Primera Opción (posición 0) → "Saber sobre el circo":** Suma todas las veces que eligió la 1ra opción de CUALQUIER NPC
- **Segunda Opción (posición 1) → "Saber sobre mí":** Suma todas las veces que eligió la 2da opción de CUALQUIER NPC

**Ejemplo práctico:**
- Payaso tiene: "Preguntar sobre él" (opción 0) y "Preguntar sobre mí" (opción 1)
- Trapecista tiene: "Preguntar sobre ella" (opción 0) y "Preguntar sobre mí" (opción 1)

Si eliges "Preguntar sobre él" del Payaso + "Preguntar sobre ella" de Trapecista → **"Saber sobre el circo": 2 veces**

### Archivos Creados:
1. **ChoiceCounterManager.cs** - Administrador central de contadores
2. **ChoiceCounterDisplay.cs** - Muestra los contadores en pantalla
3. **NPCBasicDialog.cs** - Modificado para usar el contador

---

## 🎮 CONFIGURACIÓN EN UNITY

### PASO 1: Crear el GameObject Manager (Una vez en tu escena principal)

1. En la **Hierarchy** → Click derecho → **Create Empty**
2. Renombrar a: `ChoiceCounterManager`
3. **Add Component** → Buscar `ChoiceCounterManager`
4. **¡Listo!** Este objeto persistirá entre escenas automáticamente

---

### PASO 2: Crear el Panel de Contadores en UI

#### 2.1 Crear el Panel
1. En tu **Canvas** → Click derecho → **UI** → **Panel**
2. Renombrar a: `CounterPanel`
3. Configurar posición:
   - **Anchor:** Top-Left (arriba-izquierda)
   - **Width:** 300
   - **Height:** 200
   - **Pos X:** 10
   - **Pos Y:** -10
4. **Color:** Negro semi-transparente (Alpha: 180)

#### 2.2 Agregar Texto
1. Click derecho en `CounterPanel` → **UI** → **Text - TextMeshPro**
2. Renombrar a: `CounterText`
3. Configurar:
   - **Anchor:** Stretch (ambos ejes)
   - **Left, Right, Top, Bottom:** todos en 10 (padding)
   - **Font Size:** 14
   - **Color:** Blanco
   - **Alignment:** Top-Left
   - **Overflow:** Overflow
   - **Wrapping:** Enabled

#### 2.3 Configurar el Script
1. Selecciona `CounterPanel`
2. **Add Component** → `ChoiceCounterDisplay`
3. Arrastra referencias:
   - **Counter Panel:** Arrastra `CounterPanel`
   - **Counter Text:** Arrastra `CounterText`
4. Configuración:
   - **Update Interval:** 1 (actualiza cada segundo)
   - **Hide Zero Counters:** ✅ Marcar
   - **Toggle Key:** Tab (para mostrar/ocultar)
   - **Start Visible:** ✅ Marcar (o desmarcar si quieres que inicie oculto)

---

## 🎯 ¿CÓMO FUNCIONA?

### Contadores Globales
El sistema cuenta cuántas veces se elige cada **posición de opción** de forma GLOBAL, sin importar el NPC.

### Sistema de Contadores:
- **Opción 0 → "Saber sobre el circo":** Cuenta TODAS las veces que se eligió la primera opción de CUALQUIER NPC
- **Opción 1 → "Saber sobre mí":** Cuenta TODAS las veces que se eligió la segunda opción de CUALQUIER NPC

**Ejemplo:**
- Si eliges la **primera opción** del Payaso → Contador "Saber sobre el circo" +1
- Si eliges la **primera opción** de la Trapecista → Contador "Saber sobre el circo" +1 (suma al mismo)
- Si eliges la **segunda opción** del Payaso → Contador "Saber sobre mí" +1
- Si eliges la **segunda opción** de Pintacaritas → Contador "Saber sobre mí" +1 (suma al mismo)

### ⭐ Importante:
**NO importa el texto de la opción en el diálogo**, solo importa su **posición** (primera o segunda)

---

## 📺 LO QUE VERÁS EN PANTALLA

```
📊 CONTADORES GLOBALES

• Saber sobre el circo: 8 veces
• Saber sobre mí: 5 veces
```

Esto significa:
- Se eligió la 1ra opción (sobre el circo) de CUALQUIER NPC: 8 veces en total
- Se eligió la 2da opción (sobre mí) de CUALQUIER NPC: 5 veces en total

---

## 🎮 CONTROLES

- **Tab** (por defecto): Mostrar/Ocultar el panel de contadores
- Puedes cambiar la tecla en el Inspector

---

## 🔧 FUNCIONES AVANZADAS

### Ver Contadores en Consola
1. Selecciona `ChoiceCounterManager` en la Hierarchy
2. En el Inspector, click derecho en el script
3. **Mostrar Todos los Contadores**
4. Verás todos los contadores en la Console

### Reiniciar Contadores (Por Script)
```csharp
// Reiniciar un contador específico
ChoiceCounterManager.Instance.ResetChoice("SaberSobreElCirco"); // Reinicia "Saber sobre el circo"
ChoiceCounterManager.Instance.ResetChoice("SaberSobreMi"); // Reinicia "Saber sobre mí"

// Reiniciar todos
ChoiceCounterManager.Instance.ResetAllCounters();

// Obtener el valor de un contador
int circo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreElCirco");
int yo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreMi");
```

---

## 📍 PARA EL TRAPECISTA

Si tu Trapecista usa **NPCBasicDialog** con opciones, el sistema funcionará automáticamente.

### Verificar que el Trapecista tenga opciones:
1. Selecciona el GameObject del Trapecista
2. Busca el componente `NPCBasicDialog`
3. Ve a **Missions** → Expande una misión
4. Marca **Has Choices** ✅
5. Agrega opciones en la lista **Choices**

---

## ✨ EJEMPLO DE CONFIGURACIÓN PARA TRAPECISTA

```
NPC Name: "Trapecista"

Mission 0:
  - Dialogue Text: "¿Puedes ayudarme a coser mi tela?"
  - Has Choices: ✅
  - Choices:
      Choice 0:  ← Esta es la PRIMERA OPCIÓN (posición 0)
        - Choice Text: "Claro, te ayudo"
        - Response Dialogue: "¡Gracias! Vamos al taller"
        
      Choice 1:  ← Esta es la SEGUNDA OPCIÓN (posición 1)
        - Choice Text: "Ahora no puedo"
        - Response Dialogue: "Está bien, vuelve luego"
```

### Lo mismo para Payaso:
```
NPC Name: "Payaso"

Mission 0:
  - Has Choices: ✅
  - Choices:
      Choice 0:  ← También es PRIMERA OPCIÓN
        - Choice Text: "Preguntarle sobre mi misión"
        
      Choice 1:  ← También es SEGUNDA OPCIÓN
        - Choice Text: "Hablar del clima"
```

### Resultado:
Cuando el jugador elija opciones, se sumarán al mismo contador:

**Si elige:**
1. "Claro, te ayudo" de Trapecista (posición 0) → Saber sobre el circo +1
2. "Preguntarle sobre mi misión" de Payaso (posición 0) → Saber sobre el circo +1
3. "Ahora no puedo" de Trapecista (posición 1) → Saber sobre mí +1

Verás:
```
📊 CONTADORES GLOBALES
• Saber sobre el circo: 2 veces
• Saber sobre mí: 1 veces
```

---

## 🐛 TROUBLESHOOTING

### Los contadores no aumentan
- ✅ Verifica que `ChoiceCounterManager` exista en la escena
- ✅ Verifica que el NPC use `NPCBasicDialog`
- ✅ Verifica que las opciones estén configuradas
- ✅ Revisa la Console por errores

### El panel no se muestra
- ✅ Verifica que `CounterPanel` esté activo
- ✅ Verifica que las referencias en `ChoiceCounterDisplay` estén asignadas
- ✅ Presiona Tab para mostrarlo/ocultarlo

### Los nombres se ven raros
- Los nombres ahora aparecen como "Saber sobre el circo" y "Saber sobre mí"
- El sistema cuenta por POSICIÓN (opción 0 y opción 1), pero muestra nombres amigables

---

## 🚀 PERSONALIZACIÓN

### Cambiar la posición del panel
Ajusta el **Anchor** y posición del `CounterPanel` en el Inspector

### Cambiar el estilo del texto
Modifica el `CounterText`:
- **Font:** Cambia la fuente
- **Font Size:** Ajusta el tamaño
- **Color:** Cambia el color

### Cambiar la tecla para mostrar/ocultar
En `ChoiceCounterDisplay` → **Toggle Key** → Elige otra tecla

---

## 💾 PERSISTENCIA

Los contadores persisten automáticamente entre escenas (gracias al `DontDestroyOnLoad`).

Si reinicias el juego, los contadores se reinician.

Para guardar permanentemente, necesitarás implementar un sistema de guardado (PlayerPrefs o JSON).

---

## 📝 RESUMEN RÁPIDO

1. ✅ Crear `ChoiceCounterManager` en la escena
2. ✅ Crear UI con panel y texto
3. ✅ Agregar `ChoiceCounterDisplay` al panel
4. ✅ Asignar referencias
5. ✅ ¡Jugar y ver los contadores!

---

**¡Listo!** Ahora cada vez que elijas una opción en cualquier NPC (Pintacaritas, Trapecista, etc.), verás el contador aumentar automáticamente. 🎉
