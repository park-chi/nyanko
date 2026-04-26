using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] private Text _savePathText;
    [SerializeField] private float _displayDuration = 0.3f;
    [SerializeField] private float _displaySecond = 3f;

    public void DoShot()
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        string filename = "nyanko_" + timestamp + ".png";
        ScreenCapture.CaptureScreenshot(filename);

        StartCoroutine(SetTextAfterDelay(_displayDuration, "Screenshot saved to: " + filename));

        StartCoroutine(SetTextAfterDelay(_displaySecond, ""));
    }

    private IEnumerator SetTextAfterDelay(float delay, string text)
    {
        yield return new WaitForSeconds(delay);
        _savePathText.text = text;
    }
}
