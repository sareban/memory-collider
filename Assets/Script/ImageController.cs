using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    private static int numInstances = 0;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Vector3 defaultRot;
    public float maxSpeed = 50f;
    Image img;
    private bool is_selected = false;
    private bool is_fading = false;

    // Use this for initialization
    void Start()
    {

        gameObject.tag = "Photo";
        defaultRot = transform.eulerAngles;
        rb = GetComponent<Rigidbody2D>();
        float dirX = (float)Random.Range(0, 1);
        dirX = 2f * dirX - 1f;
        float dirY = (float)Random.Range(0, 1);
        dirY = 2f * dirY - 1f;
        rb.AddForce(new Vector2(dirX * Random.Range(0f, maxSpeed * 50), dirY * Random.Range(0f, maxSpeed * 50)));

        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta -= new Vector2(20, 20);
        numInstances += 1;
     
    }


    public void SetSize(float width, float height)
    {
        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(width, height);
        GetComponent<BoxCollider2D>().size = new Vector2(width, height);
    }

    public void SetPos(Vector3 pos)
    {
        RectTransform r = GetComponent<RectTransform>();
        r.transform.position = pos;
    }

    //Provide a fading out animation before deleting the image
    IEnumerator Fade()
    {
        //better function :   sprite.color = new Color(1f,1f,1f,Mathf.SmoothStep(minimum, maximum, t));
        //public void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale);
        is_fading = true;

        while (img.color.a > 0f)
        {
            Color c = img.color;
            c.a = c.a - 0.1f;
            img.color = c;
            yield return new WaitForSeconds(0.00001f);
        }
        Destroy(gameObject, 1);
    }

    //TO REMOVE
    public void SetUpImage(string imgPath)
    {
        img = GetComponent<Image>();
        img.enabled = true;

        img.sprite = SpritesCache.instance.LoadSprite(imgPath);
        Vector3 scale;
        if (img.sprite.texture.width > img.sprite.texture.height)
        {
            scale = new Vector3(1f, (float)img.sprite.texture.height / (float)img.sprite.texture.width, 1f);
        }
        else
        {
            scale = new Vector3((float)img.sprite.texture.width / (float)img.sprite.texture.height, 1f, 1f);
        }
        img.transform.localScale = scale;
        bc = GetComponent<BoxCollider2D>();
        bc.transform.localScale = scale;
    }

    public SpriteRenderer GetImgRenderer()
    {
        return GetComponent<Image>().GetComponent<SpriteRenderer>();
    }

    public IEnumerator SetUpImage(Texture2D texture, SquareCell sqc)
    {
        img = GetComponent<Image>();

        img.sprite = SpritesCache.instance.LoadSprite(texture);
        img.enabled = true;

        Color c = img.color;
        c.a = 0;
        img.color = c;
        while (img.color.a < 1f)
        {
             c.a = c.a + 0.1f;
             img.color = c;
            yield return new WaitForSeconds(0.00001f);
        }

       Vector3 scale;
        if (img.sprite.texture.width > img.sprite.texture.height)
        {
            scale = new Vector3(1f, (float)img.sprite.texture.height / (float)img.sprite.texture.width, 1f);
        }
        else
        {
            scale = new Vector3((float)img.sprite.texture.width / (float)img.sprite.texture.height, 1f, 1f);
        }
        img.transform.localScale = scale;
        bc = GetComponent<BoxCollider2D>();
        bc.transform.localScale = scale;

        //Remove image after a while. Sets the amount of time before an image is removed. Format : min, max
        
        yield return new WaitForSeconds(Random.Range(10.0f, 50.0f));
        while (is_selected)
        {
            yield return new WaitForSeconds(Random.Range(10.0f, 50.0f));
        }
        //When executed, image will fade
        StartCoroutine(Fade());

        sqc.release();
        yield return null;
    }

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

    void Update()
    {
        transform.eulerAngles = defaultRot;
        if(rb.velocity.x + rb.velocity.y < 0.0001)
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
