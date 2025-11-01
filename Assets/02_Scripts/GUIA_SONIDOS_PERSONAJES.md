# 🔊 GUÍA: SONIDOS INDIVIDUALES PARA CADA PERSONAJE

## 📋 RESUMEN
Ahora puedes asignar sonidos de typing diferentes para cada personaje en las conversaciones grupales, dándoles personalidad única.

---

## 🎵 CÓMO FUNCIONA

### Sistema de Fallback:
```
1. ¿Tiene la línea un Speaker Sound asignado?
   ✅ SÍ → Usa ese sonido
   ❌ NO → Usa el Typing Sound general del grupo
```

---

## 🎯 CONFIGURACIÓN EN EL INSPECTOR

### En NPCGroupConversation (Grupo):

```
Initial Conversation:
├── Línea 0 (Pintacaritas1)
│   ├── Speaker Name: Pintacaritas1
│   ├── Speaker Sprite: [sprite]
│   ├── Speaker Sound: [sonido_agudo.wav] ← OPCIONAL ⭐
│   └── Dialogue: "¡Hola!"
│
├── Línea 1 (Pintacaritas3)
│   ├── Speaker Name: Pintacaritas3
│   ├── Speaker Sprite: [sprite]
│   ├── Speaker Sound: [sonido_grave.wav] ← OPCIONAL ⭐
│   └── Dialogue: "Hola..."
│
└── Línea 2 (Héctor)
    ├── Speaker Name: Héctor
    ├── Speaker Sprite: [sprite]
    ├── Speaker Sound: [VACÍO] ← Usará el sonido general
    └── Dialogue: "Buenas"

Audio:
└── Typing Sound: [sonido_general.wav] ← Se usa cuando Speaker Sound está vacío
```

---

## 💡 EJEMPLOS DE USO

### Ejemplo 1: Todos los personajes con sonidos únicos

```
Pintacaritas1:
  Speaker Sound: typing_agudo.wav (voz joven, rápido)
  
Pintacaritas3:
  Speaker Sound: typing_suave.wav (voz tímida, lento)
  
Héctor:
  Speaker Sound: typing_grave.wav (voz profunda, pausado)
  
Typing Sound: [VACÍO - no se usará]
```

**Resultado:** Cada personaje suena diferente, más personalidad.

---

### Ejemplo 2: Solo algunos personajes con sonidos únicos

```
Pintacaritas1:
  Speaker Sound: typing_especial.wav (personaje importante)
  
Pintacaritas3:
  Speaker Sound: [VACÍO]
  
Héctor:
  Speaker Sound: [VACÍO]
  
Typing Sound: typing_general.wav (todos excepto P1)
```

**Resultado:** P1 destaca, los demás usan el sonido común.

---

### Ejemplo 3: Todos usan el mismo sonido

```
Pintacaritas1:
  Speaker Sound: [VACÍO]
  
Pintacaritas3:
  Speaker Sound: [VACÍO]
  
Héctor:
  Speaker Sound: [VACÍO]
  
Typing Sound: typing_general.wav (TODOS)
```

**Resultado:** Más simple, todos suenan igual.

---

## 🎨 IDEAS CREATIVAS PARA SONIDOS

### Por Personalidad:

- **Personaje alegre:** Sonido rápido, pitch alto, notas musicales
- **Personaje serio:** Sonido lento, pitch bajo, mecánico
- **Personaje tímido:** Sonido suave, volumen bajo
- **Personaje enérgico:** Sonido fuerte, irregular, dinámico

### Por Tipo de Personaje:

- **Robot/Máquina:** Beeps electrónicos
- **Fantasma/Espíritu:** Sonido etéreo, reverb
- **Animal:** Sonidos de animal sutiles
- **Niño:** Pitch muy alto, rápido
- **Anciano:** Pitch bajo, pausado

### Por Emoción en la Escena:

```
Conversación Normal:
  Speaker Sound: typing_normal.wav

Conversación Tensa:
  Speaker Sound: typing_tenso.wav (más agresivo)

Conversación Triste:
  Speaker Sound: typing_triste.wav (más lento, melancólico)
```

---

## 🔧 CONFIGURACIÓN TÉCNICA

### Recomendaciones para los AudioClips:

**Formato:**
- WAV o MP3
- Corta duración (0.1 - 0.2 segundos)
- Loop: NO

**Configuración en Unity:**
```
Import Settings:
├── Load Type: Decompress On Load (sonidos cortos)
├── Preload Audio Data: ✅ (para que no haya delay)
├── Compression Format: PCM (mejor calidad) o ADPCM (menor tamaño)
└── Sample Rate: 22050 Hz (suficiente para efectos)
```

---

## 🎭 EJEMPLO COMPLETO: CONVERSACIÓN CON HÉCTOR

### Configuración en el Inspector:

```
Final Conversation (Grupo):

Línea 0:
  Speaker Name: Héctor
  Speaker Sprite: [sprite_hector]
  Speaker Sound: typing_hector_serio.wav ← Sonido profundo
  Dialogue: "Así que quieres saber qué pasó..."

Línea 1:
  Speaker Name: Héctor
  Speaker Sprite: [sprite_hector_triste]
  Speaker Sound: typing_hector_triste.wav ← Más lento, melancólico
  Dialogue: "Todo empezó hace mucho tiempo..."

Línea 2:
  Speaker Name: Héctor
  Speaker Sprite: [sprite_hector]
  Speaker Sound: typing_hector_serio.wav ← Vuelve al tono normal
  Dialogue: "Ahora ve con Pintacaritas2."

Typing Sound: typing_general.wav ← Por si alguna línea no tiene sonido
```

---

## 🐛 TROUBLESHOOTING

### ❌ "No se escucha ningún sonido"
**Solución:**
- Verifica que el AudioClip está asignado
- Verifica que el volumen del AudioSource en GameManager no esté en 0
- Verifica que el sonido no esté en mute en la escena

### ❌ "Todos suenan igual aunque asigné sonidos diferentes"
**Solución:**
- Verifica que realmente asignaste AudioClips diferentes
- Los sonidos pueden ser muy similares, prueba con sonidos muy distintos primero

### ❌ "El sonido se corta"
**Solución:**
- Puede ser demasiado corto
- Usa un sonido de ~0.15 segundos mínimo
- O configura el GameManager para que repita el sonido

---

## 📝 NOTAS IMPORTANTES

### ✅ **Ventajas de usar Speaker Sound:**
- Cada personaje tiene personalidad única
- Más inmersivo para el jugador
- Fácil identificar quién habla sin mirar

### ✅ **Ventajas de usar solo Typing Sound general:**
- Más simple de configurar
- Menos archivos de audio
- Más consistente

### 💡 **Consejo:**
- Empieza con un sonido general para todos
- Luego agrega sonidos únicos solo a personajes importantes
- No todos los personajes necesitan sonido único

---

## 🎮 TESTING

Para probar los sonidos:

1. **Configura al menos 2 personajes con sonidos diferentes**
2. **Juega hasta la conversación**
3. **Escucha si cada personaje suena diferente**
4. **Ajusta volumen/pitch según necesites**

---

## ✅ CHECKLIST

- [ ] AudioClips preparados y importados
- [ ] Configuración de Import Settings optimizada
- [ ] Speaker Sound asignado a líneas importantes
- [ ] Typing Sound general asignado como fallback
- [ ] Testing con volumen adecuado
- [ ] Todos los sonidos tienen duración similar
- [ ] Sonidos no se solapan o cortan

---

¿Necesitas ayuda para crear o editar los sonidos? 🎵
