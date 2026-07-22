using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MeepleManager : MonoBehaviour
{
    [Header("Population")]
    [SerializeField] int population, startingPopulation, startingFollowers;
    [Tooltip("Only read. Serialized for ease of checking values")][SerializeField] int populationFollowers;
    [Tooltip("Only read. Serialized for ease of checking values")][SerializeField] int populationResenters;
    [Header("Resenter bar")]
    [SerializeField] RectTransform resenterBar;
    [SerializeField] float resenterBarMaxWidth, resenterBarHeight;
    [Header("Conversion rates")]
    [SerializeField] float timeForFollowerConversion, timeForResenterConversion;


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
        SetResenterBar();
    }

    public int GetPopulationFollowers()
    {
        return populationFollowers;
    }

    public void AddFollowers(int num = 1, bool decreaseResenter = true)
    {
        populationFollowers++;
        if (decreaseResenter)
        {
            populationResenters--;
        }
        population = populationFollowers + populationResenters;
        SetResenterBar();
    }
    public void AddResenters(int num = 1, bool decreaseFollower = true)
    {
        if (decreaseFollower)
        {
            populationFollowers--;
        }
        populationResenters++;
        population = populationFollowers + populationResenters;
        SetResenterBar();
    }
    public void SetResenterBar()
    {
        float width = resenterBarMaxWidth * ((float)populationResenters / (float)population);
        resenterBar.sizeDelta = new Vector2(width, resenterBarHeight);
    }

    public IEnumerator FollowerConversion()
    {
        float start = Time.time;
        while (true)
        {
            if (Time.time > start + timeForFollowerConversion)
            {
                AddFollowers();
                start = Time.time;
            }
            yield return new WaitForSeconds(1);
        }
    }
    public IEnumerator ResenterConversion()
    {
        float start = Time.time;
        while (true)
        {
            if (Time.time > start + timeForFollowerConversion)
            {
                AddResenters();
                start = Time.time;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
