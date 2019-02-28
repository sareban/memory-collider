using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Video;
using System.Collections;

//This class controls the behavior of pictures on screen panels
public class SliceController : MonoBehaviour
{
    private RectTransform canvasRect;
    public GameObject image;
    public string category = null;
    public string decade = null;
    public int maxImages = 30;
    public float widthRatio = 0.75f;
    string photos_dir = "photos_low/";

    private List<Texture2D> textures;
    private SquaresTree sqt;
    public GameObject images;
    public int counter;
    private GameObject scriptHolder;


    // Use this for initialization
    void Start()
    {
        if (decade == null)
        {
            throw new System.Exception("Need a decade");
        }
        RectTransform canvasRect = GetComponent<RectTransform>();
        createBorders(canvasRect);

        images = new GameObject();
        images.name = "Images";
        images.transform.SetParent(canvasRect.transform);

        //DIR TO LOAD PICTURES FROM
        string basedir = photos_dir + decade;
        if (category != null)
        {
            basedir += "/" + category;
        }

        float available_size = (1 - (maxImages + 1) * 0.01f) * Mathf.Min(canvasRect.rect.width, canvasRect.rect.height);
        float min_width = canvasRect.rect.width / 8f * Mathf.Max(15f / (maxImages + 1f), 1f);
        float max_width = canvasRect.rect.width / 7.5f * Mathf.Max(15f / (maxImages + 1f), 1f);
        sqt = new SquaresTree(canvasRect.rect.width * widthRatio, canvasRect.rect.height, max_width, min_width, canvasRect.position - new Vector3(canvasRect.rect.width * (1 - widthRatio) * 0.5f, 0, 0));


        textures = new List<Texture2D>(Resources.LoadAll<Texture2D>(basedir));
        List<Texture2D> textures_init = new List<Texture2D>(textures);

        //----Create a new list to select a subset of pictures from the available assets 
        //Number of images to display = minimum between pictures available and set parameter
        int numImages = Mathf.Min(maxImages, textures.Count);

        Texture2D[] texturesSelection = new Texture2D[numImages];
        for (int i = 0; i < texturesSelection.Length; i++)
        {
            if (textures.Count == 0)
            {
                textures = new List<Texture2D>(textures_init);
            }
            //Algorithm to randomize the choice : simple random int
            int choice = Random.Range(0, textures.Count);
            texturesSelection[i] = textures[choice];
            textures.RemoveAt(choice);
        }

        //Call the method to display pictures on the screens
        spawnImages(texturesSelection);
    }

    void createBorders(RectTransform canvasRect)
    {
        GameObject borders = new GameObject();
        borders.name = "Borders";
        borders.transform.SetParent(canvasRect.transform);
        for (int dir = 1; dir > -2; dir -= 2)
        {
            for (int sel = 0; sel < 2; sel++)
            {
                float factor = (dir + 1) / 2f * sel * (1 - widthRatio);
                GameObject border = new GameObject();
                border.name = "border";
                border.transform.SetParent(borders.transform);
                border.transform.position = canvasRect.position + new Vector3(sel * dir * (canvasRect.rect.width / 2f + 5f) - canvasRect.rect.width * factor, (1 - sel) * dir * (canvasRect.rect.height / 2f + 5f), 0);
                border.AddComponent<BoxCollider2D>();
                border.GetComponent<BoxCollider2D>().size = new Vector2((canvasRect.rect.width * (1 - sel) + 10 * sel), (canvasRect.rect.height * sel + 10 * (1 - sel)));
            }

        }

        scriptHolder = GameObject.FindGameObjectsWithTag("ScriptHolder")[0];
    }

    //First method calls on the second one
    void spawnImages(Texture2D[] textures)
    {
        for (int i = 0; i < textures.Length; i++)
        {
            spawnImage(textures[i]);
            counter += 1;
        }
    }
    void spawnImage(Texture2D texture)
    {
        if (sqt.CountSquare() != 0)
        {
            SquareCell sqc = sqt.getSquare();
            float size = sqc.side;
            GameObject img = Instantiate(image, sqc.center, Quaternion.identity);
            img.name = string.Format("image{0}", counter);
            img.transform.SetParent(images.transform);
            (img.GetComponent<ImageController>()).StartCoroutine((img.GetComponent<ImageController>()).SetUpImage(texture, sqc));
             (img.GetComponent<ImageController>()).SetSize(size, size);
            (img.GetComponent<ImageController>()).SetPos(sqc.center);
        }
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

    // Update is called once per frame
    void Update()
    {
        //check number of image, if under threshold - pop new one as long as not in selection mode
        if (!scriptHolder.GetComponent<Select>().onSelect)
        {
            while (images.transform.childCount < maxImages && sqt.CountSquare() != 0)
            {
                int choice = Random.Range(0, textures.Count);
                spawnImage(textures[choice]);
                textures.RemoveAt(choice);
                counter += 1;

            }

        }

    }

}


