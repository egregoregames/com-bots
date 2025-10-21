using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderMedal", menuName = "Scriptable Objects/dataHolderMedal")]

public class dataHolderMedal : ScriptableObject
{
	public string medalName;
	[TextArea(3, 10)]
	public string flavorText;
	[TextArea(3, 10)]
	public string effectText;
	public GameObject image;	
}
