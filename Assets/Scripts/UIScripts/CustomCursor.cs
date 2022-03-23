using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] Texture2D customCursor;
    [SerializeField] Vector2 cursorOffset;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(customCursor, cursorOffset, CursorMode.Auto);
    }
}
