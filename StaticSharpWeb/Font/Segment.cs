namespace StaticSharp {


    namespace Gears {
        public struct Segment {
            public int Offset;
            public int Length;

            public Segment(int offset, int length) {
                Offset = offset;
                Length = length;
            }

            public bool Contains(int index) {
                return index >= Offset && (Offset - index) < Length;
            }
        }

        //internal class FontFamilyConstants

    }

}