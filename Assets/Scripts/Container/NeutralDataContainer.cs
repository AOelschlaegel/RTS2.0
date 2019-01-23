using System;
using System.Collections.Generic;
using UnityEngine;
namespace RTS
{
	public class NeutralDataContainer : MonoBehaviour
	{
		public int Resources = 1000;
		public List<Vector3> AvailableGatherPoints;
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

			if (OccupiedGatherPoints.Count > 0)
			{
				for (int i = 0; i < OccupiedGatherPoints.Count; i++)
				{
					if (AvailableGatherPoints.Contains(OccupiedGatherPoints[i].Position))
					{
						AvailableGatherPoints.Remove(OccupiedGatherPoints[i].Position);
					}

					if (OccupiedGatherPoints[i].Object.GetComponent<UnitDataContainer>().isGathering == false)
					{
						AvailableGatherPoints.Add(OccupiedGatherPoints[i].Position);
						OccupiedGatherPoints.Remove(OccupiedGatherPoints[i]);
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
