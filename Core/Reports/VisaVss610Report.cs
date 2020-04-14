using System;
using Core.Attributes;
using Core.Reports.Interfaces;

namespace Core.Reports
{
    public class VisaVss610Report : IReport
    {
        public string Id { get; } = "VSS-610";

        [ReportMapper(19, 24)]
        public string Description { get; set; }

        [ReportMapper(42, 8)]
        public DateTime Date { get; set; }

        [ReportMapper(51, 24)]
        public long Count { get; set; }

        [ReportMapper(75, 31)]
        public decimal Amount { get; set; }

        [ReportMapper(109, 30)]
        public decimal Fee { get; set; }

        public bool IsValid()
        {
            return Date != default(DateTime) &&
                   Count > 0 &&
                   Amount > 0 &&
                   Fee > 0;
        }

        public override string ToString()
        {
            return $"{Description,-30}{Date,20:O}{Count,15}{Amount,20}{Fee,20}{Id,20}";
        }
    }
}