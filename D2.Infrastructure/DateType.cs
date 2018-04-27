using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace D2.Infrastructure
{
    public class DataType : IUserType
    {
        public bool Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(object x)
        {
            throw new NotImplementedException();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            throw new NotImplementedException();
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            throw new NotImplementedException();
        }

        public object DeepCopy(object value)
        {
            throw new NotImplementedException();
        }

        public object Replace(object original, object target, object owner)
        {
            throw new NotImplementedException();
        }

        public object Assemble(object cached, object owner)
        {
            throw new NotImplementedException();
        }

        public object Disassemble(object value)
        {
            throw new NotImplementedException();
        }

        public SqlType[] SqlTypes { get; }
        public Type ReturnedType { get; }
        public bool IsMutable { get; }
    }
}