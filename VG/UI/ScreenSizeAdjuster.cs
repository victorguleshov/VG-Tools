using UnityEngine;

public class ScreenSizeAdjuster : MonoBehaviour
{
	[SerializeField] private float sizeRate;

	void Start ()
	{
		float width = ScreenSizeConverter.GetScreenToWorldHeight;
		transform.localScale = Vector3.one * width * sizeRate;
	}
}