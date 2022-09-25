using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;



public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject gummyPrefab;
    public GameObject peppermintPrefab;
    public GameObject cubePrefab;
    private GameObject gummyPlayer;
    private GameObject peppermintPlayer;
    int playersSpawned = 0;
    int help = 0;

    #region GameManager Singleton
    static private GameManager gm; //refence GameManager
    static public GameManager GM { get { return gm; } } //public access to read only gm 


    //Check to make sure only one gm of the GameManager is in the scene
    void CheckGameManagerIsInScene()
    {

        //Check if instnace is null
        if (gm == null)
        {
            gm = this; //set gm to this gm of the game object
            Debug.Log(gm);
        }
        else //else if gm is not null a Game Manager must already exsist
        {
            Destroy(this.gameObject); //In this case you need to delete this gm
            Debug.Log("Game Manager exists. Deleting...");
        }
        Debug.Log(gm);
        DontDestroyOnLoad(this.gameObject);
    }//end CheckGameManagerIsInScene()
    #endregion

    #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }

    #endregion


    #region Public Methods

    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion

    void Awake()
    {
        CheckGameManagerIsInScene();
    }

    private void Start()
    {
        if (BasicPlayerMovement.LocalPlayerInstance == null)
        {
            Debug.Log("help " + help);
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            //if (gummyPlayer == null)
            //{
            //    Debug.Log("Instantiating Gummy Bear");
            //    gummyPlayer = PhotonNetwork.Instantiate(this.gummyPrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
            //}
            //else if (peppermintPlayer == null)
            //{
            //    Debug.Log("Instantiating Peppermint");
            //    peppermintPlayer = PhotonNetwork.Instantiate(this.gummyPrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
            //}
            //else
            //{
            //    PhotonNetwork.Instantiate(this.cubePrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
            //}

            //we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            if (GameObject.Find("Player 1") == null)
            {
                Debug.Log("Instantiating Gummy Bear");
                gummyPlayer = PhotonNetwork.Instantiate(this.gummyPrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
                gummyPlayer.name = "Player 1";
            }
            else if (GameObject.Find("Player 2") == null)
            {
                Debug.Log("Instantiating Peppermint");
                peppermintPlayer = PhotonNetwork.Instantiate(this.gummyPrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
                peppermintPlayer.name = "Player 2";
            }
            else
            {
                PhotonNetwork.Instantiate(this.cubePrefab.name, new Vector2(0f, 5f), Quaternion.identity, 0);
            }
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    #region Private Methods


    void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for 2");
        }


        #endregion

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}