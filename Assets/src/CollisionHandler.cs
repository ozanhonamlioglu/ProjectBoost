using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 2f;
    [SerializeField] private AudioClip success = default;
    [SerializeField] private AudioClip fail = default;
    [SerializeField] private ParticleSystem successParticle = default;
    [SerializeField] private ParticleSystem crashParticle = default;

    private AudioSource audioSource;
    private bool onSuccessOrFail = false;
    private Movement _movement;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _movement = GetComponent<Movement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("this is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        if(!onSuccessOrFail)
        {
            audioSource.Stop(); // stop other sources
            audioSource.PlayOneShot(success);
            successParticle.Play();
            onSuccessOrFail = true;
        };
        _movement.enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartCrashSequence()
    {
        if(!onSuccessOrFail)
        {
            audioSource.Stop(); // stop other sources
            audioSource.PlayOneShot(fail);
            crashParticle.Play();
            
            _movement.directParticle.Stop();
            _movement.leftSideParticle.Stop();
            _movement.rightSideParticle.Stop();
            
            onSuccessOrFail = true;
        };
        _movement.enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }
}
