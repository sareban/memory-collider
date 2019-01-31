using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Video;

public class SliceControllerVid : MonoBehaviour
{
    private RectTransform canvasRect;
    public GameObject image;
    public string category = null;
    public string decade = null;
    public int maxImages = 30;
    public int maxFilms = 5;
    public float widthRatio = 0.75f;
    string photos_dir = "test/"; //photos_low : low resolution pictures directory


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


    }


    void spawnFilms(RectTransform canvasRect, VideoClip[] clips)
    {
        GameObject films = new GameObject();
        films.name = "Films";
        films.transform.SetParent(canvasRect.transform);

        float available_size = (1 - (clips.Length + 1) * 0.01f) * Mathf.Min(canvasRect.rect.width, canvasRect.rect.height);
        float min_width = canvasRect.rect.width / 15f * Mathf.Max(15f / (clips.Length + 1f), 1f); //Change value to change the number of vid onscreen
        float max_width = canvasRect.rect.width / 7.5f * Mathf.Max(15f / (clips.Length + 1f), 1f);
        SquaresTree sqt = new SquaresTree(canvasRect.rect.width * widthRatio, canvasRect.rect.height, max_width, min_width, canvasRect.position - new Vector3(canvasRect.rect.width * (1 - widthRatio) * 0.5f, 0, 0));
        //Debug.Log(sqt.CountSquare());
        for (int i = 0; i < clips.Length; i++)
        {
            SquareCell sqc = sqt.getSquare();
            float size = sqc.side;
            GameObject vid = Instantiate(image, sqc.center, Quaternion.identity);

            vid.name = string.Format("film{0}", i);
            vid.transform.SetParent(films.transform);

            var videoPlayer = vid.AddComponent<UnityEngine.Video.VideoPlayer>();
            
            videoPlayer.clip = clips[i];
            videoPlayer.playOnAwake = false;

            (vid.GetComponent<VideoController>()).StartCoroutine((vid.GetComponent<VideoController>()).SetUpFilm());
            (vid.GetComponent<VideoController>()).SetSize(size, size); //Will generate a blank screen
            (vid.GetComponent<VideoController>()).SetPos(sqc.center);
        }
    }




    // Use this for initialization
    void Start()
    {
        if (decade == null)
        {
            throw new System.Exception("Need a decade");
        }
        RectTransform canvasRect = GetComponent<RectTransform>();
        createBorders(canvasRect);

        //DIR TO LOAD PICTURES FROM
        string basedir = photos_dir + decade;
        if (category != null)
        {
            basedir += "/" + category;
        }

        //------- FILMS --------------
        // Load VideoClips from the defined base directory. PROBLEM :  Resources.LoadAll VERY RESOURCES CONSUMING, needs another delayed approach 
        List<VideoClip> clips = new List<VideoClip>(Resources.LoadAll<VideoClip>(basedir));
        //List<VideoClip> clips = new List<VideoClip>();
        List<VideoClip> clips_init = new List<VideoClip>(clips);

        //----Create a new list to select a subset of films from the available assets 
        //Number of films to display = minimum between films available and set parameter
        int numFilms = Mathf.Min(maxFilms, clips.Count);
        VideoClip[] films = new VideoClip[numFilms];

        for (int i = 0; i < films.Length; i++)
        {
            if (clips.Count == 0)
            {
                clips = new List<VideoClip>(clips_init);
            }
            //Algorithm to randomize the choice : simple random int
            //int choice = Random.Range(0, textures.Count);
            //films[i] = clips[choice];
            films[i] = clips[i];
            //clips.RemoveAt(choice);
        }


        //Call the method to display pictures on the screens
        spawnFilms(canvasRect, films);

    }



    // Update is called once per frame
    void Update()
    {
       
    }

}
