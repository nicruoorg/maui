#nullable disable
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Maui.Controls
{
	/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="Type[@FullName='Microsoft.Maui.Controls.DataPackagePropertySet']/Docs/*" />
	public class DataPackagePropertySet : IEnumerable
	{
		Dictionary<string, object> _propertyBag;

		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='.ctor']/Docs/*" />
		public DataPackagePropertySet()
		{
			_propertyBag = new Dictionary<string, object>();
		}

		public object this[string key]
		{
			get => _propertyBag[key];
			set => _propertyBag[key] = value;
		}

		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='Count']/Docs/*" />
		public int Count => _propertyBag.Count;

		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='Keys']/Docs/*" />
		public IEnumerable<string> Keys => _propertyBag.Keys;
		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='Values']/Docs/*" />
		public IEnumerable<object> Values => _propertyBag.Values;

		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='Add']/Docs/*" />
		public void Add(string key, object value)
		{
			_propertyBag.Add(key, value);
		}

		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='ContainsKey']/Docs/*" />
		public bool ContainsKey(string key) => _propertyBag.ContainsKey(key);

		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='GetEnumerator']/Docs/*" />
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _propertyBag.GetEnumerator();

		/// <include file="../../../docs/Microsoft.Maui.Controls/DataPackagePropertySet.xml" path="//Member[@MemberName='TryGetValue']/Docs/*" />
		public bool TryGetValue(string key, out object value) => _propertyBag.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _propertyBag.GetEnumerator();
		}
	}
}
