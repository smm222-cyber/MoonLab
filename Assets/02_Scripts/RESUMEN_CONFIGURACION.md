# 🎯 RESUMEN RÁPIDO - CONFIGURACIÓN PINTACARITAS

## ✅ Scripts Creados

He creado 3 archivos para ti:

1. **NPCGroupConversation.cs** - Para conversaciones entre múltiples NPCs
2. **PintacaritasNPC.cs** - Para las Pintacaritas con diálogos que cambian
3. **GUIA_PINTACARITAS.md** - Guía completa de configuración

---

## 🎮 CONFIGURACIÓN RÁPIDA EN UNITY

### 1️⃣ GRUPO: Pintacaritas1 + Maestro de Ceremonias

**GameObject:** Crear vacío llamado `Grupo_Pintacaritas1_Maestro`

**Componentes:**
- Circle Collider 2D (Is Trigger ✅)
- Script: `NPCGroupConversation`
- Layer: `Interactable` (el que uses para interacción)

**Configuración del Script:**
```
Mission To Give After Initial: Averiguar qué está pasando
Mission Required: Preguntar por la pelea

CONVERSACIÓN INICIAL (3 líneas):
- Maestro → "¡Necesito que pinten más caras hoy!"
- Pintacaritas1 → "Mi compañera aún no llega..."
- Maestro → "¡Pues encuéntrala rápido!"

CONVERSACIÓN DESPUÉS:
- Pintacaritas1 → "¡Hola! Gracias por tu ayuda."
- Pintacaritas1 → "Si necesitas algo, háblame cuando quieras."
```

---

### 2️⃣ TRAPECISTA

**GameObject:** Tu objeto Trapecista existente

**Componentes:**
- Circle Collider 2D (Is Trigger ✅)
- Script: `NPCBasicDialog` (ya existe)
- Layer: `Interactable`

**Configuración del Script:**
```
Uses Mission System: ✅

MISIÓN 1:
- Dialogue: "¡Ay! Viste la pelea en el pasillo? Entre Héctor el Pintacaritas y el Maestro de Ceremonias. Fue terrible... Héctor estaba muy molesto por algo. Si quieres saber más, pregúntale a él sobre lo que pasó."
- Mission Required: Averiguar qué está pasando
- Mission To Complete: Averiguar qué está pasando ✅
- Mission To Give: Preguntar por la pelea

MISIÓN 2 (opcional - para después):
- Dialogue: "Espero que se arreglen las cosas..."
- Mission Required: (vacío)
- Mission To Complete: (vacío)
- Mission To Give: (vacío)
```

---

### 3️⃣ PINTACARITAS 2

**GameObject:** Tu objeto Pintacaritas2 existente

**Componentes:**
- Circle Collider 2D (Is Trigger ✅)
- Script: `PintacaritasNPC` (NUEVO - el que acabo de crear)
- Layer: `Interactable`

**Configuración del Script:**
```
NPC Name: Pintacaritas 2
NPC Image: (Sprite de ella)

Dialogo Sin Hablar Con Pintacaritas1:
"Hola... Estoy ocupada ahora. Disculpa."

Dialogo Despues De Hablar Con Pintacaritas1:
"¡Ah! Mi compañera te envió. Ya voy para allá."

Mision Requerida Para Dialogo Largo: Escuchar la historia completa
Mision A Completar: Escuchar la historia completa
```

---

## 🔄 FLUJO DEL NIVEL

```
1. INICIO
   ↓
2. Diálogo automático (GameManager - ya lo tienes)
   ↓
3. Jugador presiona E en GRUPO
   → Ve conversación entre Maestro y Pintacaritas1
   → Se agrega misión: "Averiguar qué está pasando"
   ↓
4. Jugador presiona E en TRAPECISTA
   → Completa misión: "Averiguar qué está pasando"
   → Se agrega misión: "Preguntar por la pelea"
   ↓
5. Jugador vuelve al GRUPO
   → Completa misión: "Preguntar por la pelea"
   → Ve nueva conversación (Pintacaritas1 habla con jugador)
   → Se agrega misión: "Escuchar la historia completa"
   ↓
6. Jugador presiona E en PINTACARITAS2
   → Completa misión: "Escuchar la historia completa"
   → Si tiene la misión activa: Diálogo largo
   → Si no tiene la misión: Diálogo corto
```

---

## ⚙️ IMPORTANTE - Configuración de Layer

1. Ve a **Edit → Project Settings → Tags and Layers**
2. Asegúrate de tener un layer llamado `Interactable`
3. Asigna ese layer a todos los NPCs
4. En el jugador (InteractPlayerItem), el campo `Interactive Layers` debe tener marcado `Interactable`

---

## 🎨 UI de Interacción

Cada NPC necesita un hijo visual que diga "Presiona E":

**Crear para cada NPC:**
1. Hijo GameObject → "Interact_UI"
2. Agregar Sprite Renderer o TextMeshPro
3. Texto: "Presiona E"
4. Posición: Encima del NPC
5. **Asignar este GameObject al campo `Interact UI` del script**

---

## 🐛 CHECKLIST DE DEBUG

Si algo no funciona:

✅ El jugador tiene el componente `InteractPlayerItem`
✅ Todos los NPCs tienen Collider2D con `Is Trigger` marcado
✅ Todos los NPCs están en el layer `Interactable`
✅ El campo `Interact UI` está asignado en cada script
✅ El GameManager existe en la escena
✅ Los nombres de las misiones son EXACTAMENTE iguales (copia y pega)

---

## 📝 NOMBRES DE MISIONES PARA COPIAR

```
Averiguar qué está pasando
Preguntar por la pelea
Escuchar la historia completa
```

**⚠️ IMPORTANTE:** Copia y pega estos nombres exactamente como están (con acentos y espacios). Si hay un error de tipeo, el sistema no funcionará.

---

¡Eso es todo! Con esto tu nivel debería funcionar perfectamente. 🎉
