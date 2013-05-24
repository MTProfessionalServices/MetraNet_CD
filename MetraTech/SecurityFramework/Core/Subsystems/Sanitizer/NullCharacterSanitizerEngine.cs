﻿/**************************************************************************
* Copyright 1997-2010 by MetraTech
* All rights reserved.
*
* THIS SOFTWARE IS PROVIDED "AS IS", AND MetraTech MAKES NO
* REPRESENTATIONS OR WARRANTIES, EXPRESS OR IMPLIED. By way of
* example, but not limitation, MetraTech MAKES NO REPRESENTATIONS OR
* WARRANTIES OF MERCHANTABILITY OR FITNESS FOR ANY PARTICULAR PURPOSE
* OR THAT THE USE OF THE LICENCED SOFTWARE OR DOCUMENTATION WILL NOT
* INFRINGE ANY THIRD PARTY PATENTS, COPYRIGHTS, TRADEMARKS OR OTHER
* RIGHTS.
*
* Title to copyright in this software and any associated
* documentation shall at all times remain with MetraTech, and USER
* agrees to preserve the same.
*
* Authors: 
*
* Maksym Sukhovarov <msukhovarov@metratech.com>
*
* 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetraTech.SecurityFramework.Core.Sanitizer
{
    /// <summary>
    /// Remove null simbols (sanitize) in input data.
    /// </summary>
    internal class NullCharacterSanitizerEngine : SanitizerEngineBase
    {
        private const string NullChar = "\0";

        public NullCharacterSanitizerEngine() 
            : base(SanitizerEngineCategory.NullCharacterSanitizer)
        {
        }

        protected override ApiOutput SanitizeInternal(ApiInput input)
        {
            return new ApiOutput(input.ToString().Replace(NullChar, ""));
        }
    }
}
