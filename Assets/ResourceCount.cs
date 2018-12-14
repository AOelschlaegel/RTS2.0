using UnityEngine;
public class ResourceCount : MonoBehaviour
{
	public int Resources;

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
