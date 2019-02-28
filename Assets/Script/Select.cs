using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// This class controls the possible user interactions with archives items such as pictures and films on screen, allowing to select them for zooming in.
public class Select : MonoBehaviour
{
    Vector3 camOrigin;
    Vector3 camNew;
    Transform camTr;
    float speed = 2.5f;
    public float maxSpeed = 20f;
    Material originalMaterial, tempMaterial;
    public bool onSelect = false;
    private bool wait = false;
    public Color baseColor = Color.white;
    private GameObject current;
    private float currentSpeed;
    GameObject[] pictures;
    GameObject[] videos;

    void Start()
    {
        camTr = Camera.main.transform;
        camOrigin = camTr.position;
        camNew = camTr.position;
        
        Cursor.visible = true;

    }

    // Methods to select and deselect pictures and films. While an item is selected, all the other items should be masked. Films should resume playing on release.
    private void SelectPicture()
    {
        onSelect = true;
        wait = true;
        current.GetComponent<Image>().color = baseColor;
        current.GetComponent<ImageController>().setSelected();
        current.GetComponent<ImageController>().transform.localScale = new Vector3(3f,3f,3f);
        Vector3 pos = current.transform.position;
        current.transform.localPosition = new Vector3(pos.x,0f,pos.z);
        current.GetComponent<ImageController>().stop();

        foreach (GameObject go in pictures)
        {
            if (go != current)
            {
                go.GetComponent<ImageController>().setTransparent();
                go.GetComponent<ImageController>().setSelected();

            }
        }

        foreach (GameObject go in videos)
        {
            go.GetComponent<VideoPlayer>().Pause();
            go.GetComponent<VideoController>().setTransparent();
            go.GetComponent<VideoController>().setSelected();
        }
        StartCoroutine(Wait());
    }
    private void DeselectPicture()
    {
        wait = true;
        foreach (GameObject go in pictures)
        {
            if (go != current)
            {
                go.GetComponent<ImageController>().setColor();
                go.GetComponent<ImageController>().setUnselected();
            }
        }

        foreach (GameObject go in videos)
        {
                go.GetComponent<VideoPlayer>().Play();
                go.GetComponent<VideoController>().setColor();
                go.GetComponent<VideoController>().setUnselected();
        }

        onSelect = false;
        current.GetComponent<ImageController>().setUnselected();
        current.GetComponent<ImageController>().move();
        current.GetComponent<ImageController>().transform.localScale = new Vector3(1f, 1f, 1f);


        StartCoroutine(Wait());
    }
    private void SelectVideo()
    {
        onSelect = true;
        wait = true;
        current.GetComponent<RawImage>().color = baseColor;
        current.GetComponent<VideoController>().setSelected();
        current.GetComponent<VideoController>().transform.localScale = new Vector3(2f, 2f, 2f);
        Vector3 pos = current.transform.position;
        current.transform.localPosition = new Vector3(pos.x, 0f, pos.z);


        foreach (GameObject go in pictures)
        {
                go.GetComponent<ImageController>().setTransparent();
                go.GetComponent<ImageController>().setSelected();
        }

        foreach (GameObject go in videos)
        {
            if (go != current)
            {
                go.GetComponent<VideoPlayer>().Pause();
                go.GetComponent<VideoController>().setTransparent();
                go.GetComponent<VideoController>().setSelected();

            }
        }
        StartCoroutine(Wait());
       
    }
    private void DeselectVideo()
    {
        wait = true;
        foreach (GameObject go in videos)
        {
            if (go != current)
            {
                go.GetComponent<VideoPlayer>().Play();
                go.GetComponent<VideoController>().setColor();
                go.GetComponent<VideoController>().setUnselected();

            }
        }

        foreach (GameObject go in pictures)
        {
                go.GetComponent<ImageController>().setColor();
                go.GetComponent<ImageController>().setUnselected();
        }

        onSelect = false;
        current.GetComponent<VideoController>().setUnselected();
        current.GetComponent<VideoController>().transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(Wait());
    }


