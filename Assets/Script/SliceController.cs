using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class SliceController : MonoBehaviour
{
    private RectTransform canvasRect;
    public GameObject image;
    public string category = null;
    public string decade = null;
    public int maxImages = 30;
    public float widthRatio = 0.75f;

    void createBorders(RectTransform canvasRect)
    {
        GameObject borders = new GameObject();
        borders.name = "Borders";
        borders.transform.SetParent(canvasRect.transform);
        for (int dir = 1; dir > -2; dir -= 2)
        {
            for (int sel = 0; sel < 2; sel++)
            {
                float factor = (dir + 1) / 2f * sel * (1-widthRatio);
                GameObject border = new GameObject();
                border.name = "border";
                border.transform.SetParent(borders.transform);
                border.transform.position = canvasRect.position + new Vector3(sel * dir * (canvasRect.rect.width / 2f + 5f) - canvasRect.rect.width * factor, (1 - sel) * dir * (canvasRect.rect.height / 2f + 5f), 0);
                border.AddComponent<BoxCollider2D>();
                border.GetComponent<BoxCollider2D>().size = new Vector2((canvasRect.rect.width * (1 - sel) + 10 * sel), (canvasRect.rect.height * sel + 10 * (1 - sel)));
            }

        }


    }

    void spawnImages(RectTransform canvasRect, string[] filenames)
    {
        GameObject images = new GameObject();
        images.name = "Images";
        images.transform.SetParent(canvasRect.transform);
        float available_size = (1-(filenames.Length+1)*0.01f)*Mathf.Min(canvasRect.rect.width, canvasRect.rect.height);
        float min_width = canvasRect.rect.width/8f * Mathf.Max(15f/(filenames.Length+1f), 1f);
        float max_width = canvasRect.rect.width/7.5f * Mathf.Max(15f / (filenames.Length + 1f), 1f);
        SquaresTree sqt = new SquaresTree(canvasRect.rect.width*widthRatio, canvasRect.rect.height, max_width, min_width, canvasRect.position-new Vector3(canvasRect.rect.width * (1-widthRatio) * 0.5f, 0,0));
        for (int i = 0; i < filenames.Length; i++)
        {
            SquareCell sqc = sqt.getSquare();
            float size = sqc.side;
            GameObject img = Instantiate(image, sqc.center, Quaternion.identity);
            img.name = string.Format("image{0}", i);
            img.transform.SetParent(images.transform);
            (img.GetComponent<ImageController>()).SetUpImage(filenames[i]);
            (img.GetComponent<ImageController>()).SetSize(size, size);
			(img.GetComponent<ImageController>()).SetPos(sqc.center);
        }
    }

    void spawnImages(RectTransform canvasRect, Texture2D[] textures)
    {
        GameObject images = new GameObject();
        images.name = "Images";
        images.transform.SetParent(canvasRect.transform);
        float available_size = (1 - (textures.Length + 1) * 0.01f) * Mathf.Min(canvasRect.rect.width, canvasRect.rect.height);
        float min_width = canvasRect.rect.width / 8f * Mathf.Max(15f / (textures.Length + 1f), 1f);
        float max_width = canvasRect.rect.width / 7.5f * Mathf.Max(15f / (textures.Length + 1f), 1f);
        SquaresTree sqt = new SquaresTree(canvasRect.rect.width * widthRatio, canvasRect.rect.height, max_width, min_width, canvasRect.position - new Vector3(canvasRect.rect.width * (1 - widthRatio) * 0.5f, 0, 0));
        for (int i = 0; i < textures.Length; i++)
        {
            SquareCell sqc = sqt.getSquare();
            float size = sqc.side;
            GameObject img = Instantiate(image, sqc.center, Quaternion.identity);
            img.name = string.Format("image{0}", i);
            img.transform.SetParent(images.transform);
            (img.GetComponent<ImageController>()).SetUpImage(textures[i]);
            (img.GetComponent<ImageController>()).SetSize(size, size);
            (img.GetComponent<ImageController>()).SetPos(sqc.center);
        }
    }
    // Use this for initialization
    void Start()
    {
        if(decade == null)
        {
            throw new System.Exception("Need a decade");
        }
        RectTransform canvasRect = GetComponent<RectTransform>();
        createBorders(canvasRect);

        //DIR TO LOAD PICTURES
        string basedir = "photos_low/" + decade;
        if (category != null)
        {
            basedir += "/" + category;
        }

        List<Texture2D> textures = new List<Texture2D>(Resources.LoadAll<Texture2D>(basedir));

        List<Texture2D> textures_init = new List<Texture2D>(textures);

        int numImages = Mathf.Min(maxImages, textures.Count);
        Texture2D[] images = new Texture2D[numImages];
		for (int i = 0; i < images.Length; i++)
        {
            if (textures.Count == 0)
            {
                textures = new List<Texture2D>(textures_init);
            }
            int choice = Random.Range(0, textures.Count);
            //images[i] = imagesPath + files[i % files.Length];
            images[i] = textures[choice];
            textures.RemoveAt(choice);
        }
        spawnImages(canvasRect, images);
        //Debug.Log(files);
		//new SquaresAssigner(100, 100, 50, 10, new Vector3(0, 0, canvasRect.position.z));
        //SquaresTree sq = new SquaresTree(8, 8, 4, 1);
    }

        // Use this for initialization
  //  void Start()
  //  {
  //      if(decade == null)
  //      {
  //          throw new System.Exception("Need a decade");
  //      }
  //      RectTransform canvasRect = GetComponent<RectTransform>();
  //      createBorders(canvasRect);

  //      List<string> files = new List<string>();
  //      //        string basedir = "D:/CERN/Selection/cern/photos_low/" + decade + "/";
  //      //Texture2D[] textures = Resources.LoadAll<Texture2D>("photos_low/50s/media");
  //      Debug.Log("Length is of " + textures.Length);
  //      string basedir = Application.dataPath + "/Ressources/Images/photos_low/" + decade + "/";
  //      if (category != null)
  //      {
  //          basedir += category + "/";
  //      }
  //      files.AddRange(Directory.GetFiles(basedir, "*.jpg", SearchOption.AllDirectories));

  //      List<string> files_init = new List<string>(files);
  //      string imagesPath = Application.dataPath + "/Images/";

  //      int numImages = Mathf.Min(maxImages, files.Count);
  //      //Debug.Log(numImages +" num files"+ files.Count);
  //      string[] images = new string[numImages];
		//for (int i = 0; i < images.Length; i++)
  //      {
  //          if (files.Count == 0)
  //          {
  //              files = new List<string>(files_init);
  //          }
  //          int choice = Random.Range(0, files.Count);
  //          //images[i] = imagesPath + files[i % files.Length];
  //          images[i] = files[choice];
  //          files.RemoveAt(choice);
  //      }
  //      spawnImages(canvasRect, images);
  //      //Debug.Log(files);
		////new SquaresAssigner(100, 100, 50, 10, new Vector3(0, 0, canvasRect.position.z));
  //      //SquaresTree sq = new SquaresTree(8, 8, 4, 1);
  //  }
    // Update is called once per frame
    void Update()
    {
    }

    public void SetColor(Color color)
    {
        Image[] imgs = GetComponentsInChildren<Image>();
        Image image = null;
        foreach (Image img in imgs)
        {
            if (img.tag == "Panel")
            {
                image = img;
            }
        }
        image.color = color;
    }
}
