using System.Collections.Generic;
using UnityEngine;

public static class StudentIdGenerator
{
    /// <summary>
    /// No letter I
    /// </summary>
    private const string _playerStudentIdLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";

    public static string Generate()
    {
        var list = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(0, _playerStudentIdLetters.Length);
            char letter = _playerStudentIdLetters[random];
            random = Random.Range(1, 99);
            list.Add($"{letter}{random:00}");
        }

        return string.Join("-", list);
    }
}