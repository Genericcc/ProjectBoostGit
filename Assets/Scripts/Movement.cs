using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip sideEngine;
    [SerializeField] ParticleSystem thrusterLeft;
    [SerializeField] ParticleSystem thrusterRight;
    [SerializeField] ParticleSystem thrusterMain;
    [SerializeField] float rotationThrust = 80f;
    [SerializeField] float mainThrust = 1000f;

    Rigidbody rb;
    AudioSource aS;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aS = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        ProcessSound();
    }

    void ProcessThrust() {
        if(Input.GetKey(KeyCode.Space)) {
            StartThrusting();
        }
        else {
            StopThrusting();
        }
    }

    void StartThrusting() {
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust);
        if (!thrusterMain.isPlaying) {
            thrusterMain.Play();
        }
    }

    void StopThrusting() {
        thrusterMain.Stop();
    }

    void ProcessRotation() {
        RotateLeft();
        RotateRight();
    }

    void RotateLeft() {
        if (Input.GetKey(KeyCode.D)) { 
            ApplyRotation(-rotationThrust);
            if (!thrusterLeft.isPlaying) {
                thrusterLeft.Play();
            }
        }
        else {
            thrusterLeft.Stop();
        }
    }

    void RotateRight() {
        if (Input.GetKey(KeyCode.A)) {
            ApplyRotation(rotationThrust);
            if (!thrusterRight.isPlaying) {
                thrusterRight.Play();
            }
        }
        else {
            thrusterRight.Stop();
        }
    }

    public void ApplyRotation(float rotationThisFrame) {
        rb.freezeRotation = true; //freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        //or: transform.Rotate(0 ,0 ,rotationThisFrame * Time.deltaTime); 
        rb.freezeRotation = false;
    }

    void ProcessSound() {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Space)) {
            if (!aS.isPlaying) {
                aS.PlayOneShot(mainEngine, 0.4f);
            }
        }
        else {
            aS.Stop();
        }
    }
}
