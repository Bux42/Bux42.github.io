using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TMNFMenu : MonoBehaviour
{
    public Button NadeoTracksButton;
    public Button ClassicTracksButton;

    public Image NoSelectionImage;
    public Image LoadingImage;

    public int NadeoPage = 0;
    public int ClassicPage = 0;
    void Start()
    {
        DataManager.PreviousScenes.Add(SceneManager.GetActiveScene().name);

        NadeoTracksButton.onClick.AddListener(NadeoTracksButtonClick);
        ClassicTracksButton.onClick.AddListener(ClassicTracksButtonClick);
    }

    void Update()
    {
        
    }
    void NadeoTracksButtonClick()
    {
        NoSelectionImage.gameObject.SetActive(false);
        LoadingImage.gameObject.SetActive(true);
        NadeoTracksButton.enabled = false;
        ClassicTracksButton.enabled = false;

        StartCoroutine(NadeoTracksRequest());
    }
    void ClassicTracksButtonClick()
    {
        NoSelectionImage.gameObject.SetActive(false);
        LoadingImage.gameObject.SetActive(true);
        NadeoTracksButton.enabled = false;
        ClassicTracksButton.enabled = false;
    }

    IEnumerator NadeoTracksRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://tmnforever.tm-exchange.com/main.aspx?action=tracksearch&mode=1&id=1001"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {

            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                string trPattern = @"<tr[\s\S]*?<\/tr>";
                foreach (Match m in Regex.Matches(webRequest.downloadHandler.text, trPattern))
                {
                    if (m.Value.Contains("WindowTableCell"))
                    {
                        string trackNamePattern = @"#auto"">(.*)<\/a><";
                        string trackName = Regex.Match(m.Value, trackNamePattern).Groups[1].Value;

                        string trackIdPattern = @"<a href=""main\.aspx\?action=trackshow&amp;id=(.*)#auto"" target=""_blank"">";
                        int trackId = int.Parse(Regex.Match(m.Value, trackIdPattern).Groups[1].Value);

                        string bestTimePattern = @"<i class=""icon-time""><\/i>(" + '\n' + @".*)<\/td><td><a h";
                        string bestTime = Regex.Match(m.Value, bestTimePattern, RegexOptions.Multiline).Groups[1].Value;
                        Debug.Log(bestTime);
                    }
                }
                LoadingImage.gameObject.SetActive(false);
                NadeoTracksButton.enabled = true;
                ClassicTracksButton.enabled = true;
            }
        }
    }
}
