using EmployeeDAA.Core;
using FluentMigrator.Expressions;

using LinqToDB.Mapping;
using LinqToDB.Metadata;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace EmployeeDAA.Data.Mapping
{
    public partial class FluentMigratorMetadataReader : IMetadataReader
    {
        #region Ctor

        public FluentMigratorMetadataReader()
        {
        }

        #endregion

        #region Utils

        protected static T GetAttribute<T>(Type type, MemberInfo memberInfo) where T : Attribute
        {
            Attribute attribute = Types.GetOrAdd((type, memberInfo), t =>
            {

                if (typeof(T) != typeof(ColumnAttribute))
                {
                    return null;
                }

                bool isIgnoreColumn = memberInfo.GetCustomAttribute(typeof(NoColumnMap)) != null;
                if (memberInfo.Name == "Id")
                {
                    return new ColumnAttribute
                    {
                        //Name = "Id",
                        IsPrimaryKey = true,
                        //IsColumn = true,
                        //CanBeNull = false,
                        IsIdentity = true
                    };
                }
                if (isIgnoreColumn)
                {
                    return new ColumnAttribute
                    {
                        IsColumn = false
                    };
                }

                return null;

            });

            return (T)attribute;
        }

        protected static T[] GetAttributes<T>(Type type, Type attributeType, MemberInfo memberInfo = null)
            where T : Attribute
        {
            if (type.IsSubclassOf(typeof(BaseEntity)) && typeof(T) == attributeType && GetAttribute<T>(type, memberInfo) is T attr)
            {
                return new[] { attr };
            }

            return Array.Empty<T>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets attributes of specified type, associated with specified type.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="type">Attributes owner type.</param>
        /// <param name="inherit">If <c>true</c> - include inherited attributes.</param>
        /// <returns>Attributes of specified type.</returns>
        public virtual T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute
        {
            return GetAttributes<T>(type, typeof(TableAttribute));
        }

        /// <summary>
        /// Gets attributes of specified type, associated with specified type member.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="type">Member's owner type.</param>
        /// <param name="memberInfo">Attributes owner member.</param>
        /// <param name="inherit">If <c>true</c> - include inherited attributes.</param>
        /// <returns>Attributes of specified type.</returns>
        public virtual T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            return GetAttributes<T>(type, typeof(ColumnAttribute), memberInfo);
        }

        /// <summary>
        /// Gets the dynamic columns defined on given type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>All dynamic columns defined on given type</returns>
        public MemberInfo[] GetDynamicColumns(Type type)
        {
            return Array.Empty<MemberInfo>();
        }

        #endregion

        #region Properties

        protected static ConcurrentDictionary<(Type, MemberInfo), Attribute> Types { get; } = new ConcurrentDictionary<(Type, MemberInfo), Attribute>();
        protected static ConcurrentDictionary<Type, CreateTableExpression> Expressions { get; } = new ConcurrentDictionary<Type, CreateTableExpression>();

        #endregion
    }
}
