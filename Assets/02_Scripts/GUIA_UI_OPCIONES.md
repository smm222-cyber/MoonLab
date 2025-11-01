# 🎨 GUÍA: CÓMO IMPLEMENTAR LA UI DE OPCIONES DE DIÁLOGO

## 📋 RESUMEN
Esta guía te muestra cómo configurar un sistema de UI en Unity para mostrar las opciones de diálogo de Pintacaritas2.

---

## 🎯 OPCIÓN 1: TESTING SIN UI (Más Rápido)

Si quieres probar el sistema **SIN implementar la UI primero**:

### ✅ Qué hace:
- El script **automáticamente** elige la primera opción después de 2 segundos
- Verás en la Console todas las opciones disponibles
- Perfecto para testing rápido

### 🔧 Configuración:
1. **NO asignes nada** en el campo `Choices UI System` de Pintacaritas2
2. ¡Listo! El sistema funcionará en modo testing

### 📊 Verás en la Console:
```
[Pintacaritas2] ⚠️ SISTEMA DE OPCIONES NO IMPLEMENTADO - Usando opción por defecto
[Pintacaritas2] Opciones disponibles:
  1. Creo que ambos tienen razón en algunas cosas
  2. Necesito pensar más sobre esto
[Pintacaritas2] Simulando elección automática en 2 segundos...
[Pintacaritas2] Jugador eligió opción 0: Creo que ambos tienen razón...
```

---

## 🎯 OPCIÓN 2: UI COMPLETA (Producción)

Si quieres que el jugador **realmente elija** con botones en pantalla:

---

### 📦 PASO 1: Crear el Prefab de Botón

1. **Crea un GameObject** → UI → **Button - TextMeshPro**
   - Nombre: `ChoiceButton`

2. **Configura el botón:**
   - Tamaño recomendado: Width: `600`, Height: `80`
   - Anchor: Stretch horizontal
   
3. **Edita el texto (TMP):**
   - Font Size: `24`
   - Alignment: Center/Middle
   - Color: Blanco o el que prefieras
   - Best Fit: ✅ (opcional, para ajuste automático)

4. **Estiliza el botón** (opcional):
   - Image → Color: Color de fondo
   - Navigation: None (o configura según necesites)
   - Transition: Color Tint (ajusta colores Normal/Highlighted/Pressed)

5. **Guarda como Prefab:**
   - Arrastra el botón a tu carpeta de Prefabs
   - Elimina el botón de la escena

---

### 🗂️ PASO 2: Crear el Panel de Opciones

1. **Crea un Canvas** (si no tienes uno):
   - GameObject → UI → Canvas
   - Render Mode: Screen Space - Overlay
   - Canvas Scaler → UI Scale Mode: Scale With Screen Size
   - Reference Resolution: `1920 x 1080` (ajusta según tu juego)

2. **Dentro del Canvas, crea un Panel:**
   - Click derecho en Canvas → UI → Panel
   - Nombre: `DialogueChoicesPanel`
   - Anchor: Stretch todo (o centra según prefieras)
   - Color: Semi-transparente negro `(0, 0, 0, 200)` para oscurecer el fondo

3. **Dentro del Panel, crea un Vertical Layout Group:**
   - Click derecho en `DialogueChoicesPanel` → UI → Empty (GameObject vacío)
   - Nombre: `ButtonsContainer`
   - **Agregar componente:** Vertical Layout Group
     - Child Alignment: Middle Center
     - Control Child Size: ✅ Width, ❌ Height
     - Child Force Expand: ✅ Width, ❌ Height
     - Spacing: `20`
   - **Agregar componente:** Content Size Fitter
     - Vertical Fit: Preferred Size

4. **Posiciona el Container:**
   - Anchor: Center/Middle
   - Width: `700`
   - Height: Automático (por Content Size Fitter)

---

### 🔧 PASO 3: Agregar el Script DialogueChoicesUI

1. **Selecciona** `DialogueChoicesPanel`

2. **Add Component** → `DialogueChoicesUI`

3. **Configura en el Inspector:**
   - **Choices Panel:** Arrastra `DialogueChoicesPanel` (el mismo objeto)
   - **Buttons Container:** Arrastra `ButtonsContainer` (el hijo con Layout Group)
   - **Choice Button Prefab:** Arrastra el prefab de botón que creaste
   - **Button Spacing:** `20` (ya está en el Layout Group, pero por si acaso)

4. **Desactiva el Panel:**
   - ✅ IMPORTANTE: Desmarca la casilla junto al nombre `DialogueChoicesPanel` en el Inspector
   - Debe estar inactivo al inicio (el script lo activará cuando sea necesario)

---

### 🔗 PASO 4: Conectar con Pintacaritas2

1. **Selecciona el GameObject** `Pintacaritas2`

2. **En el Inspector**, ve a la sección **"Sistema de UI (Opcional)"**

3. **Arrastra** el `DialogueChoicesPanel` al campo **Choices UI System**

4. ✅ ¡Listo! Ahora el sistema usará tu UI real

---

