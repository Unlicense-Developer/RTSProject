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
            // ���� �뿡 ����.
            // ���ӿ� �����ϸ� OnJoinRandomFailed()�� ����Ǿ� ���� �˸�.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // ���� ���ῡ �����ϸ� ������ ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }

    // ������ �� ������ �������� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Random Failed {returnCode}:{message}");

        // ���� �Ӽ� ����
        RoomOptions myRoomOption = new RoomOptions();
        myRoomOption.MaxPlayers = 8;     // �ִ� ������ ��
        myRoomOption.IsOpen = true;       // ���� ���� ����
        myRoomOption.IsVisible = true;    // �κ񿡼� ���� �����ų�� ����

        // �� ����
        PhotonNetwork.CreateRoom("My Room", myRoomOption);
    }

    // �� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // �뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // ���� ��ġ ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        // ��Ʈ��ũ�� ĳ���� ����
        PhotonNetwork.Instantiate("player", points[idx].position, points[idx].rotation, 0);
    }
}
