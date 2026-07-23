using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [Header("Followers")]
    [SerializeField] int followerNumPurchased;
    [SerializeField] float followerCostBase, followerCostMultiplier;

    [Header("Priests")]
    [SerializeField] int priestNum;
    [SerializeField] float priestCostBase, priestCostMultiplier, followerIntervalDecreasePerPriestBase, followerIntervalDecreasePerPriestMultiplier;

    [Header("Farms")]
    [SerializeField] int farmNum;
    [SerializeField] float farmCostBase, farmCostMultiplier, resourceIncreasePerFarmBase, resourceIncreasePerFarmMultiplier;

    [Header("Churches")]
    [SerializeField] int churchNum;
    [SerializeField] float churchCostBase, churchCostMultiplier, followerIncreasePerChurchBase, followerIncreasePerChurchMultiplier;

    [Header("Statues")]
    [SerializeField] int statueNum;
    [SerializeField] float statueCostBase, statueCostMultiplier, followerIncreasePerStatueBase, followerIncreasePerStatueMultiplier,
        resourceIncrementPerFollowerBase, resourceIncrementPerFollowerMultiplier, 
        timeForResourceProductionDecreaseBase, timeForResourceProductionDecreaseMultiplier;

    //other scripts
    private MeepleManager meepleManager;
    private ResourceManager resourceManager;

    void Start()
    {
        meepleManager = gameObject.GetComponent<MeepleManager>();
        resourceManager = gameObject.GetComponent<ResourceManager>();
    }

    public float GetPurchaseCost(int numPurchased, float costBase, float costMultiplier)
    {
        return (costBase * Mathf.Pow(costMultiplier, numPurchased));
    }

    public bool TryPurchaseFollower()
    {
        if(resourceManager.TrySpendResources(GetPurchaseCost(followerNumPurchased, followerCostBase, followerCostMultiplier)))
        {
            meepleManager.AddFollowers(false);
            followerNumPurchased++;
            return true;
        }
        return false;
    }
    public bool TryPurchasePriest()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(priestNum, priestCostBase, priestCostMultiplier)))
        {
            float timeForFollowerConversionDecrease = followerIntervalDecreasePerPriestBase * Mathf.Pow(followerIntervalDecreasePerPriestMultiplier, priestNum);
            resourceManager.AddResourceIncrementFromFarms(timeForFollowerConversionDecrease);
            priestNum++;
            return true;
        }
        return false;
    }
    public bool TryPurchaseFarm()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(farmNum, farmCostBase, farmCostMultiplier)))
        {
            float resourceIncrement = resourceIncreasePerFarmBase * Mathf.Pow(resourceIncreasePerFarmMultiplier, farmNum);
            resourceManager.AddResourceIncrementFromFarms(resourceIncrement);
            farmNum++;
            return true;
        }
        return false;
    }
    public bool TryPurchaseChurch()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(churchNum, churchCostBase, churchCostMultiplier)))
        {
            int followerIncrease = (int)(followerIncreasePerChurchBase * Mathf.Pow(followerIncreasePerChurchMultiplier, churchNum));
            meepleManager.AddFollowers(false, followerIncrease);
            churchNum++;
            return true;
        }
        return false;
    }
    public bool TryPurchaseStatue()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(statueNum, statueCostBase, statueCostMultiplier)))
        {
            int followerIncrease = (int)(followerIncreasePerStatueBase * Mathf.Pow(followerIncreasePerStatueMultiplier, statueNum));
            meepleManager.AddFollowers(false, followerIncrease);
            float resourceIncrement = resourceIncrementPerFollowerBase * Mathf.Pow(resourceIncrementPerFollowerMultiplier, statueNum);
            resourceManager.AddResourceIncrementPerFollowerFromStatues(resourceIncrement);
            float timeForResourceProductionDecrease = timeForResourceProductionDecreaseBase * Mathf.Pow(timeForResourceProductionDecreaseMultiplier, statueNum);
            resourceManager.AddResourceIncrementPerFollowerFromStatues(resourceIncrement);

            statueNum++;
            return true;
        }
        return false;
    }
}