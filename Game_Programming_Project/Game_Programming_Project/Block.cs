using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project
{
    //Used with the collision detection of a block
    enum BlockCollision
    {
        //Does not affect player movement - IE air
        Passable = 0,

        //Completely solid block
        Impassable = 1,

        //Player can jump, or walk through it, but can also stand on it
        Platform = 2,
    }


    //Stores the information pertaining to a block
    struct Block
    {
        public Texture2D Texture;
        public BlockCollision Collision;

        public const int Width = 32;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public Block(Texture2D texture, BlockCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }

}
