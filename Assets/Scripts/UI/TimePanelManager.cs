using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Time;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class TimePanelManager : MonoBehaviour
    {
        Button add1HourButton;
        Button subtract1HourButton;
        Text hoursMinutesText;
        Text weeksSeasonsText;
        Text speedText;

        // TODO - this doesn't belong here but it is fine for now
        IEnumerator timeCoroutine;

        void Awake()
        {
            add1HourButton = transform.Find("Add1HourButton").GetComponent<Button>();
            add1HourButton.onClick.AddListener(Add1Hour);

            subtract1HourButton = transform.Find("Subtract1HourButton").GetComponent<Button>();
            subtract1HourButton.onClick.AddListener(Subtract1Hour);

            hoursMinutesText = transform.Find("HoursMinutesText").GetComponent<Text>();
            UpdateHoursMinutesText();

            weeksSeasonsText = transform.Find("WeeksSeasonsText").GetComponent<Text>();
            UpdateWeeksSeasonsText();

            speedText = transform.Find("SpeedText").GetComponent<Text>();
            UpdateSpeedText();

            Registry.Stores.Time.onTimeUpdated += OnTimeStateUpdated;

            StartTick();
        }

        void Update()
        {
            if (Input.GetKeyDown("`"))
            {
                UpdateSpeed(TimeSpeed.Pause);
            }

            if (Input.GetKeyDown("1"))
            {
                UpdateSpeed(TimeSpeed.Normal);
            }

            if (Input.GetKeyDown("2"))
            {
                UpdateSpeed(TimeSpeed.Fast);
            }

            if (Input.GetKeyDown("3"))
            {
                UpdateSpeed(TimeSpeed.Fastest);
            }
        }

        void Add1Hour()
        {
            Registry.Stores.Time.AddTime(new TimeInput()
            {
                hour = 1
            });
            ResetTick();
        }

        void Subtract1Hour()
        {
            Registry.Stores.Time.SubtractTime(new TimeInput()
            {
                hour = 1
            });
            ResetTick();
        }

        void OnTimeStateUpdated(TimeValue time)
        {
            UpdateHoursMinutesText();
            UpdateWeeksSeasonsText();
        }

        void UpdateHoursMinutesText()
        {
            Stores.Time.State state = Registry.Stores.Time;
            int hour = state.time.hour;
            int minute = state.time.minute;

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
            Stores.Time.State state = Registry.Stores.Time;
            int day = state.time.day;
            int week = state.time.week;
            int season = state.time.season;
            int year = state.time.year;

            weeksSeasonsText.text = $"Day: {day}, Week: {week}, Season: {season}, Year: {year}";
        }

        void UpdateSpeedText()
        {
            TimeSpeed currentSpeed = Registry.Stores.Time.speed;
            speedText.text = $"Speed: {currentSpeed}";
        }

        // TODO - this doesn't belong here but it is fine for now
        void ResetTick()
        {
            StopTick();
            StartTick();
        }

        // TODO - this doesn't belong here but it is fine for now
        void StartTick()
        {
            timeCoroutine = StartTimeCoroutine();
            StartCoroutine(timeCoroutine);
        }

        void StopTick()
        {
            StopCoroutine(timeCoroutine);
        }

        // TODO - this doesn't belong here but it is fine for now
        void OnTick()
        {
            Registry.Stores.Time.Tick();
        }

        // TODO - this doesn't belong here but it is fine for now
        IEnumerator StartTimeCoroutine()
        {
            while (true)
            {
                float interval = Stores.Time.Selectors.GetCurrentTickInterval(Registry.Stores.Time);
                yield return new WaitForSeconds(interval);
                OnTick();
            }
        }

        // TODO - this doesn't belong here but it is fine for now
        void UpdateSpeed(TimeSpeed timeSpeed)
        {
            Registry.Stores.Time.UpdateSpeed(timeSpeed);
            UpdateSpeedText();

            if (timeSpeed == TimeSpeed.Pause)
            {
                StopTick();
            }
            else
            {
                ResetTick();
            }
        }
    }
}