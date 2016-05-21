using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public GameObject heart;
    public Catheter catheter;
    public float speed;
    private Rigidbody rb;

    private InitialHeartData initialHeartData;

    private class InitialHeartData
    {
        public Vector3 position;
        public Vector3 scale;
    }

    // Use this for initialization
    void Start()
    {
        initialHeartData = new InitialHeartData
        {
            position = heart.transform.position,
            scale = heart.transform.localScale,
        };

        rb = catheter.GetComponent<Rigidbody>();
        keywords.Add("Move Up", () =>
        {
            catheter.MoveUp();
            //rb.AddForce(*speed)
        });

        keywords.Add("Move Down", () =>
        {
            catheter.MoveDown();
        });

        keywords.Add("Rotate Left", () =>
        {
            catheter.RotateLeft();
            //rb.AddForce(*speed)
        });

        keywords.Add("Rotate Right", () =>
        {
            catheter.RotateRight();
        });

        keywords.Add("Zoom In", () =>
        {
            heart.transform.localScale *= 30;
            heart.transform.position = Camera.main.gameObject.transform.position + (heart.transform.position - catheter.transform.position);
        });

        keywords.Add("Zoom Out", () =>
        {
            heart.transform.localScale = initialHeartData.scale;
            heart.transform.position = initialHeartData.position;
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}