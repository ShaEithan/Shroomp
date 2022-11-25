using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherTracker : MonoBehaviour
{
    public GameObject heartTemplate;
    private List<GameObject> feather = new List<GameObject>();
    ShroompController shroompController;
    public int featherCount;
    public int dashesLeft;
    //public int chunk, remainder;
    public float itemOffset = 60f;
    // Start is called before the first frame update
    void Start()
    {
        shroompController = FindObjectOfType<ShroompController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(shroompController.dashes != featherCount)
           // createContainers();
        featherCount = shroompController.dashAmmount;
        dashesLeft = shroompController.dashes;
        //chunk = feathers / 2;
        //remainder = feathers % 2;
       // var oldAmount = featherCount;
       //createContainers();
    }
    private int x;
    private int y;
    private void sortPosition()
    {
        x = 0;
        foreach (var item in feather)
        {
            feather[x].SetActive(true);
            feather[x].GetComponent<RectTransform>().anchoredPosition = new Vector2((x * itemOffset), (y * itemOffset));
            x++;
        }
    }
    public void createContainers()
    {
        //get rid of any old feather objects
        for (int i = 0; i < feather.Count; i++)
        {
            Destroy(feather[i].transform.gameObject);
        }
        feather.Clear();
        for (int i = 0; i < shroompController.dashes; i++)
        {
            feather.Add(Instantiate(heartTemplate, transform));
            feather[feather.Count - 1].GetComponent<FeatherContainer>().count = 1;
        }
        for (int i = 0; i < (shroompController.dashAmmount - shroompController.dashes); i++)
        {
            feather.Add(Instantiate(heartTemplate, transform));
            feather[feather.Count - 1].GetComponent<FeatherContainer>().count = 2;
        }

        sortPosition();
    }
}
