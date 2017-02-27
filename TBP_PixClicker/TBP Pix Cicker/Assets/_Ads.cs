
using UnityEngine;
using UnityEngine.Advertisements;

public class _Ads : MonoBehaviour
{
	public void ShowAd()
	{
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();
		} else {
			Advertisement.Initialize ("1324673");
		}
	}
}