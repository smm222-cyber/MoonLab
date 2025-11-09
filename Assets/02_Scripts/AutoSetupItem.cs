#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class AutoSetupItem : MonoBehaviour
{
    void Reset()
    {
        // Asegurarse de que tiene los componentes necesarios
        var item = GetComponent<Item>();
        if (item == null)
            item = gameObject.AddComponent<Item>();

        // Asegurarse de que tiene un collider
        var collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            var boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = true;
        }

        // Poner en el layer correcto
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        // Crear el InteractUI si no existe
        Transform interactUI = transform.Find("InteractUI");
        if (interactUI == null)
        {
            // Crear el objeto UI
            var uiGo = new GameObject("InteractUI");
            uiGo.transform.SetParent(transform);
            uiGo.transform.localPosition = new Vector3(0, 1f, 0); // Ajusta esto según necesites

            // Añadir Canvas
            var canvas = uiGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 1;

            // Crear el texto
            var textGo = new GameObject("Text");
            textGo.transform.SetParent(uiGo.transform);
            textGo.transform.localPosition = Vector3.zero;
            var text = textGo.AddComponent<UnityEngine.UI.Text>();
            text.text = "Presiona E";
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 14;

            // Asignar el InteractUI al componente Item
            var itemComponent = GetComponent<Item>();
            if (itemComponent != null)
            {
                var serializedObject = new SerializedObject(itemComponent);
                var interactUIProperty = serializedObject.FindProperty("interactUI");
                interactUIProperty.objectReferenceValue = uiGo;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif