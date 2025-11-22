using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace EasyUI.PickerWheelUI
{
    public class PickerWheel : MonoBehaviour
    {
        [Header("References :")]
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private Transform linesParent;

        [Space]
        [SerializeField] private Transform PickerWheelTransform;
        [SerializeField] private Transform wheelCircle;
        [SerializeField] private GameObject wheelPiecePrefab;
        [SerializeField] private Transform wheelPiecesParent;

        [Space]
        [Header("Sounds :")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip tickAudioClip;
        [SerializeField][Range(0f, 1f)] private float volume = .5f;
        [SerializeField][Range(-3f, 3f)] private float pitch = 1f;

        [Space]
        [Header("Picker wheel settings :")]
        [Range(1, 20)] public int spinDuration = 8;
        [SerializeField][Range(.2f, 2f)] private float wheelSize = 1f;

        [Space]
        [Header("Picker wheel pieces :")]
        public WheelPiece[] wheelPieces;

        // Events
        private UnityAction onSpinStartEvent;
        private UnityAction<WheelPiece> onSpinEndEvent;

        private bool _isSpinning = false;
        public bool IsSpinning { get { return _isSpinning; } }

        private Vector2 pieceMinSize = new Vector2(81f, 146f);
        private Vector2 pieceMaxSize = new Vector2(144f, 213f);
        private int piecesMin = 2;
        private int piecesMax = 12;

        private float pieceAngle;
        private float halfPieceAngle;
        private float halfPieceAngleWithPaddings;

        private double accumulatedWeight;
        private System.Random rand = new System.Random();

        private List<int> nonZeroChancesIndices = new List<int>();
        public Text mycoin;

        private void Start()
        {
            pieceAngle = 360 / wheelPieces.Length;
            halfPieceAngle = pieceAngle / 2f;
            halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);

            Generate();
            CalculateWeightsAndIndices();
            
            if (nonZeroChancesIndices.Count == 0)
                Debug.LogError("You can't set all pieces chance to zero");

            SetupAudio();
        }

        private void Update()
        {
            Transform pickerWheelTransform = GetComponent<Transform>();
            pickerWheelTransform.localScale = new Vector3(1.5f, 1.5f, 1f);
        }

        private void SetupAudio()
        {
            audioSource.clip = tickAudioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
        }

        private void Generate()
        {
            wheelPiecePrefab = InstantiatePiece();

            RectTransform rt = wheelPiecePrefab.transform.GetChild(0).GetComponent<RectTransform>();
            float pieceWidth = Mathf.Lerp(pieceMinSize.x, pieceMaxSize.x, 1f - Mathf.InverseLerp(piecesMin, piecesMax, wheelPieces.Length));
            float pieceHeight = Mathf.Lerp(pieceMinSize.y, pieceMaxSize.y, 1f - Mathf.InverseLerp(piecesMin, piecesMax, wheelPieces.Length));
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);

            for (int i = 0; i < wheelPieces.Length; i++)
                DrawPiece(i);

            Destroy(wheelPiecePrefab);
        }

        private void DrawPiece(int index)
        {
            WheelPiece piece = wheelPieces[index];
            Transform pieceTrns = InstantiatePiece().transform.GetChild(0);

            pieceTrns.GetChild(0).GetComponent<Image>().sprite = piece.Icon;
            pieceTrns.GetChild(1).GetComponent<Text>().text = piece.Label;
            pieceTrns.GetChild(2).GetComponent<Text>().text = piece.Amount.ToString();

            //Line
            Transform lineTrns = Instantiate(linePrefab, linesParent.position, Quaternion.identity, linesParent).transform;
            lineTrns.RotateAround(wheelPiecesParent.position, Vector3.back, (pieceAngle * index) + halfPieceAngle);

            pieceTrns.RotateAround(wheelPiecesParent.position, Vector3.back, pieceAngle * index);
        }

        private GameObject InstantiatePiece()
        {
            return Instantiate(wheelPiecePrefab, wheelPiecesParent.position, Quaternion.identity, wheelPiecesParent);
        }

        public void Spin()
        {
            if (!_isSpinning)
            {
                _isSpinning = true;
                if (onSpinStartEvent != null)
                    onSpinStartEvent.Invoke();

                int index = GetRandomPieceIndex();
                WheelPiece piece = wheelPieces[index];

                if (piece.Chance == 0 && nonZeroChancesIndices.Count != 0)
                {
                    index = nonZeroChancesIndices[Random.Range(0, nonZeroChancesIndices.Count)];
                    piece = wheelPieces[index];
                }

                // Calculate the target angle to land on the selected piece
                float angle = (pieceAngle * index);
                float leftOffset = angle - halfPieceAngleWithPaddings;
                float rightOffset = angle + halfPieceAngleWithPaddings;
                float randomAngle = Random.Range(leftOffset, rightOffset);

                // Add extra rotations for spinning effect
                float targetAngle = (360 * spinDuration * 2) - randomAngle;

                StartCoroutine(SpinCoroutine(targetAngle));
            }
            
            PlayerPrefs.SetInt("myspin", 1);
        }

        private IEnumerator SpinCoroutine(float targetAngle)
        {
            float startAngle = wheelCircle.eulerAngles.z;
            float currentTime = 0f;
            float prevAngle = startAngle;
            bool isIndicatorOnTheLine = false;

            while (currentTime < spinDuration)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / spinDuration;
                
                // Ease in-out curve
                float easeT = t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
                
                float currentAngle = Mathf.Lerp(startAngle, startAngle + targetAngle, easeT);
                wheelCircle.eulerAngles = new Vector3(0, 0, currentAngle);

                // Play tick sound
                float diff = Mathf.Abs(prevAngle - currentAngle);
                if (diff >= halfPieceAngle)
                {
                    if (isIndicatorOnTheLine && audioSource != null)
                    {
                        audioSource.PlayOneShot(audioSource.clip);
                    }
                    prevAngle = currentAngle;
                    isIndicatorOnTheLine = !isIndicatorOnTheLine;
                }

                yield return null;
            }

            // Ensure final position
            wheelCircle.eulerAngles = new Vector3(0, 0, startAngle + targetAngle);

            // Calculate which piece the needle is pointing at based on final rotation
            float finalAngle = (startAngle + targetAngle) % 360;
            
            // Normalize angle to 0-360
            if (finalAngle < 0) finalAngle += 360;
            
            // The needle points at top (0 degrees), calculate which piece it's pointing at
            // Since pieces are drawn clockwise, we need to find which piece range the angle falls into
            int pieceIndex = Mathf.FloorToInt(finalAngle / pieceAngle);
            pieceIndex = wheelPieces.Length - pieceIndex; // Reverse because wheel spins counter to piece layout
            pieceIndex = pieceIndex % wheelPieces.Length;
            
            WheelPiece selectedPiece = wheelPieces[pieceIndex];

            _isSpinning = false;
            if (onSpinEndEvent != null)
                onSpinEndEvent.Invoke(selectedPiece);

            ShowRewardPanel(selectedPiece);

            onSpinStartEvent = null;
            onSpinEndEvent = null;
        }

        public void OnSpinStart(UnityAction action)
        {
            onSpinStartEvent = action;
        }

        public void OnSpinEnd(UnityAction<WheelPiece> action)
        {
            onSpinEndEvent = action;
        }

        private int GetRandomPieceIndex()
        {
            double r = rand.NextDouble() * accumulatedWeight;

            for (int i = 0; i < wheelPieces.Length; i++)
                if (wheelPieces[i]._weight >= r)
                    return i;

            return 0;
        }

        private void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < wheelPieces.Length; i++)
            {
                WheelPiece piece = wheelPieces[i];

                accumulatedWeight += piece.Chance;
                piece._weight = accumulatedWeight;
                piece.Index = i;

                if (piece.Chance > 0)
                    nonZeroChancesIndices.Add(i);
            }
        }

        private void OnValidate()
        {
            if (PickerWheelTransform != null)
                PickerWheelTransform.localScale = new Vector3(wheelSize, wheelSize, 1f);

            if (wheelPieces.Length > piecesMax || wheelPieces.Length < piecesMin)
                Debug.LogError("[ PickerWheelwheel ]  pieces length must be between " + piecesMin + " and " + piecesMax);
        }

        // Reward panel
        public GameObject rewardPanel;
        public Text rewardText;

        public void ShowRewardPanel(WheelPiece piece)
        {
            if (rewardPanel != null)
                rewardPanel.SetActive(true);

            if (rewardText != null)
                rewardText.text = "Congratulations! You will get " + piece.Amount + " coins";

            StartCoroutine(AutoCloseRewardPanel());
        }

        private IEnumerator AutoCloseRewardPanel()
        {
            yield return new WaitForSeconds(3f);
            if (rewardPanel != null)
                rewardPanel.SetActive(false);
        }
    }
}