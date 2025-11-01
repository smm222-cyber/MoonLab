# 🎨 DIAGRAMA VISUAL DEL NIVEL PINTACARITAS

```
┌─────────────────────────────────────────────────────────────────┐
│                    INICIO DEL NIVEL                              │
│                   Pintacaritas_Level                             │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│  DIÁLOGO AUTOMÁTICO (GameManager - ya configurado)              │
│  Protagonista: "No vi a ninguna de las pintacaritas..."         │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│                  JUGADOR EXPLORA EL NIVEL                        │
│                                                                   │
│  [Pintacaritas1 + Maestro]  [Trapecista]  [Pintacaritas2]      │
└─────────────┬────────────────────┬────────────────┬─────────────┘
              │                    │                │
              │                    │                │
┌─────────────▼────────────────────────────────────────────────────┐
│  OPCIÓN 1: Jugador presiona E en GRUPO                           │
│  (Pintacaritas1 + Maestro de Ceremonias)                         │
└──────────────────────┬───────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│  CONVERSACIÓN ENTRE ELLOS:                                       │
│  ┌──────────────────────────────────────────────────────┐       │
│  │ Maestro: "¡Necesito que pinten más caras hoy!"      │       │
│  │ Pintacaritas1: "Mi compañera no está..."            │       │
│  │ Maestro: "¡Encuéntrala rápido!"                     │       │
│  └──────────────────────────────────────────────────────┘       │
│                                                                   │
│  ⭐ Misión Agregada: "HablasteCon_Pintacaritas1"                │
└──────────────────────┬───────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│  Jugador va con la TRAPECISTA y presiona E                       │
└──────────────────────┬───────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│  DIÁLOGO TRAPECISTA:                                             │
│  "¿Has visto a las pintacaritas?"                                │
│  "Una de ellas me debe pintar algo especial..."                  │
│                                                                   │
│  ⭐ Misión Agregada: "HablasteCon_Trapecista"                   │
│  ✅ Misión Completada: "HablasteCon_Pintacaritas1"              │
└──────────────────────┬───────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│  Jugador VUELVE con el GRUPO y presiona E                        │
└──────────────────────┬───────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│  NUEVA CONVERSACIÓN (porque ya tiene la misión):                 │
│  ┌──────────────────────────────────────────────────────┐       │
│  │ Pintacaritas1: "¡Hola! Gracias por tu ayuda."       │       │
│  │ Pintacaritas1: "Si necesitas algo, háblame."        │       │
│  └──────────────────────────────────────────────────────┘       │
└───────────────────────────────────────────────────────────────────┘


┌─────────────────────────────────────────────────────────────────┐
│  EN CUALQUIER MOMENTO: Jugador habla con PINTACARITAS 2          │
└──────────────────────┬────────┬──────────────────────────────────┘
                       │        │
        ┌──────────────┘        └──────────────┐
        │                                      │
        ▼                                      ▼
┌───────────────────┐              ┌────────────────────────┐
│ SI NO HABLÓ CON   │              │ SI YA HABLÓ CON        │
│ PINTACARITAS 1:   │              │ PINTACARITAS 1:        │
│                   │              │                        │
│ "Estoy ocupada    │              │ "¡Mi compañera te      │
│  ahora..."        │              │  envió! Ya voy."       │
└───────────────────┘              └────────────────────────┘
```

---

## 📊 ESTADO DE LAS MISIONES

```
┌──────────────────────────────────────────────────────────────┐
│  INICIO DEL NIVEL                                             │
│  ┌────────────────────────────────────────────────┐          │
│  │ HablasteCon_Pintacaritas1: ❌ No iniciada      │          │
│  │ HablasteCon_Trapecista: ❌ No iniciada         │          │
│  └────────────────────────────────────────────────┘          │
└──────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────┐
│  DESPUÉS DE HABLAR CON EL GRUPO                               │
│  ┌────────────────────────────────────────────────┐          │
│  │ HablasteCon_Pintacaritas1: ✅ ACTIVA           │          │
│  │ HablasteCon_Trapecista: ❌ No iniciada         │          │
│  └────────────────────────────────────────────────┘          │
└──────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────┐
│  DESPUÉS DE HABLAR CON TRAPECISTA                             │
│  ┌────────────────────────────────────────────────┐          │
│  │ HablasteCon_Pintacaritas1: ✅ COMPLETADA       │          │
│  │ HablasteCon_Trapecista: ✅ ACTIVA              │          │
│  └────────────────────────────────────────────────┘          │
└──────────────────────────────────────────────────────────────┘
```

---

## 🎯 COMPORTAMIENTO DE CADA NPC

### 👥 GRUPO (Pintacaritas1 + Maestro)

```
┌─────────────────────────────────────────────────────────────┐
│  ESTADO: Misión "HablasteCon_Trapecista" NO activa          │
│  ┌───────────────────────────────────────────────┐          │
│  │  Mostrar: Conversación INICIAL                │          │
│  │  (Maestro y Pintacaritas1 hablando entre sí)  │          │
│  │                                                │          │
│  │  Al terminar → Agregar misión:                │          │
│  │  "HablasteCon_Pintacaritas1"                  │          │
│  └───────────────────────────────────────────────┘          │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  ESTADO: Misión "HablasteCon_Trapecista" ACTIVA             │
│  ┌───────────────────────────────────────────────┐          │
│  │  Mostrar: Conversación ALTERNATIVA             │          │
│  │  (Pintacaritas1 hablando con la protagonista) │          │
│  └───────────────────────────────────────────────┘          │
└─────────────────────────────────────────────────────────────┘
```

### 🤸 TRAPECISTA

