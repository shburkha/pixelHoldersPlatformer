﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using pixelholdersPlatformer.classes.managers;

namespace pixelholdersPlatformer.classes.gameObjects;

public class TextElement : GameObject
{
    private String _text;

    public TextElement(float coordX, float coordY, float width, float height, String text) : base(coordX, coordY, width, height)
    {
        _text = text;
    }

    public TextElement(TextElementSchema schema) : base(schema.CoordX, schema.CoordY, schema.Width, schema.Height)
    {
        _text = schema.Text;
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