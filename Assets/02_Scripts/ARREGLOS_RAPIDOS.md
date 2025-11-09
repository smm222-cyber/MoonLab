Arreglos rápidos para los problemas actuales:

1. Arreglar "Presionar E" en objetos:
   - Selecciona cada prefab (hilo_item, aguja_item, parche_item)
   - En el Inspector, componente `Item`:
     a. Crear hijo "InteractUI":
        - Click derecho en el prefab → Create Empty
        - Renombrar a "InteractUI"
        - Posición: Ajustar Y para que aparezca arriba del objeto
     b. En el hijo "InteractUI":
        - Add Component → Canvas
        - Add Component → Text (UI) o TextMeshPro
        - Escribir "Presiona E"
     c. En el componente `Item` del padre:
        - Arrastrar el hijo "InteractUI" al campo `Interact UI`

2. Arreglar diálogos vacíos:
   En cada NPC (Trapecista, Vendedor):
   - Seleccionar el prefab
   - En componente `NPCBasicDialog`:
     - Activar `Uses Mission System = true`
     - En `missions` lista, añadir una entrada:
       Para Trapecista:
       - `Dialogue Text`: "¡Eh! ¿Vienes por el hilo? Tengo uno aquí. Tenlo, pero cuida que lo necesitas para coser el vestuario."
       - `Mission Required`: "BuscarHilo"
       - Otros campos vacíos

       Para Vendedor:
       - `Dialogue Text`: "Ah, buscas un parche. Tengo uno por aquí. Toma, y vuelve con el maestro cuando tengas todo."
       - `Mission Required`: "BuscarParche"
       - Otros campos vacíos

3. Arreglar error de ConnectingController:
   - Seleccionar los objetos item (hilo, aguja, parche)
   - En Inspector:
     - Layer: cambiar a "Interactable" (si no existe, créalo)
     - Asegurarte que tienen Box Collider 2D o Circle Collider 2D
     - Marcar "Is Trigger" en el Collider

4. Verificar misiones:
   - En Maestro de Ceremonias:
     - NPCBasicDialog → Uses Mission System = true
     - Missions:
       1. Primera entrada para diálogo inicial:
          - Dialogue Text: "¡Hola! Necesito preparar el espectáculo..."
          - Mission To Give: "BuscarHilo"
       2. Segunda entrada para diálogo final:
          - Dialogue Text: "¡Lo conseguiste! Gracias..."
          - Mission Required: "VolverConMaestro"
          - Mission To Complete: "VolverConMaestro"

5. Items prefab settings finales:
   hilo_item:
   - Require Mission = true
   - Mission To Complete = "BuscarHilo"
   - Mission To Give = "BuscarAguja"
   - Layer = Interactable
   - Is Trigger = true

   aguja_item:
   - Require Mission = true
   - Mission To Complete = "BuscarAguja"
   - Mission To Give = "BuscarParche"
   - Layer = Interactable
   - Is Trigger = true

   parche_item:
   - Require Mission = true
   - Mission To Complete = "BuscarParche"
   - Mission To Give = "VolverConMaestro"
   - Layer = Interactable
   - Is Trigger = true

6. Checklist final:
   - [ ] Todos los items tienen InteractUI hijo con texto "Presiona E"
   - [ ] Todos los NPCs tienen Uses Mission System = true
   - [ ] Todos los items están en Layer "Interactable"
   - [ ] Todos los Colliders están como "Is Trigger"
   - [ ] Maestro tiene misión inicial configurada
   - [ ] Items tienen misiones Require/Complete/Give correctas