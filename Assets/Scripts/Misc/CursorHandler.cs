using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    public static CursorHandler instance; 

    [SerializeField] Texture2D gameplayCursor;
    [SerializeField] Texture2D menuCursor;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    public void SetMenuCursor()
    {
        Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetGameplayCursor()
    {
        Cursor.SetCursor(gameplayCursor, Vector2.zero, CursorMode.Auto);
    }
}
