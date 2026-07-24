using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MeepleManager : MonoBehaviour
{
    [Header("Population")]
    [SerializeField] int population, startingPopulation, startingFollowers;
    [Tooltip("Only read. Serialized for ease of checking values")][SerializeField] int populationFollowers;
    [Tooltip("Only read. Serialized for ease of checking values")][SerializeField] int populationResenters;
    [Header("Resenter bar")]
    [SerializeField] RectTransform resenterBar;
    [SerializeField] GameObject handleBar;
    [SerializeField] Sprite followerMajorityHandle, resenterMajorityHandle;
    [SerializeField] float resenterBarMaxWidth, resenterBarHeight;
    [Header("Conversion rates")]
    [SerializeField] float timeForFollowerConversion, timeForResenterConversion, timeForFollowerConversionDecreaseFromPriests;


    void Start()
    {
        population = startingPopulation;
        populationFollowers = startingFollowers;
        populationResenters = startingPopulation - startingFollowers;
        SetResenterBar();
        StartCoroutine(FollowerConversion());
        StartCoroutine(ResenterConversion());
    }

    void Update()
    {
        //may move to relavent functions once made
        //SetResenterBar();
    }

    public int GetPopulationFollowers()
    {
        return populationFollowers;
    }

    public void AddFollowers(bool decreaseResenter = true, int num = 1)
    {
        populationFollowers += num;
        if (decreaseResenter)
        {
            populationResenters -= num;
        }
        population = populationFollowers + populationResenters;
        SetResenterBar();
    }
    public void AddResenters(bool decreaseFollower = true, int num = 1)
    {
        if (decreaseFollower)
        {
            populationFollowers -= num;
        }
        populationResenters += num;
        population = populationFollowers + populationResenters;
        SetResenterBar();
    }
    public void SetResenterBar()
    {
        float width = resenterBarMaxWidth * ((float)populationResenters / (float)population);
        resenterBar.sizeDelta = new Vector2(width, resenterBarHeight);
        RectTransform handleBarRectTransform = handleBar.GetComponent<RectTransform>();
        handleBarRectTransform.anchoredPosition = new Vector2(width, 0);
        Image handleBarImage = handleBar.GetComponent<Image>();
        if (populationFollowers >= populationResenters)
        {
            handleBarImage.sprite = followerMajorityHandle;
        }
        else
        {
            handleBarImage.sprite = resenterMajorityHandle;
        }
    }

    public void AddTimeForFollowerConversionDecreaseFromPriests(float timeForFollowerConversionDecrease)
    {
        timeForFollowerConversionDecreaseFromPriests += timeForFollowerConversionDecrease;
    }

    public IEnumerator FollowerConversion()
    {
        float start = Time.time;
        while (true)
        {
            float endTime = start + timeForFollowerConversion - timeForFollowerConversionDecreaseFromPriests;
            if (Time.time >= endTime)
            {
                Debug.Log("Converting to follower");
                AddFollowers();
                start = Time.time;
            }
            if (endTime - Time.time >= 1)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(endTime - Time.time);
            }
        }
    }
    public IEnumerator ResenterConversion()
    {
        float start = Time.time;
        while (true)
        {
            float endTime = start + timeForResenterConversion;
            if (Time.time >= endTime)
            {
                Debug.Log("Converting to resenter");
                AddResenters();
                start = Time.time;
            }
            if (endTime - Time.time >= 1)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(endTime - Time.time);
            }
        }
    }
}
