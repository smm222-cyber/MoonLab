# Configuración del Sistema de Cambio de Escena - NPCs

## 📋 Descripción General

El script `NPCBasicDialog.cs` soporta cambiar automáticamente de escena después de que el jugador complete los diálogos y elija opciones. Esto permite transiciones fluidas entre niveles y escenas.

**Este sistema funciona con cualquier NPC que use el componente `NPCBasicDialog`.**

---

## 🎯 Configuración por Opciones de Diálogo

Cuando un NPC tiene diálogos con opciones para el jugador, cada opción puede llevar a una escena diferente.

### ⚙️ Cómo Configurar Cada Opción:

En el Inspector de Unity, dentro del componente `NPCBasicDialog`:

1. **Buscar la sección de Misiones** (Missions)
2. **Expandir la misión que tiene opciones de diálogo**
3. **Dentro de `Dialogue Choices`, seleccionar una opción**
4. **Configurar los siguientes campos:**

#### Campos de la Opción:

| Campo | Descripción | Ejemplo |
|-------|-------------|---------|
| **Choice Text** | El texto que verá el jugador | "Ayudar al NPC" |
| **Response Dialogue** | Lo que dice el NPC después de elegir | "Gracias por tu ayuda..." |
| **Load Scene After Choice** | ✅ Marcar si quieres cambiar de escena | `true` |
| **Scene To Load** | Nombre EXACTO de la escena a cargar | `"Nivel2"` |
| **Delay Before Scene Change** | Segundos de espera antes de cambiar | `2.0` |

---

## ⚠️ CONFIGURACIÓN REQUERIDA: Cambio de Escena

### 🎯 ¿Dónde Cambiar?

**En el Inspector de Unity:**
1. Selecciona el GameObject del NPC
2. Busca el componente `NPCBasicDialog`
3. Expande la sección **"Missions"**
4. Selecciona la misión que tiene opciones de diálogo
5. Dentro de **"Dialogue Choices"**, expande cada opción

### 📝 ¿Qué Cambiar?

Para **CADA OPCIÓN** (Opción 0, Opción 1, etc.), debes configurar:

| Campo | Valor que DEBES poner |
|-------|----------------------|
| **Load Scene After Choice** | ✅ **MARCADO** (activar checkbox) |
| **Scene To Load** | **Nombre del siguiente nivel** |
| **Delay Before Scene Change** | `2.0` (o el tiempo que prefieras) |

### 🚨 IMPORTANTE: AMBAS Opciones Deben Tener LA MISMA Escena

**Sí, aunque parezca raro, ambas opciones deben llevar al mismo nivel.**

#### ¿Por qué?

Porque en este caso, las opciones solo cambian el **diálogo** que escucha el jugador, pero el **destino final es el mismo**. Es como elegir diferentes conversaciones, pero todos los caminos llevan al mismo siguiente nivel.

#### Ejemplo Correcto:

```
Opción 0 ("Ayudar al NPC"):
  ✅ Load Scene After Choice: MARCADO
  📝 Scene To Load: "Nivel2"
  ⏱️ Delay Before Scene Change: 2.0
  
Opción 1 ("Rechazar ayuda"):
  ✅ Load Scene After Choice: MARCADO
  📝 Scene To Load: "Nivel2"          ← ¡LA MISMA ESCENA!
  ⏱️ Delay Before Scene Change: 2.0
```

### 📌 ¿Qué Nombre de Escena Debo Poner?

El nombre depende del nivel actual:

| Si estás en esta escena | Debes poner este nombre |
|------------------------|------------------------|
| Tutorial | `"Nivel1"` |
| Nivel1 | `"Nivel2"` |
| Nivel2 | `"Nivel3"` |
| Pintacaritas_Level | El siguiente nivel de tu juego |

**💡 Tip:** Para saber los nombres exactos de las escenas:
1. Ve a `File > Build Settings`
2. Busca tu escena en la lista
3. Copia el nombre exactamente como aparece (con mayúsculas/minúsculas)

---## 🎬 Configuración de Cambio de Escena por Misión

También puedes configurar que se cambie de escena al completar una misión completa (sin importar la opción elegida).

### ⚙️ Cómo Configurar el Cambio por Misión:

En el Inspector de Unity, dentro del componente `NPCBasicDialog`:

1. **Buscar la sección de Misiones** (Missions)
2. **Expandir la misión que quieres que cambie de escena**
3. **Configurar los siguientes campos:**

| Campo | Descripción | Ejemplo |
|-------|-------------|---------|
| **Load Scene After Dialog** | ✅ Marcar para cambiar de escena después de completar esta misión | `true` |
| **Scene To Load After Dialog** | Nombre de la escena a cargar | `"Nivel2"` |
| **Delay Before Scene Change** | Segundos de espera | `2.0` |

---

## 🔄 Flujo de Ejecución

