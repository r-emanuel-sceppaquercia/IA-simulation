using DG.Tweening;
using UnityEngine;

public class HunterView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject campfire;
    [SerializeField] private GameObject exclamationSign;

    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private GameObject body;
    private Material bodyMaterial;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bodyMaterial = body.GetComponent<MeshRenderer>().material;
    }

    public void ChangeMaterialColor(Color color) => bodyMaterial.color = color;
    public void SetupCampfire(bool turnOn) => campfire.SetActive(turnOn);
    public void TurnOnExclamation(bool turnOn) => exclamationSign.SetActive(turnOn);


    public void RestAnimation(bool isResting)
    {
        animator.SetBool("isResting", isResting);
        DOVirtual.DelayedCall(0.5f, () => campfire.SetActive(isResting));
    }

    public void HuntingAnimation(bool isHunting)
    {
        animator.SetBool("isHunting", isHunting);
        exclamationSign.SetActive(isHunting);
    }
    public void ShootAnimation()
    {
        //animator.SetTrigger("shoot");
        print("Playing particle system fire effect");
        muzzleFlash.Play();
    }
    public void RechargeAnimation()
    {
        animator.SetTrigger("recharge");
    }
}
