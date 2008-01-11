// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Text;
using System.Collections.Generic;

namespace Debugger.Expressions
{
	public class IndexerExpression: Expression
	{
		Expression targetObject;
		Expression[] arguments;
		
		public Expression TargetObject {
			get { return targetObject; }
		}
		
		public Expression[] Arguments {
			get { return arguments; }
		}
		
		public IndexerExpression(Expression targetObject, Expression[] arguments)
		{
			this.targetObject = targetObject;
			this.arguments = arguments;
		}
		
		public override string Code {
			get {
				StringBuilder sb = new StringBuilder();
				sb.Append(targetObject.Code);
				sb.Append("[");
				bool isFirst = true;
				foreach(Expression argument in arguments) {
					if (isFirst) {
						isFirst = false;
					} else {
						sb.Append(", ");
					}
					sb.Append(argument.Code);
				}
				sb.Append("]");
				return sb.ToString();
			}
		}
		
		protected override Value EvaluateInternal(StackFrame context)
		{
			Value targetValue = targetObject.Evaluate(context);
			List<int> indicies = new List<int>();
			foreach(Expression argument in arguments) {
				if (argument is PrimitiveExpression) {
					PrimitiveExpression primitiveExpression = (PrimitiveExpression)argument;
					if (primitiveExpression.Value is int) {
						indicies.Add((int)primitiveExpression.Value);
					}
				}
				throw new ExpressionEvaluateException(this, "Integer index expected");
			}
			try {
				return targetValue.GetArrayElement(indicies.ToArray());
			} catch (CannotGetValueException e) {
				throw new ExpressionEvaluateException(this, e.Message);
			}
		}
	}
}
