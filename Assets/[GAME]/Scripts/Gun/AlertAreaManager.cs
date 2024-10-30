using System.Collections.Generic;
using System.Linq;
using _BASE_.Scripts.Extensions;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _GAME_.Scripts.Gun
{
    public class AlertAreaManager : Singleton<AlertAreaManager>
    {
        public AlertArea[] alertAreas;

        [ShowInInspector, ReadOnly]
        private List<AlertArea> _activeAlertAreas = new List<AlertArea>();

        [Button]
        public void SpawnAlertArea(SpawnAlertAreaData spawnAlertAreaData)
        {
            var foundAlertArea = alertAreas.FirstOrDefault(x => x.alertAreaType == spawnAlertAreaData.alertAreaType);

            if (foundAlertArea == null)
            {
                Debug.LogError("Alert Area not found");
                return;
            }

            var spawnedAlertArea = Instantiate(foundAlertArea, transform);
            
            var ray = new Ray(spawnAlertAreaData.position, Vector3.down);
            if (Physics.Raycast(ray, out var hit, 100))
            {
                hit.point += Vector3.up * 0.1f;
                spawnAlertAreaData.position = hit.point;
            }
            
            spawnedAlertArea.transform.position = spawnAlertAreaData.position;
            spawnedAlertArea.transform.rotation = Quaternion.Euler(90,0,0);
            _activeAlertAreas.Add(spawnedAlertArea);

            if (spawnAlertAreaData.delay > 0)
            {
                spawnedAlertArea.OpenAnim(new SpawnAlertAreaData(spawnAlertAreaData)).OnComplete(
                    () => OpenAlert(spawnAlertAreaData, spawnedAlertArea));

            }
            else
            {
                spawnedAlertArea.OpenAnim(new SpawnAlertAreaData(spawnAlertAreaData)).OnComplete(
                    () => NoDelayOpenAlert(spawnAlertAreaData, spawnedAlertArea));
            }
        }
        void NoDelayOpenAlert(SpawnAlertAreaData spawnAlertAreaData, AlertArea spawnedAlertArea)
        {
            spawnAlertAreaData.onSpawn?.Invoke();
            DOVirtual.DelayedCall(spawnAlertAreaData.duration, () =>
            {
                _activeAlertAreas.Remove(spawnedAlertArea);
                spawnedAlertArea.CloseAnim();
            });
        }
        void OpenAlert(SpawnAlertAreaData spawnAlertAreaData, AlertArea spawnedAlertArea)
        {
            DOVirtual.DelayedCall(spawnAlertAreaData.delay, () =>
            {
                spawnedAlertArea.OpenAnim(new SpawnAlertAreaData(spawnAlertAreaData));
                spawnAlertAreaData.onSpawn?.Invoke();
                DOVirtual.DelayedCall(spawnAlertAreaData.duration, () =>
                {
                    _activeAlertAreas.Remove(spawnedAlertArea);
                    spawnedAlertArea.CloseAnim();
                });
            });
        }
    }
}