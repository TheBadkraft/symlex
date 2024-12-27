
using FakeItEasy.Configuration;
using Synaptic.Core;

namespace Synaptic.Terminal.Tests;

//  Test suite for the IRuntime interface
[TestContainer(Ignore = false)]
public class RuntimeTesing
{
    private IRuntime runtime;

    #region Test Methods
    [Test]
    public void InitialState()
    {
        Assert.IsNotNull(runtime);
        A.CallTo(() => runtime.Initialize(A<IServiceContainer>._, A<IResourceService>._)).MustHaveHappened();
    }
    #endregion Test Methods


    #region Test SetUp & TearDown
    [SetUp]
    public void SetUp()
    {
        runtime = A.Fake<IRuntime>();
        SynapticHub.Instance.RegisterRuntime(runtime);
    }
    #endregion Test SetUp & TearDown
    #region Helper Methods

    #endregion Helper Methods
    #region Container Initialization & CleanUp

    #endregion Container Initialization & CleanUp

}
