using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GhostListItemTemplate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Text GhostNameText;
    [SerializeField]
    private Text GhostTimeText;
    [SerializeField]
    private Text GhostTopSpeedText;

    private TMNFSampleData SampleData;
    internal void InitGhostData(TMNFSampleData ghostData)
    {
        SampleData = ghostData;

        string ghostTimeStr = ghostData.RaceTime.ToString();
        GhostTimeText.text = ghostTimeStr.Substring(0, ghostTimeStr.Length - 4);

        string pattern = @"\$[nmwoszi]|\$[hl]\[[a-zA-Z0-9\/?#!&\.\\\-_=@$'()+,;:]*\]|\$[0-f]{3}";
        string substitution = @"";
        RegexOptions options = RegexOptions.Multiline;

        Regex regex = new Regex(pattern, options);
        string result = regex.Replace(ghostData.PlayerName, substitution);

        GhostNameText.text = result;
        GhostNameText.color = ghostData.Color;
        GhostTopSpeedText.text = $"Top Speed: {ghostData.MaxSpeed}";
        /*
        GhostDisplayToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(GhostDisplayToggle);
        });
        */
    }

    public void SetColor(Color color)
    {
        GhostNameText.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        float alpha = 1;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(SampleData.Color, 0.0f), new GradientColorKey(SampleData.Color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        SampleData.LineRenderer.colorGradient = gradient;
        Material mymat = SampleData.GhostCube.GetComponent<Renderer>().material;
        mymat.color = new Color(SampleData.Color.r, SampleData.Color.g, SampleData.Color.b, alpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        float alpha = .5f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(SampleData.Color, 0.0f), new GradientColorKey(SampleData.Color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        SampleData.LineRenderer.colorGradient = gradient;
        Material mymat = SampleData.GhostCube.GetComponent<Renderer>().material;
        mymat.color = new Color(SampleData.Color.r, SampleData.Color.g, SampleData.Color.b, alpha);
    }
}
