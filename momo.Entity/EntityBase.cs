using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Entity
{
    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    public abstract class EntityBase<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    #region Implements

    /// <summary>
    /// Guid 类型主键实体基类
    /// </summary>
    public abstract class EntityBase : EntityBase<Guid>
    { }

    #endregion
}
