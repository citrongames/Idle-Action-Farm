using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private int _fontSize;
    
    private float _deltaTime = 0.0f;
    GUIStyle textStyle = new GUIStyle();
    private GUIStyle _guiStyle = new GUIStyle();
    private int _width;
    private int _height;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _width = Screen.width;
        _height = Screen.height;
    }

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        Rect rect = new Rect(10, 400, _width, _fontSize);
        _guiStyle.alignment = TextAnchor.UpperLeft;
        _guiStyle.fontSize = _fontSize;
        _guiStyle.normal.textColor = Color.black;
        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;
        string text = "FPS: " + string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, _guiStyle);
        rect.y += rect.height;
        text = "FPS Limit: " + Application.targetFrameRate.ToString();
        GUI.Label(rect, text, _guiStyle);
        rect.y += rect.height;
        text = "VSync: " + QualitySettings.vSyncCount.ToString();
        GUI.Label(rect, text, _guiStyle);
        rect.y += rect.height;
        text = "Width: " + Screen.width.ToString() + " Height: " + Screen.height.ToString();
        GUI.Label(rect, text, _guiStyle);

    }

}