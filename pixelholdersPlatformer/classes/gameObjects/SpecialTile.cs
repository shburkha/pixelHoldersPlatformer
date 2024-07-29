using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pixelholdersPlatformer.classes.managers;
using static SDL2.SDL;
using SDL2;

namespace pixelholdersPlatformer.classes.gameObjects
{
    public class SpecialTile : GameObject
    {
        SpecialTileType type;
        public SpecialTile(float coordX, float coordY, float width, float height, SpecialTileType type) : base(coordX, coordY, width, height)
        {
            this.type = type;
        }

        public void SpecialCollision()
        {
            switch (type)
            {
                case SpecialTileType.Goal:
                    TileMapManager.Instance.AdvanceLevel();
                    break;
                case SpecialTileType.Kill:
                    UIManager.Instance.ChangeScene(Scene.GameOver);
                    break;
            }
        }
    }
}

public enum SpecialTileType
{
    Goal, Kill
}