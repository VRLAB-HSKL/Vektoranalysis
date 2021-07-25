using SimpleWebBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserControl : MonoBehaviour
{
    public WebBrowser Browser;

    void Start()
    {
        if(Browser is null)
        {
            if(!TryGetComponent(out Browser))
            {
                Debug.LogError("BrowserControl - WebBrowser Component not found");
            }                        
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenURL(string url)
    {
        Browser.OpenCommentFile(url);
    }

}
