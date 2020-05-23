using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameBoardMovement : MonoBehaviour
{
    public List<BaseTileModel> currentPath = new List<BaseTileModel>();

    public GameObject lastMouseOver;
    public GameObject playerReference;
    public float playerSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        #region DetectMouseOverTile
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if(lastMouseOver != hit.transform.gameObject)
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
                        lastMouseOver = hit.transform.gameObject;
                        lastMouseOver.GetComponent<BaseGroundBehaviour>().Select();
                        Vector3 playerPos = GetComponent<GlobalVariableHolder>().PLAYER_POS;
                        UnselectTiles();
                        Pathfinder pf = new Pathfinder();
                        currentPath = pf.FindPath(GetComponent<GlobalVariableHolder>(), playerPos, lastMouseOver.transform.position);
                        int childCount = 0;
                        childCount = this.gameObject.transform.childCount;
                        foreach(BaseTileModel segment in currentPath)
                        {
                            for(int i = 0; i < childCount; i++)
                            {
                                var t = this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().myData;
                                if(t.tilePosition == segment.tilePosition)
                                {
                                    this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().Select();
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
        #endregion

        #region DetectMouseClick

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(lastMouseOver != null)
            {
                MovePlayer();
                this.GetComponent<GameBoardGenerator>().GenerateNextTiles(new Vector3(lastMouseOver.transform.position.x, -1, lastMouseOver.transform.position.z));
            }
        }
        #endregion


    }

    public void MovePlayer()
    {
        foreach(var pathSegment in currentPath)
        {
            Vector3 nextPosition = new Vector3(pathSegment.tilePosition.x, playerReference.transform.position.y, pathSegment.tilePosition.z);
            while(Vector3.Distance(playerReference.transform.position, nextPosition) > 0f)
            {
                Debug.Log("trying my best diggah");
                Vector3 moveDir = (nextPosition - playerReference.transform.position).normalized;
                float distanceBefore = Vector3.Distance(playerReference.transform.position, nextPosition);
                playerReference.transform.position = playerReference.transform.position + moveDir * playerSpeed * Time.deltaTime;
            }

            this.GetComponent<GlobalVariableHolder>().PLAYER_POS = nextPosition;
        }
        UnselectTiles();
    }

    private void UnselectTiles()
    {
        currentPath.Clear();
        int childCount = 0;
        childCount = this.gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().Unselect();
        }
    }

}
