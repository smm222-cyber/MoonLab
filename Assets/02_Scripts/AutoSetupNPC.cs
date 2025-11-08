#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class AutoSetupNPC : MonoBehaviour
{
    public string npcName;
    public Sprite npcImage;
    public string dialogueText;
    public string missionRequired;
    public string missionToGive;
    public string missionToComplete;

    void Reset()
    {
        // Asegurarse de que tiene NPCBasicDialog
        var npc = GetComponent<NPCBasicDialog>();
        if (npc == null)
            npc = gameObject.AddComponent<NPCBasicDialog>();

        // Configurar el NPCBasicDialog
        var serializedObject = new SerializedObject(npc);
        
        // Activar sistema de misiones
        var usesMissionSystem = serializedObject.FindProperty("usesMissionSystem");
        usesMissionSystem.boolValue = true;

        // Configurar nombre y sprite
        var npcNameProp = serializedObject.FindProperty("npcName");
        var npcImageProp = serializedObject.FindProperty("npcImage");
        if (!string.IsNullOrEmpty(npcName))
            npcNameProp.stringValue = npcName;
        if (npcImage != null)
            npcImageProp.objectReferenceValue = npcImage;

        // Añadir misión si hay texto
        if (!string.IsNullOrEmpty(dialogueText))
        {
            var missions = serializedObject.FindProperty("missions");
            missions.arraySize = 1;
            var missionElement = missions.GetArrayElementAtIndex(0);
            
            var dialogueTextProp = missionElement.FindPropertyRelative("dialogueText");
            dialogueTextProp.stringValue = dialogueText;
            
            if (!string.IsNullOrEmpty(missionRequired))
            {
                var missionRequiredProp = missionElement.FindPropertyRelative("missionRequired");
                missionRequiredProp.stringValue = missionRequired;
            }
            
            if (!string.IsNullOrEmpty(missionToGive))
            {
                var missionToGiveProp = missionElement.FindPropertyRelative("missionToGive");
                missionToGiveProp.stringValue = missionToGive;
            }

            if (!string.IsNullOrEmpty(missionToComplete))
            {
                var missionToCompleteProp = missionElement.FindPropertyRelative("missionToComplete");
                missionToCompleteProp.stringValue = missionToComplete;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif