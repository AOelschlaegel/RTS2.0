using UnityEngine;
public class NeutralDataContainer : MonoBehaviour
{
	public int Resources = 1000;

	private void Update()
	{
		if (Resources <= 0)
		{
			gameObject.transform.Translate(0, -1f, 0);
			if(gameObject.transform.position.y < -10f)
			{
				Destroy(gameObject);
			}
		}
	}
}
