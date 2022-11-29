using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class GameWorldTimeSystemManager : MonoBehaviour
    {
        // Camera camera;
        public float elapsedSinceLastTick { get; private set; } = 0f;
        public float normalizedTickProgress { get; private set; } = 0f;

        IEnumerator timeCoroutine;

        TimeSpeed previousSpeed;

        void Awake()
        {
            // camera = Camera.main;
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        void Setup()
        {
            Registry.appState.Time.events.onTick += OnTick;
        }

        void Teardown()
        {
            Registry.appState.Time.events.onTick -= OnTick;
        }

        void Start()
        {
            StartTick();
        }

        void Update()
        {
            UpdateTickProgress();
            HandleInput();
        }

        void OnTick(TimeValue time)
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

        void HandleInput()
        {
            TimeSpeed currentSpeed = Registry.appState.Time.speed;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentSpeed == TimeSpeed.Pause)
                {
                    UpdateSpeed(previousSpeed);
                }
                else
                {
                    previousSpeed = currentSpeed;
                    UpdateSpeed(TimeSpeed.Pause);
                }
            }

            if (Input.GetKeyDown("1"))
            {
                previousSpeed = currentSpeed;
                UpdateSpeed(TimeSpeed.Normal);
            }

            if (Input.GetKeyDown("2"))
            {
                previousSpeed = currentSpeed;
                UpdateSpeed(TimeSpeed.Fast);
            }

            if (Input.GetKeyDown("3"))
            {
                previousSpeed = currentSpeed;
                UpdateSpeed(TimeSpeed.Fastest);
            }
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
                float interval = Registry.appState.Time.queries.currentTickInterval;
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

        public static GameWorldTimeSystemManager Find()
        {
            return GameObject.Find("TimeManager").GetComponent<GameWorldTimeSystemManager>();
        }
    }
}

