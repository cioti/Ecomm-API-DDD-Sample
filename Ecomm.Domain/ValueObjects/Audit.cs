using System;
using System.Collections.Generic;

namespace Ecomm.Domain.ValueObjects
{
    public class Audit : ValueObject
    {
        public string CreatedBy { get; private set; }
        public string ModifiedBy { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? ModifiedDate { get; private set; }

        public void WasModifiedBy(string modifiedBy, DateTime date)
        {
            ModifiedBy = modifiedBy;
            ModifiedDate = date;
        }

        public void WasCreatedBy(string createdBy, DateTime date)
        {
            CreatedBy = createdBy;
            CreatedDate = date;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CreatedBy;
            yield return ModifiedBy;
            yield return CreatedDate;
            yield return ModifiedDate;
        }

        public override string ToString()
        {
            string result = $"Entity was created in {ModifiedDate} by '{CreatedBy}'. ";
            if (ModifiedDate.HasValue)
            {
                result += $"Entity was last modified in {ModifiedDate} by '{ModifiedBy}.";
            }
            return result;
        }
    }
}
