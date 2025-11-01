# 🎨 CONFIGURACIÓN NIVEL PINTACARITAS - CON OPCIONES DE DIÁLOGO

## 📋 RESUMEN DEL FLUJO COMPLETO

```
1. Grupo → Da misión "Averiguar qué está pasando"
2. Pintacaritas2 (FASE 2) → Completa misión + Da "Buscar el pincel de la pintacaritas"
3. [Jugador recoge pincel]
4. Pintacaritas2 (FASE 3) → Da "Buscar las pinturas de la pintacaritas"
5. [Jugador recoge pinturas]
6. Pintacaritas2 (FASE 4) → Da "Hablar con Héctor el Pintacaritas"
7. Grupo (Conversación Final) → Completa misión + Da "Volver con Pintacaritas2"
8. Pintacaritas2 (FASE 5) → Completa misión + Muestra OPCIONES DE DIÁLOGO
9. [Jugador elige opción]
10. Pintacaritas2 (FASE 6) → Muestra diálogo final
```

---

## 🎭 1. CONFIGURAR "GRUPO" (NPCGroupConversation.cs)

### GameObject: `Grupo`
**Components:**
- `NPCGroupConversation.cs`
- `Circle Collider 2D` (Is Trigger ✅)
- `AudioSource`

### ⚙️ CONFIGURACIÓN INSPECTOR

#### **NPC Info**
- **NPC Name:** `Grupo de Pintacaritas`
- **NPC Image:** *(sprite del grupo)*
- **Interact UI:** *(referencia al GameObject indicador)*

#### **Initial Conversation** (Primera interacción)
- **Size:** 3 líneas mínimo

```
Línea 0:
  Speaker Name: Pintacaritas1
  Speaker Sprite: [sprite P1]
  Dialogue: "¡Mira, un niño! Tal vez pueda ayudarnos..."

Línea 1:
  Speaker Name: Pintacaritas3
  Speaker Sprite: [sprite P3]
  Dialogue: "No sé si debemos involucrar a alguien más en nuestros problemas..."

Línea 2:
  Speaker Name: Pintacaritas4
  Speaker Sprite: [sprite P4]
  Dialogue: "Bueno... si está aquí, al menos puede escuchar nuestra historia."
```

#### **Sistema de Misiones - Initial Conversation**
- **Mission To Give After Initial:** `Averiguar qué está pasando`

#### **After Mission Conversation** (Después de hablar con P2)
- **Mission Required:** `Preguntar por la pelea`
- **Size:** 2-3 líneas

```
Línea 0:
  Speaker Name: Pintacaritas1
  Speaker Sprite: [sprite P1]
  Dialogue: "¿Ya hablaste con Pintacaritas2? ¿Te contó algo?"

Línea 1:
  Speaker Name: Grupo de Pintacaritas
  Speaker Sprite: [sprite grupo]
  Dialogue: "Ella tiene una perspectiva diferente de lo que pasó..."
```

#### **Sistema de Misiones - After Mission**
- **Mission To Complete When Showing After:** `Preguntar por la pelea`
- **Mission To Give After Second:** `Escuchar la historia completa`

#### **Final Conversation** (Después de recoger pinturas)
- **Mission Required For Final:** `Hablar con Héctor el Pintacaritas`
- **Size:** 3-4 líneas (Héctor cuenta su historia)

```
Línea 0:
  Speaker Name: Héctor
  Speaker Sprite: [sprite Héctor]
  Dialogue: "Así que realmente quieres saber qué pasó... Muy bien, te contaré mi versión."

Línea 1:
  Speaker Name: Héctor
  Speaker Sprite: [sprite Héctor]
  Dialogue: "[Historia larga de Héctor explicando el conflicto]"

Línea 2:
  Speaker Name: Héctor
  Speaker Sprite: [sprite Héctor]
  Dialogue: "Ahora entiendes por qué las cosas están así. Ve con Pintacaritas2, seguro tiene más que decir."
```

