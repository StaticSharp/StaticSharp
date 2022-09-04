namespace StaticSharpDemo.Root {
	//DEBUG
	//COMMENT
	public partial class Material : StaticSharpEngine.IRepresentative {
		protected virtual ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
		public global::StaticSharpDemo.Language Language{get; init;}
		public Material(global::StaticSharpDemo.Language language) {
			Language = language;
		}
	}
	//DEBUG
	//COMMENT
	public partial class Ru : StaticSharpEngine.IRepresentative {
		protected override ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
		public Ru(global::StaticSharpDemo.Language language) : base(language) {
		}
	}
	namespace Components {
		//DEBUG
		//COMMENT
		public partial class Ru : StaticSharpEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			αRoot.αComponents Node => new(Language);
			StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
			public Ru(global::StaticSharpDemo.Language language) : base(language) {
			}
		}
	}
	namespace Customization {
		namespace HowToCreateNewComponent {
			//DEBUG
			//COMMENT
			public partial class Ru : StaticSharpEngine.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αCustomization.αHowToCreateNewComponent Node => new(Language);
				StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
				public Ru(global::StaticSharpDemo.Language language) : base(language) {
				}
			}
		}
	}
}
