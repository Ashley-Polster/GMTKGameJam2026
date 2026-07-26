using System;
using System.Collections;
using System.Collections.Generic;
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
    private PointManager pointManager;
    [SerializeField] List<MeepleScript> meepleList = new List<MeepleScript>();
    [SerializeField] GameObject meeplePrefab;
    [SerializeField] GameObject priestPrefab;
    [SerializeField] Transform meepleSpawnPoint;

    void Awake()
    {
        pointManager = gameObject.GetComponent<PointManager>();
        population = startingPopulation;
        populationFollowers = startingFollowers;
        populationResenters = startingPopulation - startingFollowers;
        for (int i = 0; i < startingPopulation; i++)
        {
            if (i < startingFollowers)
                SpawnMeeple();
            //spawn resenters
            else
                SpawnMeeple(true);
        }
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
    public int GetPopulation()
    {
        return population;
    }
    public List<MeepleScript> GetMeepleList()
    {
        return meepleList;
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
        pointManager.UpdatePointInfo();
        for(int i = 0; i < num; i++)
        {
            SpawnMeeple();
        }
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
        pointManager.UpdatePointInfo();
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
        Instantiate(priestPrefab, meepleSpawnPoint.position, Quaternion.identity);
    }

    public void SpawnMeeple(bool isResenter = false)
    {
        GameObject newMeeple = Instantiate(meeplePrefab, meepleSpawnPoint.position, Quaternion.identity);
        MeepleScript meepleObject = newMeeple.GetComponent<MeepleScript>();
        if (isResenter)
            meepleObject.changeMeepleType();
        meepleList.Add(meepleObject);
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
                foreach(MeepleScript m in meepleList)
                {
                    if (m.isResenter)
                    {
                        m.changeMeepleType();
                        break;
                    }
                }
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
                foreach(MeepleScript m in meepleList)
                {
                    if (!m.isResenter)
                    {
                        m.changeMeepleType();
                        break;
                    }
                }
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
