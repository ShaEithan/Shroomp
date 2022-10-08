using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour
{
    private StatusEffectController Status;
    private Transform itemTemplate;
    private Transform itemContainer;

    int x = 0;
    int y = 0;
    float itemOffset = 30f;
    RectTransform defaultPos;
    // Start is called before the first frame update
    private void Awake()
    {
        Status = FindObjectOfType<StatusEffectController>();
        itemContainer = transform.Find("UI_Inventory");
        itemTemplate = itemContainer.Find("ItemSlotTemplate");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void refreshInventory()
    {




    }
    public void addItem(SpriteRenderer itemSprite)
    {

        RectTransform itemRectTransform = Instantiate(itemTemplate, itemContainer).GetComponent<RectTransform>();
        itemRectTransform.gameObject.SetActive(true);
        itemRectTransform.anchoredPosition = new Vector2(x* itemOffset, y * itemOffset);
        itemRectTransform.GetComponent<SpriteRenderer>().sprite = itemSprite.sprite;
        x++;
    }
}
