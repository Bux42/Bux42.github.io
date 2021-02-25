using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMNFListControls : MonoBehaviour
{
    [SerializeField]
    private GameObject ListItemTemplate;
    void Start()
    {
        
    }

    public void AddItem(TMNFTrack track, Color color)
    {
        GameObject itemTemplate = Instantiate(ListItemTemplate) as GameObject;
        itemTemplate.SetActive(true);
        itemTemplate.GetComponent<Image>().color = color;
        itemTemplate.GetComponent<TMNFListItemTemplate>().InitTMNFTrack(track, color);
        itemTemplate.transform.SetParent(ListItemTemplate.transform.parent, false);
    }

}
