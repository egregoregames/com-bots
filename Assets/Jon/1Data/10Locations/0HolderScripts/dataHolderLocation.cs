using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderLocation", menuName = "Scriptable Objects/dataHolderLocation")]

public class dataHolderLocation : ScriptableObject
{
	public string bannerName;
	public AudioClip backgroundMusic;
	public bool indoors;
	public GameObject room;

	//data for NPCs, Item Crates, Litter, Doors	
}
