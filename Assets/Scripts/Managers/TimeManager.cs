using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Security.Cryptography.X509Certificates;
using System.Security;

[SerializeField]
public class TimeDate
{
    //Json에 저장할 현재 시간
    public DateTime currentTime;
    public DateTime currentDate;
}
public class TimeManager : MonoBehaviour
{
    TimeDate timeDate;
    //일출 SkyBox
    [SerializeField] private Texture2D skyboxSunrize;
    //낮 skyBox
    [SerializeField] private Texture2D skyboxDay;
    //일몰 SkyBox
    [SerializeField] private Texture2D skyboxSunset;
    //밤 skyBox
    [SerializeField] private Texture2D skyboxNight;
    //해로 사용할 directional Light
    public Light Sun;
    [SerializeField]
    private float timeMultiplier;

    [SerializeField]
    private float startHour;

    private float startDay;

    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI DateText;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunriseHour;

    [SerializeField]
    private float sunsetHour;

    [SerializeField]
    private Color dayAmbientLight;

    [SerializeField]
    private Color nightAmbientLight;

    [SerializeField]
    private AnimationCurve lightChangeCurve;

    [SerializeField]
    private float maxSunLightIntensity;

    [SerializeField]
    private Light moonLight;

    [SerializeField]
    private float maxMoonLightIntensity;

    public DateTime currentTime;
    public DateTime currentDate;

    private TimeSpan sunriseTime;

    private TimeSpan sunsetTime;
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        currentDate = DateTime.Now.Date + TimeSpan.FromDays(startDay);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateSkyboxAndLighting();
    }
    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
        // currentTime의 시간이 24시간 이상일 경우 날짜를 하루 더함
        if (currentTime.TimeOfDay.TotalHours >= 23.99f)
        {
            currentTime = currentTime.AddDays(1).Date;  // 날짜를 하루 추가하고 시간을 00:00으로 설정
            currentDate = currentDate.AddDays(1);       // 날짜를 업데이트
            DateText.text = currentDate.ToString("yyyy.MM.dd");
        }

        if (DateText != null)
        {
            DateText.text = currentDate.ToString("yyyy.MM.dd");
        }
    }
    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }
    void UpdateSkyboxAndLighting()
    {
        // 낮일 때 Skybox와 조명 설정
        if (currentTime.Hour == 4)
        {

            StartCoroutine(LerpSkyBox(skyboxNight, skyboxSunrize, 6f));
            RenderSettings.skybox.SetFloat("_Exposure2", 1f);
            if (currentTime.Hour == 6)
            {
                sunLight.enabled = true;
            }
        }
        else if (currentTime.Hour == 7)
        {
            sunLight.enabled = true;
            StartCoroutine(LerpSkyBox(skyboxSunrize, skyboxDay, 6f));
            RenderSettings.skybox.SetFloat("_Exposure2", 1f);

        }
        else if (currentTime.Hour == 16)
        {
            sunLight.enabled = true;
            StartCoroutine(LerpSkyBox(skyboxDay, skyboxSunset, 6f));
            RenderSettings.skybox.SetFloat("_Exposure2", 1f);
        }
        else if (currentTime.Hour == 19)
        {
            sunLight.enabled = false;
            StartCoroutine(LerpSkyBox(skyboxSunset, skyboxNight, 6f));
            RenderSettings.skybox.SetFloat("_Exposure2", 0.3f);
        }
        // Skybox가 변경되었음을 Unity에 알림
        DynamicGI.UpdateEnvironment();
    }
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }
    //Skybox를 부드럽게 변환하기 위한 코루틴
    private IEnumerator LerpSkyBox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }
    //현재시간을 반환하는 함수
    public DateTime GetCurrentTime()
    {
        return currentTime;
    }
}