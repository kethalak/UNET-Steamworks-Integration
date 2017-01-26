using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamServerManager : MonoBehaviour {

	static public SteamServerManager _instance;
    
    bool gs_Initialized = false;
	
	[HideInInspector]
    public bool gs_ConnectedToSteam = false;

    protected Callback<SteamServersConnected_t> Callback_ServerConnected;

	void Start(){
		_instance = this;
        Callback_ServerConnected = Callback<SteamServersConnected_t>.CreateGameServer(OnSteamServerConnected);
	}

    public void CreateServer(){
		gs_Initialized = GameServer.Init(0, 8766, 27015, 27016, EServerMode.eServerModeNoAuthentication, "1.0");
		if (!gs_Initialized) {
			Debug.Log("SteamGameServer_Init call failed");
			return;
		}
		SteamGameServer.SetModDir("SPACEWARS");
        SteamGameServer.LogOnAnonymous();
    }

	void OnSteamServerConnected(SteamServersConnected_t pLogonSuccess) {
		Debug.Log("SPACEWARS connected to Steam successfully");
		gs_ConnectedToSteam = true;
        SteamAPICall_t try_toHost = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
	}

    void OnDisable(){
		if(gs_Initialized || gs_ConnectedToSteam){
		SteamGameServer.LogOff();
		GameServer.Shutdown();
		Debug.Log("Shutdown.");
		}
	}

    void Update(){
        if(!gs_Initialized) {
			return;
		}

		GameServer.RunCallbacks();
    }
}
