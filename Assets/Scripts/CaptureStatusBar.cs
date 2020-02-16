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

    public double minimum;
    public double maximum = 1f;
    public double current;
    public Image mask;
    public ControlNodeObject capPoint;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current = capPoint.node.GetCapturePercentage();
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        double currentOffset = current - minimum;
        double maximumOffset = maximum - minimum;
        double fillAmount = currentOffset / maximum;
        mask.fillAmount = (float)fillAmount;
    }
}
