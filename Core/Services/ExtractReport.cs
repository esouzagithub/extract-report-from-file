using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Reports.Interfaces;
using Core.Services.Interfaces;

namespace Core.Services
{
    public class ExtractReport<T> : IExtractReport<T> where T : IReport
    {
        private const string PrefixIdVss = "VSS-";
        private readonly string[] _bookmarks = { "DB", "CR" };
        private readonly StreamReader _streamReader;

        public event EventHandler<T> ReceiveReportLine;

        public ExtractReport(StreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        private void ProcessFile(CancellationToken cancellationToken)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));

            while (_streamReader.Peek() >= 0 && !cancellationToken.IsCancellationRequested)
            {
                if (IdExists(_streamReader.ReadLine(), obj.Id))
                {
                    while (_streamReader.Peek() >= 0 && !cancellationToken.IsCancellationRequested)
                    {
                        var currentLine = _streamReader.ReadLine();

                        if (_bookmarks.Any(x => currentLine.Contains(x)))
                        {
                            var result = ReportMapper.Reader<T>(currentLine);

                            if (result.IsValid())
                            {
                                ReceiveReportLine?.Invoke(this, result);
                            }
                        }
                        else
                        {
                            if (currentLine.Contains(PrefixIdVss))
                            {
                                var checkReportId = IdExists(_streamReader.ReadLine(), obj.Id);

                                if (!checkReportId)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        private bool IdExists(string line, string id)
        {
            var result = line.Contains(id);
            return result;
        }

        public Task Start(CancellationToken cancellationToken = default)
        {
            return Task
                .Factory
                .StartNew(() => ProcessFile(cancellationToken), cancellationToken);
        }
    }
}