# Configuración Cambio de Escena: Vendedor_Level → MaestroCeremonias_Level

## 🎯 Configuración Requerida

### 1. Build Settings
1. Abre `File > Build Settings`
2. Verifica que ambas escenas estén incluidas:
   - `Vendedor_Level`
   - `MaestroCeremonias_Level`
3. Si alguna falta, arrástrarla desde el Project window

### 2. Configuración del NPC en Vendedor_Level

En el Inspector de Unity:
1. Selecciona el GameObject del Vendedor
2. Encuentra el componente `NPCBasicDialog`
3. En la sección "Missions":
   
#### Si el NPC usa Opciones de Diálogo:
```
Para AMBAS opciones (Opción 0 y Opción 1):
✅ Load Scene After Choice: MARCADO
📝 Scene To Load: "MaestroCeremonias_Level"
⏱️ Delay Before Scene Change: 2.0
```

#### Si el NPC usa Cambio de Escena por Misión:
```
En la misión final:
✅ Load Scene After Dialog: MARCADO
📝 Scene To Load After Dialog: "MaestroCeremonias_Level"
⏱️ Delay Before Scene Change: 2.0
```

## ⚠️ IMPORTANTE
- El nombre de la escena DEBE ser EXACTAMENTE: `"MaestroCeremonias_Level"`
- Si hay opciones de diálogo, AMBAS deben llevar a `"MaestroCeremonias_Level"`
- Recomendado: Delay de 2.0 segundos para transición suave

## 🔍 Checklist de Verificación

- [ ] `Vendedor_Level` está en Build Settings
- [ ] `MaestroCeremonias_Level` está en Build Settings
- [ ] Nombre de escena escrito correctamente: `"MaestroCeremonias_Level"`
- [ ] `Load Scene After Choice` o `Load Scene After Dialog` está marcado
- [ ] `Delay Before Scene Change` está configurado (2.0 recomendado)
- [ ] Si hay opciones, AMBAS tienen la misma configuración

## 🐛 Problemas Comunes

1. **La escena no cambia:**
   - Verifica que `MaestroCeremonias_Level` esté en Build Settings
   - Confirma que el nombre esté escrito exactamente igual
   - Asegúrate que `Load Scene After Choice` está marcado

2. **Transición muy rápida/lenta:**
   - Ajusta `Delay Before Scene Change`:
     - Muy rápido → Aumenta a 2.0-3.0
     - Muy lento → Reduce a 1.0-1.5

## 📝 Flujo de Ejecución
1. Jugador completa interacción con Vendedor
2. Sistema espera el tiempo configurado (2.0 segundos)
3. Carga automáticamente `MaestroCeremonias_Level`

---
**Última actualización:** Noviembre 2025
**Versión:** 1.0
**Proyecto:** MoonLab