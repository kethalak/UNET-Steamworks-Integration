using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 
using UnityEngine.UI;
using Steamworks;

public class SteamNetworkManager : NetworkManager {
    public Text createMatch;
    public Text findMatch;
    public Text awaitMsg;

	private Callback<LobbyCreated_t> Callback_lobbyCreated;
	private Callback<LobbyEnter_t> Callback_lobbyEnter;
	private Callback<LobbyMatchList_t> Callback_lobbyList;

	bool isHost = false;

	void Start(){
		Callback_lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
		Callback_lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
		Callback_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);

        if (SteamAPI.Init())
            Debug.Log("Steam API init -- SUCCESS!");
        else
            Debug.Log("Steam API init -- failure ...");
	}

    public void CreateMatch(){
		awaitMsg.text = "Creating Match...";
		ToggleMenu();
        SteamServerManager._instance.CreateServer();
    }

    public void FindMatch(){
		ToggleMenu();
        awaitMsg.text = "Finding Match...";
    }

	void ToggleMenu(){
		findMatch.gameObject.SetActive(!findMatch.gameObject.activeSelf);
		createMatch.gameObject.SetActive(!createMatch.gameObject.activeSelf);
	}

    void OnLobbyCreated(LobbyCreated_t result){

        if (result.m_eResult == EResult.k_EResultOK)
            Debug.Log("Lobby created -- SUCCESS!");
        else{
            Debug.Log("Lobby created -- failure ...");
            return;
        }

        uint serverIp = SteamGameServer.GetPublicIP();
        int ipaddr = System.Net.IPAddress.HostToNetworkOrder((int)serverIp);
        string ip = new System.Net.IPAddress(BitConverter.GetBytes(ipaddr)).ToString();

		SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "ServerIP", ip);

        isHost = true;

		awaitMsg.text = "";
        StartHost();
	}

    void OnLobbyEntered(LobbyEnter_t result){
		if(!isHost){
			networkAddress = SteamMatchmaking.GetLobbyData((CSteamID)result.m_ulSteamIDLobby, "ServerIP");
			awaitMsg.text = "";
			StartClient();
		}
	}

    void OnGetLobbiesList(LobbyMatchList_t result){
        for(int i=0; i < result.m_nLobbiesMatching; i++)
        {
			if(SteamMatchmaking.GetLobbyData((CSteamID)SteamMatchmaking.GetLobbyByIndex(i), "ServerIP") != null){
				SteamAPICall_t try_joinLobby = SteamMatchmaking.JoinLobby ((CSteamID)SteamMatchmaking.GetLobbyByIndex (i));
			}
        }
	}
}