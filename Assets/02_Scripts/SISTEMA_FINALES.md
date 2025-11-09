# 🎭 Sistema de Finales Múltiples - MoonLab

## 📋 Descripción General

El juego tiene un sistema de **finales múltiples** basado en las decisiones del jugador a lo largo de todos los niveles. El sistema acumula puntos según las opciones elegidas y al final decide qué escena final mostrar.

---

## 🎯 ¿Cómo Funciona?

### 1. **Acumulación de Puntos** (Durante TODOS los niveles)

A lo largo del juego, cuando el jugador elige opciones en diálogos, se acumulan puntos en **2 contadores globales**:

| Contador | Descripción | Se incrementa cuando... |
|----------|-------------|------------------------|
| **SaberSobreElCirco** | Puntos "Circo" | El jugador elige opciones relacionadas con el circo |
| **SaberSobreMi** | Puntos "Yo/Protagonista" | El jugador elige opciones relacionadas consigo mismo |

**💾 Importante:** Estos contadores se mantienen entre escenas gracias al `ChoiceCounterManager` que tiene `DontDestroyOnLoad`.

---

### 2. **Decisión del Final** (Solo en el nivel del Maestro de Ceremonias)

Al **final del juego**, en el nivel del **Maestro de Ceremonias**, después de que el jugador complete la última interacción y elija entre las opciones finales:

```
Si SaberSobreElCirco > SaberSobreMi:
    → Cargar "Final2" (Final del Circo)
    
Si SaberSobreMi > SaberSobreElCirco:
    → Cargar "Final1" (Final del Protagonista)
    
Si están empatados:
    → Elegir aleatoriamente entre Final1 o Final2
```

---

## 🔧 Configuración Técnica

### Archivos Clave:

1. **`ChoiceCounterManager.cs`**
   - Acumula los contadores globales
   - Persiste entre escenas con `DontDestroyOnLoad`
   - Método: `IncrementChoice(string choiceID)`

2. **`MissionToEndingChooser.cs`**
   - Decide qué final cargar según los contadores
   - Método: `MissionToEndingChooserHelper.DecideAndLoad()`

3. **`NPCBasicDialog.cs`**
   - Incrementa contadores cuando el jugador elige opciones
   - Llama a `DecideAndLoad()` cuando una opción tiene `triggersFinal: true`

---

## ⚙️ Configuración en Unity

### Para Acumular Puntos (En cualquier nivel):

En el Inspector del NPC con componente `NPCBasicDialog`:

1. Expandir **Missions** → Seleccionar una misión
2. Expandir **Dialogue Choices** → Seleccionar una opción
3. Configurar:
   - **Global Choice ID**: `"SaberSobreElCirco"` o `"SaberSobreMi"`
   - **Triggers Final**: ❌ **NO marcar** (solo acumula puntos)

### Para Activar el Final (Solo en nivel del Maestro):

En el Inspector del **Maestro de Ceremonias**:

1. Expandir **Missions** → Última misión con opciones finales
2. Expandir **Dialogue Choices** → Opciones finales
3. Configurar:
   - **Global Choice ID**: `"SaberSobreElCirco"` o `"SaberSobreMi"` (acumula último punto)
   - **Triggers Final**: ✅ **MARCAR** (esto activa la decisión del final)

### Configurar Nombres de Escenas Finales:

En la escena `MaestroCeremonias_Level.unity`, buscar el GameObject llamado **"EndingResolver"** con componente `MissionToEndingChooser`:

**Cómo encontrarlo:**
1. Abrir la escena `MaestroCeremonias_Level.unity`
2. En la jerarquía (Hierarchy), buscar el GameObject **"EndingResolver"**
3. En el Inspector verás el componente `MissionToEndingChooser`

**Configuración:**

| Campo | Valor Actual | Descripción |
|-------|-------------|-------------|
| **Mission Name** | `"Abrir portal secreto"` | Misión que detecta para activar (legacy, no se usa ahora) |
| **Final If Circo** | `"Final2"` | Escena si gana el contador "SaberSobreElCirco" |
| **Final If Yo** | `"Final1"` | Escena si gana el contador "SaberSobreMi" |
| **Triggered** | `0` (false) | Se marca automáticamente cuando se activa |

---

## 🚨 Errores Comunes y Soluciones

### Error: "El final se activa en el primer nivel"

**Causa:** Algún NPC en un nivel temprano tiene `triggersFinal: true` activado

**Solución:**
- Solo el **Maestro de Ceremonias** en el **último nivel** debe tener opciones con `triggersFinal: true`
- Todos los demás NPCs solo deben **acumular puntos** (tener `Global Choice ID` pero sin `triggersFinal`)

### Error: "El final se activa apenas hablo con el Maestro"

**Causa:** El prefab `NpcMaestroCeremonias.prefab` tiene `triggerFinalAfterDialog: 1`

**Solución:** ✅ Ya arreglado - ahora es `triggerFinalAfterDialog: 0`

### Error: "No se carga ninguna escena final después de elegir"

