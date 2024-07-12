using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("Parallax Controller")]
public class ParallaxController : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField, Tooltip("Loop on axis Y")]
    bool m_infiniteVertical;
    [SerializeField, Tooltip("Loop on axis X")]
    bool m_infiniteHorizontal;
    
    [SerializeField, Range(0f, 1f), Tooltip("Select parallax speed"), Space]
    float m_parallaxEffect;

    float m_textureUnitSizeX, m_textureUnitSizeY;

    Camera m_camera;
    Vector3 m_lastCameraPosition;

    void Start()
    {
        m_camera = Camera.main;
        m_lastCameraPosition = m_camera.transform.position;
        
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.drawMode = SpriteDrawMode.Tiled;

        Sprite sprite = renderer.sprite;
        Texture2D texture = sprite.texture;
        m_textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        m_textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = m_camera.transform.position - m_lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * m_parallaxEffect, deltaMovement.y * m_parallaxEffect);
        m_lastCameraPosition = m_camera.transform.position;

        if (m_infiniteHorizontal)
        {
            if (Mathf.Abs(m_camera.transform.position.x - transform.position.x) >= m_textureUnitSizeX)
            {
                float offsetPositionX = (m_camera.transform.position.x - transform.position.x) % m_textureUnitSizeX;
                transform.position = new Vector3(m_camera.transform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (m_infiniteVertical)
        {
            if (Mathf.Abs(m_camera.transform.position.y - transform.position.y) >= m_textureUnitSizeY)
            {
                float offsetPositionY = (m_camera.transform.position.y - transform.position.y) % m_textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, m_camera.transform.position.y + offsetPositionY);
            }
        }
    }
}
