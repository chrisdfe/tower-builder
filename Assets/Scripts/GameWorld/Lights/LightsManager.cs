using System.Collections.Generic;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Lights
{
    public class LightsManager : MonoBehaviour
    {
        Light sunLight;

        public class AtmosphereSetting
        {
            public TimeValue timeOfDay;
            public Color skyColor;
            public float fogDensity;
        }

        // public static Dictionary<TimeOfDay, TimeOfDayLightingSetting> timeOfDayLightingSettingMap = new Dictionary<TimeOfDay, TimeOfDayLightingSetting>()
        // {
        // { TimeOfDay. }
        // };

        public List<AtmosphereSetting> atmosphereSettings { get; } = new List<AtmosphereSetting>()
        {

        };

        float elapsedSinceLastTimeOfDay = 0f;
        float elapsedSinceLastTick = 0f;
        float normalizedTickProgress = 0f;

        // when sunlight is at -90
        static int sunlightStartTime = new TimeValue(new TimeValue.Input() { hour = 5 }).AsMinutes();
        // when sunlight is at +90
        static int sunlightEndTime = new TimeValue(new TimeValue.Input() { hour = 20 }).AsMinutes();

        static Vector3 dayStartRotation { get; } = new Vector3(20f, -90f, 0);
        static Vector3 dayEndRotation { get; } = new Vector3(20f, +90f, 0);

        void Awake()
        {
            sunLight = transform.Find("SunLight").GetComponent<Light>();

            Setup();

            UpdateSkyColor();
            UpdateSunRotation();
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
            UpdateTickProgress();

            UpdateSkyColor();
            UpdateSunRotation();
        }

        void OnTimeOfDayUpdated(TimeValue timeValue)
        {
            elapsedSinceLastTimeOfDay = 0;
        }

        void OnTick(TimeValue timeValue)
        {
            elapsedSinceLastTick = 0f;
        }

        void UpdateTickProgress()
        {
            float currentTickInterval = Registry.appState.Time.queries.currentTickInterval;
            float tickIncrement = Time.deltaTime / currentTickInterval;

            elapsedSinceLastTick += tickIncrement;
            normalizedTickProgress = Mathf.Clamp(elapsedSinceLastTick, 0f, 1f);
        }

        void UpdateSkyColor()
        {
            TimeOfDay currentTimeOfDay = Registry.appState.Time.time.GetCurrentTimeOfDay();
            TimeOfDay nextTimeOfDay = Registry.appState.Time.time.GetNextTimeOfDay();

            int currentTimeOfDayStartHourAsMinutes = new TimeValue(new TimeValue.Input() { hour = currentTimeOfDay.startsOnHour }).AsMinutes();
            int nextTimeOfDayStartHourAsMinutes = new TimeValue(new TimeValue.Input() { hour = nextTimeOfDay.startsOnHour }).AsMinutes();

            float normalizedCurrentTickTime = MathUtils.NormalizeFloat(
                Registry.appState.Time.time.ToRelative().AsMinutes(),
                currentTimeOfDayStartHourAsMinutes,
                nextTimeOfDayStartHourAsMinutes
            );

            float normalizedNextTickTime = MathUtils.NormalizeFloat(
                Registry.appState.Time.queries.nextTickTimeValue.ToRelative().AsMinutes(),
                currentTimeOfDayStartHourAsMinutes,
                nextTimeOfDayStartHourAsMinutes
            );

            Color currentTickColor = Color.Lerp(
                currentTimeOfDay.skyColor,
                nextTimeOfDay.skyColor,
                normalizedCurrentTickTime
            );

            Color nextTickColor = Color.Lerp(
                currentTimeOfDay.skyColor,
                nextTimeOfDay.skyColor,
                normalizedNextTickTime
            );

            Color currentColor = Color.Lerp(
                currentTickColor,
                nextTickColor,
                normalizedTickProgress
            );

            Camera.main.backgroundColor = currentColor;
            RenderSettings.fogColor = currentColor;
        }

        void UpdateSunRotation()
        {
            int currentTimeAsMinutes = Registry.appState.Time.time.AsMinutes();

            int currentRelativeTimeAsMinutes = Registry.appState.Time.queries.currentRelativeTimeValue.AsMinutes();
            int nextTickTimeAsMinutes = TimeValue.Add(Registry.appState.Time.time, new TimeValue.Input() { minute = Constants.MINUTES_ELAPSED_PER_TICK }).AsMinutes();

            float normalizedCurrentTickTime = MathUtils.NormalizeFloat(
                Registry.appState.Time.time.ToRelative().AsMinutes(),
                (float)sunlightStartTime,
                (float)sunlightEndTime
            );

            float normalizedNextTickTime = MathUtils.NormalizeFloat(
                Registry.appState.Time.queries.nextTickTimeValue.ToRelative().AsMinutes(),
                (float)sunlightStartTime,
                (float)sunlightEndTime
            );

            Vector3 fromRotation = Vector3.Lerp(
                dayStartRotation,
                dayEndRotation,
                normalizedCurrentTickTime
            );

            Vector3 toRotation = Vector3.Lerp(
                dayStartRotation,
                dayEndRotation,
                normalizedNextTickTime
            );

            Vector3 currentRotation = Vector3.Lerp(
                fromRotation,
                toRotation,
                normalizedTickProgress
            );

            sunLight.transform.rotation = Quaternion.Euler(currentRotation);
        }
    }
}