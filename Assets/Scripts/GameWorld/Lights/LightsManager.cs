using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Lights
{
    public class LightsManager : MonoBehaviour
    {
        Light sunLight;

        class TimeOfDayLightingSetting
        {
            public Color skyColor;
        }

        // public static Dictionary<TimeOfDay, TimeOfDayLightingSetting> timeOfDayLightingSettingMap = new Dictionary<TimeOfDay, TimeOfDayLightingSetting>()
        // {
        // { TimeOfDay. }
        // };

        float elapsedSinceLastTimeOfDay = 0f;
        float elapsedSinceLastTick = 0f;

        // when sunlight is at -90
        int sunlightStartTime = new TimeValue(new TimeValue.Input() { hour = 5 }).AsMinutes();
        // when sunlight is at +90
        int sunlightEndTime = new TimeValue(new TimeValue.Input() { hour = 20 }).AsMinutes();

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
            UpdateSkyColor();
            SetSunRotation();
        }

        void UpdateSkyColor()
        {
            Color currentColor = GetUpdateColorLerpProgressColor();
            Camera.main.backgroundColor = currentColor;
            RenderSettings.fogColor = currentColor;
        }

        void SetSunRotation()
        {
            TimeValue currentTime = Registry.appState.Time.time;

            TimeValue currentTimeFromStartOfDay = new TimeValue(new TimeValue.Input()
            {
                hour = currentTime.hour,
                minute = currentTime.minute
            });

            int currentTimeAsMinutes = currentTime.AsMinutes();
            int totalMinutesInTimeSunIsUp = sunlightEndTime - sunlightStartTime;
            int currentAbsoluteTimeAsMinutes = currentTimeFromStartOfDay.AsMinutes();
            float normalizedValue = normalize(currentAbsoluteTimeAsMinutes, (float)sunlightStartTime, (float)sunlightEndTime);

            Vector3 currentRotation = Vector3.Lerp(
                new Vector3(20f, -90f, 0),
                new Vector3(20f, 90f, 0),
                normalizedValue
            );

            sunLight.transform.rotation = Quaternion.Euler(currentRotation);

            float normalize(float val, float min, float max)
            {
                return Mathf.Clamp((val - min) / (max - min), 0f, 1f);
            }
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