using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SquaresAssigner {
    private List<List<Square>> cells = new List<List<Square>>();
    private float maxSize, minSize;

    public SquaresAssigner(float width, float height, float maxSize, float minSize, Vector2 offset) {
        int numColumns = Mathf.FloorToInt(width/minSize);
        int numRows = Mathf.FloorToInt(height/minSize);
        this.maxSize = maxSize;
        this.minSize = minSize;
        for (int i = 0; i < numColumns; i++) {
            cells.Add(new List<Square>());
            for (int j = 0; j < numRows; j++) {
                cells[i].Add(new Square(minSize, offset + new Vector2(-width/2 + (i+0.5f)*minSize, -height/2 + (j+0.5f)*minSize)));
            }
        }
    }

    public Square getSquare() {
        Square choice = new Square(1, new Vector2(1,1));
        return choice;
    }
}

public class Square {
    public float side;
    public Vector2 center;
    public bool busy = false;

    public Square(float side, Vector2 center) {
        this.side = side;
        this.center = center;
    }
    
    public void setBusy() {
        this.busy = true;
    }

}

