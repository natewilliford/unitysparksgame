using System;

[Serializable]
public class Building {
    public ObjectId _id;
    public string shortCode;
    public DateTime timeCreated;
    public DateTime timeLastCollected;
    public Position position;
}