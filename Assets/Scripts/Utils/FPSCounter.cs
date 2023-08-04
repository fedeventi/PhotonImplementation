using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public bool viewFPS;

    private int screenLongSide;
    private Rect boxRect;
    private GUIStyle style = new GUIStyle();

    private int frameCount;
    private float elapsedTime;
    private double frameRate;

    [Range(20,100)]
    public float fontSize;
    float prevSize;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        UpdateUISize();
        prevSize = fontSize;
    }


    private void Update()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);
            frameCount = 0;
            elapsedTime = 0;
            if (screenLongSide != Mathf.Max(Screen.width, Screen.height) || prevSize != fontSize)
            {
                UpdateUISize();
            }
        }
    }


    private void UpdateUISize()
    {
        screenLongSide = Mathf.Max(Screen.width, Screen.height);
        var rectLongSide = screenLongSide / 10;
        boxRect = new Rect(1, 1, screenLongSide, rectLongSide / 3);
        style.fontSize = (int)(screenLongSide / fontSize);
        style.normal.textColor = Color.green;
        style.alignment = TextAnchor.MiddleRight;
    }


    private void OnGUI()
    {
        if(viewFPS)
            GUI.Label(boxRect, " " + frameRate + "fps", style);
    }
}
