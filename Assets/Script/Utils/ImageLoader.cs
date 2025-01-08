using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private RawImage targetImage;
    
    public void LoadImageFromURL(string url)
    {
        StartCoroutine(DownloadImage(url));
    }    

    private IEnumerator DownloadImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                targetImage.texture = texture;
            }
            else
            {
                Debug.LogError($"Error loading image: {request.error}");
            }

        }    
    }    

}
