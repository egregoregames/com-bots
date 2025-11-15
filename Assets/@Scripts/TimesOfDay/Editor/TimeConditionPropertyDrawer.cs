using System;
using System.Collections.Generic;
using System.Linq;
using ComBots.TimesOfDay;
using ComBots.World.NPCs;
using UnityEditor;
using UnityEngine;

namespace ComBots.World.NPCs.Editor
{
    /// <summary>
    /// Custom property drawer for NPC_Config.TimeCondition that displays Terms and Times of Day as checkboxes
    /// instead of array elements. Provides "All" and "None" buttons for quick selection.
    /// </summary>
    [CustomPropertyDrawer(typeof(NPC_Config.TimeCondition))]
    public class TimeConditionPropertyDrawer : PropertyDrawer
    {
        private const float CheckboxWidth = 16f;
        private const float LabelSpacing = 5f;
        private const float SectionSpacing = 10f;
        private const float HeaderHeight = 18f;
        private const float LineHeight = 18f;
        private const float VerticalSpacing = 2f;
        private const float ButtonWidth = 60f;
        private const float ButtonSpacing = 5f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get the serialized properties
            SerializedProperty termsProperty = property.FindPropertyRelative("Terms");
            SerializedProperty timesOfDayProperty = property.FindPropertyRelative("TimesOfDay");

            if (termsProperty == null || timesOfDayProperty == null)
            {
                EditorGUI.LabelField(position, "Error: Could not find Terms or TimesOfDay properties");
                EditorGUI.EndProperty();
                return;
            }

            float currentY = position.y;

            // Draw main label
            EditorGUI.LabelField(new Rect(position.x, currentY, position.width, HeaderHeight), label, EditorStyles.boldLabel);
            currentY += HeaderHeight + VerticalSpacing;

            // Draw Terms section
            Rect termsHeaderRect = new (position.x, currentY, position.width - (2 * ButtonWidth + ButtonSpacing), HeaderHeight);
            Rect selectAllTermsRect = new (position.x + position.width - (2 * ButtonWidth + ButtonSpacing), currentY, ButtonWidth, HeaderHeight);
            Rect clearAllTermsRect = new (position.x + position.width - ButtonWidth, currentY, ButtonWidth, HeaderHeight);
            
            EditorGUI.LabelField(termsHeaderRect, "Terms", EditorStyles.boldLabel);
            
            if (GUI.Button(selectAllTermsRect, "All", EditorStyles.miniButtonLeft))
            {
                HashSet<Term> allTerms = new HashSet<Term>();
                foreach (Term term in Enum.GetValues(typeof(Term)))
                {
                    allTerms.Add(term);
                }
                UpdateSerializedArray(termsProperty, allTerms.ToArray());
            }
            
            if (GUI.Button(clearAllTermsRect, "None", EditorStyles.miniButtonRight))
            {
                UpdateSerializedArray(termsProperty, new Term[0]);
            }
            
            currentY += HeaderHeight + VerticalSpacing;

            // Draw Term checkboxes
            foreach (Term term in Enum.GetValues(typeof(Term)))
            {
                bool isSelected = IsTermSelected(termsProperty, term);
                string displayName = GetTermDisplayName(term);

                Rect checkboxRect = new Rect(position.x, currentY, CheckboxWidth, LineHeight);
                Rect labelRect = new Rect(position.x + CheckboxWidth + LabelSpacing, currentY, position.width - CheckboxWidth - LabelSpacing, LineHeight);

                EditorGUI.BeginChangeCheck();
                bool newValue = EditorGUI.Toggle(checkboxRect, isSelected);
                if (EditorGUI.EndChangeCheck())
                {
                    if (newValue)
                        AddTermToArray(termsProperty, term);
                    else
                        RemoveTermFromArray(termsProperty, term);
                }
                
                EditorGUI.LabelField(labelRect, displayName);

                currentY += LineHeight + VerticalSpacing;
            }

            currentY += SectionSpacing;

