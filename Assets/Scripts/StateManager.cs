using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

//Json ������ ���� StateData
[SerializeField]
public class StateData
{
    //���� HP
    public int currentHp = 100;
    //���� Energy
    public int currentEnergy = 100;
    //���� Water
    public int currentWater = 100;
    //���� Exhasution
    //�Ƿε��� ������ �г�Ƽ �޴� �ý���
    public int currentExhastion = 0;
}

public class StateManager : MonoBehaviour
{
    //Hp�����̴�
    [SerializeField] private Slider HpSlider;
    //Energy�����̴�
    [SerializeField] private Slider EnergySlider;
    //Water�����̴�
    [SerializeField] private Slider WaterSlider;
    //Exhastion�����̴�
    [SerializeField] private Slider ExhastionSlider;

    //Hp�̹���
    [SerializeField] private Image HpImage;
    //Energy�̹���
    [SerializeField] private Image EnergyImage;
    //Water�̹���
    [SerializeField] private Image WaterImage;
    //Exhastion�̹���
    [SerializeField] private Image ExhastionImage;

    //Hp Text
    [SerializeField] private Text HpText;
    //Energy Text
    [SerializeField] private Text EnergyText;
    //Water Text
    [SerializeField] private Text WaterText;
    //Exhastion Text
    [SerializeField] private Text ExhastionText;
    //�÷��̾� ���� JsonData�� �ޱ� ���� ����
    StateData stateData = new StateData();
    //���� �ð��� ���� ���º����� ���� ����
    TimeManager timeManager;
    //Ui�Ŵ����� �޾ƿ��� ����
    UIManager uIManager;
    // FadeCamera ����
    public FadeCamera fadeCamera;
    //UI�ڷ�ƾ �ݺ������� �������� ����
    bool isWarningUI = true;

    // ������ ��ϵ� �ð�
    private float lastCheckedHour = 0f;
    private void Awake()
    {
        //�÷��̾��� ���°� ����� Json ���� �ε��ؿ���
        //TimeManager ��������
        timeManager = GetComponent<TimeManager>();
        //���� �÷��̾� ���¸� �����̴��� ����
        UpdateSlider();
        //���� �÷��̾� ���¸� �ؽ�Ʈ��
        SetText();
        //UIManager��������
        uIManager = GameObject.Find("Watch_UI").GetComponent<UIManager>();

    }

