using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] TMP_Text resourceText;
    [SerializeField] float resources, resourceIncrementFromFarms, resourceIncrementPerFollower, resourceIncrementPerFollowerFromStatues, 
        timeForResourceProduction, timeForResourceProductionDecreaseFromStatues;
    private MeepleManager meepleManager;

    void Start()
    {
        meepleManager = gameObject.GetComponent<MeepleManager>();
        resourceText.text = resources.ToString();
        StartCoroutine(ResourceProduction());
    }

    public void AddResources(float newResources)
    {
        resources += newResources;
        resourceText.text = resources.ToString();
    }
    public bool TrySpendResources(float spendResources)
    {
        if (resources - spendResources >= 0)
        {
            resources -= spendResources;
            resourceText.text = resources.ToString();
            return true;
        }
        return false;
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

    public IEnumerator ResourceProduction()
    {
        float start = Time.time;
        while (true)
        {
            if (Time.time > start + timeForResourceProduction - timeForResourceProductionDecreaseFromStatues)
            {
                float newResources = (resourceIncrementPerFollower + resourceIncrementPerFollowerFromStatues) * (float)meepleManager.GetPopulationFollowers() + resourceIncrementFromFarms;
                AddResources(newResources);
                start = Time.time;
            }
            yield return new WaitForSeconds(1);
        }
    }

}
