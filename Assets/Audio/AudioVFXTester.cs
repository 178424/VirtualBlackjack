using UnityEngine;

/// <summary>
/// Attach to any GameObject in the scene.
/// In Play Mode, right-click this component in the Inspector → choose any "Test X" option.
/// Lets you test every audio/VFX effect without needing VR, UI, or the full game running.
/// </summary>
public class AudioVFXTester : MonoBehaviour
{
    [ContextMenu("Test / Card Deal Sound + Effect")]
    void TestCardDeal()
    {
        AudioManager.Instance?.PlayCardDeal();
        VFXManager.Instance?.PlayDealEffect();
        Debug.Log("[AudioVFXTester] Card Deal");
    }

    [ContextMenu("Test / Card Flip Sound")]
    void TestCardFlip()
    {
        AudioManager.Instance?.PlayCardFlip();
        Debug.Log("[AudioVFXTester] Card Flip");
    }

    [ContextMenu("Test / Button Click")]
    void TestButtonClick()
    {
        AudioManager.Instance?.PlayButtonClick();
        Debug.Log("[AudioVFXTester] Button Click");
    }

    [ContextMenu("Test / Chip Place Sound")]
    void TestChipPlace()
    {
        AudioManager.Instance?.PlayChipPlace();
        Debug.Log("[AudioVFXTester] Chip Place");
    }

    [ContextMenu("Test / Win Sound + Effect")]
    void TestWin()
    {
        AudioManager.Instance?.PlayWin();
        VFXManager.Instance?.PlayWinEffect();
        Debug.Log("[AudioVFXTester] Win");
    }

    [ContextMenu("Test / Blackjack Sound + Effect")]
    void TestBlackjack()
    {
        AudioManager.Instance?.PlayBlackjack();
        VFXManager.Instance?.PlayBlackjackEffect();
        Debug.Log("[AudioVFXTester] Blackjack");
    }

    [ContextMenu("Test / Lose Sound")]
    void TestLose()
    {
        AudioManager.Instance?.PlayLose();
        Debug.Log("[AudioVFXTester] Lose");
    }

    [ContextMenu("Test / Bust Sound + Effect")]
    void TestBust()
    {
        AudioManager.Instance?.PlayBust();
        VFXManager.Instance?.PlayBustEffect();
        Debug.Log("[AudioVFXTester] Bust");
    }

    [ContextMenu("Test / Push Sound + Effect")]
    void TestPush()
    {
        AudioManager.Instance?.PlayPush();
        VFXManager.Instance?.PlayPushEffect();
        Debug.Log("[AudioVFXTester] Push");
    }

    [ContextMenu("Test / Double Down Sound")]
    void TestDoubleDown()
    {
        AudioManager.Instance?.PlayDoubleDown();
        Debug.Log("[AudioVFXTester] Double Down");
    }
}
