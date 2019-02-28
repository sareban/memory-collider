using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using UnityEditor;

// This class implement a single video  displayed on the screen. It is instatiated by a SliceControllerVid
public class VideoController : MonoBehaviour
{
    private static int numInstances = 0;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Vector3 defaultRot;
    public float maxSpeed = 20f;
    int destroyTime = 5;
    private bool is_selected = false;
    private bool is_fading = false;
    RawImage screen;
    Color baseColor;


    // Use this for initialization : define statics and dynamics properties
    void Start()
    {
        gameObject.tag = "Video";
        defaultRot = transform.eulerAngles;
        rb = GetComponent<Rigidbody2D>();
        baseColor = screen.color;

        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta -= new Vector2(20, 20);
        numInstances += 1;

    }

    // Method used to display a video on a screen
    public IEnumerator SetUpFilm()
    {
        //Prepare screen
        screen = GetComponent<RawImage>();
        var tex = new RenderTexture(256, 256, 24);
        tex.Create();
        screen.texture = tex;
        screen.enabled = true;

        Color c = screen.color;
        c.a = 0;
        screen.color = c;
        while (screen.color.a < 1f)
        {
            c.a = c.a + 0.1f;
            screen.color = c;
            yield return new WaitForSeconds(0.00001f);
        }

        //Adjust screen

        Vector3 scale;
        if (screen.texture.width > screen.texture.height)
        {
            scale = new Vector3(1f, (float)screen.texture.height / (float)screen.texture.width, 1f);
        }
        else
        {
            scale = new Vector3((float)screen.texture.width / (float)screen.texture.height, 1f, 1f);
        }
        screen.transform.localScale = scale;
        bc = GetComponent<BoxCollider2D>();
        bc.transform.localScale = scale;

        //Load videoPlayer
        var videoPlayer = GetComponent<VideoPlayer>();

        //To disabéle afterwards
        videoPlayer.isLooping = true;

        //Wait until video is prepared
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        //Play video once prepared
        if (videoPlayer.isPrepared)
        {
            videoPlayer.targetTexture = (RenderTexture)screen.texture; 
            videoPlayer.Play();
        }

        //Remove video after a while. Sets the amount of time before a video is removed
        yield return new WaitForSeconds(Random.Range(10.0f, 25.0f));
        //StartCoroutine(Fade());
        //sqc.release();
        //yield return null;
    }

    // Sets the dimensions of the film
    public void SetSize(float width, float height)
    {
        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(width, height);
        GetComponent<BoxCollider2D>().size = new Vector2(width, height);
    }
    // Sets position of the film 
    public void SetPos(Vector3 pos)
    {
        RectTransform r = GetComponent<RectTransform>();
        r.transform.position = pos;
    }

    //Provide a fading out animation before deleting the video
    IEnumerator Fade()
    {
        while (screen.color.a >0f)
        {
            Color c = screen.color;
            c.a = c.a - 0.1f;
            screen.color = c;
            yield return new WaitForSeconds(0.00001f);
        }
        Destroy(gameObject, 1);
    }

    // is_selected and is_fading prevent certain automatic and user behaviors when set to true
    public void setSelected()
    {
        is_selected = true;
    }
    public void setUnselected()
    {
        is_selected = false;
    }
    public bool isSelected()
    {
        return is_selected;
    }
    public bool isFading()
    {
        return is_fading;
    }

    public void setTransparent()
    {
        screen.color = Color.clear;
    }
    public void setColor()
    {
        Color c = baseColor;
        c.a = 1;
        screen.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = defaultRot;
        if (rb.velocity.x + rb.velocity.y < 0.0001)
        {
            float dirX = (float)Random.Range(0, 1);
            dirX = 2f * dirX - 1f;
            float dirY = (float)Random.Range(0, 1);
            dirY = 2f * dirY - 1f;
            rb.AddForce(new Vector2(dirX * Random.Range(0f, maxSpeed * 0.05f), dirY * Random.Range(0f, maxSpeed * 0.05f)));
        }
        rb.velocity = new Vector2(Mathf.Min(rb.velocity.x, maxSpeed), Mathf.Min(rb.velocity.y, maxSpeed));

    }
}

