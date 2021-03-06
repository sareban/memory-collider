﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//This class controls the user interactions to change the mode of display, either default, decades or categories.
public class ViewsManager : MonoBehaviour
{
    public GameObject main;
    public GameObject decadesContainer;
    public GameObject categoriesContainer;
    public GameObject cylinder;
    public bool isMain;
    public bool isTransitioning = false;
    public bool preload = false;

    public List<GameObject> decades;
    public List<GameObject> categories;

    private GameObject currentView;
    private bool isDecade;
    private int numDecades;
    private int numCategories;
    private int currentDecadeIdx = 0;
    private int currentCategoryIdx = 0;

    // Use this for initialization
    void Start()
    {
        decades.Add(main);
        foreach(string decade in new[] { "50s", "60s", "70s", "80s", "90s", "00s", "10s", })
        {
            GameObject go = decadesContainer.transform.Find(decade).gameObject;
            decades.Add(go);
            if (preload)
            {
                go.SetActive(true);
                StartCoroutine(QuickDisable(go));
            }
            if (decade=="50s")
            {
                go.SetActive(true);
                StartCoroutine(QuickDisable(go));
            }
        }
       
        numDecades = decades.Count;

        categories.Add(main);
        foreach(string category in new[] { "Portraits of science", "Science and politics", "Inside the lab", "Outside the lab", "Machines", "Accelerators", "Experiments visualization", "Data processing", "Media", "CERN sceneries" })
        {
            GameObject go = categoriesContainer.transform.Find(category).gameObject;
            categories.Add(go);
            if (preload)
            {
                go.SetActive(true);
                StartCoroutine(QuickDisable(go));
            }
        }


        numCategories = categories.Count;
        currentView = main;
        isMain = true;
        isDecade = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((OVRInput.GetUp(OVRInput.Button.Back) || Input.GetKey(KeyCode.Backspace)) && !isTransitioning && !isMain)
        {
            isDecade = false;
            isMain = true;
            currentCategoryIdx = 0;
            currentDecadeIdx = 0;
            Transition(currentView, main, false, false);
            currentView = main;
            VideoPlayer[] videoPlayers = Object.FindObjectsOfType<VideoPlayer>();
            foreach (VideoPlayer videoPlayer in videoPlayers)
            {
                videoPlayer.Play();
            }

        }

        // Go forward in decades
        if ((OVRInput.GetDown(OVRInput.Button.DpadRight) || Input.GetKey(KeyCode.RightArrow)) && !isTransitioning && (isMain || isDecade))
        {
            if (currentDecadeIdx == numDecades - 1)
            {
                return;
            }
            isMain = false;
            isDecade = true;
            currentDecadeIdx++;
            Transition(currentView, decades[currentDecadeIdx], true, true);
            currentView = decades[currentDecadeIdx];
        }

        // Go backward in decades
        if ((OVRInput.GetDown(OVRInput.Button.DpadLeft) || Input.GetKey(KeyCode.LeftArrow)) && !isTransitioning && (!isMain && isDecade))
        {
            currentDecadeIdx--;
            Transition(currentView, decades[currentDecadeIdx], false, true);
            currentView = decades[currentDecadeIdx];

            isMain = currentDecadeIdx == 0;
            isDecade = !isMain;
        }

        // Change category
        if ((OVRInput.GetDown(OVRInput.Button.DpadDown) || Input.GetKey(KeyCode.DownArrow)) && !isTransitioning && (isMain || !isDecade))
            {
            if (currentCategoryIdx == numCategories - 1)
            {
                return;
            }
            isMain = false;
            isDecade = false;
            currentCategoryIdx++;
            Transition(currentView, categories[currentCategoryIdx], true, false);
            currentView = categories[currentCategoryIdx];
        }

        if ((OVRInput.GetDown(OVRInput.Button.DpadUp) || Input.GetKey(KeyCode.UpArrow)) && !isTransitioning && (!isMain && !isDecade))
        {
            currentCategoryIdx--;
            Transition(currentView, categories[currentCategoryIdx], false, false);
            currentView = categories[currentCategoryIdx];

            isMain = currentCategoryIdx == 0;
            if (isMain)
            {
                VideoPlayer[] videoPlayers = Object.FindObjectsOfType<VideoPlayer>();
                foreach (VideoPlayer videoPlayer in videoPlayers)
                {
                    videoPlayer.Play();
                }

            }
            isDecade = false;
        }


    }

    private void Transition(GameObject from, GameObject to, bool next, bool decade)
    {
        to.SetActive(true);
        to.GetComponent<Animator>().SetBool("Decade", decade);
        from.GetComponent<Animator>().SetBool("Decade", decade);

        if (decade) 
        {
            if (next)
            {
                cylinder.GetComponent<Animator>().SetBool("transition", true);
            }
            else
            {
                cylinder.GetComponent<Animator>().SetBool("transitionPrev", true);
            }
        } else
        {
            to.GetComponent<Animator>().SetBool("Next", next);
            to.GetComponent<Animator>().enabled = false;
            from.GetComponent<Animator>().SetBool("Next", next);
        }

        to.GetComponent<Animator>().enabled = true;

        from.GetComponent<Animator>().SetTrigger("Exit");
        StartCoroutine(DisableGameObject(from));
    }

    private IEnumerator QuickDisable(GameObject toDisable)
    {
        yield return new WaitForSeconds(1);
        toDisable.SetActive(false);
    }

    private IEnumerator DisableGameObject(GameObject toDisable)
    {
        isTransitioning = true;
        yield return new WaitForSeconds(1);
        toDisable.SetActive(false);
        isTransitioning = false;
        cylinder.GetComponent<Animator>().SetBool("transition", false);
        cylinder.GetComponent<Animator>().SetBool("transitionPrev", false);
    }
}