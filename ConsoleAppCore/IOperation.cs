using System;

namespace ConsoleAppCore
{
    public interface IOperation
    {
        Guid OperationId { get; }
    }
    public interface IOperationSingleton : IOperation { }
    public interface IOperationTransient : IOperation { }
    public interface IOperationScoped : IOperation { }

    public class Operation :
        IOperationSingleton,
        IOperationTransient,
        IOperationScoped
    {
        private readonly Guid _guid;

        public Operation()
        {
            _guid = Guid.NewGuid();
        }

        public Operation(Guid guid)
        {
            _guid = guid;
        }

        public Guid OperationId => _guid;
    }
}