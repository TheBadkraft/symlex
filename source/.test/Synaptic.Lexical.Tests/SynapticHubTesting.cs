
using Synaptic.Core;

namespace Synaptic.Terminal.Tests;

//  Test suite for the SynapticHub class
[TestContainer(Ignore = true)]
public class SynapticHubTesting
{
    private ISynapticHub synapticHub;
    private List<IDisposable> disposables = new();

    public static TestContext TestContext { get; set; }

    #region Test Methods
    //  Test inistial state of the SynapticHub
    [Test]
    public void InitialState()
    {
        Assert.IsNotNull(synapticHub);
        Assert.IsFalse(synapticHub.GetState(s => s.Is(RuntimeState.Shutdown)));
        Assert.IsFalse(synapticHub.GetState(s => s.Is(SynapticState.Shutdown)));
    }
    //  Tests subscription and notification of observers
    [Test]
    public void SubscriptionNotification()
    {
        var receivedNotifications = false;
        bool runtimeShutdown = false;
        bool runtimeError = false;

        var observer = new StateObserver(state =>
        {
            runtimeShutdown = state.GetState(s => s.Is(RuntimeState.Shutdown));
            runtimeError = state.GetState(s => s.Is(RuntimeState.Error));
        });

        disposables.Add(synapticHub.Subscribe(observer));

        synapticHub.UpdateState(state =>
        {
            state.SetState(RuntimeState.Shutdown);
        });

        Assert.IsTrue(receivedNotifications);
        Assert.IsTrue(runtimeShutdown);
        Assert.IsTrue(runtimeError);


    }
    #endregion Test Methods

    #region Helper Methods
    private class StateObserver : IObserver<IStateChange>
    {
        private readonly Action<IStateChange> _onNextAction;

        public StateObserver(Action<IStateChange> onNextAction)
        {
            _onNextAction = onNextAction;
        }

        public void OnNext(IStateChange state)
        {
            _onNextAction(state);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
    #endregion Helper Methods
    #region Test Setup & TearDown
    [SetUp]
    public void Setup()
    {
        //  TODO: mock the SynapticHub
        synapticHub = SynapticHub.Instance;
    }
    [TearDown]
    public void TearDown()
    {
        foreach (var disposable in disposables)
        {
            disposable.Dispose();
        }
        disposables.Clear();
        synapticHub = null;

        Debug.WriteLine($"{TestContext.ContainerName}.{TestContext.TestMethodName} :: Result [{TestContext.TestResult}]{(!string.IsNullOrEmpty(TestContext.ErrorMessage) ? $" :: {TestContext.ErrorMessage}" : string.Empty)}");
    }
    #endregion Test Setup & TearDown
    #region Initialize and CleanUp

    #endregion Initialize and CleanUp
}
