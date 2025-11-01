# ⚡ INICIO RÁPIDO - 5 MINUTOS

## 🎯 LO QUE NECESITAS SABER

He creado **4 scripts nuevos** para tu nivel de Pintacaritas:

1. ✅ `NPCGroupConversation.cs` - Para conversaciones entre múltiples NPCs
2. ✅ `PintacaritasNPC.cs` - Para NPCs con diálogos que cambian según el progreso
3. ✅ `PintacaritasLevelController.cs` - Para debugging (opcional)
4. ✅ **3 Guías completas** con instrucciones paso a paso

---

## 🚀 PASOS PARA IMPLEMENTAR (Resumen Ultra-Rápido)

### 1️⃣ Crear el Grupo (Pintacaritas1 + Maestro)

1. Crea GameObject vacío: `Grupo_Pintacaritas1_Maestro`
2. Agregar:
   - `Circle Collider 2D` → Marcar `Is Trigger`
   - Script `NPCGroupConversation`
   - Layer `Interactable`
3. En el script, configurar:
   - 3 líneas para conversación inicial
   - 2 líneas para conversación después
   - Misiones: `HablasteCon_Pintacaritas1` y `HablasteCon_Trapecista`

**📄 Ver textos completos en:** `TEXTOS_DIALOGOS.md`

---

### 2️⃣ Configurar Trapecista

1. Seleccionar tu GameObject `Trapecista`
2. Agregar script `NPCBasicDialog` (si no lo tiene)
3. Marcar `Uses Mission System` ✅
4. Agregar 2 misiones:
   - Misión 1: Da `HablasteCon_Trapecista`
   - Misión 2: Diálogo de seguimiento

---

### 3️⃣ Configurar Pintacaritas 2

1. Seleccionar tu GameObject `Pintacaritas2`
2. Agregar script `PintacaritasNPC`
3. Configurar:
   - Diálogo corto (antes de hablar con Pintacaritas1)
   - Diálogo largo (después de hablar con Pintacaritas1)
   - Misiones: `HablasteCon_Pintacaritas1` y `HablasteCon_Trapecista`

---

## 📖 LEE ESTAS GUÍAS

### Para configurar paso a paso:
📄 **`RESUMEN_CONFIGURACION.md`** ← Empieza aquí

### Para entender el flujo:
📄 **`DIAGRAMA_VISUAL.md`** ← Diagramas y explicaciones

### Para los textos:
📄 **`TEXTOS_DIALOGOS.md`** ← Copia y pega los diálogos

### Para detalles completos:
📄 **`GUIA_PINTACARITAS.md`** ← Guía completa

---

## ⚠️ IMPORTANTE - Nombres de Misiones

Copia estos nombres EXACTAMENTE:

```
HablasteCon_Pintacaritas1
```

```
HablasteCon_Trapecista
```

---

## 🎮 CÓMO FUNCIONA

```
1. Jugador habla con GRUPO
   → Ve conversación Maestro + Pintacaritas1
   → Se agrega misión

2. Jugador habla con TRAPECISTA
   → Ve su diálogo
   → Se completa misión anterior

3. Jugador vuelve al GRUPO
   → Ve nueva conversación (ahora con la prota)

4. PINTACARITAS 2 tiene diálogo diferente
   → Según si ya habló con Pintacaritas1 o no
```

---

## 🐛 Si algo no funciona

1. Verifica que todos los NPCs tengan:
   - `Circle Collider 2D` con `Is Trigger` ✅
   - Layer `Interactable`
   - UI hijo "Presiona E"

2. Verifica que los nombres de las misiones sean EXACTAMENTE iguales

3. Verifica que el GameManager esté en la escena

4. Usa el script `PintacaritasLevelController` para debugging

---

## 🎨 ORDEN RECOMENDADO

1. Lee `RESUMEN_CONFIGURACION.md` (5 min)
2. Configura el Grupo (10 min)
3. Configura Trapecista (5 min)
4. Configura Pintacaritas 2 (5 min)
5. Prueba el nivel (10 min)

**Total: ~35 minutos**

---

## 💡 PRÓXIMOS PASOS

Una vez que funcione todo:

- [ ] Ajusta los textos según tu historia
- [ ] Agrega más líneas de diálogo si quieres
- [ ] Personaliza las animaciones de los NPCs
- [ ] Agrega efectos de sonido

---

¡Todo está listo! Solo sigue las guías paso a paso. 🎉

**¿Dudas?** Revisa `DIAGRAMA_VISUAL.md` para ver el flujo completo.
