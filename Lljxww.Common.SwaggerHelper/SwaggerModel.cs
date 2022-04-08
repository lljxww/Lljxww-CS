namespace Lljxww.Common.SwaggerHelper;

public abstract class SwaggerModel
{
    public virtual string Version { get; }

    public T Cast<T>() where T : SwaggerModel
    {
        return (T)this;
    }
}