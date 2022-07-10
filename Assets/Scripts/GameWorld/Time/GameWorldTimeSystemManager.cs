using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld
{

    public class GameWorldTimeSystemManager : MonoBehaviour
    {
        Camera camera;

        IEnumerator timeCoroutine;

        float currentTransitionTimeInSeconds = 0;

        void Awake()
        {
            camera = Camera.main;

            Registry.appState.Time.onTimeUpdated += OnTimeUpdated;

            StartTick();

            currentTransitionTimeInSeconds = Time.deltaTime;

            // SetCurrentSkyColor();
            UpdateSkyColor();
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

            UpdateSkyColor();
        }

        void OnTimeUpdated(TimeValue timeValue)
        {
            ResetTick();

            GetProgressToNextTimeOfDay();
            UpdateSkyColor();
        }

        void OnTick()
        {
            Registry.appState.Time.Tick();
        }

        void UpdateSkyColor()
        {
            Color currentColor = GetUpdateColorLerpProgressColor();
            camera.backgroundColor = currentColor;
            RenderSettings.fogColor = currentColor;
        }

        void SetSkyColor(Color color)
        {
            camera.backgroundColor = color;
        }

        void ResetTick()
        {
            StopTick();
            StartTick();
        }

        void StartTick()
        {
            timeCoroutine = StartTimeCoroutine();
            StartCoroutine(timeCoroutine);
        }

        void StopTick()
        {
            StopCoroutine(timeCoroutine);
        }

        IEnumerator StartTimeCoroutine()
        {
            while (true)
            {
                float interval = State.Time.Selectors.GetCurrentTickInterval(Registry.appState.Time);
                yield return new WaitForSeconds(interval);
                OnTick();
            }
        }

        void UpdateSpeed(TimeSpeed timeSpeed)
        {
            Registry.appState.Time.UpdateSpeed(timeSpeed);

            if (timeSpeed == TimeSpeed.Pause)
            {
                StopTick();
            }
            else
            {
                ResetTick();
            }
        }

        float GetProgressToNextTimeOfDay()
        {
            TimeValue currentTime = Registry.appState.Time.time;
            TimeValue absoluteCurrentTime = new TimeValue(new TimeInput()
            {
                minute = currentTime.minute,
                hour = currentTime.hour
            });

            int currentTimeAsMinutes = absoluteCurrentTime.AsMinutes();

            TimeOfDay currentTimeOfDay = absoluteCurrentTime.GetCurrentTimeOfDay();
            TimeOfDay nextTimeOfDay = absoluteCurrentTime.GetNextTimeOfDay();

            int currentTimeOfDayStartHourAsMinutes = new TimeValue(new TimeInput() { hour = currentTimeOfDay.startsOnHour }).AsMinutes();
            int nextTimeOfDayStartHourAsMinutes = new TimeValue(new TimeInput() { hour = nextTimeOfDay.startsOnHour }).AsMinutes();

            int totalDifference = nextTimeOfDayStartHourAsMinutes - currentTimeOfDayStartHourAsMinutes;
            int currentProgress = currentTimeAsMinutes - currentTimeOfDayStartHourAsMinutes;
            float progress = (float)currentProgress / (float)totalDifference;

            return progress;
        }

        float GetCurrentTimeOfDayDistance()
        {
            TimeValue currentTime = Registry.appState.Time.time;

            TimeOfDay currentTimeOfDay = currentTime.GetCurrentTimeOfDay();
            TimeOfDay nextTimeOfDay = currentTime.GetNextTimeOfDay();

            int currentTimeOfDayStartHourAsMinutes = new TimeValue(new TimeInput() { hour = currentTimeOfDay.startsOnHour }).AsMinutes();
            int nextTimeOfDayStartHourAsMinutes = new TimeValue(new TimeInput() { hour = nextTimeOfDay.startsOnHour }).AsMinutes();

            int totalDifference = nextTimeOfDayStartHourAsMinutes - currentTimeOfDayStartHourAsMinutes;

            return totalDifference / Constants.MINUTES_ELAPSED_PER_TICK;
        }

        Color GetUpdateColorLerpProgressColor()
        {
            TimeValue currentTime = Registry.appState.Time.time;
            TimeSpeed currentSpeed = Registry.appState.Time.speed;

            TimeOfDay currentTimeOfDay = currentTime.GetCurrentTimeOfDay();
            TimeOfDay nextTimeOfDay = currentTime.GetNextTimeOfDay();

            int currentTimeOfDayStartHourAsMinutes = new TimeValue(new TimeInput() { hour = currentTimeOfDay.startsOnHour }).AsMinutes();

            // int currentTimeOfDayStartHourAsTicks = currentTimeOfDayStartHourAsMinutes / Constants.MINUTES_ELAPSED_PER_TICK;

            int minutesSinceBeginningOfCurrentTimeOfDayStart = (currentTime.AsMinutes() - currentTimeOfDayStartHourAsMinutes);

            int ticksSinceBeginningOfCurrentTimeOfDayStart = minutesSinceBeginningOfCurrentTimeOfDayStart / Constants.MINUTES_ELAPSED_PER_TICK;

            float tickProgress = GetProgressToNextTimeOfDay();

            // currentTransitionTimeInSeconds += Time.deltaTime;
            // float currentTickInterval = Constants.TIME_SPEED_TICK_INTERVALS[currentSpeed];


            return Color.Lerp(
                currentTimeOfDay.skyColor,
                nextTimeOfDay.skyColor,
                tickProgress
            );
        }
    }
}
