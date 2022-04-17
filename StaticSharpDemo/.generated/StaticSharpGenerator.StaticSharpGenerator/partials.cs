namespace StaticSharpDemo.Root {
	public partial class Material : StaticSharpEngine.IRepresentative {
		protected virtual ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
		public global::StaticSharpDemo.Language Language{get; init;}
		public Material(global::StaticSharpDemo.Language language) {
			Language = language;
		}
	}
	public partial class Ru : StaticSharpEngine.IRepresentative {
		protected override ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
		public Ru(global::StaticSharpDemo.Language language) : base(language) {
		}
	}
	namespace Components {
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
