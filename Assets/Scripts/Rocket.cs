using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private bool collisionOff = false;
    
    enum RocketGameState { Playing, Dead, StartNextLevel }

    private RocketGameState state = RocketGameState.Playing;

    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float flySpeed = 100f;
    [SerializeField] private AudioClip flySound;
    [SerializeField] private AudioClip finishSound;
    [SerializeField] private AudioClip deadSound;
    [SerializeField] private ParticleSystem boomParticle;
    [SerializeField] private ParticleSystem flyParticle;
    [SerializeField] private ParticleSystem finishParticle;
    [SerializeField] private float energyApply = 50f;
    [SerializeField] private int energyTotal = 2000;
    [SerializeField] private int energyToAdd = 500;
    [SerializeField] private Text energyText; 

    void Start()
    {
        energyText.text = energyTotal.ToString();
        state = RocketGameState.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == RocketGameState.Playing)
        {
            StartMove();
            MoveToLeftRigth();
        }
        if(Debug.isDebugBuild)
         DebugKeys();
    }

    void DebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
            LoadNextLevel();
        else if (Input.GetKeyDown(KeyCode.C))
            collisionOff = !collisionOff;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != RocketGameState.Playing || collisionOff) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("friend");
                break;
            case "Battery":
                PlusEnergy(energyToAdd, collision.gameObject);
                break;
            case "Finish":
                Finish();
                break;
            default:
                Lose();
                break;
        }
    }

    void PlusEnergy(int energyToAdd, GameObject batteryObj)
    {
        energyTotal += energyToAdd;
        energyText.text = energyTotal.ToString();
        Destroy(batteryObj);
    }

    void Lose()
    {
        state = RocketGameState.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(deadSound);
        boomParticle.Play();
        Invoke("LoadActiveLevel", 1.5f);
        Invoke("HideRocket", 0.7f);
    }

    void HideRocket()
    {
        gameObject.SetActive(false);
    }
    void Finish()
    {
        state = RocketGameState.StartNextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticle.Play();
        Invoke("LoadNextLevel", 1.5F);
        Invoke("HideRocket", 0.7f);

    }
    void LoadNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
            nextLevelIndex = 1;
        SceneManager.LoadScene(nextLevelIndex);
    }
    void LoadActiveLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void StartMove()
    {
        if (Input.GetKey(KeyCode.Space) && energyTotal > 0)
        {
            energyTotal -= Mathf.RoundToInt(energyApply * Time.deltaTime);
            energyText.text = energyTotal.ToString();
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(flySound);
            flyParticle.Play();
        }
        else
        {
            audioSource.Pause();
            flyParticle.Stop();
        }

    }
    void MoveToLeftRigth()
    {
        float rotationSpeed = rotateSpeed * Time.deltaTime;
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward * rotationSpeed);

        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(-Vector3.forward * rotationSpeed);

        rigidBody.freezeRotation = false;
    }
}
