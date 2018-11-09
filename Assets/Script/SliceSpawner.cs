using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SliceSpawner : MonoBehaviour
{
    public GameObject Slice;
    List<GameObject> Slices = new List<GameObject>();

    public bool isMain = true;
    // Use this for initialization

    void Start()
    {
        SpawnMainFrame();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            clearSlices();
            if (isMain)
            {
                SpawnCategories();
            } else
            {
                SpawnMainFrame();
            }
            isMain = !isMain;
        }
    }

    void SpawnCategories() // i to iterate over categories, then dimensions entered for the size of the screen in VR - Has to be changed for projection on a real screen
    {
        for (int i = -5; i < 5; i++)
        {
            GameObject slice = Instantiate(Slice);
            slice.name = string.Format("Slice{00}", i);
            slice.GetComponent<RectTransform>().transform.position = new Vector3(i * 1600+800, 0, -5000);
            Slices.Add(slice);
            slice.GetComponent<SliceController>().SetColor(i % 2 == 0 ? Color.black : Color.gray);
        }
    }

    void SpawnMainFrame()
    {
        GameObject slice = Instantiate(Slice);
        slice.name = string.Format("Slice");
		slice.GetComponent<RectTransform> ().transform.position = new Vector3 (0, 0, -5000);
        slice.GetComponent<RectTransform>().sizeDelta = new Vector2(16000, 2000);
        Slices.Add(slice);
    }

    void clearSlices()
    {
        foreach (GameObject slice in Slices)
        {
            Destroy(slice);
        }
        Slices.Clear();
    }
}
