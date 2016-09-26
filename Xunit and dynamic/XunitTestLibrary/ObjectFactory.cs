using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace XunitTestLibrary
{
	public class ObjectFactory
	{
		public static string[] PropertyNames = new[] { "Name", "Count" };
		public static Type[] PropertyTypes = new[] { typeof(string), typeof(int) };

		private readonly ModuleBuilder _moduleBuilder = null;

		public ObjectFactory()
		{
			var assemblyName = new AssemblyName() { Name = "LiteObjectTypes" };
			_moduleBuilder = Thread.GetDomain()
				.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run)
				.DefineDynamicModule(assemblyName.Name);
		}

		public object GetObject()
		{
			const string className = "LiteObject";
			var typeBuilder = _moduleBuilder.DefineType(className,
				TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

			for (var pos = 0; pos < PropertyNames.Length; pos++)
			{
				// create private field and init it by default value from DefaultValue attribute
				var fieldName = $"_{PropertyNames[pos].ToLower()[0]}{PropertyNames[pos].Substring(1)}";
				var fieldBuilder = typeBuilder.DefineField(fieldName, PropertyTypes[pos],
											FieldAttributes.Private);

				var propertyBuilder = typeBuilder.DefineProperty(PropertyNames[pos],
											PropertyAttributes.HasDefault,
											PropertyTypes[pos], null);

				// the property set and property get methods require a special set of attributes.
				const MethodAttributes accessorAttributes =
					MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

				// define the "get" accessor method for CustomerName.
				var getterMethod = typeBuilder.DefineMethod($"get_{PropertyNames[pos]}",
										accessorAttributes,
										PropertyTypes[pos],
										Type.EmptyTypes);

				var getterMethodIL = getterMethod.GetILGenerator();

				getterMethodIL.Emit(OpCodes.Ldarg_0);
				getterMethodIL.Emit(OpCodes.Ldfld, fieldBuilder);
				getterMethodIL.Emit(OpCodes.Ret);

				// Define the "set" accessor method for CustomerName.
				var setterMethod = typeBuilder.DefineMethod($"set_{PropertyNames[pos]}",
										accessorAttributes,
										null,
										new Type[] { PropertyTypes[pos] });

				var setterMethodIL = setterMethod.GetILGenerator();

				setterMethodIL.Emit(OpCodes.Ldarg_0);
				setterMethodIL.Emit(OpCodes.Ldarg_1);
				setterMethodIL.Emit(OpCodes.Stfld, fieldBuilder);
				setterMethodIL.Emit(OpCodes.Ret);

				// Last, we must map the two methods created above to our PropertyBuilder to 
				// their corresponding behaviors, "get" and "set" respectively. 
				propertyBuilder.SetGetMethod(getterMethod);
				propertyBuilder.SetSetMethod(setterMethod);
			}

			var liteObjectType = typeBuilder.CreateType();
			if (liteObjectType == null)
				return null;

			return Activator.CreateInstance(liteObjectType);
		}

		public object GetObjectViaReflection(string name, int count)
		{
			var liteObject = GetObject();
			if (liteObject == null)
				return null;

			var values = new object[] { name, count };

			var propertyInfos = TypeDescriptor.GetProperties(liteObject.GetType());
			for (var pos = 0; pos < PropertyNames.Length; pos++)
			{
				var property = propertyInfos[ObjectFactory.PropertyNames[pos]];
				property?.SetValue(liteObject, values[pos]);
			}

			return liteObject;
		}

		public object GetObjectViaDynamic(string name, int count)
		{
			dynamic liteObject = GetObject();
			if (liteObject == null)
				return null;

			liteObject.Name = name;
			liteObject.Count = count;

			return liteObject;
		}
	}
}
