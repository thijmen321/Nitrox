using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ProtoBufNet;
using ProtoBufNet.Meta;

namespace NitroxModel.Helper
{
    public class TypeModelBuilder
    {
        private const BindingFlags ALL_INSTANCE_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        public RuntimeTypeModel Model { get; }

        private readonly TypeModelMembers members;
        private readonly Dictionary<Type, int> tags;

        public TypeModelBuilder(TypeModelMembers members)
        {
            Model = TypeModel.Create();
            Model.AutoAddMissingTypes = false;
            Model.SetDefaultFactory(typeof(FormatterServices).GetMethod(nameof(FormatterServices.GetUninitializedObject)));

            this.members = members;
            tags = new Dictionary<Type, int>();
        }

        private int NextTag(MetaType meta)
        {
            Type key = meta.Type;
            if (tags.ContainsKey(key))
            {
                return ++tags[key];
            }
            else
            {
                tags.Add(key, 1);
                return 1;
            }
        }

        public RuntimeTypeModel Compile()
        {
            FixPolymorphism();

            return Model;
        }

        public TypeModelBuilder AddSurrogate<TSurrogate, TFor>() => AddSurrogate(typeof(TSurrogate), typeof(TFor));
        public TypeModelBuilder AddSurrogate(Type surrogate, Type forType)
        {
            AddTypeInternal(surrogate);
            Model.Add(forType, false).SetSurrogate(surrogate);

            return this;
        }

        public TypeModelBuilder AddTypesWithAttribute<A>(Assembly assembly) where A : Attribute
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsPublic &&
                    !type.ContainsGenericParameters &&
                    Attribute.IsDefined(type, typeof(A), false))
                {
                    Console.WriteLine($"Adding type {type.Name}");

                    AddTypeInternal(type);
                }
            }

            return this;
        }

        public TypeModelBuilder AddType<T>() => AddType(typeof(T));
        public TypeModelBuilder AddType(Type type)
        {
            AddTypeInternal(type);

            return this;
        }

        private void FixPolymorphism()
        {
            IEnumerable<MetaType> types = Model.GetTypes().Cast<MetaType>();
            Dictionary<Type, MetaType> typeLookup = types.ToDictionary(m => m.Type);

            foreach (MetaType meta in types)
            {
                MetaType baseMeta = null;
                if (typeLookup.TryGetValue(meta.Type.BaseType, out baseMeta))
                {
                    baseMeta.AddSubType(NextTag(baseMeta), meta.Type);
                }
            }
        }

        private MetaType AddTypeInternal(Type type)
        {
            if (type.ContainsGenericParameters || Model.CanSerializeBasicType(type)) // If the type is basic
            {
                return null;
            }

            if (Model.CanSerializeContractType(type)) // If the type is already known
            {
                return Model.Add(type, false);
            }

            MetaType meta = Model.Add(type, false); // Fails when type is a basic type
            DecorateType(meta, members);

            return meta;
        }

        private void DecorateType(MetaType type, TypeModelMembers members)
        {
            if (members != TypeModelMembers.None)
            {
                MethodInfo bS = null,
                    aS = null,
                    bD = null,
                    aD = null;

                foreach (MemberInfo member in type.Type.GetMembers(ALL_INSTANCE_BINDING_FLAGS)
                    .Where(p => !Attribute.IsDefined(p, typeof(ProtoIgnoreAttribute), false) &&
                                !Attribute.IsDefined(p, typeof(NonSerializedAttribute), false))
                    .OrderBy(p => p.Name))
                {
                    FieldInfo field;
                    PropertyInfo prop;
                    MethodInfo method;
                    if ((field = member as FieldInfo) != null)
                    {
                        if ((field.IsPublic && (members & TypeModelMembers.PublicFields) == TypeModelMembers.PublicFields) ||
                            (!field.IsPublic && (members & TypeModelMembers.PrivateFields) == TypeModelMembers.PrivateFields))
                        {
                            ValueMember vm = type.AddField(NextTag(type), field.Name);

                            if (field.FieldType.IsInterface)// || property.PropertyType == typeof(object))
                            {
                                vm.DynamicType = true;
                            }
                        }
                    }
                    else if ((prop = member as PropertyInfo) != null)
                    {
                        bool isPublic = prop.GetGetMethod(false) != null;
                        if ((isPublic && (members & TypeModelMembers.PublicProperties) == TypeModelMembers.PublicProperties) ||
                            (!isPublic && (members & TypeModelMembers.PrivateProperties) == TypeModelMembers.PrivateProperties))
                        {
                            ValueMember vm = type.AddField(NextTag(type), prop.Name);

                            if (prop.PropertyType.IsInterface)// || property.PropertyType == typeof(object))
                            {
                                vm.DynamicType = true;
                            }
                        }
                    }
                    else if ((method = member as MethodInfo) != null)
                    {
                        if (Attribute.IsDefined(method, typeof(ProtoBeforeSerializationAttribute), false) ||
                            Attribute.IsDefined(method, typeof(OnSerializingAttribute), false))
                        {
                            bS = method;
                        }
                        else if (Attribute.IsDefined(method, typeof(ProtoAfterSerializationAttribute), false) ||
                                 Attribute.IsDefined(method, typeof(OnSerializedAttribute), false))
                        {
                            aS = method;
                        }
                        else if (Attribute.IsDefined(method, typeof(ProtoBeforeDeserializationAttribute), false) ||
                                 Attribute.IsDefined(method, typeof(OnDeserializingAttribute), false))
                        {
                            bD = method;
                        }
                        else if (Attribute.IsDefined(method, typeof(ProtoAfterDeserializationAttribute), false) ||
                                 Attribute.IsDefined(method, typeof(OnDeserializedAttribute), false))
                        {
                            aD = method;
                        }
                    }
                }

                type.SetCallbacks(bS, aS, bD, aD);
            }
        }
    }
}
