using System.Collections.Generic;
using TowerBuilder.DataTypes;
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
            public Color skyColor;

            // public float sunRotation;
            public float sunColor;
            public float sunIntensity;

            public float fogDensity;
        }

        public Dictionary<TimeOfDay.Key, AtmosphereSetting> TimeOfDayAtmosphereSettingMap = new Dictionary<TimeOfDay.Key, AtmosphereSetting>()
        {
            {
                TimeOfDay.Key.MorningNight,
                new AtmosphereSetting() {
                    skyColor = ColorUtils.ColorFromHex("#111E1E")
                }
            },
            {
                TimeOfDay.Key.Dawn,
                new AtmosphereSetting() {
                    skyColor = ColorUtils.ColorFromHex("#E37768")
                }
            },
            {
                TimeOfDay.Key.Morning,
                new AtmosphereSetting() {
                    skyColor = ColorUtils.ColorFromHex("#C7E6D5")
                }
            },
            {
                TimeOfDay.Key.Afternoon,
                new AtmosphereSetting() {
                    skyColor = ColorUtils.ColorFromHex("#ECE4D5")
                }
            },
            {
                TimeOfDay.Key.Evening,
                new AtmosphereSetting() {
                    skyColor = ColorUtils.ColorFromHex("#C8E6D6")
                }
            },
            {
                TimeOfDay.Key.Dusk,
                new AtmosphereSetting() {
                    skyColor = ColorUtils.ColorFromHex("#FFA885")
                }
            },
            {
                TimeOfDay.Key.DuskNight,
                new AtmosphereSetting() {
                    skyColor = ColorUtils.ColorFromHex("#111E1E")
                }
            }
        };

        float elapsedSinceLastTimeOfDay = 0f;

        // when sunlight is at -90
        TimeValue sunlightStartTime = new TimeValue(new TimeValue.Input() { hour = 5 });
        // when sunlight is at +90
        TimeValue sunlightEndTime = new TimeValue(new TimeValue.Input() { hour = 20 });

        static Vector3 dayStartRotation { get; } = new Vector3(20f, -90f, 0);
        static Vector3 dayEndRotation { get; } = new Vector3(20f, +90f, 0);

        GameWorldTimeSystemManager timeSystemManager;

        void Awake()
        {
            sunLight = transform.Find("SunLight").GetComponent<Light>();

            timeSystemManager = GameWorldTimeSystemManager.Find();

            Setup();

            UpdateSkyColor();
            UpdateSunRotation();
        }

        void Setup()
        {
            Registry.appState.Time.events.onTimeOfDayUpdated += OnTimeOfDayUpdated;
        }

        void Teardown()
        {
            Registry.appState.Time.events.onTimeOfDayUpdated -= OnTimeOfDayUpdated;
        }

        void Update()
        {
            UpdateSkyColor();
            UpdateSunRotation();
        }

        void OnTimeOfDayUpdated(TimeValue timeValue)
        {
            elapsedSinceLastTimeOfDay = 0;
        }

        void UpdateSkyColor()
        {
            TimeValue currentTime = Registry.appState.Time.time;
            TimeValue currentTimeOfDayStartHour = new TimeValue(new TimeValue.Input() { hour = currentTime.timeOfDay.startsOnHour });
            TimeValue nextTimeOfDayStartHour = new TimeValue(new TimeValue.Input() { hour = currentTime.nextTimeOfDay.startsOnHour });
            float normalizedTickProgress = timeSystemManager.normalizedTickProgress;

            (float normalizedCurrentTickTime, float normalizedNextTickTime) = GameWorldUtils.GetNormalizedCurrentAndNextTickTimesBetween(currentTimeOfDayStartHour, nextTimeOfDayStartHour);
            (AtmosphereSetting currentAtmosphereSetting, AtmosphereSetting nextAtmosphereSetting) = GetCurrentAndNextAtmosphereSettings();

            Color currentTickColor = Color.Lerp(
                currentAtmosphereSetting.skyColor,
                nextAtmosphereSetting.skyColor,
                normalizedCurrentTickTime
            );

            Color nextTickColor = Color.Lerp(
                currentAtmosphereSetting.skyColor,
                nextAtmosphereSetting.skyColor,
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
            (float normalizedCurrentTickTime, float normalizedNextTickTime) = GameWorldUtils.GetNormalizedCurrentAndNextTickTimesBetween(sunlightStartTime, sunlightEndTime);
            float normalizedTickProgress = timeSystemManager.normalizedTickProgress;

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

        CurrentAndNext<AtmosphereSetting> GetCurrentAndNextAtmosphereSettings()
        {
            TimeOfDay currentTimeOfDay = Registry.appState.Time.time.timeOfDay;
            TimeOfDay nextTimeOfDay = Registry.appState.Time.time.nextTimeOfDay;

            return new CurrentAndNext<AtmosphereSetting>()
            {
                current = TimeOfDayAtmosphereSettingMap[currentTimeOfDay.key],
                next = TimeOfDayAtmosphereSettingMap[nextTimeOfDay.key]
            };
        }
    }
}