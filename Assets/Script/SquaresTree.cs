using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SquaresTree {
    private List<SquareCell> topCells = new List<SquareCell>();

    public SquaresTree(float width, float height, float maxSize, float minSize, Vector3 offset) {
        int numColumns = Mathf.FloorToInt(width/maxSize);
        int numRows = Mathf.FloorToInt(height/maxSize);
        for (int i = 0; i < numColumns; i++) {
            for (int j = 0; j < numRows; j++) {
                topCells.Add(new SquareCell(maxSize, minSize, offset + new Vector3(-width/2.0f + (i+0.5f)*maxSize, -height/2.0f + (j+0.5f)*maxSize)));
            }
        }
    }

    public SquareCell getSquare() {
        List<SquareCell> squares = new List<SquareCell>();
        foreach (SquareCell topCell in topCells) {
            if(!topCell.busy){
                squares.AddRange(topCell.getSquares());
            }
        }
        if (squares.Count == 0) {
            throw new Exception("No more available square");
        }
        squares = squares.OrderBy(i => i.side)
            .ThenBy(i => i.center.x)
            .ThenBy(i => i.center.y)
            .ToList();
        SquareCell choice = squares[UnityEngine.Random.Range(0, squares.Count)];
        choice.setBusy();
        //Debug.Log("Side: "+ choice.side + ", center: " + choice.center + ", num choices: " + squares.Count);
        return choice;
    }
}

public class SquareCell {
    public float side;
    public Vector3 center;
    private List<SquareCell> children = new List<SquareCell>();
    public bool busy = false;

    public SquareCell(float side, float minSize, Vector3 center) {
        this.side = side;
        this.center = center;
        float childrenSide = side/2;
        if (childrenSide >= minSize) {
            for (int i = -1; i < 2; i +=2) {
                for (int j = -1; j < 2; j +=2) {
                    Vector3 childCenter = center + new Vector3(i*0.5f*childrenSide, j*0.5f*childrenSide);
					children.Add(new SquareCell(childrenSide, minSize, childCenter));
                }
            }
        }
    }
    
    public void setBusy() {
        this.busy = true;
        foreach (SquareCell child in children) {
            child.setBusy();
        }
    }

    public bool isBusy() {
        if (busy) {
            return true;
        }
        foreach (SquareCell child in children) {
            if (child.isBusy()) {
                return true;
            }
        }
        return false;
    }

    public List<SquareCell> getSquares() {
        List<SquareCell> squares = new List<SquareCell>();
        if (busy) {
            return squares;
        }
        bool childBusy = false;
        foreach (SquareCell child in children) {
            if (!child.busy) {
                squares.AddRange(child.getSquares());
            }
            if (child.isBusy()) {
                childBusy = true;
            }
        }
        if (!childBusy) {
            squares.Add(this);
        }
        return squares;
    }

}

