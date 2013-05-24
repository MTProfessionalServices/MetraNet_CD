﻿/**************************************************************************
* Copyright 1997-2011 by MetraTech.SecurityFramework
* All rights reserved.
*
* THIS SOFTWARE IS PROVIDED "AS IS", AND MetraTech.SecurityFramework MAKES NO
* REPRESENTATIONS OR WARRANTIES, EXPRESS OR IMPLIED. By way of
* example, but not limitation, MetraTech.SecurityFramework MAKES NO REPRESENTATIONS OR
* WARRANTIES OF MERCHANTABILITY OR FITNESS FOR ANY PARTICULAR PURPOSE
* OR THAT THE USE OF THE LICENCED SOFTWARE OR DOCUMENTATION WILL NOT
* INFRINGE ANY THIRD PARTY PATENTS, COPYRIGHTS, TRADEMARKS OR OTHER
* RIGHTS.
*
* Title to copyright in this software and any associated
* documentation shall at all times remain with MetraTech.SecurityFramework, and USER
* agrees to preserve the same.
*
* Authors: Viktor Grytsay
*
* <vgrytsay@MetraTech.SecurityFramework.com>
*
* 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetraTech.SecurityFramework.Serialization.Attributes
{	
	/// <summary>
	/// Implementing declarative restriction for serialize properties if source data is external.
	/// </summary>
	public class SerializeNestedAttribute : SerializeAttribute
	{
		/// <summary>
		/// Path to external source
		/// </summary>
		public string PathToSource
		{
			get;
			set;
		}

		/// <summary>
		/// Serializer type for current property.
		/// </summary>
		public string SerializerTypeName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets path type to source file
		/// </summary>
		public PathType PathType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets path name to directory with source configuration.
		/// </summary>
		public string NestedPath
		{
			get;
			set;
		}
	}
}
