using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// A Container for all types of unity events.
/// </summary>

#region Generic
[System.Serializable]
public class UnityVoidEvent : UnityEvent { }

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> { }

[System.Serializable]
public class UnityBoolEvent : UnityEvent<bool> { }

[System.Serializable]
public class UnityStringEvent : UnityEvent<string> { }
#endregion

#region Unity
[System.Serializable]
public class UnitySpriteEvent : UnityEvent<Sprite> { }
#endregion

#region Custom
#endregion