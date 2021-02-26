using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostListControls : MonoBehaviour
{
    [SerializeField]
    private GameObject GhostItemTemplate;

    public List<GameObject> GhostItems = new List<GameObject>();
    void Start()
    {

    }

    internal void AddGhost(TMNFSampleData ghost)
    {
        GameObject ghostTemplate = Instantiate(GhostItemTemplate) as GameObject;
        ghostTemplate.SetActive(true);
        ghostTemplate.name = ghost.PlayerName;
        ghostTemplate.GetComponent<GhostListItemTemplate>().InitGhostData(ghost);
        ghostTemplate.transform.SetParent(GhostItemTemplate.transform.parent, false);
        GhostItems.Add(ghostTemplate);
    }
}
