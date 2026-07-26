using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ApocalypseSequence : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] GameObject musicOG;
    [SerializeField] GameObject musicApocalypse;
    [Header("Other apocalypse toggles")]
    [SerializeField] GameObject shop;
    [SerializeField] GameObject pointBreakdown, pointExit, pointsFinal, menuButtons, backgroundApocalypse, fireballPrefab;
    [SerializeField] Transform leftFireballBound, rightFireballBound;
    private MeepleManager meepleManager;
    private ResourceManager resourceManager;
    private PointManager pointManager;

    private void Start()
    {
        meepleManager = GetComponent<MeepleManager>();
        resourceManager = GetComponent<ResourceManager>();
        pointManager = GetComponent<PointManager>();
    }

    public void ApocalypseTime()
    {
        //stop meeple conversion
        meepleManager.StopAllCoroutines();
        //stop resource production
        resourceManager.StopAllCoroutines();
        
        //switch music
        musicOG.SetActive(false);
        musicApocalypse.SetActive(true);
        //turn off extra info
        shop.SetActive(false);
        pointBreakdown.SetActive(false);
        pointExit.SetActive(false);

        //update point value and turn on object
        TMP_Text pointsFinalTMP = pointsFinal.GetComponentInChildren<TMP_Text>();
        string pointsFinalString = pointsFinalTMP.text;
        pointsFinalString = pointsFinalString.Replace("[totalPoints]", pointManager.GetPoints().ToString());
        pointsFinalTMP.text = pointsFinalString;
        pointsFinal.SetActive(true);

        //turn on menu buttons
        menuButtons.SetActive(true);

        //turn on apocalypse animation, panic everyone
        List<MeepleScript> meepleList = meepleManager.GetMeepleList();
        foreach (MeepleScript meeple in meepleManager.GetMeepleList()) 
        {
            meeple.apocalypseAnimation();
        }
        //spawn fireballs
        StartCoroutine(SpawnFireballsOverTime());
        //change background
        StartCoroutine(FadeBackground(backgroundApocalypse.GetComponent<SpriteRenderer>()));
    }

    public void SpawnFireballInBounds(float y)
    {
        float x = UnityEngine.Random.Range(leftFireballBound.position.x, rightFireballBound.position.x);
        Instantiate(fireballPrefab, new Vector2(x, y), Quaternion.identity);
    }

    public IEnumerator SpawnFireballsOverTime()
    {
        float y = leftFireballBound.position.y;
        while (true)
        {
            SpawnFireballInBounds(y);
            yield return new WaitForSeconds(1);
        }
    }
    public IEnumerator FadeBackground(SpriteRenderer apocalypseRenderer)
    {
        Color baseColor = apocalypseRenderer.color;

        for (float i = .01f; i < 1f; i += .01f)
        {
            baseColor.a = i;
            apocalypseRenderer.color = baseColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
