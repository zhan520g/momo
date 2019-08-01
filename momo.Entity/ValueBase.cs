using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Entity
{
    /// <summary>
    /// 泛型值对象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueBase<T> where T : ValueBase<T>
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
