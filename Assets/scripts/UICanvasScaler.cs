using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasScaler : MonoBehaviour
{
    float UIHeight;
    float UIWidth;
    RectTransform rt;
    Text txt;
    public RectTransform topAnchor;
    public RectTransform bottomAnchor;
    public RectTransform resultBar;
    public RectTransform infoBar;
    public Button restart;
    public Button nextLevel;
    public Button menu;
    public Button mode;
    public Button confirm;
    public Button nextTutorial;
    public Text countertxt;
    public Text leveltxt;
    public Text rekordtxt;
    public RectTransform HowToPlayLesson;
    public Canvas TutorialCanvas;

    void Start()
    {
        UIHeight = Screen.height * (cameraWidth.mainCamera.orthographicSize - 28f) / (2 * cameraWidth.mainCamera.orthographicSize);
        UIWidth = Screen.width;
        //settin top anchor area
        rt = topAnchor.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, UIHeight);
        //setting bottom anchor area
        rt = bottomAnchor.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, UIHeight);
        //settin result bar area
        rt = resultBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, UIHeight/4);
        resultBar.anchoredPosition = new Vector2(0, -UIHeight / 2);
        //settin info bar area
        rt = infoBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, UIHeight / 2);

        //buttons through all width and height of 1/4
        rt = restart.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth/3, UIHeight / 4);
        rt = nextLevel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, UIHeight / 4);
        rt = menu.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, UIHeight / 4);
        rt = mode.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, UIHeight / 4);
        rt = confirm.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, UIHeight / 4);
        

        //text sizes and fonts
        rt = countertxt.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, rt.sizeDelta.y);
        rt = leveltxt.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, rt.sizeDelta.y);
        rt = rekordtxt.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, rt.sizeDelta.y);
        //setting all fonts in canvas to the same font, font size depend on screen size
        int newFontSize = (int)(UIWidth * 50 / 1080);
        Canvas canvas = GetComponent<Canvas>();
        Text[] textComponents = canvas.GetComponentsInChildren<Text>(includeInactive: true);
        foreach (Text textComponent in textComponents)
        {
            textComponent.fontSize = newFontSize;
        }


        //How to play canvas
        rt = HowToPlayLesson.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth, 3 * UIHeight / 4);
        rt = nextTutorial.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(UIWidth / 3, UIHeight / 4);
        canvas = TutorialCanvas.GetComponent<Canvas>();
        textComponents = canvas.GetComponentsInChildren<Text>(includeInactive: true);
        foreach (Text textComponent in textComponents)
        {
            textComponent.fontSize = newFontSize;
        }
    }

}
