using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Utilities
{
    class Tuple<T1, T2>
    {
        public T1 first = default(T1);
        public T2 second = default(T2);

        public Tuple(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