**Posibles causas:**
1. El GameObject `EndingResolver` tiene `missionName` configurado con una misión que no se activa
2. Las opciones finales no tienen `triggersFinal: true` marcado
3. El orden de las opciones está invertido

**Solución:**

**Opción A - Desactivar el sistema de misiones (RECOMENDADO):**
1. Abrir escena `MaestroCeremonias_Level.unity`
2. Buscar GameObject `EndingResolver` en la jerarquía
3. En el Inspector, componente `MissionToEndingChooser`
4. **Borrar** el texto del campo `Mission Name` (dejarlo vacío)
5. Guardar la escena

**Opción B - Verificar las opciones del Maestro:**
1. En la escena, seleccionar el NPC `Maestro de ceremonias`
2. En el componente `NPCBasicDialog`, expandir `Missions`
3. Buscar las misiones con opciones finales (mission[1] y mission[2])
4. Expandir `Dialogue Choices`
5. Verificar que las opciones tengan:
   - **Opción 0** ("Entregar materiales"): `triggersFinal: true` ✅
   - **Opción 1** ("Abrir portal"): Puede o no tener `triggersFinal: true`
6. **IMPORTANTE:** El orden de las opciones determina qué contador incrementan:
   - **Opción índice 0** → Incrementa `SaberSobreElCirco` (Circo)
   - **Opción índice 1** → Incrementa `SaberSobreMi` (Yo/Protagonista)

### Error: "Siempre va al mismo final"

**Posibles causas:**
1. Los contadores no se están acumulando correctamente
2. El `Global Choice ID` está mal escrito (debe ser exactamente `"SaberSobreElCirco"` o `"SaberSobreMi"`)
3. El `ChoiceCounterManager` no existe en la escena inicial

**Solución:**
- Verificar que cada opción de diálogo tenga el `Global Choice ID` correcto
- Verificar en la consola los logs: `[ChoiceCounterManager] Incrementado: 'SaberSobreElCirco' (ahora = X)`
- Usar el menú de debug: `Tools > Debug Ending System > Show Counter Values`

---

## 🧪 Cómo Probar el Sistema

### Opción 1: Juego Completo

1. Jugar desde el principio
2. Elegir siempre opciones del **Circo** (SaberSobreElCirco)
3. Al terminar con el Maestro → Debería ir a `Final2`
4. Repetir eligiendo opciones de **Yo** (SaberSobreMi)
5. Al terminar → Debería ir a `Final1`

### Opción 2: Debug Menu (Más rápido)

1. En Unity, ir a **Tools > Debug Ending System**
2. **Show Counter Values** - Ver contadores actuales
3. **Increment "SaberSobreElCirco"** - Simular puntos circo
4. **Increment "SaberSobreMi"** - Simular puntos protagonista
5. **Decide and Load Ending** - Ver qué final se elegiría ahora

---

## 📊 Flujo Completo del Sistema

```
Nivel 1 (Tutorial):
  └─ NPC1 presenta 2 opciones
     ├─ Opción A: "Sobre el circo" → +1 SaberSobreElCirco
     └─ Opción B: "Sobre mí" → +1 SaberSobreMi

Nivel 2 (Pintacaritas):
  └─ NPC2 presenta 2 opciones
     ├─ Opción A: "Sobre el circo" → +1 SaberSobreElCirco
     └─ Opción B: "Sobre mí" → +1 SaberSobreMi

... (más niveles) ...

Nivel Final (Maestro de Ceremonias):
  └─ Maestro presenta opciones finales
     ├─ Opción A: "Entregar materiales" 
     │   └─ +1 SaberSobreElCirco
     │   └─ triggersFinal: true ✅
     │   └─ DecideAndLoad() ejecutado
     │   └─ Si SaberSobreElCirco ganó → Final2 (Circo)
     │       Si SaberSobreMi ganó → Final1 (Yo)
     │       Si empate → Aleatorio
     └─ Opción B: "Abrir portal"
         └─ Similar al anterior
```

---

## 🎓 Resumen

✅ **Acumulación:** Todos los niveles acumulan puntos con `Global Choice ID`

✅ **Decisión:** Solo el Maestro de Ceremonias activa el final con `triggersFinal: true`

✅ **Persistencia:** `ChoiceCounterManager` mantiene los puntos entre escenas

✅ **Finales:** Final1 (protagonista) o Final2 (circo) según puntos acumulados

---

## 📚 Archivos Relacionados

- **Scripts:**
  - `Assets/02_Scripts/ChoiceCounterManager.cs`
  - `Assets/02_Scripts/MissionToEndingChooser.cs`
  - `Assets/02_Scripts/NPCBasicDialog.cs`

- **Prefabs:**
  - `Assets/03_Prefabs/NpcMaestroCeremonias.prefab`

- **Escenas:**
  - `Assets/01_Scenes/Levels/MaestroCeremonias_Level.unity`
  - `Assets/01_Scenes/Levels/Final1.unity`
  - `Assets/01_Scenes/Levels/Final2.unity`

---

**Fecha:** Noviembre 2025  
**Versión:** 1.0  
**Autor:** GitHub Copilot  
**Proyecto:** MoonLab