            // Draw Times of Day section
            Rect timesHeaderRect = new (position.x, currentY, position.width - (2 * ButtonWidth + ButtonSpacing), HeaderHeight);
            Rect selectAllTimesRect = new (position.x + position.width - (2 * ButtonWidth + ButtonSpacing), currentY, ButtonWidth, HeaderHeight);
            Rect clearAllTimesRect = new (position.x + position.width - ButtonWidth, currentY, ButtonWidth, HeaderHeight);
            
            EditorGUI.LabelField(timesHeaderRect, "Times of Day", EditorStyles.boldLabel);
            
            if (GUI.Button(selectAllTimesRect, "All", EditorStyles.miniButtonLeft))
            {
                HashSet<TimeOfDay> allTimes = new HashSet<TimeOfDay>();
                foreach (TimeOfDay timeOfDay in Enum.GetValues(typeof(TimeOfDay)))
                {
                    allTimes.Add(timeOfDay);
                }
                UpdateSerializedArray(timesOfDayProperty, allTimes.ToArray());
            }
            
            if (GUI.Button(clearAllTimesRect, "None", EditorStyles.miniButtonRight))
            {
                UpdateSerializedArray(timesOfDayProperty, new TimeOfDay[0]);
            }
            
            currentY += HeaderHeight + VerticalSpacing;

            // Draw TimeOfDay checkboxes
            foreach (TimeOfDay timeOfDay in Enum.GetValues(typeof(TimeOfDay)))
            {
                bool isSelected = IsTimeOfDaySelected(timesOfDayProperty, timeOfDay);
                string displayName = GetTimeOfDayDisplayName(timeOfDay);

                Rect checkboxRect = new Rect(position.x, currentY, CheckboxWidth, LineHeight);
                Rect labelRect = new Rect(position.x + CheckboxWidth + LabelSpacing, currentY, position.width - CheckboxWidth - LabelSpacing, LineHeight);

                EditorGUI.BeginChangeCheck();
                bool newValue = EditorGUI.Toggle(checkboxRect, isSelected);
                if (EditorGUI.EndChangeCheck())
                {
                    if (newValue)
                        AddTimeOfDayToArray(timesOfDayProperty, timeOfDay);
                    else
                        RemoveTimeOfDayFromArray(timesOfDayProperty, timeOfDay);
                }
                
                EditorGUI.LabelField(labelRect, displayName);

                currentY += LineHeight + VerticalSpacing;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int termCount = Enum.GetValues(typeof(Term)).Length;
            int timeOfDayCount = Enum.GetValues(typeof(TimeOfDay)).Length;

            float height = HeaderHeight + VerticalSpacing; // Main label
            height += HeaderHeight + VerticalSpacing; // Terms header
            height += (LineHeight + VerticalSpacing) * termCount; // Term checkboxes
            height += SectionSpacing; // Spacing between sections
            height += HeaderHeight + VerticalSpacing; // Times of Day header
            height += (LineHeight + VerticalSpacing) * timeOfDayCount; // TimeOfDay checkboxes

            return height;
        }

        private HashSet<Term> GetSelectedTerms(SerializedProperty termsProperty)
        {
            HashSet<Term> selectedTerms = new HashSet<Term>();
            for (int i = 0; i < termsProperty.arraySize; i++)
            {
                SerializedProperty element = termsProperty.GetArrayElementAtIndex(i);
                selectedTerms.Add((Term)element.enumValueIndex);
            }
            return selectedTerms;
        }

        private HashSet<TimeOfDay> GetSelectedTimesOfDay(SerializedProperty timesOfDayProperty)
        {
            HashSet<TimeOfDay> selectedTimesOfDay = new HashSet<TimeOfDay>();
            for (int i = 0; i < timesOfDayProperty.arraySize; i++)
            {
                SerializedProperty element = timesOfDayProperty.GetArrayElementAtIndex(i);
                selectedTimesOfDay.Add((TimeOfDay)element.enumValueIndex);
            }
            return selectedTimesOfDay;
        }

