using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleportation : MonoBehaviour
{
    [SerializeField] protected Player _player;
    [SerializeField] protected Portal[] _portals;

    private Portal _currentPortal;
    private int _currentPortalNumber;
    private bool _isShipPortal;

    public event UnityAction PlayerTeleportedToLevel;
    public event UnityAction PlayerTeleportedToSpaceShip;

    private void Start()
    {
        if (_portals.Length > 1)
            _isShipPortal = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _currentPortal = _portals[_currentPortalNumber];
            StartCoroutine(Teleport(_currentPortal));
            _currentPortalNumber += 1;

            if (_currentPortalNumber == _portals.Length)
                _currentPortalNumber = 0;

            if (_isShipPortal)
                PlayerTeleportedToLevel?.Invoke();
            else
                PlayerTeleportedToSpaceShip?.Invoke();
        }
    }

    private IEnumerator Teleport(Portal portal)
    {
        yield return new WaitForSeconds(0.5f);
        _player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);
    }
}