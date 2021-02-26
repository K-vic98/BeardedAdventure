using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalTransmitter : MonoBehaviour
{
    [SerializeField] protected ReceivingPortal[] _portals;

    private Player _player;
    private ReceivingPortal _currentPortal;
    private int _currentPortalNumber;
    private AudioSource _audioEffect;

    public event UnityAction PlayerTeleportedToSpaceShip;

    private void Start()
    {
        _audioEffect = gameObject.GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _audioEffect.Play();
            _player = collision.GetComponent<Player>();
            _currentPortal = _portals[_currentPortalNumber];
            StartCoroutine(Teleport(_currentPortal));
            if (!gameObject.TryGetComponent<ShipPortal>(out ShipPortal shipPortal))
            {
                PlayerTeleportedToSpaceShip?.Invoke();
            }
            else
            {
                _currentPortalNumber += 1;
                if (_currentPortalNumber == _portals.Length)
                    _currentPortalNumber = 0;
            }
        }
    }

    private IEnumerator Teleport(ReceivingPortal portal)
    {
        yield return new WaitForSeconds(0.5f);
        _player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);
    }
}