#### **Sistema de Misiones - Final Conversation**
- **Mission To Complete In Final:** `Hablar con Héctor el Pintacaritas`
- **Mission To Give After Final:** `Volver con Pintacaritas2` ⭐ **NUEVO**

#### **Audio**
- **Typing Sound:** *(clip de sonido)*

---

## 🎨 2. CONFIGURAR "PINTACARITAS2" (PintacaritasNPC.cs)

### GameObject: `Pintacaritas2`
**Components:**
- `PintacaritasNPC.cs`
- `Circle Collider 2D` (Is Trigger ✅)
- `AudioSource`

### ⚙️ CONFIGURACIÓN INSPECTOR

#### **NPC Info**
- **NPC Name:** `Pintacaritas2`
- **NPC Image:** *(sprite de Pintacaritas2)*
- **Interact UI:** *(referencia al GameObject indicador)*

---

### 📝 **FASE 1: Diálogo Corto Inicial**
```
Dialogo Sin Hablar Con Pintacaritas1:
"Hola... no sé si quieras hablar conmigo. Habla con el grupo primero."
```

---

### 📝 **FASE 2: Diálogo Largo + Primera Misión de Objeto**

#### **Misión Requerida Fase 2:** `Averiguar qué está pasando`

#### **Diálogo Fase 2:**
```
"Así que el grupo te envió... Supongo que quieren que escuches mi lado de la historia. 
[Tu diálogo largo aquí explicando el conflicto desde su perspectiva]
Para que entiendas mejor, necesito que me ayudes a encontrar mi pincel. Lo dejé por aquí cerca..."
```

#### **Misiones:**
- **Misión A Completar Fase 2:** `Averiguar qué está pasando`
- **Misión A Dar Fase 2:** `Buscar el pincel de la pintacaritas`

---

### 📝 **FASE 3: Diálogo Corto + Segunda Misión de Objeto**

#### **Misión Requerida Fase 3:** `Buscar el pincel de la pintacaritas`

#### **Diálogo Fase 3:**
```
"¡Encontraste mi pincel! Gracias. Ahora necesito mis pinturas... 
¿Podrías buscarlas también? Creo que las dejé en el otro lado del patio."
```

#### **Misión A Dar Fase 3:** `Buscar las pinturas de la pintacaritas`

---

### 📝 **FASE 4: Diálogo + Misión de Hablar con Héctor**

#### **Misión Requerida Fase 4:** `Buscar las pinturas de la pintacaritas`

#### **Diálogo Fase 4:**
```
"Perfecto, ya tengo mis pinturas. Ahora que me ayudaste, creo que deberías 
escuchar la versión de Héctor. Búscalo en el grupo y pregúntale su lado de la historia."
```

#### **Misión A Dar Fase 4:** `Hablar con Héctor el Pintacaritas`

---

### 📝 **FASE 5: OPCIONES DE DIÁLOGO** ⭐ **NUEVO**

#### **Misión Requerida Fase 5:** `Volver con Pintacaritas2`

#### **Diálogo Fase 5 Inicial:**
```
"Ya hablaste con Héctor, ¿verdad? Entonces ya conoces ambos lados de la historia. 
Ahora dime... ¿qué piensas de todo esto?"
```

#### **Misión A Completar Fase 5:** `Volver con Pintacaritas2`

#### **Opciones Fase 5** (List - Size: 2)

**Opción 0:**
- **Choice Text:** `"Creo que ambos tienen razón en algunas cosas"`
- **Response Dialogue:**
```
"Esa es una respuesta muy madura... Tienes razón, nadie es completamente bueno o malo 
en esta situación. Tal vez podamos encontrar una solución si hablamos más. 
Gracias por escucharnos a todos."
```

**Opción 1:**
- **Choice Text:** `"Necesito pensar más sobre esto"`
- **Response Dialogue:**
```
"Entiendo... Es una situación complicada y no es fácil tomar partido. 
Está bien que te tomes tu tiempo para procesar todo lo que escuchaste. 
De todas formas, gracias por ayudarnos."
```

---

### 📝 **FASE 6: Diálogo Final**

