using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ShareOnSocialMedia : MonoBehaviour
{
	public static ShareOnSocialMedia Instance;
	public string SubjectText = "This is my game";
	public string LinkText = "Share your link with your friends";

	public string SubjectTextIOS = "This is my game";
	public string LinkTextIOS = "Share your link with your friends";

	private void Awake()
    {
		Instance = this;
	}

    public void Share()
	{
		StartCoroutine ("TakeScreenShotAndShare");
	}

	IEnumerator TakeScreenShotAndShare ()
	{
		yield return new WaitForEndOfFrame ();

		Texture2D tx = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		tx.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		tx.Apply ();

		string path = Path.Combine (Application.temporaryCachePath, "sharedImage.png");//image name
		File.WriteAllBytes (path, tx.EncodeToPNG ());

		Destroy (tx); //to avoid memory leaks

#if UNITY_IOS
		new NativeShare()
				.AddFile(path)
				.SetSubject(SubjectTextIOS)
				.SetText(LinkTextIOS)
				.Share();
#elif UNITY_ANDROID
	new NativeShare ()
				.AddFile (path)
				.SetSubject (SubjectText)
				.SetText (LinkText)
				.Share ();
#endif

	}
}
