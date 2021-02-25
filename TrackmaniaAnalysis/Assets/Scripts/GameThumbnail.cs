using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameThumbnail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RawImage Image;

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("TMNFMenu");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color currColor = Image.color;
        currColor.a = .5f;
        Image.color = currColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color currColor = Image.color;
        currColor.a = 1;
        Image.color = currColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
