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

        float elapsedSinceLastTimeOfDay = 0f;
        float elapsedSinceLastTick = 0f;

        void Awake()
        {
            camera = Camera.main;

            Registry.appState.Time.onTick += OnTick;
            Registry.appState.Time.onTimeUpdated += OnTimeUpdated;
            Registry.appState.Time.onTimeOfDayChanged += OnTimeOfDayChanged;

            StartTick();

            // SetCurrentSkyColor();
            UpdateSkyColor();
        }

        void Update()
        {
            elapsedSinceLastTick += Time.deltaTime;

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

        void OnTimeOfDayChanged(TimeValue timeValue)
        {
            Debug.Log("new time of day");
            elapsedSinceLastTimeOfDay = 0;
        }

        void OnTimeUpdated(TimeValue timeValue)
        {
            ResetTick();

            UpdateSkyColor();
        }

        void OnTick(TimeValue timeValue)
        {
            elapsedSinceLastTick = 0f;
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
                float interval = Registry.appState.Time.GetCurrentTickInterval();
                yield return new WaitForSeconds(interval);
                Registry.appState.Time.Tick();
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

        Color GetUpdateColorLerpProgressColor()
        {
            TimeValue currentTime = Registry.appState.Time.time;
            TimeSpeed currentSpeed = Registry.appState.Time.speed;
            TimeValue absoluteCurrentTime = new TimeValue(new TimeInput()
            {
                minute = currentTime.minute,
                hour = currentTime.hour
            });

            TimeOfDay currentTimeOfDay = currentTime.GetCurrentTimeOfDay();
            TimeOfDay nextTimeOfDay = currentTime.GetNextTimeOfDay();

            int currentTimeOfDayStartHourAsMinutes = new TimeValue(new TimeInput()
            {
                hour = currentTimeOfDay.startsOnHour
            }).AsMinutes();

            int currentTimeAsMinutes = absoluteCurrentTime.AsMinutes();

            int nextTimeOfDayStartHourAsMinutes = new TimeValue(new TimeInput()
            {
                hour = nextTimeOfDay.startsOnHour
            }).AsMinutes();

            int totalDifferenceAsMinutes = nextTimeOfDayStartHourAsMinutes - currentTimeOfDayStartHourAsMinutes;
            int currentProgressAsMinutes = currentTimeAsMinutes - currentTimeOfDayStartHourAsMinutes;

            float minutesProgress = (float)currentProgressAsMinutes / (float)totalDifferenceAsMinutes;

            int currentProgressAsTicks = currentProgressAsMinutes * Constants.MINUTES_ELAPSED_PER_TICK;
            int totalDifferenceAsTicks = totalDifferenceAsMinutes * Constants.MINUTES_ELAPSED_PER_TICK;

            float ticksProgress = (float)currentProgressAsTicks / (float)totalDifferenceAsTicks;

            // Work out frame-level progress
            float currentProgressAsRealSeconds = InGameMinutesToRealSeconds(currentProgressAsMinutes);
            float totalDifferenceAsRealSeconds = InGameMinutesToRealSeconds(totalDifferenceAsMinutes);

            float minutesProgressInSeconds = currentProgressAsRealSeconds / currentProgressAsRealSeconds;

            float currentTickInterval = Registry.appState.Time.GetCurrentTickInterval();

            float currentTickProgress = elapsedSinceLastTick / currentTickInterval;

            float progress = (
                // (elapsedSinceLastTimeOfDay / totalDifferenceAsRealSeconds) / currentTickInterval
                (ticksProgress)
            // + (currentTickProgress / totalDifferenceAsRealSeconds)
            );

            // Debug.Log("---");

            return Color.Lerp(
                currentTimeOfDay.skyColor,
                nextTimeOfDay.skyColor,
                progress
            );
        }

        float InGameMinutesToRealSeconds(int minutes)
        {
            // float currentSpeed = Constants.TIME_SPEED_TICK_INTERVALS[Registry.appState.Time.speed];
            // float framerate = 1 / Time.unscaledDeltaTime;
            // Debug.Log("framerate: " + framerate);

            return (
                minutes *
                (Constants.TICK_INTERVAL / Constants.MINUTES_ELAPSED_PER_TICK)
            );
        }
    }
}
