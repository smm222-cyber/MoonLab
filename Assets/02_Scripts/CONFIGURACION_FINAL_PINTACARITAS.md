# 🎯 CONFIGURACIÓN FINAL - NIVEL PINTACARITAS

## 📋 FLUJO COMPLETO DE MISIONES

1. **Hablas con GRUPO** → Te dan "Averiguar qué está pasando"
2. **Hablas con TRAPECISTA** → Completa "Averiguar qué está pasando" ✅ + Te da "Preguntar por la pelea"
3. **Hablas con GRUPO (2da vez)** → Completa "Preguntar por la pelea" ✅ + Te da "Escuchar la historia completa"
4. **Hablas con PINTACARITAS2** → Completa "Escuchar la historia completa" ✅

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

[SISTEMA DE MISIONES]
Mission To Give After Initial: Averiguar qué está pasando
Mission To Complete When Showing After: Preguntar por la pelea
Mission To Give After Second: Escuchar la historia completa

[AUDIO]
Typing Sound: (tu sonido de escritura)

[CONTROL DE ACTIVACIÓN]
Is Active: ✅
```

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

[DIÁLOGOS]
Dialogo Sin Hablar Con Pintacaritas1: "Hola... Estoy ocupada, vuelve más tarde."

Dialogo Despues De Hablar Con Pintacaritas1: "Ah, hola otra vez. ¿Necesitas algo? Sigo ocupada..."

Dialogo Final: "¡Hola! Ya me contaron todo. Héctor y el Maestro ya arreglaron sus problemas. Todo está bien ahora. ¡Gracias por tu ayuda!"

[SISTEMA DE MISIONES]
Mision Requerida Para Dialogo Largo: Escuchar la historia completa
Mision A Completar: Escuchar la historia completa

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
- [ ] Trapecista: Mission Required = "Averiguar qué está pasando"
- [ ] Trapecista: Mission To Complete = "Averiguar qué está pasando"
- [ ] Trapecista: Mission To Give = "Preguntar por la pelea"
- [ ] Pintacaritas2: Mision Requerida = "Escuchar la historia completa"
- [ ] Pintacaritas2: Mision A Completar = "Escuchar la historia completa"

### Diálogos
- [ ] Grupo: 3 líneas en Initial Conversation
- [ ] Grupo: 4 líneas en After Mission Conversation
- [ ] Trapecista: 1 misión con diálogo largo
- [ ] Pintacaritas2: 3 diálogos diferentes

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
    │   HABLAR CON PINTACARITAS2    │
    │   Completa: "Escuchar la      │
    │             historia          │
    │             completa" ✅       │
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
- ✅ Completa misiones ANTES de mostrar la conversación posterior
- ✅ Puede dar misiones después de la segunda conversación

### PintacaritasNPC.cs
- ✅ Ya estaba correctamente configurado
- ✅ No se hicieron cambios

---

¡Listo! Ahora solo falta que configures todo en Unity siguiendo esta guía paso a paso. 🚀
