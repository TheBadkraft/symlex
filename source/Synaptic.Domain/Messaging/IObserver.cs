
namespace Synaptic.Messaging;

/// <summary>
/// Interface for observers to receive notifications about function assimilation.
/// </summary>
public interface IObserver<in T>
{
    void OnNext(T value);
    //  TODO: We don't raise exceptions in the current implementation
    void OnError(Exception error);
    void OnCompleted();
}
