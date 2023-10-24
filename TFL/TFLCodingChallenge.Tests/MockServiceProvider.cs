using Moq;

using TFLCodingChallenge.Service;

namespace TFLCodingChallenge.Tests
{
    public class MockServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _provider;
        private readonly Mock<IRoadService> _roadServiceMock;

        public MockServiceProvider(IServiceProvider Provider, Mock<IRoadService> roadServiceMock)
        {
            _provider = Provider;
            _roadServiceMock = roadServiceMock;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IRoadService))
            {
                return _roadServiceMock.Object;
            }

            // Forward any other service requests to the real provider
            return _provider.GetService(serviceType);
        }
    }
}
