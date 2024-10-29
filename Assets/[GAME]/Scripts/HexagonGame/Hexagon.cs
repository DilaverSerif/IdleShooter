using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME_.Scripts.HexagonGame
{
    public class Hexagon : MonoBehaviour
    {
        private HexagonHealth _hexagonHealth;
        private Transform HexagonTransform => _hexagonHealth.transform;
        [SerializeField]
        private WeaponTrigger weaponTrigger;
        [SerializeField]
        private TextMeshPro healthText,levelText;
        [SerializeField]
        private Transform weaponPoint;
        [SerializeField]
        private float baseScaleY;

        public int HexagonLevel;
        private void Awake()
        {
            _hexagonHealth = GetComponentInChildren<HexagonHealth>();
        }

        private void OnEnable()
        {
            SetHealthText();
            SetScaleHexagon();
            SetPositionWeaponTrigger();
            
            _hexagonHealth.OnDie += Die;
            _hexagonHealth.OnHit += Hit;
        }
        
        private void OnDisable()
        {
            _hexagonHealth.OnDie -= Die;
            _hexagonHealth.OnHit -= Hit;
        }
        
        private void Start()
        {
            weaponTrigger.OpenTrigger(false);
            levelText.text = $"Lv.{HexagonLevel}";
        }
        
        private void SetHealthText()
        {
            healthText.text = _hexagonHealth.GetCurrentHealth().ToString("F2");
        }
        private void SetScaleHexagon()
        {
            var localScale = HexagonTransform.localScale;
            localScale.y = GetHexagonScale();
            HexagonTransform.localScale = localScale;
            
            _effectPlaying = false;
        }
        private void SetPositionWeaponTrigger()
        {
            weaponTrigger.transform.position = weaponPoint.position;
        }
        
        private float GetHexagonScale()
        {
            return _hexagonHealth.GetCurrentHealth() * GameSettings.Instance.gameOptions.hexagonScaleMultiplier + baseScaleY;
        }
    
        private void Die()
        {
            weaponTrigger.OpenTrigger();
        }

        private bool _effectPlaying;
        private void Hit()
        {

            if (_effectPlaying)
            {
                HexagonTransform.DOKill(true);
                
                HexagonTransform.DOScaleY(GetHexagonScale(), .1F);
                
                _effectPlaying = false;
            }
            else
            {
                _effectPlaying = true;
                HexagonTransform.DOPunchScale(new Vector3(0, Random.Range(.1f,.25f), 0), 0.25f, 1, 0.5f)
                    .OnComplete(SetScaleHexagon); 
            }
            
            SetPositionWeaponTrigger();
            SetHealthText();
        }
    }
}