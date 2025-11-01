# 🎨 GUÍA DE CONFIGURACIÓN - NIVEL PINTACARITAS

## 📋 Resumen del Sistema

Este nivel tiene 4 NPCs con diálogos encadenados:
1. **Pintacaritas 1** + **Maestro de Ceremonias** (conversación grupal)
2. **Trapecista** (diálogo individual)
3. **Pintacaritas 2** (diálogo que cambia según el progreso)

## 🎯 Flujo de la Historia

```
INICIO
  ↓
Diálogo de introducción (Protagonista)
  ↓
Jugador encuentra a Pintacaritas1 + Maestro (conversación entre ellos)
  ↓
Jugador habla con Trapecista
  ↓
Jugador vuelve con Pintacaritas1 (ahora habla con la prota)
  ↓
Pintacaritas2 cambia su diálogo si ya hablaste con Pintacaritas1
```

---

## 🛠️ CONFIGURACIÓN EN UNITY

### PASO 1: Configurar la Conversación Grupal (Pintacaritas1 + Maestro)

1. **Crear un GameObject vacío** en la escena llamado `Grupo_Pintacaritas1_Maestro`
2. Posicionarlo entre los dos personajes
3. **Agregar componentes:**
   - `Circle Collider 2D` → Marcar como `Is Trigger`
   - Script `NPCGroupConversation`

4. **Configurar el script `NPCGroupConversation`:**

   **Group Name:** "Conversación"
   **Group Icon:** (Sprite genérico o de uno de ellos)
   
   **CONVERSACIÓN INICIAL:**
   - **Línea 1:**
     - Speaker Name: "Maestro de Ceremonias"
     - Speaker Sprite: (Imagen del maestro)
     - Dialogue: "¡Necesito que pinten más caras hoy! ¡El espectáculo debe ser perfecto!"
   
   - **Línea 2:**
     - Speaker Name: "Pintacaritas 1"
     - Speaker Sprite: (Imagen de Pintacaritas1)
     - Dialogue: "Lo sé, lo sé... pero mi compañera aún no llega. No puedo hacerlo sola."
   
   - **Línea 3:**
     - Speaker Name: "Maestro de Ceremonias"
     - Speaker Sprite: (Imagen del maestro)
     - Dialogue: "¡Pues encuéntrala rápido! La función es pronto."

   **CONVERSACIÓN DESPUÉS DE MISIÓN:**
   - Mission Required: `HablasteCon_Trapecista`
   
   - **Línea 1:**
     - Speaker Name: "Pintacaritas 1"
     - Speaker Sprite: (Imagen de Pintacaritas1)
     - Dialogue: "¡Hola! Gracias por tu ayuda. Ahora puedo seguir con mi trabajo."
   
   - **Línea 2:**
     - Speaker Name: "Pintacaritas 1"
     - Speaker Sprite: (Imagen de Pintacaritas1)
     - Dialogue: "Si necesitas algo, háblame cuando quieras."

5. **Crear UI de Interacción:**
   - Crear un hijo de este GameObject con un Sprite o Texto que diga "Presiona E"
   - Asignarlo al campo `Interact UI`

---

### PASO 2: Configurar la Trapecista

1. Seleccionar el GameObject de la **Trapecista**
2. **Agregar/Configurar el script `NPCBasicDialog`**

3. **Configuración:**
   - **NPC Name:** "Trapecista"
   - **NPC Image:** (Sprite de la trapecista)
   - **Uses Mission System:** ✅ (Activar)
   
   **MISIÓN 1:**
   - Dialogue Text: "Hola, ¿has visto a las pintacaritas? Una de ellas me debe pintar algo especial para el show."
   - Mission To Give: `HablasteCon_Trapecista`
   - Mission Required: (Dejar vacío)

   **MISIÓN 2 (Diálogo después de completar):**
   - Dialogue Text: "Gracias por preguntar. Espero que las encuentres."
   - Mission To Give: (Dejar vacío)
   - Mission Required: (Dejar vacío)

