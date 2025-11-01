# 🎯 CONFIGURACIÓN FINAL - NIVEL PINTACARITAS

## 📋 FLUJO COMPLETO DE MISIONES

1. **Hablas con GRUPO** → Te dan "Averiguar qué está pasando"
2. **Hablas con TRAPECISTA** → Completa "Averiguar qué está pasando" ✅ + Te da "Preguntar por la pelea"
3. **Hablas con GRUPO (2da vez)** → Completa "Preguntar por la pelea" ✅ + Te da "Escuchar la historia completa"
4. **Hablas con PINTACARITAS2 (1ra vez)** → Completa "Escuchar la historia completa" ✅ + Te da "Buscar el pincel de la pintacaritas"
5. **Agarras el PINCEL** → Completa "Buscar el pincel de la pintacaritas" ✅
6. **Hablas con PINTACARITAS2 (2da vez)** → Te da "Buscar las pinturas de la pintacaritas"
7. **Agarras las PINTURAS** → Completa "Buscar las pinturas de la pintacaritas" ✅
8. **Hablas con PINTACARITAS2 (3ra vez)** → Te da "Hablar con Héctor el Pintacaritas"
9. **Hablas con PINTACARITAS1 (del grupo)** → Completa "Hablar con Héctor el Pintacaritas" ✅

---

## 🎮 CONFIGURACIÓN EN UNITY INSPECTOR

### 1️⃣ GRUPO (Pintacaritas1 + Maestro de Ceremonias)

**GameObject:** `Grupo_Pintacaritas1_Maestro`

**Componentes:**
- Circle Collider 2D (Is Trigger ✅)
- Script: `NPCGroupConversation`
- Layer: `Interactable`

**Campos del Script:**

```
Group Name: Grupo Pintacaritas
Group Icon: (sprite del grupo o uno de ellos)
Interact UI: (tu indicador E)

[CONVERSACIÓN INICIAL]
Initial Conversation (tamaño: 3):
  Element 0:
    Speaker Name: Pintacaritas1
    Speaker Sprite: (sprite de Pintacaritas1)
    Dialogue: "¡No puede ser! ¿Por qué me dijo eso?"
  
  Element 1:
    Speaker Name: Maestro
    Speaker Sprite: (sprite del Maestro)
    Dialogue: "Cálmate, Héctor..."
  
  Element 2:
    Speaker Name: Pintacaritas1
    Speaker Sprite: (sprite de Pintacaritas1)
    Dialogue: "¡No me voy a calmar! ¡Es increíble!"

[CONVERSACIÓN DESPUÉS DE MISIÓN]
Mission Required: Preguntar por la pelea
Mission To Complete When Showing After: Preguntar por la pelea

After Mission Conversation (tamaño: 4):
  Element 0:
    Speaker Name: Maestro
    Speaker Sprite: (sprite del Maestro)
    Dialogue: "Ah, regresaste. Déjame explicarte toda la situación..."
  
  Element 1:
    Speaker Name: Pintacaritas1
    Speaker Sprite: (sprite de Pintacaritas1)
    Dialogue: "Resulta que me dijeron que ya no era necesario..."
  
  Element 2:
    Speaker Name: Maestro
    Speaker Sprite: (sprite del Maestro)
    Dialogue: "Pero luego nos enteramos de que fue un malentendido."
  
  Element 3:
    Speaker Name: Pintacaritas1
    Speaker Sprite: (sprite de Pintacaritas1)
    Dialogue: "Ahora está todo aclarado. ¡Gracias por tu ayuda!"

[CONVERSACIÓN FINAL - HÉCTOR]
Mission Required For Final Conversation: Hablar con Héctor el Pintacaritas

Final Conversation (tamaño: 2):
  Element 0:
    Speaker Name: Pintacaritas1
    Speaker Sprite: (sprite de Pintacaritas1)
    Dialogue: "¡Hola de nuevo! Gracias por ayudar a mi compañera con todo."
  
  Element 1:
    Speaker Name: Pintacaritas1
    Speaker Sprite: (sprite de Pintacaritas1)
    Dialogue: "Realmente lo apreciamos. Ahora todo está listo para el show. ¡Eres increíble!"

[SISTEMA DE MISIONES]
Mission To Give After Initial: Averiguar qué está pasando
Mission To Complete When Showing After: Preguntar por la pelea
Mission To Give After Second: Escuchar la historia completa
Mission To Complete In Final Conversation: Hablar con Héctor el Pintacaritas

[AUDIO]
Typing Sound: (tu sonido de escritura)

[CONTROL DE ACTIVACIÓN]
Is Active: ✅
```

