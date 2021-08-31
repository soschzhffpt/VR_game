using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public Transform PlayerPosition;
    // 플레이어의 현재 위치를 저장하는 변수

    public GameObject[] Map = new GameObject[3];
    // 무한으로 생성할 3가지 맵을 저장하는 변수

    private int SpawnedMap;
    // 현재 생성된 맵을 저장하는 변수

    // Use this for initialization
    void Start()
    {
        SpawnedMap = 3;
        // 현재 생성된 맵의 수는 3개이기 때문에 3으로 초기화한다.
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPosition.position.z >= 50 * (SpawnedMap - 2))
        // 플레이어의 z값이 50 * 생성된 맵의 수 - 2의 위치일 경우
        // 생성된 맵 3개 기준 50 * (3 - 2) = 50, 플레이어가 달리면서 z값이 50 이상 될 경우
        {
            Vector3 NextSpawn = new Vector3(0, 0, 50 * SpawnedMap);
            // 다음 생성될 맵의 위치를 저장하는 변수

            Instantiate(Map[Random.Range(0, 3)], NextSpawn, transform.rotation);
            // Map1~3 배열 기준 0이상 3미만 중 랜덤하게 하나를 NextSpawn 위치에 생성한다.

            SpawnedMap++;
            // 현재 생성된 맵의 수를 1 증가한다.
        }
    }
}
