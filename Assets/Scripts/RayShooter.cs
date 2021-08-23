using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip hitWallSound;
    [SerializeField] private AudioClip hitEnemySound;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    target.ReactToHit();
                    soundSource.PlayOneShot(hitEnemySound);
                    Messenger.Broadcast(GameEvent.ENEMY_HIT);
                }
                else
                {
                    StartCoroutine(SphereIndicator(hit.point));
                    soundSource.PlayOneShot(hitWallSound);
                }
            }
        }
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(1); 
        Destroy(sphere);
    }

    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*"); 
    }
}
