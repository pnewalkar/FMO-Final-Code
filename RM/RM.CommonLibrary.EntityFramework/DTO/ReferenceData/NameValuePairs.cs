using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DTO.ReferenceData
{
    public class NameValuePairsDTO<TKey, TValue>
    {
        public NameValuePairsDTO(TKey tKey, TValue tValue)
        {
            this.keyName = tKey;
            this.NameValuePairs = tValue;
        }

        public TKey keyName { get; set; }
        public TValue NameValuePairs { get; set; }
    }
}
