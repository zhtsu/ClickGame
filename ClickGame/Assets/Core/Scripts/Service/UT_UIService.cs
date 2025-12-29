using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum UT_EUILayer
{
    None,
    Bottom,
    Main,
    Top,
    Popup,
}

public class UT_UIService : UT_Service, UT_IUIService
{
    public override string ServiceName => "UI Service";

    private UT_SO_UIConfig _UIConfig;
    private UT_UIRoot _UIRoot;
    private UT_IPrefabService _IPrefabService;

    private Dictionary<string, UT_UIView> _CachedUIDict = new Dictionary<string, UT_UIView>();
    private Stack<UT_UIView> _ActiveUIStack = new Stack<UT_UIView>();

    public UT_UIService(UT_SO_UIConfig UIConfig, UT_IPrefabService IPrefabService)
    {
        _UIConfig = UIConfig;
        _IPrefabService = IPrefabService;
    }

    public override UniTask Initialize()
    {
        _CachedUIDict.Clear();
        _ActiveUIStack.Clear();

        if (_UIConfig == null)
        {
            Debug.LogError("UI config is null!");
            return UniTask.CompletedTask;
        }

        _UIRoot = UnityEngine.Object.Instantiate(_UIConfig.UIRootPrefab)?.GetComponent<UT_UIRoot>();
        return UniTask.CompletedTask;
    }

    public override void Destroy()
    {
        foreach (var Pair in _CachedUIDict)
        {
            if (Pair.Value != null)
                GameObject.Destroy(Pair.Value.gameObject);
        }

        _CachedUIDict.Clear();
        _ActiveUIStack.Clear();
    }

    public void OpenUI(string UITypeKey, UT_UIParams Params = null)
    {
        if (_UIRoot == null)
            return;

        if (_ActiveUIStack.Count > 0 && _ActiveUIStack.Peek().TypeKey == UITypeKey)
            return;

        UT_SO_UIDescriptor UIDesc = GetUIDesc(UITypeKey);
        if (UIDesc == null)
            return;

        if (_CachedUIDict.TryGetValue(UITypeKey, out UT_UIView CachedUI))
        {
            OpenUI_Internal(CachedUI, Params, UIDesc);
            return;
        }

        GameObject Prefab = _IPrefabService.GetPrefab(UIDesc.PrefabAddress);
        if (Prefab == null)
        {
            Debug.LogError($"Failed to load prefab for UI type {UITypeKey}!");
            return;
        }
        GameObject UIObj = GameObject.Instantiate(Prefab);
        UT_UIView UIComp = UIObj.GetComponent<UT_UIView>();
        if (UIComp != null)
        {
            _CachedUIDict.Add(UITypeKey, UIComp);
            OpenUI_Internal(UIComp, Params, UIDesc);
        }
        else
        {
            Debug.LogError($"UI Component is missing in prefab for UI type {UITypeKey}!");
            GameObject.Destroy(UIObj);
        }
    }

    private void OpenUI_Internal(UT_UIView UI, UT_UIParams Params, UT_SO_UIDescriptor UIDesc)
    {
        if (_ActiveUIStack.Count > 0)
            _ActiveUIStack.Peek().OnPause();

        UI.Initialize(Params);
        UI.OnOpen();

        RectTransform Rect = UI.GetComponent<RectTransform>();
        SetFullStretch(Rect);

        Transform LayerTransform = _UIRoot.GetLayerObject(UIDesc.Layer).transform;
        UI.transform.SetParent(LayerTransform, false);
        UI.enabled = true;

        _ActiveUIStack.Push(UI);
    }

    public void Back()
    {
        if (_ActiveUIStack.Count == 0)
            return;

        UT_UIView TopUI = _ActiveUIStack.Peek();

        TopUI.OnClose();
        TopUI.transform.SetParent(null);
        TopUI.enabled = false;

        _ActiveUIStack.Pop();

        if (_ActiveUIStack.Count > 0)
            _ActiveUIStack.Peek().OnResume();
    }

    public void Close()
    {
        while (_ActiveUIStack.Count > 0)
        {
            UT_UIView TopUI = _ActiveUIStack.Pop();
            TopUI.OnClose();
            TopUI.transform.SetParent(null);
            TopUI.enabled = false;
        }
    }

    private UT_SO_UIDescriptor GetUIDesc(string UITypeKey)
    {
        if (_UIConfig == null)
            return null;

        foreach (var Desc in _UIConfig.UITypeDescriptorList)
        {
            if (Desc.TypeKey == UITypeKey)
                return Desc;
        }

        Debug.LogError($"UI descriptor for type {UITypeKey} not found in config!");
        return null;
    }

    public static void SetFullStretch(RectTransform TargetRectTransform)
    {
        TargetRectTransform.anchorMin = Vector2.zero;
        TargetRectTransform.anchorMax = Vector2.one;

        TargetRectTransform.offsetMin = Vector2.zero;
        TargetRectTransform.offsetMax = Vector2.zero;

        TargetRectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
}