#### **Diálogo Final:**
```
"Gracias por todo tu apoyo. Creo que con tu ayuda, podemos empezar a resolver 
nuestras diferencias. Vuelve cuando quieras."
```

---

### 🎵 **Audio**
- **Typing Sound:** *(mismo clip que el grupo)*

### ⚙️ **Configuración Avanzada**
- **Max Characters Per Page:** `40` (ajustar según necesites)

---

## ✅ CHECKLIST DE CONFIGURACIÓN

### Grupo (NPCGroupConversation):
- [ ] 3 conversaciones configuradas (Initial, After, Final)
- [ ] Sprites asignados a cada línea de diálogo
- [ ] 6 misiones configuradas:
  - [ ] `missionToGiveAfterInitial`: "Averiguar qué está pasando"
  - [ ] `missionRequired`: "Preguntar por la pelea"
  - [ ] `missionToCompleteWhenShowingAfter`: "Preguntar por la pelea"
  - [ ] `missionToGiveAfterSecond`: "Escuchar la historia completa"
  - [ ] `missionRequiredForFinalConversation`: "Hablar con Héctor el Pintacaritas"
  - [ ] `missionToCompleteInFinalConversation`: "Hablar con Héctor el Pintacaritas"
  - [ ] `missionToGiveAfterFinal`: **"Volver con Pintacaritas2"** ⭐

### Pintacaritas2 (PintacaritasNPC):
- [ ] 6 Fases configuradas (1-6)
- [ ] Diálogos cortos para Fase 1, 3
- [ ] Diálogos largos para Fase 2, 4, 5
- [ ] 5 misiones configuradas:
  - [ ] `misionRequeridaFase2`: "Averiguar qué está pasando"
  - [ ] `misionACompletarFase2`: "Averiguar qué está pasando"
  - [ ] `misionADarFase2`: "Buscar el pincel de la pintacaritas"
  - [ ] `misionRequeridaFase3`: "Buscar el pincel de la pintacaritas"
  - [ ] `misionADarFase3`: "Buscar las pinturas de la pintacaritas"
  - [ ] `misionRequeridaFase4`: "Buscar las pinturas de la pintacaritas"
  - [ ] `misionADarFase4`: "Hablar con Héctor el Pintacaritas"
  - [ ] `misionRequeridaFase5`: **"Volver con Pintacaritas2"** ⭐
  - [ ] `misionACompletarFase5`: **"Volver con Pintacaritas2"** ⭐
- [ ] **2 Opciones de diálogo configuradas** ⭐
- [ ] Diálogo final configurado (Fase 6)

### Sistema de Objetos:
- [ ] Pincel recogible (tu sistema de items)
- [ ] Pinturas recogibles (tu sistema de items)
- [ ] Ambos completan sus respectivas misiones al recogerlos

### General:
- [ ] GameManager existe en la escena
- [ ] Todos los NPCs tienen tag "NPC" (opcional)
- [ ] Player tiene tag "Player"
- [ ] AudioClips asignados
- [ ] Colliders configurados como Trigger

---

## 🎮 FLUJO DE TESTING

### Paso a Paso para Testear:

1. **Iniciar Juego** → Interactuar con Grupo
   - ✅ Debe dar misión "Averiguar qué está pasando"

2. **Interactuar con Pintacaritas2** (FASE 2)
   - ✅ Diálogo largo
   - ✅ Completa "Averiguar qué está pasando"
   - ✅ Da "Buscar el pincel de la pintacaritas"

3. **Recoger Pincel**
   - ✅ Tu sistema de items debe completar la misión

4. **Volver con Pintacaritas2** (FASE 3)
   - ✅ Diálogo corto
   - ✅ Da "Buscar las pinturas de la pintacaritas"

5. **Recoger Pinturas**
   - ✅ Tu sistema de items debe completar la misión

6. **Volver con Pintacaritas2** (FASE 4)
   - ✅ Diálogo explicando que hables con Héctor
   - ✅ Da "Hablar con Héctor el Pintacaritas"

