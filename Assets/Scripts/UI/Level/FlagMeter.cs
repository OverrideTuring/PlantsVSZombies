using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagMeter : MonoBehaviour
{
    [SerializeField] private GameObject flagMeterBody;
    [SerializeField] private GameObject barMask;
    [SerializeField] private GameObject levelNameText;
    [SerializeField] private List<GameObject> flags;
    [SerializeField] private GameObject zombieHeadIcon;
    private List<int> waveZombieCountList;
    private float orignialPositionX;
    private float endPositionX;
    private float finalEndPositionX;
    private float flagEndPositionY = 17f;
    private int currentWave = 0;
    private int totalZombieCount = 0;
    private int deadZombieCount = 0;
    private bool firstZombieSpawned = false;
    private bool waveEnd = false;

    private void Start()
    {
        levelNameText.GetComponent<TextMeshProUGUI>().text = "Level " + LevelManager.Instance.LevelData.levelName;
        flagMeterBody.SetActive(false);
        levelNameText.SetActive(false);
        
        orignialPositionX = zombieHeadIcon.GetComponent<RectTransform>().anchoredPosition.x;
        endPositionX = flags[0].GetComponent<RectTransform>().anchoredPosition.x;
        finalEndPositionX = flags.Last().GetComponent<RectTransform>().anchoredPosition.x; 
        
        GameEventManager.Instance.OnLevelStart += OnLevelStart;
        GameEventManager.Instance.OnWaveEnd += OnWaveEnd;
        GameEventManager.Instance.OnZombieSpawned += OnZombieSpawned;
        GameEventManager.Instance.OnZombieDie += OnZombieDie;
    }

    public void OnLevelStart()
    {
        levelNameText.SetActive(true);
        waveZombieCountList = ZombieManager.Instance.getWaveZombieCountList();
        totalZombieCount = waveZombieCountList[0];
    }

    public void OnWaveEnd()
    {
        waveEnd = true;
    }

    public void OnWaveBegin()
    {
        flags[currentWave].GetComponent<RectTransform>().DOAnchorPosY(flagEndPositionY, 0.5f);
        zombieHeadIcon.GetComponent<RectTransform>().DOAnchorPosX(endPositionX, 1.0f).OnUpdate(UpdateProgress); ;
        currentWave++;
        if (currentWave == flags.Count) return;
        endPositionX = flags[currentWave].GetComponent<RectTransform>().anchoredPosition.x;
        totalZombieCount += waveZombieCountList[currentWave];
    }

    public void OnZombieSpawned(Zombie zombie)
    {
        if (!firstZombieSpawned)
        {
            firstZombieSpawned = true;
            Vector2 newPosition = levelNameText.GetComponent<RectTransform>().anchoredPosition;
            newPosition.x = 390;
            levelNameText.GetComponent<RectTransform>().anchoredPosition = newPosition;
            flagMeterBody.SetActive(true);
        }else if (waveEnd)
        {
            waveEnd = false;
            OnWaveBegin();
        }
    }

    public void OnZombieDie(Zombie zombie)
    {
        if (currentWave + 1 == waveZombieCountList.Count) return;
        deadZombieCount++;
        if (deadZombieCount == totalZombieCount) return;
        float ratio = (float)deadZombieCount / totalZombieCount;
        float targetPositionX = (endPositionX - orignialPositionX) * ratio + orignialPositionX;
        zombieHeadIcon.GetComponent<RectTransform>().DOAnchorPosX(targetPositionX, 1.0f).OnUpdate(UpdateProgress);
    }

    private void UpdateProgress()
    {
        float currentPositionX = zombieHeadIcon.GetComponent<RectTransform>().anchoredPosition.x;
        barMask.GetComponent<Image>().fillAmount = (finalEndPositionX - currentPositionX) / (finalEndPositionX - orignialPositionX);
    }
}
