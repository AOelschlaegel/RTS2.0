using System.Collections.Generic;
using UnityEngine;
using RTS;
public class UnitDataContainer : MonoBehaviour
{
	public int HP = 100;
	public int Resources;
	public bool IsGathering;
	public NeutralDataContainer.GatherPoint GatherPoint;
	public Vector3 DestinationPosition;
	public Vector3 CurrentPosition;

	private void Update()
	{
		CurrentPosition = transform.position;
	}
}
