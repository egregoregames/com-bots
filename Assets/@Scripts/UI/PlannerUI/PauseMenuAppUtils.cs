using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PlannerPanel
{
    public static class PauseMenuAppUtils
    {
        private static void UpdateListItemVisibility(
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

        public static void UpdateItemList<T>(int max, GameObject upArrow,
            GameObject downArrow, T selected, IEnumerable<T> list) where T : Component
        {
            UpdateItemList(max, upArrow, downArrow, selected.gameObject,
                list.Select(x => x.gameObject).ToList());
        }

        public static void UpdateItemList(int max, GameObject UpArrow,
            GameObject DownArrow, GameObject selected, List<GameObject> list)
        {
            // Get index of selected
            int selQuestInd = list.IndexOf(selected);
            int total = list.Count();
            selected.SetActive(true);

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
    }
}