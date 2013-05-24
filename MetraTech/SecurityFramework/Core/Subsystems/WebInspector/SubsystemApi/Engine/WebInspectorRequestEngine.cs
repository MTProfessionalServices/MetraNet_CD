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

namespace MetraTech.SecurityFramework
{
	public class WebInspectorRequestEngine : WebInspectorEngineBase
	{
		public WebInspectorRequestEngine()
			: base(WebInspectorEngineCategory.WebInspectorRequest)
		{ }

		protected override ApiOutput ExecuteInternal(ApiInput input)
		{
			ApiOutput result = base.ExecuteInternal(input);
			WebInspectorRequestApiInput apiInput = input.Value as WebInspectorRequestApiInput;

			if (apiInput == null)
			{
				throw new WebInspectorException(string.Format("Input parameter type for WebInspector {0} is not valid!", Id));
			}

			Inspector.ProcessRequest(apiInput.App, apiInput.Context, apiInput.Request, apiInput.Response);

			return result;
		}
	}
}
