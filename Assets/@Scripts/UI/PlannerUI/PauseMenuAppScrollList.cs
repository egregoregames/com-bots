using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PauseMenuAppScrollList
{
    [field: SerializeField]
    private GameObject UpArrow { get; set; }

    [field: SerializeField]
    private GameObject DownArrow { get; set; }

    [field: SerializeField]
    private int MaxItemsOnScreen { get; set; } = 7;

    public void UpdateItemList<T>(T selected, IEnumerable<T> list) where T : Component
    {
        if (list.Count() < 1)
        {
            DownArrow.SetActive(false);
            UpArrow.SetActive(false);
            return;
        }

        UpdateItemList(selected.gameObject, list.Select(x => x.gameObject).ToList());
    }

    public void UpdateItemList(GameObject selected, List<GameObject> list)
    {
        if (list.Count < 1)
        {
            DownArrow.SetActive(false);
            UpArrow.SetActive(false);
            return;
        }

        DownArrow.transform.SetAsLastSibling();

        // Get index of selected
        int selQuestInd = list.IndexOf(selected);
        int total = list.Count();
        selected.SetActive(true);
        int max = MaxItemsOnScreen;

        int half = (int)Math.Floor(max / 2d);

        bool isMoreThanMax = total > max;
        UpArrow.SetActive(selQuestInd > half && isMoreThanMax);
        DownArrow.SetActive((total - 1) - selQuestInd >= half && isMoreThanMax);

        foreach (var questItem in list)
        {
            if (questItem == selected)
                continue;

            UpdateListItemVisibility(
                total, selQuestInd, half, max, questItem, list.IndexOf(questItem));
        }
    }

    private void UpdateListItemVisibility(
        int totalItemCount, int selectedItemIndex, int halfOfMaxItemsOnScreen,
        int maxItemsOnScreen, GameObject listItem, int localIndex)
    {
        bool active;

        bool isNearTop = selectedItemIndex <= halfOfMaxItemsOnScreen;
        bool isNearBottom = (totalItemCount - 1) - selectedItemIndex <=
            halfOfMaxItemsOnScreen;

        if (isNearTop)
        {
            active = localIndex < maxItemsOnScreen;
        }
        else if (isNearBottom)
        {
            bool isActive = (totalItemCount - 1) - localIndex <= maxItemsOnScreen - 1;
            active = isActive;
        }
        else
        {
            active = Math.Abs(localIndex - selectedItemIndex) <=
                halfOfMaxItemsOnScreen;
        }

        listItem.SetActive(active);
    }
}