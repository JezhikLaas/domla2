using System;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace D2.Infrastructure
{
    [Serializable]
    public class VariantType : ICompositeUserType
    {
        public object GetPropertyValue(object component, int property)
        {
            if (component == null) return null;
            Variant item = (Variant)component;
            switch (property) {
                case 0: return item.Raw;
                case 1: return item.Tag;
                default : throw new ArgumentOutOfRangeException("property");
            }
        }

        public void SetPropertyValue(object component, int property, object value)
        { }

        public new bool Equals(object one, object two)
        {
            if (one == null ^ two == null) return false;
            if (one == null) return true;
            return one.Equals (two);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
        {
            if (dr == null) return null;
            
            string tag = (string)NHibernateUtil.String.NullSafeGet(dr, names[0], session, owner);
            if (tag == null) return null;
            
            string value = (string)NHibernateUtil.String.NullSafeGet(dr, names[1], session, owner);
            if (value == null) return null;

            return new Variant(tag, value);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
        {
            if (value == null) return;
            
            var variant = (Variant) value;
            NHibernateUtil.String.NullSafeSet(cmd, variant.Tag.ToString(), index, session);
            NHibernateUtil.String.NullSafeSet(cmd, variant.Encode(), index + 1, session);
        }

        public object DeepCopy(object value)
        {
            if (value == null) return null;
            return new Variant((Variant)value);
        }

        public object Disassemble(object value, ISessionImplementor session)
        {
            return DeepCopy(value);
        }

        public object Assemble(object cached, ISessionImplementor session, object owner)
        {
            return DeepCopy(cached);
        }

        public object Replace(object original, object target, ISessionImplementor session, object owner)
        {
            return original;
        }

        public string[] PropertyNames => new string[2] {"Tag", "Value"};
        public IType[] PropertyTypes => new NHibernate.Type.IType[2] { NHibernateUtil.String, NHibernateUtil.String };
        public Type ReturnedClass => typeof(Variant);
        public bool IsMutable => true;
    }
}