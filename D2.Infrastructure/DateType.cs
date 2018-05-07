using System;
using System.Data.Common;
using D2.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace D2.Infrastructure
{
    public class DataType : IUserType
    {
        public object Assemble(object cached, object owner)
        {
            if (cached == null) return null;
            return DeepCopy(cached);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if (value == null) {
                NHibernateUtil.Date.NullSafeSet(cmd, null, index, session);
            }
            else {
                NHibernateUtil.Date.NullSafeSet(cmd, Date.fromObject(value).DateTime, index, session);
            }
        }

        public object DeepCopy(object value)
        {
            if (value == null) return null;
            return new Date(((Date)value).Days);
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        public new bool Equals(object x, object y)
        {
            if (x == null ^ y == null) return false;
            if (x == null) return true;
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            if (rs == null) return null;
            var data = NHibernateUtil.Date.NullSafeGet(rs, names[0], session);
            
            if (data == null) return null;
            return new Date((DateTime)data);
        }

        public bool IsMutable => false;

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType => typeof(Date);

        public SqlType[] SqlTypes => new[] { new SqlType(System.Data.DbType.Date) };
    }
}