**⚠️ IMPORTANTE:** El mismo GameObject del GRUPO ahora maneja 3 conversaciones:
1. **Conversación Inicial**: Primera vez que hablas con ellos
2. **Conversación Después de Misión**: Cuando tienes "Preguntar por la pelea"
3. **Conversación Final**: Cuando tienes "Hablar con Héctor el Pintacaritas" ✨ NUEVO

---



### 2️⃣ TRAPECISTA

**GameObject:** Tu NPC Trapecista existente

**Componentes:**
- Circle Collider 2D (Is Trigger ✅)
- Script: `NPCBasicDialog`
- Layer: `Interactable`

**Campos del Script:**

```
NPC Name: Trapecista
NPC Image: (sprite de la Trapecista)
Interact UI: (tu indicador E)

[SISTEMA DE MISIONES]
Uses Mission System: ✅

Missions (tamaño: 1):
  Element 0:
    Dialogue Text: "¡Ay! Viste la pelea en el pasillo? Entre Héctor el Pintacaritas y el Maestro de Ceremonias. Fue terrible... Héctor estaba muy molesto por algo. Si quieres saber más, pregúntale a él sobre lo que pasó."
    Mission Required: Averiguar qué está pasando
    Mission To Complete: Averiguar qué está pasando
    Mission To Give: Preguntar por la pelea

[DIÁLOGO SIMPLE]
(Dejar vacío - no se usa porque Uses Mission System está activado)

Max Characters Per Page: 40
Typing Sound: (tu sonido de escritura)
```

---

### 3️⃣ PINTACARITAS 2

**GameObject:** Tu NPC Pintacaritas2 existente

**Componentes:**
- Circle Collider 2D (Is Trigger ✅)
- Script: `PintacaritasNPC`
- Layer: `Interactable`

**Campos del Script:**

```
NPC Name: Pintacaritas 2
NPC Image: (sprite de ella)
Interact UI: (tu indicador E)

[FASE 1: DIÁLOGO INICIAL]
Dialogo Sin Hablar Con Pintacaritas1: "Hola... Estoy ocupada, vuelve más tarde."

[FASE 2: PRIMER DIÁLOGO LARGO]
Mision Requerida Fase2: Escuchar la historia completa
Dialogo Fase2: "¡Hola! Ya me contaron todo. Héctor y el Maestro ya arreglaron sus problemas. Todo está bien ahora. ¡Gracias por tu ayuda! Ah, por cierto, ¿podrías traerme mi pincel? Lo dejé en el camerino."
Mision A Completar Fase2: Escuchar la historia completa
Mision A Dar Fase2: Buscar el pincel de la pintacaritas

[FASE 3: DESPUÉS DEL PRIMER OBJETO]
Mision Requerida Fase3: Buscar el pincel de la pintacaritas
Dialogo Fase3: "¡Gracias por traerme el pincel! Ah, y también necesito mis pinturas especiales. ¿Podrías buscarlas? Están en el almacén."
Mision A Dar Fase3: Buscar las pinturas de la pintacaritas

[FASE 4: DESPUÉS DEL SEGUNDO OBJETO]
Mision Requerida Fase4: Buscar las pinturas de la pintacaritas
Dialogo Fase4: "¡Perfecto! Ya tengo todo lo que necesito. Ahora deberías ir a hablar con Héctor, seguro quiere agradecerte personalmente."
Mision A Dar Fase4: Hablar con Héctor el Pintacaritas

[FASE 5: DIÁLOGO FINAL]
Dialogo Final: "Todo está listo. Gracias por tu ayuda, ¡eres increíble!"

Max Characters Per Page: 40
Typing Sound: (tu sonido de escritura)
```

---

## ✅ CHECKLIST DE CONFIGURACIÓN

### Scripts
- [x] NPCGroupConversation.cs creado
- [x] PintacaritasNPC.cs creado
- [x] NPCBasicDialog.cs actualizado con sistema de completar misiones

