using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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
    [SerializeField] GameObject farmPrefab;
    [SerializeField] int farmNum;
    [SerializeField] float farmCostBase, farmCostMultiplier, resourceIncreasePerFarmBase, resourceIncreasePerFarmMultiplier;

    [Header("Churches")]
    [SerializeField] TMP_Text churchPriceTMP;
    [SerializeField] GameObject churchPrefab;
    [SerializeField] int churchNum;
    [SerializeField] float churchCostBase, churchCostMultiplier, followerIncreasePerChurchBase, followerIncreasePerChurchMultiplier;

    [Header("Statues")]
    [SerializeField] TMP_Text statuePriceTMP;
    [SerializeField] GameObject statuePrefab;
    [SerializeField] int statueNum;
    [SerializeField] float statueCostBase, statueCostMultiplier, followerIncreasePerStatueBase, followerIncreasePerStatueMultiplier,
        resourceIncrementPerFollowerBase, resourceIncrementPerFollowerMultiplier, 
        timeForResourceProductionDecreaseBase, timeForResourceProductionDecreaseMultiplier;

    [Header("Building Bounds")]
    [SerializeField] Transform topLeftBound;
    [SerializeField] Transform bottomRightBound;

    //other scripts
    private MeepleManager meepleManager;
    private ResourceManager resourceManager;
    private PointManager pointManager;
    private SoundManager soundManager;

    void Start()
    {
        meepleManager = GetComponent<MeepleManager>();
        resourceManager = GetComponent<ResourceManager>();
        pointManager = GetComponent<PointManager>();
        soundManager = GetComponent<SoundManager>();
        UpdatePrice(followerPriceTMP, followerNumPurchased, followerCostBase, followerCostMultiplier);
        UpdatePrice(priestPriceTMP, priestNum, priestCostBase, priestCostMultiplier);
        UpdatePrice(farmPriceTMP, farmNum, farmCostBase, farmCostMultiplier);
        UpdatePrice(churchPriceTMP, churchNum, churchCostBase, churchCostMultiplier);
        UpdatePrice(statuePriceTMP, statueNum, statueCostBase, statueCostMultiplier);
    }

    public int GetStatueNum()
    {
        return statueNum;
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
            soundManager.playSound();
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
            soundManager.playSound();
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
            soundManager.playSound();
            PlaceBuilding(farmPrefab);
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
            soundManager.playSound();
            PlaceBuilding(churchPrefab);
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
            pointManager.UpdatePointInfo();
            soundManager.playSound();
            PlaceBuilding(statuePrefab);
        }
    }

    public void PlaceBuilding(GameObject buildingPrefab)
    {
        float x, y, yTop, yBottom, layerPercent, layerNumber;
        yTop = topLeftBound.position.y;
        yBottom = bottomRightBound.position.y;
        x = UnityEngine.Random.Range(topLeftBound.position.x, bottomRightBound.position.x);
        y = UnityEngine.Random.Range(yBottom, yTop);
        layerPercent = Mathf.InverseLerp(yBottom, yTop, y);
        layerNumber = Mathf.Lerp(short.MaxValue, short.MinValue, layerPercent);
        GameObject newBuilding = Instantiate(buildingPrefab, new Vector2(x, y), Quaternion.identity);
        newBuilding.GetComponent<SpriteRenderer>().sortingOrder = (int)layerNumber;
    }
}