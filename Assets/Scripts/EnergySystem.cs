using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    int maxEnergy = 10;
    int currentEnergy;
    int restoreDuration;
    DateTime nextEnergyTime;
    DateTime lastEnergyTime;
    bool isRestoring;

    void Start()
    {
        if (!PlayerPrefs.HasKey("currentEnergy"))
        {
            PlayerPrefs.SetInt("currentEnergy", maxEnergy);
            Load();
            StartCoroutine(RestoreEnergy());
        }
        else
        {
            Load();
            StartCoroutine(RestoreEnergy());
        }
    }

    IEnumerator RestoreEnergy()
    {
        UpdateEnergyTime();
        isRestoring = true;
        while (currentEnergy < maxEnergy)
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime nextDateTime = nextEnergyTime;
            bool isEnergyAdding = false;
            while (currentDateTime > nextEnergyTime)
            {
                if (currentEnergy < maxEnergy)
                {
                    isEnergyAdding = true;
                    currentEnergy++;
                    // UpdateEnergy();
                    DateTime timeToAdd = lastEnergyTime > nextDateTime ? lastEnergyTime : nextDateTime;
                    nextDateTime = AddDuration(timeToAdd, restoreDuration);
                }
                else
                {
                    break;
                }
            }
            if (isEnergyAdding)
            {
                lastEnergyTime = DateTime.Now;
                nextEnergyTime = nextDateTime;
            }
            UpdateEnergyTime();
            Save();
            yield return null;
        }
        isRestoring = false;

    }

    DateTime AddDuration(DateTime dateTime, int duration)
    {
        return dateTime.AddSeconds(duration);
    }

    void UpdateEnergyTime()
    {
        if (currentEnergy >= maxEnergy)
        {
            Debug.Log("Full");
            return;
        }

        TimeSpan time = nextEnergyTime - DateTime.Now;
        string timeValue = String.Format("{0:D2}:{1:d1}", time.Milliseconds, time.Seconds);
        Debug.Log(timeValue);
    }

    DateTime StringToDate(string dateTime)
    {
        if (String.IsNullOrEmpty(dateTime))
        {
            return DateTime.Now;
        }
        else
        {
            return DateTime.Parse(dateTime);
        }
    }

    void Load()
    {
        currentEnergy = PlayerPrefs.GetInt("currentEnergy");
        nextEnergyTime = StringToDate(PlayerPrefs.GetString("nextEnergyTime"));
        lastEnergyTime = StringToDate(PlayerPrefs.GetString("lastEnergyTime"));
    }

    void Save()
    {
        PlayerPrefs.SetInt("currentEnergy", currentEnergy);
        PlayerPrefs.SetString("nextEnergyTime", nextEnergyTime.ToString());
        PlayerPrefs.SetString("lastEnergyTime", lastEnergyTime.ToString());
    }
}
