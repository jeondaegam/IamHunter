using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeColor : MonoBehaviour
{
    // Fill Amount 값에 따라 HpGauge 색상을 변경한다 .
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = Color.HSVToRGB(image.fillAmount / 3, 1, 1);

        // 색상(Hue), 채도(Saturation), 명도(Value)
        // Hue는 값이 0일땐 빨강, 1/3일땐 초록으로 표시된다.
        // fillAmount 값이 1일땐 초록색으로 표시한다
        // 따라서 fillAmount 값이 1일때 (1/3)로 만들어서 초록색으로 표시한다 ~ 
    }
}
