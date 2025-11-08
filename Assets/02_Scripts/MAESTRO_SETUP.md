Maestro de Ceremonias — Instrucciones para crear el nivel

Resumen
-------
Esta guía describe, paso a paso, cómo montar el nivel "MaestroCeremonias" reutilizando una escena base (por ejemplo `Vendedor_Level`) y distribuyendo los NPCs y objetos en varias escenas (RedStage, HallBlueRed, Outside). Incluye los nombres exactos de misiones y cómo configurar los prefabs de items para que la progresión se haga de forma encadenada y sin romper el sistema `Item.cs` existente.

Flujo de misión (recomendado)
-----------------------------
1. Maestro (al inicio) da la misión: `BuscarHilo`.
2. Trapecista (en `RedStage_Scene`) tiene cerca el prefab `hilo_item`: al recogerlo completa `BuscarHilo` y da `BuscarAguja`.
3. Pintacaritas1/2 (en `HallBlueRed_Scene`) tiene cerca el prefab `aguja_item`: al recogerlo completa `BuscarAguja` y da `BuscarParche`.
4. Vendedor (en `Outside_Scene`) tiene cerca el prefab `parche_item`: al recogerlo completa `BuscarParche` y da la misión `VolverConMaestro`.
5. Volver con el Maestro: el diálogo final del Maestro completa `VolverConMaestro` y muestra la secuencia final.

Nombres de escenas (sugeridos)
------------------------------
- `Assets/01_Scenes/Levels/MaestroCeremonias_Level.unity` (copia de `Vendedor_Level` o similar)
- Escenas donde colocarás los NPCs (si ya existen):
  - `RedStage_Scene`  -> Trapecista + `hilo_item`
  - `HallBlueRed_Scene` -> Pintacaritas1 + `aguja_item`
  - `Outside_Scene` -> Vendedor + `parche_item`
  - `CircusMaster_Scene` (o la escena principal del Maestro) -> Maestro prefab

Prefabs que debes crear/usar (nombres exactos)
----------------------------------------------
- `MaestroCeremonias` (prefab del NPC maestro)
- `NpcTrapecista` (si ya existe, usar y ajustar diálogo)
- `NpcVendedor` (usar / ajustar)
- `PintaCaritas1` o `PintaCaritas2` (usar / ajustar)
- Items (prefabs): `hilo_item`, `aguja_item`, `parche_item`

Cómo convertir tus imágenes a prefab de item (paso a paso en Unity)
-------------------------------------------------------------------
1. En Unity, en la ventana `Project` -> arrastra tu sprite (p. ej. `hilo.png`) a la escena.
2. En la jerarquía, renombra el GameObject a `hilo_item`.
3. Añade componentes al GameObject:
   - Sprite Renderer (asigna el sprite)
   - Collider 2D (BoxCollider2D) o Collider (BoxCollider) según proyecto; ajusta tamaño para cubrir la imagen. Recomiendo marcar `Is Trigger` si la interacción se hace por trigger.
   - (Opcional) Rigidbody2D (setear Body Type a `Kinematic`) si quieres detección por trigger estable.
   - Script `Item` (arrastrar `Assets/02_Scripts/Item.cs` al objeto).
4. Configura el `Item` inspector:
   - `interactUI`: arrastra el GameObject hijo que usas como indicador "Presiona E" (reusar uno de otra escena o crear uno nuevo como en otros items).
   - `itemIcon`: arrastra el mismo Sprite usado en el Sprite Renderer.
   - `missionToComplete`: pon el nombre de la misión que debe completarse al recoger este item (ver flujo más arriba):
       - `hilo_item` -> `BuscarHilo`
       - `aguja_item` -> `BuscarAguja`
       - `parche_item` -> `BuscarParche`
   - `missionToGive`: pon la misión que se añadirá al recogerlo:
       - `hilo_item` -> `BuscarAguja`
       - `aguja_item` -> `BuscarParche`
       - `parche_item` -> `VolverConMaestro`
   - `requiresMission`: CHECK (true) — así el item solo muestra el indicador si la misión previa está activa.
   - `lockedMessage`: (opcional) mensaje si el jugador intenta interactuar sin la misión.
5. En la jerarquía, crea un hijo llamado `InteractUI` (o reusa el que uses en otros items/NPCs). Ajusta su posición encima del item. Puedes copiar el estilo que ya usan `MissionTrigger`/`NPCBasicDialog`.
6. Arrastra `hilo_item` desde la jerarquía a `Assets/03_Prefabs/` para crear el prefab.
7. Repite para `aguja_item` y `parche_item`.

Cómo colocar items y NPCs en las escenas (qué tú haces)
-------------------------------------------------------
- En `RedStage_Scene`: arrastra el prefab `NpcTrapecista` a la posición deseada y coloca el prefab `hilo_item` cerca (puede ser hijo del NPC o en un pedestal al lado). Ajusta colisiones y visual.
- En `HallBlueRed_Scene`: coloca `PintaCaritas1` (o 2) y `aguja_item` cerca.
- En `Outside_Scene`: coloca `NpcVendedor` y `parche_item` cerca.
- En la escena del Maestro (`MaestroCeremonias_Level` o `CircusMaster_Scene`): coloca `MaestroCeremonias` en el centro de la plaza o donde quieras que comience la misión.

