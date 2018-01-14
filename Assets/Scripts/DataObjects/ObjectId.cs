using System;

[Serializable]
public class ObjectId : IEquatable<ObjectId> {
    public string oid;

    public ObjectId() {}

    public ObjectId(string oid) {
        this.oid = oid;
    }

    public bool Equals(ObjectId otherObjectId) {
        return this.oid.Equals(otherObjectId.oid);
    }
}