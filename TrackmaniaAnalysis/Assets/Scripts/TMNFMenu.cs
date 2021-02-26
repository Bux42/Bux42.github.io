using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TMNFMenu : MonoBehaviour
{
    public GameObject ScrollList;

    public Button BackButton;
    public Button NadeoTracksButton;
    public Button ClassicTracksButton;

    public Image NoSelectionImage;
    public Image LoadingImage;
    public Image SearchResultsImage;

    public int NadeoPage = 0;
    public int ClassicPage = 0;
    void Start()
    {
        DataManager.PreviousScenes.Add(SceneManager.GetActiveScene().name);

        BackButton.onClick.AddListener(BackButtonClick);

        NadeoTracksButton.onClick.AddListener(NadeoTracksButtonClick);
        ClassicTracksButton.onClick.AddListener(ClassicTracksButtonClick);
    }

    void Update()
    {
        
    }

    void BackButtonClick()
    {
        DataManager.PreviousScenes.Remove(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(DataManager.PreviousScenes.Last());
    }
    void NadeoTracksButtonClick()
    {
        NoSelectionImage.gameObject.SetActive(false);
        SearchResultsImage.gameObject.SetActive(false);
        LoadingImage.gameObject.SetActive(true);
        NadeoTracksButton.enabled = false;
        ClassicTracksButton.enabled = false;

        StartCoroutine(TMNFTrackRequest("https://raw.githubusercontent.com/Bux42/Bux42.github.io/main/Data/TrackLists/TMNF/NadeoTracks.json"));
    }
    void ClassicTracksButtonClick()
    {
        NoSelectionImage.gameObject.SetActive(false);
        SearchResultsImage.gameObject.SetActive(false);
        LoadingImage.gameObject.SetActive(true);
        NadeoTracksButton.enabled = false;
        ClassicTracksButton.enabled = false;
        StartCoroutine(TMNFTrackRequest("https://raw.githubusercontent.com/Bux42/Bux42.github.io/main/Data/TrackLists/TMNF/ClassicTracks.json"));
    }

    IEnumerator TMNFTrackRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {

            }
            else
            {
                TMNFListControls listControls = ScrollList.GetComponent<TMNFListControls>();
                int i = 0;
                Color[] colors = {
                    new Color(.1f, .1f, .1f),
                    new Color(.15f, .15f, .15f)
                };

                Debug.Log(webRequest.downloadHandler.text);
                TMNFTrackList tMNFTrackList = JsonUtility.FromJson<TMNFTrackList>(webRequest.downloadHandler.text);

                foreach (var track in tMNFTrackList.Tracks)
                {
                    TMNFTrack tMNFTrack = new TMNFTrack()
                    {
                        BestTime = track.WRTime,
                        BestTimeAuthor = track.WRAuthor,
                        TrackId = track.TrackId,
                        TrackName = track.TrackName,
                        TrackAuthor = track.TrackAuthor
                    };
                    listControls.AddItem(tMNFTrack, colors[i % 2]);
                    i++;
                }

                LoadingImage.gameObject.SetActive(false);
                SearchResultsImage.gameObject.SetActive(true);
                NadeoTracksButton.enabled = true;
                ClassicTracksButton.enabled = true;
            }
        }
    }
}
