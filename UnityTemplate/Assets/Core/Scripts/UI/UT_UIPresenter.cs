
public abstract class UT_UIPresenter<TUIModel, TUIView>
    where TUIModel : UT_SO_UIModel
    where TUIView : UT_UIView
{
    private readonly TUIModel _Model;
    private readonly TUIView _View;

    UT_UIPresenter(TUIModel Model, TUIView View)
    {
        _Model = Model;
        _View = View;
    }
}
