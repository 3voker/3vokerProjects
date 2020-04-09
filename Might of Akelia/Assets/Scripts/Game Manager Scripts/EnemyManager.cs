using System;
using UnityEngine;

[Serializable]
public class EnemyManager
{
    public Color m_PlayerColor;
    public Transform m_SpawnPoint;
    [HideInInspector]
    public int enemy_Number;
    [HideInInspector]
    public string m_ColoredPlayerText;
    [HideInInspector]
    public GameObject enemy_Instance;
    [HideInInspector]
    public int m_Wins;


    private EnemyController enemy_Movement;
    private SpellCaster enemy_Shooting;
    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        enemy_Movement = enemy_Instance.GetComponent<EnemyController>();
        enemy_Shooting = enemy_Instance.GetComponent<SpellCaster>();
        m_CanvasGameObject = enemy_Instance.GetComponentInChildren<Canvas>().gameObject;

        enemy_Movement.m_EnemyMovementNumber = enemy_Number;
        enemy_Shooting.caster_Number = enemy_Number;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + enemy_Number + "</color>";

        MeshRenderer[] renderers = enemy_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public void DisableControl()
    {
        enemy_Movement.enabled = false;
        enemy_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        enemy_Movement.enabled = true;
        enemy_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        enemy_Instance.transform.position = m_SpawnPoint.position;
        enemy_Instance.transform.rotation = m_SpawnPoint.rotation;

        enemy_Instance.SetActive(false);
        enemy_Instance.SetActive(true);
    }
}
