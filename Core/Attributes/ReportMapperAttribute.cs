using System;

namespace Core.Attributes
{
    internal class ReportMapperAttribute : Attribute
    {
        public virtual int StartIndex { get; }
        public virtual int Length { get; }

        public ReportMapperAttribute(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }
}
