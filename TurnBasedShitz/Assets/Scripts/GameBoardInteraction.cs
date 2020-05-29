using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameBoardInteraction : MonoBehaviour
{
    public float playerSpeed;
    public GameObject playerReference;
    private float Timer;
    private GameObject lastMouseOver;
    public List<BaseTileModel> currentPath = new List<BaseTileModel>();
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
                    // END OF MOVING
                }
                else
                {
                    Timer = 0;
                    pathIndex++;
                    nextPosition = GetPlayerEquivalent(currentPath[pathIndex].tilePosition);
                    this.GetComponent<GameBoardGenerator>().GenerateNextTiles(nextPosition);
                    this.GetComponent<GameBoardGenerator>().UnselectTileAt(currentPath[pathIndex-1].tilePosition);

                    GameObject currentTile = this.GetComponent<GameBoardGenerator>().GetBaseTileAt(nextPosition);
                    if(currentTile.GetComponent<BaseGroundBehaviour>().myData.item != null)
                    {
                        currentTile.GetComponent<BaseGroundBehaviour>().CollectItem();
                    }


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
                            if(hit.transform.gameObject.GetComponent<BaseGroundBehaviour>().myData != null)
                            {
                                if (hit.transform.gameObject.GetComponent<BaseGroundBehaviour>().myData.tileType != TileType.Water)
                                {
                                    lastMouseOver = hit.transform.gameObject;
                                    lastMouseOver.GetComponent<BaseGroundBehaviour>().Select();
                                    this.GetComponent<GameBoardGenerator>().UnselectAllTiles();
                                    Pathfinder pf = new Pathfinder();
                                    currentPath = pf.FindPath(this.gameObject.GetComponent<GameBoardGenerator>().GetGameBoard(), playerReference.transform.position, lastMouseOver.transform.position);
                                    
                                    if (currentPath != null)
                                    {
                                        this.GetComponent<GameBoardGenerator>().SelectPath();
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
                            this.GetComponent<GameBoardGenerator>().UnselectTileAt(nextPosition);
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

    private Vector3 GetPlayerEquivalent(Vector3 tile)
    {
        return new Vector3(tile.x, playerReference.transform.position.y, tile.z);
    }

    public void SendPlayerToPos(Vector3 pos)
    {
        playerReference.transform.position = GetPlayerEquivalent(pos);
    }

}
