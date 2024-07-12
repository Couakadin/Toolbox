using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Header("Camera Instancier")]
    [SerializeField]
    Camera m_camera;
    Vector3 m_cameraStartPosition;
    float m_distance;

    [Header("Background Instancier")]
    GameObject[] m_backgrounds;
    Material[] m_materials;
    float[] m_backgroundSpeeds;
    float m_farthestBackground;

    [Header("Parallax Settings")]
    [SerializeField, Range(.01f, .05f)]
    float m_parallaxSpeed;

    void Awake()
    {
        m_camera = m_camera ?? Camera.main;
        m_cameraStartPosition = m_camera.transform.position;

        BackgroundSpeedCalculate(BackgroundCount());
    }

    void LateUpdate()
    {
        m_distance = m_camera.transform.position.x - m_cameraStartPosition.x;
        transform.position = new Vector3(m_camera.transform.position.x, transform.position.y, transform.position.z);

        for (int i = 0; i < m_backgrounds.Length; i++)
        {
            float m_speed = m_backgroundSpeeds[i] * m_parallaxSpeed;
            m_materials[i].SetTextureOffset("_MainTex", new Vector2(m_distance, 0) * m_speed);
        }
    }

    int BackgroundCount()
    {
        int backgroundCount = transform.childCount;
        m_materials = new Material[backgroundCount];
        m_backgroundSpeeds = new float[backgroundCount];
        m_backgrounds = new GameObject[backgroundCount];

        for (int i = 0; i < backgroundCount; i++)
        {
            m_backgrounds[i] = transform.GetChild(i).gameObject;
            m_materials[i] = m_backgrounds[i].GetComponent<Renderer>().material;
        }

        return backgroundCount;
    }

    void BackgroundSpeedCalculate(int _backgroundCount)
    {
        for (int i = 0; i < _backgroundCount; i++)
        {
            if ((m_backgrounds[i].transform.position.z - m_camera.transform.position.z) > m_farthestBackground)
                m_farthestBackground = m_backgrounds[i].transform.position.z - m_camera.transform.position.z;
        }

        for (int i = 0; i < _backgroundCount; i++)
        {
            m_backgroundSpeeds[i] = 1 - (m_backgrounds[i].transform.position.z - m_camera.transform.position.z) / m_farthestBackground;
        }
    }
}
