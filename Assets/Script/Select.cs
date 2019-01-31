using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    Vector3 camPos;
    Transform camTr;
    float speed = 2.5f;
    public string ObjectName;
    private Color highlightColor;
    Material originalMaterial, tempMaterial;


    void Start()
    {

        camTr = Camera.main.transform;
        camPos = camTr.position;
        Cursor.visible = true;

        highlightColor = Color.green;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clic");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray = Camera.main.ScreenPointToRay(Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)));
            Physics.Raycast(ray, out hit);
            Debug.Log(hit.collider);

                if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Photo")
            {
                Debug.Log("Coucou");
                GameObject[] blocks = GameObject.FindGameObjectsWithTag("Photo");
                foreach (GameObject go in blocks)
        
                    if (go == hit.collider.gameObject)
                    {
                        camPos.x = go.transform.position.x;
                        camPos.y = go.transform.position.y;
                  
                    }
                    else
                    {
                        go.SetActive(false);
             
                    }
                }
          
        }

        camTr.position = Vector3.Lerp(camTr.position, camPos, Time.deltaTime * speed);


    }

    void debug() { 
}
}
