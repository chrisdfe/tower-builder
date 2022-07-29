using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.State;
using TowerBuilder.State.Time;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class TimePanelManager : MonoBehaviour
    {
        Text hoursMinutesText;
        Text weeksSeasonsText;
        Text speedText;
        Text timeOfDayText;

        void Awake()
        {
            hoursMinutesText = transform.Find("HoursMinutesText").GetComponent<Text>();
            UpdateHoursMinutesText();

            weeksSeasonsText = transform.Find("WeeksSeasonsText").GetComponent<Text>();
            UpdateWeeksSeasonsText();

            speedText = transform.Find("SpeedText").GetComponent<Text>();
            UpdateSpeedText();

            timeOfDayText = transform.Find("TimeOfDayText").GetComponent<Text>();
            UpdateTimeOfDayText();

            Registry.appState.Time.time.onValueChanged += OnTimeStateUpdated;
            Registry.appState.Time.speed.onValueChanged += OnTimeSpeedUpdated;
        }

        void OnTimeStateUpdated(TimeValue time, TimeValue previousTime)
        {
            UpdateHoursMinutesText();
            UpdateWeeksSeasonsText();
            UpdateTimeOfDayText();
        }

        void OnTimeSpeedUpdated(TimeSpeed speed)
        {
            UpdateSpeedText();
        }

        void UpdateHoursMinutesText()
        {
            State.Time.State state = Registry.appState.Time;
            int hour = state.time.value.hour;
            int minute = state.time.value.minute;

            string hourAsString = hour.ToString();
            if (hour < 10)
            {
                hourAsString = "0" + hour.ToString();
            }

            string minuteAsString = minute.ToString();
            if (minute < 10)
            {
                minuteAsString = "0" + minute.ToString();
            }

            hoursMinutesText.text = hourAsString + ":" + minuteAsString;
        }

        void UpdateWeeksSeasonsText()
        {
            State.Time.State state = Registry.appState.Time;
            int day = state.time.value.day;
            int week = state.time.value.week;
            int season = state.time.value.season;
            int year = state.time.value.year;

            weeksSeasonsText.text = $"Day: {day}, Week: {week}, Season: {season}, Year: {year}";
        }

        void UpdateSpeedText()
        {
            TimeSpeed currentSpeed = Registry.appState.Time.speed.value;
            speedText.text = $"Speed: {currentSpeed}";
        }

        void UpdateTimeOfDayText()
        {
            TimeValue currentTime = Registry.appState.Time.time.value;
            TimeOfDay currentTimeOfDay = currentTime.GetCurrentTimeOfDay();
            timeOfDayText.text = currentTimeOfDay.name;
        }
    }
}