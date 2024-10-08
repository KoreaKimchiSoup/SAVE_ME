using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

//Json 저장을 위한 StateData
[SerializeField]
public class StateData
{
    //현재 HP
    public int currentHp = 100;
    //현재 Energy
    public int currentEnergy = 100;
    //현재 Water
    public int currentWater = 100;
    //현재 Exhasution
    //피로도는 찰수록 패널티 받는 시스템
    public int currentExhastion = 0;
}

public class StateManager : MonoBehaviour
{
    //Hp슬라이더
    [SerializeField] private Slider HpSlider;
    //Energy슬라이더
    [SerializeField] private Slider EnergySlider;
    //Water슬라이더
    [SerializeField] private Slider WaterSlider;
    //Exhastion슬라이더
    [SerializeField] private Slider ExhastionSlider;

    //Hp이미지
    [SerializeField] private Image HpImage;
    //Energy이미지
    [SerializeField] private Image EnergyImage;
    //Water이미지
    [SerializeField] private Image WaterImage;
    //Exhastion이미지
    [SerializeField] private Image ExhastionImage;

    //Hp Text
    [SerializeField] private Text HpText;
    //Energy Text
    [SerializeField] private Text EnergyText;
    //Water Text
    [SerializeField] private Text WaterText;
    //Exhastion Text
    [SerializeField] private Text ExhastionText;
    //플레이어 상태 JsonData를 받기 위한 변수
    StateData stateData = new StateData();
    //현재 시간에 따른 상태변경을 위한 변수
    TimeManager timeManager;
    //Ui매니저를 받아오는 변수
    UIManager uIManager;
    // FadeCamera 참조
    public FadeCamera fadeCamera;
    //UI코루틴 반복실행을 막기위한 변수
    bool isWarningUI = true;

    // 이전에 기록된 시간
    private float lastCheckedHour = 0f;
    private void Awake()
    {
        //플레이어의 상태가 저장된 Json 파일 로드해오기
        //TimeManager 가져오기
        timeManager = GetComponent<TimeManager>();
        //현재 플레이어 상태를 슬라이더에 설정
        UpdateSlider();
        //현재 플레이어 상태를 텍스트로
        SetText();
        //UIManager가져오기
        uIManager = GameObject.Find("Watch_UI").GetComponent<UIManager>();

    }

