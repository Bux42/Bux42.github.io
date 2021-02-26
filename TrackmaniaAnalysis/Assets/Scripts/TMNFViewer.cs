using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TMNFViewer : MonoBehaviour
{
    public GameObject MapBlockTextTemplate;

    public float RightClickSpeedH = 2.0f;
    public float RightClickSpeedV = 2.0f;
    public float RightClickYaw = 0.0f;
    public float RightClickPitch = 0.0f;
    void Start()
    {
        DataManager.PreviousScenes.Add(SceneManager.GetActiveScene().name);
        if (DataManager.TMNFMap != null)
        {
            LoadMapBlocks();
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

    void LoadMapBlocks()
    {
        Vector3 posDiff = Vector3.zero;
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
