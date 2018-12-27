using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
	[SerializeField] private Transform _trees01ProxyRoot;
	[SerializeField] private Transform _trees01InstanceRoot;
	[SerializeField] private GameObject tree01;

	[ContextMenu("Place Proxies")]
	void PlaceProxies()
	{
		ClearAllProxies();
		PlaceTrees();
	}

	[ContextMenu("Clear All Proxies")]
	void ClearAllProxies()
	{
		for (int i = 0; i < 100; i++)
		{
			foreach (Transform tree in _trees01InstanceRoot)
			{
				DestroyImmediate(tree.gameObject);
			}
		}
	}

	void PlaceTrees()
	{
		foreach(Transform tree in _trees01ProxyRoot)
		{
			var instance = Instantiate(tree01, tree.position, Quaternion.identity);
			instance.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
			instance.transform.parent = _trees01InstanceRoot;
		}
	}
}
