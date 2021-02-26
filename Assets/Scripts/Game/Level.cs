using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Portals")]
    [SerializeField] private ReceivingPortal _receivingPortal;
    [SerializeField] private PortalTransmitter _portalTransmitter;
    [Header("CoinActivator")]
    [SerializeField] private GameObject _coins;
    [Header("DeactivationFinalLava")]
    [SerializeField] private FinalLava _finalLava;
    [Header("DeactivationFinalLadder")]
    [SerializeField] private GameObject _finalLadders;

    public ReceivingPortal ReceivingPortal => _receivingPortal;
    public PortalTransmitter PortalTransmitter => _portalTransmitter;
    public GameObject Coins => _coins;
    public FinalLava FinalLava => _finalLava;
    public GameObject FinalLadders => _finalLadders;
}