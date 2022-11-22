using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class GameWorldTimeSystemManager : MonoBehaviour
    {
        // Camera camera;

        IEnumerator timeCoroutine;

        TimeSpeed previousSpeed;

        void Awake()
        {
            // camera = Camera.main;
            // Registry.appState.Time.events.onTimeUpdated += OnTimeUpdated
        }

        void Start()
        {
            StartTick();
        }

        void Update()
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
    }
}

