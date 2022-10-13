using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Count : MonoBehaviour
{
    public int slotNumber = 0;
	public Button yourButton;
	private SimpleInventory sInventory;

	void Start()
	{
		sInventory = FindObjectOfType<SimpleInventory>();

		yourButton.onClick.AddListener(TaskOnClick);
		Debug.Log(yourButton);
	}

	public void TaskOnClick()
	{
		Debug.Log("Remove attempt");
		sInventory.removeAt(slotNumber-1);
		
	}
}
