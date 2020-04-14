using System;
using System.IO;
using System.Threading;
using Core.Reports;
using Core.Services;

namespace ExtractReport.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            const string path = @"C:\Users\esouza\Downloads\419894020200331A0606OCE.itf";


            using var fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var streamReader = new StreamReader(fileStream);

            //using (var streamReader = new StreamReader(
            //    File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            //{
            try
            {
                var extractReport = new ExtractReport<VisaVss610Report>(streamReader);

                extractReport.ReceiveReportLine += OnReceiveReportLine;

                var task = extractReport.Start(cancellationTokenSource.Token);

                //cancellationTokenSource.CancelAfter(1000);

                task.Wait(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                if (e.CancellationToken == cancellationTokenSource.Token)
                    Console.WriteLine("Canceled from UI thread throwing OCE.");
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("AggregateException caught: " + ae.InnerException);
                foreach (var inner in ae.InnerExceptions)
                {
                    Console.WriteLine(inner.Message + inner.Source);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }

            Console.WriteLine("Process completed");
            Console.ReadKey();
            //}
        }

        private static void OnReceiveReportLine(object sender, VisaVss610Report e)
        {
            Console.WriteLine(e);
        }
    }
}
