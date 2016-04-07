﻿using System;

// Place this file in any folder that is or is a descendant of a folder named "Scripts"
namespace CatchCo  
{
	// Restrict to methods only
	[AttributeUsage(AttributeTargets.Method)]
	public class ExposeMethodInEditorAttribute : Attribute
	{
	}
}