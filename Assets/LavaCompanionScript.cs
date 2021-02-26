using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LavaCompanionScript : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer)
        {
            _spriteRenderer.sharedMaterial.SetVector("_RenderingBoundSize", _spriteRenderer.size * transform.localScale);
        }
    }
}
