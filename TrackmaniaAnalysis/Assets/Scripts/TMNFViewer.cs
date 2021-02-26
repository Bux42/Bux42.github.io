using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TMNFViewer : MonoBehaviour
{
    public GameObject MapBlockTextTemplate;
    public Button BackButton;
    public Button RandomizeColorsButton;
    public GameObject GhostScrollList;
    public Slider LineThicknessSlider;

    public float RightClickSpeedH = 2.0f;
    public float RightClickSpeedV = 2.0f;
    public float RightClickYaw = 0.0f;
    public float RightClickPitch = 0.0f;
    void Start()
    {
        DataManager.PreviousScenes.Add(SceneManager.GetActiveScene().name);

        BackButton.onClick.AddListener(BackButtonClick);
        RandomizeColorsButton.onClick.AddListener(RandomizeColorsButtonClick);
        LineThicknessSlider.onValueChanged.AddListener(LineThicknessSliderValueChanged);

        GhostListControls ghostListControls = GhostScrollList.GetComponent<GhostListControls>();

        Vector3 startPos = Vector3.zero;
        foreach (var ghostData in DataManager.TMNFMapGhosts)
        {
            foreach (var sample in ghostData.Samples)
                sample.Position.x *= -1;

            ghostData.LineRenderer = new GameObject().AddComponent<LineRenderer>();
            ghostData.LineRenderer.name = $"LineRenderer_{ghostData.PlayerName}";
            ghostData.LineRenderer.positionCount = ghostData.Samples.Count;
            for (int i = 0; i < ghostData.Samples.Count; i++)
            {
                if (startPos == Vector3.zero)
                    startPos = ghostData.Samples[i].Position;
                ghostData.LineRenderer.SetPosition(i, ghostData.Samples[i].Position);
            }
            ghostData.LineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            ghostData.LineRenderer.SetWidth(1, 1);
            float alpha = 0.5f;
            Gradient gradient = new Gradient();
            Color color = new Color(
                      UnityEngine.Random.Range(0f, 1f),
                      UnityEngine.Random.Range(0f, 1f),
                      UnityEngine.Random.Range(0f, 1f)
                  );

            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            ghostData.LineRenderer.colorGradient = gradient;
            ghostData.Color = color;
            ghostListControls.AddGhost(ghostData);
        }

        if (DataManager.TMNFMap != null)
        {
            MapBlockTMNF startMapBlock = DataManager.TMNFMap.Blocks.Find(x => x.Name == "StadiumRoadMainStartLine");
            Vector3 posDiff = Vector3.zero;

            if (startMapBlock != null)
            {
                Vector3 inverted = startMapBlock.Coord;
                inverted.x *= -1;
                posDiff = startPos - inverted;
                Debug.Log($"StartPos: {startPos}, StartBlockPos: {inverted}, Diff: {startPos - inverted}");
            }
            LoadMapBlocks(posDiff);
        }
    }

    private void LineThicknessSliderValueChanged(float arg0)
    {
        foreach (var ghostData in DataManager.TMNFMapGhosts)
        {
            ghostData.LineRenderer.SetWidth(LineThicknessSlider.value, LineThicknessSlider.value);
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            RightClickYaw += RightClickSpeedH * Input.GetAxis("Mouse X");
            RightClickPitch -= RightClickSpeedV * Input.GetAxis("Mouse Y");
            Camera.main.transform.eulerAngles = new Vector3(RightClickPitch, RightClickYaw, 0.0f);
        }
        if (Input.GetKey(KeyCode.Z))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition + Camera.main.transform.forward;
        if (Input.GetKey(KeyCode.S))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition - Camera.main.transform.forward;
        if (Input.GetKey(KeyCode.Q))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition - Camera.main.transform.right;
        if (Input.GetKey(KeyCode.D))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition + Camera.main.transform.right;

        if (DataManager.TMNFMap != null)
        {
            foreach (var block in DataManager.TMNFMap.Blocks)
            {
                var mesh = block.Text3D.GetComponent<TextMesh>();
                mesh.transform.eulerAngles = Camera.main.transform.eulerAngles;
            }
        }
    }
    void RandomizeColorsButtonClick()
    {
        GhostListControls ghostListControls = GhostScrollList.GetComponent<GhostListControls>();
        foreach (var ghostData in DataManager.TMNFMapGhosts)
        {
            float alpha = 0.5f;
            Gradient gradient = new Gradient();
            Color color = new Color(
                      UnityEngine.Random.Range(0f, 1f),
                      UnityEngine.Random.Range(0f, 1f),
                      UnityEngine.Random.Range(0f, 1f)
                  );

            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            ghostData.LineRenderer.colorGradient = gradient;
            ghostData.Color = color;

            GameObject itemTemplate = ghostListControls.GhostItems.Find(x => x.name == ghostData.PlayerName);

            if (itemTemplate != null)
            {
                itemTemplate.GetComponent<GhostListItemTemplate>().SetColor(ghostData.Color);
            }
        }
    }

    void BackButtonClick()
    {
        DataManager.PreviousScenes.Remove(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(DataManager.PreviousScenes.Last());
    }

    void LoadMapBlocks(Vector3 posDiff)
    {
        foreach (var block in DataManager.TMNFMap.Blocks)
        {
            block.Text3D = Instantiate(MapBlockTextTemplate);
            block.Text3D.name = block.Name;
            var mesh = block.Text3D.GetComponent<TextMesh>();
            block.Coord.x *= -1;
            block.Coord += posDiff;
            if (block.Name == "StadiumPlatformCheckpoint")
                mesh.color = Color.blue;
            if (block.Name == "StadiumRoadMainFinishLine")
                mesh.color = Color.red;
            if (block.Name == "StadiumRoadMainStartLine")
                mesh.color = Color.green;
            mesh.text = $"{block.Name}";
            mesh.transform.position = block.Coord;
        }
    }
}
