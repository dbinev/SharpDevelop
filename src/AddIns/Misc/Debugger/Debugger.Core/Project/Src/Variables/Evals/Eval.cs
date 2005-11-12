﻿// <file>
//     <copyright see="prj:///doc/copyright.txt">2002-2005 AlphaSierraPapa</copyright>
//     <license see="prj:///doc/license.txt">GNU General Public License</license>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections;
using System.Runtime.InteropServices;

using Debugger;
using Debugger.Interop.CorDebug;
using Debugger.Interop.MetaData;

namespace Debugger
{
	/// <summary>
	/// This class holds information about function evaluation.
	/// </summary>
	public class Eval 
	{
		NDebugger debugger;
		
		ICorDebugEval     corEval;
		ICorDebugFunction corFunction;
		ICorDebugValue[]  args;
		
		bool              completed = false;
		Variable          result;
		
		public event EventHandler<EvalEventArgs> EvalComplete;
		
		/// <summary>
		/// True if the evaluation has been completed.
		/// </summary>
		public bool Completed {
			get {
				return completed;
			}
		}
		
		/// <summary>
		/// The result of the evaluation if the evaluation is complete and has returned a value. Null otherwise.
		/// </summary>
		public Variable Result {
			get {
				if (completed) {
					return result;
				} else {
					return null;
				}
			}
		}
		
		internal ICorDebugEval CorEval {
			get {
				return corEval;
			}
		}
		
		internal Eval(NDebugger debugger, ICorDebugFunction corFunction, ICorDebugValue[] args)
		{
			this.debugger = debugger;
			this.corFunction = corFunction;
			this.args = args;
		}
		
		/// <returns>True is setup was successful</returns>
		internal bool SetupEvaluation()
		{
			debugger.AssertPaused();
			
			// TODO: What if this thread is not suitable?
			debugger.CurrentThread.CorThread.CreateEval(out corEval);
			
			corEval.CallFunction(corFunction, (uint)args.Length, args);
			
			return true;
		}
		
		protected internal virtual void OnEvalComplete(EvalEventArgs e) 
		{
			completed = true;
			
			ICorDebugValue corValue;
			corEval.GetResult(out corValue);
			result = VariableFactory.CreateVariable(debugger, corValue, "eval result");
				
			if (EvalComplete != null) {
				EvalComplete(this, e);
			}
		}
	}
}
