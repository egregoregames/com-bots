using R3;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Subcomponent used by pause menu apps to easily reorganize a scroll list
/// upon UI navigation
/// </summary>
[Serializable]
public class PauseMenuAppScrollList<T>
{
    [field: SerializeField]
    public GameObject UpArrow { get; private set; }

    [field: SerializeField]
    public GameObject DownArrow { get; private set; }

    [field: SerializeField]
    private int MaxItemsOnScreen { get; set; } = 5;

    [field: SerializeField]
    public GameObject ItemTemplate { get; private set; }

    [field: SerializeField, ReadOnly]
    public List<PauseMenuAppSelectableListItem<T>> InstantiatedItems { get; private set; }

    public void ClearItems()
    {
        InstantiatedItems
            .Where(x => x != null)
            .ToList()
            .ForEach(x => Object.Destroy(x.gameObject));

        InstantiatedItems.Clear();
    }

    public void SetSelected(int increment)
    {
        if (increment != 1 && increment != -1)
        {
            throw new Exception(
                "Improper usage. Argument must be 1 (next quest) or -1 (prev quest)");
        }

        if (InstantiatedItems.Count <= 1) return;

        var selectedQuest = InstantiatedItems.First(x => x.IsSelected);
        int selectedQuestIndex = InstantiatedItems.IndexOf(selectedQuest);

        var newIndex = selectedQuestIndex + increment;
        if (newIndex < 0)
        {
            // Wrap back to bottom of quest list
            newIndex = InstantiatedItems.Count - 1;
        }
        else if (newIndex > InstantiatedItems.Count - 1)
        {
            // Wrap to top of quest list
            newIndex = 0;
        }

        selectedQuest.Deselect();
        InstantiatedItems[newIndex].Select();
    }

    public async Task InstantiateItems(IEnumerable<T> all)
    {
        ItemTemplate.SetActive(true);

        foreach (var item in all)
        {
            var newObj = Object.Instantiate(
                ItemTemplate, ItemTemplate.transform.parent);

            var comp = newObj.GetComponent<PauseMenuAppSelectableListItem<T>>();
            await comp.SetDatum(item);
            comp.Deselect();
            InstantiatedItems.Add(comp);
        }

        ItemTemplate.SetActive(false);
    }

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