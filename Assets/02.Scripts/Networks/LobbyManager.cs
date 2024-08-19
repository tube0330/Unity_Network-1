using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

/*마스터서버(Listen server)와 match making 룸 접속 담당*/

public class LobbyManager : MonoBehaviourPunCallbacks
{
    string gameVersion = "1.0";
    public Text connectionText; //네트워크 정보 표시
    public Button joinBtn;  //방 접속 버튼 방만들기 버튼

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();   //설정한 정보로 마스터 서버 접속 시도

        joinBtn.interactable = false;
        connectionText.text = "Connecting...";
    }

    //마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        joinBtn.interactable = true;
        connectionText.text = "Connected!";
    }

    //마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinBtn.interactable = false;
        connectionText.text = "Connection failed.";
    }

    //방 접속 시도, Join 버튼을 누를 때 호출 될 함수
    public void Connect()
    {
        joinBtn.interactable = false;   //중복접근을 막기 위해 비활성화

        if (PhotonNetwork.IsConnected)   //Master 서버에 접속 중
        {
            connectionText.text = "Connect to room...";
            PhotonNetwork.JoinRandomRoom();  //아무 방에 접속
        }

        else //Master 서버에 접속실패
        {
            connectionText.text = "Reconnecting...";  //재접속 시도
            PhotonNetwork.ConnectUsingSettings();   //Master 서버로의 재접속 시도
        }
    }

    //빈 방이 없어 랜덤 방 참가에 실패시 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionText.text = "Failed to join room. Creating room...";   //빈 방이 없음
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });     //생성된 룸 목록을 확인하는 기능은 만들지 않으므로 방의 이름은 입력하지 않고 null로 설정
        //생성된 방은 리슨서버 방식으로 동작하며 방을 생성한 클라이언트가 호스트 역할을 맡음
    }

    //방 참가 성공시 자동 실행
    public override void OnJoinedRoom()
    {
        connectionText.text = "Connected to room!";
        PhotonNetwork.LoadLevel("Main");    //모든 참가자가 Main scene을 로드
    }
}