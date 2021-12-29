using System;
using System.Collections.Generic;
using Boardgame;
using Common.States;
using Common.Ui;
using HarmonyLib;
using MelonLoader;
using Photon.Realtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoomFinder.UI
{
    internal class RoomListPanel
    {
        private readonly UiUtil uiUtil;
        private readonly GameObject panelObject;

        public static RoomListPanel NewInstance(UiUtil uiUtil)
        {
            return new RoomListPanel(uiUtil, new GameObject("RoomListPanel"));
        }

        private RoomListPanel(UiUtil uiUtil, GameObject panelObject)
        {
            this.uiUtil = uiUtil;
            this.panelObject = panelObject;
        }

        public GameObject Reinitialize(List<RoomInfo> rooms)
        {
            foreach (Transform child in panelObject.transform)
            {
                Object.Destroy(child.gameObject);
            }

            RenderHeader();
            for (var i = 0; i < rooms.Count; i++)
            {
                RenderRoomRow(rooms[i], i);
            }

            return panelObject;
        }

        private void RenderHeader()
        {
            var headerContainer = new GameObject("Header");
            headerContainer.transform.SetParent(panelObject.transform, worldPositionStays: false);

            var joinLabel = uiUtil.CreateLabelText("Code");
            joinLabel.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            joinLabel.transform.localPosition = new Vector3(-3f, 0, 0);

            var gameLabel = uiUtil.CreateLabelText("Game");
            gameLabel.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            gameLabel.transform.localPosition = new Vector3(-0.4f, 0, 0);

            var floorLabel = uiUtil.CreateLabelText("Floor");
            floorLabel.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            floorLabel.transform.localPosition = new Vector3(1.75f, 0, 0);

            var playersLabel = uiUtil.CreateLabelText("Players");
            playersLabel.transform.SetParent(headerContainer.transform, worldPositionStays: false);
            playersLabel.transform.localPosition = new Vector3(3.5f, 0, 0);
        }

        private void RenderRoomRow(RoomInfo room, int row)
        {
            var yOffset = (1 + row) * -1f;
            var roomRowContainer = new GameObject($"Row{row}");
            roomRowContainer.transform.SetParent(panelObject.transform, worldPositionStays: false);
            roomRowContainer.transform.localPosition = new Vector3(0, yOffset, 0);

            object obj;
            var gameType = room.CustomProperties.TryGetValue("at", out obj)
                ? (LevelSequence.GameType) obj
                : LevelSequence.GameType.Invalid;
            var floorIndex = room.CustomProperties.TryGetValue("fi", out obj) ? (int) obj : -1;

            if (gameType == LevelSequence.GameType.Invalid || floorIndex < 0) return;

            var joinButton = uiUtil.CreateButton(JoinRoomAction(room.Name));
            joinButton.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            joinButton.transform.localScale = new Vector3(0.4f, 0.7f, 0.7f);
            joinButton.transform.localPosition = new Vector3(-3f, 0, 0);

            var joinText = uiUtil.CreateText(room.Name, Color.white, UiUtil.DefaultLabelFontSize);
            joinText.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            joinText.transform.localPosition = new Vector3(-3f, 0, 0);

            var gameName = StringifyGameType(gameType);
            var gameLabel = uiUtil.CreateLabelText(gameName);
            gameLabel.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            gameLabel.transform.localPosition = new Vector3(-0.4f, 0, 0);

            var floorLabel = uiUtil.CreateLabelText(floorIndex.ToString());
            floorLabel.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            floorLabel.transform.localPosition = new Vector3(1.75f, 0, 0);

            var playersText = $"{room.PlayerCount}/{room.MaxPlayers}";
            var playersLabel = uiUtil.CreateLabelText(playersText);
            playersLabel.transform.SetParent(roomRowContainer.transform, worldPositionStays: false);
            playersLabel.transform.localPosition = new Vector3(3.25f, 0, 0);
        }

        private static string StringifyGameType(LevelSequence.GameType gameType)
        {
            return gameType.ToString();
        }

        private static Action JoinRoomAction(string roomCode)
        {
            return () =>
            {
                MelonLogger.Msg($"Joining room [{roomCode}].");
                Traverse.Create(GameContextState.LobbyMenuController).Method("JoinGame", roomCode, true).GetValue();
            };
        }
    }
}