## 🎮 TESTING

### Caso 1: Sin UI (Testing Mode)
```
1. No asignar nada en "Choices UI System"
2. Jugar hasta FASE 5
3. Ver en Console las opciones
4. Esperar 2 segundos
5. Se elige automáticamente la opción 0
```

### Caso 2: Con UI (Production Mode)
```
1. Asignar DialogueChoicesPanel en "Choices UI System"
2. Jugar hasta FASE 5
3. Ver el panel con botones en pantalla
4. Click en un botón
5. Ver la respuesta del NPC
6. Panel se cierra automáticamente
```

---

## 🐛 TROUBLESHOOTING

### ❌ "Los botones no aparecen"
**Solución:**
- Verifica que `ButtonsContainer` tenga el componente `Vertical Layout Group`
- Verifica que el prefab de botón esté asignado en el script
- Verifica que el Panel esté desactivado al inicio (se activa automáticamente)

### ❌ "Los botones están muy juntos/separados"
**Solución:**
- Ajusta el `Spacing` en el Vertical Layout Group de `ButtonsContainer`
- Ajusta el `Height` de tu prefab de botón

### ❌ "El texto del botón no se ve"
**Solución:**
- Verifica que el botón tiene un componente `TextMeshProUGUI` como hijo
- Verifica el tamaño de fuente y color del texto
- Usa Best Fit si el texto es muy largo

### ❌ "Los botones no responden al click"
**Solución:**
- Verifica que el Canvas tenga un componente `Graphic Raycaster`
- Verifica que hay un `EventSystem` en la escena
- Verifica que el botón tiene el componente `Button` activo

### ❌ "El panel no se oculta después de elegir"
**Solución:**
- El script `DialogueChoicesUI` debe estar en el Panel principal, no en el Container
- Verifica que la referencia `choicesPanel` apunte al objeto correcto

---

## 🎨 PERSONALIZACIÓN

### Cambiar diseño de botones:
Edita el prefab `ChoiceButton`:
- Cambia colores
- Agrega imágenes de fondo
- Ajusta fuente y tamaño
- Agrega efectos de hover/press

### Cambiar layout:
En `ButtonsContainer`, cambia:
- **Vertical Layout Group** → **Horizontal Layout Group** (opciones lado a lado)
- **Grid Layout Group** (opciones en cuadrícula)

### Cambiar posición del panel:
En `DialogueChoicesPanel`:
- Cambia Anchor a Bottom (abajo)
- Cambia Anchor a Top (arriba)
- Centra según tu preferencia

### Agregar animaciones:
1. Crea un Animator Controller
2. Anima la escala del Panel (0 → 1) para "pop in"
3. Anima el alpha para fade in/out
4. Asigna el Animator al Panel

---

## 📝 CÓDIGO PERSONALIZADO

### Si quieres modificar `DialogueChoicesUI.cs`:

**Agregar sonido al hacer click:**
```csharp
public AudioClip clickSound;

private void OnChoiceClicked(int choiceIndex)
{
    // Reproducir sonido
    if (clickSound != null)
    {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
    }
    
    // ... resto del código
}
```

**Agregar delay antes de cerrar el panel:**
```csharp
private void OnChoiceClicked(int choiceIndex)
{
    Debug.Log($"DialogueChoicesUI: Jugador eligió opción {choiceIndex}");
    
    if (currentNPC != null)
    {
        currentNPC.OnChoiceSelected(choiceIndex);
    }
    
    // Esperar un poco antes de cerrar
    StartCoroutine(HideAfterDelay(0.5f));
}

IEnumerator HideAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    HideChoices();
}
```

**Desactivar botones después de elegir:**
```csharp
private void OnChoiceClicked(int choiceIndex)
{
    // Desactivar todos los botones para evitar clicks múltiples
    foreach (GameObject buttonObj in spawnedButtons)
    {
        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }
    }
    
    // ... resto del código
}
```

---

## ✅ CHECKLIST FINAL

### Setup Básico:
- [ ] Prefab de botón creado con TextMeshProUGUI
- [ ] Canvas con DialogueChoicesPanel creado
- [ ] ButtonsContainer con Vertical Layout Group
- [ ] DialogueChoicesUI script agregado al Panel
- [ ] Referencias asignadas en el script
- [ ] Panel desactivado por defecto

### Conexión:
- [ ] DialogueChoicesPanel asignado en Pintacaritas2
- [ ] Campo "Choices UI System" lleno

### Testing:
- [ ] Jugar hasta FASE 5
- [ ] Ver que aparecen los botones
- [ ] Click en una opción
- [ ] Ver respuesta del NPC
- [ ] Panel se cierra solo

---

## 🚀 PRÓXIMOS PASOS

1. ✅ Implementa la UI básica según esta guía
2. ⏳ Testea con tu flujo completo
3. ⏳ Personaliza diseño según tu juego
4. ⏳ Agrega animaciones (opcional)
5. ⏳ Agrega sonidos (opcional)

---

¿Necesitas ayuda con algo específico de la UI? 🎨
