using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCanvas : MonoBehaviour
{
    public static BaseCanvas instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        EXPChange(0);

        MANAGER.Session.onHPChanged += HPChanged;
        MANAGER.Session.onExpChanged += EXPChange;
        MANAGER.Session.onMonsterCountChanged += MonsterCountText;
        MANAGER.Session.onSelectedCard += SetSkillFrame;
        MANAGER.Session.onBossTime += AppearBossText;

        SelectCard(true);
    }

    private void OnDestroy()
    {
        MANAGER.Session.onHPChanged -= HPChanged;
        MANAGER.Session.onExpChanged -= EXPChange;
        MANAGER.Session.onMonsterCountChanged -= MonsterCountText;
        MANAGER.Session.onSelectedCard -= SetSkillFrame;
        MANAGER.Session.onBossTime -= AppearBossText;

    }

    public Transform HOLDERLAYER;

    public Image ExpFill;
    public CardSelector CardObject;
    public Treasure Treasure;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI monsterCountText;
    public TextMeshProUGUI timerText;

    Coroutine HP_Coroutine;

    public Image HPFill;
    public Image HPFillSec;
    public TextMeshProUGUI HPText;

    public SkillFrame frame;
    public Transform activeFrameContent;
    public Transform passiveFrameContent;
    List<GameObject> SkillFrameGorvage = new List<GameObject>();

    public GameObject BossAnimText;
    private void Update()
    {
        timerText.text = UtilsUI.FormatTime(MANAGER.Session.gameTime);
    }

    private void AppearBossText()
    {
        BossAnimText.SetActive(true);
        Invoke(nameof(BossTextDisabled), 1.2f);
    }

    private void BossTextDisabled() => BossAnimText.SetActive(false);
    private void SetSkillFrame()
    {

        if(SkillFrameGorvage.Count > 0)
        {
            for(int i = 0; i <  SkillFrameGorvage.Count; i++)
            {
                Destroy(SkillFrameGorvage[i]);
            }
            SkillFrameGorvage.Clear();
        }

        foreach(var data in MANAGER.Session.selectedCard)
        {
            var go = Instantiate(frame,
                data.Value.db.state == CardState.Active ? 
                activeFrameContent : 
                passiveFrameContent);

            go.Initalize(data.Value);
            SkillFrameGorvage.Add(go.gameObject);
        }
        
    }

    public void SelectCard(bool AllActive = false)
    {
        Time.timeScale = 0;
        CardObject.Initalize(AllActive);
    }

    public void SelectTreasure(int value)
    {
        Time.timeScale = 0;
        Treasure.Initlize(value);
    }

    private void MonsterCountText(int value) => monsterCountText.text = value.ToString();
    public void EXPChange(float exp)
    {
        float expPecentage = exp / MANAGER.Session.GetRequiredExp();
        ExpFill.fillAmount = expPecentage;
        LevelText.text =
            string.Format(
                "Lv.{0} <color=#FFFFFF>({1:0.0}%)</color>",
                (MANAGER.Session.playerLevel + 1),
                expPecentage * 100.0f);
    }

    public void HPChanged(float hp)
    {
        float hpPercentage = hp / MANAGER.Session.MaxHP;
        HPText.text = string.Format("{0:0}/{1:0}", hp, MANAGER.Session.MaxHP);
        HPFill.fillAmount = hpPercentage;

        // 실행중이라면 코루틴종료(hp바UI 감소연출)
        if(HP_Coroutine != null)
        {
            StopCoroutine(HP_Coroutine);
        }
        HP_Coroutine = StartCoroutine(SecondFillAmount(HPFill.fillAmount));
    }

    IEnumerator SecondFillAmount(float percentage)
    {
        float speed = 2.0f;
        float t = 0.0f;
        while(HPFillSec.fillAmount > percentage)
        {
            t += Time.deltaTime * speed;
            HPFillSec.fillAmount = Mathf.Lerp(
                HPFillSec.fillAmount,
                percentage,
                t);

            yield return null;
        }

        // 목표값으로 초기화
        HPFillSec.fillAmount = percentage;
    }
}
