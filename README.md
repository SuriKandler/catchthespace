# Catch The Space

## Overview

This repo is for the software implementation of a classic game which I have titled Catch the Space. 

## Game Details

The players are presented with a chart of 16 dots. Each player is assigned a colour and they take turns drawing lines from one dot to another in either a vertical or horizontal direction.  The dots are the vertices of 9 squares. The player that draws the last line and creates a square – no matter if the other 3 lines were their colour or not – acquires the centre space of that square.  The lines around this square change to black and the centre colour takes the background colour of the player.
In one go, if the line that the player drew completes one square and the line changed the adjacent square to 3 lines, then he may make the 4th line and get this square too in this turn.  The players turn ends when no more connecting squares can be acquired as a result of the first line created that turn.
The game ends when all the space – squares are taken; the player with the most squares win.


