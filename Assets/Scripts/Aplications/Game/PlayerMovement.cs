using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    public float time;
    public GameObject parent;


    public float angle;
    public float endValue;
    public float jumpTime;
    public bool jump;
    public bool isStart;
    
    private void Start()
    {
        transform.position = new Vector3(-5, transform.position.y, transform.position.z);
        FactoryEventServices.GameAction.PlayGame += Play;
        FactoryEventServices.GameAction.StopGame += Stop;
        FactoryEventServices.GameAction.PauseGame += PauseGame;
    }

    private void OnDestroy()
    {
        FactoryEventServices.GameAction.PlayGame -= Play;
        FactoryEventServices.GameAction.StopGame -= Stop;
        FactoryEventServices.GameAction.PauseGame -= PauseGame;
    }
    private void Play()
    {
        isStart = true;
        AnimControl("Run", true);
        anim.speed = 1;
    }
    private void Stop()
    {
        isStart = false;
    }

    void Update()
    {
        if (!isStart)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.DOMoveX(-1 * 5, time);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.DOMoveX(1 * 5, time);
        }

        if (Input.GetKeyDown(KeyCode.W) && jump == false)
        {
            AnimControlTrigger("Jump");
            jump = true;
            DOTween.To(() => angle, x => angle = x, endValue, jumpTime)
                .OnUpdate(() =>
                {
                    transform.position = new Vector3(transform.position.x, angle, transform.position.z);
                })
                .OnKill(() => { })
                .OnComplete(() =>
                {
                    angle = endValue;
                    DOTween.To(() => angle, x => angle = x, 1, jumpTime)
                        .OnUpdate(() =>
                        {
                            transform.position = new Vector3(transform.position.x, angle, transform.position.z);
                        }).OnComplete((() =>
                        {
                            jump = false;
                            angle = 1;
                        }));
                });
        }

        parent.transform.localRotation = new Quaternion(0, 0, 0, 0);
        parent.transform.localPosition = Vector3.zero;
    }


    public void AnimControl(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    public void AnimControlTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Debug.Log("finish Game");
            AnimControl("Die", true);
            FactoryEventServices.GameAction.StopGame?.Invoke();
            FactoryEventServices.isPlay = false;
        }

        if (other.CompareTag("Coin"))
        {
            PlayerPrefs.SetInt(FactoryEnum.Coin.ToString(), PlayerPrefs.GetInt(FactoryEnum.Coin.ToString()) + 10);
            FactoryEventServices.GameAction.BuyCoin?.Invoke();
        }
    }

    public void PauseGame()
    {
        anim.speed = 0;
    }
}