### GameObjects
- [ ] Grupo_Pintacaritas1_Maestro creado con NPCGroupConversation
- [ ] Trapecista tiene NPCBasicDialog
- [ ] Pintacaritas2 tiene PintacaritasNPC
- [ ] Todos tienen Circle Collider 2D (Is Trigger ✅)
- [ ] Todos están en Layer "Interactable"

### Configuración de Misiones
- [ ] Grupo: Mission To Give After Initial = "Averiguar qué está pasando"
- [ ] Grupo: Mission Required = "Preguntar por la pelea"
- [ ] Grupo: Mission To Complete When Showing After = "Preguntar por la pelea"
- [ ] Grupo: Mission To Give After Second = "Escuchar la historia completa"
- [ ] Grupo: Mission Required For Final Conversation = "Hablar con Héctor el Pintacaritas"
- [ ] Grupo: Mission To Complete In Final Conversation = "Hablar con Héctor el Pintacaritas"
- [ ] Grupo: Final Conversation configurada con 2+ líneas de diálogo
- [ ] Trapecista: Mission Required = "Averiguar qué está pasando"
- [ ] Trapecista: Mission To Complete = "Averiguar qué está pasando"
- [ ] Trapecista: Mission To Give = "Preguntar por la pelea"
- [ ] Pintacaritas2 FASE 2: Mision Requerida = "Escuchar la historia completa"
- [ ] Pintacaritas2 FASE 2: Mision A Completar = "Escuchar la historia completa"
- [ ] Pintacaritas2 FASE 2: Mision A Dar = "Buscar el pincel de la pintacaritas"
- [ ] Pintacaritas2 FASE 3: Mision Requerida = "Buscar el pincel de la pintacaritas"
- [ ] Pintacaritas2 FASE 3: Mision A Dar = "Buscar las pinturas de la pintacaritas"
- [ ] Pintacaritas2 FASE 4: Mision Requerida = "Buscar las pinturas de la pintacaritas"
- [ ] Pintacaritas2 FASE 4: Mision A Dar = "Hablar con Héctor el Pintacaritas"

### Diálogos
- [ ] Grupo: 3 líneas en Initial Conversation
- [ ] Grupo: 4 líneas en After Mission Conversation
- [ ] Grupo: 2+ líneas en Final Conversation (diálogo de Héctor)
- [ ] Trapecista: 1 misión con diálogo largo
- [ ] Pintacaritas2: 5 diálogos diferentes (inicial, fase 2, fase 3, fase 4, final)

### Objetos Interactivos
- [ ] Crear objeto "Pincel" en tu sistema de items
  - Misión asociada: "Buscar el pincel de la pintacaritas"
- [ ] Crear objeto "Pinturas" en tu sistema de items
  - Misión asociada: "Buscar las pinturas de la pintacaritas"



---

## 🐛 DEBUGGING

Si algo no funciona, verifica:

1. **GameManager existe en la escena** y tiene DontDestroyOnLoad
2. **Los nombres de las misiones** son EXACTAMENTE iguales en todos lados (copia y pega para evitar errores de tipeo)
3. **Los Colliders** están en Is Trigger y el player tiene tag "Player"
4. **La capa Interactable** existe y está asignada
5. Revisa la **Consola de Unity** - los scripts tienen mensajes de debug

### Mensajes de Debug Útiles

En la consola verás mensajes como:
- "GameManager.Instance es null" → GameManager no existe o no se inicializó
- "Misión [nombre] agregada" → La misión se agregó correctamente
- "Misión [nombre] completada" → La misión se completó correctamente

---

## 📝 NOMBRES DE MISIONES (COPIAR Y PEGAR)

Para evitar errores de tipeo, copia estos nombres exactamente:

```
Averiguar qué está pasando
Preguntar por la pelea
Escuchar la historia completa
Buscar el pincel de la pintacaritas
Buscar las pinturas de la pintacaritas
Hablar con Héctor el Pintacaritas
```

---

## 🎬 FLUJO VISUAL

