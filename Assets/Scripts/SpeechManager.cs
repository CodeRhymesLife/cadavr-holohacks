using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public Catheter catheter;
    public float speed;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
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