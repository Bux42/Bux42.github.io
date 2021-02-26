using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour
{
    public Image SquareImage;
    public Text LoadingText;
    float currentValue;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        speed = 50;
    }

    // Update is called once per frame
    void Update()
    {
        currentValue += speed * Time.deltaTime;
        currentValue %= 100;
        SquareImage.fillAmount = currentValue / 100;
    }

    public void SetLoadingText(string text)
    {
        LoadingText.text = text;
    }
}
