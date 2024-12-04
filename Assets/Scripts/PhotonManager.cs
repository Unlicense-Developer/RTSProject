using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.NickName = "Hyunjun";

        Debug.Log(PhotonNetwork.SendRate);

        PhotonNetwork.ConnectUsingSettings();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            // 랜덤 룸에 접속.
            // 접속에 실패하면 OnJoinRandomFailed()이 실행되어 실패 알림.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // 서버 연결에 실패하면 서버에 연결 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤한 룸 입장이 실패했을 때 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Random Failed {returnCode}:{message}");

        // 룸의 속성 정의
        RoomOptions myRoomOption = new RoomOptions();
        myRoomOption.MaxPlayers = 8;     // 최대 접속자 수
        myRoomOption.IsOpen = true;       // 룸의 오픈 여부
        myRoomOption.IsVisible = true;    // 로비에서 룸을 노출시킬지 여부

        // 룸 생성
        PhotonNetwork.CreateRoom("My Room", myRoomOption);
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 룸에 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 출현 위치 정보를 배열에 저장
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        // 네트워크상에 캐릭터 생성
        PhotonNetwork.Instantiate("player", points[idx].position, points[idx].rotation, 0);
    }
}
