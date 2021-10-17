using System;
using System.Collections.Generic;
using System.Text;

namespace kennards.generic.classes
{ /// <summary>
  /// Intersection - a spot on the board. 
  ///     <list type="bullet">
  ///         <item>
  ///             <term>OccupiedBy <see cref="Player">Player</see></term>
  ///             <description>
  ///                 The player at with this intersection
  ///             </description>
  ///         </item>
  ///         <item>
  ///             <term>Row int</term>
  ///             <description>
  ///                The row of the intersection                
  ///             </description>
  ///         </item>
  ///         <item>
  ///             <term>Column int</term>
  ///             <description>
  ///                the column of the intersectin               
  ///             </description>
  ///         </item>
  ///     </list>
  /// </summary>
  /// 
    public class Intersection
    {
        public Player OccupiedBy { get; set; } = null;
        public int Row { get; set; }
        public int Column { get; set; }
        /// <summary>Default Construction</summary>
        public Intersection()
        {
            OccupiedBy = new Player();
            Row = -1;
            Column = -1;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="P" type="Player">The player that is at intersection</param>
        /// <param name="R" type="int">The row</param>
        /// <param name="C" type="int">The Column</param>
        public Intersection(Player P, int R, int C)
        {
            OccupiedBy = P;
            Row = R;
            Column = C;
        }
    }
}
