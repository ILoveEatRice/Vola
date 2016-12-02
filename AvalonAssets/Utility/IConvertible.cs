namespace AvalonAssets.Utility
{
    public interface IConvertible<out T>
    {
        T ConvertTo();
    }
}