        private void UpdateSerializedArray<T>(SerializedProperty arrayProperty, T[] newValues) where T : Enum
        {
            arrayProperty.ClearArray();
            arrayProperty.arraySize = newValues.Length;
            for (int i = 0; i < newValues.Length; i++)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                element.enumValueIndex = Convert.ToInt32(newValues[i]);
            }
            arrayProperty.serializedObject.ApplyModifiedProperties();
        }

        private string GetTermDisplayName(Term term)
        {
            switch (term)
            {
                case Term.FirstTerm: return "First Term (Early Fall)";
                case Term.SecondTerm: return "Second Term (Late Fall)";
                case Term.ThirdTerm: return "Third Term (Winter Vacation)";
                case Term.FourthTerm: return "Fourth Term (Early Spring)";
                case Term.FifthTerm: return "Fifth Term (Late Spring)";
                case Term.SixthTerm: return "Sixth Term (Summer Vacation)";
                default: return term.ToString();
            }
        }

        private string GetTimeOfDayDisplayName(TimeOfDay timeOfDay)
        {
            switch (timeOfDay)
            {
                case TimeOfDay.Morning: return "Morning (6 AM - 12 PM)";
                case TimeOfDay.Day: return "Day (12 PM - 6 PM)";
                case TimeOfDay.Evening: return "Evening (6 PM - 12 AM)";
                case TimeOfDay.Night: return "Night (12 AM - 6 AM)";
                default: return timeOfDay.ToString();
            }
        }

        private bool IsTermSelected(SerializedProperty termsProperty, Term term)
        {
            for (int i = 0; i < termsProperty.arraySize; i++)
            {
                SerializedProperty element = termsProperty.GetArrayElementAtIndex(i);
                if ((Term)element.enumValueIndex == term)
                    return true;
            }
            return false;
        }

        private bool IsTimeOfDaySelected(SerializedProperty timesOfDayProperty, TimeOfDay timeOfDay)
        {
            for (int i = 0; i < timesOfDayProperty.arraySize; i++)
            {
                SerializedProperty element = timesOfDayProperty.GetArrayElementAtIndex(i);
                if ((TimeOfDay)element.enumValueIndex == timeOfDay)
                    return true;
            }
            return false;
        }

        private void AddTermToArray(SerializedProperty termsProperty, Term term)
        {
            if (IsTermSelected(termsProperty, term))
                return;

            int newIndex = termsProperty.arraySize;
            termsProperty.InsertArrayElementAtIndex(newIndex);
            SerializedProperty newElement = termsProperty.GetArrayElementAtIndex(newIndex);
            newElement.enumValueIndex = (int)term;
        }

        private void RemoveTermFromArray(SerializedProperty termsProperty, Term term)
        {
            for (int i = termsProperty.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty element = termsProperty.GetArrayElementAtIndex(i);
                if ((Term)element.enumValueIndex == term)
                {
                    termsProperty.DeleteArrayElementAtIndex(i);
                    return;
                }
            }
        }

        private void AddTimeOfDayToArray(SerializedProperty timesOfDayProperty, TimeOfDay timeOfDay)
        {
            if (IsTimeOfDaySelected(timesOfDayProperty, timeOfDay))
                return;

            int newIndex = timesOfDayProperty.arraySize;
            timesOfDayProperty.InsertArrayElementAtIndex(newIndex);
            SerializedProperty newElement = timesOfDayProperty.GetArrayElementAtIndex(newIndex);
            newElement.enumValueIndex = (int)timeOfDay;
        }

        private void RemoveTimeOfDayFromArray(SerializedProperty timesOfDayProperty, TimeOfDay timeOfDay)
        {
            for (int i = timesOfDayProperty.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty element = timesOfDayProperty.GetArrayElementAtIndex(i);
                if ((TimeOfDay)element.enumValueIndex == timeOfDay)
                {
                    timesOfDayProperty.DeleteArrayElementAtIndex(i);
                    return;
                }
            }
        }
    }
}