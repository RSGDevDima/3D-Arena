using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float _fireRate = 20f;
    [SerializeField] private float _shellSpeed = 10f;

    
    private Coroutine _shootProcess;

    private void Start() {
        _shellSpeed /= 10;
    }

    private void Update()
    {
        if (Input.touchCount == 0 && Input.GetMouseButtonDown(0))
            Shoot();
    }

    public void Shoot(){
        if(_shootProcess == null)
            _shootProcess = StartCoroutine(ShootSequence());
    }

    private IEnumerator ShootSequence(){
        Camera mainCamera = Camera.main;
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward.normalized / 10;

        GameObject shellObject = ObjectPooller.Current.GetPooledObject("PlayerShell");
        shellObject.transform.position = spawnPosition;
        shellObject.transform.rotation = mainCamera.transform.rotation;
        shellObject.SetActive(true);

        BombShell shellScript = shellObject.GetComponent<BombShell>();
        shellScript.MoveInDirection(mainCamera.transform.forward, _shellSpeed);

        yield return new WaitForSeconds(100 / _fireRate);
        _shootProcess = null;
    }

}
