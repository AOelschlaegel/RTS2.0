using System;
using System.Collections.Generic;
using UnityEngine;
namespace RTS
{
	public class NeutralDataContainer : MonoBehaviour
	{
		public int Resources = 1000;
		public List<GatherPoint> AvailableGatherPoints;
		public List<GatherPoint> OccupiedGatherPoints;
		public string ObjectSize;

		private void Start()
		{
			AvailableGatherPoints = GetComponent<DataContainer>().GatherPoints;
		}

		private void Update()
		{
			if (Resources <= 0)
			{
				gameObject.transform.Translate(0, -1f, 0);
				if (gameObject.transform.position.y < -10f)
				{
					Destroy(gameObject);
				}
			}

			if (OccupiedGatherPoints.Count == 0)
			{
				AvailableGatherPoints = GetComponent<DataContainer>().GatherPoints;
			}

			// If there are Occupied GatherPoints
			if (OccupiedGatherPoints.Count > 0)
			{
				// Check each one
				for (int i = 0; i < OccupiedGatherPoints.Count; i++)
				{
					// Reference the Unit
					var _gatheringUnit = OccupiedGatherPoints[i].Object;
					var _unitDataContainer = _gatheringUnit.GetComponent<UnitDataContainer>();

					// Compare 
					foreach (GatherPoint _gatherpoint in AvailableGatherPoints)
					{
						if (_gatherpoint.Position == OccupiedGatherPoints[i].Position)
						{
							AvailableGatherPoints.Remove(_gatherpoint);
						}

						// If Unit is already gathering
						if (_unitDataContainer.IsGathering == false)
						{
							AvailableGatherPoints.Add(OccupiedGatherPoints[i]);
							OccupiedGatherPoints.Remove(OccupiedGatherPoints[i]);
						}
					}	
				}
			}
		}

		[Serializable]
		public class GatherPoint
		{
			public GameObject Object;
			public Vector3 Position;
		}
	}
}
