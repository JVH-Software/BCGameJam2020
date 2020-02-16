using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CaptureStatusBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Capture Status Bar")]
    public static void AddCaptureStatusBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Capture Status Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform);
    }
#endif

    public float minimum;
    public float maximum = 1f;
    public float current;
    public Image mask;
    public Image statusImg;
    public CapturePoint capPoint;


    // arr[0] = neutral, arr[1] = player, arr[2+] = enemies;
    public Sprite[] backgroundArray;
    public Sprite[] foregroundArray;
    private Sprite background;
    private Sprite foreground;
    private const float BLINK_DURATION = 0.5f;
    private bool isBlinking = false;

    private string previousCapturer;
    private string previousOwner;

    // Start is called before the first frame update
    void Start()
    {
        background = backgroundArray[0];
        foreground = foregroundArray[0];
        statusImg.overrideSprite = background;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();

        if (capPoint.IsBeingCaptured() || capPoint.isContested())
        {
            SwitchImages();
            if (capPoint.IsOwned() && capPoint.owner != previousOwner)
            {
                previousOwner = capPoint.owner;
            }

            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(Blink());
            }

            previousCapturer = capPoint.capturerName;
        }

    }

    void GetCurrentFill()
    {
        current = capPoint.ownership;
        float currentOffset = current - minimum;
        float fillAmount = currentOffset / maximum;
        mask.fillAmount = fillAmount;
    }
    
    void SwitchImages()
    {
        if (capPoint.capturerName == previousCapturer) { return; }

        switch (capPoint.capturerName)
        {
            case "Player":
                background = backgroundArray[1];
                foreground = foregroundArray[1];
                break;
            case "AlphaPack":
                background = backgroundArray[2];
                foreground = foregroundArray[2];
                break;
            default:
                background = backgroundArray[0];
                foreground = foregroundArray[0];
                break;
        }
    }

    IEnumerator Blink()
    {
        while (capPoint.IsBeingCaptured() || capPoint.isContested())
        {
            if (statusImg.overrideSprite == background)
            {
                statusImg.overrideSprite = foreground;
            } else
            {
                statusImg.overrideSprite = background;
            }
            yield return new WaitForSeconds(BLINK_DURATION);
        }
        isBlinking = false;
    }
}
