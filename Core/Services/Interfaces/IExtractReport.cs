using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IExtractReport<T>
    {
        event EventHandler<T> ReceiveReportLine;
        Task Start(CancellationToken cancellationToken);
    }
}