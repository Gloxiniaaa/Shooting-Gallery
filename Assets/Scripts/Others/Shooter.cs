using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bulletText;
    [SerializeField] private int _bulletAmount;
    private int _numBulletRemains;
    private bool _canShoot = false;
    private Camera _camera;
    [Header("SFX")]
    [SerializeField] private AudioGroupSO _gunReloadSfx;
    [SerializeField] private AudioGroupSO _shootSfx;
    [SerializeField] private AudioGroupSO _outOfBulletSfx;


    [Header("Broadcast on channel:")]
    [SerializeField] private AudioEventChannelSO SfxChannel;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO StartGameEvent;
    [SerializeField] private VoidEventChannelSO TimesUpEvent;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _numBulletRemains = _bulletAmount;
        _bulletText.text = _numBulletRemains.ToString();
        StartGameEvent.OnEventRaised += AlowShooting;
        TimesUpEvent.OnEventRaised += StopShooting;
    }


    private void AlowShooting()
    {
        _canShoot = true;
    }

    private void StopShooting()
    {
        _canShoot = false;
    }

    private void Update()
    {
        // match crosshair pos with mouse pos
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (_numBulletRemains <= 0 || !_canShoot)
        {
            SfxChannel.RaiseEvent(_outOfBulletSfx);
            return;
        }

        SfxChannel.RaiseEvent(_shootSfx);
        _numBulletRemains--;
        _bulletText.text = _numBulletRemains.ToString();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            Target target = hit.collider.gameObject.GetComponent<Target>() as Target;
            if (target)
            {
                target.OnShot();
            }
        }
    }

    private void Reload()
    {
        if (_numBulletRemains < _bulletAmount)
        {
            SfxChannel.RaiseEvent(_gunReloadSfx);
            _numBulletRemains = _bulletAmount;
            _bulletText.text = _numBulletRemains.ToString();
        }
    }

    private void OnDisable()
    {
        StartGameEvent.OnEventRaised -= AlowShooting;
        TimesUpEvent.OnEventRaised -= StopShooting;
    }
}