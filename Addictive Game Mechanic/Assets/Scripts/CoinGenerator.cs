using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public ObjectPooler coinPool; // coin pooler we are going to use to create/reuse coins

    // this is a function used to spawn two coins, randomly positioned on the platform
    public void SpawnCoins(Vector3 _startPos, float _platformWidth)
    {
        GameObject coinLeft = coinPool.GetPooledObject(); // get a coin and call it coinleft
        coinLeft.transform.position = new Vector3(_startPos.x - (_platformWidth * Random.Range(0.1f, 0.4f)), _startPos.y, _startPos.z); // set its Y and Z at respectable positions, then random X
        coinLeft.SetActive(true); // turn it on in the game world

        // repeat for the second coin but instead add the random X position to move it to the right
        GameObject coinRight = coinPool.GetPooledObject();
        coinRight.transform.position = new Vector3(_startPos.x + (_platformWidth * Random.Range(0.1f, 0.4f)), _startPos.y, _startPos.z);
        coinRight.SetActive(true);
    }
}
