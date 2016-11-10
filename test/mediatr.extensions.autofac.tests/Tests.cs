namespace mediatr.extensions.autofac.tests
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Autofac;
    using MediatR;
    using Xunit;

    public class Tests
    {
        [Fact]
        public async Task TestResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddMediatR(typeof(Tests).GetTypeInfo().Assembly);
            var container = builder.Build();

            var handler = container.Resolve<IAsyncRequestHandler<Ping, Unit>>();
            await handler.Handle(new Ping { Name = "Bob" });
        }

        [Fact]
        public async Task TestMediator()
        {
            var builder = new ContainerBuilder();
            builder.AddMediatR(typeof(Tests).GetTypeInfo().Assembly);
            var container = builder.Build();
            var mediator = container.Resolve<IMediator>();
            await mediator.SendAsync(new Ping { Name = "Bob" });
        }
    }

    public class Ping : IAsyncRequest
    {
        public string Name { get; set; }
    }

    public class PingHandler : IAsyncRequestHandler<Ping, Unit>
    {
        public Task<Unit> Handle(Ping message)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}