    // Provide a wait function to avoid instant select/deselect when using the trigger of the oculus remote. Time parameter could be ajusted for smoothness
    private IEnumerator Wait()
    {

        yield return new WaitForSeconds(0.3f);
        wait = false;
    }


    // Provides a projection from the user view (cylinder) onto the real view (plane screen)
    // !! To modify if size of screen changes !!
    private Vector2 RayOrigin()
    {
       Vector3 origin = camTr.forward;
       float x, y;

        if ( origin.x > 0 && origin.z > 0)
        {
            x = -8000f + (Vector3.Angle(new Vector3(origin.x, 0, origin.z), new Vector3(0f, 0f, 1f))/90f) * 4000f;
        }
        else if (origin.x > 0 && origin.z < 0)
        {
            x = 0f - (Vector3.Angle(new Vector3(origin.x, 0, origin.z), new Vector3(0f, 0f, -1f))/90f) * 4000f;
        }
        else if (origin.x < 0 && origin.z > 0)
        {
            x = 8000f - (Vector3.Angle(new Vector3(origin.x, 0, origin.z), new Vector3(0f, 0f, 1f))/90f) * 4000f;
        }
        else
        {
           x = 0f + (Vector3.Angle(new Vector3(origin.x, 0, origin.z), new Vector3(0f, 0f, -1f))/90f) * 4000f;
        }

        return new Vector2(x, origin.y * 2200f);

    }

    void Update()
    {
        Debug.DrawLine(new Vector3(RayOrigin().x, RayOrigin().y, 9000f), new Vector3(0, 0, 11000f), Color.red);
        pictures = GameObject.FindGameObjectsWithTag("Photo");
        videos = GameObject.FindGameObjectsWithTag("Video");

        // To deselect an item currently selected
        if (onSelect)
        {
            if (!wait && (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || Input.GetMouseButtonDown(0)))
            {
                if (current.tag == "Photo")
                {
                        DeselectPicture();
                }
                else if (current.tag == "Video")
                {
                        DeselectVideo();
                }
            }
        }

        // To color the item pointed at and select an item
        else
        { 
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(RayOrigin().x, RayOrigin().y), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Photo" && !onSelect)
            {
                if (current != hit.collider.gameObject && current != null)
                {
                    if (current.tag == "Photo")
                    {
                        current.GetComponent<Image>().color = baseColor;
                    }
                    else if (current.tag == "Video")
                    {
                        current.GetComponent<RawImage>().color = baseColor;
                    }
                    current = null;
                }

                current = hit.collider.gameObject;
                current.GetComponent<Image>().color = Color.red;
            }

            else if (hit.collider != null && hit.collider.tag == "Video" && !onSelect)
            {
                if (current != hit.collider.gameObject && current != null)
                {
                    if (current.tag == "Photo")
                    {
                        current.GetComponent<Image>().color = baseColor;
                    }
                    else if (current.tag == "Video")
                    {
                        current.GetComponent<RawImage>().color = baseColor;
                    }
                    current = null; 
                }

                current = hit.collider.gameObject;
                current.GetComponent<RawImage>().color = Color.red;
            }

            else //If we're not pointing at anything
            {
                if (current != null)
                {
                    if (current.tag == "Photo")
                    {
                        current.GetComponent<Image>().color = baseColor;
                    }
                    else if (current.tag == "Video")
                    {
                        current.GetComponent<RawImage>().color = baseColor;
                    }
                    current = null;
                }

            }
            
            // To select a picture or film
            if (!onSelect)
            {  
                if (!wait && (current != null && (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)  || Input.GetMouseButtonDown(0))))
                {
                    if (current.tag == "Photo")
                    {
                        if (!current.GetComponent<ImageController>().isFading())
                        {
                            SelectPicture();
                        }
                    }
                    else if (current.tag == "Video")
                    {
                        if (!current.GetComponent<VideoController>().isFading())
                        {
                            Debug.Log("Select");
                            SelectVideo();
                        }
                    }



                }
            }
        }
        







    }
}
