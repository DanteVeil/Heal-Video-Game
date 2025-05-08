using NUnit.Framework;
using UnityEngine;

public class AiHealthSystemTests
{
    private GameObject aiObject;
    private AiHealthSystem healthSystem;

    [SetUp]
    public void Setup()
    {
        aiObject = new GameObject();
        healthSystem = aiObject.AddComponent<AiHealthSystem>();
        aiObject.AddComponent<AiAgent>(); // Mock AiAgent required by script
        healthSystem.ResetHealth(); // Resets to maxHealth (default is 3)
    }

    [Test]
    public void TakesDamageCorrectly()
    {
        healthSystem.TakeDamage(1);
        Assert.AreEqual(2, healthSystem.GetCurrentHealth());
    }

    [Test]
    public void DiesAtZeroHealth()
    {
        healthSystem.TakeDamage(3);
        Assert.IsTrue(healthSystem.IsDead());
        Assert.AreEqual(0, healthSystem.GetCurrentHealth());
    }

    [Test]
    public void ResetsHealthCorrectly()
    {
        healthSystem.TakeDamage(2);
        healthSystem.ResetHealth();
        Assert.AreEqual(3, healthSystem.GetCurrentHealth());
        Assert.IsFalse(healthSystem.IsDead());
    }
}
