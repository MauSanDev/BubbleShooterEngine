using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleDatabase", menuName = "Gameplay/Bubble Database")]
public class BubbleDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<BubbleReference> bubbleReferences;
    
    private Dictionary<string, BubbleBehavior> bubbleReferencesDictionary = new Dictionary<string, BubbleBehavior>();

    public BubbleBehavior GetBubbleById(string bubbleId)
    {
        if (!bubbleReferencesDictionary.ContainsKey(bubbleId))
        {
            throw new Exception($"{GetType()} :: The bubble with ID {bubbleId} wasn't found.");
        }

        return bubbleReferencesDictionary[bubbleId];
    }

    #region ISerializationCallbackReceiver Methods
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        bubbleReferencesDictionary.Clear();
        foreach (BubbleReference reference in bubbleReferences)
        {
            bubbleReferencesDictionary.Add(reference.id, reference.bubble);
        }
    }
    #endregion
    
    [Serializable]
    private class BubbleReference
    {
        [SerializeField] public string id;
        [SerializeField] public BubbleBehavior bubble;
    }
}
