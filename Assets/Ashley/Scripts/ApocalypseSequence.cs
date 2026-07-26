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
    [SerializeField] GameObject losingMessage;
    [SerializeField] GameObject shop, pointBreakdown, pointExit, pointsFinal, menuButtons, backgroundApocalypse, fireballPrefab;
    [SerializeField] Transform leftFireballBound, rightFireballBound;
    private MeepleManager meepleManager;
    private ResourceManager resourceManager;
    CountdownManager countdownManager;
    private PointManager pointManager;

    private void Start()
    {
        meepleManager = GetComponent<MeepleManager>();
        resourceManager = GetComponent<ResourceManager>();
        countdownManager = GetComponent<CountdownManager>();
        pointManager = GetComponent<PointManager>();
    }

    public void ClearBusyCoroutinesAndScreen()
    {
        //stop meeple conversion
        meepleManager.StopAllCoroutines();
        //stop resource production
        resourceManager.StopAllCoroutines();

        //turn off extra info
        shop.SetActive(false);
        pointBreakdown.SetActive(false);
        pointExit.SetActive(false);

    }
    public void LosingSequence()
    {
        Debug.Log("Losing");
        ClearBusyCoroutinesAndScreen();
        //turn on menu buttons
        menuButtons.SetActive(true);
        //turn on losing message
        losingMessage.SetActive(true);
    }
    public void ApocalypseTime()
    {
        Debug.Log("Winning");
        ClearBusyCoroutinesAndScreen();
        
        //switch music
        musicOG.SetActive(false);
        musicApocalypse.SetActive(true);

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
