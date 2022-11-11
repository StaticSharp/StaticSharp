namespace StaticSharpDemo.Root {
	public partial class Page : StaticSharp.Tree.IRepresentative {
		protected override ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
		public global::StaticSharpDemo.Root.Language Language{get; init;}
		public Page(global::StaticSharpDemo.Root.Language language) {
			Language = language;
		}
	}
	public partial class Ru : StaticSharp.Tree.IRepresentative {
		protected override ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
		public Ru(global::StaticSharpDemo.Root.Language language) : base(language) {
		}
	}
	namespace Components {
		public partial class ComponentPage : StaticSharp.Tree.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			αRoot.αComponents Node => new(Language);
			StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
			public ComponentPage(global::StaticSharpDemo.Root.Language language) : base(language) {
			}
		}
		public partial class Ru : StaticSharp.Tree.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			αRoot.αComponents Node => new(Language);
			StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
			public Ru(global::StaticSharpDemo.Root.Language language) : base(language) {
			}
		}
		namespace MaterialDesignIconsComponent {
			public partial class En : StaticSharp.Tree.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αComponents.αMaterialDesignIconsComponent Node => new(Language);
				StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
				public En(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
			}
		}
		namespace ParagraphComponent {
			public partial class En : StaticSharp.Tree.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αComponents.αParagraphComponent Node => new(Language);
				StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
				public En(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
			}
		}
		namespace ScrollLayoutComponent {
			public partial class Ru : StaticSharp.Tree.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αComponents.αScrollLayoutComponent Node => new(Language);
				StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
				public Ru(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
			}
		}
		namespace VideoPlayer {
			public partial class Ru : StaticSharp.Tree.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αComponents.αVideoPlayer Node => new(Language);
				StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
				public Ru(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
			}
		}
	}
	namespace Customization {
		namespace HowToCreateNewComponent {
			public partial class Ru : StaticSharp.Tree.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αCustomization.αHowToCreateNewComponent Node => new(Language);
				StaticSharp.Tree.Node StaticSharp.Tree.IRepresentative.Node => Node;
				public Ru(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
			}
		}
	}
}