Qué configurar en el `MaestroCeremonias` prefab (lo que yo puedo dejar listo en texto para pegar)
---------------------------------------------------------------------------------------------
- Asegúrate de que el prefab del Maestro tenga uno de los scripts de NPC que ya existen (por ejemplo `NPCBasicDialog` o `MaestroCeremonias.cs`).
- En su componente de diálogo (Inspector):
  - `InitialMissionToGive` (o campo similar): poner `BuscarHilo` — esto hará que al hablar, el Maestro active la misión inicial.
  - Si el script permite `missionToComplete`, añadir la misión final `VolverConMaestro` para el diálogo de cierre (o en el diálogo final dejar que el `NPC` llame a `GameManager.Instance.CompleteMission("VolverConMaestro")`).

Diálogos sugeridos (cópialos en los campos de texto del NPC)
-----------------------------------------------------------
- Maestro (inicio):
  - "Hola, necesito que me ayudes a preparar el espectáculo. ¿Puedes traerme tres cosas? Un hilo, una aguja y un parche. Empieza por hablar con la trapecista en el Red Stage."
  - (MissionToGive) -> `BuscarHilo`

- Trapecista (al encontrarla en `RedStage_Scene`):
  - Texto inicial: "¡Eh! ¿Vienes por el hilo? Tengo uno aquí. Toma, pero cuida que lo necesitas para coser el vestuario."
  - (Ninguna misión extra desde el NPC; el item cercano controla la misión) — el item `hilo_item` hará Complete `BuscarHilo` y Add `BuscarAguja`.

- Pintacaritas (en `HallBlueRed_Scene`):
  - Texto: "¿Una aguja? Sí, tengo una. Tenla, que te vendrá bien para los remiendos."
  - El `aguja_item` completa `BuscarAguja` y da `BuscarParche`.

- Vendedor (en `Outside_Scene`):
  - Texto: "Ah, buscas un parche. Tengo uno por aquí. Toma, y vuelve con el maestro cuando tengas todo."
  - El `parche_item` completa `BuscarParche` y da `VolverConMaestro`.

- Maestro (final):
  - Texto final al tener `VolverConMaestro`: "¡Lo conseguiste! Gracias por traer todo. Ahora el show puede empezar."
  - Debe completar `VolverConMaestro` (marcar misión como completada).

Checklist de verificación (antes de probar)
-------------------------------------------
- [ ] Cada item prefab tiene `Item` script con `itemIcon` asignado y `interactUI` enlazado.
- [ ] `requiresMission` = true en los items y `missionToComplete`/`missionToGive` están exactamente como los nombres arriba.
- [ ] Maestro tiene `BuscarHilo` como misión dada al inicio.
- [ ] Items solo aparecen o son interactuables si la misión correspondiente está activa.
- [ ] Todos los prefabs están guardados en `Assets/03_Prefabs/`.

Qué puedo hacer yo (yo haré):
----------------------------
- Si quieres, creo un archivo con estos textos y el listado de campos exactos (ya hecho: este archivo) para que copies/pegues.
- Puedo también preparar archivos `.md` adicionales con extractos de diálogo listos para pegar o con instrucciones más visuales (qué botón pulsar) si lo prefieres.
- No puedo editar escenas .unity de forma segura desde aquí (binario), así que necesito que tú hagas la copia de la escena base y coloques los prefabs en las escenas. Yo te guío exactamente qué botones y campos editar.

Siguiente paso recomendado (qué tú haces ahora)
----------------------------------------------
1. En Unity: copia la escena `Vendedor_Level` a `MaestroCeremonias_Level` (File -> Save As).
2. Crea los prefabs `hilo_item`, `aguja_item`, `parche_item` siguiendo la sección "Cómo convertir tus imágenes a prefab".
3. Coloca cada item junto a su NPC en la escena correspondiente (RedStage, HallBlueRed, Outside).
4. Coloca el prefab `MaestroCeremonias` en la escena principal del Maestro.
5. Dime cuando hayas colocado los prefabs (o pega aquí el nombre exacto de los campos que te pide el inspector si ves nombres distintos) y yo te doy el texto final de los diálogos listos para pegar y reviso cualquier script que haga falta ajustar.

Notas técnicas y compatibilidad
-------------------------------
- El flujo usa el sistema `Item.cs` ya presente en el proyecto. Esto evita tocar la lógica del inventario o del GameManager.
- Si prefieres que los NPCs den el item directamente (sin que el jugador lo recoja del suelo) eso requiere pequeñas modificaciones al script del NPC para llamar a `InventoryManager.Instance.AddItem(sprite)`; puedo añadir un pequeño helper script si lo prefieres.

Si quieres, ahora mismo puedo también generar los textos de diálogo finales (cada bloque en formato listo para pegar) y crear un pequeño `TODO.md` con las tareas concretas para marcar en tu lista. ¿Qué prefieres ahora: (A) te doy los diálogos listos para pegar, o (B) te hago el helper script para que los NPCs den items directamente al hablar?