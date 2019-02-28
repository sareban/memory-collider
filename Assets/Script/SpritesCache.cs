using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// Load pictures into a sprite cache to be queried afterwards
public class SpritesCache : MonoBehaviour {
    private static SpritesCache _instance;

    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    private Dictionary<Texture2D, Sprite> spriteCacheTex = new Dictionary<Texture2D, Sprite>();

    public static SpritesCache instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.

            if (_instance == null)
                _instance = GameObject.FindObjectOfType<SpritesCache>();
            return _instance;
        }
    }

    public Sprite LoadSprite(string path)
    {
        Sprite sprite = null;
        if (!spriteCache.TryGetValue(path, out sprite))
        {
            {
                sprite = IMG2Sprite.instance.LoadNewSprite(path); 
                spriteCache.Add(path, sprite);
            }
                      
        }

        return sprite;
    }

    public Sprite LoadSprite(Texture2D texture)
    {
        Sprite sprite = null;
        if (!spriteCacheTex.TryGetValue(texture, out sprite))
        {
            sprite = IMG2Sprite.instance.LoadNewSprite(texture);

            spriteCacheTex.Add(texture, sprite);
        }

        return sprite;
    }

}
