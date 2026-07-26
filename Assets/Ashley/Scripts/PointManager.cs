using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] TMP_Text pointsText, followerText, statueText, percentageText, calcText;
    [SerializeField] int pointsPerFollower, multiplierPerStatue;
    private int points, pointsFromFollowers;
    private float multiplierFromStatues, multiplierFromPercentage;
    private string followerStringBase, statueStringBase, percentageStringBase, calcStringBase;
    private MeepleManager meepleMangager;
    private UpgradesManager upgradesMangager;

    void Start()
    {
        meepleMangager = gameObject.GetComponent<MeepleManager>();
        upgradesMangager = gameObject.GetComponent<UpgradesManager>();

        string followerString = followerText.text;
        followerString = followerString.Replace("[pointsPerFollower]", pointsPerFollower.ToString());
        followerStringBase = followerString;
        followerText.text = followerString;

        string statueString = statueText.text;
        statueString = statueString.Replace("[multiplierPerStatue]", multiplierPerStatue.ToString());
        statueStringBase = statueString;
        statueText.text = statueString;

        percentageStringBase = percentageText.text;
        calcStringBase = calcText.text;
        UpdatePointInfo();
    }

    public int GetPoints()
    {
        return points;
    }

    public void UpdatePoints()
    {
        int followerNum = meepleMangager.GetPopulationFollowers();
        pointsFromFollowers = pointsPerFollower * followerNum;

        int statueNum = upgradesMangager.GetStatueNum();
        multiplierFromStatues = multiplierPerStatue * statueNum;
        if (multiplierFromStatues < 1)
            multiplierFromStatues = 1;

        int populationNum = meepleMangager.GetPopulation();
        multiplierFromPercentage = (float)followerNum / (float)populationNum + 1f;

        points = (int)(pointsFromFollowers * multiplierFromStatues * multiplierFromPercentage);
        pointsText.text = points.ToString();
    }
    public void UpdatePointInfo()
    {
        int followerNum = meepleMangager.GetPopulationFollowers();
        pointsFromFollowers = pointsPerFollower * followerNum;
        string followerString = followerStringBase;
        followerString = followerString.Replace("[followerNum]", followerNum.ToString());
        followerString = followerString.Replace("[pointsFromFollowers]", pointsFromFollowers.ToString());
        followerText.text = followerString;

        int statueNum = upgradesMangager.GetStatueNum();
        multiplierFromStatues = multiplierPerStatue * statueNum;
        if(multiplierFromStatues < 1)
            multiplierFromStatues = 1;
        string statueString = statueStringBase;
        statueString = statueString.Replace("[statueNum]", statueNum.ToString());
        statueString = statueString.Replace("[multiplierFromStatues]", multiplierFromStatues.ToString());
        statueText.text = statueString;

        int populationNum = meepleMangager.GetPopulation();
        multiplierFromPercentage = (float)followerNum / (float)populationNum + 1f;
        string percentageString = percentageStringBase;
        percentageString = percentageString.Replace("[followerNum]", followerNum.ToString());
        percentageString = percentageString.Replace("[populationNum]", populationNum.ToString());
        percentageString = percentageString.Replace("[multiplierFromPercentage]", multiplierFromPercentage.ToString());
        percentageText.text = percentageString;

        points = (int)(pointsFromFollowers * multiplierFromStatues * multiplierFromPercentage);
        string calcString = calcStringBase;
        calcString = calcString.Replace("[pointsFromFollowers]", pointsFromFollowers.ToString());
        calcString = calcString.Replace("[multiplierFromStatues]", multiplierFromStatues.ToString());
        calcString = calcString.Replace("[multiplierFromPercentage]", multiplierFromPercentage.ToString());
        calcString = calcString.Replace("[totalPoints]", points.ToString());
        calcText.text = calcString;
        pointsText.text = points.ToString();
    }
}
