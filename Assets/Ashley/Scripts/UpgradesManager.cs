using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradesManager : MonoBehaviour
{
    [Header("Followers")]
    [SerializeField] TMP_Text followerPriceTMP;
    [SerializeField] int followerNumPurchased;
    [SerializeField] float followerCostBase, followerCostMultiplier;

    [Header("Priests")]
    [SerializeField] TMP_Text priestPriceTMP;
    [SerializeField] int priestNum;
    [SerializeField] float priestCostBase, priestCostMultiplier, followerIntervalDecreasePerPriestBase, followerIntervalDecreasePerPriestMultiplier;

    [Header("Farms")]
    [SerializeField] TMP_Text farmPriceTMP;
    [SerializeField] int farmNum;
    [SerializeField] float farmCostBase, farmCostMultiplier, resourceIncreasePerFarmBase, resourceIncreasePerFarmMultiplier;

    [Header("Churches")]
    [SerializeField] TMP_Text churchPriceTMP;
    [SerializeField] int churchNum;
    [SerializeField] float churchCostBase, churchCostMultiplier, followerIncreasePerChurchBase, followerIncreasePerChurchMultiplier;

    [Header("Statues")]
    [SerializeField] TMP_Text statuePriceTMP;
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
        UpdatePrice(followerPriceTMP, followerNumPurchased, followerCostBase, followerCostMultiplier);
        UpdatePrice(priestPriceTMP, priestNum, priestCostBase, priestCostMultiplier);
        UpdatePrice(farmPriceTMP, farmNum, farmCostBase, farmCostMultiplier);
        UpdatePrice(churchPriceTMP, churchNum, churchCostBase, churchCostMultiplier);
        UpdatePrice(statuePriceTMP, statueNum, statueCostBase, statueCostMultiplier);
    }

    public int GetPurchaseCost(int numPurchased, float costBase, float costMultiplier)
    {
        return (int)(costBase * Mathf.Pow(costMultiplier, numPurchased));
    }
    public void UpdatePrice(TMP_Text price, int numPurchased, float costBase, float costMultiplier)
    {
        price.text = GetPurchaseCost(numPurchased, costBase, costMultiplier).ToString();
    }

    public void TryPurchaseFollower()
    {
        if(resourceManager.TrySpendResources(GetPurchaseCost(followerNumPurchased, followerCostBase, followerCostMultiplier)))
        {
            meepleManager.AddFollowers(false);
            followerNumPurchased++;
            UpdatePrice(followerPriceTMP, followerNumPurchased, followerCostBase, followerCostMultiplier);
        }
    }
    public void TryPurchasePriest()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(priestNum, priestCostBase, priestCostMultiplier)))
        {
            float timeForFollowerConversionDecrease = followerIntervalDecreasePerPriestBase * Mathf.Pow(followerIntervalDecreasePerPriestMultiplier, priestNum);
            meepleManager.AddTimeForFollowerConversionDecreaseFromPriests(timeForFollowerConversionDecrease);
            priestNum++;
            UpdatePrice(priestPriceTMP, priestNum, priestCostBase, priestCostMultiplier);
        }
    }
    public void TryPurchaseFarm()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(farmNum, farmCostBase, farmCostMultiplier)))
        {
            float resourceIncrement = resourceIncreasePerFarmBase * Mathf.Pow(resourceIncreasePerFarmMultiplier, farmNum);
            resourceManager.AddResourceIncrementFromFarms(resourceIncrement);
            farmNum++;
            UpdatePrice(farmPriceTMP, farmNum, farmCostBase, farmCostMultiplier);
        }
    }
    public void TryPurchaseChurch()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(churchNum, churchCostBase, churchCostMultiplier)))
        {
            int followerIncrease = (int)(followerIncreasePerChurchBase * Mathf.Pow(followerIncreasePerChurchMultiplier, churchNum));
            meepleManager.AddFollowers(false, followerIncrease);
            churchNum++;
            UpdatePrice(churchPriceTMP, churchNum, churchCostBase, churchCostMultiplier);
        }
    }
    public void TryPurchaseStatue()
    {
        if (resourceManager.TrySpendResources(GetPurchaseCost(statueNum, statueCostBase, statueCostMultiplier)))
        {
            int followerIncrease = (int)(followerIncreasePerStatueBase * Mathf.Pow(followerIncreasePerStatueMultiplier, statueNum));
            meepleManager.AddFollowers(false, followerIncrease);
            float resourceIncrement = resourceIncrementPerFollowerBase * Mathf.Pow(resourceIncrementPerFollowerMultiplier, statueNum);
            resourceManager.AddResourceIncrementPerFollowerFromStatues(resourceIncrement);
            float timeForResourceProductionDecrease = timeForResourceProductionDecreaseBase * Mathf.Pow(timeForResourceProductionDecreaseMultiplier, statueNum);
            resourceManager.AddTimeForResourceProductionDecreaseFromStatues(resourceIncrement);

            statueNum++;
            UpdatePrice(statuePriceTMP, statueNum, statueCostBase, statueCostMultiplier);
        }
    }
}