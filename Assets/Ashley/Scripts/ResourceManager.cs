using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] TMP_Text resourceText;
    [SerializeField] int resources, resourceIncrementPerFollower, resourceIncrementFromFarms;
    [SerializeField] float timeForResourceProduction;
    private MeepleManager meepleManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meepleManager = gameObject.GetComponent<MeepleManager>();
        resourceText.text = resources.ToString();
        StartCoroutine(ResourceProduction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddResources(int newResources)
    {
        resources += newResources;
        resourceText.text = resources.ToString();
    }
    public bool TrySpendResources(int spendResources)
    {
        if (resources - spendResources >= 0)
        {
            resources -= spendResources;
            resourceText.text = resources.ToString();
            return true;
        }
        return false;
    }

    public IEnumerator ResourceProduction()
    {
        float start = Time.time;
        while (true)
        {
            if (Time.time > start + timeForResourceProduction)
            {
                int newResources = resourceIncrementPerFollower * meepleManager.GetPopulationFollowers() + resourceIncrementFromFarms;
                AddResources(newResources);
                start = Time.time;
            }
            yield return new WaitForSeconds(1);
        }
    }

}