4. Agregar:
   - `Circle Collider 2D` → `Is Trigger`
   - UI de interacción (hijo con sprite/texto "Presiona E")

---

### PASO 3: Configurar Pintacaritas 2

1. Seleccionar el GameObject de **Pintacaritas 2**
2. **Agregar el script `PintacaritasNPC`**

3. **Configuración:**
   - **NPC Name:** "Pintacaritas 2"
   - **NPC Image:** (Sprite de Pintacaritas2)
   
   **Diálogos:**
   - **Dialogo Sin Hablar Con Pintacaritas1:** 
     ```
     "Hola... Estoy ocupada ahora. Disculpa."
     ```
   
   - **Dialogo Despues De Hablar Con Pintacaritas1:**
     ```
     "¡Ah! Mi compañera te envió. Estoy aquí, dile que ya voy para allá. Gracias por avisarme."
     ```
   
   - **Dialogo Final:** (Opcional - si quieres un tercer diálogo)
     ```
     "Gracias por tu ayuda. Ahora podemos trabajar juntas."
     ```

   **Sistema de Progreso:**
   - **Mision Pintacaritas1:** `HablasteCon_Pintacaritas1`
   - **Mision Trapecista:** `HablasteCon_Trapecista`

4. Agregar:
   - `Circle Collider 2D` → `Is Trigger`
   - UI de interacción

---

### PASO 4: IMPORTANTE - Sistema de Misiones

Para que todo funcione, necesitas **completar las misiones en el momento correcto**.

#### OPCIÓN A: Completar automáticamente cuando termina el diálogo

En el script `NPCGroupConversation`, agregar al final del método `ShowGroupConversation`:

```csharp
// Después de mostrar toda la conversación, dar una misión
if (!string.IsNullOrEmpty(missionRequired))
{
    manager.AddMission("HablasteCon_Pintacaritas1");
}
```

#### OPCIÓN B: Sistema manual con NPCBasicDialog

Usar el sistema de misiones que ya tienes en `NPCBasicDialog` para la Trapecista.

---

## 🎮 FLUJO FINAL DE MISIONES

1. **Jugador interactúa con Grupo (Pintacaritas1 + Maestro)**
   - Se muestra la conversación inicial
   - Se agrega misión: `HablasteCon_Pintacaritas1`

2. **Jugador interactúa con Trapecista**
   - Se muestra su diálogo
   - Se agrega misión: `HablasteCon_Trapecista`
   - Se completa automáticamente: `HablasteCon_Pintacaritas1`

3. **Jugador vuelve con el Grupo**
   - Como `HablasteCon_Trapecista` está activa, muestra la conversación alternativa

4. **Jugador habla con Pintacaritas 2**
   - Si aún no completó `HablasteCon_Pintacaritas1`: Diálogo corto
   - Si ya la completó: Diálogo largo

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### Problema: El diálogo no cambia
- Verifica que los nombres de las misiones coincidan EXACTAMENTE
- Revisa en el panel de misiones si las misiones se están agregando

### Problema: El UI de interacción no aparece
- Verifica que el GameObject tenga el tag `Player`
- Verifica que el Collider esté marcado como `Is Trigger`
- Verifica que el campo `Interact UI` esté asignado

### Problema: Los NPCs no interactúan
- Verifica que implementen la interfaz `IInteractable`
- Verifica que el jugador tenga el script para detectar la tecla E

---

## 📝 NOTAS ADICIONALES

- Todos los scripts usan el sistema de misiones del GameManager
- Las misiones activas se pueden ver en el panel de misiones
- Puedes ajustar el `maxCharactersPerPage` para controlar cuánto texto aparece por página
- El sistema funciona con el sistema de diálogo existente (typewriter effect)

---

¡Listo! Ahora solo necesitas configurar los GameObjects en Unity siguiendo estos pasos. 🎉
