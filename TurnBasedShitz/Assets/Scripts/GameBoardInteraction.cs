using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardInteraction : MonoBehaviour
{
    public float playerSpeed;
    public GameObject playerReference;
    private float Timer;
    private GameObject lastMouseOver;
    private List<BaseTileModel> currentPath = new List<BaseTileModel>();
    private int pathIndex = 0;
    private Vector3 nextPosition;
    private bool isPlayerMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region PlayerMovement
        if (isPlayerMoving == true)
        {
            Timer += Time.deltaTime * playerSpeed;
            if (playerReference.transform.position != GetPlayerEquivalent(nextPosition))
            {
                playerReference.transform.position = LerpAndStop(playerReference.transform.position, GetPlayerEquivalent(nextPosition), Timer);
            }
            else
            {
                if (pathIndex >= currentPath.Count - 1)
                {
                    isPlayerMoving = false;
                    pathIndex = 0;
                    this.GetComponent<GameBoardGenerator>().GenerateNextTiles(new Vector3(lastMouseOver.transform.position.x, -1, lastMouseOver.transform.position.z));
                    UnselectTiles();
                }
                else
                {
                    Timer = 0;
                    pathIndex++;
                    nextPosition = GetPlayerEquivalent(currentPath[pathIndex].tilePosition);
                }
            }
        }
        #endregion

        #region DetectMouseOverTile
        if(isPlayerMoving == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (lastMouseOver != hit.transform.gameObject)
                {
                    if (hit.transform.tag == "base")
                    {
                        if (lastMouseOver != hit.transform.gameObject && lastMouseOver != null)
                        {
                            lastMouseOver.GetComponent<BaseGroundBehaviour>().Unselect();
                            lastMouseOver = null;
                        }
                        else
                        {
                            if(hit.transform.gameObject.GetComponent<BaseGroundBehaviour>().myData.tileType != TileType.Water)
                            {
                                lastMouseOver = hit.transform.gameObject;
                                lastMouseOver.GetComponent<BaseGroundBehaviour>().Select();
                                UnselectTiles();
                                Pathfinder pf = new Pathfinder();
                                currentPath = pf.FindPath(this.gameObject.GetComponent<GameBoardGenerator>().GetGameBoard(), playerReference.transform.position, lastMouseOver.transform.position);
                                int childCount = 0;
                                childCount = this.gameObject.transform.childCount;
                                if (currentPath != null)
                                {
                                    foreach (BaseTileModel segment in currentPath)
                                    {
                                        for (int i = 0; i < childCount; i++)
                                        {
                                            BaseTileModel segmentTile = this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().myData;
                                            if (segmentTile.tilePosition == segment.tilePosition)
                                            {
                                                this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().Select();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lastMouseOver != null)
                        {
                            lastMouseOver.GetComponent<BaseGroundBehaviour>().Unselect();
                            lastMouseOver = null;
                        }
                    }
                }
            }
        }
        #endregion

        #region DetectMouseClick
        if(isPlayerMoving == false)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (lastMouseOver != null)
                {
                    if (playerReference.transform.position != GetPlayerEquivalent(lastMouseOver.transform.position))
                    {
                        if (currentPath != null && currentPath.Count >= pathIndex)
                        {
                            pathIndex = 0;
                            nextPosition = GetPlayerEquivalent(currentPath[pathIndex].tilePosition);
                            isPlayerMoving = true;
                        }
                    }
                }
            }
        }
        
        #endregion
    }

    private Vector3 LerpAndStop(Vector3 currentPos, Vector3 targetPos, float t)
    {
        return Vector3.Distance(currentPos, targetPos) <= t ? targetPos : Vector3.Lerp(currentPos, targetPos, t);
    }

    private void UnselectTiles()
    {
        if(currentPath != null)
        {
            currentPath.Clear();
            currentPath = new List<BaseTileModel>();
        }
        
        int childCount = 0;
        childCount = this.gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().Unselect();
        }
        
    }

    private Vector3 GetPlayerEquivalent(Vector3 tile)
    {
        return new Vector3(tile.x, playerReference.transform.position.y, tile.z);
    }
}
