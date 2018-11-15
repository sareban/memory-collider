using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

// Problem : need to add resources at runtime to allow for new pictures / sound clips / films to be added after compilation.
public class LoadMemory : MonoBehaviour
{

    public string category = null;
    public string decade = null;
    public static string[] files;

    public static List<Texture2D> LoadPictures(string path, string category, string decade)
    {
        string basepath = "http://127.0.0.1/photos_low/" + decade;
        if (category != null)
        {
            basepath += "/" + category;
        }
        
     //   files = System.IO.Directory.GetFiles(p);
        List<Texture2D> loadedPictures = new List<Texture2D>();
        //Networking.UnityWebRequest GetTexture(string uri);
        return loadedPictures;
    }
}
