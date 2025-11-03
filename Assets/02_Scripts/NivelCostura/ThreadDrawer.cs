using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreadDrawer : MonoBehaviour
{
    [Header("Parent for Lines")]
    public RectTransform hiloParent; // Debe ser un RectTransform dentro del Canvas

    [Header("Line Settings")]
    public float lineWidth = 5f; // en píxeles
    public Color lineColor = Color.red;

    private List<GameObject> drawnLines = new List<GameObject>();
    private int maxNormalLines = 5;
    private int maxInvertedLines = 5;

    void Update()
    {
        if (!enabled || hiloParent == null) return;

        // Dibujar una línea al presionar L
        if (Input.GetKeyDown(KeyCode.L))
        {
            int totalLimit = maxNormalLines + maxInvertedLines;

            if (drawnLines.Count < totalLimit)
            {
                if (drawnLines.Count < maxNormalLines)
                {
                    // Primeras 5 líneas (normales)
                    Vector3 start = new Vector3(100 + drawnLines.Count * 40, 100, 0);
                    Vector3 end = new Vector3(300 + drawnLines.Count * 40, 300, 0);
                    DrawLine(start, end);
                }
                else
                {
                    // Siguientes 5 líneas (invertidas)
                    int i = drawnLines.Count - maxNormalLines; // índice 0 a 4
                    Vector3 start = new Vector3(300 + i * 40, 300, 0);
                    Vector3 end = new Vector3(100 + i * 40, 100, 0);
                    DrawLine(start, end, Color.blue); // color diferente para distinguir
                }
            }
            else
            {
                Debug.Log("Ya se han dibujado las 10 líneas (5 normales + 5 invertidas).");
            }
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Color? customColor = null)
    {
        GameObject lineGO = new GameObject("ThreadLine", typeof(RectTransform), typeof(Image));
        lineGO.transform.SetParent(hiloParent, false);

        Image img = lineGO.GetComponent<Image>();
        img.color = customColor ?? lineColor;

        RectTransform rt = lineGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0, 0);
        rt.position = (start + end) / 2f;
        rt.sizeDelta = new Vector2(Vector3.Distance(start, end), lineWidth);

        Vector3 dir = end - start;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle);

        drawnLines.Add(lineGO);
    }

    public void ClearLines()
    {
        foreach (GameObject line in drawnLines)
        {
            if (line != null)
                Destroy(line);
        }
        drawnLines.Clear();
    }
}