### Así funciona el cambio de escena:

1. Jugador habla con el NPC
2. NPC presenta opciones → "Opción 0" y "Opción 1"
3. Jugador elige **Opción 0** (o cualquier opción)
4. NPC muestra `Response Dialogue` → "Gracias por tu ayuda..."
5. ⏱️ Espera `Delay Before Scene Change` (2 segundos)
6. 🎬 **Carga la escena** configurada (ejemplo: `"Nivel2"`)

**Importante:** No importa qué opción elija el jugador, siempre irá a la misma escena (pero habrá escuchado un diálogo diferente).

---

## 📝 Lista de Verificación (Checklist)

Antes de probar el cambio de escena, verifica:

- [ ] Las escenas están agregadas en **Build Settings** (`File > Build Settings`)
- [ ] Los nombres en `Scene To Load` coinciden EXACTAMENTE con los nombres en Build Settings
- [ ] Si usas opciones con escenas, cada opción tiene una escena DIFERENTE (o solo una tiene escena)
- [ ] El `Delay Before Scene Change` es mayor a 0 (recomendado: 1-3 segundos)
- [ ] Probaste que el diálogo se muestra completamente antes del cambio

---

## 🐛 Solución de Problemas

### Problema: "La escena no cambia"

**Posibles causas:**
1. ❌ La escena no está en Build Settings
2. ❌ El nombre está mal escrito (mayúsculas/minúsculas importan)
3. ❌ Olvidaste marcar `Load Scene After Choice`
4. ❌ El `Delay` es 0 y cambia antes de que termines de leer

**Solución:**
- Ir a `File > Build Settings`
- Arrastrar la escena deseada a la lista
- Copiar el nombre exacto desde allí

### Problema: "Ambas opciones llevan al mismo nivel"

**Respuesta:**
- ✅ **¡Eso es CORRECTO!** Ambas opciones DEBEN llevar al mismo nivel
- Las opciones solo cambian el diálogo, no el destino
- Esto es intencional para que el jugador pueda elegir diferentes conversaciones pero seguir progresando

### Problema: "El cambio es muy rápido / muy lento"

**Solución:**
- Ajustar `Delay Before Scene Change`:
  - Muy rápido: Aumentar a 2-3 segundos
  - Muy lento: Reducir a 1-1.5 segundos

---

## 💡 Ejemplo Completo de Configuración

### Configuración Típica (Ambas Opciones al Mismo Nivel):

```
NPC con Opciones:
  
  Opción 0 ("Pregunta A"):
    ✅ Load Scene After Choice: MARCADO
    📝 Scene To Load: "Nivel2"
    ⏱️ Delay: 2.0
    💬 Response: "Respuesta interesante A..."
  
  Opción 1 ("Pregunta B"):
    ✅ Load Scene After Choice: MARCADO
    📝 Scene To Load: "Nivel2"      ← LA MISMA
    ⏱️ Delay: 2.0
    💬 Response: "Respuesta interesante B..."
```

**Resultado:**
- Si elige Opción 0: Escucha "Respuesta A" → Va a Nivel2
- Si elige Opción 1: Escucha "Respuesta B" → Va a Nivel2
- Ambos jugadores avanzan al mismo nivel, solo cambia la historia que escucharon

---

## 📝 Lista de Verificación (Checklist)

Antes de probar el cambio de escena, verifica:

- [ ] **AMBAS opciones tienen marcado `Load Scene After Choice`**
- [ ] **AMBAS opciones tienen EL MISMO nombre en `Scene To Load`**
- [ ] Las escenas están agregadas en **Build Settings** (`File > Build Settings`)
- [ ] Los nombres en `Scene To Load` coinciden EXACTAMENTE con los nombres en Build Settings
- [ ] El `Delay Before Scene Change` es mayor a 0 (recomendado: 2.0 segundos)
- [ ] Probaste que el diálogo se muestra completamente antes del cambio

---

## 🎓 Resumen

- ✅ **AMBAS opciones DEBEN tener la misma escena configurada**
- ✅ Las opciones solo cambian el diálogo, no el destino
- ✅ El nombre de escena debe ser el siguiente nivel (ej: "Nivel2", "Nivel3")
- ✅ Asegúrate de que las escenas estén en Build Settings
- ✅ Los nombres deben coincidir EXACTAMENTE (mayúsculas y minúsculas importan)

---

## 📚 Archivos Relacionados

- **Script Principal:** `Assets/02_Scripts/NPCBasicDialog.cs`
- **Sistema de Opciones:** `DialogueChoicesUIFixed.cs`
- **Contador de Opciones:** `ChoiceCounterManager.cs`

---

**Fecha:** Noviembre 2025  
**Versión:** 2.0  
**Autor:** GitHub Copilot  
**Proyecto:** MoonLab

**Autor:** GitHub Copilot  
**Proyecto:** MoonLab
