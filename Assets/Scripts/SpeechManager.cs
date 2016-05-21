using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public GameObject Heart;

    // Use this for initialization
    void Start()
    {
        keywords.Add("Move Up", () =>
        {
            Heart.transform.position = new Vector3 (Heart.transform.position.x, Heart.transform.position.y+1, Heart.transform.position.z);
     
        });

        keywords.Add("Move Down", () =>
        {
            Heart.transform.position = new Vector3(Heart.transform.position.x, Heart.transform.position.y-1, Heart.transform.position.z);
        });

        keywords.Add("Rotate Left", () =>
        {
            Heart.transform.Rotate(Vector3.left * 180);
        });

        keywords.Add("Rotate Right", () =>
        {
            Heart.transform.Rotate(Vector3.right * 180);
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