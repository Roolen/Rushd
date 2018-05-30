using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorElement : MonoBehaviour
{
    public int typeElement;

    private EditorManager editor;

    private Color stableColor;

    private void Start()
    {
        stableColor = GetComponent<Renderer>().material.color;

        editor = GameObject.FindObjectOfType<EditorManager>();
    }

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.cyan;
        Renderer[] rends = GetComponentsInChildren<Renderer>().ToArray();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = Color.cyan;
        }
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = stableColor;

        Renderer[] rends = GetComponentsInChildren<Renderer>().ToArray();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = stableColor;
        }
    }

    private void OnMouseDown()
    {
        if (editor.TypeSelectElement == typeElement)
        {
            Transform nowPosition = gameObject.transform;

            GameObject instNewElement = Instantiate(editor.SelectElement, nowPosition.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }


}
