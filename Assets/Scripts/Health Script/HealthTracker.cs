using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTracker : MonoBehaviour
{
    public GameObject heartTemplate;
    private List<GameObject> hearts = new List<GameObject>();
    ShroompController shroompController;
    public int health;
    public int chunk, remainder;
    public float itemOffset = 60f;
    // Start is called before the first frame update
    void Start()
    {
        shroompController = FindObjectOfType<ShroompController>();
    }

    // Update is called once per frame
    void Update()
    {
        health = shroompController.currentHealth;
        chunk = health / 2;
        remainder = health % 2;


    }
    private int x;
    private int y;
    private void sortPosition()
    {
        x = 0;
        foreach (var item in hearts)
        {
            hearts[x].GetComponent<RectTransform>().anchoredPosition = new Vector2((x * itemOffset)+20, (y * itemOffset)-20);
            x++;
        }
    }
    public void createContainers()
    {
        //get rid of any old health objects
        for (int i = 0; i < hearts.Count; i++)
        {
            Destroy(hearts[i].transform.gameObject);
        }
        hearts.Clear();
        for (int i = 0; i < shroompController.currentHealth / 2; i++)
        {
            hearts.Add(Instantiate(heartTemplate, transform.parent));
            hearts[hearts.Count - 1].GetComponent<Container>().count = 2;
        }
        if(shroompController.currentHealth % 2 == 1)
        {
            hearts.Add(Instantiate(heartTemplate, transform.parent));
            hearts[hearts.Count - 1].GetComponent<Container>().count = 1;
        }
        sortPosition();
    }
}
