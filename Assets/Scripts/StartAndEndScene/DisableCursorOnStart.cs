using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCursorOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}
