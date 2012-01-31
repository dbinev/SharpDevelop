﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;

using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.CodeQuality.Engine.Dom
{
	public class EventNode : INode
	{
		public string Name {
			get {
				throw new NotImplementedException();
			}
		}
		
		public IList<INode> Children {
			get { return null; }
		}
		
		public IEnumerable<INode> Uses {
			get {
				throw new NotImplementedException();
			}
		}
		
		public IEnumerable<INode> UsedBy {
			get {
				throw new NotImplementedException();
			}
		}
		
		public Relationship GetRelationship(INode value)
		{
			Relationship r = new Relationship();
			if (value == this)
				r.AddRelationship(RelationshipType.Same);
			if (Uses.Contains(value))
				r.AddRelationship(RelationshipType.Uses);
			if (UsedBy.Contains(value))
				r.AddRelationship(RelationshipType.UsedBy);
			return r;
		}
	}
}