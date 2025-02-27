using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;
using UnityEngine.UI;
  
public class menuManager : MonoBehaviour//, IInputActionCollection2
{
    [SerializeField] private GameObject[] menuList;
    [SerializeField] private Button[] firstSelected;
    [SerializeField] public int menuID = 0; 
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuSelected(menuID);
    }

    // Update is called once per frame
    //public void OnOpenMenu()
   // {
		//if (menuID == -1)
		//{
			//menuID = 0;
		//}            
   // }

    private void menuSelected(int menuID)
    {
	foreach (GameObject menu in menuList)
	{
		menu.SetActive(false);
	}
    menuList[menuID].SetActive(true);
    firstSelected[menuID].Select();
    }

    public void ActivateMenu(Button button)
    {
	if (button.name == "buttonTilePlanner")
	{
	menuID = 1;
	menuSelected(menuID);
	}
	else if (button.name == "buttonTileBotlink")
	{
	menuID = 2;
	menuSelected(menuID);
	}
    }
}