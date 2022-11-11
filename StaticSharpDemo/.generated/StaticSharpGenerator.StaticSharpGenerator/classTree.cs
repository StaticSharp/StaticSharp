using System.Collections.Generic;
using System.Linq;
using System;
#pragma warning disable IDE1006 // Naming Styles
namespace StaticSharpDemo.Root {
	public class αRoot : ProtoNode {
		public αRoot(global::StaticSharpDemo.Root.Language language) : base(language) {
		}
		public override αRoot WithLanguage(global::StaticSharpDemo.Root.Language language) {
			return new αRoot(language);
		}
		public override ProtoNode Parent => null;
		public override αRoot Root => new αRoot(Language);
		public override string[] Path => new string[]{};
		public override string Name => "Root";
		public static implicit operator global::StaticSharpDemo.Root.Page(αRoot α) => α.Representative;
		public override global::StaticSharpDemo.Root.Page Representative => new global::StaticSharpDemo.Root.Ru(Language);
		public override IEnumerable<ProtoNode> Children {
			get {
				yield return Components;
				yield return Customization;
			}
		}
		public virtual αComponents Components => new(Language);
		public class αComponents : ProtoNode {
			public αComponents(global::StaticSharpDemo.Root.Language language) : base(language) {
			}
			public override αComponents WithLanguage(global::StaticSharpDemo.Root.Language language) {
				return new αComponents(language);
			}
			public override αRoot Parent => new αRoot(Language);
			public override αRoot Root => new αRoot(Language);
			public override string[] Path => new string[]{"Components"};
			public override string Name => "Components";
			public static implicit operator global::StaticSharpDemo.Root.Components.ComponentPage(αComponents α) => α.Representative;
			public override global::StaticSharpDemo.Root.Components.ComponentPage Representative => new global::StaticSharpDemo.Root.Components.Ru(Language);
			public override IEnumerable<ProtoNode> Children {
				get {
					yield return MaterialDesignIconsComponent;
					yield return ParagraphComponent;
					yield return ScrollLayoutComponent;
					yield return VideoPlayer;
				}
			}
			public virtual αMaterialDesignIconsComponent MaterialDesignIconsComponent => new(Language);
			public class αMaterialDesignIconsComponent : ProtoNode {
				public αMaterialDesignIconsComponent(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
				public override αMaterialDesignIconsComponent WithLanguage(global::StaticSharpDemo.Root.Language language) {
					return new αMaterialDesignIconsComponent(language);
				}
				public override αComponents Parent => new αComponents(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Components","MaterialDesignIconsComponent"};
				public override string Name => "MaterialDesignIconsComponent";
				public static implicit operator global::StaticSharpDemo.Root.Components.ComponentPage(αMaterialDesignIconsComponent α) => α.Representative;
				public override global::StaticSharpDemo.Root.Components.ComponentPage Representative => new global::StaticSharpDemo.Root.Components.MaterialDesignIconsComponent.En(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
			public virtual αParagraphComponent ParagraphComponent => new(Language);
			public class αParagraphComponent : ProtoNode {
				public αParagraphComponent(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
				public override αParagraphComponent WithLanguage(global::StaticSharpDemo.Root.Language language) {
					return new αParagraphComponent(language);
				}
				public override αComponents Parent => new αComponents(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Components","ParagraphComponent"};
				public override string Name => "ParagraphComponent";
				public static implicit operator global::StaticSharpDemo.Root.Components.ComponentPage(αParagraphComponent α) => α.Representative;
				public override global::StaticSharpDemo.Root.Components.ComponentPage Representative => new global::StaticSharpDemo.Root.Components.ParagraphComponent.En(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
			public virtual αScrollLayoutComponent ScrollLayoutComponent => new(Language);
			public class αScrollLayoutComponent : ProtoNode {
				public αScrollLayoutComponent(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
				public override αScrollLayoutComponent WithLanguage(global::StaticSharpDemo.Root.Language language) {
					return new αScrollLayoutComponent(language);
				}
				public override αComponents Parent => new αComponents(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Components","ScrollLayoutComponent"};
				public override string Name => "ScrollLayoutComponent";
				public static implicit operator global::StaticSharpDemo.Root.Components.ComponentPage(αScrollLayoutComponent α) => α.Representative;
				public override global::StaticSharpDemo.Root.Components.ComponentPage Representative => new global::StaticSharpDemo.Root.Components.ScrollLayoutComponent.Ru(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
			public virtual αVideoPlayer VideoPlayer => new(Language);
			public class αVideoPlayer : ProtoNode {
				public αVideoPlayer(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
				public override αVideoPlayer WithLanguage(global::StaticSharpDemo.Root.Language language) {
					return new αVideoPlayer(language);
				}
				public override αComponents Parent => new αComponents(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Components","VideoPlayer"};
				public override string Name => "VideoPlayer";
				public static implicit operator global::StaticSharpDemo.Root.Components.ComponentPage(αVideoPlayer α) => α.Representative;
				public override global::StaticSharpDemo.Root.Components.ComponentPage Representative => new global::StaticSharpDemo.Root.Components.VideoPlayer.Ru(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
		}
		public virtual αCustomization Customization => new(Language);
		public class αCustomization : ProtoNode {
			public αCustomization(global::StaticSharpDemo.Root.Language language) : base(language) {
			}
			public override αCustomization WithLanguage(global::StaticSharpDemo.Root.Language language) {
				return new αCustomization(language);
			}
			public override αRoot Parent => new αRoot(Language);
			public override αRoot Root => new αRoot(Language);
			public override string[] Path => new string[]{"Customization"};
			public override string Name => "Customization";
			public override StaticSharp.Page Representative => null;
			public override IEnumerable<ProtoNode> Children {
				get {
					yield return HowToCreateNewComponent;
				}
			}
			public virtual αHowToCreateNewComponent HowToCreateNewComponent => new(Language);
			public class αHowToCreateNewComponent : ProtoNode {
				public αHowToCreateNewComponent(global::StaticSharpDemo.Root.Language language) : base(language) {
				}
				public override αHowToCreateNewComponent WithLanguage(global::StaticSharpDemo.Root.Language language) {
					return new αHowToCreateNewComponent(language);
				}
				public override αCustomization Parent => new αCustomization(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Customization","HowToCreateNewComponent"};
				public override string Name => "HowToCreateNewComponent";
				public static implicit operator global::StaticSharpDemo.Root.Page(αHowToCreateNewComponent α) => α.Representative;
				public override global::StaticSharpDemo.Root.Page Representative => new global::StaticSharpDemo.Root.Customization.HowToCreateNewComponent.Ru(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
		}
	}
}
