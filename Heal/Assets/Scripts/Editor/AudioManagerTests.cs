using NUnit.Framework;
using UnityEngine;

public class AudioManagerTests
{
    private GameObject go;
    private AudioManager audioManager;

    [SetUp]
    public void Setup()
    {
        go = new GameObject();
        audioManager = go.AddComponent<AudioManager>();
        audioManager.StopMusic(); // Ensures clean state
    }

    [Test]
    public void MusicVolumeCanBeSet()
    {
        audioManager.SetMusicVolume(0.2f);
        Assert.AreEqual(0.2f, go.GetComponent<AudioSource>().volume);
    }

    [Test]
    public void SFXVolumeCanBeSet()
    {
        audioManager.SetSFXVolume(0.9f);
        // There will be a second AudioSource on this object
        AudioSource[] sources = go.GetComponents<AudioSource>();
        Assert.IsTrue(sources[1].volume == 0.9f || sources[0].volume == 0.9f);
    }
}
