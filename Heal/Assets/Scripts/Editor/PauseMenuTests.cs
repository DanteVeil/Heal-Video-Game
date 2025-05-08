using NUnit.Framework;
using UnityEngine;

public class PauseMenuTests
{
    private GameObject obj;
    private PauseMenu pauseMenu;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject();
        pauseMenu = obj.AddComponent<PauseMenu>();
        pauseMenu.pauseCanvas = new GameObject();
        pauseMenu.optionsCanvas = new GameObject();
    }

    [Test]
    public void CanOpenOptions()
    {
        pauseMenu.OpenOptions();
        Assert.IsTrue(pauseMenu.optionsCanvas.activeSelf);
        Assert.IsFalse(pauseMenu.pauseCanvas.activeSelf);
    }

    [Test]
    public void CanCloseOptions()
    {
        pauseMenu.CloseOptions();
        Assert.IsTrue(pauseMenu.pauseCanvas.activeSelf);
        Assert.IsFalse(pauseMenu.optionsCanvas.activeSelf);
    }
}
