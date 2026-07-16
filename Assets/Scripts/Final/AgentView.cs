using UnityEngine;

public class AgentView : MonoBehaviour
{
    private Animator animator;
    private MeshRenderer[] renderers;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void ChangeColor(Color color)
    {
        if (renderers.Length == 0) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = color;
        }
    }
}
