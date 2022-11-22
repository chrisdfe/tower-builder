using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Lights
{
    public class LightsManager : MonoBehaviour
    {
        Light sunLight;

        // public static SunLight

        float elapsedSinceLastTimeOfDay = 0f;
        float elapsedSinceLastTick = 0f;


        void Awake()
        {
            sunLight = transform.Find("SunLight").GetComponent<Light>();

            UpdateSkyColor();

            Setup();
        }

        void Setup()
        {
            Registry.appState.Time.events.onTick += OnTick;
            Registry.appState.Time.events.onTimeOfDayUpdated += OnTimeOfDayUpdated;
        }

        void Teardown()
        {
            Registry.appState.Time.events.onTick -= OnTick;
            Registry.appState.Time.events.onTimeOfDayUpdated -= OnTimeOfDayUpdated;
        }

        void Update()
        {
            elapsedSinceLastTick += Time.deltaTime;
        }

        void OnTimeOfDayUpdated(TimeValue timeValue)
        {
            elapsedSinceLastTimeOfDay = 0;
        }


        void OnTick(TimeValue timeValue)
        {
            elapsedSinceLastTick = 0f;
        }

        void UpdateSkyColor()
        {
            Color currentColor = GetUpdateColorLerpProgressColor();
            Camera.main.backgroundColor = currentColor;
            RenderSettings.fogColor = currentColor;
        }

        Color GetUpdateColorLerpProgressColor()
        {
            TimeValue currentTime = Registry.appState.Time.time;
            TimeSpeed currentSpeed = Registry.appState.Time.speed;
            TimeValue absoluteCurrentTime = new TimeValue(new TimeValue.Input()
            {
                minute = currentTime.minute,
                hour = currentTime.hour
            });

            TimeOfDay currentTimeOfDay = currentTime.GetCurrentTimeOfDay();
            TimeOfDay nextTimeOfDay = currentTime.GetNextTimeOfDay();

            int currentTimeOfDayStartHourAsMinutes = new TimeValue(new TimeValue.Input()
            {
                hour = currentTimeOfDay.startsOnHour
            }).AsMinutes();

            int currentTimeAsMinutes = absoluteCurrentTime.AsMinutes();

            int nextTimeOfDayStartHourAsMinutes = new TimeValue(new TimeValue.Input()
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