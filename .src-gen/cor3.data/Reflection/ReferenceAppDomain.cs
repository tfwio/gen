#region User/License
// Copyright (c) 2005-2013 tfwroble
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
/*
 * User: oIo
 * Date: 11/15/2010 ? 2:33 AM
 */
using System;
using System.Collections.Generic;
using System.Runtime.Hosting;
using System.Security.Policy;
using System.Xml.Serialization;

namespace System.Cor3.Reflection
{
	public class ReferenceAppDomain
	{
		public const string DefaultName = "separate";
		static public DICT<string,AppDomain> domains = new DICT<string, AppDomain>();
		
		static public bool HasDomain(string name) { return domains.ContainsKey(name); }
		
		static public AppDomain SeparateDomain(string name, AppDomainSetup setup)
		{
//			setup.ApplicationBase = "(some directory)";
			// Set up the Evidence
//			Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
//			Evidence evidence = new Evidence(baseEvidence);
//			evidence.AddAssembly("Cor3.Core");
//			evidence.AddHost("(some host)");

			// Create the AppDomain      
//			AppDomain newDomain = AppDomain.CreateDomain(name, null, setup);

			if (!HasDomain(name))
				domains.Add(name,CreateAppDomain(name,setup));
			return domains[name];
		}
		static public AppDomainSetup AppDomainSetupAlt
		{
			get
			{
				// Construct and initialize settings for a second AppDomain.
				AppDomainSetup ads = new AppDomainSetup();
				ads.ApplicationBase = System.Environment.CurrentDirectory;
				ads.DisallowBindingRedirects = false;
				ads.DisallowCodeDownload = true; // was true
				ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
				return ads;
			}
		}
		#region nothing
		public void that(AppDomain ad2)
		{
			// Create an instance of MarshalbyRefType in the second AppDomain.
			// A proxy to the object is returned.
			string callingDomainName = System.Threading.Thread.GetDomain().FriendlyName;
			string exeAssembly = System.Reflection.Assembly.GetEntryAssembly().FullName;
			MarshalByRefType mbrt = 
				(MarshalByRefType)
				ad2.CreateInstanceAndUnwrap(exeAssembly,typeof(MarshalByRefType).FullName);
			// Call a method on the object via the proxy, passing the
			// default AppDomain's friendly name in as a parameter.
			mbrt.SomeMethod(callingDomainName);
		}
		public AppDomain this[string name]
		{
			get{
				return null;
			}
		}
		#endregion
		static int DomainIncrament = 0;
		static public AppDomain CreateAppDomain() { return CreateAppDomain(string.Format("AD #{0}",DomainIncrament++),AppDomainSetupAlt); }
		static public AppDomain CreateAppDomain(string name) { return CreateAppDomain(name,AppDomainSetupAlt); }
		static public AppDomain CreateAppDomain(string domainname, AppDomainSetup appdomainsetup)
		{
			DomainIncrament++;
			return AppDomain.CreateDomain(domainname,null,appdomainsetup);
		}
	}


	/* This code produces output similar to the following:
AppDomainX.exe
AppDomainX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
AppName=, AppBase=C:\AppDomain\bin, ConfigFile=C:\AppDomain\bin\AppDomainX.exe.config
Calling from 'AppDomainX.exe' to 'AD #2'.
Failed call; this is expected.
	 */
}