7. **Volver con Grupo** (Conversación Final)
   - ✅ Héctor cuenta su historia
   - ✅ Completa "Hablar con Héctor el Pintacaritas"
   - ✅ Da **"Volver con Pintacaritas2"** ⭐

8. **Volver con Pintacaritas2** (FASE 5) ⭐
   - ✅ Completa "Volver con Pintacaritas2"
   - ✅ Muestra diálogo inicial
   - ✅ **Muestra opciones de diálogo**
   - ⚠️ Por ahora selecciona automáticamente opción 0 (testing)

9. **Después de elegir opción**
   - ✅ Muestra respuesta del NPC según la opción

10. **Siguiente interacción** (FASE 6)
    - ✅ Muestra diálogo final
    - ✅ No da más misiones

---

## ⚠️ IMPORTANTE: SISTEMA DE UI PARA OPCIONES

### 🔴 PENDIENTE DE IMPLEMENTAR

El script `PintacaritasNPC.cs` tiene un **placeholder** para el sistema de opciones.

**Método actual (línea ~354):**
```csharp
IEnumerator MostrarOpcionesDeDialogo()
{
    // TODO: Aquí deberías mostrar tu UI de opciones
    // Por ahora simula que el jugador eligió la primera opción
    yield return new WaitForSeconds(2f);
    OnChoiceSelected(0); // Opción por defecto para testing
}
```

### 📝 CÓMO IMPLEMENTAR TU UI:

1. **Crea un Panel UI** con:
   - Botones para cada opción (puedes instanciarlos dinámicamente)
   - Script que escuche los clicks

2. **En tu script de UI:**
```csharp
public void OnButtonClicked(int index)
{
    // Encuentra la referencia a Pintacaritas2
    PintacaritasNPC npc = FindObjectOfType<PintacaritasNPC>();
    npc.OnChoiceSelected(index);
    
    // Cierra el panel de opciones
    gameObject.SetActive(false);
}
```

3. **Modifica `MostrarOpcionesDeDialogo()` para:**
   - Activar tu panel de UI
   - Poblar los botones con `opcionesFase5[i].choiceText`
   - Esperar a que el jugador haga click
   - No llamar automáticamente a `OnChoiceSelected(0)`

---

## 🐛 DEBUGGING

### Ver el flujo en la Console:

```
[Grupo] Dando misión inicial: Averiguar qué está pasando
[Pintacaritas2] FASE 2 - Diálogo largo inicial
[Pintacaritas2] FASE 2 - Misión completada: Averiguar qué está pasando
[Pintacaritas2] FASE 2 - Nueva misión dada: Buscar el pincel...
[Pintacaritas2] FASE 3 - Después del primer objeto
[Pintacaritas2] FASE 3 - Nueva misión dada: Buscar las pinturas...
[Pintacaritas2] FASE 4 - Después del segundo objeto
[Pintacaritas2] FASE 4 - Nueva misión dada: Hablar con Héctor...
[Grupo] Mostrando conversación FINAL
[Grupo] Dando misión después de conversación final: Volver con Pintacaritas2
[Pintacaritas2] FASE 5 - Preparando opciones de diálogo
[Pintacaritas2] Mostrando opciones de diálogo
[Pintacaritas2] ⚠️ SISTEMA DE OPCIONES NO IMPLEMENTADO
[Pintacaritas2] Jugador eligió opción 1: [texto de la opción]
[Pintacaritas2] FASE 6 - Diálogo final después de elegir opción
```

### Comandos útiles en la Console de Unity:
- Busca por `[Grupo]` para ver el flujo del grupo
- Busca por `[Pintacaritas2]` para ver el flujo de P2
- Busca por `⚠️` para ver warnings del sistema de opciones

---

## 🎯 PRÓXIMOS PASOS

1. ✅ Scripts creados y configurados
2. ⏳ **Configurar en Unity Inspector** (TÚ)
3. ⏳ **Implementar UI de opciones** (TÚ)
4. ⏳ Testing completo del flujo
5. ⏳ Ajustar diálogos según feedback

---

¿Necesitas ayuda con algo más? 🚀
