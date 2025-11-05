using System;
using Ludo;
using UnityEngine;

namespace DefaultNamespace
{
    public class LudoBoardDebug : MonoBehaviour
    {
        public LudoBoard ludoBoard;

        public int token;
        public int dice;

        [ContextMenu(nameof(Setup))]
        public void Setup()
        {
        }

        private void OnValidate()
        {
            var isTokenMoved = ludoBoard.TryMoveToken(token, dice, out byte position, out var evictedTokenIndex, out var error);
            string logMessage = $"[{DateTime.Now:HH:mm:ss.fff}] " +
                                $"Token: {token}, " +
                                $"Dice: {dice}, " +
                                $"Evicted: {evictedTokenIndex}, " +
                                $"Result: {(isTokenMoved ? "SUCCESS" : "FAILURE")}, ";

            logMessage += ludoBoard.ToString();
            if (!isTokenMoved)
            {
                logMessage += $", Error: {error}";
            }

            Debug.Log(logMessage, this);
        }
    }
}