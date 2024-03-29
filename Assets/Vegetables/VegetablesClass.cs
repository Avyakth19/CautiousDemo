﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VegetablesClass : MonoBehaviour
{
   // public List<Sprite> images;
  //  SpriteRenderer sr;
    public Button shareButton;

    private bool isFocus = false;

    private string shareSubject, shareMessage;
    private bool isProcessing = false;
    private string screenshotName;

    public Image randomImage;
    public Sprite s0;
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite[] images;


    // public List<Sprite> images;
    void Start()
    {
        images = new Sprite[4];
        images[0] = s0;
        images[1] = s1;
        images[2] = s2;
        images[3] = s3;
        shareButton.onClick.AddListener(OnShareButtonClick);
       // sr = GetComponent<SpriteRenderer>();
     //   showRandomImage();
    }

    void changeImage()
    {
        int num = UnityEngine.Random.Range(0, images.Length);
        randomImage.sprite = images[num];
    }
  

    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

    public void OnShareButtonClick()
    {

        screenshotName = "fireblock_highscore.png";
        shareSubject = "I challenge you to beat my high score in Fire Block";
        shareMessage = "I challenge you to beat my high score in Fire Block. " +
        ". Get the Fire Block app from the link below. \nCheers\n" +
        "\nhttp://onelink.to/fireblock";

        ShareScreenshot();
    }


    private void ShareScreenshot()
    {

#if UNITY_ANDROID
        if (!isProcessing)
        {
            StartCoroutine(ShareScreenshotInAnroid());
        }

#else
		Debug.Log("No sharing set up for this platform.");
#endif
    }



#if UNITY_ANDROID
    public IEnumerator ShareScreenshotInAnroid()
    {

        isProcessing = true;
        // wait for graphics to render
        yield return new WaitForEndOfFrame();

        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        ScreenCapture.CaptureScreenshot(screenshotName, 1);
        yield return new WaitForSeconds(0.5f);

        if (!Application.isEditor)
        {

            //Create intent for action send
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

            //create image URI to add it to the intent
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + screenShotPath);

            //put image and string extra
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");
            currentActivity.Call("startActivity", chooser);
        }

        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }
#endif
    public void openplaystore()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.greengold.SB_LadduQuest&hl=hi");
    }
}
