
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsExample : MonoBehaviour
{
	public void ShowAd()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
	}
}