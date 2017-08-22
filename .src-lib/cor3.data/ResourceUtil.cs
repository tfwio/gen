/*
 * User: oIo
 * Date: 11/15/2010 ? 2:49 AM
 */
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace System
{
	class ResourceUtil
	{
		
		static public string GetString(string key)
		{
			return ResourceManager.GetString(key);
		}
		static public string GetString(string key, object value)
		{
			return string.Format(ResourceManager.GetString(key,Culture),value);
		}
		
		private static ResourceManager resourceMan;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(ResourceUtil.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("System.Cor3.Data.Messages", typeof(ResourceUtil).Assembly);
					ResourceUtil.resourceMan = resourceManager;
				}
				return ResourceUtil.resourceMan;
			}
		}
		
		private static CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return ResourceUtil.resourceCulture;
			}
			set
			{
				ResourceUtil.resourceCulture = value;
			}
		}
	}
}
