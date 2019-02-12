using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    Vector3 camPos;
    Transform camTr;
    float speed = 2.5f;
    public string ObjectName;
    private Color highlightColor;
    Material originalMaterial, tempMaterial;
    private bool onSelect = false;
    public Color baseColor = Color.white;
    private GameObject current;
    

    void Start()
    {

        camTr = Camera.main.transform;
        camPos = camTr.position;
        Cursor.visible = true;

        highlightColor = Color.green;
    }

    private void SelectPictureTest()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // APPLY GEOMETRICAL TRANSFORMATION TO REACH THE SCREEN ?
        // Ray ray = Camera.main.ScreenPointToRay(Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0))); // APPLY TRANSFORMATION HERE
        //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)), camTr.forward);

        //  Debug.DrawRay(Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)), camTr.forward * 15000f, Color.red);

        Ray ray = new Ray((Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0))), camTr.forward * 15000f);
        Physics.Raycast(ray, out hit);
        Debug.DrawRay((Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0))), camTr.forward * 15000f, Color.red);
        Debug.Log(hit.collider);

        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Photo")
        {
            Debug.Log("Coucou");
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Photo");
            foreach (GameObject go in blocks)

                if (go == hit.collider.gameObject)
                {
                    //if (!go.isFading())
                    camPos.x = go.transform.position.x;
                    camPos.y = go.transform.position.y;

                }
                else
                {
                    go.SetActive(false);

                }
        }
    

        camTr.position = Vector3.Lerp(camTr.position, camPos, Time.deltaTime * speed);


    }

    private void SelectPicture()
    {


    }

        private void DeselectPicture()
    {


    }

    private IEnumerator Highlight()
    {


        yield return null;
    }
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }


    //To modify if size of screen changes
    private Vector2 RayOrigin()
    {
       Vector3 origin = camTr.forward;
        //Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        if (origin.y > 0)
        {
           y = 0 + Mathf.Sin(Vector3.Angle(new Vector3(0f, origin.y, origin.z), new Vector3(0f, 0f, 1f)) * Mathf.Deg2Rad) * 1780f;
          // y = 0 + (Vector3.Angle(new Vector3(0f, origin.y, origin.z), new Vector3(0f, 0f, 1f)) /90) * 1800f;
        }
        else
        {
             y = 0 + -( Mathf.Sin(Vector3.Angle(new Vector3(0f, origin.y, origin.z), new Vector3(0f, 0f, 1f)) * Mathf.Deg2Rad)) * 1780f;
            //y = 0 + -(Vector3.Angle(new Vector3(0f, origin.y, origin.z), new Vector3(0f, 0f, 1f)) /90f) * 1800f;
        }
        Debug.Log(new Vector2(x, y+200f));
        return new Vector2(x, y);
    }
    void Update()
    {

        //   if (Input.GetMouseButtonDown(0) && !onSelect)
        //   {
        //      SelectPicture()
        //   }

        //   if (Input.GetMouseButtonDown(0) && onSelect)
        //   {
        //      DeselectPicture()
        //   }

        Debug.DrawLine(new Vector3(RayOrigin().x, RayOrigin().y, 9000f), new Vector3(0, 0, 11000f), Color.red);
       RaycastHit2D hit = Physics2D.Raycast(new Vector2(RayOrigin().x, RayOrigin().y), Vector2.zero);
        //   Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Color.red);
        if (hit.collider != null && hit.collider.tag == "Photo")
        {
            Debug.Log(hit.collider.gameObject);
            if (current != hit.collider.gameObject && current != null)
            {
                current.GetComponent<Image>().color = baseColor;
                current = null;
            }

            current = hit.collider.gameObject;
            current.GetComponent<Image>().color = Color.red;
        }

      
        else //If we're not pointing at anything
        {       
            if (current != null)
            {
                current.GetComponent<Image>().color = baseColor;
                current = null;
            }

        }




    }
}
