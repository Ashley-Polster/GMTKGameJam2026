using UnityEngine;

public class RuinsTracker : MonoBehaviour
{
    public static RuinsTracker instance;
    [SerializeField] int farmRuins, churchRuins, statueRuins;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            farmRuins = 0;
            churchRuins = 0;
            statueRuins = 0;
        }
        else
            Destroy(gameObject);
    }

    public int GetFarmRuinsNum()
    {
        return farmRuins;
    }
    public int GetChurchRuinsNum()
    {
        return churchRuins;
    }
    public int GetStatueRuinsNum()
    {
        return statueRuins;
    }

    public void RecordRuins(int farmNum, int churchNum, int statueNum)
    {
        farmRuins = farmNum;
        churchRuins = churchNum;
        statueRuins = statueNum;
    }
}
