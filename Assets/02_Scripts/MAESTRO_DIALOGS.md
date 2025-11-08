Maestro de Ceremonias — Diálogos listos para pegar (formato para `NPCBasicDialog.missions`)

Instrucciones rápidas
- Abre el prefab del NPC en Unity.
- En el componente `NPCBasicDialog` activa `usesMissionSystem = true`.
- Haz clic en + para agregar entradas a la lista `missions`.
- Copia el bloque "dialogueText" a cada `NPCMission.dialogueText` y pega los valores en los campos `missionToGive`, `missionRequired` o `missionToComplete` según indique.

Nombres de misiones (usar exactamente):
- BuscarHilo
- BuscarAguja
- BuscarParche
- VolverConMaestro

--- Maestro (prefab `MaestroCeremonias`) ---
Misión inicial (dar la misión BuscarHilo):
- dialogueText:
  "¡Hola! Necesito preparar el espectáculo. ¿Puedes ayudarme a conseguir tres cosas: un hilo, una aguja y un parche? Empieza por hablar con la trapecista en el Red Stage."
- missionToGive: BuscarHilo
- missionRequired: (vacío)
- missionToComplete: (vacío)

Misión final (dialogo que aparece cuando el jugador vuelve con todo):
- dialogueText:
  "¡Lo conseguiste! Gracias por traer todo. Ahora el show puede empezar."
- missionRequired: VolverConMaestro
- missionToComplete: VolverConMaestro
- missionToGive: (vacío)

--- Trapecista (prefab `NpcTrapecista`) ---
Diálogo principal (usa para introducir la escena / dar contexto):
- dialogueText:
  "¡Eh! ¿Vienes por el hilo? Tengo uno aquí. Tenlo, pero cuida que lo necesitas para coser el vestuario."
- missionRequired: BuscarHilo
- missionToComplete: (vacío)
- missionToGive: (vacío)

Nota: Si estás usando items en el mundo, coloca el prefab `hilo_item` cerca del NPC con Item.cs configurado: missionToComplete = BuscarHilo, missionToGive = BuscarAguja, requiresMission = true.

--- Pintacaritas (prefab `PintaCaritas1` o `PintaCaritas2`) ---
- dialogueText:
  "¿Una aguja? Sí, tengo una. Tenla, que te vendrá bien para los remiendos."
- missionRequired: BuscarAguja
- missionToComplete: (vacío)
- missionToGive: (vacío)

Nota: el `aguja_item` debe tener missionToComplete = BuscarAguja y missionToGive = BuscarParche si usas item prefabs.

--- Vendedor (prefab `NpcVendedor`) ---
- dialogueText:
  "Ah, buscas un parche. Tengo uno por aquí. Toma, y vuelve con el maestro cuando tengas todo."
- missionRequired: BuscarParche
- missionToComplete: (vacío)
- missionToGive: (vacío)

Nota: el `parche_item` debe tener missionToComplete = BuscarParche y missionToGive = VolverConMaestro.

Uso recomendado (items en el mundo, paso a paso):
1. Crear prefab `hilo_item` con `Item`:
   - `interactUI`: Asignar el indicador "Presiona E" (GameObject hijo)
   - `itemIcon`: Sprite del hilo
   - `requiresMission`: true
   - `missionToComplete`: "BuscarHilo"
   - `missionToGive`: "BuscarAguja"
   - `lockedMessage`: "No necesito esto ahora."
2. Repetir para `aguja_item` y `parche_item` con sus misiones respectivas.
3. En cada escena (RedStage, HallBlueRed, Outside) colocar el prefab item físicamente cerca del NPC.
4. El jugador debe hablar con el Maestro (da `BuscarHilo`) y luego desplazarse entre escenas/áreas para recoger los items. `Item.cs` completará y dará misiones según la configuración.

Consejos de inspector para `NPCBasicDialog`:
- `usesMissionSystem` = true
- `missions` lista: agrega una entrada por cada estado/dialogo especial. La prioridad de `NPCBasicDialog` usa: (1) misiones que requieren otra misión activa; (2) diálogos que dan misiones nuevas; (3) diálogos neutros.
- Si quieres un diálogo simple sin misiones, usa `dialogueText` fuera de la lista `missions`.

Si quieres, ahora puedo:
- Generar exactamente los bloques listos para pegar en cada `NPCMission` (texto + valores) en un formato aún más directo paso-a-paso para Unity.
- O crear un breve vídeo GIF (no posible desde aquí) o checklist de clicks. 

Dime cuál prefieres: (1) Bloques listos para pegar uno por uno; (2) instrucciones de inspector paso a paso con capturas de campos (texto).