    private void Update()
    {
        UpdateSlider();
        CheckTimeAndUpdateWater();
        SetText();
    }
    //���� ���� �� Json���� �޾ƿ� �����ͷ� �����̴� value ����
    void SetText()
    {
        HpText.text = stateData.currentHp.ToString("0") + "%";
        WaterText.text = stateData.currentWater.ToString("0") + "%";
        EnergyText.text = stateData.currentEnergy.ToString("0") + "%";
        ExhastionText.text = stateData.currentExhastion.ToString("0") + "%";

    }
    //�ð��� �������� üũ�ϱ����� �Լ�
    void CheckTimeAndUpdateWater()
    {
        // TimeManager�� ���� �ð��� Ȯ��
        float currentHour = (float)timeManager.GetCurrentTime().TimeOfDay.TotalHours;

        // 1�ð��� ����ߴ��� Ȯ��
        if (currentHour >= lastCheckedHour + 1f || (currentHour < lastCheckedHour && currentHour + 24 >= lastCheckedHour + 1f))
        {
            Debug.Log("1�ð� ���");
            MinusPerHour(); // 1�ð��� ������ ȣ��
            UPdateHp();
            lastCheckedHour = currentHour; // ������ üũ �ð� ������Ʈ
        }
    }
    // ���� �ð����� ���а� ��������  �پ��� �Լ�
    void MinusPerHour()
    {
        //�ð����� 7�� ���� ����
        if (stateData.currentWater > 0)
        {
            stateData.currentWater -= 7;
            if (stateData.currentWater <= 0)
            {
                //0������ ������ �������� �ʰ�
                stateData.currentWater = 0;

            }
        }
        //�ð����� 5�� ������ ����
        if (stateData.currentEnergy > 0)
        {
            stateData.currentEnergy -= 5;
            //0������ �������� �������� �ʰ�
            if (stateData.currentEnergy <= 0)
            {
                stateData.currentEnergy = 0;
            }
        }

        //�ð� �� 2�� �Ƿε� ����
        if (stateData.currentExhastion < 100)
        {
            stateData.currentExhastion += 2;
            //100�̻����� �Ƿε��� ���������ʰ�
            if (stateData.currentExhastion >= 100)
            {
                stateData.currentExhastion = 100;
            }
        }

    }
    // ���� 1�ð� �� 10�� ����.
    void UPdateHp()
    {
        //���а� ������ �� �ϳ��� 30�̸��� �� Hp 1�ð� �� 10�� ���� 
        if (stateData.currentEnergy < 30 || stateData.currentWater < 30)
        {
            //10�д� 5������
            if (stateData.currentHp > 0)
            {
                stateData.currentHp -= 10;
            }
            //ü���� 0���Ϸ� �������� �ʰ�.
            else
            {
                stateData.currentHp = 0;
            }
        }
        //���а� �������� �Ѵ� 80�̻��� �� Hp 1�ð��� 5�� ȸ��
        else if (stateData.currentWater >= 80 && stateData.currentHp >= 80)
        {

            //���� ü���� �ִ�ü���� �����ʵ���.
            if (stateData.currentHp <= 100)
            {
                stateData.currentHp = 100;
            }
            //1�ð� �� 5����
            else
            {
                stateData.currentHp += 5;
            }
        }

    }
    //�����̴� ������ �� �������ִ� �Լ�
    void UpdateSlider()
    {
        //�ִ�ü�� �̻����� ȸ������ ���ϵ���
        if (stateData.currentHp >= 100)
        {
            stateData.currentHp = 100;
        }
        //�ִ� ������ �̻����� ȸ������ ���ϵ���
        if (stateData.currentEnergy >= 100)
        {
            stateData.currentEnergy = 100;
        }
        //�ִ� ���� �̻����� ȸ������ ���ϵ��� 
        if (stateData.currentWater >= 100)
        {
            stateData.currentWater = 100;
        }
        //�Ƿε��� 0 ���Ϸ� ���������ʰ�
        if (stateData.currentExhastion <= 0)
        {
            stateData.currentExhastion = 0;
        }
        //Hp�����̴� ������Ʈ
        HpSlider.value = Mathf.Lerp(HpSlider.value, (float)stateData.currentHp / 100,
            Time.deltaTime * 7);
        //Water �����̴� ������Ʈ 
        WaterSlider.value = Mathf.Lerp(WaterSlider.value, (float)stateData.currentWater / 100,
            Time.deltaTime * 7);
        //Energy�����̴� ������Ʈ
        EnergySlider.value = Mathf.Lerp(EnergySlider.value, (float)stateData.currentEnergy / 100,
           Time.deltaTime * 7);
        //Exhastion �����̴� ������Ʈ
        ExhastionSlider.value = Mathf.Lerp(ExhastionSlider.value, (float)stateData.currentExhastion / 100,
           Time.deltaTime * 7);

        //���� ü���� 0���� �϶� 
        if (stateData.currentHp <= 0)
        {
            //ü���� 0������ �������� �ʰ�
            stateData.currentHp = 0;
            //�÷��̾� ���
            Debug.Log("Player Die");
            uIManager.DieImage();
        }
        //���� �Ƿε��� 100�϶� ���� 
        else if (stateData.currentExhastion >= 100)
        {
            //�Ƿε��� 100�̻����� �ö��� �ʰ�.
            stateData.currentExhastion = 100;
            //�÷��̾� ����
            Debug.Log("Player swoon");
        }
        //Hp 30�̸� �϶�
        else if (stateData.currentHp < 50 && isWarningUI == true)
        {
            //��� UI
            StartCoroutine(UIWarning());
        }
        //Energy 30�̸��϶�
        else if (stateData.currentEnergy < 30 && isWarningUI == true)
        {
            //��� UI
            StartCoroutine(UIWarning());
        }
        //Water 30�̸��϶�
        else if (stateData.currentWater < 30 && isWarningUI == true)
        {
            //���UI
            StartCoroutine(UIWarning());
        }


    }
    //Hp�� Energy, water�� 30%�̸��϶� ��� ǥ�ø� ���� �Լ�
    IEnumerator UIWarning()
    {
        isWarningUI = false;
        //Hp�� 50�̸��� ��
        if (stateData.currentHp < 50)
        {

            HpImage.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            HpImage.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        //Energy�� 30�̸��϶� 
        if (stateData.currentEnergy < 30)
        {
            EnergyImage.color = Color.yellow;
            yield return new WaitForSeconds(0.2F);
            EnergyImage.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        //Water�� 30 �̸��϶� 
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
        //  ���� �� , Energy 5ȸ��.
        stateData.currentEnergy += 5;
    }
    public void Friedfish()
    {
        //���� ���� �� , Energy 10ȸ��
        stateData.currentEnergy += 10;
    }
    public void FriedChicken()
    {
        //����  �� ���, Energy 35ȸ��\
        stateData.currentEnergy += 35;
    }
    public void FriedDeermeat()
    {
        //���� �罿 ��� , Energy 45ȸ��
        stateData.currentEnergy += 45;
    }
    public void FriedBoarmeat()
    {
        //���� ����� ��� ,Energy 50ȸ��
        stateData.currentEnergy += 50;
    }
    public void FriedSnake()
    {
        //���� �� ��� , Energy 30ȸ��
        stateData.currentEnergy += 30;
    }
    public void FriedElegatormeat()
    {
        //���� �Ǿ���, Enenrgy 50ȸ��
        stateData.currentEnergy += 50;
    }
    public void Octopus()
    {
        //���� , ����� Energy 30 ȸ��
        stateData.currentEnergy += 30;
    }
    public void friedEgg()
    {
        //���� ��� ,���� �� Energy  15 ȸ��
        stateData.currentEnergy += 15;
    }
    public void friedCrab()
    {
        //���� �ɰ� , ���� ��  Energy 10 ȸ��
        stateData.currentEnergy += 10;
    }
    public void Coconut()
    {
        //���ڳ� , ���� �� ���� 30 ȸ��
        stateData.currentWater += 30;
    }
    public void Papaya()
    {
        //���ľ� , ����� ���� 20 ȸ��
        stateData.currentWater += 20;
    }
    public void rainWater()
    {
        //���� , ����� ���� 20 ȸ�� 
        stateData.currentWater += 20;
    }

    //UI���� '�ڱ�' ��ư ������ ����Ǳ�
    public void Sleep()
    {
        //ī�޶� ������ �ʰ� �����
        fadeCamera.Sleep();
        //�ð��� 5�ð� �ڷ�
        FindAnyObjectByType<TimeManager>().AddFiveHours();
        //�Ƿ� ȸ�� �Ϸ�
        stateData.currentExhastion -= 100;
    }
}
