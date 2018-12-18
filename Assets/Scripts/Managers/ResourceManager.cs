using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{ 
	public int food;

	public Text foodText;

	private void Update()
	{
		if (food != 0)
		{
			foodText.text = "FOOD: " + food.ToString();
		}
		else foodText.text = null;
	}
}
