using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.classes.Component;

namespace pixelholdersPlatformer.classes.gameObjects;

public class TextElement : GameObject
{
    private String _text;
    public bool IsClickable = false;

    public TextElement(float coordX, float coordY, float width, float height, String text, bool isClickable = false) : base(coordX, coordY, width, height)
    {
        _text = text;
        IsClickable = isClickable; 
    }

    public TextElement(TextElementSchema schema) : base(schema.CoordX, schema.CoordY, schema.Width, schema.Height)
    {
        _text = schema.Text;
        IsClickable = schema.IsClickable;
    }

    public String GetText()
    {
        return _text;
    }

    public void SetText(String text)
    {
        _text = text;
    }
}