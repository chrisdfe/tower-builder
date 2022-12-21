using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Lights
{
    public class LightsManager : MonoBehaviour, IFindable
    {
        public class AtmosphereSetting
        {
            public TimeOfDay.Key key;

            public TimeOfDay timeOfDay
            {
                get => TimeOfDay.FindByKey(key);
            }

            public TimeValue relativeStartTime
            {
                get => timeOfDay.relativeStartTime;
            }

            public override string ToString() => $"Atmosphere Setting for {key}";

            public Color? skyColor;

            public float? sunRotation;
            public Color? sunColor;
            public float? sunIntensity;

            public float? fogDensity;

            public float? interiorLightIntensity;
        }

        public static List<AtmosphereSetting> AtmosphereSettingMap = new List<AtmosphereSetting>()
        {
            new AtmosphereSetting() {
                key = TimeOfDay.Key.MorningNight,
                skyColor = ColorUtils.ColorFromHex("#111E1E"),
                sunColor = ColorUtils.ColorFromHex("#111E1E"),
                sunRotation = -170f,
                interiorLightIntensity = 1f
            },

            new AtmosphereSetting() {
                key =  TimeOfDay.Key.Dawn,
                skyColor = ColorUtils.ColorFromHex("#E37768"),
                sunColor = ColorUtils.ColorFromHex("#E37768"),
                sunRotation = -90f,
            },

            new AtmosphereSetting() {
                key = TimeOfDay.Key.Morning,
                skyColor = ColorUtils.ColorFromHex("#C7E6D5"),
                sunColor = ColorUtils.ColorFromHex("#FFCE62"),
                interiorLightIntensity = .5f,
            },

            new AtmosphereSetting() {
                key = TimeOfDay.Key.Afternoon,
                skyColor = ColorUtils.ColorFromHex("#ECE4D5"),
                sunColor = ColorUtils.ColorFromHex("#ECE4D5"),
                interiorLightIntensity = 0f,
            },

            new AtmosphereSetting()
            {
                key = TimeOfDay.Key.Evening,
                skyColor = ColorUtils.ColorFromHex("#C8E6D6"),
                sunColor = ColorUtils.ColorFromHex("#C8E6D6"),
                interiorLightIntensity = .5f,
            },

            new AtmosphereSetting()
            {
                key = TimeOfDay.Key.Dusk,
                skyColor = ColorUtils.ColorFromHex("#FFA885"),
                sunColor = ColorUtils.ColorFromHex("#FFA885"),
                sunRotation = 90f,
                interiorLightIntensity = 1f,
            },

            new AtmosphereSetting()
            {
                key = TimeOfDay.Key.DuskNight,
                skyColor = ColorUtils.ColorFromHex("#111E1E"),
                sunColor = ColorUtils.ColorFromHex("#111E1E"),
                interiorLightIntensity = 1f,
                sunRotation = 170f,
            }
        };

        public List<Light> interiorLights { get; private set; } = new List<Light>();

        Light sunLight;
        float elapsedSinceLastTimeOfDay = 0f;


        GameWorldTimeSystemManager timeSystemManager;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            sunLight = transform.Find("SunLight").GetComponent<Light>();

            timeSystemManager = GameWorldTimeSystemManager.Find();

            Setup();
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
            UpdateInteriorLights();
            UpdateSkyColor();
            UpdateSunRotation();
            UpdateSunColor();
        }

        /* 
            Internals
        */
        void UpdateInteriorLights()
        {
            (
                AtmosphereSetting currentAtmosphereSetting,
                AtmosphereSetting nextAtmosphereSetting
            ) = GetCurrentAndNextAtmosphereSettingsWhere((atmosphereSetting) => atmosphereSetting.interiorLightIntensity != null);

            (
                float normalizedCurrentTickTime,
                float normalizedNextTickTime
            ) = GameWorldUtils.GetNormalizedCurrentAndNextTickTimesBetween(
                currentAtmosphereSetting.relativeStartTime,
                nextAtmosphereSetting.relativeStartTime
            );

            float normalizedTickProgress = timeSystemManager.normalizedTickProgress;

            float currentTickIntensity = Mathf.Lerp(
                currentAtmosphereSetting.interiorLightIntensity.GetValueOrDefault(),
                nextAtmosphereSetting.interiorLightIntensity.GetValueOrDefault(),
                normalizedCurrentTickTime
            );

            float nextTickIntensity = Mathf.Lerp(
                currentAtmosphereSetting.interiorLightIntensity.GetValueOrDefault(),
                nextAtmosphereSetting.interiorLightIntensity.GetValueOrDefault(),
                normalizedNextTickTime
            );

            float currentIntensity = Mathf.Lerp(
                currentTickIntensity,
                nextTickIntensity,
                normalizedTickProgress
            );

            foreach (Light interiorLight in interiorLights)
            {
                interiorLight.intensity = currentIntensity;
            }
        }

        void UpdateSkyColor()
        {
            (
                AtmosphereSetting currentAtmosphereSetting,
                AtmosphereSetting nextAtmosphereSetting
            ) = GetCurrentAndNextAtmosphereSettingsWhere((atmosphereSetting) => atmosphereSetting.skyColor != null);

            (
                float normalizedCurrentTickTime,
                float normalizedNextTickTime
            ) = GameWorldUtils.GetNormalizedCurrentAndNextTickTimesBetween(
                currentAtmosphereSetting.relativeStartTime,
                nextAtmosphereSetting.relativeStartTime
            );

            float normalizedTickProgress = timeSystemManager.normalizedTickProgress;

            Color currentTickColor = Color.Lerp(
                currentAtmosphereSetting.skyColor.GetValueOrDefault(),
                nextAtmosphereSetting.skyColor.GetValueOrDefault(),
                normalizedCurrentTickTime
            );

            Color nextTickColor = Color.Lerp(
                currentAtmosphereSetting.skyColor.GetValueOrDefault(),
                nextAtmosphereSetting.skyColor.GetValueOrDefault(),
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
            TimeOfDay timeOfDay = Registry.appState.Time.time.previousTimeOfDay;

            (
                AtmosphereSetting startAtmosphereSetting,
                AtmosphereSetting endAtmosphereSetting
            ) = GetCurrentAndNextAtmosphereSettingsWhere(setting => setting.sunRotation != null);

            TimeOfDay sunlightStartTime = TimeOfDay.FindByKey(startAtmosphereSetting.key);
            TimeOfDay sunlightEndTime = TimeOfDay.FindByKey(endAtmosphereSetting.key);

            (
                float normalizedCurrentTickTime,
                float normalizedNextTickTime
            ) = GameWorldUtils.GetNormalizedCurrentAndNextTickTimesBetween(sunlightStartTime.relativeStartTime, sunlightEndTime.relativeStartTime);

            float normalizedTickProgress = timeSystemManager.normalizedTickProgress;

            float fromAngle = Mathf.LerpAngle(
                startAtmosphereSetting.sunRotation.GetValueOrDefault(),
                endAtmosphereSetting.sunRotation.GetValueOrDefault(),
                normalizedCurrentTickTime
            );

            float toAngle = Mathf.LerpAngle(
                startAtmosphereSetting.sunRotation.GetValueOrDefault(),
                endAtmosphereSetting.sunRotation.GetValueOrDefault(),
                normalizedNextTickTime
            );

            float angle = Mathf.LerpAngle(
                fromAngle,
                toAngle,
                normalizedTickProgress
            );

            sunLight.transform.eulerAngles = new Vector3(20f, angle, 0f);
        }

        void UpdateSunColor()
        {
            (
                AtmosphereSetting currentAtmosphereSetting,
                AtmosphereSetting nextAtmosphereSetting
            ) = GetCurrentAndNextAtmosphereSettingsWhere((atmosphereSetting) => atmosphereSetting.sunColor != null);

            (
                float normalizedCurrentTickTime,
                float normalizedNextTickTime
            ) = GameWorldUtils.GetNormalizedCurrentAndNextTickTimesBetween(
                currentAtmosphereSetting.relativeStartTime,
                nextAtmosphereSetting.relativeStartTime
            );

            float normalizedTickProgress = timeSystemManager.normalizedTickProgress;

            Color currentTickColor = Color.Lerp(
                currentAtmosphereSetting.skyColor.GetValueOrDefault(),
                nextAtmosphereSetting.skyColor.GetValueOrDefault(),
                normalizedCurrentTickTime
            );

            Color nextTickColor = Color.Lerp(
                currentAtmosphereSetting.skyColor.GetValueOrDefault(),
                nextAtmosphereSetting.skyColor.GetValueOrDefault(),
                normalizedNextTickTime
            );

            Color currentColor = Color.Lerp(
                currentTickColor,
                nextTickColor,
                normalizedTickProgress
            );

            sunLight.color = currentColor;
        }

        delegate bool AtmosphereSettingPredicate(AtmosphereSetting atmosphereSetting);
        CurrentAndNext<AtmosphereSetting> GetCurrentAndNextAtmosphereSettingsWhere(AtmosphereSettingPredicate matches)
        {
            AtmosphereSetting startAtmosphereSetting = GetAtmosphereSettingWhere(matches, false);
            AtmosphereSetting endAtmosphereSetting = GetAtmosphereSettingWhere(matches);

            return new CurrentAndNext<AtmosphereSetting>(startAtmosphereSetting, endAtmosphereSetting);
        }

        AtmosphereSetting GetAtmosphereSettingWhere(AtmosphereSettingPredicate matches, bool searchForwards = true)
        {
            TimeValue currentTime = Registry.appState.Time.time;
            TimeOfDay timeOfDay = currentTime.timeOfDay;

            if (searchForwards)
            {
                timeOfDay = timeOfDay.nextTimeOfDay;
            }

            AtmosphereSetting resultAtmosphereSetting = null;

            uint index = 0;
            while (resultAtmosphereSetting == null && index < Constants.TIMES_OF_DAY.Length)
            {
                AtmosphereSetting testAtmosphereSetting = AtmosphereSettingMap.Find(setting => setting.key == timeOfDay.key);

                if (matches(testAtmosphereSetting))
                {
                    resultAtmosphereSetting = testAtmosphereSetting;
                }
                else
                {
                    index++;

                    timeOfDay = searchForwards
                        ? timeOfDay.nextTimeOfDay
                        : timeOfDay.previousTimeOfDay;
                }
            }

            return resultAtmosphereSetting;
        }

        /*
            Event Handlers 
        */
        void OnTimeOfDayUpdated(TimeValue timeValue)
        {
            elapsedSinceLastTimeOfDay = 0;
        }

        /*
            Static API 
        */
        public static LightsManager Find() =>
            GameWorldFindableCache.Find<LightsManager>("LightsManager");
    }
}