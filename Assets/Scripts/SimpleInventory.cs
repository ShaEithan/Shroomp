using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour
{
    private StatusEffectController Status;
    private Transform itemTemplate;
    private Transform itemContainer;
    private List<string> inventoryTrack = new List<string>();
    private List<GameObject> UI_Slots = new List<GameObject>();
    private bool fireAdded, iceAdded, bombAdded,bDashAdded = false;
    public Sprite iceSprite;
    public Sprite fireSprite;
    public Sprite bombSprite;
    public Sprite bDashSprite;

    int x = 0;
    int y = 0;
    public float itemOffset = 60f;
    RectTransform defaultPos;
    // Start is called before the first frame update
    private void Awake()
    {
        Status = FindObjectOfType<StatusEffectController>();
        itemContainer = transform.Find("UI_Inventory");
        itemTemplate = itemContainer.Find("ItemSlotTemplate");
    }
    private void refresh()
    {
        

        foreach (var item in inventoryTrack)
        {
            if(item == "fireUp" && !fireAdded)
            {
                UI_Slots.Add(Instantiate(itemTemplate, itemContainer).gameObject);
                UI_Slots[UI_Slots.Count - 1].SetActive(true);
                UI_Slots[UI_Slots.Count - 1].GetComponent<SpriteRenderer>().sprite = fireSprite;
                UI_Slots[UI_Slots.Count - 1].GetComponent<UI_Count>().slotNumber = UI_Slots.Count;
                sortPosition();
                fireAdded = true;
            }
            if (item == "iceUp" && !iceAdded)
            {
                UI_Slots.Add(Instantiate(itemTemplate, itemContainer).gameObject);
                UI_Slots[UI_Slots.Count - 1].SetActive(true);
                UI_Slots[UI_Slots.Count - 1].GetComponent<SpriteRenderer>().sprite = iceSprite;
                UI_Slots[UI_Slots.Count - 1].GetComponent<UI_Count>().slotNumber = UI_Slots.Count;
                sortPosition();
                iceAdded = true;
            }
            if (item == "bombUp" && !bombAdded)
            {
                UI_Slots.Add(Instantiate(itemTemplate, itemContainer).gameObject);
                UI_Slots[UI_Slots.Count - 1].SetActive(true);
                UI_Slots[UI_Slots.Count - 1].GetComponent<SpriteRenderer>().sprite = bombSprite;
                UI_Slots[UI_Slots.Count - 1].GetComponent<UI_Count>().slotNumber = UI_Slots.Count;
                sortPosition();
                bombAdded = true;
            }
            if (item == "bDashUp" && !bDashAdded)
            {
                UI_Slots.Add(Instantiate(itemTemplate, itemContainer).gameObject);
                UI_Slots[UI_Slots.Count - 1].SetActive(true);
                UI_Slots[UI_Slots.Count - 1].GetComponent<SpriteRenderer>().sprite = bDashSprite;
                UI_Slots[UI_Slots.Count - 1].GetComponent<UI_Count>().slotNumber = UI_Slots.Count;
                sortPosition();
                bDashAdded = true;
            }
        }
        if(inventoryTrack.Count==3)
            removeAt(0);
    }
    private void sortPosition()
    {
        x = 0;
        foreach (var item in UI_Slots)
        {
            UI_Slots[x].GetComponent<RectTransform>().anchoredPosition = new Vector2(x * itemOffset, y * itemOffset);
            x++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void test()
    {

    }
    public void removeAt(int i)
    {
        if (inventoryTrack.Count >= i)
        {
            if (inventoryTrack[i] == "fireUp")
            {
                fireAdded = false;
                Status.fireUp = false;
            }
            if (inventoryTrack[i] == "iceUp")
            {
                iceAdded = false;
                Status.iceUp = false;
            }
            if (inventoryTrack[i] == "bombUp")
            {
                bombAdded = false;
                Status.bombUp = false;
            }
            if (inventoryTrack[i] == "bDashUp")
            {
                bDashAdded = false;
                Status.bDashUp = false;
            }
            inventoryTrack.RemoveAt(i);
            Destroy(UI_Slots[i].transform.gameObject);
            UI_Slots.RemoveAt(i);
            sortPosition();
        }
    }
    public void addItem(string powerUpName)
    {
        /**
        RectTransform itemRectTransform = Instantiate(itemTemplate, itemContainer).GetComponent<RectTransform>();
        itemRectTransform.gameObject.SetActive(true);
        itemRectTransform.anchoredPosition = new Vector2(x* itemOffset, y * itemOffset);
        itemRectTransform.GetComponent<SpriteRenderer>().sprite = itemSprite.sprite;
        x++;
        **/
        inventoryTrack.Add(powerUpName);
        refresh();
    }
}
