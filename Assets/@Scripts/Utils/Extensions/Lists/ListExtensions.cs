using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    private static Random random = new Random();

    public static T GetRandomElement<T>(this List<T> list, T exclude)
    {
        if (list == null || list.Count == 0)
            throw new InvalidOperationException("Cannot get a random element from an empty or null list.");

        var filteredList = list.Where(item => !EqualityComparer<T>.Default.Equals(item, exclude)).ToList();

        if (filteredList.Count == 0)
            throw new InvalidOperationException("No available elements after exclusion.");

        return filteredList[random.Next(filteredList.Count)];
    }
    public static List<T> Without<T>(this List<T> list, T element)
    {
        return list.Where(item => !EqualityComparer<T>.Default.Equals(item, element)).ToList();
    }
    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            throw new InvalidOperationException("Cannot get a random element from an empty or null list.");

        return list[random.Next(list.Count)];
    }
}