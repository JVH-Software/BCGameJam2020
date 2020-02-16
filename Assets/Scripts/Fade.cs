using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image;

    public void FadeOut() {
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade() {
        image = GetComponent<Image>();
        while (image.color.a < 255) {
            var tempColor = image.color;
            tempColor.a += Time.deltaTime / 2;
            image.color = tempColor;
            yield return null;
        }
        yield return null;
    }
}
