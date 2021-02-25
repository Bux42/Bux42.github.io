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

        StartCoroutine(NadeoTracksRequest());
    }
    void ClassicTracksButtonClick()
    {
        NoSelectionImage.gameObject.SetActive(false);
        SearchResultsImage.gameObject.SetActive(false);
        LoadingImage.gameObject.SetActive(true);
        NadeoTracksButton.enabled = false;
        ClassicTracksButton.enabled = false;
    }

    IEnumerator NadeoTracksRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://tmnforever.tm-exchange.com/main.aspx?action=tracksearch&mode=1&id=1001"))
        {
            webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
            // Request and wait for the desired page.

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {

            }
            else
            {
                TMNFListControls listControls = ScrollList.GetComponent<TMNFListControls>();

                Debug.Log(webRequest.downloadHandler.text);
                string trPattern = @"<tr[\s\S]*?<\/tr>";
                int i = 0;
                Color[] colors = {
                    new Color(.1f, .1f, .1f),
                    new Color(.15f, .15f, .15f)
                };
                foreach (Match m in Regex.Matches(webRequest.downloadHandler.text, trPattern))
                {
                    if (m.Value.Contains("WindowTableCell"))
                    {
                        string trackNamePattern = @"#auto"">(.*)<\/a><";
                        string trackName = Regex.Match(m.Value, trackNamePattern).Groups[1].Value;

                        string trackIdPattern = @"<a href=""main\.aspx\?action=trackshow&amp;id=(.*)#auto"" target=""_blank"">";
                        int trackId = int.Parse(Regex.Match(m.Value, trackIdPattern).Groups[1].Value);

                        string bestTimePattern = @"\d{1,}:\d{1,}.\d{1,}";
                        string bestTime = Regex.Match(m.Value, bestTimePattern, RegexOptions.Multiline).Groups.SyncRoot.ToString();

                        string bestTimeAuthorPattern = @"k"">(.*)<\/a><\/td><td><a OnMouseOver";
                        string bestTimeAuthor = Regex.Match(m.Value, bestTimeAuthorPattern).Groups[1].Value;

                        TMNFTrack track = new TMNFTrack()
                        {
                            BestTime = bestTime,
                            BestTimeAuthor = bestTimeAuthor,
                            TrackId = trackId,
                            TrackName = trackName
                        };
                        listControls.AddItem(track, colors[i % 2]);
                        Debug.Log(colors[i % 2]);
                        i++;
                    }
                }
                LoadingImage.gameObject.SetActive(false);
                SearchResultsImage.gameObject.SetActive(true);
                NadeoTracksButton.enabled = true;
                ClassicTracksButton.enabled = true;
            }
        }
    }
}