    private void Update()
    {
        UpdateSlider();
        CheckTimeAndUpdateWater();
        SetText();
    }
    //게임 시작 시 Json에서 받아온 데이터로 슬라이더 value 설정
    void SetText()
    {
        HpText.text = stateData.currentHp.ToString("0") + "%";
        WaterText.text = stateData.currentWater.ToString("0") + "%";
        EnergyText.text = stateData.currentEnergy.ToString("0") + "%";
        ExhastionText.text = stateData.currentExhastion.ToString("0") + "%";

    }
    //시간이 지났는지 체크하기위한 함수
    void CheckTimeAndUpdateWater()
    {
        // TimeManager의 현재 시간을 확인
        float currentHour = (float)timeManager.GetCurrentTime().TimeOfDay.TotalHours;

        // 1시간이 경과했는지 확인
        if (currentHour >= lastCheckedHour + 1f || (currentHour < lastCheckedHour && currentHour + 24 >= lastCheckedHour + 1f))
        {
            Debug.Log("1시간 경과");
            MinusPerHour(); // 1시간이 지나면 호출
            UPdateHp();
            lastCheckedHour = currentHour; // 마지막 체크 시간 업데이트
        }
    }
    // 게임 시간마다 수분과 에너지가  줄어드는 함수
    void MinusPerHour()
    {
        //시간마다 7씩 수분 감소
        if (stateData.currentWater > 0)
        {
            stateData.currentWater -= 7;
            if (stateData.currentWater <= 0)
            {
                //0밑으로 수분이 감소하지 않게
                stateData.currentWater = 0;

            }
        }
        //시간마다 5씩 에너지 감소
        if (stateData.currentEnergy > 0)
        {
            stateData.currentEnergy -= 5;
            //0밑으로 에너지가 감소하지 않게
            if (stateData.currentEnergy <= 0)
            {
                stateData.currentEnergy = 0;
            }
        }

        //시간 당 2씩 피로도 증가
        if (stateData.currentExhastion < 100)
        {
            stateData.currentExhastion += 2;
            //100이상으로 피로도가 증가하지않게
            if (stateData.currentExhastion >= 100)
            {
                stateData.currentExhastion = 100;
            }
        }

    }
    // 게임 1시간 당 10씩 조정.
    void UPdateHp()
    {
        //수분과 에너지 중 하나가 30미만일 시 Hp 1시간 당 10씩 감소 
        if (stateData.currentEnergy < 30 || stateData.currentWater < 30)
        {
            //10분당 5씩감소
            if (stateData.currentHp > 0)
            {
                stateData.currentHp -= 10;
            }
            //체력이 0이하로 떨어지지 않게.
            else
            {
                stateData.currentHp = 0;
            }
        }
        //수분과 에너지가 둘다 80이상일 때 Hp 1시간당 5씩 회복
        else if (stateData.currentWater >= 80 && stateData.currentHp >= 80)
        {

            //현재 체력이 최대체력을 넘지않도록.
            if (stateData.currentHp <= 100)
            {
                stateData.currentHp = 100;
            }
            //1시간 당 5증가
            else
            {
                stateData.currentHp += 5;
            }
        }

    }
    //슬라이더 변동시 값 변경해주는 함수
    void UpdateSlider()
    {
        //최대체력 이상으로 회복하지 못하도록
        if (stateData.currentHp >= 100)
        {
            stateData.currentHp = 100;
        }
        //최대 에너지 이상으로 회복하지 못하도록
        if (stateData.currentEnergy >= 100)
        {
            stateData.currentEnergy = 100;
        }
        //최대 수분 이상으로 회복하지 못하도록 
        if (stateData.currentWater >= 100)
        {
            stateData.currentWater = 100;
        }
        //피로도가 0 이하로 내려가지않게
        if (stateData.currentExhastion <= 0)
        {
            stateData.currentExhastion = 0;
        }
        //Hp슬라이더 업데이트
        HpSlider.value = Mathf.Lerp(HpSlider.value, (float)stateData.currentHp / 100,
            Time.deltaTime * 7);
        //Water 슬라이더 업데이트 
        WaterSlider.value = Mathf.Lerp(WaterSlider.value, (float)stateData.currentWater / 100,
            Time.deltaTime * 7);
        //Energy슬라이더 업데이트
        EnergySlider.value = Mathf.Lerp(EnergySlider.value, (float)stateData.currentEnergy / 100,
           Time.deltaTime * 7);
        //Exhastion 슬라이더 업데이트
        ExhastionSlider.value = Mathf.Lerp(ExhastionSlider.value, (float)stateData.currentExhastion / 100,
           Time.deltaTime * 7);

        //현재 체력이 0이하 일때 
        if (stateData.currentHp <= 0)
        {
            //체력이 0밑으로 내려가지 않게
            stateData.currentHp = 0;
            //플레이어 사망
            Debug.Log("Player Die");
            uIManager.DieImage();
        }
        //현재 피로도가 100일때 기절 
        else if (stateData.currentExhastion >= 100)
        {
            //피로도가 100이상으로 올라가지 않게.
            stateData.currentExhastion = 100;
            //플레이어 기절
            Debug.Log("Player swoon");
        }
        //Hp 30미만 일때
        else if (stateData.currentHp < 50 && isWarningUI == true)
        {
            //경고 UI
            StartCoroutine(UIWarning());
        }
        //Energy 30미만일때
        else if (stateData.currentEnergy < 30 && isWarningUI == true)
        {
            //경고 UI
            StartCoroutine(UIWarning());
        }
        //Water 30미만일때
        else if (stateData.currentWater < 30 && isWarningUI == true)
        {
            //경고UI
            StartCoroutine(UIWarning());
        }


    }
    //Hp와 Energy, water가 30%미만일때 경고 표시를 위한 함수
    IEnumerator UIWarning()
    {
        isWarningUI = false;
        //Hp가 50미만일 때
        if (stateData.currentHp < 50)
        {

            HpImage.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            HpImage.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        //Energy가 30미만일때 
        if (stateData.currentEnergy < 30)
        {
            EnergyImage.color = Color.yellow;
            yield return new WaitForSeconds(0.2F);
            EnergyImage.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        //Water가 30 미만일때 
        if (stateData.currentWater < 30)
        {
            WaterImage.color = Color.blue;
            yield return new WaitForSeconds(0.2f);
            WaterImage.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        isWarningUI = true;

    }
    public void Justfish()
    {
        //  생선 살 , Energy 5회복.
        stateData.currentEnergy += 5;
    }
    public void Friedfish()
    {
        //구운 생선 살 , Energy 10회복
        stateData.currentEnergy += 10;
    }
    public void FriedChicken()
    {
        //구운  닭 고기, Energy 35회복\
        stateData.currentEnergy += 35;
    }
    public void FriedDeermeat()
    {
        //구운 사슴 고기 , Energy 45회복
        stateData.currentEnergy += 45;
    }
    public void FriedBoarmeat()
    {
        //구운 멧돼지 고기 ,Energy 50회복
        stateData.currentEnergy += 50;
    }
    public void FriedSnake()
    {
        //구운 뱀 고기 , Energy 30회복
        stateData.currentEnergy += 30;
    }
    public void FriedElegatormeat()
    {
        //구운 악어고기, Enenrgy 50회복
        stateData.currentEnergy += 50;
    }
    public void Octopus()
    {
        //문어 , 섭취시 Energy 30 회복
        stateData.currentEnergy += 30;
    }
    public void friedEgg()
    {
        //구운 계란 ,섭취 시 Energy  15 회복
        stateData.currentEnergy += 15;
    }
    public void friedCrab()
    {
        //구운 꽃게 , 섭취 시  Energy 10 회복
        stateData.currentEnergy += 10;
    }
    public void Coconut()
    {
        //코코넛 , 섭취 시 수분 30 회복
        stateData.currentWater += 30;
    }
    public void Papaya()
    {
        //파파야 , 섭취시 수분 20 회복
        stateData.currentWater += 20;
    }
    public void rainWater()
    {
        //빗물 , 섭취시 수분 20 회복 
        stateData.currentWater += 20;
    }

    //UI에서 '자기' 버튼 누르면 실행되기
    public void Sleep()
    {
        //카메라가 보이지 않게 만들기
        fadeCamera.Sleep();
        //시간을 5시간 뒤로
        FindAnyObjectByType<TimeManager>().AddFiveHours();
        //피로 회복 완료
        stateData.currentExhastion -= 100;
    }
}
