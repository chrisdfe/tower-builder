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

            public Color? skyColor;

            public float? sunRotation;
            public float? sunColor;
            public float? sunIntensity;

            public float? fogDensity;

            public float? interiorLightBrightness;
        }

        public static List<AtmosphereSetting> AtmosphereSettingMap = new List<AtmosphereSetting>()
        {
            new AtmosphereSetting() {
                key = TimeOfDay.Key.MorningNight,
                skyColor = ColorUtils.ColorFromHex("#111E1E"),
                sunRotation = -170f,
            },

            new AtmosphereSetting() {
                key =  TimeOfDay.Key.Dawn,
                skyColor = ColorUtils.ColorFromHex("#E37768"),
                sunRotation = -90f,
            },

            new AtmosphereSetting() {
                key = TimeOfDay.Key.Morning,
                skyColor = ColorUtils.ColorFromHex("#C7E6D5"),
            },

            new AtmosphereSetting() {
                key = TimeOfDay.Key.Afternoon,
                skyColor = ColorUtils.ColorFromHex("#ECE4D5"),
            },

            new AtmosphereSetting()
            {
                key = TimeOfDay.Key.Evening,
                skyColor = ColorUtils.ColorFromHex("#C8E6D6"),
            },

            new AtmosphereSetting()
            {
                key = TimeOfDay.Key.Dusk,
                skyColor = ColorUtils.ColorFromHex("#FFA885"),
                sunRotation = 90f,
            },

            new AtmosphereSetting()
            {
                key = TimeOfDay.Key.DuskNight,
                skyColor = ColorUtils.ColorFromHex("#111E1E"),
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

            // UpdateSkyColor();
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
            UpdateInteriorLights();
            UpdateSkyColor();
            UpdateSunRotation();
        }

        /* 
            Internals
        */
        void UpdateInteriorLights()
        {

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

            AtmosphereSetting startAtmosphereSetting =
                GetAtmosphereSettingWhere(timeOfDay.key, (atmosphereSetting) => atmosphereSetting.sunRotation != null);
            AtmosphereSetting endAtmosphereSetting =
                GetAtmosphereSettingWhere(startAtmosphereSetting.key, (atmosphereSetting) => atmosphereSetting.sunRotation != null);

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

            sunLight.transform.eulerAngles = new Vector3(20f, angle, 0f); ;
        }

        CurrentAndNext<AtmosphereSetting> GetCurrentAndNextAtmosphereSettings()
        {
            TimeOfDay currentTimeOfDay = Registry.appState.Time.time.timeOfDay;
            TimeOfDay nextTimeOfDay = Registry.appState.Time.time.nextTimeOfDay;

            return new CurrentAndNext<AtmosphereSetting>()
            {
                current = AtmosphereSettingMap.Find(setting => setting.key == currentTimeOfDay.key),
                next = AtmosphereSettingMap.Find(setting => setting.key == nextTimeOfDay.key)
            };
        }

        delegate bool AtmosphereSettingPredicate(AtmosphereSetting atmosphereSetting);
        CurrentAndNext<AtmosphereSetting> GetCurrentAndNextAtmosphereSettingsWhere(AtmosphereSettingPredicate matches)
        {
            TimeOfDay previousTimeOfDay = Registry.appState.Time.time.previousTimeOfDay;

            AtmosphereSetting startAtmosphereSetting =
                GetAtmosphereSettingWhere(previousTimeOfDay.key, matches);
            AtmosphereSetting endAtmosphereSetting =
                GetAtmosphereSettingWhere(startAtmosphereSetting.key, matches);

            return new CurrentAndNext<AtmosphereSetting>(startAtmosphereSetting, endAtmosphereSetting);
        }

        AtmosphereSetting GetAtmosphereSettingWhere(TimeOfDay.Key startingTimeOfDayKey, AtmosphereSettingPredicate matches)
        {
            TimeOfDay startingTimeOfDay =
                new List<TimeOfDay>(Constants.TIMES_OF_DAY).Find(timeOfDay => timeOfDay.key == startingTimeOfDayKey);

            TimeOfDay timeOfDay = startingTimeOfDay;
            AtmosphereSetting resultAtmosphereSetting = null;

            uint index = 0;
            while (resultAtmosphereSetting == null && index < Constants.TIMES_OF_DAY.Length)
            {
                timeOfDay = timeOfDay.nextTimeOfDay;
                AtmosphereSetting testAtmosphereSetting = AtmosphereSettingMap.Find(setting => setting.key == timeOfDay.key);

                if (matches(testAtmosphereSetting))
                {
                    resultAtmosphereSetting = testAtmosphereSetting;
                }
                else
                {
                    index++;
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