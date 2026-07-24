using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] TMP_Text resourceText;
    [SerializeField] float resources, resourceIncrementFromFarms, resourceIncrementPerFollower, resourceIncrementPerFollowerFromStatues, 
        timeForResourceProduction, timeForResourceProductionDecreaseFromStatues;
    [Header("Progress bar")]
    [SerializeField] RectTransform resourceBar;
    [SerializeField] float resourceBarMaxWidth, resourceBarHeight;
    [Header("Icon Progress Spin")]
    [SerializeField] Image resourceIcon;

    private MeepleManager meepleManager;
    private float startTime, endTime;

    void Start()
    {
        meepleManager = gameObject.GetComponent<MeepleManager>();
        UpdateResourceTextUI();
        StartCoroutine(ResourceProduction());
    }
    void Update()
    {
        SetResourceProgressBar();
        SetResourceProgressSpin();
    }

    public void AddResources(float newResources)
    {
        resources += newResources;
        UpdateResourceTextUI();
    }
    public bool TrySpendResources(float spendResources)
    {
        if (resources - spendResources >= 0)
        {
            resources -= spendResources;
            UpdateResourceTextUI();
            return true;
        }
        return false;
    }
    public void UpdateResourceTextUI()
    {
        resourceText.text = ((int)resources).ToString();
    }

    public void AddResourceIncrementFromFarms(float resourceIncrement)
    {
        resourceIncrementFromFarms += resourceIncrement;
    }
    public void AddResourceIncrementPerFollowerFromStatues(float resourceIncrementPerFollower)
    {
        resourceIncrementPerFollowerFromStatues += resourceIncrementPerFollower;
    }
    public void AddTimeForResourceProductionDecreaseFromStatues(float timeForResourceProductionDecrease)
    {
        timeForResourceProductionDecreaseFromStatues += timeForResourceProductionDecrease;
    }

    public void SetResourceProgressBar()
    {
        float width = resourceBarMaxWidth * (Time.time - startTime) / (endTime - startTime);
        resourceBar.sizeDelta = new Vector2(width, resourceBarHeight);
    }
    public void SetResourceProgressSpin()
    {
        float percent = (Time.time - startTime) / (endTime - startTime);
        resourceIcon.fillAmount = percent;
    }

    private void SetStartTime()
    {
        startTime = Time.time;
    }
    private void SetEndTime()
    {
        endTime = startTime + timeForResourceProduction - timeForResourceProductionDecreaseFromStatues;
    }

    public IEnumerator ResourceProduction()
    {
        SetStartTime();
        SetEndTime();
        while (true)
        {
            SetEndTime();
            if (Time.time > endTime)
            {
                float newResources = (resourceIncrementPerFollower + resourceIncrementPerFollowerFromStatues) * (float)meepleManager.GetPopulationFollowers() + resourceIncrementFromFarms;
                AddResources(newResources);
                SetStartTime();
                SetEndTime();
            }
            if(endTime - Time.time >= 1)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(endTime - Time.time);
            }
        }
    }

    //public IEnumerator ResourceProduction()
    //{
    //    float start = Time.time;
    //    while (true)
    //    {
    //        if (Time.time > start + timeForResourceProduction - timeForResourceProductionDecreaseFromStatues)
    //        {
    //            float newResources = (resourceIncrementPerFollower + resourceIncrementPerFollowerFromStatues) * (float)meepleManager.GetPopulationFollowers() + resourceIncrementFromFarms;
    //            AddResources(newResources);
    //            start = Time.time;
    //        }
    //        yield return new WaitForSeconds(1);
    //    }
    //}
}
