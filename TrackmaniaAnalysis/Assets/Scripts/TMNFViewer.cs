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
    public GameObject GhostCubeTemplate;

    public Button BackButton;
    public Button RandomizeColorsButton;
    public GameObject GhostScrollList;
    public Slider LineThicknessSlider;
    public Toggle ShowBlocksToggle;
    public Text TrackNameText;

    public Button PlayButton;
    public Button PauseButton;
    public Button StopButton;

    public int PlayerIndex = 0;
    public bool Playing = false;

    public float RightClickSpeedH = 2.0f;
    public float RightClickSpeedV = 2.0f;
    public float RightClickYaw = 0.0f;
    public float RightClickPitch = 0.0f;
    void Start()
    {
        DataManager.PreviousScenes.Add(SceneManager.GetActiveScene().name);

        if (DataManager.MoveKeys == null)
        {
            DataManager.KeyboardLayoutChanged(PlayerPrefs.GetInt("KeyboardLayout"));
        }

        LineThicknessSlider.value = PlayerPrefs.GetFloat("LineThickness");

        BackButton.onClick.AddListener(BackButtonClick);
        RandomizeColorsButton.onClick.AddListener(RandomizeColorsButtonClick);
        LineThicknessSlider.onValueChanged.AddListener(LineThicknessSliderValueChanged);
        ShowBlocksToggle.onValueChanged.AddListener(delegate {
            ShowBlocksToggleValueChanged();
        });

        PlayButton.onClick.AddListener(PlayButtonClick);
        PauseButton.onClick.AddListener(PauseButtonClick);
        StopButton.onClick.AddListener(StopButtonClick);

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
            ghostData.LineRenderer.SetWidth(PlayerPrefs.GetFloat("LineThickness"), PlayerPrefs.GetFloat("LineThickness"));
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

            ghostData.GhostCube = Instantiate(GhostCubeTemplate);
            ghostData.GhostCube.name = $"GhostCube_{ghostData.PlayerName}";
            ghostData.GhostCube.transform.position = ghostData.Samples[0].Position;
            ghostData.GhostCube.transform.eulerAngles = PitchYawRollToEulerAngles(ghostData.Samples[0].PitchYawRoll);
            Material mymat = ghostData.GhostCube.GetComponent<Renderer>().material;
            mymat.color = new Color(color.r, color.g, color.b, .5f);
            ghostData.GhostCube.SetActive(false);
            ghostListControls.AddGhost(ghostData);
        }

        if (DataManager.TMNFMap != null)
        {
            TrackNameText.text = DataManager.TMNFMap.MapId;
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
        if (DataManager.TMNFMapGhosts.Count > 0)
        {
            TMNFSampleData first = DataManager.TMNFMapGhosts.First();
            Vector3 pos = first.Samples[0].Position;
            pos.y += 5;
            Camera.main.transform.position = pos;
            Camera.main.transform.eulerAngles = PitchYawRollToEulerAngles(first.Samples[0].PitchYawRoll);
        }
        InvokeRepeating("GhostPlayer", 0, .05f);
    }

    public Vector3 PitchYawRollToEulerAngles(Vector3 pitchYawRoll)
    {
        Vector3 ret = new Vector3(
            pitchYawRoll.x * Mathf.Rad2Deg,
            pitchYawRoll.y * -Mathf.Rad2Deg,
            pitchYawRoll.z * Mathf.Rad2Deg);
        return (ret);
    }
    public void GhostPlayer()
    {
        if (Playing)
        {
            foreach (var ghostData in DataManager.TMNFMapGhosts)
            {
                if (ghostData.Samples.Count > PlayerIndex)
                {
                    ghostData.GhostCube.transform.position = ghostData.Samples[PlayerIndex].Position;
                    ghostData.GhostCube.transform.eulerAngles = PitchYawRollToEulerAngles(ghostData.Samples[PlayerIndex].PitchYawRoll);
                }
            }
            PlayerIndex++;
        }
    }

    private void StopButtonClick()
    {
        foreach (var ghostData in DataManager.TMNFMapGhosts)
        {
            ghostData.GhostCube.SetActive(false);
        }
        Playing = false;
        PlayerIndex = 0;
    }

    private void PauseButtonClick()
    {
        Playing = false;
    }

    private void PlayButtonClick()
    {
        foreach (var ghostData in DataManager.TMNFMapGhosts)
        {
            ghostData.GhostCube.SetActive(true);
        }
        Playing = true;
    }

    private void ShowBlocksToggleValueChanged()
    {
        if (ShowBlocksToggle.isOn)
        {
            foreach (var block in DataManager.TMNFMap.Blocks)
                block.Text3D.SetActive(true);
        }
        else
        {
            foreach (var block in DataManager.TMNFMap.Blocks)
                block.Text3D.SetActive(false);
        }
    }

    private void LineThicknessSliderValueChanged(float arg0)
    {
        PlayerPrefs.SetFloat("LineThickness", LineThicknessSlider.value);
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
        if (Input.GetKey(DataManager.MoveKeys[0]))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition + Camera.main.transform.forward;
        if (Input.GetKey(DataManager.MoveKeys[1]))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition - Camera.main.transform.forward;
        if (Input.GetKey(DataManager.MoveKeys[2]))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition - Camera.main.transform.right;
        if (Input.GetKey(DataManager.MoveKeys[3]))
            Camera.main.transform.localPosition = Camera.main.transform.localPosition + Camera.main.transform.right;

        float test = Input.GetAxis("Vertical");

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

            Material mymat = ghostData.GhostCube.GetComponent<Renderer>().material;
            mymat.color = new Color(color.r, color.g, color.b, .5f);

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
