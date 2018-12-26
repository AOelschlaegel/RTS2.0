using System.Collections.Generic;
using UnityEngine;
public class FractionBuildingContainer : MonoBehaviour
{
	public List<GameObject> TownCenters;
	public Transform BuildingsRoot;

	private void Start()
	{
		TownCenters = new List<GameObject>();
		BuildingsRoot = GameObject.Find("Buildings").transform;
	}

	private void Update()
	{
		foreach(Transform building in BuildingsRoot)
		{
			if(building.name == "TownCenter")
			{
				if(!TownCenters.Contains(building.gameObject))
				TownCenters.Add(building.gameObject);
			}
		}
	}
}
