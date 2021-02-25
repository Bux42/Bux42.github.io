using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TMNFListItemTemplate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Text TrackNameText;
    [SerializeField]
    private Text BestTimeText;
    [SerializeField]
    private Text BestTimeAuthorText;

    private Color BaseColor;

    TMNFTrack TemplateTMNFTrack;

    public void InitTMNFTrack(TMNFTrack track, Color color)
    {
        TemplateTMNFTrack = track;

        BaseColor = color;

        TrackNameText.text = track.TrackName;
        BestTimeText.text = track.BestTime;
        BestTimeAuthorText.text = track.BestTimeAuthor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color hoverColor = new Color(BaseColor.r + .01f, BaseColor.g + .01f, BaseColor.b + .01f);
        GetComponent<Image>().color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = BaseColor;
    }
}
