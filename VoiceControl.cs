using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceControl : MonoBehaviour
{
    private Dictionary<string, Action> keyActs = new Dictionary<string, Action>();
    private KeywordRecognizer recognizer;

    private MeshRenderer cubeRend;

    private bool spinningRight;

    private AudioSource soundSource;
    public AudioClip[] sounds;

    void Start()
    {
        cubeRend = GetComponent<MeshRenderer>();
        soundSource = GetComponent<AudioSource>();
        Transform myTransform = transform;
        Vector3 myPosition = transform.position;

        keyActs.Add("red", Red);
        keyActs.Add("green", Green);
        keyActs.Add("blue", Blue);
        keyActs.Add("white", White);

        keyActs.Add("spin right", SpinRight);
        keyActs.Add("spin left", SpinLeft);

        keyActs.Add("please say something", Talk);

        keyActs.Add("pizza is a wonderful food that makes me very happy", FactAcknowledgement);

        keyActs.Add("destroy cube", Destroy);

        keyActs.Add("open link", OpenURL);

        keyActs.Add("move cube", Move);

        keyActs.Add("create new cube", Create);

        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
        recognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Command: " + args.text);
        keyActs[args.text].Invoke();
    }

    void Red()
    {
        cubeRend.material.SetColor("_Color", Color.red);
    }

    void Green()
    {
        cubeRend.material.SetColor("_Color", Color.green);
    }

    void Blue()
    {
        cubeRend.material.SetColor("_Color", Color.blue);
    }

    void White()
    {
        cubeRend.material.SetColor("_Color", Color.white);
    }

    void SpinRight()
    {
        spinningRight = true;
        StartCoroutine(RotateObject(1f));
    }

    void SpinLeft()
    {
        spinningRight = false;
        StartCoroutine(RotateObject(1f));
    }

    private IEnumerator RotateObject(float duration)
    {
        float startRot = transform.eulerAngles.x;
        float endRot;

        if (spinningRight)
            endRot = startRot - 360f;
        else
            endRot = startRot + 360f;

        float t = 0f;
        float yRot;

        while (t < duration)
        {
            t += Time.deltaTime;
            yRot = Mathf.Lerp(startRot, endRot, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRot, transform.eulerAngles.z);
            yield return null;
        }
    }

    void Talk()
    {
        soundSource.clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        soundSource.Play();
    }

    void FactAcknowledgement()
    {
        Debug.Log("How right you are.");
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    void OpenURL()
    {
        Application.OpenURL("http://unity3d.com/");
    }

    void Move()
    {
        transform.position = new Vector3(0, 0, 2) * Time.deltaTime;
    }

    void Create()
    {
        GameObject objToSpawn;

        objToSpawn = new GameObject();
        objToSpawn.AddComponent<Rigidbody>();
        objToSpawn.AddComponent<MeshFilter>();
        objToSpawn.AddComponent<BoxCollider>();
        objToSpawn.AddComponent<MeshRenderer>();
    }
}
