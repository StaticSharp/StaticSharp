using System.Collections.Generic;
using System.Linq;
using System;
#pragma warning disable IDE1006 // Naming Styles
namespace CsmlContent {
	partial record Generated {
		public virtual αIndex Index => new();
		public record αIndex : Node {
			public override CsmlEngine.INode Parent => new Generated();
			public virtual αSoftware Software => new();
			public record αSoftware : Node {
				public override CsmlEngine.INode Parent => new αSoftware();
				public virtual αAntilatencyService AntilatencyService => new();
				public record αAntilatencyService : Node {
					public override CsmlEngine.INode Parent => new αAntilatencyService();
					public virtual αUserInterface UserInterface => new();
					public record αUserInterface : Node {
						public override CsmlEngine.INode Parent => new αUserInterface();
						public override IEnumerable<CsmlEngine.INode> Children {
							get {
							}
						}
					}
					public override IEnumerable<CsmlEngine.INode> Children {
						get {
							yield return UserInterface;
						}
					}
				}
				public override IEnumerable<CsmlEngine.INode> Children {
					get {
						yield return AntilatencyService;
					}
				}
			}
			public virtual αHardware Hardware => new();
			public record αHardware : Node {
				public override CsmlEngine.INode Parent => new αHardware();
				public virtual αSockets Sockets => new();
				public record αSockets : global::CsmlContent.Index.Hardware.Sockets._ {
					public override CsmlEngine.INode Parent => new αSockets();
					public virtual αBracer Bracer => new();
					public record αBracer : Node {
						public override CsmlEngine.INode Parent => new αBracer();
						public override IEnumerable<CsmlEngine.INode> Children {
							get {
							}
						}
					}
					public override IEnumerable<CsmlEngine.INode> Children {
						get {
							yield return Bracer;
						}
					}
				}
				public virtual αAlt Alt => new();
				public record αAlt : global::CsmlContent.Index.Hardware.Material {
					static IEnumerable<global::CsmlContent.Index.Hardware.Material> Representatives {
						get {
							yield return new global::CsmlContent.Index.Hardware.Alt.En();
							yield return new global::CsmlContent.Index.Hardware.Alt.Ru();
						}
					}
					static global::CsmlContent.Index.Hardware.Material Representative => Representatives.Select(x => ((x as IAccordance)?.Value ?? 0, Object:x)).Max().Object;
					public override string Title => Representative.Title;
					public override global::System.FormattableString Description => Representative.Description;
					public override void Do(float value) => Representative.Do(value);
					public override global::Image Image => Representative.Image;
					public override global::Material.TChildren Children => Representative.Children;
					public override CsmlEngine.INode Parent => new αAlt();
					public virtual αIndicator Indicator => new();
					public record αIndicator : global::CsmlContent.Index.Hardware.Alt.Indicator._ {
						public override CsmlEngine.INode Parent => new αIndicator();
						public override IEnumerable<CsmlEngine.INode> Children {
							get {
							}
						}
					}
					public override IEnumerable<CsmlEngine.INode> Children {
						get {
							yield return Indicator;
						}
					}
				}
				public override IEnumerable<CsmlEngine.INode> Children {
					get {
						yield return Sockets;
						yield return Alt;
					}
				}
			}
			public override IEnumerable<CsmlEngine.INode> Children {
				get {
					yield return Software;
					yield return Hardware;
				}
			}
		}
		public override IEnumerable<CsmlEngine.INode> Children {
			get {
				yield return Index;
			}
		}
	}
}