```
┌─────────────────────────────────────────────────────────────┐
│  PRIMERA INTERACCIÓN                                         │
│  ┌───────────────────────────────────────────────┐          │
│  │  Mostrar: Diálogo sobre las pintacaritas      │          │
│  │  Agregar misión: "HablasteCon_Trapecista"     │          │
│  └───────────────────────────────────────────────┘          │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  SIGUIENTES INTERACCIONES                                    │
│  ┌───────────────────────────────────────────────┐          │
│  │  Mostrar: Diálogo de agradecimiento           │          │
│  └───────────────────────────────────────────────┘          │
└─────────────────────────────────────────────────────────────┘
```

### 🎨 PINTACARITAS 2

```
┌─────────────────────────────────────────────────────────────┐
│  SI: Misión "HablasteCon_Pintacaritas1" ACTIVA              │
│  ┌───────────────────────────────────────────────┐          │
│  │  Mostrar: Diálogo CORTO                       │          │
│  │  "Estoy ocupada ahora..."                     │          │
│  └───────────────────────────────────────────────┘          │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│  SI: Misión "HablasteCon_Pintacaritas1" COMPLETADA          │
│  ┌───────────────────────────────────────────────┐          │
│  │  Mostrar: Diálogo LARGO                       │          │
│  │  "¡Mi compañera te envió! Ya voy."            │          │
│  └───────────────────────────────────────────────┘          │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔧 CONFIGURACIÓN EN UNITY - RESUMEN VISUAL

```
ESCENA: Pintacaritas_Level
│
├── 📦 GameManager (ya existe)
│
├── 👤 Player (ya existe)
│   └── InteractPlayerItem component
│
├── 👥 Grupo_Pintacaritas1_Maestro [NUEVO]
│   ├── Circle Collider 2D (Is Trigger)
│   ├── Script: NPCGroupConversation
│   │   ├── missionToGiveAfterInitial: "HablasteCon_Pintacaritas1"
│   │   ├── missionRequired: "HablasteCon_Trapecista"
│   │   ├── initialConversation: [3 líneas]
│   │   └── afterMissionConversation: [2 líneas]
│   │
│   └── 💬 Interact_UI (hijo)
│       └── Sprite/Text: "Presiona E"
│
├── 🤸 Trapecista (ya existe)
│   ├── Circle Collider 2D (Is Trigger)
│   ├── Script: NPCBasicDialog
│   │   ├── usesMissionSystem: true
│   │   └── missions: [2 misiones]
│   │
│   └── 💬 Interact_UI (hijo)
│
└── 🎨 Pintacaritas2 (ya existe)
    ├── Circle Collider 2D (Is Trigger)
    ├── Script: PintacaritasNPC
    │   ├── misionPintacaritas1: "HablasteCon_Pintacaritas1"
    │   ├── misionTrapecista: "HablasteCon_Trapecista"
    │   ├── dialogoSinHablarConPintacaritas1: "..."
    │   └── dialogoDespuesDeHablarConPintacaritas1: "..."
    │
    └── 💬 Interact_UI (hijo)
```

---

## ✅ CHECKLIST DE IMPLEMENTACIÓN

### Fase 1: Preparación
- [ ] Leer RESUMEN_CONFIGURACION.md
- [ ] Leer GUIA_PINTACARITAS.md (completa)
- [ ] Verificar que GameManager existe en la escena

### Fase 2: Crear el Grupo
- [ ] Crear GameObject vacío `Grupo_Pintacaritas1_Maestro`
- [ ] Agregar Circle Collider 2D (Is Trigger)
- [ ] Agregar script NPCGroupConversation
- [ ] Configurar conversación inicial (3 líneas)
- [ ] Configurar conversación alternativa (2 líneas)
- [ ] Configurar misiones:
  - [ ] missionToGiveAfterInitial: `HablasteCon_Pintacaritas1`
  - [ ] missionRequired: `HablasteCon_Trapecista`
- [ ] Crear UI hijo "Presiona E"
- [ ] Asignar layer `Interactable`

### Fase 3: Configurar Trapecista
- [ ] Seleccionar GameObject Trapecista
- [ ] Agregar/Verificar Circle Collider 2D (Is Trigger)
- [ ] Agregar script NPCBasicDialog (si no lo tiene)
- [ ] Marcar `usesMissionSystem` = true
- [ ] Configurar 2 misiones
- [ ] Crear UI hijo "Presiona E" (si no lo tiene)
- [ ] Asignar layer `Interactable`

### Fase 4: Configurar Pintacaritas 2
- [ ] Seleccionar GameObject Pintacaritas2
- [ ] Agregar/Verificar Circle Collider 2D (Is Trigger)
- [ ] Agregar script PintacaritasNPC
- [ ] Configurar 2-3 diálogos
- [ ] Configurar nombres de misiones (copiar y pegar)
- [ ] Crear UI hijo "Presiona E" (si no lo tiene)
- [ ] Asignar layer `Interactable`

### Fase 5: Testing
- [ ] Probar hablar con el grupo primero
- [ ] Verificar que se agrega la misión
- [ ] Probar hablar con la trapecista
- [ ] Verificar que cambia el diálogo del grupo
- [ ] Probar Pintacaritas 2 antes y después de hablar con el grupo
- [ ] Verificar que el UI "Presiona E" aparece y desaparece

---

## 🐛 DEBUG

Para debuggear, puedes agregar el script `PintacaritasLevelController` a un GameObject vacío:

1. Crear GameObject vacío: `LevelController`
2. Agregar script: `PintacaritasLevelController`
3. Asignar las referencias de los 3 NPCs
4. Haz click derecho en el script → Ver opciones de debug:
   - Resetear Misiones
   - Completar Misión Pintacaritas1
   - Completar Misión Trapecista
   - Ver Estado Actual

Esto te permite testear diferentes estados sin jugar todo el nivel.

---

¡Todo listo para implementar! 🎉
