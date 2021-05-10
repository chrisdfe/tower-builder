using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TowerBuilder.Stores;
using TowerBuilder.Stores.Time;

namespace TowerBuilder.UI
{
    public class TimePanelManager : MonoBehaviour
    {
        Button add1HourButton;
        Button subtract1HourButton;
        Text hoursMinutesText;
        Text weeksSeasonsText;

        // TODO - this doesn't belong here but it is fine for now
        private IEnumerator timeCoroutine;

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

            TimeStore.Events.onTimeStateUpdated += OnTimeStateUpdated;

            StartTick();
        }

        void Add1Hour()
        {
            TimeStore.Mutations.AddTime(new TimeInput()
            {
                hour = 1
            });
            ResetTick();
        }

        void Subtract1Hour()
        {
            TimeStore.Mutations.SubtractTime(new TimeInput()
            {
                hour = 1
            });
            ResetTick();
        }

        void OnTimeStateUpdated(TimeStore.StateEventPayload payload)
        {
            UpdateHoursMinutesText();
            UpdateWeeksSeasonsText();
        }

        void UpdateHoursMinutesText()
        {
            TimeState state = Registry.storeRegistry.timeStore.state;
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
            TimeState state = Registry.storeRegistry.timeStore.state;
            int day = state.time.day;
            int week = state.time.week;
            int season = state.time.season;
            int year = state.time.year;

            weeksSeasonsText.text = $"Day: {day}, Week: {week}, Season: {season}, Year: {year}";
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
            TimeStore.Mutations.Tick();
        }

        // TODO - this doesn't belong here but it is fine for now
        IEnumerator StartTimeCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(TimeStore.Constants.TICK_INTERVAL);
                OnTick();
            }
        }
    }
}