using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TMNFListItemTemplate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Text TrackNameText;
    [SerializeField]
    private Text TrackAuthorText;
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
        TrackAuthorText.text = track.TrackAuthor;
        BestTimeText.text = track.BestTime;
        BestTimeAuthorText.text = track.BestTimeAuthor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string url = $"https://raw.githubusercontent.com/Bux42/Bux42.github.io/main/Data/TrackMaps/TMNF/Json/{TemplateTMNFTrack.TrackName}_{TemplateTMNFTrack.TrackId}.json";
        StartCoroutine(TMNFTrackJsonRequest(url));
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

    IEnumerator TMNFTrackJsonRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {

            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                DataManager.TMNFMap = JsonUtility.FromJson<ConvertedMapTMNF>(webRequest.downloadHandler.text);
                SceneManager.LoadScene("TMNFViewer");
            }
        }
    }
}
