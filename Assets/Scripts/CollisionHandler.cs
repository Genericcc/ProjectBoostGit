using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip deathExploasion;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] float loadDelay = 1f;

    AudioSource aS;
        
    bool isTransitioning = false;
    bool cheatOn = false;
    
    void Start() {
        aS = GetComponent<AudioSource>();
    }

    void Update() {
        CheatHandler();
        SkipLevel();
    }

    void CheatHandler() {
        if(Input.GetKeyDown(KeyCode.C)) {
            cheatOn = !cheatOn;
            Debug.Log("Cheats are: " + cheatOn);
        }
    }  

    void SkipLevel() {
        if(Input.GetKeyDown(KeyCode.L)) {
            StartSuccessSequence();
        }
    }    

    void OnCollisionEnter(Collision other) {
        if (isTransitioning || cheatOn) { return; }
        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence(); 
                break;
        }
    }

    void StartSuccessSequence() {
        isTransitioning = true;
        aS.Stop();
        aS.PlayOneShot(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadDelay + 0.5f);
    }

    void StartCrashSequence() {
        isTransitioning = true;
        aS.Stop();
        aS.PlayOneShot(deathExploasion);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //int nextSceneIndex = currentSceneIndex + 1;
        if (currentSceneIndex+1 != SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(currentSceneIndex+1);
        }
        else { 
            SceneManager.LoadScene(0);
        }
    }
}
