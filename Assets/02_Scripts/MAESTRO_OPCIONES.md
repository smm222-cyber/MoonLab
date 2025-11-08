// Configuración para el Maestro de Ceremonias
// En el componente NPCBasicDialog:

1. Diálogo Inicial:
- usesMissionSystem = true
- En la lista `missions`, primera entrada:
  * Dialogue Text: "¡Hola! Necesito que me ayudes a preparar el espectáculo. ¿Puedes traerme tres cosas? Un hilo, una aguja y un parche. Empieza por hablar con la trapecista en el Red Stage."
  * Mission To Give: "BuscarHilo"
  * Mission Required: (vacío)
  * Mission To Complete: (vacío)
  * Has Choices: false

2. Diálogo Final con Opciones:
- En la lista `missions`, segunda entrada:
  * Dialogue Text: "¡Lo conseguiste! Ahora tienes dos opciones..."
  * Mission Required: "VolverConMaestro"
  * Mission To Complete: (vacío)
  * Mission To Give: (vacío)
  * Has Choices: true
  * Choices:
    - Choice 1:
      * Choice Text: "Entregar los materiales"
      * Response Dialogue: "¡Gracias por traer todo! Ahora el show puede empezar."
      * Mission To Complete: "VolverConMaestro"
    - Choice 2:
      * Choice Text: "Abrir portal secreto"
      * Response Dialogue: "¡El portal se ha abierto! Una nueva aventura te espera..."
      * Mission To Complete: "VolverConMaestro"

3. Asegúrate que el GameObject que tiene el NPCBasicDialog:
   - Tiene asignado en el campo `choices UI` el objeto que tiene el componente `DialogueChoicesUIFixed`
   - Tiene su imagen asignada en `npc Image`
   - Tiene "Maestro de Ceremonias" en `npc Name`

Nota: Para que las opciones funcionen, necesitas tener en la escena el prefab que tiene el `DialogueChoicesUIFixed` (el panel de opciones).