```
┌─────────────────────────────────────────────────────┐
│  INICIO                                             │
└───────────────────┬─────────────────────────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │   HABLAR CON GRUPO (1ra vez) │
    │   Da: "Averiguar qué está     │
    │        pasando"               │
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │   HABLAR CON TRAPECISTA       │
    │   Completa: "Averiguar qué    │
    │             está pasando" ✅   │
    │   Da: "Preguntar por la pelea"│
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │   HABLAR CON GRUPO (2da vez)  │
    │   Completa: "Preguntar por    │
    │             la pelea" ✅       │
    │   Da: "Escuchar la historia   │
    │        completa"              │
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │HABLAR CON PINTACARITAS2 (1ra)│
    │   Completa: "Escuchar la      │
    │             historia          │
    │             completa" ✅       │
    │   Da: "Buscar el pincel de    │
    │        la pintacaritas"       │
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │   AGARRAR OBJETO (PINCEL)     │
    │   Completa: "Buscar el pincel │
    │             de la             │
    │             pintacaritas" ✅   │
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │HABLAR CON PINTACARITAS2 (2da)│
    │   Da: "Buscar las pinturas    │
    │        de la pintacaritas"    │
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │  AGARRAR OBJETO (PINTURAS)    │
    │   Completa: "Buscar las       │
    │             pinturas de la    │
    │             pintacaritas" ✅   │
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │HABLAR CON PINTACARITAS2 (3ra)│
    │   Da: "Hablar con Héctor el   │
    │        Pintacaritas"          │
    └───────────────┬───────────────┘
                    │
                    ▼
    ┌───────────────────────────────┐
    │ HABLAR CON PINTACARITAS1      │
    │          (HÉCTOR)             │
    │   Completa: "Hablar con       │
    │             Héctor el         │
    │             Pintacaritas" ✅   │
    └───────────────┬───────────────┘
                    │
                    ▼
            ┌───────────────┐
            │   ¡NIVEL      │
            │   COMPLETADO! │
            └───────────────┘
```

---

## 🔧 CAMBIOS REALIZADOS EN LOS SCRIPTS

### NPCBasicDialog.cs
- ✅ Agregado campo `missionToComplete` a la clase NPCMission
- ✅ Cambiada lógica para buscar misión basada en `missionRequired`
- ✅ Ahora completa misiones ANTES de dar nuevas misiones
- ✅ Usa coroutina `HandleMissionsAfterDialog`

### NPCGroupConversation.cs
- ✅ Agregado campo `missionToCompleteWhenShowingAfter`
- ✅ Agregado campo `missionToGiveAfterSecond`
- ✅ Agregada TERCERA CONVERSACIÓN: `finalConversation` para diálogo de Héctor
- ✅ Agregado campo `missionRequiredForFinalConversation`
- ✅ Agregado campo `missionToCompleteInFinalConversation`
- ✅ Completa misiones ANTES de mostrar las conversaciones
- ✅ Sistema de prioridades: Final → Después de misión → Inicial

### PintacaritasNPC.cs
- ✅ Agregado campo `misionADar` para dar misiones después del diálogo
- ✅ Agregado flag `misionYaDada` para evitar dar la misión múltiples veces
- ✅ Mejorada función `DeterminarDialogo()` con 3 estados:
  - Sin misión requerida → Diálogo corto
  - Con misión requerida → Diálogo largo (completa y da misión)
  - Después de dar misión → Diálogo final (repetible)
- ✅ Usa coroutina `HandleMissionsDespuesDelDialogo`

---

## 🎯 CONFIGURAR LOS OBJETOS COLECCIONABLES

Ya tienes un sistema de items que maneja la lógica de recoger objetos cuando son parte de una misión. Solo necesitas:

### Configurar los Objetos en tu Sistema de Items

1. **Crea el item "Pincel"** en tu sistema
   - **Misión asociada**: `Buscar el pincel de la pintacaritas`

2. **Crea el item "Pinturas"** en tu sistema
   - **Misión asociada**: `Buscar las pinturas de la pintacaritas`

Tu sistema de items ya debería completar automáticamente las misiones cuando el jugador recoja los objetos, siempre y cuando tenga la misión activa.

### Verificación (Solo si necesitas ajustar tu código)

Si necesitas agregar o verificar el código en tu sistema de items, asegúrate de que cuando se recojan los objetos, se llame a:

```csharp
// Para el pincel:
GameManager.Instance.CompleteMission("Buscar el pincel de la pintacaritas");

// Para las pinturas:
GameManager.Instance.CompleteMission("Buscar las pinturas de la pintacaritas");
```

**⚠️ IMPORTANTE:** Los nombres de las misiones deben ser **exactamente** iguales en todos lados. Usa copiar y pegar de la sección "NOMBRES DE MISIONES" arriba.

---

¡Listo! Ahora solo falta que configures todo en Unity siguiendo esta guía paso a paso. 🚀
