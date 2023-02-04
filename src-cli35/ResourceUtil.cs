/*
 * Created by SharpDevelop.
 * User: oio
 * Date: 5/25/2011
 * Time: 1:20 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace System
{
	static class ResourceUtil
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
					ResourceManager resourceManager = new ResourceManager("System.Cor3.Parsers.Resources", typeof(ResourceUtil).Assembly);
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
